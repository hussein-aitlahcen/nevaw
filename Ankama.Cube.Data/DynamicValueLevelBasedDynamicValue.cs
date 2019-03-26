using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class DynamicValueLevelBasedDynamicValue : LevelBasedDynamicValue
	{
		private List<DynamicValue> m_values;

		public IReadOnlyList<DynamicValue> values => m_values;

		public override string ToString()
		{
			int count = m_values.Count;
			if (count == 0)
			{
				return "No values....";
			}
			return $"{m_values[0]} .. {m_values[count - 1]}";
		}

		public new static DynamicValueLevelBasedDynamicValue FromJsonToken(JToken token)
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
			DynamicValueLevelBasedDynamicValue dynamicValueLevelBasedDynamicValue = new DynamicValueLevelBasedDynamicValue();
			dynamicValueLevelBasedDynamicValue.PopulateFromJson(jsonObject);
			return dynamicValueLevelBasedDynamicValue;
		}

		public static DynamicValueLevelBasedDynamicValue FromJsonProperty(JObject jsonObject, string propertyName, DynamicValueLevelBasedDynamicValue defaultValue = null)
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
			JArray val = Serialization.JsonArray(jsonObject, "values");
			m_values = new List<DynamicValue>((val != null) ? val.get_Count() : 0);
			if (val != null)
			{
				foreach (JToken item in val)
				{
					m_values.Add(DynamicValue.FromJsonToken(item));
				}
			}
		}

		public override bool GetValue(DynamicValueContext context, out int value)
		{
			int level = context?.level ?? 1;
			return GetDynamicValue(level).GetValue(context, out value);
		}

		public DynamicValue GetDynamicValue(int level)
		{
			return m_values[level - 1];
		}
	}
}
