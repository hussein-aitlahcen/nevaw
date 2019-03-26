using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class ExplosionEffectDefinition : EffectExecutionDefinition
	{
		private DynamicValue m_value;

		private ISingleEntitySelector m_initiator;

		public DynamicValue value => m_value;

		public ISingleEntitySelector initiator => m_initiator;

		public override string ToString()
		{
			return string.Format("Explosion of {0} damage around {1}{2}", m_value, m_executionTargetSelector, (m_condition == null) ? "" : $" if {m_condition}");
		}

		public new static ExplosionEffectDefinition FromJsonToken(JToken token)
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
			ExplosionEffectDefinition explosionEffectDefinition = new ExplosionEffectDefinition();
			explosionEffectDefinition.PopulateFromJson(jsonObject);
			return explosionEffectDefinition;
		}

		public static ExplosionEffectDefinition FromJsonProperty(JObject jsonObject, string propertyName, ExplosionEffectDefinition defaultValue = null)
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
			m_initiator = ISingleEntitySelectorUtils.FromJsonProperty(jsonObject, "initiator");
		}
	}
}
