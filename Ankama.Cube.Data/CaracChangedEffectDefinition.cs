using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class CaracChangedEffectDefinition : EffectExecutionDefinition
	{
		private ICaracIdSelector m_caracSelector;

		private ValueModifier m_modifier;

		private DynamicValue m_value;

		private ISingleEntitySelector m_source;

		public ICaracIdSelector caracSelector => m_caracSelector;

		public ValueModifier modifier => m_modifier;

		public DynamicValue value => m_value;

		public ISingleEntitySelector source => m_source;

		public override string ToString()
		{
			switch (modifier)
			{
			case ValueModifier.Set:
				return string.Format("Set {0} to {1} for {2}{3}", m_caracSelector, m_value, m_executionTargetSelector, (m_condition == null) ? "" : $" if {m_condition}");
			case ValueModifier.Add:
				return string.Format("Add {0} to {1} for {2}{3} ", m_value, m_caracSelector, m_executionTargetSelector, (m_condition == null) ? "" : $" if {m_condition}");
			default:
				return $"operator unknow: {m_modifier} ";
			}
		}

		public new static CaracChangedEffectDefinition FromJsonToken(JToken token)
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
			CaracChangedEffectDefinition caracChangedEffectDefinition = new CaracChangedEffectDefinition();
			caracChangedEffectDefinition.PopulateFromJson(jsonObject);
			return caracChangedEffectDefinition;
		}

		public static CaracChangedEffectDefinition FromJsonProperty(JObject jsonObject, string propertyName, CaracChangedEffectDefinition defaultValue = null)
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
			m_caracSelector = ICaracIdSelectorUtils.FromJsonProperty(jsonObject, "caracSelector");
			m_modifier = (ValueModifier)Serialization.JsonTokenValue<int>(jsonObject, "modifier", 1);
			m_value = DynamicValue.FromJsonProperty(jsonObject, "value");
			m_source = ISingleEntitySelectorUtils.FromJsonProperty(jsonObject, "source");
		}
	}
}
