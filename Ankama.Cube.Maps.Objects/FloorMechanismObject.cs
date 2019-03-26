using Ankama.Animations;
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
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace Ankama.Cube.Maps.Objects
{
	public sealed class FloorMechanismObject : CharacterObject, IObjectWithActivation, ICharacterObject, IMovableIsoObject, IIsoObject, IObjectWithAssemblage, ITextTooltipDataProvider, ITooltipDataProvider
	{
		[SerializeField]
		private FloorMechanismDefinition m_definition;

		[SerializeField]
		private FloorMechanismBase m_base;

		private FloorMechanismAnimator m_animator;

		private GameObject m_instance;

		private AnimatedFloorMechanismData m_characterData;

		private DynamicFightValueProvider m_tooltipValueProvider;

		private bool m_alliedWithLocalPlayer;

		private FloorMechanismObjectContext m_context;

		public override IsoObjectDefinition definition
		{
			get
			{
				return m_definition;
			}
			protected set
			{
				m_definition = (FloorMechanismDefinition)value;
			}
		}

		public bool alliedWithLocalPlayer
		{
			get
			{
				return m_alliedWithLocalPlayer;
			}
			set
			{
				m_alliedWithLocalPlayer = value;
				m_direction = (value ? Direction.SouthEast.Rotate(m_mapRotation) : Direction.SouthWest.Rotate(m_mapRotation));
			}
		}

		public override Direction direction
		{
			get
			{
				return m_direction;
			}
			set
			{
			}
		}

		public override TooltipDataType tooltipDataType
		{
			get;
		} = TooltipDataType.FloorMechanism;


		public override KeywordReference[] keywordReferences => m_definition.precomputedData.keywordReferences;

		public void Initialize(FightStatus fightStatus, PlayerStatus ownerStatus, FloorMechanismStatus floorMechanismStatus)
		{
			m_tooltipValueProvider = new DynamicFightValueProvider(floorMechanismStatus, floorMechanismStatus.level);
			TimelineContextUtility.SetFightContext(m_playableDirector, fightStatus.context);
			m_base.Setup(ownerStatus.playerType);
		}

		public override void SetColorModifier(Color value)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			base.SetColorModifier(value);
			m_base.color = value;
		}

		public override void Destroy()
		{
			FightObjectFactory.ReleaseFloorMechanismObject(this);
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			m_context = new FloorMechanismObjectContext(this);
			m_context.Initialize();
		}

		private void PlayTimeline(string timelineKey, string animationName, bool restart)
		{
			ITimelineAssetProvider characterData = m_characterData;
			if (characterData == null)
			{
				return;
			}
			TimelineAsset timelineAsset;
			bool flag = characterData.TryGetTimelineAsset(timelineKey, out timelineAsset);
			if (flag && null != timelineAsset)
			{
				if (timelineAsset != m_playableDirector.get_playableAsset())
				{
					m_playableDirector.Play(timelineAsset);
				}
				else
				{
					if (restart || !m_animator.animationName.Equals(animationName))
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
					Log.Warning("Character named '" + GetAnimatedCharacterData().get_name() + "' has a timeline setup for key '" + timelineKey + "' but the actual asset is null.", 121, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\Objects\\FloorMechanismObject.cs");
				}
				m_playableDirector.set_time(0.0);
				m_playableDirector.Pause();
				m_hasTimeline = false;
			}
		}

		protected override IAnimator2D GetAnimator()
		{
			return m_animator;
		}

		protected override AnimatedCharacterData GetAnimatedCharacterData()
		{
			return m_characterData;
		}

		protected override void SetAnimatedCharacterData(AnimatedCharacterData data)
		{
			AnimatedFloorMechanismData animatedFloorMechanismData = data as AnimatedFloorMechanismData;
			if (null == animatedFloorMechanismData)
			{
				Log.Error("Data type mismatch: an instance of " + base.GetType().Name + " cannot be created with a data asset of type '" + ((object)data).GetType().Name + "'.", 149, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\Objects\\FloorMechanismObject.cs");
			}
			else
			{
				m_characterData = animatedFloorMechanismData;
			}
		}

		protected override void ClearAnimatedCharacterData()
		{
			m_characterData = null;
		}

		protected override IEnumerator SetAnimatorDefinition()
		{
			GameObject prefab = m_characterData.prefab;
			if (null == prefab)
			{
				Log.Error("AnimatedFloorMechanismData named '" + m_characterData.get_name() + " has no prefab set.", 169, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\Objects\\FloorMechanismObject.cs");
				yield break;
			}
			m_instance = Object.Instantiate<GameObject>(prefab, this.get_transform().get_position(), m_direction.GetRotation(), this.get_transform());
			m_animator = m_instance.GetComponent<FloorMechanismAnimator>();
			if (null == m_animator)
			{
				Log.Error("Floor mechanism prefab named '" + prefab.get_name() + "' is missing a FloorMechanismAnimator component.", 178, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\Objects\\FloorMechanismObject.cs");
			}
			InitializeAnimator();
		}

		protected override void ClearAnimatorDefinition()
		{
			if (null != m_instance)
			{
				Object.Destroy(m_instance);
				m_instance = null;
			}
		}

		protected override void PlayIdleAnimation()
		{
			if (!(null == m_animator) && m_animator.TryGetIdleAnimationName(out string animName))
			{
				PlayTimeline("idle", animName, restart: false);
				m_animationCallback.Setup(animName, restart: false);
				m_animator.SetAnimation(animName, animLoops: true, async: true, restart: false);
			}
		}

		protected override IEnumerator PlaySpawnAnimation()
		{
			if (!(null == m_animator))
			{
				if (m_animator.TryGetSpawnAnimationName(out string animName))
				{
					PlayTimeline("spawn", animName, restart: false);
					m_animationCallback.Setup(animName, restart: false, (Action)((CharacterObject)this).PlayIdleAnimation, (Action)null, (Action)null);
					m_animator.SetAnimation(animName, animLoops: false);
				}
				else if (m_animator.TryGetIdleAnimationName(out animName))
				{
					PlayTimeline("spawn", animName, restart: true);
					m_animationCallback.Setup(animName, restart: false, (Action)((CharacterObject)this).PlayIdleAnimation, (Action)null, (Action)null);
					m_animator.SetAnimation(animName, animLoops: true);
				}
			}
			yield break;
		}

		protected override IEnumerator PlayDeathAnimation()
		{
			if (!(null == m_animator) && m_animator.TryGetDestructionAnimationName(out string animName))
			{
				PlayTimeline("die", animName, restart: false);
				m_animationCallback.Setup(animName, restart: false);
				m_animator.SetAnimation(animName, animLoops: false);
			}
			yield break;
		}

		public override void ShowSpellTargetFeedback(bool isSelected)
		{
			this.get_gameObject().SetLayerRecursively(LayerMaskNames.characterFocusLayer);
			m_base.SetTargetState((!isSelected) ? FloorMechanismBase.TargetState.Targetable : FloorMechanismBase.TargetState.Targeted);
		}

		public override void HideSpellTargetFeedback()
		{
			this.get_gameObject().SetLayerRecursively(LayerMaskNames.defaultLayer);
			m_base.SetTargetState(FloorMechanismBase.TargetState.None);
		}

		public override void ChangeDirection(Direction newDirection)
		{
		}

		public override void CheckParentCellIndicator()
		{
		}

		public IEnumerator ActivatedByAlly()
		{
			FloorMechanismAnimator animator = m_animator;
			if (!(null == animator) && animator.TryGetAllyActivationAnimationName(out string activationAnimationName))
			{
				PlayTimeline("ally_activation", activationAnimationName, restart: false);
				m_animationCallback.Setup(activationAnimationName, restart: false);
				animator.SetAnimation(activationAnimationName, animLoops: false, async: false);
				do
				{
					yield return null;
				}
				while (!CharacterObjectUtility.HasAnimationReachedLabel(animator, activationAnimationName, "shot"));
			}
		}

		public IEnumerator ActivatedByOpponent()
		{
			FloorMechanismAnimator animator = m_animator;
			if (!(null == animator) && animator.TryGetOpponentActivationAnimationName(out string activationAnimationName))
			{
				PlayTimeline("opponent_activation", activationAnimationName, restart: false);
				m_animationCallback.Setup(activationAnimationName, restart: false);
				animator.SetAnimation(activationAnimationName, animLoops: false, async: false);
				do
				{
					yield return null;
				}
				while (!CharacterObjectUtility.HasAnimationReachedLabel(animator, activationAnimationName, "shot"));
			}
		}

		public IEnumerator WaitForActivationEnd()
		{
			FloorMechanismAnimator animator = m_animator;
			if (!(null == animator))
			{
				string activationAnimationName = animator.animationName;
				while (!CharacterObjectUtility.HasAnimationEnded(animator, activationAnimationName))
				{
					yield return null;
				}
			}
		}

		public void PlayDetectionAnimation()
		{
			FloorMechanismAnimator animator = m_animator;
			if (!(null == animator) && animator.TryGetDetectionAnimationName(out string animName))
			{
				PlayTimeline("detection", animName, restart: false);
				m_animationCallback.Setup(animName, restart: false);
				animator.SetAnimation(animName, animLoops: false);
			}
		}

		public void RefreshAssemblage(IEnumerable<Vector2Int> otherObjectInAssemblage)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			m_base.RefreshAssemblage(m_cellObject.coords, otherObjectInAssemblage);
		}

		protected override void FocusCharacter()
		{
		}

		protected override void UnFocusCharacter()
		{
		}

		public override ITimelineContext GetTimelineContext()
		{
			return m_context;
		}

		public override int GetTitleKey()
		{
			return m_definition.i18nNameId;
		}

		public override int GetDescriptionKey()
		{
			return m_definition.i18nDescriptionId;
		}

		public override IFightValueProvider GetValueProvider()
		{
			return m_tooltipValueProvider;
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
