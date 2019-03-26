using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class SpellElementsFilter : SpellFilter
	{
		private ShouldBeInOrNot m_condition;

		private List<Element> m_elements;

		public ShouldBeInOrNot condition => m_condition;

		public IReadOnlyList<Element> elements => m_elements;

		public override string ToString()
		{
			switch (m_elements.Count)
			{
			case 0:
				return "elements is <unset>";
			case 1:
				return $"elements {condition} {m_elements[0]}";
			default:
				return $"elements {condition} \n - " + string.Join("\n - ", m_elements);
			}
		}

		public new static SpellElementsFilter FromJsonToken(JToken token)
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
			SpellElementsFilter spellElementsFilter = new SpellElementsFilter();
			spellElementsFilter.PopulateFromJson(jsonObject);
			return spellElementsFilter;
		}

		public static SpellElementsFilter FromJsonProperty(JObject jsonObject, string propertyName, SpellElementsFilter defaultValue = null)
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
			m_elements = Serialization.JsonArrayAsList<Element>(jsonObject, "elements");
		}

		public override bool Accept(int spellInstanceId, SpellDefinition spellDef)
		{
			Element element = spellDef.element;
			for (int i = 0; i < m_elements.Count; i++)
			{
				if (m_elements[i] == element)
				{
					return m_condition == ShouldBeInOrNot.ShouldBeIn;
				}
			}
			return m_condition == ShouldBeInOrNot.ShouldNotBeIn;
		}
	}
}
