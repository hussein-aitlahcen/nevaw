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
	public sealed class FilteredEntitySelector : EntitySelectorForCast
	{
		private List<IEntityFilter> m_filters;

		public IReadOnlyList<IEntityFilter> filters => m_filters;

		public override string ToString()
		{
			switch (m_filters.Count)
			{
			case 0:
				return "All entities";
			case 1:
				return $"entity with ({m_filters[0]})";
			default:
				return "Entities where:\n - " + string.Join("\n - ", filters);
			}
		}

		public new static FilteredEntitySelector FromJsonToken(JToken token)
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
			FilteredEntitySelector filteredEntitySelector = new FilteredEntitySelector();
			filteredEntitySelector.PopulateFromJson(jsonObject);
			return filteredEntitySelector;
		}

		public static FilteredEntitySelector FromJsonProperty(JObject jsonObject, string propertyName, FilteredEntitySelector defaultValue = null)
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

		public static FilteredEntitySelector From(params IEntityFilter[] filters)
		{
			return new FilteredEntitySelector
			{
				m_filters = filters.ToList()
			};
		}

		public override IEnumerable<IEntity> EnumerateEntities(DynamicValueContext context)
		{
			DynamicValueFightContext dynamicValueFightContext = context as DynamicValueFightContext;
			if (dynamicValueFightContext != null)
			{
				IEnumerable<IEntity> enumerable = dynamicValueFightContext.fightStatus.EnumerateEntities();
				int count = m_filters.Count;
				for (int i = 0; i < count; i++)
				{
					enumerable = m_filters[i].Filter(enumerable, context);
				}
				return enumerable;
			}
			return Enumerable.Empty<IEntity>();
		}
	}
}
