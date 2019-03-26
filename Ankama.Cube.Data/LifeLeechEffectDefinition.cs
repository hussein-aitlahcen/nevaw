using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class LifeLeechEffectDefinition : DamageEffectDefinition
	{
		private bool m_physicalDamage;

		private ISingleEntitySelector m_source;

		private ISingleEntitySelector m_leecher;

		public bool physicalDamage => m_physicalDamage;

		public ISingleEntitySelector source => m_source;

		public ISingleEntitySelector leecher => m_leecher;

		public override string ToString()
		{
			return string.Format("Leech life {0} on {1}{2}", m_value, m_executionTargetSelector, (m_condition == null) ? "" : $" if {m_condition}");
		}

		public new static LifeLeechEffectDefinition FromJsonToken(JToken token)
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
			LifeLeechEffectDefinition lifeLeechEffectDefinition = new LifeLeechEffectDefinition();
			lifeLeechEffectDefinition.PopulateFromJson(jsonObject);
			return lifeLeechEffectDefinition;
		}

		public static LifeLeechEffectDefinition FromJsonProperty(JObject jsonObject, string propertyName, LifeLeechEffectDefinition defaultValue = null)
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
			m_physicalDamage = Serialization.JsonTokenValue<bool>(jsonObject, "physicalDamage", false);
			m_source = ISingleEntitySelectorUtils.FromJsonProperty(jsonObject, "source");
			m_leecher = ISingleEntitySelectorUtils.FromJsonProperty(jsonObject, "leecher");
		}
	}
}
