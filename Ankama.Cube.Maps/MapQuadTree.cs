using Ankama.Cube.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Maps
{
	[Serializable]
	public class MapQuadTree : ISerializationCallbackReceiver
	{
		[Serializable]
		public struct SerializableNode
		{
			[SerializeField]
			public Vector2 min;

			[SerializeField]
			public Vector2 max;

			[SerializeField]
			public float height;

			[SerializeField]
			public int topLeftIndex;

			[SerializeField]
			public int topRightIndex;

			[SerializeField]
			public int bottomLeftIndex;

			[SerializeField]
			public int bottomRightIndex;

			[SerializeField]
			public List<int> connectedNodeIndex;
		}

		public class Node
		{
			public Vector2 min;

			public Vector2 max;

			public float height;

			public Node topLeft;

			public Node topRight;

			public Node bottomLeft;

			public Node bottomRight;

			public List<Node> connectedNodes;

			public Vector2 center => (max + min) / 2f;

			public Vector2 size => max - min;

			public bool hasNoChildren
			{
				get
				{
					if (topLeft == null && topRight == null && bottomLeft == null)
					{
						return bottomRight == null;
					}
					return false;
				}
			}

			public bool hasConnections
			{
				get
				{
					if (connectedNodes != null)
					{
						return connectedNodes.Count > 0;
					}
					return false;
				}
			}

			public unsafe bool IsInside(Vector2 worldPos)
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				if (((IntPtr)(void*)worldPos).x > min.x && ((IntPtr)(void*)worldPos).x <= max.x && ((IntPtr)(void*)worldPos).y > min.y)
				{
					return ((IntPtr)(void*)worldPos).y <= max.y;
				}
				return false;
			}

			public Vector2Int ClampPositionToCell(Vector2 worldPos)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0021: Unknown result type (might be due to invalid IL or missing references)
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				//IL_0030: Unknown result type (might be due to invalid IL or missing references)
				//IL_0035: Unknown result type (might be due to invalid IL or missing references)
				//IL_003a: Unknown result type (might be due to invalid IL or missing references)
				//IL_003f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0040: Unknown result type (might be due to invalid IL or missing references)
				//IL_0041: Unknown result type (might be due to invalid IL or missing references)
				//IL_0046: Unknown result type (might be due to invalid IL or missing references)
				//IL_0049: Unknown result type (might be due to invalid IL or missing references)
				//IL_004a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0050: Unknown result type (might be due to invalid IL or missing references)
				Vector2Int val = (min + Vector2.get_one() * 0.5f).RoundToInt();
				Vector2Int val2 = (max - Vector2.get_one() * 0.5f).RoundToInt();
				Vector2Int result = worldPos.RoundToInt();
				result.Clamp(val, val2);
				return result;
			}

			public Vector2 ClampPosition(Vector2 worldPos)
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				return worldPos.Clamp(min, max);
			}

			public float DistanceToPoint(Vector2 worldPos)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				return ClampPosition(worldPos).DistanceTo(worldPos);
			}
		}

		[SerializeField]
		private int m_serializableRootNodeIndex = -1;

		[SerializeField]
		private List<SerializableNode> m_serializableNodeList = new List<SerializableNode>();

		public Node rootNode
		{
			get;
			private set;
		}

		public void SetNodes(Node value)
		{
			Clear();
			rootNode = value;
		}

		public void Clear()
		{
			rootNode = null;
			m_serializableRootNodeIndex = -1;
			m_serializableNodeList.Clear();
		}

		public bool TryGetClosestCell(Vector2 worldPos, out Node node, out Vector2 coords)
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			node = null;
			coords = Vector2.get_zero();
			if (TryGetClosestNodeRecurively(rootNode, worldPos, out node, out float _))
			{
				coords = Vector2Int.op_Implicit(node.ClampPositionToCell(worldPos));
				return true;
			}
			return false;
		}

		private bool TryGetClosestNodeRecurively(Node node, Vector2 worldPos, out Node outNode, out float distance)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			outNode = null;
			distance = float.MaxValue;
			if (node == null)
			{
				return false;
			}
			if (node.hasNoChildren)
			{
				outNode = node;
				distance = node.DistanceToPoint(worldPos);
				return true;
			}
			Node node2 = null;
			float num = float.MaxValue;
			if (TryGetClosestNodeRecurively(node.topLeft, worldPos, out outNode, out distance) && distance < num)
			{
				num = distance;
				node2 = outNode;
			}
			if (TryGetClosestNodeRecurively(node.topRight, worldPos, out outNode, out distance) && distance < num)
			{
				num = distance;
				node2 = outNode;
			}
			if (TryGetClosestNodeRecurively(node.bottomLeft, worldPos, out outNode, out distance) && distance < num)
			{
				num = distance;
				node2 = outNode;
			}
			if (TryGetClosestNodeRecurively(node.bottomRight, worldPos, out outNode, out distance) && distance < num)
			{
				num = distance;
				node2 = outNode;
			}
			distance = num;
			outNode = node2;
			return true;
		}

		public Node GetNodeAt(Vector2 worldPos)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return GetNodeAtRecurively(rootNode, worldPos);
		}

		private Node GetNodeAtRecurively(Node node, Vector2 worldPos)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			if (node == null)
			{
				return null;
			}
			if (!node.IsInside(worldPos))
			{
				return null;
			}
			if (node.hasNoChildren)
			{
				return node;
			}
			Node nodeAtRecurively = GetNodeAtRecurively(node.topLeft, worldPos);
			if (nodeAtRecurively == null)
			{
				nodeAtRecurively = GetNodeAtRecurively(node.topRight, worldPos);
			}
			if (nodeAtRecurively == null)
			{
				nodeAtRecurively = GetNodeAtRecurively(node.bottomLeft, worldPos);
			}
			if (nodeAtRecurively == null)
			{
				nodeAtRecurively = GetNodeAtRecurively(node.bottomRight, worldPos);
			}
			return nodeAtRecurively;
		}

		public void OnAfterDeserialize()
		{
			List<Node> list = new List<Node>();
			for (int i = 0; i < m_serializableNodeList.Count; i++)
			{
				list.Add(new Node());
			}
			rootNode = DeSerializeNode(m_serializableRootNodeIndex, list);
		}

		private Node DeSerializeNode(int index, List<Node> nodeByIndex)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			if (index < 0 || index >= m_serializableNodeList.Count || m_serializableNodeList.Count == 0)
			{
				return null;
			}
			SerializableNode serializableNode = m_serializableNodeList[index];
			Node node = nodeByIndex[index];
			node.min = serializableNode.min;
			node.max = serializableNode.max;
			node.height = serializableNode.height;
			node.topLeft = DeSerializeNode(serializableNode.topLeftIndex, nodeByIndex);
			node.topRight = DeSerializeNode(serializableNode.topRightIndex, nodeByIndex);
			node.bottomLeft = DeSerializeNode(serializableNode.bottomLeftIndex, nodeByIndex);
			node.bottomRight = DeSerializeNode(serializableNode.bottomRightIndex, nodeByIndex);
			node.connectedNodes = new List<Node>();
			for (int i = 0; i < serializableNode.connectedNodeIndex.Count; i++)
			{
				int index2 = serializableNode.connectedNodeIndex[i];
				node.connectedNodes.Add(nodeByIndex[index2]);
			}
			return node;
		}

		public void OnBeforeSerialize()
		{
			List<Node> nodeByIndex = new List<Node>();
			m_serializableNodeList.Clear();
			m_serializableRootNodeIndex = SerializeNode(rootNode, nodeByIndex);
			SerializeNodeConnection(rootNode, nodeByIndex);
		}

		private int SerializeNode(Node node, List<Node> nodeByIndex)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			if (node == null)
			{
				return -1;
			}
			SerializableNode item = default(SerializableNode);
			item.min = node.min;
			item.max = node.max;
			item.height = node.height;
			item.topLeftIndex = SerializeNode(node.topLeft, nodeByIndex);
			item.topRightIndex = SerializeNode(node.topRight, nodeByIndex);
			item.bottomLeftIndex = SerializeNode(node.bottomLeft, nodeByIndex);
			item.bottomRightIndex = SerializeNode(node.bottomRight, nodeByIndex);
			int count = m_serializableNodeList.Count;
			m_serializableNodeList.Add(item);
			nodeByIndex.Add(node);
			return count;
		}

		private void SerializeNodeConnection(Node node, List<Node> nodeByIndex)
		{
			if (node == null)
			{
				return;
			}
			int index = nodeByIndex.IndexOf(node);
			SerializableNode serializableNode = m_serializableNodeList[index];
			if (node.connectedNodes != null)
			{
				for (int i = 0; i < node.connectedNodes.Count; i++)
				{
					Node item = node.connectedNodes[i];
					int item2 = nodeByIndex.IndexOf(item);
					if (serializableNode.connectedNodeIndex == null)
					{
						serializableNode.connectedNodeIndex = new List<int>();
					}
					serializableNode.connectedNodeIndex.Add(item2);
				}
			}
			m_serializableNodeList[index] = serializableNode;
			SerializeNodeConnection(node.topLeft, nodeByIndex);
			SerializeNodeConnection(node.topRight, nodeByIndex);
			SerializeNodeConnection(node.bottomLeft, nodeByIndex);
			SerializeNodeConnection(node.bottomRight, nodeByIndex);
		}
	}
}
