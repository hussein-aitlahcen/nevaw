using Ankama.Cube.Fight.Entities;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class CasterSpecificCompanionSelector : IEditableContent, ITargetSelector, ISingleEntitySelector, IEntitySelector, ISingleTargetSelector
	{
		private Id<CompanionDefinition> m_companion;

		public Id<CompanionDefinition> companion => m_companion;

		public override string ToString()
		{
			if (!(m_companion == null))
			{
				CompanionDefinition companion = ObjectReference.GetCompanion(m_companion.value);
				if (companion == null)
				{
					return null;
				}
				return companion.get_idAndName();
			}
			return "<not defined>";
		}

		public static CasterSpecificCompanionSelector FromJsonToken(JToken token)
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
			CasterSpecificCompanionSelector casterSpecificCompanionSelector = new CasterSpecificCompanionSelector();
			casterSpecificCompanionSelector.PopulateFromJson(jsonObject);
			return casterSpecificCompanionSelector;
		}

		public static CasterSpecificCompanionSelector FromJsonProperty(JObject jsonObject, string propertyName, CasterSpecificCompanionSelector defaultValue = null)
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
			m_companion = Serialization.JsonTokenIdValue<CompanionDefinition>(jsonObject, "companion");
		}

		public IEnumerable<IEntity> EnumerateEntities(DynamicValueContext context)
		{
			if (TryGetEntity(context, out IEntity entity))
			{
				yield return entity;
			}
		}

		public bool TryGetEntity<T>(DynamicValueContext context, out T entity) where T : class, IEntity
		{
			DynamicValueFightContext dynamicValueFightContext = context as DynamicValueFightContext;
			if (dynamicValueFightContext != null)
			{
				foreach (T item in dynamicValueFightContext.fightStatus.EnumerateEntities<T>())
				{
					CompanionStatus companionStatus = item as CompanionStatus;
					if (companionStatus != null && companionStatus.ownerId == dynamicValueFightContext.playerId && companionStatus.definition.get_id() == m_companion.value)
					{
						entity = item;
						return true;
					}
				}
			}
			entity = null;
			return false;
		}
	}
}
