using Ankama.Cube.Fight;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.UI.Fight;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	[RelatedToEvents(new EventCategory[]
	{
		EventCategory.ElementPointsChanged
	})]
	public sealed class ElementPointsCost : Cost
	{
		private CaracId m_element;

		private DynamicValue m_value;

		public CaracId element => m_element;

		public DynamicValue value => m_value;

		public override string ToString()
		{
			return $"{value} {element}";
		}

		public new static ElementPointsCost FromJsonToken(JToken token)
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
			ElementPointsCost elementPointsCost = new ElementPointsCost();
			elementPointsCost.PopulateFromJson(jsonObject);
			return elementPointsCost;
		}

		public static ElementPointsCost FromJsonProperty(JObject jsonObject, string propertyName, ElementPointsCost defaultValue = null)
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
			m_element = (CaracId)Serialization.JsonTokenValue<int>(jsonObject, "element", 0);
			m_value = DynamicValue.FromJsonProperty(jsonObject, "value");
		}

		protected override CastValidity InternalCheckValidity(PlayerStatus status, DynamicValueContext castTargetContext)
		{
			this.value.GetValue(castTargetContext, out int value);
			if (status.GetCarac(element) >= value)
			{
				return CastValidity.SUCCESS;
			}
			return CastValidity.NOT_ENOUGH_ELEMENT_POINTS;
		}

		public override void UpdateModifications(ref GaugesModification modifications, PlayerStatus player, DynamicValueContext context)
		{
			this.value.GetValue(context, out int value);
			modifications.Increment(element, -value);
		}
	}
}
