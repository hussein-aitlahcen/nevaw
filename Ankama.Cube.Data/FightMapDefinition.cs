using Ankama.Cube.Fight;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class FightMapDefinition : EditableData, IMapDefinition
	{
		[SerializeField]
		private FightMapConfiguration m_configuration;

		[SerializeField]
		private Vector3Int m_origin;

		[SerializeField]
		private Vector2Int m_sizeMin;

		[SerializeField]
		private Vector2Int m_sizeMax;

		[SerializeField]
		private FightCellState[] m_cellStates;

		[SerializeField]
		private FightMapRegionDefinition[] m_regions;

		public FightMapConfiguration configuration => m_configuration;

		public Vector3Int origin => m_origin;

		public Vector2Int sizeMin => m_sizeMin;

		public Vector2Int sizeMax => m_sizeMax;

		public FightCellState[] cellStates => m_cellStates;

		public FightMapRegionDefinition[] regions => m_regions;

		public int regionCount => m_regions.Length;

		public override void PopulateFromJson(JObject jsonObject)
		{
			this.PopulateFromJson(jsonObject);
		}

		public FightMapRegionDefinition GetRegion(int index)
		{
			return m_regions[index];
		}

		public int GetCellIndex(int x, int y)
		{
			int num = m_sizeMax.get_x() - m_sizeMin.get_x();
			int num2 = m_sizeMin.get_y() * num + m_sizeMin.get_x();
			return y * num + x - num2;
		}

		public Vector2Int GetCellCoords(int index)
		{
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			int num = m_sizeMax.get_x() - m_sizeMin.get_x();
			int num2 = m_sizeMin.get_y() * num + m_sizeMin.get_x();
			int num3 = m_sizeMin.get_x() + index % num;
			int num4 = (index + num2 - num3) / num;
			return new Vector2Int(num3, num4);
		}

		public FightMapStatus CreateFightMapStatus(int regionIndex)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			FightMapRegionDefinition obj = m_regions[regionIndex];
			Vector2Int sizeMin = obj.sizeMin;
			Vector2Int sizeMax = obj.sizeMax;
			Vector2Int val = sizeMax - sizeMin;
			int num = val.get_x() * val.get_y();
			int num2 = m_sizeMax.get_x() - m_sizeMin.get_x();
			int num3 = m_sizeMin.get_y() * num2 + m_sizeMin.get_x();
			int num4 = sizeMax.get_x() - sizeMin.get_x();
			int num5 = sizeMin.get_y() * num4 + sizeMin.get_x();
			FightCellState[] cellStates = m_cellStates;
			FightCellState[] array = new FightCellState[num];
			for (int i = sizeMin.get_y(); i < sizeMax.get_y(); i++)
			{
				for (int j = sizeMin.get_x(); j < sizeMax.get_x(); j++)
				{
					int num6 = i * num2 + j - num3;
					int num7 = i * num4 + j - num5;
					array[num7] = cellStates[num6];
				}
			}
			return new FightMapStatus(array, sizeMin, sizeMax);
		}

		public FightMapDefinition()
			: this()
		{
		}
	}
}
