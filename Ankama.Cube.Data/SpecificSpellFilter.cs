using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class SpecificSpellFilter : SpellFilter
	{
		private ShouldBeInOrNot m_condition;

		private List<Id<SpellDefinition>> m_spellDefinition;

		public ShouldBeInOrNot condition => m_condition;

		public IReadOnlyList<Id<SpellDefinition>> spellDefinition => m_spellDefinition;

		public override string ToString()
		{
			switch (m_spellDefinition.Count)
			{
			case 0:
				return "spellDefinition is <unset>";
			case 1:
				return $"spellDefinition {condition} {m_spellDefinition[0]}";
			default:
				return $"spellDefinition {condition} \n - " + string.Join("\n - ", m_spellDefinition);
			}
		}

		public new static SpecificSpellFilter FromJsonToken(JToken token)
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
			SpecificSpellFilter specificSpellFilter = new SpecificSpellFilter();
			specificSpellFilter.PopulateFromJson(jsonObject);
			return specificSpellFilter;
		}

		public static SpecificSpellFilter FromJsonProperty(JObject jsonObject, string propertyName, SpecificSpellFilter defaultValue = null)
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
			m_condition = (ShouldBeInOrNot)Serialization.JsonTokenValue<int>(jsonObject, "condition", 1);
			JArray val = Serialization.JsonArray(jsonObject, "spellDefinition");
			m_spellDefinition = new List<Id<SpellDefinition>>((val != null) ? val.get_Count() : 0);
			if (val != null)
			{
				foreach (JToken item2 in val)
				{
					Id<SpellDefinition> item = Serialization.JsonTokenIdValue<SpellDefinition>(item2);
					m_spellDefinition.Add(item);
				}
			}
		}

		public override bool Accept(int spellInstanceId, SpellDefinition spellDef)
		{
			int id = spellDef.get_id();
			for (int i = 0; i < m_spellDefinition.Count; i++)
			{
				if (m_spellDefinition[i].value == id)
				{
					return m_condition == ShouldBeInOrNot.ShouldBeIn;
				}
			}
			return m_condition == ShouldBeInOrNot.ShouldNotBeIn;
		}
	}
}
