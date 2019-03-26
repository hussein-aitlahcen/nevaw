using Ankama.Cube.Fight.Entities;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class CentralSymmetryCoordSelector : IEditableContent, ITargetSelector, ISingleCoordSelector, ICoordSelector, ISingleTargetSelector
	{
		private ISingleTargetSelector m_symmetryCenter;

		private ISingleTargetSelector m_refCoords;

		public ISingleTargetSelector symmetryCenter => m_symmetryCenter;

		public ISingleTargetSelector refCoords => m_refCoords;

		public override string ToString()
		{
			return GetType().Name;
		}

		public static CentralSymmetryCoordSelector FromJsonToken(JToken token)
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
			CentralSymmetryCoordSelector centralSymmetryCoordSelector = new CentralSymmetryCoordSelector();
			centralSymmetryCoordSelector.PopulateFromJson(jsonObject);
			return centralSymmetryCoordSelector;
		}

		public static CentralSymmetryCoordSelector FromJsonProperty(JObject jsonObject, string propertyName, CentralSymmetryCoordSelector defaultValue = null)
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
			m_symmetryCenter = ISingleTargetSelectorUtils.FromJsonProperty(jsonObject, "symmetryCenter");
			m_refCoords = ISingleTargetSelectorUtils.FromJsonProperty(jsonObject, "refCoords");
		}

		public IEnumerable<Coord> EnumerateCoords(DynamicValueContext context)
		{
			Coord? coord = TryGetCoord(m_refCoords, context);
			if (coord.HasValue)
			{
				Coord? coord2 = TryGetCoord(m_symmetryCenter, context);
				if (coord2.HasValue)
				{
					Coord value = coord.Value;
					Coord value2 = coord2.Value;
					yield return new Coord(2 * value2.x - value.x, 2 * value2.y - value.y);
				}
			}
		}

		public bool TryGetCoord(DynamicValueContext context, out Coord coord)
		{
			coord = default(Coord);
			return false;
		}

		private Coord? TryGetCoord(ISingleTargetSelector selector, DynamicValueContext context)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			ISingleEntitySelector singleEntitySelector = selector as ISingleEntitySelector;
			if (singleEntitySelector != null)
			{
				if (singleEntitySelector.TryGetEntity(context, out IEntityWithBoardPresence entity))
				{
					return new Coord(entity.area.refCoord);
				}
				return null;
			}
			ISingleCoordSelector singleCoordSelector = selector as ISingleCoordSelector;
			if (singleCoordSelector != null)
			{
				if (singleCoordSelector.TryGetCoord(context, out Coord coord))
				{
					return coord;
				}
				return null;
			}
			return null;
		}
	}
}
