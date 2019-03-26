using Ankama.Cube.Data;
using Ankama.Cube.Fight;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Fight.Movement;
using Ankama.Cube.Maps.Objects;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Maps
{
	public class FightMapMovementContext
	{
		[Flags]
		public enum CellState
		{
			None = 0x0,
			Movement = 0x1,
			Reachable = 0x2,
			Occupied = 0x4,
			Targetable = 0x8,
			Targeted = 0x10,
			Tracked = 0x20
		}

		public struct Cell
		{
			public readonly Vector2Int coords;

			public readonly CellState state;

			public readonly IEntityWithBoardPresence entity;

			public Cell(Vector2Int coords, CellState state, IEntityWithBoardPresence entity)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				this.coords = coords;
				this.state = state;
				this.entity = entity;
			}
		}

		public readonly Cell[] grid;

		public readonly IMapStateProvider stateProvider;

		public readonly IMapEntityProvider entityProvider;

		private bool m_hasEnded;

		private readonly HashSet<IObjectTargetableByAction> m_objectsTargetableByAction = new HashSet<IObjectTargetableByAction>();

		public ICharacterEntity trackedCharacter
		{
			get;
			private set;
		}

		public IEntityWithBoardPresence targetedEntity
		{
			get;
			private set;
		}

		public bool canMove
		{
			get;
			private set;
		}

		public bool canPassThrough
		{
			get;
			private set;
		}

		public bool canDoActionOnTarget
		{
			get;
			private set;
		}

		public bool hasEnded
		{
			get
			{
				bool hasEnded = m_hasEnded;
				m_hasEnded = false;
				return hasEnded;
			}
		}

		public FightMapMovementContext([NotNull] IMapStateProvider stateProvider, [NotNull] IMapEntityProvider entityProvider)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			this.stateProvider = stateProvider;
			this.entityProvider = entityProvider;
			Vector2Int sizeMin = stateProvider.sizeMin;
			Vector2Int val = stateProvider.sizeMax - sizeMin;
			int num = val.get_y() * val.get_x();
			int num2 = sizeMin.get_y() * val.get_x() + sizeMin.get_x();
			grid = new Cell[num];
			Vector2Int coords = default(Vector2Int);
			for (int i = 0; i < num; i++)
			{
				FightCellState cellState = stateProvider.GetCellState(i);
				int num3 = sizeMin.get_x() + i % val.get_x();
				int num4 = (i + num2 - num3) / val.get_x();
				coords._002Ector(num3, num4);
				CellState state;
				switch (cellState)
				{
				case FightCellState.None:
					state = CellState.None;
					break;
				case FightCellState.Movement:
					state = CellState.Movement;
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
				grid[i] = new Cell(coords, state, null);
			}
		}

		public Cell GetCell(Vector2Int coords)
		{
			int cellIndex = stateProvider.GetCellIndex(coords.get_x(), coords.get_y());
			return grid[cellIndex];
		}

		public bool Contains(Vector2Int coords)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			Vector2Int sizeMin = stateProvider.sizeMin;
			Vector2Int sizeMax = stateProvider.sizeMax;
			if (coords.get_x() >= sizeMin.get_x() && coords.get_x() < sizeMax.get_x() && coords.get_y() >= sizeMin.get_y())
			{
				return coords.get_y() < sizeMax.get_y();
			}
			return false;
		}

		public void Begin([NotNull] ICharacterEntity tracked, FightPathFinder pathFinder)
		{
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_0203: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			IMapStateProvider mapStateProvider = stateProvider;
			IMapEntityProvider mapEntityProvider = entityProvider;
			HashSet<IObjectTargetableByAction> objectsTargetableByAction = m_objectsTargetableByAction;
			canMove = tracked.canMove;
			canPassThrough = tracked.HasProperty(PropertyId.CanPassThrough);
			canDoActionOnTarget = tracked.canDoActionOnTarget;
			int num = grid.Length;
			for (int i = 0; i < num; i++)
			{
				Cell cell = grid[i];
				if (cell.state != 0)
				{
					Vector2Int coords = cell.coords;
					IEntityWithBoardPresence entity;
					CellState state = (!mapEntityProvider.TryGetEntityBlockingMovementAt(coords, out entity)) ? CellState.Movement : ((entity != tracked) ? (CellState.Movement | CellState.Occupied) : (CellState.Movement | CellState.Reachable | CellState.Tracked));
					grid[i] = new Cell(cell.coords, state, entity);
				}
			}
			pathFinder.FloodFill(mapStateProvider, grid, tracked.area.refCoord, tracked.movementPoints, canPassThrough);
			ActionType actionType = tracked.actionType;
			IEntitySelector customActionTarget = tracked.customActionTarget;
			if (customActionTarget != null)
			{
				CharacterActionValueContext context = new CharacterActionValueContext((FightStatus)mapEntityProvider, tracked);
				foreach (IEntity item in customActionTarget.EnumerateEntities(context))
				{
					IEntityTargetableByAction entityTargetableByAction;
					if (item != tracked && (entityTargetableByAction = (item as IEntityTargetableByAction)) != null)
					{
						Vector2Int refCoord = entityTargetableByAction.area.refCoord;
						if (IsInActionRange(refCoord, tracked))
						{
							IObjectTargetableByAction objectTargetableByAction;
							if ((objectTargetableByAction = (entityTargetableByAction.view as IObjectTargetableByAction)) != null)
							{
								objectTargetableByAction.ShowActionTargetFeedback(actionType, isSelected: false);
								objectsTargetableByAction.Add(objectTargetableByAction);
							}
							int cellIndex = mapStateProvider.GetCellIndex(refCoord.get_x(), refCoord.get_y());
							Cell cell2 = grid[cellIndex];
							grid[cellIndex] = new Cell(refCoord, cell2.state | CellState.Targetable, entityTargetableByAction);
						}
					}
				}
			}
			else
			{
				foreach (IEntityTargetableByAction item2 in mapEntityProvider.EnumerateEntities<IEntityTargetableByAction>())
				{
					if (item2 != tracked)
					{
						Vector2Int refCoord2 = item2.area.refCoord;
						if (IsInActionRange(refCoord2, tracked))
						{
							IObjectTargetableByAction objectTargetableByAction2;
							if ((objectTargetableByAction2 = (item2.view as IObjectTargetableByAction)) != null)
							{
								objectTargetableByAction2.ShowActionTargetFeedback(actionType, isSelected: false);
								objectsTargetableByAction.Add(objectTargetableByAction2);
							}
							int cellIndex2 = mapStateProvider.GetCellIndex(refCoord2.get_x(), refCoord2.get_y());
							Cell cell3 = grid[cellIndex2];
							grid[cellIndex2] = new Cell(refCoord2, cell3.state | CellState.Targetable, item2);
						}
					}
				}
			}
			trackedCharacter = tracked;
			targetedEntity = null;
		}

		public void UpdateTarget([CanBeNull] IEntityWithBoardPresence targeted)
		{
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			if (targetedEntity == targeted)
			{
				return;
			}
			if (targetedEntity != null)
			{
				IObjectTargetableByAction objectTargetableByAction;
				if ((objectTargetableByAction = (targetedEntity.view as IObjectTargetableByAction)) != null)
				{
					objectTargetableByAction.ShowActionTargetFeedback(trackedCharacter.actionType, isSelected: false);
					m_objectsTargetableByAction.Add(objectTargetableByAction);
				}
				Vector2Int refCoord = targetedEntity.area.refCoord;
				int cellIndex = stateProvider.GetCellIndex(refCoord.get_x(), refCoord.get_y());
				Cell cell = grid[cellIndex];
				grid[cellIndex] = new Cell(refCoord, cell.state & ~CellState.Targeted, targetedEntity);
			}
			if (targeted != null)
			{
				IObjectTargetableByAction objectTargetableByAction2;
				if ((objectTargetableByAction2 = (targeted.view as IObjectTargetableByAction)) != null)
				{
					objectTargetableByAction2.ShowActionTargetFeedback(trackedCharacter.actionType, isSelected: true);
					m_objectsTargetableByAction.Add(objectTargetableByAction2);
				}
				Vector2Int refCoord2 = targeted.area.refCoord;
				int cellIndex2 = stateProvider.GetCellIndex(refCoord2.get_x(), refCoord2.get_y());
				Cell cell2 = grid[cellIndex2];
				grid[cellIndex2] = new Cell(refCoord2, cell2.state | CellState.Targeted, targeted);
			}
			targetedEntity = targeted;
		}

		public void End()
		{
			foreach (IObjectTargetableByAction item in m_objectsTargetableByAction)
			{
				item?.HideActionTargetFeedback();
			}
			m_objectsTargetableByAction.Clear();
			m_hasEnded = true;
			trackedCharacter = null;
			targetedEntity = null;
		}

		public bool IsInActionRange(Vector2Int coord, ICharacterEntity tracked)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			int num = tracked.area.MinDistanceWith(coord);
			if (tracked.hasRange)
			{
				if (num >= tracked.rangeMin)
				{
					return num <= tracked.rangeMax;
				}
				return false;
			}
			if (num == 1)
			{
				return true;
			}
			IMapStateProvider mapStateProvider = stateProvider;
			Vector2Int sizeMin = mapStateProvider.sizeMin;
			Vector2Int sizeMax = mapStateProvider.sizeMax;
			int x = sizeMin.get_x();
			int y = sizeMin.get_y();
			int x2 = sizeMax.get_x();
			int y2 = sizeMax.get_y();
			Vector2Int val = default(Vector2Int);
			val._002Ector(coord.get_x(), coord.get_y() + 1);
			if (val.get_x() >= x && val.get_x() < x2 && val.get_y() >= y && val.get_y() < y2)
			{
				int cellIndex = mapStateProvider.GetCellIndex(val.get_x(), val.get_y());
				if ((grid[cellIndex].state & (CellState.Reachable | CellState.Occupied)) == CellState.Reachable)
				{
					return true;
				}
			}
			Vector2Int val2 = default(Vector2Int);
			val2._002Ector(coord.get_x() - 1, coord.get_y());
			if (val2.get_x() >= x && val2.get_x() < x2 && val2.get_y() >= y && val2.get_y() < y2)
			{
				int cellIndex2 = mapStateProvider.GetCellIndex(val2.get_x(), val2.get_y());
				if ((grid[cellIndex2].state & (CellState.Reachable | CellState.Occupied)) == CellState.Reachable)
				{
					return true;
				}
			}
			Vector2Int val3 = default(Vector2Int);
			val3._002Ector(coord.get_x() + 1, coord.get_y());
			if (val3.get_x() >= x && val3.get_x() < x2 && val3.get_y() >= y && val3.get_y() < y2)
			{
				int cellIndex3 = mapStateProvider.GetCellIndex(val3.get_x(), val3.get_y());
				if ((grid[cellIndex3].state & (CellState.Reachable | CellState.Occupied)) == CellState.Reachable)
				{
					return true;
				}
			}
			Vector2Int val4 = default(Vector2Int);
			val4._002Ector(coord.get_x(), coord.get_y() - 1);
			if (val4.get_x() >= x && val4.get_x() < x2 && val4.get_y() >= y && val4.get_y() < y2)
			{
				int cellIndex4 = mapStateProvider.GetCellIndex(val4.get_x(), val4.get_y());
				if ((grid[cellIndex4].state & (CellState.Reachable | CellState.Occupied)) == CellState.Reachable)
				{
					return true;
				}
			}
			return false;
		}
	}
}
