using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class DiscardSpellAndDrawEffectDefinition : EffectExecutionDefinition
	{
		private bool m_randomly;

		private DynamicValue m_count;

		private List<SpellFilter> m_discardSpellFilters;

		private List<SpellFilter> m_drawSpellFilters;

		public bool randomly => m_randomly;

		public DynamicValue count => m_count;

		public IReadOnlyList<SpellFilter> discardSpellFilters => m_discardSpellFilters;

		public IReadOnlyList<SpellFilter> drawSpellFilters => m_drawSpellFilters;

		public override string ToString()
		{
			return string.Format("{0} discards {1} spells and draws the same amount{2}", m_executionTargetSelector, count, (m_condition == null) ? "" : $" if {m_condition}");
		}

		public new static DiscardSpellAndDrawEffectDefinition FromJsonToken(JToken token)
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
			DiscardSpellAndDrawEffectDefinition discardSpellAndDrawEffectDefinition = new DiscardSpellAndDrawEffectDefinition();
			discardSpellAndDrawEffectDefinition.PopulateFromJson(jsonObject);
			return discardSpellAndDrawEffectDefinition;
		}

		public static DiscardSpellAndDrawEffectDefinition FromJsonProperty(JObject jsonObject, string propertyName, DiscardSpellAndDrawEffectDefinition defaultValue = null)
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
			m_randomly = Serialization.JsonTokenValue<bool>(jsonObject, "randomly", true);
			m_count = DynamicValue.FromJsonProperty(jsonObject, "count");
			JArray val = Serialization.JsonArray(jsonObject, "discardSpellFilters");
			m_discardSpellFilters = new List<SpellFilter>((val != null) ? val.get_Count() : 0);
			if (val != null)
			{
				foreach (JToken item in val)
				{
					m_discardSpellFilters.Add(SpellFilter.FromJsonToken(item));
				}
			}
			JArray val2 = Serialization.JsonArray(jsonObject, "drawSpellFilters");
			m_drawSpellFilters = new List<SpellFilter>((val2 != null) ? val2.get_Count() : 0);
			if (val2 != null)
			{
				foreach (JToken item2 in val2)
				{
					m_drawSpellFilters.Add(SpellFilter.FromJsonToken(item2));
				}
			}
		}
	}
}
