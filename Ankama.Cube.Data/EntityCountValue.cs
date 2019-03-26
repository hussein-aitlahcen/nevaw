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
	[RelatedToEvents(new EventCategory[]
	{
		EventCategory.EntityAddedOrRemoved
	})]
	public sealed class EntityCountValue : DynamicValue
	{
		private List<IEntityFilter> m_entityFilters;

		public IReadOnlyList<IEntityFilter> entityFilters => m_entityFilters;

		public override string ToString()
		{
			return "<filtered entities count>";
		}

		public new static EntityCountValue FromJsonToken(JToken token)
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
			EntityCountValue entityCountValue = new EntityCountValue();
			entityCountValue.PopulateFromJson(jsonObject);
			return entityCountValue;
		}

		public static EntityCountValue FromJsonProperty(JObject jsonObject, string propertyName, EntityCountValue defaultValue = null)
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
			JArray val = Serialization.JsonArray(jsonObject, "entityFilters");
			m_entityFilters = new List<IEntityFilter>((val != null) ? val.get_Count() : 0);
			if (val != null)
			{
				foreach (JToken item in val)
				{
					m_entityFilters.Add(IEntityFilterUtils.FromJsonToken(item));
				}
			}
		}

		public override bool GetValue(DynamicValueContext context, out int value)
		{
			DynamicValueFightContext dynamicValueFightContext = context as DynamicValueFightContext;
			if (dynamicValueFightContext != null)
			{
				IEnumerable<IEntity> enumerable = dynamicValueFightContext.fightStatus.EnumerateEntities();
				for (int i = 0; i < m_entityFilters.Count; i++)
				{
					enumerable = m_entityFilters[i].Filter(enumerable, context);
				}
				value = enumerable.Count();
				return true;
			}
			value = 0;
			return false;
		}
	}
}
