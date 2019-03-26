using Ankama.Animations;
using Ankama.Cube.Animations;
using Ankama.Cube.Data;
using Ankama.Cube.Extensions;
using Ankama.Cube.Fight;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps.UI;
using Ankama.Utilities;
using System;
using System.Collections;
using UnityEngine;

namespace Ankama.Cube.Maps.Objects
{
	[SelectionBase]
	public sealed class ObjectMechanismObject : BoardCharacterObject, IObjectWithActivationAnimation, ICharacterObject, IMovableIsoObject, IIsoObject, IObjectMechanismTooltipDataProvider, ITooltipDataProvider
	{
		[SerializeField]
		private ObjectMechanismDefinition m_definition;

		[SerializeField]
		private CharacterMechanismLifeUI m_lifeUI;

		private AnimatedObjectMechanismData m_characterData;

		private bool m_alliedWithLocalPlayer;

		private ObjectMechanismObjectContext m_context;

		public override IsoObjectDefinition definition
		{
			get
			{
				return m_definition;
			}
			protected set
			{
				m_definition = (ObjectMechanismDefinition)value;
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
		} = TooltipDataType.ObjectMechanism;


		public override KeywordReference[] keywordReferences => m_definition.precomputedData.keywordReferences;

		protected override AnimatedCharacterData GetAnimatedCharacterData()
		{
			return m_characterData;
		}

		protected override void SetAnimatedCharacterData(AnimatedCharacterData data)
		{
			base.SetAnimatedCharacterData(data);
			AnimatedObjectMechanismData animatedObjectMechanismData = data as AnimatedObjectMechanismData;
			if (null == animatedObjectMechanismData)
			{
				Log.Error("Data type mismatch: an instance of " + base.GetType().Name + " cannot be created with a data asset of type '" + ((object)data).GetType().Name + "'.", 62, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\Objects\\ObjectMechanismObject.cs");
			}
			else
			{
				m_characterData = animatedObjectMechanismData;
			}
		}

		protected override void ClearAnimatedCharacterData()
		{
			m_characterData = null;
		}

		public void Initialize(FightStatus fightStatus, PlayerStatus ownerStatus, ObjectMechanismStatus objectMechanismStatus)
		{
			base.Initialize(fightStatus, ownerStatus, objectMechanismStatus);
			base.life = objectMechanismStatus.life;
			base.baseLife = objectMechanismStatus.baseLife;
			TimelineContextUtility.SetFightContext(m_playableDirector, fightStatus.context);
			m_lifeUI.set_enabled(false);
			m_lifeUI.SetValue(objectMechanismStatus.life);
		}

		public override void SetColorModifier(Color value)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			base.SetColorModifier(value);
			m_lifeUI.color = value;
		}

		public override void Destroy()
		{
			FightObjectFactory.ReleaseObjectMechanismObject(this);
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			m_context = new ObjectMechanismObjectContext(this);
			m_context.Initialize();
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			m_context.Release();
		}

		public override void ChangeDirection(Direction newDirection)
		{
		}

		public override void SetArmoredLife(int newLife, int newArmor)
		{
			base.SetArmoredLife(newLife, newArmor);
			m_lifeUI.ChangeValue(newLife);
		}

		public override void ShowSpellTargetFeedback(bool isSelected)
		{
			base.ShowSpellTargetFeedback(isSelected);
			m_lifeUI.set_enabled(true);
		}

		public override void HideSpellTargetFeedback()
		{
			base.HideSpellTargetFeedback();
			m_lifeUI.set_enabled(m_isFocused);
		}

		public override void ShowActionTargetFeedback(ActionType sourceActionType, bool isSelected)
		{
			base.ShowActionTargetFeedback(sourceActionType, isSelected);
			m_lifeUI.set_enabled(true);
		}

		public override void HideActionTargetFeedback()
		{
			base.HideActionTargetFeedback();
			m_lifeUI.set_enabled(m_isFocused);
		}

		protected override void FocusCharacter()
		{
			base.FocusCharacter();
			m_lifeUI.sortingOrder = 1;
			m_lifeUI.set_enabled(true);
		}

		protected override void UnFocusCharacter()
		{
			base.UnFocusCharacter();
			m_lifeUI.sortingOrder = 0;
			m_lifeUI.set_enabled(m_forceDisplayUI);
		}

		public IEnumerator PlayActivationAnimation()
		{
			Animator2D animator = m_animator2D;
			if (!m_characterData.hasActivationAnimation)
			{
				Log.Warning(animator.GetDefinition().get_name() + " does not have an activation animation.", 204, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\Objects\\ObjectMechanismObject.cs");
				yield break;
			}
			CharacterAnimationInfo activationAnimationInfo = new CharacterAnimationInfo(Vector2Int.op_Implicit(m_cellObject.coords), "attack", "attack", loops: false, direction, m_mapRotation);
			StartFightAnimation(activationAnimationInfo, (Action)((CharacterObject)this).PlayIdleAnimation, (Action)null, restart: true, async: false);
			while (!CharacterObjectUtility.HasAnimationReachedLabel(animator, activationAnimationInfo, "shot"))
			{
				yield return null;
			}
			TriggerActivationEffect();
		}

		public void TriggerActivationEffect()
		{
			CharacterEffect activationEffect = m_characterData.activationEffect;
			if (!(null == activationEffect))
			{
				Component val = activationEffect.Instantiate(m_cellObject.get_transform(), this);
				if (null != val)
				{
					this.StartCoroutine(activationEffect.DestroyWhenFinished(val));
				}
			}
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

		public int GetArmorValue()
		{
			return base.life;
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
