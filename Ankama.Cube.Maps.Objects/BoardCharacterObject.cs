using Ankama.Animations;
using Ankama.Animations.Events;
using Ankama.Cube.Animations;
using Ankama.Cube.Data;
using Ankama.Cube.Data.UI.Localization.TextFormatting.ValueProviders.FightValueProviders;
using Ankama.Cube.Extensions;
using Ankama.Cube.Fight;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps.UI;
using Ankama.Cube.SRP;
using Ankama.Utilities;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Timeline;

namespace Ankama.Cube.Maps.Objects
{
	public abstract class BoardCharacterObject : CharacterObject, IObjectWithArmoredLife, ICharacterObject, IMovableIsoObject, IIsoObject, IObjectTargetableByAction
	{
		[SerializeField]
		protected Animator2D m_animator2D;

		[SerializeField]
		protected CharacterBase m_base;

		[SerializeField]
		protected CharacterUIContainer m_uiContainer;

		[SerializeField]
		protected CharacterAttackableUI m_attackableUI;

		protected DynamicFightValueProvider m_tooltipValueProvider;

		protected CharacterAnimationParameters m_animationParameters;

		protected bool m_forceDisplayUI;

		public int life
		{
			get;
			protected set;
		}

		public int armor
		{
			get;
			protected set;
		}

		public int baseLife
		{
			get;
			protected set;
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			SetComponentsActive(value: false);
		}

		protected override void SetAnimatedCharacterData(AnimatedCharacterData data)
		{
			AnimatedBoardCharacterData animatedBoardCharacterData = (AnimatedBoardCharacterData)data;
			m_uiContainer.SetCharacterHeight(animatedBoardCharacterData.height);
		}

		protected void Initialize<T>(FightStatus fightStatus, PlayerStatus ownerStatus, T characterStatus) where T : class, IDynamicValueSource, IEntityWithLevel, IEntityWithTeam
		{
			m_isFocused = false;
			m_forceDisplayUI = false;
			m_tooltipValueProvider = new DynamicFightValueProvider(characterStatus, characterStatus.level);
			SetComponentsActive(value: true);
			m_uiContainer.Setup();
			m_base.Setup(ownerStatus.playerType);
			m_attackableUI.Setup();
		}

		public override void SetColorModifier(Color value)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			base.SetColorModifier(value);
			m_animator2D.set_color(value);
			m_uiContainer.color = value;
			m_base.color = value;
			m_attackableUI.color = value;
		}

		private void SetComponentsActive(bool value)
		{
			m_base.get_gameObject().SetActive(value);
			m_uiContainer.get_gameObject().SetActive(value);
		}

		protected unsafe override IEnumerator SetAnimatorDefinition()
		{
			Animator2D animator = m_animator2D;
			animator.add_Initialised(new Animator2DInitialisedEventHandler((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			AnimatedBoardCharacterData animatedBoardCharacterData = (AnimatedBoardCharacterData)GetAnimatedCharacterData();
			animator.SetDefinition(animatedBoardCharacterData.animatedObjectDefinition, null, (Graphic[])null);
			while (true)
			{
				Animator2DInitialisationState initialisationState = animator.GetInitialisationState();
				if ((int)initialisationState == 1 || ((int)initialisationState == 2 && animator.get_isActiveAndEnabled()))
				{
					yield return null;
					continue;
				}
				break;
			}
		}

		private unsafe void OnAnimatorInitialized(object sender, Animator2DInitialisedEventArgs e)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected O, but got Unknown
			Animator2D animator2D = m_animator2D;
			animator2D.remove_Initialised(new Animator2DInitialisedEventHandler((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			animator2D.set_paused(false);
			PlayIdleAnimation();
		}

		protected unsafe override void ClearAnimatorDefinition()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			m_animator2D.remove_Initialised(new Animator2DInitialisedEventHandler((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_animator2D.SetDefinition(null, null, (Graphic[])null);
		}

		protected void StartFightAnimation(CharacterAnimationInfo animationInfo, Action onComplete = null, Action onCancel = null, bool restart = true, bool async = false)
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			string animationName = animationInfo.animationName;
			string timelineKey = animationInfo.timelineKey;
			m_animator2D.get_transform().set_localRotation(animationInfo.flipX ? Quaternion.Euler(0f, -135f, 0f) : Quaternion.Euler(0f, 45f, 0f));
			direction = animationInfo.direction;
			ITimelineAssetProvider animatedCharacterData = GetAnimatedCharacterData();
			if (animatedCharacterData != null)
			{
				TimelineAsset timelineAsset;
				bool flag = animatedCharacterData.TryGetTimelineAsset(timelineKey, out timelineAsset);
				if (flag && null != timelineAsset)
				{
					if (timelineAsset != m_playableDirector.get_playableAsset())
					{
						m_playableDirector.Play(timelineAsset);
					}
					else
					{
						if (restart || !m_animator2D.get_animationName().Equals(animationName))
						{
							m_playableDirector.set_time(0.0);
						}
						m_playableDirector.Resume();
					}
					m_hasTimeline = true;
				}
				else
				{
					if (flag)
					{
						Log.Warning("Character named '" + GetAnimatedCharacterData().get_name() + "' has a timeline setup for key '" + timelineKey + "' but the actual asset is null.", 171, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\Objects\\BoardCharacterObject.cs");
					}
					m_playableDirector.set_time(0.0);
					m_playableDirector.Pause();
					m_hasTimeline = false;
				}
			}
			m_animationCallback.Setup(animationName, restart, onComplete, onCancel);
			m_animator2D.SetAnimation(animationName, animationInfo.loops, async, restart);
			m_animationParameters = animationInfo.parameters;
		}

		protected override void OnDisable()
		{
			SetFocus(value: false);
			base.OnDisable();
		}

		protected override IAnimator2D GetAnimator()
		{
			return m_animator2D;
		}

		protected override void PlayIdleAnimation()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			CharacterAnimationInfo animationInfo = new CharacterAnimationInfo(Vector2Int.op_Implicit(m_cellObject.coords), "idle", "idle", loops: true, m_direction, m_mapRotation);
			StartFightAnimation(animationInfo, null, null, restart: false);
		}

		protected override IEnumerator PlaySpawnAnimation()
		{
			CharacterAnimationInfo animationInfo = new CharacterAnimationInfo(Vector2Int.op_Implicit(m_cellObject.coords), "idle", "spawn", loops: false, direction, m_mapRotation);
			StartFightAnimation(animationInfo, (Action)((CharacterObject)this).PlayIdleAnimation, (Action)null, restart: true, async: false);
			yield break;
		}

		protected override IEnumerator PlayDeathAnimation()
		{
			CharacterAnimationInfo deathAnimationInfo = new CharacterAnimationInfo(Vector2Int.op_Implicit(m_cellObject.coords), "hit", "death", loops: false, direction, m_mapRotation);
			StartFightAnimation(deathAnimationInfo, null, null, restart: false);
			Animator2D animator = m_animator2D;
			int num = default(int);
			if (animator.CurrentAnimationHasLabel("die", ref num))
			{
				while (!CharacterObjectUtility.HasAnimationReachedLabel(animator, deathAnimationInfo, "die"))
				{
					yield return null;
				}
				animator.set_paused(true);
			}
			else
			{
				Log.Warning(animator.GetDefinition().get_name() + " is missing the 'die' label in the animation named '" + deathAnimationInfo.animationName + "'.", 244, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\Objects\\BoardCharacterObject.cs");
			}
		}

		public override void CheckParentCellIndicator()
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			CellObject cellObject = m_cellObject;
			if (null == cellObject)
			{
				m_uiContainer.SetCellIndicator(MapCellIndicator.None);
				return;
			}
			IMap parentMap = cellObject.parentMap;
			if (parentMap == null)
			{
				m_uiContainer.SetCellIndicator(MapCellIndicator.None);
				return;
			}
			Vector2Int coords = cellObject.coords;
			MapCellIndicator cellIndicator = parentMap.GetCellIndicator(coords.get_x(), coords.get_y());
			m_uiContainer.SetCellIndicator(cellIndicator);
		}

		public override void ShowSpellTargetFeedback(bool isSelected)
		{
			m_forceDisplayUI = true;
			this.get_gameObject().SetLayerRecursively(LayerMaskNames.characterFocusLayer);
			m_base.SetTargetState((!isSelected) ? CharacterBase.TargetState.Targetable : CharacterBase.TargetState.Targeted);
		}

		public override void HideSpellTargetFeedback()
		{
			m_forceDisplayUI = false;
			this.get_gameObject().SetLayerRecursively(LayerMaskNames.defaultLayer);
			m_base.SetTargetState(CharacterBase.TargetState.None);
		}

		public override void ChangeDirection(Direction newDirection)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			if (newDirection != m_direction)
			{
				Vector2 position = Vector2Int.op_Implicit(m_cellObject.coords);
				Animator2D animator2D = m_animator2D;
				CharacterAnimationParameters animationParameters = m_animationParameters;
				CharacterAnimationInfo characterAnimationInfo = (m_animationParameters.secondDirection == Direction.None) ? new CharacterAnimationInfo(position, animationParameters.animationName, animationParameters.timelineKey, animationParameters.loops, newDirection, m_mapRotation) : new CharacterAnimationInfo(previousDirection: DirectionExtensions.Rotate(angle: m_direction.DirectionAngleTo(newDirection), value: animationParameters.firstDirection), position: position, animationName: animationParameters.animationName, timelineKey: animationParameters.timelineKey, loops: animationParameters.loops, direction: newDirection, mapRotation: m_mapRotation);
				animator2D.get_transform().set_localRotation(characterAnimationInfo.flipX ? Quaternion.Euler(0f, -135f, 0f) : Quaternion.Euler(0f, 45f, 0f));
				direction = newDirection;
				string animationName = characterAnimationInfo.animationName;
				int currentFrame = animator2D.get_currentFrame();
				m_animationCallback.ChangeAnimationName(animationName);
				animator2D.SetAnimation(animationName, characterAnimationInfo.loops, false, true);
				animator2D.set_currentFrame(currentFrame);
				m_animationParameters = characterAnimationInfo.parameters;
			}
		}

		public override void Teleport(Vector2Int target)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			if (!(null != m_cellObject))
			{
				return;
			}
			IMap parentMap = m_cellObject.parentMap;
			if (parentMap != null)
			{
				Direction direction = m_cellObject.coords.GetDirectionTo(target);
				if (!direction.IsAxisAligned())
				{
					direction = direction.GetAxisAligned(this.direction);
				}
				this.direction = direction;
				CellObject cellObject = parentMap.GetCellObject(target.get_x(), target.get_y());
				SetCellObject(cellObject);
				SetCellObjectInnerPosition(Vector2.get_zero());
				PlayIdleAnimation();
			}
		}

		public virtual void ShowActionTargetFeedback(ActionType sourceActionType, bool isSelected)
		{
			m_forceDisplayUI = true;
			m_base.SetTargetState((!isSelected) ? CharacterBase.TargetState.Targetable : CharacterBase.TargetState.Targeted);
			m_attackableUI.SetValue(sourceActionType, isSelected);
		}

		public virtual void HideActionTargetFeedback()
		{
			m_forceDisplayUI = false;
			m_base.SetTargetState(CharacterBase.TargetState.None);
			m_attackableUI.SetValue(ActionType.None, selected: false);
		}

		public virtual void SetArmoredLife(int lifeValue, int armorValue)
		{
			life = lifeValue;
			armor = armorValue;
		}

		public virtual void SetBaseLife(int lifeValue)
		{
			baseLife = lifeValue;
		}

		public IEnumerator PlayHitAnimation()
		{
			CharacterAnimationInfo hitAnimationInfo = new CharacterAnimationInfo(Vector2Int.op_Implicit(m_cellObject.coords), "hit", "hit", loops: false, direction, m_mapRotation);
			StartFightAnimation(hitAnimationInfo, (Action)((CharacterObject)this).PlayIdleAnimation, (Action)null, restart: true, async: false);
			Animator2D animator = m_animator2D;
			while (!CharacterObjectUtility.HasAnimationReachedLabel(animator, hitAnimationInfo, "die"))
			{
				yield return null;
			}
		}

		public IEnumerator PlayLethalHitAnimation()
		{
			Animator2D animator = m_animator2D;
			string animationNameBackup = animator.get_animationName();
			yield return null;
			if (string.Equals(animator.get_animationName(), animationNameBackup))
			{
				CharacterAnimationInfo animationInfo = new CharacterAnimationInfo(Vector2Int.op_Implicit(m_cellObject.coords), "hit", "hit", loops: false, direction, m_mapRotation);
				StartFightAnimation(animationInfo, (Action)((CharacterObject)this).PlayIdleAnimation, (Action)null, restart: true, async: false);
			}
		}

		protected override void FocusCharacter()
		{
			m_attackableUI.sortingOrder = 1;
		}

		protected override void UnFocusCharacter()
		{
			m_attackableUI.sortingOrder = 0;
		}

		public override IFightValueProvider GetValueProvider()
		{
			return m_tooltipValueProvider;
		}

		protected override void OnMapRotationChanged(DirectionAngle previousMapRotation, DirectionAngle newMapRotation)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Invalid comparison between Unknown and I4
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			base.OnMapRotationChanged(previousMapRotation, newMapRotation);
			Animator2D animator2D = m_animator2D;
			if ((int)animator2D.GetInitialisationState() == 3)
			{
				Vector2 position = Vector2Int.op_Implicit(m_cellObject.coords);
				CharacterAnimationParameters animationParameters = m_animationParameters;
				CharacterAnimationInfo characterAnimationInfo;
				if (m_animationParameters.secondDirection == Direction.None)
				{
					characterAnimationInfo = new CharacterAnimationInfo(position, animationParameters.animationName, animationParameters.timelineKey, animationParameters.loops, direction, newMapRotation);
				}
				else
				{
					DirectionAngle angle = newMapRotation.Substract(previousMapRotation);
					Direction previousDirection = animationParameters.firstDirection.Rotate(angle);
					characterAnimationInfo = new CharacterAnimationInfo(position, animationParameters.animationName, animationParameters.timelineKey, animationParameters.loops, previousDirection, direction, newMapRotation);
				}
				animator2D.get_transform().set_localRotation(characterAnimationInfo.flipX ? Quaternion.Euler(0f, -135f, 0f) : Quaternion.Euler(0f, 45f, 0f));
				string animationName = characterAnimationInfo.animationName;
				int currentFrame = animator2D.get_currentFrame();
				m_animationCallback.ChangeAnimationName(animationName);
				animator2D.SetAnimation(animationName, characterAnimationInfo.loops, false, true);
				animator2D.set_currentFrame(currentFrame);
				m_animationParameters = characterAnimationInfo.parameters;
			}
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
