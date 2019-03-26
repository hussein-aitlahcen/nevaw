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
	public sealed class ReservePointsCost : Cost
	{
		private DynamicValue m_value;

		public DynamicValue value => m_value;

		public override string ToString()
		{
			return $"{value} Reserve";
		}

		public new static ReservePointsCost FromJsonToken(JToken token)
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
			ReservePointsCost reservePointsCost = new ReservePointsCost();
			reservePointsCost.PopulateFromJson(jsonObject);
			return reservePointsCost;
		}

		public static ReservePointsCost FromJsonProperty(JObject jsonObject, string propertyName, ReservePointsCost defaultValue = null)
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

		protected override CastValidity InternalCheckValidity(PlayerStatus status, DynamicValueContext castTargetContext)
		{
			this.value.GetValue(castTargetContext, out int value);
			if (status.GetCarac(CaracId.ReservePoints) >= value)
			{
				return CastValidity.SUCCESS;
			}
			return CastValidity.NOT_ENOUGH_RESERVE_POINTS;
		}

		public override void UpdateModifications(ref GaugesModification modifications, PlayerStatus player, DynamicValueContext context)
		{
			this.value.GetValue(context, out int value);
			modifications.Increment(CaracId.ReservePoints, -value);
		}
	}
}
