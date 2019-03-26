using Ankama.Cube.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Maps
{
	public class MapQuadTreePathfinding
	{
		private struct Node
		{
			public Vector2 coords;

			public Vector2 fromCoords;

			public float cost;

			public float priority;

			public MapQuadTree.Node quadTreeNode;

			public MapQuadTree.Node fromQuadTreeNode;

			public Node(Vector2 coord, Vector2 fromCoord, float cost, float priority, MapQuadTree.Node node, MapQuadTree.Node fromNode)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				coords = coord;
				fromCoords = fromCoord;
				this.cost = cost;
				this.priority = priority;
				quadTreeNode = node;
				fromQuadTreeNode = fromNode;
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
				float priority = x.priority;
				float priority2 = y.priority;
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

		private Dictionary<MapQuadTree.Node, Node> m_steps = new Dictionary<MapQuadTree.Node, Node>(16);

		private NodePriorityQueue m_frontier = new NodePriorityQueue(16);

		public unsafe bool FindPath(MapData data, Vector3 startWorldPos, Vector3 endWorldPos, List<Vector3> path)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			Vector2 val = default(Vector2);
			val._002Ector(((IntPtr)(void*)startWorldPos).x, ((IntPtr)(void*)startWorldPos).z);
			Vector2 coords = default(Vector2);
			coords._002Ector((float)Mathf.RoundToInt(((IntPtr)(void*)endWorldPos).x), (float)Mathf.RoundToInt(((IntPtr)(void*)endWorldPos).z));
			MapQuadTree.Node nodeAt = data.quadTree.GetNodeAt(val);
			if (nodeAt == null)
			{
				return false;
			}
			MapQuadTree.Node node = data.quadTree.GetNodeAt(coords);
			if (node == null && !data.quadTree.TryGetClosestCell(new Vector2(((IntPtr)(void*)endWorldPos).x, ((IntPtr)(void*)endWorldPos).z), out node, out coords))
			{
				return false;
			}
			return ComputeFullPath(nodeAt, node, val, coords, path);
		}

		private bool ComputeFullPath(MapQuadTree.Node startQuadTreeNode, MapQuadTree.Node endQuadTreeNode, Vector2 start, Vector2 end, List<Vector3> path)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			NodePriorityQueue frontier = m_frontier;
			Dictionary<MapQuadTree.Node, Node> steps = m_steps;
			frontier.Clear();
			steps.Clear();
			start.RoundToInt();
			Node node = new Node(start, start, 0f, 0f, startQuadTreeNode, null);
			frontier.Enqueue(node);
			steps[startQuadTreeNode] = node;
			while (frontier.Count() != 0)
			{
				Node node2 = frontier.Dequeue();
				Vector2 coords = node2.coords;
				float cost = node2.cost;
				MapQuadTree.Node quadTreeNode = node2.quadTreeNode;
				MapQuadTree.Node fromQuadTreeNode = node2.fromQuadTreeNode;
				List<MapQuadTree.Node> connectedNodes = node2.quadTreeNode.connectedNodes;
				if (quadTreeNode == endQuadTreeNode)
				{
					ReconstructPath(end, node2, path);
					return true;
				}
				if (connectedNodes == null)
				{
					continue;
				}
				int count = connectedNodes.Count;
				for (int i = 0; i < count; i++)
				{
					MapQuadTree.Node node3 = connectedNodes[i];
					if (node3 != fromQuadTreeNode)
					{
						Vector2 val = Vector2Int.op_Implicit(node3.ClampPositionToCell(coords));
						float num = cost + coords.DistanceTo(val);
						float num2 = val.DistanceTo(end);
						if (!steps.TryGetValue(node3, out Node value) || !(value.cost < num))
						{
							value = new Node(val, coords, num, num + num2, node3, quadTreeNode);
							frontier.Enqueue(value);
							steps[node3] = value;
						}
					}
				}
			}
			return false;
		}

		private unsafe void ReconstructPath(Vector2 end, Node lastNode, List<Vector3> path)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			path.Clear();
			float height = lastNode.quadTreeNode.height;
			path.Insert(0, new Vector3(((IntPtr)(void*)end).x, height, ((IntPtr)(void*)end).y));
			bool flag = AreDifferent(((IntPtr)(void*)lastNode.coords).x, ((IntPtr)(void*)end).x);
			bool flag2 = AreDifferent(((IntPtr)(void*)lastNode.coords).y, ((IntPtr)(void*)end).y);
			if (flag | flag2)
			{
				if (flag && flag2)
				{
					path.Insert(0, new Vector3(((IntPtr)(void*)end).x, height, ((IntPtr)(void*)lastNode.coords).y));
				}
				path.Insert(0, new Vector3(((IntPtr)(void*)lastNode.coords).x, height, ((IntPtr)(void*)lastNode.coords).y));
			}
			while (lastNode.fromQuadTreeNode != null)
			{
				Vector2 coords = lastNode.coords;
				Vector2 fromCoords = lastNode.fromCoords;
				Node node = m_steps[lastNode.fromQuadTreeNode];
				height = node.quadTreeNode.height;
				flag = AreDifferent(((IntPtr)(void*)coords).x, ((IntPtr)(void*)fromCoords).x);
				flag2 = AreDifferent(((IntPtr)(void*)coords).y, ((IntPtr)(void*)fromCoords).y);
				if (flag && flag2)
				{
					Vector2 val = Vector2Int.op_Implicit(node.quadTreeNode.ClampPositionToCell(coords)) - coords;
					if (Mathf.Abs(((IntPtr)(void*)val).y) > Mathf.Abs(((IntPtr)(void*)val).x))
					{
						path.Insert(0, new Vector3(((IntPtr)(void*)coords).x, height, ((IntPtr)(void*)fromCoords).y));
					}
					else
					{
						path.Insert(0, new Vector3(((IntPtr)(void*)fromCoords).x, height, ((IntPtr)(void*)coords).y));
					}
				}
				lastNode = node;
				path.Insert(0, new Vector3(((IntPtr)(void*)lastNode.coords).x, height, ((IntPtr)(void*)lastNode.coords).y));
			}
		}

		private bool AreDifferent(float v1, float v2)
		{
			return Mathf.Abs(v1 - v2) > 0.0001f;
		}
	}
}
