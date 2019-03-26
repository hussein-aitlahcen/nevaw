using Ankama.Animations;
using Ankama.Cube.Animations;
using Ankama.Cube.Data;
using Ankama.Cube.Extensions;
using Ankama.Cube.Fight;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps.UI;
using Ankama.Cube.UI.Components;
using Ankama.Utilities;
using System;
using System.Collections;
using UnityEngine;

namespace Ankama.Cube.Maps.Objects
{
	public abstract class FightCharacterObject : BoardCharacterObject, IObjectWithMovement, ICharacterObject, IMovableIsoObject, IIsoObject, IObjectWithAction, IObjectWithElementaryState, ICharacterTooltipDataProvider, ITooltipDataProvider
	{
		[SerializeField]
		private CharacterArmoredLifeUI m_lifeUI;

		[SerializeField]
		protected CharacterActionUI m_actionUI;

		[SerializeField]
		private CharacterElementaryStateUI m_elementaryStateUI;

		protected AnimatedFightCharacterData m_characterData;

		private const float MovementSingleCellTraversalTime = 5f;

		private const float MovementMultipleCellTraversalTime = 4f;

		private FightCharacterObjectContext m_context;

		public override Direction direction
		{
			get
			{
				return m_direction;
			}
			set
			{
				if (value != m_direction)
				{
					if (m_context != null)
					{
						m_context.UpdateDirection(m_direction, value);
					}
					m_direction = value;
				}
			}
		}

		public int movementPoints
		{
			get;
			protected set;
		}

		public int baseMovementPoints
		{
			get;
			protected set;
		}

		public int? actionValue
		{
			get;
			protected set;
		}

		public ActionType actionType
		{
			get;
			protected set;
		}

		public int physicalDamageBoost
		{
			get;
			protected set;
		}

		public int physicalHealBoost
		{
			get;
			protected set;
		}

		public bool hasRange
		{
			get;
			protected set;
		}

		public ElementaryStates elementaryState
		{
			get;
			protected set;
		}

		public override TooltipDataType tooltipDataType
		{
			get;
		}

		protected override AnimatedCharacterData GetAnimatedCharacterData()
		{
			return m_characterData;
		}

		protected override void SetAnimatedCharacterData(AnimatedCharacterData data)
		{
			base.SetAnimatedCharacterData(data);
			AnimatedFightCharacterData animatedFightCharacterData = data as AnimatedFightCharacterData;
			if (null == animatedFightCharacterData)
			{
				Log.Error("Data type mismatch: an instance of " + base.GetType().Name + " cannot be created with a data asset of type '" + ((object)data).GetType().Name + "'.", 47, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\Objects\\FightCharacterObject.cs");
			}
			else
			{
				m_characterData = animatedFightCharacterData;
			}
		}

		protected override void ClearAnimatedCharacterData()
		{
			m_characterData = null;
		}

		public void Initialize(FightStatus fightStatus, PlayerStatus ownerStatus, CharacterStatus characterStatus)
		{
			base.Initialize(fightStatus, ownerStatus, characterStatus);
			base.life = characterStatus.life;
			actionValue = characterStatus.actionValue;
			base.armor = characterStatus.armor;
			base.baseLife = characterStatus.baseLife;
			actionType = characterStatus.actionType;
			hasRange = characterStatus.hasRange;
			movementPoints = characterStatus.movementPoints;
			baseMovementPoints = characterStatus.movementPoints;
			physicalDamageBoost = characterStatus.physicalDamageBoost;
			physicalHealBoost = characterStatus.physicalHealBoost;
			m_context.SetParameterValue("life", base.life);
			TimelineContextUtility.SetFightContext(m_playableDirector, fightStatus.context);
			m_base.InitializeState(fightStatus, characterStatus, ownerStatus);
			m_lifeUI.set_enabled(false);
			m_lifeUI.Setup(characterStatus.life);
			m_lifeUI.SetValues(characterStatus.life, characterStatus.armor);
			m_actionUI.set_enabled(false);
			m_actionUI.Setup(characterStatus.actionType, characterStatus.hasRange);
			if (actionValue.HasValue)
			{
				m_actionUI.SetValue(actionValue.Value);
			}
			m_elementaryStateUI.Setup();
		}

		public override void SetColorModifier(Color value)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			base.SetColorModifier(value);
			m_lifeUI.color = value;
			m_actionUI.color = value;
			m_elementaryStateUI.color = value;
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			m_context = new FightCharacterObjectContext(this);
			m_context.Initialize();
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			m_context.Release();
		}

		protected override void ClearAttachedEffects()
		{
			base.ClearAttachedEffects();
			ClearFloatingCounterEffect();
		}

		protected override void DestroyAttachedEffects()
		{
			base.DestroyAttachedEffects();
			ClearFloatingCounterEffect();
		}

		public override void SetArmoredLife(int lifeValue, int armorValue)
		{
			base.SetArmoredLife(lifeValue, armorValue);
			m_lifeUI.ChangeValues(lifeValue, armorValue);
			m_context.SetParameterValue("life", base.life);
		}

		public override void SetBaseLife(int lifeValue)
		{
			base.SetBaseLife(lifeValue);
			m_lifeUI.SetMaximumLife(lifeValue);
		}

		public void SetMovementPoints(int value)
		{
			movementPoints = value;
		}

		public IEnumerator Move(Vector2Int[] movementCells)
		{
			yield return MoveToRoutine(movementCells);
			PlayIdleAnimation();
		}

		public IEnumerator MoveToAction(Vector2Int[] movementCells, Direction actionDirection, bool hasFollowUpAnimation = true)
		{
			yield return m_characterData.hasDashAnimations ? MoveToDoActionRoutine(movementCells, actionDirection) : MoveToRoutine(movementCells);
			if (!hasFollowUpAnimation)
			{
				PlayIdleAnimation();
			}
		}

		protected unsafe IEnumerator MoveToRoutine(Vector2Int[] movementCells)
		{
			int movementCellsCount = movementCells.Length;
			if (movementCellsCount == 0)
			{
				yield break;
			}
			CellObject cellObj = m_cellObject;
			IMap parentMap = cellObj.parentMap;
			Animator2D animator = m_animator2D;
			AnimatedFightCharacterData.IdleToRunTransitionMode idleToRunTransitionMode = m_characterData.idleToRunTransitionMode;
			Vector2Int startCell = movementCells[0];
			Vector2Int endCell = movementCells[movementCellsCount - 1];
			if (cellObj.coords != startCell)
			{
				Log.Warning($"Was not on the start cell of a new movement sequence: {cellObj.coords} instead of {startCell} ({this.get_gameObject().get_name()}).", 232, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\Objects\\FightCharacterObject.cs");
				CellObject cellObject = parentMap.GetCellObject(startCell.get_x(), startCell.get_y());
				SetCellObject(cellObject);
			}
			if (idleToRunTransitionMode.HasFlag(AnimatedFightCharacterData.IdleToRunTransitionMode.IdleToRun))
			{
				Direction direction = (movementCellsCount >= 2) ? startCell.GetDirectionTo(movementCells[1]) : this.direction;
				CharacterAnimationInfo transitionAnimationInfo2 = new CharacterAnimationInfo(Vector2Int.op_Implicit(startCell), "idle_run", "idle-to-run", loops: false, direction, m_mapRotation);
				StartFightAnimation(transitionAnimationInfo2);
				while (!CharacterObjectUtility.HasAnimationEnded(animator, transitionAnimationInfo2))
				{
					yield return null;
				}
			}
			Vector2Int val = startCell;
			float cellTraversalDuration = ((movementCellsCount <= 2) ? 5f : 4f) / (float)animator.get_frameRate();
			foreach (CharacterAnimationInfo item in CharacterFightMovementSequencer.ComputeMovement(movementCells, m_mapRotation))
			{
				Vector2Int cellCoords = item.position.RoundToInt();
				CellObject movementCell = parentMap.GetCellObject(cellCoords.get_x(), cellCoords.get_y());
				bool goingUp = ((IntPtr)(void*)movementCell.get_transform().get_position()).y >= ((IntPtr)(void*)cellObj.get_transform().get_position()).y;
				Vector2 innerPositionStart;
				Vector2 innerPositionEnd;
				if (goingUp)
				{
					SetCellObject(movementCell);
					innerPositionStart = Vector2Int.op_Implicit(val - cellCoords);
					innerPositionEnd = Vector2.get_zero();
				}
				else
				{
					innerPositionStart = Vector2.get_zero();
					innerPositionEnd = Vector2Int.op_Implicit(cellCoords - val);
				}
				StartFightAnimation(item, null, null, restart: false);
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
				val = cellCoords;
				cellObj = movementCell;
				if (cellCoords != endCell && movementCell.TryGetIsoObject(out IObjectWithActivation isoObject))
				{
					isoObject.PlayDetectionAnimation();
				}
				cellCoords = default(Vector2Int);
			}
			if (idleToRunTransitionMode.HasFlag(AnimatedFightCharacterData.IdleToRunTransitionMode.RunToIdle))
			{
				CharacterAnimationInfo transitionAnimationInfo2 = new CharacterAnimationInfo(Vector2Int.op_Implicit(val), "run_idle", "run-to-idle", loops: false, this.direction, m_mapRotation);
				StartFightAnimation(transitionAnimationInfo2);
				while (!CharacterObjectUtility.HasAnimationEnded(animator, transitionAnimationInfo2))
				{
					yield return null;
				}
			}
		}

		protected IEnumerator MoveToDoActionRoutine(Vector2Int[] movementCells, Direction actionDirection)
		{
			int movementCellsCount = movementCells.Length;
			if (movementCellsCount != 0)
			{
				CellObject cellObject = m_cellObject;
				IMap parentMap = cellObject.parentMap;
				Vector2Int val = movementCells[0];
				if (cellObject.coords != val)
				{
					Log.Warning($"Was not on the start cell of a new movement sequence: {cellObject.coords} instead of {val} ({this.get_gameObject().get_name()}).", 341, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\Objects\\FightCharacterObject.cs");
					CellObject cellObject2 = parentMap.GetCellObject(val.get_x(), val.get_y());
					SetCellObject(cellObject2);
				}
				Animator2D animator = m_animator2D;
				foreach (CharacterAnimationInfo item in CharacterFightMovementSequencer.ComputeMovementToAction(movementCells, actionDirection, m_mapRotation))
				{
					CharacterAnimationInfo sequenceItem = item;
					SetPosition(parentMap, sequenceItem.position);
					StartFightAnimation(sequenceItem);
					while (!CharacterObjectUtility.HasAnimationEnded(animator, sequenceItem))
					{
						yield return null;
					}
				}
				Vector2Int val2 = movementCells[movementCellsCount - 1];
				SetPosition(parentMap, Vector2Int.op_Implicit(val2));
				if (m_cellObject.TryGetIsoObject(out IObjectWithActivation _))
				{
					direction = actionDirection;
					PlayIdleAnimation();
				}
			}
		}

		public override void ShowSpellTargetFeedback(bool isSelected)
		{
			base.ShowSpellTargetFeedback(isSelected);
			m_lifeUI.set_enabled(true);
			m_actionUI.set_enabled(true);
		}

		public override void HideSpellTargetFeedback()
		{
			base.HideSpellTargetFeedback();
			bool isFocused = m_isFocused;
			m_lifeUI.set_enabled(isFocused);
			m_actionUI.set_enabled(isFocused);
		}

		public override void ShowActionTargetFeedback(ActionType sourceActionType, bool isSelected)
		{
			base.ShowActionTargetFeedback(sourceActionType, isSelected);
			m_lifeUI.set_enabled(true);
			m_actionUI.set_enabled(true);
		}

		public override void HideActionTargetFeedback()
		{
			base.HideActionTargetFeedback();
			bool isFocused = m_isFocused;
			m_lifeUI.set_enabled(isFocused);
			m_actionUI.set_enabled(isFocused);
		}

		public void SetPhysicalDamageBoost(int value)
		{
			physicalDamageBoost = value;
			if (actionValue.HasValue)
			{
				m_actionUI.ChangeValue(actionValue.Value + value);
			}
		}

		public void SetPhysicalHealBoost(int value)
		{
			physicalHealBoost = value;
			if (actionValue.HasValue)
			{
				m_actionUI.ChangeValue(actionValue.Value + value);
			}
		}

		public void SetActionUsed(bool actionUsed, bool turnEnded)
		{
			if (turnEnded)
			{
				m_base.SetState(CharacterBase.State.NotPlayable);
			}
			else
			{
				m_base.SetState(actionUsed ? CharacterBase.State.ActionUsed : CharacterBase.State.ActionAvailable);
			}
		}

		public IEnumerator PlayActionAnimation(Direction directionToAttack, bool waitForAnimationEndOnMissingLabel)
		{
			CharacterAnimationInfo attackAnimationInfo = new CharacterAnimationInfo(Vector2Int.op_Implicit(m_cellObject.coords), "attack", "attack", loops: false, directionToAttack, m_mapRotation);
			StartFightAnimation(attackAnimationInfo, (Action)((CharacterObject)this).PlayIdleAnimation, (Action)null, restart: true, async: false);
			Animator2D animator = m_animator2D;
			int num = default(int);
			if (animator.CurrentAnimationHasLabel("shot", ref num))
			{
				while (!CharacterObjectUtility.HasAnimationReachedLabel(animator, attackAnimationInfo, "shot"))
				{
					yield return null;
				}
				yield break;
			}
			Log.Warning(animator.GetDefinition().get_name() + " is missing the 'shot' label in the animation named '" + attackAnimationInfo.animationName + "'.", 475, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\Objects\\FightCharacterObject.cs");
			if (waitForAnimationEndOnMissingLabel)
			{
				while (!CharacterObjectUtility.HasAnimationEnded(animator, attackAnimationInfo))
				{
					yield return null;
				}
			}
		}

		public IEnumerator PlayRangedActionAnimation(Direction directionToAttack)
		{
			if (!m_characterData.hasRangedAttackAnimations)
			{
				yield return PlayActionAnimation(directionToAttack, waitForAnimationEndOnMissingLabel: true);
				yield break;
			}
			CharacterAnimationInfo rangedAttackAnimationInfo = new CharacterAnimationInfo(Vector2Int.op_Implicit(m_cellObject.coords), "rangedattack", "rangedattack", loops: false, directionToAttack, m_mapRotation);
			StartFightAnimation(rangedAttackAnimationInfo, (Action)((CharacterObject)this).PlayIdleAnimation, (Action)null, restart: true, async: false);
			Animator2D animator = m_animator2D;
			int num = default(int);
			if (animator.CurrentAnimationHasLabel("shot", ref num))
			{
				while (!CharacterObjectUtility.HasAnimationReachedLabel(animator, rangedAttackAnimationInfo, "shot"))
				{
					yield return null;
				}
			}
			else
			{
				Log.Warning(animator.GetDefinition().get_name() + " is missing the 'shot' label in the animation named '" + rangedAttackAnimationInfo.animationName + "'.", 512, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\Objects\\FightCharacterObject.cs");
			}
		}

		public void TriggerActionEffect(Vector2Int target)
		{
			CharacterEffect actionEffect = m_characterData.actionEffect;
			if (!(null == actionEffect))
			{
				CellObject cellObject = m_cellObject.parentMap.GetCellObject(target.get_x(), target.get_y());
				Component val = actionEffect.Instantiate(cellObject.get_transform(), this);
				if (null != val)
				{
					this.StartCoroutine(actionEffect.DestroyWhenFinished(val));
				}
			}
		}

		protected override void FocusCharacter()
		{
			base.FocusCharacter();
			m_lifeUI.sortingOrder = 1;
			m_lifeUI.set_enabled(true);
			m_actionUI.sortingOrder = 1;
			m_actionUI.set_enabled(true);
			m_elementaryStateUI.sortingOrder = 1;
		}

		protected override void UnFocusCharacter()
		{
			base.UnFocusCharacter();
			bool forceDisplayUI = m_forceDisplayUI;
			m_lifeUI.sortingOrder = 0;
			m_lifeUI.set_enabled(forceDisplayUI);
			m_actionUI.sortingOrder = 0;
			m_actionUI.set_enabled(forceDisplayUI);
			m_elementaryStateUI.sortingOrder = 0;
		}

		public void SetElementaryState(ElementaryStates value)
		{
			elementaryState = value;
			m_elementaryStateUI.ChangeValue(value);
		}

		public override ITimelineContext GetTimelineContext()
		{
			return m_context;
		}

		public ActionType GetActionType()
		{
			return actionType;
		}

		public TooltipActionIcon GetActionIcon()
		{
			return TooltipWindowUtility.GetActionIcon(actionType, hasRange);
		}

		public bool TryGetActionValue(out int value)
		{
			value = (actionValue ?? 0);
			return actionValue.HasValue;
		}

		public int GetLifeValue()
		{
			return base.life;
		}

		public int GetMovementValue()
		{
			return movementPoints;
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
