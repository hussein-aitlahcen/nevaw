using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class ConstIntLevelBasedDynamicValue : LevelBasedDynamicValue, ILevelOnlyDependant, IEditableContent, IReferenceableContent
	{
		private string m_referenceId;

		private List<int> m_values;

		public string referenceId => m_referenceId;

		public IReadOnlyList<int> values => m_values;

		public override string ToString()
		{
			int count = m_values.Count;
			if (count == 0)
			{
				return "No values....";
			}
			return $"{m_values[0]} .. {m_values[count - 1]}";
		}

		public new static ConstIntLevelBasedDynamicValue FromJsonToken(JToken token)
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
			ConstIntLevelBasedDynamicValue constIntLevelBasedDynamicValue = new ConstIntLevelBasedDynamicValue();
			constIntLevelBasedDynamicValue.PopulateFromJson(jsonObject);
			return constIntLevelBasedDynamicValue;
		}

		public static ConstIntLevelBasedDynamicValue FromJsonProperty(JObject jsonObject, string propertyName, ConstIntLevelBasedDynamicValue defaultValue = null)
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
			m_referenceId = Serialization.JsonTokenValue<string>(jsonObject, "referenceId", "");
			m_values = Serialization.JsonArrayAsList<int>(jsonObject, "values");
		}

		public override bool GetValue(DynamicValueContext context, out int value)
		{
			int level = context?.level ?? 1;
			value = GetValue(level);
			return true;
		}

		public int GetValueWithLevel(int level)
		{
			return GetValue(level);
		}

		private int GetValue(int level)
		{
			return m_values[level - 1];
		}
	}
}
