using Ankama.Cube.Fight.Entities;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class SpecificObjectMechanismFilter : IEditableContent, ISpecificEntityFilter, IEntityFilter, ITargetFilter
	{
		private ShouldBeInOrNot m_condition;

		private List<Id<ObjectMechanismDefinition>> m_objectMechanisms;

		public ShouldBeInOrNot condition => m_condition;

		public IReadOnlyList<Id<ObjectMechanismDefinition>> objectMechanisms => m_objectMechanisms;

		public override string ToString()
		{
			switch (m_objectMechanisms.Count)
			{
			case 0:
				return "objectMechanism is <unset>";
			case 1:
				return $"objectMechanism {condition} {m_objectMechanisms[0]}";
			default:
				return $"objectMechanism {condition} \n - " + string.Join("\n - ", m_objectMechanisms);
			}
		}

		public static SpecificObjectMechanismFilter FromJsonToken(JToken token)
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
			SpecificObjectMechanismFilter specificObjectMechanismFilter = new SpecificObjectMechanismFilter();
			specificObjectMechanismFilter.PopulateFromJson(jsonObject);
			return specificObjectMechanismFilter;
		}

		public static SpecificObjectMechanismFilter FromJsonProperty(JObject jsonObject, string propertyName, SpecificObjectMechanismFilter defaultValue = null)
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
			JArray val = Serialization.JsonArray(jsonObject, "objectMechanisms");
			m_objectMechanisms = new List<Id<ObjectMechanismDefinition>>((val != null) ? val.get_Count() : 0);
			if (val != null)
			{
				foreach (JToken item2 in val)
				{
					Id<ObjectMechanismDefinition> item = Serialization.JsonTokenIdValue<ObjectMechanismDefinition>(item2);
					m_objectMechanisms.Add(item);
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
			ObjectMechanismStatus objectMechanismStatus;
			if ((objectMechanismStatus = (entity as ObjectMechanismStatus)) == null)
			{
				return false;
			}
			bool flag = m_condition == ShouldBeInOrNot.ShouldBeIn;
			return Contains(objectMechanismStatus.definition.get_id()) == flag;
		}

		private bool Contains(int id)
		{
			int count = m_objectMechanisms.Count;
			for (int i = 0; i < count; i++)
			{
				if (id == m_objectMechanisms[i].value)
				{
					return true;
				}
			}
			return false;
		}
	}
}
