using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class SumValue : DynamicValue
	{
		private List<DynamicValue> m_valuesToSum;

		public IReadOnlyList<DynamicValue> valuesToSum => m_valuesToSum;

		public override string ToString()
		{
			return string.Join(" + ", valuesToSum) ?? "";
		}

		public new static SumValue FromJsonToken(JToken token)
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
			SumValue sumValue = new SumValue();
			sumValue.PopulateFromJson(jsonObject);
			return sumValue;
		}

		public static SumValue FromJsonProperty(JObject jsonObject, string propertyName, SumValue defaultValue = null)
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
			JArray val = Serialization.JsonArray(jsonObject, "valuesToSum");
			m_valuesToSum = new List<DynamicValue>((val != null) ? val.get_Count() : 0);
			if (val != null)
			{
				foreach (JToken item in val)
				{
					m_valuesToSum.Add(DynamicValue.FromJsonToken(item));
				}
			}
		}

		public override bool GetValue(DynamicValueContext context, out int value)
		{
			int num = 0;
			bool flag = true;
			for (int i = 0; i < m_valuesToSum.Count; i++)
			{
				flag &= m_valuesToSum[i].GetValue(context, out int value2);
				num += value2;
			}
			value = num;
			return flag;
		}

		public override bool ToString(DynamicValueContext context, out string value)
		{
			int value2;
			bool value3 = GetValue(context, out value2);
			value = value2.ToString();
			return value3;
		}
	}
}
