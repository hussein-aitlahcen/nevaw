using Ankama.Cube.Fight.Entities;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	[RelatedToEvents(new EventCategory[]
	{
		EventCategory.LifeArmorChanged
	})]
	public sealed class ArmorFilter : IEditableContent, IEntityFilter, ITargetFilter
	{
		private ValueFilter m_valueFilter;

		public ValueFilter valueFilter => m_valueFilter;

		public override string ToString()
		{
			return $"entity with Armor {valueFilter}";
		}

		public static ArmorFilter FromJsonToken(JToken token)
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
			ArmorFilter armorFilter = new ArmorFilter();
			armorFilter.PopulateFromJson(jsonObject);
			return armorFilter;
		}

		public static ArmorFilter FromJsonProperty(JObject jsonObject, string propertyName, ArmorFilter defaultValue = null)
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
			foreach (IEntity entity in entities)
			{
				IEntityWithLife entityWithLife = entity as IEntityWithLife;
				if (entityWithLife != null && entityWithLife.hasArmor && m_valueFilter.Matches(entityWithLife.armor, context))
				{
					yield return entity;
				}
			}
		}
	}
}
