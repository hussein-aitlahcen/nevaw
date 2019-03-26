using Ankama.Cube.Fight.Entities;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class CastTargetFilter : IEditableContent, IEntityFilter, ITargetFilter
	{
		private int m_castTargetIndex;

		public int castTargetIndex => m_castTargetIndex;

		public override string ToString()
		{
			return GetType().Name;
		}

		public static CastTargetFilter FromJsonToken(JToken token)
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
			CastTargetFilter castTargetFilter = new CastTargetFilter();
			castTargetFilter.PopulateFromJson(jsonObject);
			return castTargetFilter;
		}

		public static CastTargetFilter FromJsonProperty(JObject jsonObject, string propertyName, CastTargetFilter defaultValue = null)
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
			m_castTargetIndex = Serialization.JsonTokenValue<int>(jsonObject, "castTargetIndex", 0);
		}

		public IEnumerable<IEntity> Filter(IEnumerable<IEntity> entities, DynamicValueContext context)
		{
			CastTargetContext castTargetContext = context as CastTargetContext;
			if (castTargetContext != null)
			{
				Target target = castTargetContext.GetTarget(m_castTargetIndex);
				if (target.type == Target.Type.Entity)
				{
					IEntity entity = target.entity;
					foreach (IEntity entity2 in entities)
					{
						if (entity == entity2)
						{
							yield return entity;
							yield break;
						}
					}
				}
			}
		}
	}
}
