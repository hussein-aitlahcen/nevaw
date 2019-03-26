using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class ClampValue : DynamicValue
	{
		private DynamicValue m_valueToClamp;

		private DynamicValue m_min;

		private DynamicValue m_max;

		public DynamicValue valueToClamp => m_valueToClamp;

		public DynamicValue min => m_min;

		public DynamicValue max => m_max;

		public override string ToString()
		{
			return GetType().Name;
		}

		public new static ClampValue FromJsonToken(JToken token)
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
			ClampValue clampValue = new ClampValue();
			clampValue.PopulateFromJson(jsonObject);
			return clampValue;
		}

		public static ClampValue FromJsonProperty(JObject jsonObject, string propertyName, ClampValue defaultValue = null)
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
			m_valueToClamp = DynamicValue.FromJsonProperty(jsonObject, "valueToClamp");
			m_min = DynamicValue.FromJsonProperty(jsonObject, "min");
			m_max = DynamicValue.FromJsonProperty(jsonObject, "max");
		}

		public override bool GetValue(DynamicValueContext context, out int value)
		{
			m_valueToClamp.GetValue(context, out int value2);
			if (m_min != null)
			{
				m_min.GetValue(context, out int value3);
				value2 = Math.Max(value2, value3);
			}
			if (m_max != null)
			{
				m_max.GetValue(context, out int value4);
				value2 = Math.Min(value2, value4);
			}
			value = value2;
			return true;
		}
	}
}
