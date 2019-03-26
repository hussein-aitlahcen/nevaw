using Ankama.Cube.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Maps
{
	[DisallowMultipleComponent]
	public class MapData : MonoBehaviour
	{
		public enum CellState
		{
			Walkable,
			NotWalkable
		}

		[Serializable]
		public class CellData
		{
			[SerializeField]
			public CellState state = CellState.NotWalkable;
		}

		[SerializeField]
		[HideInInspector]
		private Vector2Int m_origin;

		[SerializeField]
		[HideInInspector]
		private Vector2Int m_size;

		[SerializeField]
		[HideInInspector]
		private int m_height;

		[SerializeField]
		[HideInInspector]
		private CellData[] m_cells = new CellData[0];

		[SerializeField]
		[HideInInspector]
		private MapQuadTree m_quadTree;

		private static List<MapData> s_list = new List<MapData>();

		public Vector2Int min => m_origin;

		public Vector2Int max => m_origin * m_size;

		public Vector3 center => new Vector3((float)m_origin.get_x() - 0.5f + (float)m_size.get_x() * 0.5f, (float)m_height, (float)m_origin.get_y() - 0.5f + (float)m_size.get_y() * 0.5f);

		public float height => m_height;

		public Vector2Int size => m_size;

		public CellData[] cells => m_cells;

		public MapQuadTree quadTree => m_quadTree;

		public unsafe static MapData GetMapFromWorldPos(Vector3 worldPos)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			for (int i = 0; i < s_list.Count; i++)
			{
				MapData mapData = s_list[i];
				if (mapData.quadTree != null && mapData.quadTree.rootNode.IsInside(new Vector2(((IntPtr)(void*)worldPos).x, ((IntPtr)(void*)worldPos).z)))
				{
					return mapData;
				}
			}
			return null;
		}

		private void OnEnable()
		{
			s_list.Add(this);
		}

		private void OnDisable()
		{
			s_list.Remove(this);
		}

		public bool RayCast(Ray ray, out Vector3 hit)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			Plane val = default(Plane);
			val._002Ector(Vector3.get_up(), height);
			float num = default(float);
			if (val.Raycast(ray, ref num))
			{
				hit = ray.GetPoint(num);
				return true;
			}
			hit = Vector3.get_zero();
			return false;
		}

		public void SetCell(CellData cell, Vector2Int localCoord)
		{
			m_cells[GetCellIndexLocal(localCoord.get_x(), localCoord.get_y())] = cell;
		}

		public bool TryGetCell(Vector2Int worldCoord, out CellData cell, out Vector2Int localCoord)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			localCoord = worldCoord - m_origin;
			int x = localCoord.get_x();
			int y = localCoord.get_y();
			if (x < 0 || x >= m_size.get_x() || y < 0 || y >= m_size.get_y())
			{
				cell = null;
				return false;
			}
			cell = m_cells[GetCellIndexLocal(x, y)];
			return true;
		}

		public unsafe Vector2Int WorldToLocalCoord(Vector3 worldPos)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2Int(Mathf.RoundToInt(((IntPtr)(void*)worldPos).x), Mathf.RoundToInt(((IntPtr)(void*)worldPos).z)) - m_origin;
		}

		public unsafe Vector3 TwoDToThreeDWorldCoord(Vector2 worldCoord)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			return new Vector3(((IntPtr)(void*)worldCoord).x, (float)m_height, ((IntPtr)(void*)worldCoord).y);
		}

		public Vector3 LocalToWorldCoord(Vector2Int localCoord)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			Vector2Int val = m_origin + localCoord;
			return new Vector3((float)val.get_x(), (float)m_height, (float)val.get_y());
		}

		public int GetCellIndexLocal(int x, int y)
		{
			return x + m_size.get_x() * y;
		}

		public bool IsInsideLocal(int x, int y)
		{
			if (x >= 0 && x < m_size.get_x() && y >= 0)
			{
				return y < m_size.get_y();
			}
			return false;
		}

		public bool IsAreaFullOfState(Vector2Int start, Vector2Int end, CellState state)
		{
			if (!IsInsideLocal(start.get_x(), start.get_y()) || !IsInsideLocal(end.get_x(), end.get_y()))
			{
				return false;
			}
			for (int i = start.get_x(); i < end.get_x() + 1; i++)
			{
				for (int j = start.get_y(); j < end.get_y() + 1; j++)
				{
					if (m_cells[GetCellIndexLocal(i, j)].state != state)
					{
						return false;
					}
				}
			}
			return true;
		}

		public void Clear()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			m_size = Vector2Int.get_zero();
			m_cells = new CellData[0];
		}

		public void Init(Vector2Int origin, Vector2Int size)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			m_origin = origin;
			m_size = size;
			m_cells = new CellData[size.get_x() * size.get_y()];
		}

		public void Resize(Vector2Int start, Vector2Int end)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			Vector2Int val = m_origin + start;
			Vector2Int size = end + Vector2Int.get_one() - start;
			CellData[] array = new CellData[size.get_x() * size.get_y()];
			Vector2Int val2 = Vector2Int.Max(start, Vector2Int.get_zero());
			Vector2Int val3 = Vector2Int.Min(end, m_size - Vector2Int.get_one()) + Vector2Int.get_one() - val2;
			Vector2Int val4 = Vector2Int.Max(m_origin - val, Vector2Int.get_zero());
			for (int i = 0; i < val3.get_y(); i++)
			{
				Array.Copy(m_cells, val2.get_x() + (i + val2.get_y()) * m_size.get_x(), array, val4.get_x() + (i + val4.get_y()) * size.get_x(), val3.get_x());
			}
			m_origin = val;
			m_size = size;
			m_cells = array;
		}

		public void GenerateNodeQuadTree()
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			int num = Mathf.NextPowerOfTwo(m_size.get_x());
			int num2 = Mathf.NextPowerOfTwo(m_size.get_y());
			int num3 = Mathf.Max(num, num2);
			MapQuadTree.Node node = GenerateNodes(m_cells, Vector2Int.get_zero(), new Vector2Int(num3 - 1, num3 - 1));
			m_quadTree.SetNodes(node);
			GenerateNodeConnections(null, node);
		}

		private void GenerateNodeConnections(MapQuadTree.Node parentNode, MapQuadTree.Node node)
		{
			if (node == null)
			{
				return;
			}
			if (node.hasNoChildren)
			{
				List<MapQuadTree.Node> neighbours = GetNeighbours(node);
				for (int i = 0; i < neighbours.Count; i++)
				{
					MapQuadTree.Node node2 = neighbours[i];
					if (node.connectedNodes == null || !node.connectedNodes.Contains(node2))
					{
						if (node.connectedNodes == null)
						{
							node.connectedNodes = new List<MapQuadTree.Node>();
						}
						node.connectedNodes.Add(node2);
					}
					if (node2.connectedNodes == null || !node2.connectedNodes.Contains(node))
					{
						if (node2.connectedNodes == null)
						{
							node2.connectedNodes = new List<MapQuadTree.Node>();
						}
						node2.connectedNodes.Add(node);
					}
				}
			}
			GenerateNodeConnections(node, node.topLeft);
			GenerateNodeConnections(node, node.topRight);
			GenerateNodeConnections(node, node.bottomLeft);
			GenerateNodeConnections(node, node.bottomRight);
		}

		private List<MapQuadTree.Node> GetNeighbours(MapQuadTree.Node node)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			List<MapQuadTree.Node> list = new List<MapQuadTree.Node>();
			List<Vector2> allCellAroudNode = GetAllCellAroudNode(node);
			for (int i = 0; i < allCellAroudNode.Count; i++)
			{
				MapQuadTree.Node nodeAt = quadTree.GetNodeAt(allCellAroudNode[i]);
				if (nodeAt != null && !list.Contains(nodeAt))
				{
					list.Add(nodeAt);
				}
			}
			return list;
		}

		private List<Vector2> GetAllCellAroudNode(MapQuadTree.Node node)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			List<Vector2> list = new List<Vector2>();
			Vector2 val4 = node.size * 0.5f;
			Vector2 value = node.min - Vector2.get_one() * 0.5f;
			Vector2 value2 = node.max + Vector2.get_one() * 0.5f;
			Vector2Int val = value.RoundToInt();
			Vector2Int val2 = value2.RoundToInt();
			Vector2Int val3 = val2 + Vector2Int.get_one() - val;
			for (int i = val.get_x() + 1; i < val2.get_x(); i++)
			{
				list.Add(new Vector2((float)i, (float)val.get_y()));
				if (val3.get_y() > 1)
				{
					list.Add(new Vector2((float)i, (float)val2.get_y()));
				}
			}
			for (int j = val.get_y() + 1; j < val2.get_y(); j++)
			{
				list.Add(new Vector2((float)val.get_x(), (float)j));
				if (val3.get_x() > 1)
				{
					list.Add(new Vector2((float)val2.get_x(), (float)j));
				}
			}
			return list;
		}

		private MapQuadTree.Node GenerateNodes(CellData[] cells, Vector2Int start, Vector2Int end)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			Vector2Int val = end + Vector2Int.get_one() - start;
			MapQuadTree.Node node = new MapQuadTree.Node();
			node.min = Vector2Int.op_Implicit(m_origin + start) - Vector2.get_one() * 0.5f;
			node.max = Vector2Int.op_Implicit(m_origin + end) + Vector2.get_one() * 0.5f;
			int num = val.get_x() * val.get_y();
			int num2 = 0;
			for (int i = start.get_x(); i < end.get_x() + 1; i++)
			{
				for (int j = start.get_y(); j < end.get_y() + 1; j++)
				{
					if (IsInsideLocal(i, j) && cells[GetCellIndexLocal(i, j)].state == CellState.Walkable)
					{
						num2++;
					}
				}
			}
			if (num2 == 0)
			{
				return null;
			}
			if (num2 == num)
			{
				return node;
			}
			if (start == end)
			{
				return null;
			}
			node.height = height;
			Vector2Int val2 = (node.size * 0.5f).RoundToInt();
			Vector2Int val3 = val2 - Vector2Int.get_one();
			Vector2Int val4 = start + new Vector2Int(0, val2.get_y());
			node.topLeft = GenerateNodes(cells, val4, val4 + val3);
			Vector2Int val5 = start + val2;
			node.topRight = GenerateNodes(cells, val5, val5 + val3);
			Vector2Int val6 = start;
			node.bottomLeft = GenerateNodes(cells, val6, val6 + val3);
			Vector2Int val7 = start + new Vector2Int(val2.get_x(), 0);
			node.bottomRight = GenerateNodes(cells, val7, val7 + val3);
			return node;
		}

		public MapData()
			: this()
		{
		}
	}
}
