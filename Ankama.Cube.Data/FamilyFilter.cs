using Ankama.Cube.Fight.Entities;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class FamilyFilter : IEditableContent, IEntityFilter, ITargetFilter
	{
		private ShouldBeInOrNot m_condition;

		private List<Family> m_families;

		public ShouldBeInOrNot condition => m_condition;

		public IReadOnlyList<Family> families => m_families;

		public override string ToString()
		{
			switch (m_families.Count)
			{
			case 0:
				return "family <unset>";
			case 1:
				return $"family {condition} {m_families[0]}";
			default:
				return $"family {condition} \n - " + string.Join("\n - ", m_families);
			}
		}

		public static FamilyFilter FromJsonToken(JToken token)
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
			FamilyFilter familyFilter = new FamilyFilter();
			familyFilter.PopulateFromJson(jsonObject);
			return familyFilter;
		}

		public static FamilyFilter FromJsonProperty(JObject jsonObject, string propertyName, FamilyFilter defaultValue = null)
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
			m_families = Serialization.JsonArrayAsList<Family>(jsonObject, "families");
		}

		public IEnumerable<IEntity> Filter(IEnumerable<IEntity> entities, DynamicValueContext context)
		{
			bool shouldContains = m_condition == ShouldBeInOrNot.ShouldBeIn;
			foreach (IEntity entity in entities)
			{
				IEntityWithFamilies entityWithFamilies = entity as IEntityWithFamilies;
				if (entityWithFamilies != null && FamiliesIntersects(m_families, entityWithFamilies.families) == shouldContains)
				{
					yield return entity;
				}
			}
		}

		private static bool FamiliesIntersects(List<Family> families, IReadOnlyList<Family> others)
		{
			int count = families.Count;
			foreach (Family other in others)
			{
				for (int i = 0; i < count; i++)
				{
					if (other == families[i])
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
