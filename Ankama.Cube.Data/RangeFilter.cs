using Ankama.Cube.Fight.Entities;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class RangeFilter : IEditableContent, IEntityFilter, ITargetFilter
	{
		private ValueFilter m_valueFilter;

		public ValueFilter valueFilter => m_valueFilter;

		public override string ToString()
		{
			if (m_valueFilter != null)
			{
				return $"entity with RangeMax {valueFilter}";
			}
			return "entity with No Range";
		}

		public static RangeFilter FromJsonToken(JToken token)
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
			RangeFilter rangeFilter = new RangeFilter();
			rangeFilter.PopulateFromJson(jsonObject);
			return rangeFilter;
		}

		public static RangeFilter FromJsonProperty(JObject jsonObject, string propertyName, RangeFilter defaultValue = null)
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
			m_valueFilter = ValueFilter.FromJsonProperty(jsonObject, "valueFilter");
		}

		public IEnumerable<IEntity> Filter(IEnumerable<IEntity> entities, DynamicValueContext context)
		{
			ValueFilter filter = m_valueFilter;
			foreach (IEntity entity in entities)
			{
				IEntityWithAction entityWithAction = entity as IEntityWithAction;
				if (entityWithAction != null)
				{
					if (filter == null)
					{
						if (!entityWithAction.hasRange)
						{
							yield return entity;
						}
					}
					else if (entityWithAction.hasRange && filter.Matches(entityWithAction.rangeMax, context))
					{
						yield return entity;
					}
				}
			}
		}
	}
}
