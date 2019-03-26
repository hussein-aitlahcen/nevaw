using Ankama.Cube.Extensions;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Maps
{
	public class MapPathFinding
	{
		private struct Node
		{
			public Vector2Int coords;

			public Vector2Int fromCoords;

			public int cost;

			public int priority;

			public Node(Vector2Int coord, Vector2Int fromCoord, int cost, int priority)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				coords = coord;
				fromCoords = fromCoord;
				this.cost = cost;
				this.priority = priority;
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

		private Dictionary<int, Node> m_steps = new Dictionary<int, Node>(16);

		private NodePriorityQueue m_frontier = new NodePriorityQueue(16);

		private AdjacentCoord[] m_adjacentCoordsBuffer = new AdjacentCoord[4];

		public bool FindPath(MapData data, Vector3 startWorldPos, Vector3 endWorldPos, List<Vector3> path)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			Vector2Int start = data.WorldToLocalCoord(startWorldPos);
			Vector2Int end = data.WorldToLocalCoord(endWorldPos);
			if (!data.IsInsideLocal(start.get_x(), start.get_y()) || !data.IsInsideLocal(end.get_x(), end.get_y()))
			{
				path.Clear();
				return false;
			}
			int cellIndexLocal = data.GetCellIndexLocal(start.get_x(), start.get_y());
			if (data.cells[cellIndexLocal].state == MapData.CellState.NotWalkable)
			{
				path.Clear();
				return false;
			}
			cellIndexLocal = data.GetCellIndexLocal(end.get_x(), end.get_y());
			if (data.cells[cellIndexLocal].state == MapData.CellState.NotWalkable)
			{
				path.Clear();
				return false;
			}
			return ComputeFullPath(data, start, end, path);
		}

		private bool ComputeFullPath(MapData data, Vector2Int start, Vector2Int end, List<Vector3> path)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			NodePriorityQueue frontier = m_frontier;
			Dictionary<int, Node> steps = m_steps;
			AdjacentCoord[] adjacentCoordsBuffer = m_adjacentCoordsBuffer;
			frontier.Clear();
			steps.Clear();
			Node item = new Node(start, start, 0, 0);
			frontier.Enqueue(item);
			while (frontier.Count() != 0)
			{
				Node node = frontier.Dequeue();
				Vector2Int coords = node.coords;
				Vector2Int fromCoords = node.fromCoords;
				int cost = node.cost;
				if (coords == end)
				{
					ReconstructPath(data, start, end, node, path);
					return true;
				}
				ComputeAdjacentCoords(data, coords, fromCoords);
				for (int i = 0; i < 4; i++)
				{
					AdjacentCoord adjacentCoord = adjacentCoordsBuffer[i];
					if (adjacentCoord.isValid)
					{
						Vector2Int coords2 = adjacentCoord.coords;
						int num = cost + 1;
						int num2 = coords2.DistanceTo(end);
						int cellIndexLocal = data.GetCellIndexLocal(coords2.get_x(), coords2.get_y());
						if (!steps.TryGetValue(cellIndexLocal, out Node value) || value.cost >= num)
						{
							value = new Node(coords2, coords, num, num + num2);
							frontier.Enqueue(value);
							steps[cellIndexLocal] = value;
						}
					}
				}
			}
			return false;
		}

		private void ReconstructPath(MapData data, Vector2Int start, Vector2Int end, Node lastNode, List<Vector3> path)
		{
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			int cost = lastNode.cost;
			Dictionary<int, Node> steps = m_steps;
			int count = path.Count;
			int num = cost + 1;
			if (path.Capacity < num)
			{
				path.Capacity = num;
			}
			if (count > num)
			{
				path.RemoveRange(num, count - num);
			}
			else if (count < num)
			{
				for (int i = count; i < num; i++)
				{
					path.Add(new Vector3(0f, 0f, 0f));
				}
			}
			path[cost] = data.LocalToWorldCoord(end);
			for (int num2 = cost - 1; num2 > 0; num2--)
			{
				Vector2Int fromCoords = lastNode.fromCoords;
				int cellIndexLocal = data.GetCellIndexLocal(fromCoords.get_x(), fromCoords.get_y());
				lastNode = steps[cellIndexLocal];
				path[num2] = data.LocalToWorldCoord(lastNode.coords);
			}
			path[0] = data.LocalToWorldCoord(start);
		}

		private void ComputeAdjacentCoords(MapData data, Vector2Int coords, Vector2Int from)
		{
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			int x = coords.get_x();
			int y = coords.get_y();
			Vector2Int val = default(Vector2Int);
			val._002Ector(x, y + 1);
			Vector2Int val2 = default(Vector2Int);
			val2._002Ector(x - 1, y);
			Vector2Int val3 = default(Vector2Int);
			val3._002Ector(x + 1, y);
			Vector2Int val4 = default(Vector2Int);
			val4._002Ector(x, y - 1);
			int x2 = val.get_x();
			int y2 = val.get_y();
			bool isValid;
			if (val != from && data.IsInsideLocal(x2, y2))
			{
				int cellIndexLocal = data.GetCellIndexLocal(x2, y2);
				isValid = (data.cells[cellIndexLocal].state == MapData.CellState.Walkable);
			}
			else
			{
				isValid = false;
			}
			int x3 = val2.get_x();
			int y3 = val2.get_y();
			bool isValid2;
			if (val2 != from && data.IsInsideLocal(x3, y3))
			{
				int cellIndexLocal2 = data.GetCellIndexLocal(x3, y3);
				isValid2 = (data.cells[cellIndexLocal2].state == MapData.CellState.Walkable);
			}
			else
			{
				isValid2 = false;
			}
			int x4 = val3.get_x();
			int y4 = val3.get_y();
			bool isValid3;
			if (val3 != from && data.IsInsideLocal(x4, y4))
			{
				int cellIndexLocal3 = data.GetCellIndexLocal(x4, y4);
				isValid3 = (data.cells[cellIndexLocal3].state == MapData.CellState.Walkable);
			}
			else
			{
				isValid3 = false;
			}
			int x5 = val4.get_x();
			int y5 = val4.get_y();
			bool isValid4;
			if (val4 != from && data.IsInsideLocal(x5, y5))
			{
				int cellIndexLocal4 = data.GetCellIndexLocal(x5, y5);
				isValid4 = (data.cells[cellIndexLocal4].state == MapData.CellState.Walkable);
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
	}
}
