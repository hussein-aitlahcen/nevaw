using Ankama.Cube.Data;
using Ankama.Cube.Extensions;
using Ankama.Cube.Maps;
using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Fight.Movement
{
	public class FightPathFinder
	{
		private struct Node
		{
			public readonly Vector2Int coords;

			public readonly Vector2Int fromCoords;

			public readonly int cost;

			public readonly int priority;

			public readonly Direction direction;

			public Node(Vector2Int coords, Vector2Int fromCoords, int cost, int priority, Direction direction)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				this.coords = coords;
				this.fromCoords = fromCoords;
				this.cost = cost;
				this.priority = priority;
				this.direction = direction;
			}
		}

		private struct AdjacentCoord
		{
			public readonly Vector2Int coords;

			public readonly bool isValid;

			public AdjacentCoord(Vector2Int coords, bool isValid)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				this.coords = coords;
				this.isValid = isValid;
			}
		}

		private struct FloodFillNode
		{
			public readonly Vector2Int coords;

			public readonly int cost;

			public FloodFillNode(Vector2Int coords, int cost)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				this.coords = coords;
				this.cost = cost;
			}
		}

		private class NodePriorityQueue : IComparer<Node>
		{
			private readonly List<Node> m_data;

			public NodePriorityQueue(int capacity)
			{
				m_data = new List<Node>(capacity);
			}

			public void Enqueue(Node item)
			{
				List<Node> data = m_data;
				data.Add(item);
				int num = data.Count - 1;
				while (num > 0)
				{
					int num2 = (num - 1) / 2;
					if (Compare(data[num], data[num2]) < 0)
					{
						Node value = data[num];
						data[num] = data[num2];
						data[num2] = value;
						num = num2;
						continue;
					}
					break;
				}
			}

			public Node Dequeue()
			{
				List<Node> data = m_data;
				int num = data.Count - 1;
				Node result = data[0];
				data[0] = data[num];
				data.RemoveAt(num);
				num--;
				int num2 = 0;
				while (true)
				{
					int num3 = num2 * 2 + 1;
					if (num3 > num)
					{
						break;
					}
					int num4 = num3 + 1;
					if (num4 <= num && Compare(data[num4], data[num3]) < 0)
					{
						num3 = num4;
					}
					if (Compare(data[num2], data[num3]) <= 0)
					{
						break;
					}
					Node value = data[num2];
					data[num2] = data[num3];
					data[num3] = value;
					num2 = num3;
				}
				return result;
			}

			public int Count()
			{
				return m_data.Count;
			}

			public void Clear()
			{
				m_data.Clear();
			}

			[Pure]
			public int Compare(Node x, Node y)
			{
				int priority = x.priority;
				int priority2 = y.priority;
				if (priority < priority2)
				{
					return -1;
				}
				if (priority > priority2)
				{
					return 1;
				}
				return y.cost.CompareTo(x.cost);
			}
		}

		private readonly NodePriorityQueue m_frontier;

		private readonly Dictionary<int, Node> m_steps;

		private readonly List<Node> m_previousStepsBuffer;

		private readonly AdjacentCoord[] m_adjacentCoordsBuffer;

		private readonly Queue<FloodFillNode> m_floodFillFrontier;

		private int m_movementPoints;

		private bool m_canPassThrough;

		public bool tracking
		{
			get;
			private set;
		}

		public List<Vector2Int> currentPath
		{
			get;
		}

		public FightPathFinder(int pathCapacity = 8, int priorityQueueCapacity = 16)
		{
			currentPath = new List<Vector2Int>(pathCapacity);
			m_frontier = new NodePriorityQueue(priorityQueueCapacity);
			m_steps = new Dictionary<int, Node>(priorityQueueCapacity);
			m_previousStepsBuffer = new List<Node>(pathCapacity);
			m_adjacentCoordsBuffer = new AdjacentCoord[4];
			m_floodFillFrontier = new Queue<FloodFillNode>(32);
		}

		public void FloodFill(IMapStateProvider mapStateProvider, FightMapMovementContext.Cell[] grid, Vector2Int position, int movementPoints, bool canPassThrough)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_021e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0243: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
			Queue<FloodFillNode> floodFillFrontier = m_floodFillFrontier;
			Vector2Int sizeMin = mapStateProvider.sizeMin;
			Vector2Int sizeMax = mapStateProvider.sizeMax;
			floodFillFrontier.Clear();
			FloodFillNode item = new FloodFillNode(position, 0);
			floodFillFrontier.Enqueue(item);
			Vector2Int coords2 = default(Vector2Int);
			Vector2Int coords3 = default(Vector2Int);
			Vector2Int coords4 = default(Vector2Int);
			Vector2Int coords5 = default(Vector2Int);
			do
			{
				FloodFillNode floodFillNode = floodFillFrontier.Dequeue();
				Vector2Int coords = floodFillNode.coords;
				int cost = floodFillNode.cost;
				if (cost >= movementPoints)
				{
					continue;
				}
				int cost2 = cost + 1;
				int x = coords.get_x();
				int y = coords.get_y();
				int x2 = sizeMin.get_x();
				int y2 = sizeMin.get_y();
				int x3 = sizeMax.get_x();
				int y3 = sizeMax.get_y();
				coords2._002Ector(x, y + 1);
				coords3._002Ector(x - 1, y);
				coords4._002Ector(x + 1, y);
				coords5._002Ector(x, y - 1);
				int x4 = coords2.get_x();
				int y4 = coords2.get_y();
				if (x4 >= x2 && x4 < x3 && y4 >= y2 && y4 < y3)
				{
					int cellIndex = mapStateProvider.GetCellIndex(x4, y4);
					FightMapMovementContext.Cell cell = grid[cellIndex];
					FightMapMovementContext.CellState state = cell.state;
					if ((state & (FightMapMovementContext.CellState.Movement | FightMapMovementContext.CellState.Reachable)) == FightMapMovementContext.CellState.Movement)
					{
						grid[cellIndex] = new FightMapMovementContext.Cell(coords2, state | FightMapMovementContext.CellState.Reachable, cell.entity);
						if (((state & FightMapMovementContext.CellState.Occupied) == FightMapMovementContext.CellState.None) | canPassThrough)
						{
							FloodFillNode item2 = new FloodFillNode(coords2, cost2);
							floodFillFrontier.Enqueue(item2);
						}
					}
				}
				int x5 = coords3.get_x();
				int y5 = coords3.get_y();
				if (x5 >= x2 && x5 < x3 && y5 >= y2 && y5 < y3)
				{
					int cellIndex2 = mapStateProvider.GetCellIndex(x5, y5);
					FightMapMovementContext.Cell cell2 = grid[cellIndex2];
					FightMapMovementContext.CellState state2 = cell2.state;
					if ((state2 & (FightMapMovementContext.CellState.Movement | FightMapMovementContext.CellState.Reachable)) == FightMapMovementContext.CellState.Movement)
					{
						grid[cellIndex2] = new FightMapMovementContext.Cell(coords3, state2 | FightMapMovementContext.CellState.Reachable, cell2.entity);
						if (((state2 & FightMapMovementContext.CellState.Occupied) == FightMapMovementContext.CellState.None) | canPassThrough)
						{
							FloodFillNode item3 = new FloodFillNode(coords3, cost2);
							floodFillFrontier.Enqueue(item3);
						}
					}
				}
				int x6 = coords4.get_x();
				int y6 = coords4.get_y();
				if (x6 >= x2 && x6 < x3 && y6 >= y2 && y6 < y3)
				{
					int cellIndex3 = mapStateProvider.GetCellIndex(x6, y6);
					FightMapMovementContext.Cell cell3 = grid[cellIndex3];
					FightMapMovementContext.CellState state3 = cell3.state;
					if ((state3 & (FightMapMovementContext.CellState.Movement | FightMapMovementContext.CellState.Reachable)) == FightMapMovementContext.CellState.Movement)
					{
						grid[cellIndex3] = new FightMapMovementContext.Cell(coords4, state3 | FightMapMovementContext.CellState.Reachable, cell3.entity);
						if (((state3 & FightMapMovementContext.CellState.Occupied) == FightMapMovementContext.CellState.None) | canPassThrough)
						{
							FloodFillNode item4 = new FloodFillNode(coords4, cost2);
							floodFillFrontier.Enqueue(item4);
						}
					}
				}
				int x7 = coords5.get_x();
				int y7 = coords5.get_y();
				if (x7 < x2 || x7 >= x3 || y7 < y2 || y7 >= y3)
				{
					continue;
				}
				int cellIndex4 = mapStateProvider.GetCellIndex(x7, y7);
				FightMapMovementContext.Cell cell4 = grid[cellIndex4];
				FightMapMovementContext.CellState state4 = cell4.state;
				if ((state4 & (FightMapMovementContext.CellState.Movement | FightMapMovementContext.CellState.Reachable)) == FightMapMovementContext.CellState.Movement)
				{
					grid[cellIndex4] = new FightMapMovementContext.Cell(coords5, state4 | FightMapMovementContext.CellState.Reachable, cell4.entity);
					if (((state4 & FightMapMovementContext.CellState.Occupied) == FightMapMovementContext.CellState.None) | canPassThrough)
					{
						FloodFillNode item5 = new FloodFillNode(coords5, cost2);
						floodFillFrontier.Enqueue(item5);
					}
				}
			}
			while (floodFillFrontier.Count > 0);
		}

		public void Begin(Vector2Int position, int movementPoints, bool canPassThrough)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			m_movementPoints = movementPoints;
			m_canPassThrough = canPassThrough;
			currentPath.Clear();
			currentPath.Add(position);
			tracking = true;
		}

		public void Move(IMapStateProvider mapStateProvider, FightMapMovementContext.Cell[] grid, Vector2Int position, bool isTargeting)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			List<Vector2Int> currentPath = this.currentPath;
			int count = currentPath.Count;
			int movementPoints = m_movementPoints;
			if (isTargeting)
			{
				movementPoints++;
				if (currentPath[0].DistanceTo(position) > movementPoints)
				{
					currentPath.RemoveRange(1, count - 1);
					return;
				}
				Vector2Int val = currentPath[count - 1];
				if (val.DistanceTo(position) != 1 || !CanStopAt(mapStateProvider, grid, val))
				{
					if (count == 1 || !AppendPartialPath(mapStateProvider, grid, position, movementPoints, isTargeting: true))
					{
						ComputeFullPath(mapStateProvider, grid, currentPath[0], position, movementPoints, isTargeting: true);
					}
					count = currentPath.Count;
					if (count > 1)
					{
						currentPath.RemoveAt(count - 1);
					}
				}
				return;
			}
			if (currentPath[0].DistanceTo(position) > movementPoints)
			{
				currentPath.RemoveRange(1, count - 1);
				return;
			}
			for (int i = 0; i < count; i++)
			{
				if (currentPath[i] == position)
				{
					currentPath.RemoveRange(i + 1, count - 1 - i);
					return;
				}
			}
			if (count <= movementPoints && currentPath[count - 1].DistanceTo(position) == 1)
			{
				currentPath.Add(position);
			}
			else if (count == 1 || !AppendPartialPath(mapStateProvider, grid, position, movementPoints, isTargeting: false))
			{
				ComputeFullPath(mapStateProvider, grid, currentPath[0], position, movementPoints, isTargeting: false);
			}
		}

		public void Reset()
		{
			currentPath.RemoveRange(1, currentPath.Count - 1);
		}

		public void End()
		{
			tracking = false;
		}

		private bool AppendPartialPath(IMapStateProvider mapStateProvider, FightMapMovementContext.Cell[] grid, Vector2Int end, int movementPoints, bool isTargeting)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0223: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			//IL_025d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0278: Unknown result type (might be due to invalid IL or missing references)
			//IL_027a: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02af: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_030f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0311: Unknown result type (might be due to invalid IL or missing references)
			List<Vector2Int> currentPath = this.currentPath;
			List<Node> previousStepsBuffer = m_previousStepsBuffer;
			NodePriorityQueue frontier = m_frontier;
			Dictionary<int, Node> steps = m_steps;
			AdjacentCoord[] adjacentCoordsBuffer = m_adjacentCoordsBuffer;
			int num = currentPath.Count;
			Vector2Int val = mapStateProvider.sizeMax;
			int x = val.get_x();
			val = mapStateProvider.sizeMin;
			int num2 = x - val.get_x();
			if (previousStepsBuffer.Capacity < num)
			{
				previousStepsBuffer.Capacity = num;
			}
			previousStepsBuffer.Clear();
			Vector2Int val2 = currentPath[0];
			previousStepsBuffer.Add(new Node(val2, val2, 0, 0, val2.GetDirectionTo(end)));
			for (int i = 1; i < num; i++)
			{
				Vector2Int val3 = currentPath[i];
				previousStepsBuffer.Add(new Node(val3, val2, i, 0, val2.GetDirectionTo(val3)));
				val2 = val3;
			}
			for (int num3 = num - 1; num3 > 0; num3--)
			{
				Node node = previousStepsBuffer[num3];
				int num4 = node.coords.DistanceTo(end);
				if (num3 + num4 <= movementPoints)
				{
					frontier.Clear();
					frontier.Enqueue(node);
					steps.Clear();
					for (int j = 0; j < num3; j++)
					{
						Node node2 = previousStepsBuffer[j];
						Vector2Int coords = node2.coords;
						int key = coords.get_y() * num2 + coords.get_x();
						steps.Add(key, node2);
					}
					do
					{
						Node node3 = frontier.Dequeue();
						Vector2Int coords2 = node3.coords;
						Vector2Int fromCoords = node3.fromCoords;
						int cost = node3.cost;
						Direction direction = node3.direction;
						if (node3.coords == end)
						{
							if (isTargeting && m_canPassThrough && !CanStopAt(mapStateProvider, grid, fromCoords))
							{
								int key2 = coords2.get_y() * num2 + coords2.get_x();
								steps[key2] = new Node(coords2, fromCoords, int.MaxValue, node3.priority, direction);
								continue;
							}
							int num5 = cost + 1;
							if (currentPath.Capacity < num5)
							{
								currentPath.Capacity = num5;
							}
							if (num > num5)
							{
								currentPath.RemoveRange(num5, num - num5);
							}
							else if (num < num5)
							{
								for (int k = num; k < num5; k++)
								{
									currentPath.Add(end);
								}
							}
							currentPath[cost] = end;
							for (int num6 = cost - 1; num6 >= num; num6--)
							{
								Vector2Int fromCoords2 = node3.fromCoords;
								int key3 = fromCoords2.get_y() * num2 + fromCoords2.get_x();
								node3 = steps[key3];
								currentPath[num6] = node3.coords;
							}
							return true;
						}
						ComputeAdjacentCoords(mapStateProvider, grid, coords2, fromCoords);
						for (int l = 0; l < 4; l++)
						{
							AdjacentCoord adjacentCoord = adjacentCoordsBuffer[l];
							if (!adjacentCoord.isValid)
							{
								continue;
							}
							Vector2Int coords3 = adjacentCoord.coords;
							int num7 = cost + 1;
							int num8 = coords3.DistanceTo(end);
							if (num7 + num8 <= movementPoints)
							{
								int key4 = coords3.get_y() * num2 + coords3.get_x();
								if (!steps.TryGetValue(key4, out Node value) || value.cost >= num7)
								{
									Direction directionTo = coords2.GetDirectionTo(coords3);
									int num9 = (directionTo != direction) ? 1 : 0;
									int num10 = (num8 << 1) + num9;
									value = new Node(coords3, coords2, num7, num7 + num10, directionTo);
									frontier.Enqueue(value);
									steps[key4] = value;
								}
							}
						}
					}
					while (frontier.Count() > 0);
				}
				currentPath.RemoveAt(num3);
				num--;
			}
			return false;
		}

		private void ComputeFullPath(IMapStateProvider mapStateProvider, FightMapMovementContext.Cell[] grid, Vector2Int start, Vector2Int end, int movementPoints, bool isTargeting)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_024b: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			//IL_028e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			List<Vector2Int> currentPath = this.currentPath;
			AdjacentCoord[] adjacentCoordsBuffer = m_adjacentCoordsBuffer;
			NodePriorityQueue frontier = m_frontier;
			Dictionary<int, Node> steps = m_steps;
			int count = currentPath.Count;
			Vector2Int val = mapStateProvider.sizeMax;
			int x = val.get_x();
			val = mapStateProvider.sizeMin;
			int num = x - val.get_x();
			frontier.Clear();
			steps.Clear();
			if (start.DistanceTo(end) <= movementPoints)
			{
				Direction directionTo = start.GetDirectionTo(end);
				Node item = new Node(start, start, 0, 0, directionTo);
				frontier.Enqueue(item);
				do
				{
					Node node = frontier.Dequeue();
					Vector2Int coords = node.coords;
					Vector2Int fromCoords = node.fromCoords;
					int cost = node.cost;
					Direction direction = node.direction;
					if (coords == end)
					{
						if (isTargeting && m_canPassThrough && !CanStopAt(mapStateProvider, grid, fromCoords))
						{
							int key = coords.get_y() * num + coords.get_x();
							steps[key] = new Node(coords, fromCoords, int.MaxValue, node.priority, direction);
							continue;
						}
						int num2 = cost + 1;
						if (currentPath.Capacity < num2)
						{
							currentPath.Capacity = num2;
						}
						if (count > num2)
						{
							currentPath.RemoveRange(num2, count - num2);
						}
						else if (count < num2)
						{
							for (int i = count; i < num2; i++)
							{
								currentPath.Add(end);
							}
						}
						currentPath[cost] = end;
						for (int num3 = cost - 1; num3 > 0; num3--)
						{
							Vector2Int fromCoords2 = node.fromCoords;
							int key2 = fromCoords2.get_y() * num + fromCoords2.get_x();
							node = steps[key2];
							currentPath[num3] = node.coords;
						}
						currentPath[0] = start;
						return;
					}
					ComputeAdjacentCoords(mapStateProvider, grid, coords, fromCoords);
					for (int j = 0; j < 4; j++)
					{
						AdjacentCoord adjacentCoord = adjacentCoordsBuffer[j];
						if (!adjacentCoord.isValid)
						{
							continue;
						}
						Vector2Int coords2 = adjacentCoord.coords;
						int num4 = cost + 1;
						int num5 = coords2.DistanceTo(end);
						if (num4 + num5 <= movementPoints)
						{
							int key3 = coords2.get_y() * num + coords2.get_x();
							if (!steps.TryGetValue(key3, out Node value) || value.cost >= num4)
							{
								Direction directionTo2 = coords.GetDirectionTo(coords2);
								int num6 = (directionTo2 != direction) ? 1 : 0;
								int num7 = (num5 << 1) + num6;
								value = new Node(coords2, coords, num4, num4 + num7, directionTo2);
								frontier.Enqueue(value);
								steps[key3] = value;
							}
						}
					}
				}
				while (frontier.Count() > 0);
			}
			if (count == 0)
			{
				currentPath.Add(start);
				return;
			}
			currentPath[0] = start;
			currentPath.RemoveRange(1, count - 1);
		}

		private void ComputeAdjacentCoords(IMapStateProvider mapStateProvider, FightMapMovementContext.Cell[] grid, Vector2Int coords, Vector2Int from)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_024e: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_026e: Unknown result type (might be due to invalid IL or missing references)
			//IL_027d: Unknown result type (might be due to invalid IL or missing references)
			Vector2Int sizeMin = mapStateProvider.sizeMin;
			Vector2Int sizeMax = mapStateProvider.sizeMax;
			int x = coords.get_x();
			int y = coords.get_y();
			int x2 = sizeMin.get_x();
			int y2 = sizeMin.get_y();
			int x3 = sizeMax.get_x();
			int y3 = sizeMax.get_y();
			bool canPassThrough = m_canPassThrough;
			Vector2Int val = default(Vector2Int);
			val._002Ector(x, y + 1);
			Vector2Int val2 = default(Vector2Int);
			val2._002Ector(x - 1, y);
			Vector2Int val3 = default(Vector2Int);
			val3._002Ector(x + 1, y);
			Vector2Int val4 = default(Vector2Int);
			val4._002Ector(x, y - 1);
			int x4 = val.get_x();
			int y4 = val.get_y();
			bool isValid;
			if (val != from && x4 >= x2 && x4 < x3 && y4 >= y2 && y4 < y3)
			{
				int cellIndex = mapStateProvider.GetCellIndex(x4, y4);
				FightMapMovementContext.CellState state = grid[cellIndex].state;
				isValid = (((state & FightMapMovementContext.CellState.Reachable) != 0 && (((state & FightMapMovementContext.CellState.Occupied) == FightMapMovementContext.CellState.None) | canPassThrough)) || (state & FightMapMovementContext.CellState.Targeted) != FightMapMovementContext.CellState.None);
			}
			else
			{
				isValid = false;
			}
			int x5 = val2.get_x();
			int y5 = val2.get_y();
			bool isValid2;
			if (val2 != from && x5 >= x2 && x5 < x3 && y5 >= y2 && y5 < y3)
			{
				int cellIndex2 = mapStateProvider.GetCellIndex(x5, y5);
				FightMapMovementContext.CellState state2 = grid[cellIndex2].state;
				isValid2 = (((state2 & FightMapMovementContext.CellState.Reachable) != 0 && (((state2 & FightMapMovementContext.CellState.Occupied) == FightMapMovementContext.CellState.None) | canPassThrough)) || (state2 & FightMapMovementContext.CellState.Targeted) != FightMapMovementContext.CellState.None);
			}
			else
			{
				isValid2 = false;
			}
			int x6 = val3.get_x();
			int y6 = val3.get_y();
			bool isValid3;
			if (val3 != from && x6 >= x2 && x6 < x3 && y6 >= y2 && y6 < y3)
			{
				int cellIndex3 = mapStateProvider.GetCellIndex(x6, y6);
				FightMapMovementContext.CellState state3 = grid[cellIndex3].state;
				isValid3 = (((state3 & FightMapMovementContext.CellState.Reachable) != 0 && (((state3 & FightMapMovementContext.CellState.Occupied) == FightMapMovementContext.CellState.None) | canPassThrough)) || (state3 & FightMapMovementContext.CellState.Targeted) != FightMapMovementContext.CellState.None);
			}
			else
			{
				isValid3 = false;
			}
			int x7 = val4.get_x();
			int y7 = val4.get_y();
			bool isValid4;
			if (val4 != from && x7 >= x2 && x7 < x3 && y7 >= y2 && y7 < y3)
			{
				int cellIndex4 = mapStateProvider.GetCellIndex(x7, y7);
				FightMapMovementContext.CellState state4 = grid[cellIndex4].state;
				isValid4 = (((state4 & FightMapMovementContext.CellState.Reachable) != 0 && (((state4 & FightMapMovementContext.CellState.Occupied) == FightMapMovementContext.CellState.None) | canPassThrough)) || (state4 & FightMapMovementContext.CellState.Targeted) != FightMapMovementContext.CellState.None);
			}
			else
			{
				isValid4 = false;
			}
			AdjacentCoord[] adjacentCoordsBuffer = m_adjacentCoordsBuffer;
			adjacentCoordsBuffer[0] = new AdjacentCoord(val, isValid);
			adjacentCoordsBuffer[1] = new AdjacentCoord(val2, isValid2);
			adjacentCoordsBuffer[2] = new AdjacentCoord(val3, isValid3);
			adjacentCoordsBuffer[3] = new AdjacentCoord(val4, isValid4);
		}

		private static bool CanStopAt(IMapStateProvider mapStateProvider, FightMapMovementContext.Cell[] grid, Vector2Int coords)
		{
			int cellIndex = mapStateProvider.GetCellIndex(coords.get_x(), coords.get_y());
			return (grid[cellIndex].state & FightMapMovementContext.CellState.Occupied) == FightMapMovementContext.CellState.None;
		}
	}
}
