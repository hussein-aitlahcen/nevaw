using Ankama.Animations;
using Ankama.Animations.Events;
using Ankama.AssetManagement;
using Ankama.AssetManagement.AssetBundles;
using Ankama.AssetManagement.AssetReferences;
using Ankama.Cube.Animations;
using Ankama.Cube.Data;
using Ankama.Cube.Data.UI.Localization.TextFormatting.ValueProviders.FightValueProviders;
using Ankama.Cube.Extensions;
using Ankama.Cube.Fight;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps.Feedbacks;
using Ankama.Cube.Maps.VisualEffects;
using Ankama.Cube.UI.Components;
using Ankama.Cube.UI.Fight;
using Ankama.Cube.Utility;
using Ankama.Utilities;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Ankama.Cube.Maps.Objects
{
	public abstract class CharacterObject : MovableIsoObject, IMovableObject, ICharacterObject, IMovableIsoObject, IIsoObject, IObjectWithFocus, ITimelineContextProvider, ITooltipDataProvider, IObjectWithCounterEffects
	{
		[SerializeField]
		protected PlayableDirector m_playableDirector;

		[SerializeField]
		protected Transform m_attachableEffectsContainer;

		protected Direction m_direction = Direction.SouthEast;

		protected Component m_deathEffectInstance;

		protected Component m_spawnEffectInstance;

		protected readonly Dictionary<PropertyId, VisualEffect> m_propertyEffects = new Dictionary<PropertyId, VisualEffect>(PropertyIdComparer.instance);

		protected bool m_hasTimeline;

		protected DirectionAngle m_mapRotation;

		protected CharacterAnimationCallback m_animationCallback;

		protected bool m_isFocused;

		private Color m_colorModifier = Color.get_white();

		private BundleCategory m_activeCharacterDataBundleCategory;

		private BundleCategory m_animatedCharacterDataBundleCategory;

		private const float SlideCellTraversalTime = 2f;

		private FloatingCounterFeedback m_currentFloatingCounterFeedback;

		public DirectionAngle mapRotation => m_mapRotation;

		public virtual Direction direction
		{
			get
			{
				return m_direction;
			}
			set
			{
				m_direction = value;
			}
		}

		public abstract TooltipDataType tooltipDataType
		{
			get;
		}

		public abstract KeywordReference[] keywordReferences
		{
			get;
		}

		protected abstract IAnimator2D GetAnimator();

		protected abstract AnimatedCharacterData GetAnimatedCharacterData();

		protected abstract void SetAnimatedCharacterData([NotNull] AnimatedCharacterData data);

		protected abstract void ClearAnimatedCharacterData();

		protected unsafe void InitializeAnimator()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			IAnimator2D animator = GetAnimator();
			if (animator != null)
			{
				animator.add_AnimationLooped(new AnimationLoopedEventHandler((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
				m_animationCallback = new CharacterAnimationCallback(animator);
			}
		}

		protected unsafe void ReleaseAnimator()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			IAnimator2D animator = GetAnimator();
			if (animator != null)
			{
				animator.remove_AnimationLooped(new AnimationLoopedEventHandler((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
				ClearAnimatorDefinition();
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			InitializeAnimator();
			m_playableDirector.set_playableAsset(null);
			m_playableDirector.set_extrapolationMode(0);
			TimelineContextUtility.SetContextProvider(m_playableDirector, this);
			CameraHandler.AddMapRotationListener(OnMapRotationChanged);
		}

		protected virtual void OnDisable()
		{
			CameraHandler.RemoveMapRotationListener(OnMapRotationChanged);
			DestroyAttachedEffects();
			if (null != m_playableDirector)
			{
				TimelineContextUtility.ClearFightContext(m_playableDirector);
				m_playableDirector.Stop();
				m_playableDirector.set_playableAsset(null);
			}
			if (m_animationCallback != null)
			{
				m_animationCallback.Release();
				m_animationCallback = null;
			}
			ReleaseAnimator();
			AnimatedCharacterData animatedCharacterData = GetAnimatedCharacterData();
			if (null != animatedCharacterData)
			{
				animatedCharacterData.UnloadTimelineResources();
				ClearAnimatedCharacterData();
				AssetManager.UnloadAssetBundle(AssetBundlesUtility.GetAnimatedCharacterDataBundle(m_animatedCharacterDataBundleCategory));
				m_animatedCharacterDataBundleCategory = BundleCategory.None;
			}
			if (m_activeCharacterDataBundleCategory != 0)
			{
				AssetManager.UnloadAssetBundle(AssetBundlesUtility.GetAnimatedCharacterDataBundle(m_activeCharacterDataBundleCategory));
				m_activeCharacterDataBundleCategory = BundleCategory.None;
			}
		}

		public IEnumerator LoadAnimationDefinitions(int skinId, Gender gender = Gender.Male)
		{
			if (!RuntimeData.characterSkinDefinitions.TryGetValue(skinId, out CharacterSkinDefinition characterSkinDefinition))
			{
				Log.Error($"Could not find character skin definition with id {skinId}.", 150, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\Objects\\CharacterObject.cs");
				yield break;
			}
			BundleCategory bundleCategory = characterSkinDefinition.bundleCategory;
			string bundleName = AssetBundlesUtility.GetAnimatedCharacterDataBundle(bundleCategory);
			AssetBundleLoadRequest bundleLoadRequest = AssetManager.LoadAssetBundle(bundleName);
			m_activeCharacterDataBundleCategory = bundleCategory;
			while (!bundleLoadRequest.get_isDone())
			{
				yield return null;
			}
			if (AssetManagerError.op_Implicit(bundleLoadRequest.get_error()) != 0)
			{
				Log.Error($"Failed to load asset bundle named '{bundleName}' for character skin {characterSkinDefinition.get_displayName()} ({characterSkinDefinition.get_id()}): {bundleLoadRequest.get_error()}", 169, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\Objects\\CharacterObject.cs");
				yield break;
			}
			AssetReference animatedCharacterDataReference = characterSkinDefinition.GetAnimatedCharacterDataReference(gender);
			AssetLoadRequest<AnimatedCharacterData> animatedCharacterDataLoadRequest = animatedCharacterDataReference.LoadFromAssetBundleAsync<AnimatedCharacterData>(bundleName);
			while (!animatedCharacterDataLoadRequest.get_isDone())
			{
				yield return null;
			}
			if (AssetManagerError.op_Implicit(animatedCharacterDataLoadRequest.get_error()) != 0)
			{
				AssetManager.UnloadAssetBundle(bundleName);
				m_activeCharacterDataBundleCategory = BundleCategory.None;
				Log.Error(string.Format("Failed to load {0} asset from bundle '{1}' for character skin {2} ({3}): {4}", "AnimatedCharacterData", bundleName, characterSkinDefinition.get_displayName(), characterSkinDefinition.get_id(), animatedCharacterDataLoadRequest.get_error()), 186, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\Objects\\CharacterObject.cs");
				yield break;
			}
			AnimatedCharacterData asset = animatedCharacterDataLoadRequest.get_asset();
			SetAnimatedCharacterData(asset);
			m_animatedCharacterDataBundleCategory = m_activeCharacterDataBundleCategory;
			m_activeCharacterDataBundleCategory = BundleCategory.None;
			yield return asset.LoadTimelineResources();
			yield return SetAnimatorDefinition();
		}

		public IEnumerator ChangeAnimatedCharacterData(int skinId, Gender gender)
		{
			if (!RuntimeData.characterSkinDefinitions.TryGetValue(skinId, out CharacterSkinDefinition characterSkinDefinition))
			{
				Log.Error($"Could not find character skin definition with id {skinId}.", 211, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\Objects\\CharacterObject.cs");
				yield break;
			}
			BundleCategory bundleCategory = characterSkinDefinition.bundleCategory;
			string bundleName = AssetBundlesUtility.GetAnimatedCharacterDataBundle(bundleCategory);
			AssetBundleLoadRequest bundleLoadRequest = AssetManager.LoadAssetBundle(bundleName);
			m_activeCharacterDataBundleCategory = bundleCategory;
			while (!bundleLoadRequest.get_isDone())
			{
				yield return null;
			}
			if (AssetManagerError.op_Implicit(bundleLoadRequest.get_error()) != 0)
			{
				Log.Error($"Failed to load asset bundle named '{bundleName}' for character skin {characterSkinDefinition.get_displayName()} ({characterSkinDefinition.get_id()}): {bundleLoadRequest.get_error()}", 230, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\Objects\\CharacterObject.cs");
				yield break;
			}
			AssetReference animatedCharacterDataReference = characterSkinDefinition.GetAnimatedCharacterDataReference(gender);
			AssetLoadRequest<AnimatedCharacterData> animatedCharacterDataLoadRequest = animatedCharacterDataReference.LoadFromAssetBundleAsync<AnimatedCharacterData>(bundleName);
			while (!animatedCharacterDataLoadRequest.get_isDone())
			{
				yield return null;
			}
			if (AssetManagerError.op_Implicit(animatedCharacterDataLoadRequest.get_error()) != 0)
			{
				AssetManager.UnloadAssetBundle(bundleName);
				m_activeCharacterDataBundleCategory = BundleCategory.None;
				Log.Error(string.Format("Failed to load requested {0} asset from bundle '{1}' for character skin {2} ({3}): {4}", "AnimatedCharacterData", bundleName, characterSkinDefinition.get_displayName(), characterSkinDefinition.get_id(), animatedCharacterDataLoadRequest.get_error()), 247, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\Objects\\CharacterObject.cs");
				yield break;
			}
			AnimatedCharacterData newAnimatedCharacterData = animatedCharacterDataLoadRequest.get_asset();
			yield return newAnimatedCharacterData.LoadTimelineResources();
			AnimatedCharacterData animatedCharacterData = GetAnimatedCharacterData();
			if (null != animatedCharacterData)
			{
				string animatedCharacterDataBundle = AssetBundlesUtility.GetAnimatedCharacterDataBundle(m_animatedCharacterDataBundleCategory);
				animatedCharacterData.UnloadTimelineResources();
				AssetManager.UnloadAssetBundle(animatedCharacterDataBundle);
				m_animatedCharacterDataBundleCategory = BundleCategory.None;
			}
			SetAnimatedCharacterData(newAnimatedCharacterData);
			m_animatedCharacterDataBundleCategory = m_activeCharacterDataBundleCategory;
			m_activeCharacterDataBundleCategory = BundleCategory.None;
			IAnimator2D animator = GetAnimator();
			int animationFrame = animator.get_currentFrame();
			yield return SetAnimatorDefinition();
			animator.set_currentFrame(animationFrame);
		}

		public Color GetColorModifier()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return m_colorModifier;
		}

		public virtual void SetColorModifier(Color value)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			m_colorModifier = value;
			foreach (VisualEffect value2 in m_propertyEffects.Values)
			{
				value2.SetColorModifier(value);
			}
			if (null != m_currentFloatingCounterFeedback)
			{
				m_currentFloatingCounterFeedback.SetColorModifier(value);
			}
		}

		public IEnumerator AddPropertyEffect(AttachableEffect attachableEffect, PropertyId propertyId)
		{
			VisualEffect visualEffect = attachableEffect.InstantiateMainEffect(m_attachableEffectsContainer, this);
			if (null != visualEffect)
			{
				m_propertyEffects.Add(propertyId, visualEffect);
				visualEffect.Play();
			}
			yield return (object)new WaitForTime(attachableEffect.mainEffectDelay);
		}

		public IEnumerator RemovePropertyEffect(AttachableEffect attachableEffect, PropertyId propertyId)
		{
			if (m_propertyEffects.TryGetValue(propertyId, out VisualEffect value))
			{
				if (null != value)
				{
					value.Stop();
				}
				m_propertyEffects.Remove(propertyId);
			}
			VisualEffect visualEffect = attachableEffect.InstantiateStopEffect(m_attachableEffectsContainer, this);
			if (null != visualEffect)
			{
				visualEffect.Play();
			}
			yield return (object)new WaitForTime(attachableEffect.stopEffectDelay);
		}

		protected virtual void ClearAttachedEffects()
		{
			foreach (VisualEffect value in m_propertyEffects.Values)
			{
				value.Stop();
			}
			m_propertyEffects.Clear();
		}

		protected virtual void DestroyAttachedEffects()
		{
			foreach (VisualEffect value in m_propertyEffects.Values)
			{
				Object.Destroy(value);
			}
			m_propertyEffects.Clear();
		}

		public IEnumerator Spawn()
		{
			yield return PlaySpawnAnimation();
			yield return PlaySpawnEffect();
			CheckParentCellIndicator();
		}

		public IEnumerator Die()
		{
			yield return PlayDeathAnimation();
			ClearAttachedEffects();
			yield return PlayDeathEffect();
		}

		public IEnumerator PlaySpawnEffect()
		{
			CharacterEffect spawnEffect = GetAnimatedCharacterData().spawnEffect;
			if (!(null == spawnEffect))
			{
				Component val = spawnEffect.Instantiate(m_cellObject.get_transform(), this);
				if (null != val)
				{
					this.StartCoroutine(spawnEffect.DestroyWhenFinished(val));
				}
				m_spawnEffectInstance = val;
				do
				{
					yield return null;
				}
				while (null != m_spawnEffectInstance && m_spawnEffectInstance.get_gameObject().get_activeSelf());
			}
		}

		public IEnumerator PlayDeathEffect()
		{
			CharacterEffect deathEffect = GetAnimatedCharacterData().deathEffect;
			if (!(null == deathEffect))
			{
				Component val = deathEffect.Instantiate(m_cellObject.get_transform(), this);
				if (null != val)
				{
					this.StartCoroutine(deathEffect.DestroyWhenFinished(val));
				}
				m_deathEffectInstance = val;
				do
				{
					yield return null;
				}
				while (null != m_deathEffectInstance && m_deathEffectInstance.get_gameObject().get_activeSelf());
			}
		}

		public abstract void CheckParentCellIndicator();

		public abstract void ShowSpellTargetFeedback(bool isSelected);

		public abstract void HideSpellTargetFeedback();

		public void SetPosition(IMap map, Vector2 position)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			Vector2Int val = position.RoundToInt();
			CellObject cellObject = map.GetCellObject(val.get_x(), val.get_y());
			SetCellObject(cellObject);
			Vector2 cellObjectInnerPosition = position - Vector2Int.op_Implicit(val);
			SetCellObjectInnerPosition(cellObjectInnerPosition);
		}

		public abstract void ChangeDirection(Direction newDirection);

		protected abstract void PlayIdleAnimation();

		protected abstract IEnumerator PlaySpawnAnimation();

		protected abstract IEnumerator PlayDeathAnimation();

		public IEnumerator Pull(Vector2Int[] movementCells)
		{
			return SlideRoutine(movementCells, followDirection: true);
		}

		public IEnumerator Push(Vector2Int[] movementCells)
		{
			return SlideRoutine(movementCells, followDirection: false);
		}

		public virtual void Teleport(Vector2Int target)
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			if (null != m_cellObject)
			{
				IMap parentMap = m_cellObject.parentMap;
				if (parentMap != null)
				{
					CellObject cellObject = parentMap.GetCellObject(target.get_x(), target.get_y());
					SetCellObject(cellObject);
					SetCellObjectInnerPosition(Vector2.get_zero());
					PlayIdleAnimation();
				}
			}
		}

		protected unsafe IEnumerator SlideRoutine(Vector2Int[] movementCells, bool followDirection)
		{
			int movementCellsCount = movementCells.Length;
			if (movementCellsCount == 0)
			{
				yield break;
			}
			CellObject cellObject = base.cellObject;
			IMap parentMap = cellObject.parentMap;
			Vector2Int val = movementCells[0];
			if (cellObject.coords != val)
			{
				Log.Warning($"Was not on the start cell of a new movement sequence: {cellObject.coords} instead of {val} ({this.get_gameObject().get_name()}).", 528, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\Objects\\CharacterObject.cs");
				CellObject cellObject2 = parentMap.GetCellObject(val.get_x(), val.get_y());
				SetCellObject(cellObject2);
			}
			if (movementCellsCount > 1)
			{
				Direction direction = followDirection ? val.GetDirectionTo(movementCells[1]) : movementCells[1].GetDirectionTo(val);
				if (!direction.IsAxisAligned())
				{
					direction = direction.GetAxisAligned(this.direction);
				}
				ChangeDirection(direction);
			}
			Vector2Int val2 = val;
			float cellTraversalDuration = 2f / (float)GetAnimator().get_frameRate();
			int num;
			for (int i = 1; i < movementCellsCount; i = num)
			{
				Vector2Int cellCoords = movementCells[i];
				CellObject movementCell = parentMap.GetCellObject(cellCoords.get_x(), cellCoords.get_y());
				bool goingUp = ((IntPtr)(void*)movementCell.get_transform().get_position()).y >= ((IntPtr)(void*)cellObject.get_transform().get_position()).y;
				Vector2 innerPositionStart;
				Vector2 innerPositionEnd;
				if (goingUp)
				{
					SetCellObject(movementCell);
					innerPositionStart = Vector2Int.op_Implicit(val2 - cellCoords);
					innerPositionEnd = Vector2.get_zero();
				}
				else
				{
					innerPositionStart = Vector2.get_zero();
					innerPositionEnd = Vector2Int.op_Implicit(cellCoords - val2);
				}
				float animationTime = 0f;
				do
				{
					Vector2 cellObjectInnerPosition = Vector2.Lerp(innerPositionStart, innerPositionEnd, animationTime / cellTraversalDuration);
					SetCellObjectInnerPosition(cellObjectInnerPosition);
					yield return null;
					animationTime += Time.get_deltaTime();
				}
				while (animationTime < cellTraversalDuration);
				SetCellObjectInnerPosition(innerPositionEnd);
				if (!goingUp)
				{
					SetCellObject(movementCell);
				}
				val2 = cellCoords;
				cellObject = movementCell;
				if (i < movementCellsCount - 1 && movementCell.TryGetIsoObject(out IObjectWithActivation isoObject))
				{
					isoObject.PlayDetectionAnimation();
				}
				cellCoords = default(Vector2Int);
				num = i + 1;
			}
		}

		public virtual void SetFocus(bool value)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			if (null == this || value == m_isFocused)
			{
				return;
			}
			if (value)
			{
				if (FightUIRework.tooltipsEnabled)
				{
					FocusCharacter();
				}
				TooltipWindowUtility.ShowFightCharacterTooltip(this, this.get_transform().get_position());
			}
			else
			{
				UnFocusCharacter();
				FightUIRework.HideTooltip();
			}
			m_isFocused = value;
		}

		protected abstract void FocusCharacter();

		protected abstract void UnFocusCharacter();

		public IEnumerator InitializeFloatingCounterEffect(FloatingCounterEffect floatingCounterEffect, int value)
		{
			if (null != m_currentFloatingCounterFeedback)
			{
				Log.Warning("InitializeFloatingCounterEffect called on " + base.GetType().Name + " named " + this.get_name() + " but a floating counter feedback is already attached.", 662, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\Objects\\CharacterObject.cs");
			}
			FloatingCounterFeedback floatingCounterFeedback = FightSpellEffectFactory.InstantiateFloatingCounterFeedback(m_attachableEffectsContainer);
			if (!(null == floatingCounterFeedback))
			{
				m_currentFloatingCounterFeedback = floatingCounterFeedback;
				yield return floatingCounterFeedback.Launch(this, floatingCounterEffect, value);
			}
		}

		public IEnumerator ChangeFloatingCounterEffect(FloatingCounterEffect floatingCounterEffect)
		{
			if (m_currentFloatingCounterFeedback != null && m_currentFloatingCounterFeedback.effect != floatingCounterEffect)
			{
				int count = m_currentFloatingCounterFeedback.objectsCount;
				yield return RemoveFloatingCounterEffect();
				yield return InitializeFloatingCounterEffect(floatingCounterEffect, count);
			}
		}

		public IEnumerator RemoveFloatingCounterEffect()
		{
			if (null != m_currentFloatingCounterFeedback)
			{
				yield return m_currentFloatingCounterFeedback.FadeOut();
				ClearFloatingCounterEffect();
			}
		}

		public void ClearFloatingCounterEffect()
		{
			if (null != m_currentFloatingCounterFeedback)
			{
				FightSpellEffectFactory.DestroyFloatingCounterFeedback(m_currentFloatingCounterFeedback);
				m_currentFloatingCounterFeedback = null;
			}
		}

		[CanBeNull]
		public FloatingCounterFeedback GetCurrentFloatingCounterFeedback()
		{
			return m_currentFloatingCounterFeedback;
		}

		public Object GetTimelineBinding()
		{
			return this;
		}

		public abstract ITimelineContext GetTimelineContext();

		public abstract int GetTitleKey();

		public abstract int GetDescriptionKey();

		public abstract IFightValueProvider GetValueProvider();

		protected abstract IEnumerator SetAnimatorDefinition();

		protected abstract void ClearAnimatorDefinition();

		private void OnAnimationLooped(object sender, AnimationLoopedEventArgs e)
		{
			if (m_hasTimeline)
			{
				m_playableDirector.set_time(0.0);
				m_playableDirector.Resume();
			}
		}

		protected virtual void OnMapRotationChanged(DirectionAngle previousMapRotation, DirectionAngle newMapRotation)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			Transform transform = this.get_transform();
			transform.set_rotation(transform.get_rotation() * (previousMapRotation.GetRotation() * newMapRotation.GetInverseRotation()));
			m_mapRotation = newMapRotation;
		}

		GameObject IIsoObject.get_gameObject()
		{
			return this.get_gameObject();
		}

		Transform IIsoObject.get_transform()
		{
			return this.get_transform();
		}
	}
}
