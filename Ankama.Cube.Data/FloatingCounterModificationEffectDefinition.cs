using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class FloatingCounterModificationEffectDefinition : EffectExecutionWithDurationDefinition
	{
		private CaracId m_counterType;

		private ValueModifier m_modifier;

		private DynamicValue m_value;

		private ISingleEntitySelector m_source;

		private bool m_canAddWithoutPriorSet;

		private int? m_maximumValue;

		public CaracId counterType => m_counterType;

		public ValueModifier modifier => m_modifier;

		public DynamicValue value => m_value;

		public ISingleEntitySelector source => m_source;

		public bool canAddWithoutPriorSet => m_canAddWithoutPriorSet;

		public int? maximumValue => m_maximumValue;

		public override string ToString()
		{
			switch (modifier)
			{
			case ValueModifier.Set:
				return string.Format("{0}: Initialized to {1}{2}", m_counterType, m_value, (m_condition == null) ? "" : $" if {m_condition}");
			case ValueModifier.Add:
				return string.Format("{0}: Add {1}{2}", m_counterType, m_value, (m_condition == null) ? "" : $" if {m_condition}");
			default:
				return $"operator unknow: {modifier} ";
			}
		}

		public new static FloatingCounterModificationEffectDefinition FromJsonToken(JToken token)
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
			FloatingCounterModificationEffectDefinition floatingCounterModificationEffectDefinition = new FloatingCounterModificationEffectDefinition();
			floatingCounterModificationEffectDefinition.PopulateFromJson(jsonObject);
			return floatingCounterModificationEffectDefinition;
		}

		public static FloatingCounterModificationEffectDefinition FromJsonProperty(JObject jsonObject, string propertyName, FloatingCounterModificationEffectDefinition defaultValue = null)
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
			m_counterType = (CaracId)Serialization.JsonTokenValue<int>(jsonObject, "counterType", 20);
			m_modifier = (ValueModifier)Serialization.JsonTokenValue<int>(jsonObject, "modifier", 1);
			m_value = DynamicValue.FromJsonProperty(jsonObject, "value");
			m_source = ISingleEntitySelectorUtils.FromJsonProperty(jsonObject, "source");
			m_canAddWithoutPriorSet = Serialization.JsonTokenValue<bool>(jsonObject, "canAddWithoutPriorSet", false);
			m_maximumValue = Serialization.JsonTokenValue<int?>(jsonObject, "maximumValue", (int?)null);
		}
	}
}
