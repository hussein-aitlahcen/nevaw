using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class DynamicValueEvaluatorCondition : EffectCondition
	{
		private DynamicValue m_value;

		private ValueFilter m_evaluator;

		public DynamicValue value => m_value;

		public ValueFilter evaluator => m_evaluator;

		public override string ToString()
		{
			return $"{m_value} {m_evaluator}";
		}

		public new static DynamicValueEvaluatorCondition FromJsonToken(JToken token)
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
			DynamicValueEvaluatorCondition dynamicValueEvaluatorCondition = new DynamicValueEvaluatorCondition();
			dynamicValueEvaluatorCondition.PopulateFromJson(jsonObject);
			return dynamicValueEvaluatorCondition;
		}

		public static DynamicValueEvaluatorCondition FromJsonProperty(JObject jsonObject, string propertyName, DynamicValueEvaluatorCondition defaultValue = null)
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
			m_value = DynamicValue.FromJsonProperty(jsonObject, "value");
			m_evaluator = ValueFilter.FromJsonProperty(jsonObject, "evaluator");
		}

		public override bool IsValid(DynamicValueContext context)
		{
			int value = 0;
			this.value.GetValue(context, out value);
			return evaluator.Matches(value, context);
		}
	}
}
