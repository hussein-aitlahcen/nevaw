using Ankama.Cube.Fight.Entities;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class IndexedCastTargetSelector : IEditableContent, ITargetSelector, ISingleEntitySelector, IEntitySelector, ISingleTargetSelector, ISingleCoordSelector, ICoordSelector
	{
		private int m_castTargetIndex;

		public int castTargetIndex => m_castTargetIndex;

		public override string ToString()
		{
			return GetType().Name;
		}

		public static IndexedCastTargetSelector FromJsonToken(JToken token)
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
			IndexedCastTargetSelector indexedCastTargetSelector = new IndexedCastTargetSelector();
			indexedCastTargetSelector.PopulateFromJson(jsonObject);
			return indexedCastTargetSelector;
		}

		public static IndexedCastTargetSelector FromJsonProperty(JObject jsonObject, string propertyName, IndexedCastTargetSelector defaultValue = null)
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

		public bool TryGetEntity<T>(DynamicValueContext context, out T entity) where T : class, IEntity
		{
			CastTargetContext castTargetContext = context as CastTargetContext;
			if (castTargetContext != null)
			{
				Target target = castTargetContext.GetTarget(m_castTargetIndex);
				if (target.type == Target.Type.Entity)
				{
					IEntity entity2 = target.entity;
					if (entity2 is T)
					{
						entity = (T)entity2;
						return true;
					}
				}
			}
			entity = null;
			return false;
		}

		public bool TryGetCoord(DynamicValueContext context, out Coord coord)
		{
			CastTargetContext castTargetContext = context as CastTargetContext;
			if (castTargetContext != null)
			{
				Target target = castTargetContext.GetTarget(m_castTargetIndex);
				if (target.type == Target.Type.Coord)
				{
					coord = target.coord;
					return true;
				}
			}
			coord = default(Coord);
			return false;
		}

		public IEnumerable<IEntity> EnumerateEntities(DynamicValueContext context)
		{
			if (TryGetEntity(context, out IEntity entity))
			{
				yield return entity;
			}
		}

		public IEnumerable<Coord> EnumerateCoords(DynamicValueContext context)
		{
			if (TryGetCoord(context, out Coord coord))
			{
				yield return coord;
			}
		}
	}
}
