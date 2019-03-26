using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public abstract class DamageEffectDefinition : EffectExecutionDefinition
	{
		protected DynamicValue m_value;

		public DynamicValue value => m_value;

		public override string ToString()
		{
			return string.Format("Damage {0} on {1}{2}", m_value, m_executionTargetSelector, (m_condition == null) ? "" : $" if {m_condition}");
		}

		public new static DamageEffectDefinition FromJsonToken(JToken token)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if ((int)token.get_Type() != 1)
			{
				Debug.LogWarning((object)("Malformed token : type Object expected, but " + token.get_Type() + " found"));
				return null;
			}
			JObject val = Extensions.Value<JObject>((IEnumerable<JToken>)token);
			JToken val2 = default(JToken);
			if (!val.TryGetValue("type", ref val2))
			{
				Debug.LogWarning((object)"Malformed json: no 'type' property in object of class DamageEffectDefinition");
				return null;
			}
			string text = Extensions.Value<string>((IEnumerable<JToken>)val2);
			DamageEffectDefinition damageEffectDefinition;
			switch (text)
			{
			case "PhysicalDamageEffectDefinition":
				damageEffectDefinition = new PhysicalDamageEffectDefinition();
				break;
			case "MagicalDamageEffectDefinition":
				damageEffectDefinition = new MagicalDamageEffectDefinition();
				break;
			case "LifeLeechEffectDefinition":
				damageEffectDefinition = new LifeLeechEffectDefinition();
				break;
			default:
				Debug.LogWarning((object)("Unknown type: " + text));
				return null;
			}
			damageEffectDefinition.PopulateFromJson(val);
			return damageEffectDefinition;
		}

		public static DamageEffectDefinition FromJsonProperty(JObject jsonObject, string propertyName, DamageEffectDefinition defaultValue = null)
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
		}
	}
}
