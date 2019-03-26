using Ankama.Cube.Fight.Entities;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class EntityTypeFilter : IEditableContent, IEntityFilter, ITargetFilter
	{
		private ShouldBeInOrNot m_condition;

		private List<EntityType> m_entityTypes;

		public ShouldBeInOrNot condition => m_condition;

		public IReadOnlyList<EntityType> entityTypes => m_entityTypes;

		public override string ToString()
		{
			switch (m_entityTypes.Count)
			{
			case 0:
				return "entity type <unset>";
			case 1:
				return $"entity type {condition} {m_entityTypes[0]}";
			default:
				return $"entity type {condition} \n - " + string.Join("\n - ", m_entityTypes);
			}
		}

		public static EntityTypeFilter FromJsonToken(JToken token)
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
			EntityTypeFilter entityTypeFilter = new EntityTypeFilter();
			entityTypeFilter.PopulateFromJson(jsonObject);
			return entityTypeFilter;
		}

		public static EntityTypeFilter FromJsonProperty(JObject jsonObject, string propertyName, EntityTypeFilter defaultValue = null)
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
			m_condition = (ShouldBeInOrNot)Serialization.JsonTokenValue<int>(jsonObject, "condition", 1);
			m_entityTypes = Serialization.JsonArrayAsList<EntityType>(jsonObject, "entityTypes");
		}

		public IEnumerable<IEntity> Filter(IEnumerable<IEntity> entities, DynamicValueContext context)
		{
			bool shouldContains = m_condition == ShouldBeInOrNot.ShouldBeIn;
			foreach (IEntity entity in entities)
			{
				if (ContainsEntityType(entity.type) == shouldContains)
				{
					yield return entity;
				}
			}
		}

		private bool ContainsEntityType(EntityType entityType)
		{
			int count = m_entityTypes.Count;
			for (int i = 0; i < count; i++)
			{
				if (entityType == m_entityTypes[i])
				{
					return true;
				}
			}
			return false;
		}
	}
}
