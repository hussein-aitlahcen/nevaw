using Ankama.Cube.Fight.Entities;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class SpecificCompanionFilter : IEditableContent, ISpecificEntityFilter, IEntityFilter, ITargetFilter
	{
		private ShouldBeInOrNot m_condition;

		private List<Id<CompanionDefinition>> m_companions;

		public ShouldBeInOrNot condition => m_condition;

		public IReadOnlyList<Id<CompanionDefinition>> companions => m_companions;

		public override string ToString()
		{
			switch (m_companions.Count)
			{
			case 0:
				return "companion is <unset>";
			case 1:
				return $"companion {condition} {m_companions[0]}";
			default:
				return $"companion {condition} \n - " + string.Join("\n - ", m_companions);
			}
		}

		public static SpecificCompanionFilter FromJsonToken(JToken token)
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
			SpecificCompanionFilter specificCompanionFilter = new SpecificCompanionFilter();
			specificCompanionFilter.PopulateFromJson(jsonObject);
			return specificCompanionFilter;
		}

		public static SpecificCompanionFilter FromJsonProperty(JObject jsonObject, string propertyName, SpecificCompanionFilter defaultValue = null)
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
			JArray val = Serialization.JsonArray(jsonObject, "companions");
			m_companions = new List<Id<CompanionDefinition>>((val != null) ? val.get_Count() : 0);
			if (val != null)
			{
				foreach (JToken item2 in val)
				{
					Id<CompanionDefinition> item = Serialization.JsonTokenIdValue<CompanionDefinition>(item2);
					m_companions.Add(item);
				}
			}
		}

		public IEnumerable<IEntity> Filter(IEnumerable<IEntity> entities, DynamicValueContext context)
		{
			foreach (IEntity entity in entities)
			{
				if (ValidFor(entity))
				{
					yield return entity;
				}
			}
		}

		public bool ValidFor(IEntity entity)
		{
			CompanionStatus companionStatus;
			if ((companionStatus = (entity as CompanionStatus)) == null)
			{
				return false;
			}
			bool flag = m_condition == ShouldBeInOrNot.ShouldBeIn;
			return Contains(companionStatus.definition.get_id()) == flag;
		}

		private bool Contains(int id)
		{
			int count = m_companions.Count;
			for (int i = 0; i < count; i++)
			{
				if (id == m_companions[i].value)
				{
					return true;
				}
			}
			return false;
		}
	}
}
