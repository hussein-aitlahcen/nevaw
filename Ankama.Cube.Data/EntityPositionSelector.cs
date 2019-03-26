using Ankama.Cube.Fight.Entities;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class EntityPositionSelector : IEditableContent, ITargetSelector, ISingleCoordSelector, ICoordSelector, ISingleTargetSelector
	{
		private ISingleEntitySelector m_entity;

		public ISingleEntitySelector entity => m_entity;

		public override string ToString()
		{
			return m_entity.ToString();
		}

		public static EntityPositionSelector FromJsonToken(JToken token)
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
			EntityPositionSelector entityPositionSelector = new EntityPositionSelector();
			entityPositionSelector.PopulateFromJson(jsonObject);
			return entityPositionSelector;
		}

		public static EntityPositionSelector FromJsonProperty(JObject jsonObject, string propertyName, EntityPositionSelector defaultValue = null)
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
			m_entity = ISingleEntitySelectorUtils.FromJsonProperty(jsonObject, "entity");
		}

		public IEnumerable<Coord> EnumerateCoords(DynamicValueContext context)
		{
			if (TryGetCoord(context, out Coord coord))
			{
				yield return coord;
			}
		}

		public bool TryGetCoord(DynamicValueContext context, out Coord coord)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			if (m_entity.TryGetEntity(context, out IEntityWithBoardPresence entity))
			{
				coord = new Coord(entity.area.refCoord);
				return true;
			}
			coord = default(Coord);
			return false;
		}
	}
}
