using Ankama.Cube.Fight;
using Ankama.Cube.Fight.CostModifiers;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.UI.Fight;
using Ankama.Utilities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	[RelatedToEvents(new EventCategory[]
	{
		EventCategory.ActionPointsChanged
	})]
	public sealed class ActionPointsCost : Cost
	{
		private DynamicValue m_value;

		public DynamicValue value => m_value;

		public override string ToString()
		{
			return $"{value} AP";
		}

		public new static ActionPointsCost FromJsonToken(JToken token)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if ((int)token.get_Type() != 1)
			{
				Debug.LogWarning((object)("Malformed token : type Object expected, but " + token.get_Type() + " found"));
				return null;
			}
			JObject jsonObject = Extensions.Value<JObject>((IEnumerable<JToken>)token);
			ActionPointsCost actionPointsCost = new ActionPointsCost();
			actionPointsCost.PopulateFromJson(jsonObject);
			return actionPointsCost;
		}

		public static ActionPointsCost FromJsonProperty(JObject jsonObject, string propertyName, ActionPointsCost defaultValue = null)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Invalid comparison between Unknown and I4
			JProperty val = jsonObject.Property(propertyName);
			if (val == null || (int)val.get_Value().get_Type() == 10)
			{
				return defaultValue;
			}
			return FromJsonToken(val.get_Value());
		}

		public override void PopulateFromJson(JObject jsonObject)
		{
			base.PopulateFromJson(jsonObject);
			m_value = DynamicValue.FromJsonProperty(jsonObject, "value");
		}

		protected override CastValidity InternalCheckValidity(PlayerStatus status, DynamicValueContext context)
		{
			this.value.GetValue(context, out int value);
			switch (context.type)
			{
			case DynamicValueHolderType.Spell:
				return CheckForSpell(status, context, value);
			case DynamicValueHolderType.Companion:
				return CheckForCompanion(status, value);
			case DynamicValueHolderType.CharacterAction:
				return CastValidity.SUCCESS;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		private static CastValidity CheckForSpell(PlayerStatus playerStatus, DynamicValueContext context, int baseCost)
		{
			CastTargetContext castTargetContext = context as CastTargetContext;
			if (castTargetContext == null)
			{
				return CastValidity.DATA_ERROR;
			}
			if (!RuntimeData.spellDefinitions.TryGetValue(castTargetContext.spellDefinitionId, out SpellDefinition value))
			{
				Log.Error($"Could not find spell definition with id {castTargetContext.spellDefinitionId}.", 44, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\Costs\\ActionPointsCost.cs");
				return CastValidity.DATA_ERROR;
			}
			int num = SpellCostModification.ApplyCostModification(playerStatus.spellCostModifiers, baseCost, value, castTargetContext);
			if (playerStatus.actionPoints < num)
			{
				return CastValidity.NOT_ENOUGH_ACTION_POINTS;
			}
			return CastValidity.SUCCESS;
		}

		private static CastValidity CheckForCompanion(PlayerStatus status, int baseCost)
		{
			if (status.GetCarac(CaracId.ActionPoints) < baseCost)
			{
				return CastValidity.NOT_ENOUGH_ACTION_POINTS;
			}
			return CastValidity.SUCCESS;
		}

		public override void UpdateModifications(ref GaugesModification modifications, PlayerStatus player, DynamicValueContext context)
		{
			this.value.GetValue(context, out int value);
			CastTargetContext castTargetContext = context as CastTargetContext;
			if (castTargetContext != null)
			{
				if (!RuntimeData.spellDefinitions.TryGetValue(castTargetContext.spellDefinitionId, out SpellDefinition value2))
				{
					Log.Error($"Could not find spell definition with id {castTargetContext.spellDefinitionId}.", 69, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\Costs\\ActionPointsCost.cs");
					return;
				}
				value = SpellCostModification.ApplyCostModification(player.spellCostModifiers, value, value2, castTargetContext);
			}
			modifications.Increment(CaracId.ActionPoints, -value);
		}
	}
}
