using Ankama.Cube.Fight.Entities;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class BounceEntitiesSelector : IEditableContent, ITargetSelector, IEntitySelector
	{
		private ISingleEntitySelector m_start;

		private bool m_includeStart;

		private List<IEntityFilter> m_bounceFilters;

		public ISingleEntitySelector start => m_start;

		public bool includeStart => m_includeStart;

		public IReadOnlyList<IEntityFilter> bounceFilters => m_bounceFilters;

		public override string ToString()
		{
			return GetType().Name;
		}

		public static BounceEntitiesSelector FromJsonToken(JToken token)
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
			BounceEntitiesSelector bounceEntitiesSelector = new BounceEntitiesSelector();
			bounceEntitiesSelector.PopulateFromJson(jsonObject);
			return bounceEntitiesSelector;
		}

		public static BounceEntitiesSelector FromJsonProperty(JObject jsonObject, string propertyName, BounceEntitiesSelector defaultValue = null)
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
			m_start = ISingleEntitySelectorUtils.FromJsonProperty(jsonObject, "start");
			m_includeStart = Serialization.JsonTokenValue<bool>(jsonObject, "includeStart", true);
			JArray val = Serialization.JsonArray(jsonObject, "bounceFilters");
			m_bounceFilters = new List<IEntityFilter>((val != null) ? val.get_Count() : 0);
			if (val != null)
			{
				foreach (JToken item in val)
				{
					m_bounceFilters.Add(IEntityFilterUtils.FromJsonToken(item));
				}
			}
		}

		public IEnumerable<IEntity> EnumerateEntities(DynamicValueContext context)
		{
			yield break;
		}
	}
}
