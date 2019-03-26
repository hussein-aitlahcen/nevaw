using Ankama.Cube.Fight.Entities;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class CharacterActionTypeFilter : IEditableContent, IEntityFilter, ITargetFilter
	{
		private ShouldBeInOrNot m_condition;

		private List<ActionType> m_actionTypes;

		public ShouldBeInOrNot condition => m_condition;

		public IReadOnlyList<ActionType> actionTypes => m_actionTypes;

		public override string ToString()
		{
			switch (m_actionTypes.Count)
			{
			case 0:
				return "actionType <unset>";
			case 1:
				return $"actionType {condition} {m_actionTypes[0]}";
			default:
				return $"actionType {condition} \n - " + string.Join("\n - ", m_actionTypes);
			}
		}

		public static CharacterActionTypeFilter FromJsonToken(JToken token)
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
			CharacterActionTypeFilter characterActionTypeFilter = new CharacterActionTypeFilter();
			characterActionTypeFilter.PopulateFromJson(jsonObject);
			return characterActionTypeFilter;
		}

		public static CharacterActionTypeFilter FromJsonProperty(JObject jsonObject, string propertyName, CharacterActionTypeFilter defaultValue = null)
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
			m_actionTypes = Serialization.JsonArrayAsList<ActionType>(jsonObject, "actionTypes");
		}

		public IEnumerable<IEntity> Filter(IEnumerable<IEntity> entities, DynamicValueContext context)
		{
			bool shouldContains = m_condition == ShouldBeInOrNot.ShouldBeIn;
			foreach (IEntity entity in entities)
			{
				IEntityWithAction entityWithAction = entity as IEntityWithAction;
				if (entityWithAction != null && ContainsEntityType(entityWithAction.actionType) == shouldContains)
				{
					yield return entity;
				}
			}
		}

		private bool ContainsEntityType(ActionType actionType)
		{
			int count = m_actionTypes.Count;
			for (int i = 0; i < count; i++)
			{
				if (actionType == m_actionTypes[i])
				{
					return true;
				}
			}
			return false;
		}
	}
}
