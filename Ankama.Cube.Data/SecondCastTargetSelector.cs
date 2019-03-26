using Ankama.Cube.Fight.Entities;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class SecondCastTargetSelector : IEditableContent, ITargetSelector, ISingleEntitySelector, IEntitySelector, ISingleTargetSelector, ISingleCoordSelector, ICoordSelector
	{
		public override string ToString()
		{
			return GetType().Name;
		}

		public static SecondCastTargetSelector FromJsonToken(JToken token)
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
			SecondCastTargetSelector secondCastTargetSelector = new SecondCastTargetSelector();
			secondCastTargetSelector.PopulateFromJson(jsonObject);
			return secondCastTargetSelector;
		}

		public static SecondCastTargetSelector FromJsonProperty(JObject jsonObject, string propertyName, SecondCastTargetSelector defaultValue = null)
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
		}

		public bool TryGetEntity<T>(DynamicValueContext context, out T entity) where T : class, IEntity
		{
			CastTargetContext castTargetContext = context as CastTargetContext;
			if (castTargetContext != null)
			{
				Target target = castTargetContext.GetTarget(1);
				if (target.type == Target.Type.Entity)
				{
					T val = target.entity as T;
					if (val != null)
					{
						entity = val;
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
				Target target = castTargetContext.GetTarget(1);
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
