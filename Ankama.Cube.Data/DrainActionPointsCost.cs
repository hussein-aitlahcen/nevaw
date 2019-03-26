using Ankama.Cube.Fight;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.UI.Fight;
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
	public sealed class DrainActionPointsCost : Cost
	{
		public override string ToString()
		{
			return "Consume all AP";
		}

		public new static DrainActionPointsCost FromJsonToken(JToken token)
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
			DrainActionPointsCost drainActionPointsCost = new DrainActionPointsCost();
			drainActionPointsCost.PopulateFromJson(jsonObject);
			return drainActionPointsCost;
		}

		public static DrainActionPointsCost FromJsonProperty(JObject jsonObject, string propertyName, DrainActionPointsCost defaultValue = null)
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
		}

		protected override CastValidity InternalCheckValidity(PlayerStatus status, DynamicValueContext castTargetContext)
		{
			if (status.actionPoints > 0)
			{
				return CastValidity.SUCCESS;
			}
			return CastValidity.NOT_ENOUGH_ACTION_POINTS;
		}

		public override void UpdateModifications(ref GaugesModification modifications, PlayerStatus player, DynamicValueContext context)
		{
			modifications.Increment(CaracId.ActionPoints, -player.actionPoints);
		}
	}
}
