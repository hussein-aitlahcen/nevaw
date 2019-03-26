using Ankama.Cube.Fight.Entities;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	[RelatedToEvents(new EventCategory[]
	{
		EventCategory.PropertyChanged
	})]
	public sealed class PropertiesFilter : IEditableContent, IEntityFilter, ITargetFilter
	{
		private ListComparison m_comparison;

		private List<PropertyId> m_properties;

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
			return $"{m_comparison} {text}";
		}

		public static PropertiesFilter FromJsonToken(JToken token)
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
			PropertiesFilter propertiesFilter = new PropertiesFilter();
			propertiesFilter.PopulateFromJson(jsonObject);
			return propertiesFilter;
		}

		public static PropertiesFilter FromJsonProperty(JObject jsonObject, string propertyName, PropertiesFilter defaultValue = null)
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

		public void PopulateFromJson(JObject jsonObject)
		{
			m_comparison = (ListComparison)Serialization.JsonTokenValue<int>(jsonObject, "comparison", 1);
			m_properties = Serialization.JsonArrayAsList<PropertyId>(jsonObject, "properties");
		}

		public IEnumerable<IEntity> Filter(IEnumerable<IEntity> entities, DynamicValueContext context)
		{
			ListComparison comp = m_comparison;
			List<PropertyId> expectedProperties = m_properties;
			foreach (IEntity entity in entities)
			{
				if (ValidateCondition(entity, comp, expectedProperties))
				{
					yield return entity;
				}
			}
		}

		public static bool ValidateCondition(IEntity entity, ListComparison comparison, List<PropertyId> properties)
		{
			return ListComparisonUtility.ValidateCondition(entity.properties, comparison, properties);
		}
	}
}
