using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class SpellTagsFilter : SpellFilter
	{
		private ListComparison m_comparison;

		private List<SpellTag> m_spellTags;

		public ListComparison comparison => m_comparison;

		public IReadOnlyList<SpellTag> spellTags => m_spellTags;

		public override string ToString()
		{
			string text = "";
			switch (m_spellTags.Count)
			{
			case 0:
				text = "<unset>";
				break;
			case 1:
				text = $"{m_spellTags[0]}";
				break;
			default:
				text = "\n - " + string.Join("\n - ", m_spellTags);
				break;
			}
			return $"{m_comparison} {text}";
		}

		public new static SpellTagsFilter FromJsonToken(JToken token)
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
			SpellTagsFilter spellTagsFilter = new SpellTagsFilter();
			spellTagsFilter.PopulateFromJson(jsonObject);
			return spellTagsFilter;
		}

		public static SpellTagsFilter FromJsonProperty(JObject jsonObject, string propertyName, SpellTagsFilter defaultValue = null)
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
			m_comparison = (ListComparison)Serialization.JsonTokenValue<int>(jsonObject, "comparison", 1);
			m_spellTags = Serialization.JsonArrayAsList<SpellTag>(jsonObject, "spellTags");
		}

		public override bool Accept(int spellInstanceId, SpellDefinition spellDef)
		{
			return ListComparisonUtility.ValidateCondition(spellDef.tags, m_comparison, m_spellTags);
		}
	}
}
