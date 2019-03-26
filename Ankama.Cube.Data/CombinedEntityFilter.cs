using Ankama.Cube.Fight.Entities;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class CombinedEntityFilter : IEditableContent, IEntityFilter, ITargetFilter
	{
		private List<IEntityFilter> m_filters;

		public IReadOnlyList<IEntityFilter> filters => m_filters;

		public override string ToString()
		{
			return GetType().Name;
		}

		public static CombinedEntityFilter FromJsonToken(JToken token)
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
			CombinedEntityFilter combinedEntityFilter = new CombinedEntityFilter();
			combinedEntityFilter.PopulateFromJson(jsonObject);
			return combinedEntityFilter;
		}

		public static CombinedEntityFilter FromJsonProperty(JObject jsonObject, string propertyName, CombinedEntityFilter defaultValue = null)
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
			JArray val = Serialization.JsonArray(jsonObject, "filters");
			m_filters = new List<IEntityFilter>((val != null) ? val.get_Count() : 0);
			if (val != null)
			{
				foreach (JToken item in val)
				{
					m_filters.Add(IEntityFilterUtils.FromJsonToken(item));
				}
			}
		}

		public IEnumerable<IEntity> Filter(IEnumerable<IEntity> entities, DynamicValueContext context)
		{
			int i = 0;
			for (int count = m_filters.Count; i < count; i++)
			{
				entities = m_filters[i].Filter(entities, context);
			}
			return entities;
		}
	}
}
