using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class AddSpellInGameEffectDefinition : EffectExecutionDefinition
	{
		private Id<SpellDefinition> m_spellDef;

		private SpellDestination m_destination;

		public Id<SpellDefinition> spellDef => m_spellDef;

		public SpellDestination destination => m_destination;

		public override string ToString()
		{
			return string.Format("Add spell {0} to {1}{2}", m_spellDef, m_executionTargetSelector, (m_condition == null) ? "" : $" if {m_condition}");
		}

		public new static AddSpellInGameEffectDefinition FromJsonToken(JToken token)
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
			AddSpellInGameEffectDefinition addSpellInGameEffectDefinition = new AddSpellInGameEffectDefinition();
			addSpellInGameEffectDefinition.PopulateFromJson(jsonObject);
			return addSpellInGameEffectDefinition;
		}

		public static AddSpellInGameEffectDefinition FromJsonProperty(JObject jsonObject, string propertyName, AddSpellInGameEffectDefinition defaultValue = null)
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
			m_spellDef = Serialization.JsonTokenIdValue<SpellDefinition>(jsonObject, "spellDef");
			m_destination = (SpellDestination)Serialization.JsonTokenValue<int>(jsonObject, "destination", 0);
		}
	}
}
