using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class ConditionalValue : DynamicValue
	{
		private EffectCondition m_condition;

		private DynamicValue m_ifTrue;

		private DynamicValue m_ifFalse;

		public EffectCondition condition => m_condition;

		public DynamicValue ifTrue => m_ifTrue;

		public DynamicValue ifFalse => m_ifFalse;

		public override string ToString()
		{
			return GetType().Name;
		}

		public new static ConditionalValue FromJsonToken(JToken token)
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
			ConditionalValue conditionalValue = new ConditionalValue();
			conditionalValue.PopulateFromJson(jsonObject);
			return conditionalValue;
		}

		public static ConditionalValue FromJsonProperty(JObject jsonObject, string propertyName, ConditionalValue defaultValue = null)
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
			m_condition = EffectCondition.FromJsonProperty(jsonObject, "condition");
			m_ifTrue = DynamicValue.FromJsonProperty(jsonObject, "ifTrue");
			m_ifFalse = DynamicValue.FromJsonProperty(jsonObject, "ifFalse");
		}

		public override bool GetValue(DynamicValueContext context, out int value)
		{
			if (m_condition.IsValid(context))
			{
				return m_ifTrue.GetValue(context, out value);
			}
			return m_ifFalse.GetValue(context, out value);
		}
	}
}
