using Ankama.Cube.Fight.Entities;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class PropertyCondition : EffectCondition
	{
		private ISingleEntitySelector m_selector;

		private ListComparison m_comparison;

		private List<PropertyId> m_properties;

		public ISingleEntitySelector selector => m_selector;

		public ListComparison comparison => m_comparison;

		public IReadOnlyList<PropertyId> properties => m_properties;

		public override string ToString()
		{
			string text = "";
			switch (m_properties.Count)
			{
			case 0:
				text = "<unset>";
				break;
			case 1:
				text = $"{m_properties[0]}";
				break;
			default:
				text = "\n - " + string.Join("\n - ", m_properties);
				break;
			}
			return $"{m_selector} {m_comparison} {text}";
		}

		public new static PropertyCondition FromJsonToken(JToken token)
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
			PropertyCondition propertyCondition = new PropertyCondition();
			propertyCondition.PopulateFromJson(jsonObject);
			return propertyCondition;
		}

		public static PropertyCondition FromJsonProperty(JObject jsonObject, string propertyName, PropertyCondition defaultValue = null)
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
			m_selector = ISingleEntitySelectorUtils.FromJsonProperty(jsonObject, "selector");
			m_comparison = (ListComparison)Serialization.JsonTokenValue<int>(jsonObject, "comparison", 1);
			m_properties = Serialization.JsonArrayAsList<PropertyId>(jsonObject, "properties");
		}

		public override bool IsValid(DynamicValueContext context)
		{
			if (!selector.TryGetEntity(context, out EntityStatus entity))
			{
				return false;
			}
			return PropertiesFilter.ValidateCondition(entity, m_comparison, m_properties);
		}
	}
}
