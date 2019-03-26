using Ankama.Cube.Fight.Entities;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class WoundedFilter : IEditableContent, IEntityFilter, ITargetFilter
	{
		private bool m_isWounded;

		public bool isWounded => m_isWounded;

		public override string ToString()
		{
			return (m_isWounded ? "" : "not ") + "wounded";
		}

		public static WoundedFilter FromJsonToken(JToken token)
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
			WoundedFilter woundedFilter = new WoundedFilter();
			woundedFilter.PopulateFromJson(jsonObject);
			return woundedFilter;
		}

		public static WoundedFilter FromJsonProperty(JObject jsonObject, string propertyName, WoundedFilter defaultValue = null)
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
			m_isWounded = Serialization.JsonTokenValue<bool>(jsonObject, "isWounded", true);
		}

		public IEnumerable<IEntity> Filter(IEnumerable<IEntity> entities, DynamicValueContext context)
		{
			foreach (IEntity entity in entities)
			{
				IEntityWithLife entityWithLife = entity as IEntityWithLife;
				if (entityWithLife != null && entityWithLife.wounded)
				{
					yield return entity;
				}
			}
		}
	}
}
