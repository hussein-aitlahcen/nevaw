using Ankama.Cube.Fight.Entities;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class IntoAreaOfEntityFilter : IEditableContent, IEntityFilter, ITargetFilter
	{
		private ISingleEntitySelector m_areaOfEntity;

		public ISingleEntitySelector areaOfEntity => m_areaOfEntity;

		public override string ToString()
		{
			return $"into {m_areaOfEntity}";
		}

		public static IntoAreaOfEntityFilter FromJsonToken(JToken token)
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
			IntoAreaOfEntityFilter intoAreaOfEntityFilter = new IntoAreaOfEntityFilter();
			intoAreaOfEntityFilter.PopulateFromJson(jsonObject);
			return intoAreaOfEntityFilter;
		}

		public static IntoAreaOfEntityFilter FromJsonProperty(JObject jsonObject, string propertyName, IntoAreaOfEntityFilter defaultValue = null)
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
			m_areaOfEntity = ISingleEntitySelectorUtils.FromJsonProperty(jsonObject, "areaOfEntity");
		}

		public IEnumerable<IEntity> Filter(IEnumerable<IEntity> entities, DynamicValueContext context)
		{
			if (ZoneAreaFilterUtils.SingleTargetToCompareArea(m_areaOfEntity, context, out Area area))
			{
				foreach (IEntity entity in entities)
				{
					IEntityWithBoardPresence entityWithBoardPresence = entity as IEntityWithBoardPresence;
					if (entityWithBoardPresence != null && area.Intersects(entityWithBoardPresence.area))
					{
						yield return entity;
					}
				}
			}
		}
	}
}
