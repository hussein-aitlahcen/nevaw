using Ankama.Cube.Data;
using Ankama.Cube.Fight;
using Ankama.Cube.Fight.CostModifiers;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps;
using Ankama.Cube.Protocols.FightCommonProtocol;
using Ankama.Cube.UI.DeckMaker;
using System.Collections.Generic;
using System.Linq;

namespace Ankama.Cube.UI.Fight
{
	public static class CastValidityHelper
	{
		private static bool HasCompanionValidTargets(PlayerStatus casterStatus, ReserveCompanionStatus companionStatus)
		{
			CompanionDefinition definition = companionStatus.definition;
			if (null == definition)
			{
				return false;
			}
			OneCastTargetContext context = new OneCastTargetContext(FightStatus.local, casterStatus.id, DynamicValueHolderType.Companion, definition.get_id(), companionStatus.level, 0);
			FightMap current = FightMap.current;
			if (null == current)
			{
				return false;
			}
			return definition.spawnLocation?.EnumerateCoords(context).GetEnumerator().MoveNext() ?? false;
		}

		private static bool HasSpellValidTargets(PlayerStatus casterStatus, SpellStatus spellStatus)
		{
			SpellDefinition definition = spellStatus.definition;
			if (null == definition)
			{
				return false;
			}
			ICastTargetDefinition castTarget = definition.castTarget;
			if (castTarget == null)
			{
				return false;
			}
			CastTargetContext castTargetContext = castTarget.CreateCastTargetContext(FightStatus.local, casterStatus.id, DynamicValueHolderType.Spell, definition.get_id(), spellStatus.level, spellStatus.instanceId);
			return castTarget.EnumerateTargets(castTargetContext).GetEnumerator().MoveNext();
		}

		private static CastValidity ComputeCastValidity(ICastableStatus status)
		{
			PrecomputedData precomputedData = status.GetDefinition()?.precomputedData;
			FightStatus local = FightStatus.local;
			if (precomputedData != null && local != null)
			{
				PlayerStatus player = local.GetLocalPlayer();
				HeroStatus heroStatus = player.heroStatus;
				WeaponDefinition weaponDefinition = (WeaponDefinition)heroStatus.definition;
				if (precomputedData.checkNumberOfSummonings)
				{
					int num = local.EnumerateEntities((SummoningStatus s) => s.ownerId == player.id).Count();
					int valueWithLevel = weaponDefinition.maxSummoningsOnBoard.GetValueWithLevel(heroStatus.level);
					if (num >= valueWithLevel)
					{
						return CastValidity.TOO_MANY_SUMMONING;
					}
				}
				if (precomputedData.checkNumberOfMechanisms)
				{
					int num2 = local.EnumerateEntities((MechanismStatus s) => s.ownerId == player.id).Count();
					int valueWithLevel2 = weaponDefinition.maxMechanismsOnBoard.GetValueWithLevel(heroStatus.level);
					if (num2 >= valueWithLevel2)
					{
						return CastValidity.TOO_MANY_MECHANISM;
					}
				}
			}
			return CastValidity.SUCCESS;
		}

		public static CastValidity ComputeSpellCostCastValidity(PlayerStatus owner, SpellStatus spellStatus)
		{
			if (owner.HasProperty(PropertyId.PlaySpellForbidden))
			{
				return CastValidity.NOT_ALLOW_TO_PLAY_SPELLS;
			}
			CastTargetContext castTargetContext = spellStatus.CreateCastTargetContext();
			IReadOnlyList<Cost> costs = spellStatus.definition.costs;
			for (int i = 0; i < costs.Count; i++)
			{
				CastValidity castValidity = costs[i].CheckValidity(owner, castTargetContext);
				if (castValidity != 0)
				{
					return castValidity;
				}
			}
			return CastValidity.SUCCESS;
		}

		public static CastValidity ComputeCompanionCostCastValidity(PlayerStatus owner, ReserveCompanionStatus companionStatus)
		{
			DynamicValueFightContext castTargetContext = companionStatus.CreateValueContext();
			if (companionStatus.state == CompanionReserveState.Idle && companionStatus.isGiven)
			{
				return CastValidity.SUCCESS;
			}
			IReadOnlyList<Cost> cost = companionStatus.definition.cost;
			int i = 0;
			for (int count = cost.Count; i < count; i++)
			{
				CastValidity castValidity = cost[i].CheckValidity(owner, castTargetContext);
				if (castValidity != 0)
				{
					return castValidity;
				}
			}
			return CastValidity.SUCCESS;
		}

		public static CastValidity ComputeSpellCastValidity(PlayerStatus playerStatus, SpellStatus spellStatus)
		{
			CastValidity castValidity = ComputeCastValidity(spellStatus);
			if (castValidity != 0)
			{
				return castValidity;
			}
			if (!HasSpellValidTargets(playerStatus, spellStatus))
			{
				return CastValidity.NO_TARGET_AVAILABLE;
			}
			return CastValidity.SUCCESS;
		}

		public static CastValidity ComputeCompanionCastValidity(PlayerStatus owner, ReserveCompanionStatus companionStatus)
		{
			CastValidity castValidity = ComputeCastValidity(companionStatus);
			if (castValidity != 0)
			{
				return castValidity;
			}
			if (!HasCompanionValidTargets(owner, companionStatus))
			{
				return CastValidity.NO_TARGET_AVAILABLE;
			}
			return CastValidity.SUCCESS;
		}

		public static void RecomputeCompanionCastValidity(PlayerStatus owner, ReserveCompanionStatus status, ref CompanionStatusData data)
		{
			data.hasResources = (ComputeCompanionCostCastValidity(owner, status) == CastValidity.SUCCESS);
		}

		public static void RecomputeSpellCastValidity(PlayerStatus owner, SpellStatus spellStatus, ref SpellStatusData data)
		{
			data.hasEnoughAp = (ComputeSpellCostCastValidity(owner, spellStatus) == CastValidity.SUCCESS);
		}

		public static void RecomputeCompanionCost(ReserveCompanionStatus status, ref CompanionStatusData data)
		{
			data.cost = (status.isGiven ? null : status.definition.cost);
		}

		public static void RecomputeSpellCost(SpellStatus spellStatus, ref SpellStatusData data)
		{
			if (spellStatus.ownerPlayer != null)
			{
				int? baseCost = spellStatus.baseCost;
				if (!baseCost.HasValue)
				{
					data.apCost = null;
					data.baseCost = null;
					return;
				}
				SpellDefinition definition = spellStatus.definition;
				CastTargetContext context = spellStatus.CreateCastTargetContext();
				int cost = spellStatus.definition.GetCost(context) ?? 0;
				data.apCost = SpellCostModification.ApplyCostModification(spellStatus.ownerPlayer.spellCostModifiers, cost, definition, context);
				data.baseCost = baseCost.Value;
			}
		}
	}
}
