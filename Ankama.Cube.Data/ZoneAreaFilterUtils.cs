using Ankama.Cube.Fight.Entities;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	public static class ZoneAreaFilterUtils
	{
		public static bool SingleTargetToCompareArea(ISingleTargetSelector targetToCompare, DynamicValueContext context, out Area area)
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			ISingleEntitySelector singleEntitySelector = targetToCompare as ISingleEntitySelector;
			if (singleEntitySelector != null && singleEntitySelector.TryGetEntity(context, out IEntityWithBoardPresence entity))
			{
				area = entity.area;
				return true;
			}
			ISingleCoordSelector singleCoordSelector = targetToCompare as ISingleCoordSelector;
			if (singleCoordSelector != null && singleCoordSelector.TryGetCoord(context, out Coord coord))
			{
				area = new PointArea((Vector2Int)coord);
				return true;
			}
			area = null;
			return false;
		}

		public static IEnumerable<Area> TargetsToCompareAreaList(ITargetSelector targetToCompare, DynamicValueContext context)
		{
			IEntitySelector entitySelector = targetToCompare as IEntitySelector;
			if (entitySelector != null)
			{
				foreach (IEntity item in entitySelector.EnumerateEntities(context))
				{
					IEntityWithBoardPresence entityWithBoardPresence = item as IEntityWithBoardPresence;
					if (entityWithBoardPresence != null)
					{
						yield return entityWithBoardPresence.area;
					}
				}
			}
			ICoordSelector coordSelector = targetToCompare as ICoordSelector;
			if (coordSelector != null)
			{
				foreach (Coord item2 in coordSelector.EnumerateCoords(context))
				{
					yield return new PointArea((Vector2Int)item2);
				}
			}
		}
	}
}
