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
	public sealed class GaugeValue : IEditableContent
	{
		private CaracId m_element;

		private DynamicValue m_value;

		public CaracId element => m_element;

		public DynamicValue value => m_value;

		public override string ToString()
		{
			return $"{m_value} {m_element}";
		}

		public static GaugeValue FromJsonToken(JToken token)
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
			GaugeValue gaugeValue = new GaugeValue();
			gaugeValue.PopulateFromJson(jsonObject);
			return gaugeValue;
		}

		public static GaugeValue FromJsonProperty(JObject jsonObject, string propertyName, GaugeValue defaultValue = null)
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

		public void PopulateFromJson(JObject jsonObject)
		{
			m_element = (CaracId)Serialization.JsonTokenValue<int>(jsonObject, "element", 0);
			m_value = DynamicValue.FromJsonProperty(jsonObject, "value");
		}

		public void UpdateModifications(ref GaugesModification modifications, PlayerStatus playerStatus, DynamicValueContext context)
		{
			this.value.GetValue(context, out int value);
			modifications.Increment(element, value);
		}
	}
}
