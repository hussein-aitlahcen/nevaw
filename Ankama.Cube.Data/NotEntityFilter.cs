using Ankama.Cube.Fight.Entities;
using Ankama.Utilities;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class NotEntityFilter : IEditableContent, IEntityFilter, ITargetFilter
	{
		private IEntityFilter m_filter;

		public IEntityFilter filter => m_filter;

		public override string ToString()
		{
			return $"not ({filter})";
		}

		public static NotEntityFilter FromJsonToken(JToken token)
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
			NotEntityFilter notEntityFilter = new NotEntityFilter();
			notEntityFilter.PopulateFromJson(jsonObject);
			return notEntityFilter;
		}

		public static NotEntityFilter FromJsonProperty(JObject jsonObject, string propertyName, NotEntityFilter defaultValue = null)
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
			m_filter = IEntityFilterUtils.FromJsonProperty(jsonObject, "filter");
		}

		public IEnumerable<IEntity> Filter(IEnumerable<IEntity> entities, DynamicValueContext context)
		{
			List<IEntity> entityList = ListPool<IEntity>.Get();
			List<IEntity> filtered = ListPool<IEntity>.Get();
			entityList.AddRange(entities);
			filtered.AddRange(m_filter.Filter(entityList, context));
			int coordCount = entityList.Count;
			int filteredCount = filtered.Count;
			int num2;
			for (int i = 0; i < coordCount; i = num2)
			{
				IEntity entity = entityList[i];
				int num = 0;
				while (true)
				{
					if (num < filteredCount)
					{
						if (entity == filtered[num])
						{
							break;
						}
						num++;
						continue;
					}
					yield return entity;
					break;
				}
				num2 = i + 1;
			}
			ListPool<IEntity>.Release(entityList);
			ListPool<IEntity>.Release(filtered);
		}
	}
}
