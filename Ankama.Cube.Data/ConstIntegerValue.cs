using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class ConstIntegerValue : DynamicValue, ILevelOnlyDependant, IEditableContent, IReferenceableContent
	{
		private string m_referenceId;

		private int m_value;

		public string referenceId => m_referenceId;

		public int value => m_value;

		public override string ToString()
		{
			return $"{value}";
		}

		public new static ConstIntegerValue FromJsonToken(JToken token)
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
			ConstIntegerValue constIntegerValue = new ConstIntegerValue();
			constIntegerValue.PopulateFromJson(jsonObject);
			return constIntegerValue;
		}

		public static ConstIntegerValue FromJsonProperty(JObject jsonObject, string propertyName, ConstIntegerValue defaultValue = null)
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
			m_value = Serialization.JsonTokenValue<int>(jsonObject, "value", 0);
		}

		public override bool GetValue(DynamicValueContext context, out int val)
		{
			val = m_value;
			return true;
		}

		public int GetValueWithLevel(int level)
		{
			return m_value;
		}
	}
}
