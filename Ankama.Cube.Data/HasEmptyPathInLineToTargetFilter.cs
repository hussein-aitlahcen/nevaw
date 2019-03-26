using Ankama.Cube.Fight;
using Ankama.Cube.Fight.Entities;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class HasEmptyPathInLineToTargetFilter : IEditableContent, ICoordOrEntityFilter, ICoordFilter, ITargetFilter, IEntityFilter
	{
		private ISingleTargetSelector m_startCoords;

		public ISingleTargetSelector startCoords => m_startCoords;

		public override string ToString()
		{
			return GetType().Name;
		}

		public static HasEmptyPathInLineToTargetFilter FromJsonToken(JToken token)
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
			HasEmptyPathInLineToTargetFilter hasEmptyPathInLineToTargetFilter = new HasEmptyPathInLineToTargetFilter();
			hasEmptyPathInLineToTargetFilter.PopulateFromJson(jsonObject);
			return hasEmptyPathInLineToTargetFilter;
		}

		public static HasEmptyPathInLineToTargetFilter FromJsonProperty(JObject jsonObject, string propertyName, HasEmptyPathInLineToTargetFilter defaultValue = null)
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
			m_startCoords = ISingleTargetSelectorUtils.FromJsonProperty(jsonObject, "startCoords");
		}

		public IEnumerable<IEntity> Filter(IEnumerable<IEntity> entities, DynamicValueContext context)
		{
			DynamicValueFightContext dynamicValueFightContext = context as DynamicValueFightContext;
			if (dynamicValueFightContext != null && TryGetSrcEntityCoords(context, out Coord srcCoord))
			{
				FightStatus fightStatus = dynamicValueFightContext.fightStatus;
				foreach (IEntity entity in entities)
				{
					IEntityWithBoardPresence entityWithBoardPresence = entity as IEntityWithBoardPresence;
					if (entityWithBoardPresence != null && IsPathEmptyBetween(dest: new Coord(entityWithBoardPresence.area.refCoord), fightStatus: fightStatus, src: srcCoord, context: context))
					{
						yield return entityWithBoardPresence;
					}
				}
			}
		}

		public IEnumerable<Coord> Filter(IEnumerable<Coord> coords, DynamicValueContext context)
		{
			DynamicValueFightContext dynamicValueFightContext = context as DynamicValueFightContext;
			if (dynamicValueFightContext != null && TryGetSrcEntityCoords(context, out Coord srcCoord))
			{
				FightStatus fightStatus = dynamicValueFightContext.fightStatus;
				foreach (Coord coord in coords)
				{
					if (IsPathEmptyBetween(fightStatus, srcCoord, coord, context))
					{
						yield return coord;
					}
				}
			}
		}

		private bool TryGetSrcEntityCoords(DynamicValueContext context, out Coord coords)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			ISingleEntitySelector singleEntitySelector = m_startCoords as ISingleEntitySelector;
			if (singleEntitySelector != null)
			{
				if (singleEntitySelector.TryGetEntity(context, out IEntityWithBoardPresence entity))
				{
					coords = new Coord(entity.area.refCoord);
					return true;
				}
				coords = default(Coord);
				return false;
			}
			ISingleCoordSelector singleCoordSelector = m_startCoords as ISingleCoordSelector;
			if (singleCoordSelector != null)
			{
				return singleCoordSelector.TryGetCoord(context, out coords);
			}
			coords = default(Coord);
			return false;
		}

		private static bool IsPathEmptyBetween(FightStatus fightStatus, Coord src, Coord dest, DynamicValueContext context)
		{
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			if (!src.IsAlignedWith(dest))
			{
				return false;
			}
			FightMapStatus mapStatus = fightStatus.mapStatus;
			foreach (Coord item in src.StraightPathUntil(dest))
			{
				switch (mapStatus.GetCellState(item.x, item.y))
				{
				case FightCellState.None:
					return false;
				default:
					throw new ArgumentOutOfRangeException();
				case FightCellState.Movement:
					break;
				}
				if (fightStatus.HasEntityBlockingMovementAt((Vector2Int)item))
				{
					return false;
				}
			}
			return true;
		}
	}
}
