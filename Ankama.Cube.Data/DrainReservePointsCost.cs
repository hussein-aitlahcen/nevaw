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
		EventCategory.ReserveChanged
	})]
	public sealed class DrainReservePointsCost : Cost
	{
		public override string ToString()
		{
			return "Consume whole reserve";
		}

		public new static DrainReservePointsCost FromJsonToken(JToken token)
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
			DrainReservePointsCost drainReservePointsCost = new DrainReservePointsCost();
			drainReservePointsCost.PopulateFromJson(jsonObject);
			return drainReservePointsCost;
		}

		public static DrainReservePointsCost FromJsonProperty(JObject jsonObject, string propertyName, DrainReservePointsCost defaultValue = null)
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
			if (status.reservePoints > 0)
			{
				return CastValidity.SUCCESS;
			}
			return CastValidity.NOT_ENOUGH_RESERVE_POINTS;
		}

		public override void UpdateModifications(ref GaugesModification modifications, PlayerStatus player, DynamicValueContext context)
		{
			modifications.Increment(CaracId.ReservePoints, -player.reservePoints);
		}
	}
}
