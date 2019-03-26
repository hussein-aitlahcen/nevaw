using Ankama.Cube.Data;
using Ankama.Cube.Data.UI.Localization.TextFormatting.ValueProviders.FightValueProviders;
using Ankama.Cube.Maps.Objects;
using System.Collections.Generic;

namespace Ankama.Cube.Fight.Entities
{
	public abstract class CharacterStatus : EntityStatus, ICharacterEntity, IEntityWithOwner, IEntityWithTeam, IEntity, IEntityWithLevel, IEntityWithMovement, IEntityWithBoardPresence, IEntityWithAction, IEntityWithLife, IEntityWithFamilies, IEntityWithElementaryState, IDynamicValueSource, IEntityTargetableByAction
	{
		private ElementaryStates m_elemState;

		public CharacterDefinition definition
		{
			get;
			protected set;
		}

		public int teamId
		{
			get;
		}

		public int teamIndex
		{
			get;
		}

		public int ownerId
		{
			get;
		}

		public int baseLife => GetCarac(CaracId.LifeMax, definition.life.GetValueWithLevel(level));

		public int life => GetCarac(CaracId.Life);

		public bool hasArmor => GetCarac(CaracId.Armor) > 0;

		public int armor => GetCarac(CaracId.Armor);

		public int armoredLife => life + armor;

		public int resistance => GetCarac(CaracId.Resistance);

		public int hitLimit => GetCarac(CaracId.HitLimit);

		public int physicalDamageBoost => GetCarac(CaracId.PhysicalDamageModifier);

		public int physicalHealBoost => GetCarac(CaracId.PhysicalHealModifier);

		public bool wounded => life < baseLife;

		public Area area
		{
			get;
			protected set;
		}

		public IsoObject view
		{
			get;
			set;
		}

		public bool blocksMovement => true;

		public int baseMovementPoints => definition.movementPoints.GetValueWithLevel(level);

		public int movementPoints => GetCarac(CaracId.MovementPoints);

		public ActionType actionType
		{
			get
			{
				if (!(definition != null))
				{
					return ActionType.None;
				}
				return definition.actionType;
			}
		}

		public int? actionValue => definition.actionValue?.GetValueWithLevel(level);

		public bool hasRange => definition.actionRange != null;

		public int rangeMin => GetCarac(CaracId.RangeMin);

		public int rangeMax => GetCarac(CaracId.RangeMax);

		public IReadOnlyList<Family> families => definition.families;

		public int level
		{
			get;
		}

		public IReadOnlyList<ILevelOnlyDependant> dynamicValues => definition.precomputedData.dynamicValueReferences;

		public bool actionUsed
		{
			get;
			set;
		}

		public bool canMove
		{
			get
			{
				if (movementPoints > 0)
				{
					return !HasAnyProperty(PropertiesUtility.propertiesWhichPreventVoluntaryMove);
				}
				return false;
			}
		}

		public bool canDoActionOnTarget
		{
			get
			{
				if (definition.actionType != 0)
				{
					return !HasAnyProperty(PropertiesUtility.propertiesWhichPreventAction);
				}
				return false;
			}
		}

		public IEntitySelector customActionTarget => definition.customActionTarget;

		public void ChangeElementaryState(ElementaryStates elemState)
		{
			m_elemState = elemState;
		}

		public bool HasElementaryState(ElementaryStates elemState)
		{
			return m_elemState == elemState;
		}

		protected CharacterStatus(int id, int ownerId, int teamId, int teamIndex, int level)
			: base(id)
		{
			this.ownerId = ownerId;
			this.teamId = teamId;
			this.teamIndex = teamIndex;
			this.level = level;
		}

		protected static void InitializeStatus(CharacterStatus status, CharacterDefinition definition)
		{
			int level = status.level;
			int valueWithLevel = definition.life.GetValueWithLevel(level);
			status.SetCarac(CaracId.Life, valueWithLevel);
			int valueWithLevel2 = definition.movementPoints.GetValueWithLevel(level);
			status.SetCarac(CaracId.MovementPoints, valueWithLevel2);
			ActionRange actionRange = definition.actionRange;
			if (actionRange != null)
			{
				int valueWithLevel3 = actionRange.min.GetValueWithLevel(level);
				int valueWithLevel4 = actionRange.max.GetValueWithLevel(level);
				status.SetCarac(CaracId.RangeMin, valueWithLevel3);
				status.SetCarac(CaracId.RangeMax, valueWithLevel4);
			}
		}
	}
}
