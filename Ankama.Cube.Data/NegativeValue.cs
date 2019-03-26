using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class NegativeValue : DynamicValue
	{
		private DynamicValue m_valueToNeg;

		public DynamicValue valueToNeg => m_valueToNeg;

		public override string ToString()
		{
			return $"-{valueToNeg}";
		}

		public new static NegativeValue FromJsonToken(JToken token)
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
			NegativeValue negativeValue = new NegativeValue();
			negativeValue.PopulateFromJson(jsonObject);
			return negativeValue;
		}

		public static NegativeValue FromJsonProperty(JObject jsonObject, string propertyName, NegativeValue defaultValue = null)
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
			m_valueToNeg = DynamicValue.FromJsonProperty(jsonObject, "valueToNeg");
		}

		public override bool GetValue(DynamicValueContext context, out int value)
		{
			int value2;
			bool value3 = m_valueToNeg.GetValue(context, out value2);
			value = -value2;
			return value3;
		}
	}
}
