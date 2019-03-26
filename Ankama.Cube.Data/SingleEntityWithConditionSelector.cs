using Ankama.Cube.Fight.Entities;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class SingleEntityWithConditionSelector : IEditableContent, ITargetSelector, ISingleEntitySelector, IEntitySelector, ISingleTargetSelector, ISingleTargetWithFiltersSelector
	{
		private ISingleEntitySelector m_from;

		private List<IEntityFilter> m_onlyIf;

		public ISingleEntitySelector from => m_from;

		public IReadOnlyList<IEntityFilter> onlyIf => m_onlyIf;

		public override string ToString()
		{
			return GetType().Name;
		}

		public static SingleEntityWithConditionSelector FromJsonToken(JToken token)
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
			SingleEntityWithConditionSelector singleEntityWithConditionSelector = new SingleEntityWithConditionSelector();
			singleEntityWithConditionSelector.PopulateFromJson(jsonObject);
			return singleEntityWithConditionSelector;
		}

		public static SingleEntityWithConditionSelector FromJsonProperty(JObject jsonObject, string propertyName, SingleEntityWithConditionSelector defaultValue = null)
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
			m_from = ISingleEntitySelectorUtils.FromJsonProperty(jsonObject, "from");
			JArray val = Serialization.JsonArray(jsonObject, "onlyIf");
			m_onlyIf = new List<IEntityFilter>((val != null) ? val.get_Count() : 0);
			if (val != null)
			{
				foreach (JToken item in val)
				{
					m_onlyIf.Add(IEntityFilterUtils.FromJsonToken(item));
				}
			}
		}

		public IEnumerable<IEntity> EnumerateEntities(DynamicValueContext context)
		{
			DynamicValueFightContext dynamicValueFightContext = context as DynamicValueFightContext;
			if (dynamicValueFightContext != null)
			{
				IEnumerable<IEntity> enumerable = dynamicValueFightContext.fightStatus.EnumerateEntities();
				int count = m_onlyIf.Count;
				for (int i = 0; i < count; i++)
				{
					enumerable = m_onlyIf[i].Filter(enumerable, context);
				}
				return enumerable;
			}
			return Enumerable.Empty<IEntity>();
		}

		public bool TryGetEntity<T>(DynamicValueContext context, out T entity) where T : class, IEntity
		{
			throw new NotImplementedException();
		}
	}
}
