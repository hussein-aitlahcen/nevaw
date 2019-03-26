using System;
using UnityEngine;

namespace Ankama.Cube.Data.Maps
{
	[Serializable]
	public struct BossFightMapResources
	{
		[SerializeField]
		private MonsterSpawnCellDefinition m_monsterSpawnCellDefinition;

		[SerializeField]
		private GameObject[] m_heroLostFeedbacks;

		public MonsterSpawnCellDefinition monsterSpawnCellDefinition => m_monsterSpawnCellDefinition;

		public GameObject[] heroLostFeedbacks => m_heroLostFeedbacks;
	}
}
