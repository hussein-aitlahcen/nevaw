using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class LinearLevelBasedDynamicValue : LevelBasedDynamicValue, ILevelOnlyDependant, IEditableContent, IReferenceableContent
	{
		private string m_referenceId;

		private int m_baseValue;

		private float m_factor;

		public string referenceId => m_referenceId;

		public int baseValue => m_baseValue;

		public float factor => m_factor;

		public override string ToString()
		{
			if (m_factor == 1f)
			{
				if (m_baseValue <= 0)
				{
					return $"level {m_baseValue}";
				}
				return $"level+{m_baseValue}";
			}
			if (m_baseValue <= 0)
			{
				return $"{m_factor} * level {m_baseValue}";
			}
			return $"{m_factor} * level +{m_baseValue}";
		}

		public new static LinearLevelBasedDynamicValue FromJsonToken(JToken token)
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
			LinearLevelBasedDynamicValue linearLevelBasedDynamicValue = new LinearLevelBasedDynamicValue();
			linearLevelBasedDynamicValue.PopulateFromJson(jsonObject);
			return linearLevelBasedDynamicValue;
		}

		public static LinearLevelBasedDynamicValue FromJsonProperty(JObject jsonObject, string propertyName, LinearLevelBasedDynamicValue defaultValue = null)
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
			m_baseValue = Serialization.JsonTokenValue<int>(jsonObject, "baseValue", 0);
			m_factor = Serialization.JsonTokenValue<float>(jsonObject, "factor", 0f);
		}

		public override bool GetValue(DynamicValueContext context, out int value)
		{
			int num = context?.level ?? 1;
			value = (int)Math.Round((float)baseValue + (float)num * factor);
			return true;
		}

		public int GetValueWithLevel(int level)
		{
			return (int)Math.Round((float)baseValue + (float)level * factor);
		}
	}
}
