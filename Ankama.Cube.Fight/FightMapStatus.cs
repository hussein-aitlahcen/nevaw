using Ankama.Cube.Data;
using Ankama.Cube.Maps;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Fight
{
	public class FightMapStatus : IMapStateProvider
	{
		private readonly FightCellState[] m_cellStates;

		private readonly Vector2Int m_sizeMin;

		private readonly Vector2Int m_sizeMax;

		private readonly int m_width;

		private readonly int m_delta;

		public Vector2Int sizeMin => m_sizeMin;

		public Vector2Int sizeMax => m_sizeMax;

		public FightMapStatus(FightCellState[] cellStates, Vector2Int sizeMin, Vector2Int sizeMax)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			m_cellStates = cellStates;
			m_sizeMin = sizeMin;
			m_sizeMax = sizeMax;
			m_width = sizeMax.get_x() - sizeMin.get_x();
			m_delta = sizeMin.get_y() * m_width + sizeMin.get_x();
		}

		public int GetCellIndex(int x, int y)
		{
			return y * m_width + x - m_delta;
		}

		public FightCellState GetCellState(int index)
		{
			return m_cellStates[index];
		}

		public FightCellState GetCellState(int x, int y)
		{
			return m_cellStates[y * m_width + x - m_delta];
		}

		public bool TryGetCellState(int index, out FightCellState fightCellState)
		{
			if (index < 0 || index >= m_cellStates.Length)
			{
				fightCellState = FightCellState.None;
				return false;
			}
			fightCellState = m_cellStates[index];
			return true;
		}

		public bool TryGetCellState(int x, int y, out FightCellState fightCellState)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			Vector2Int sizeMin = m_sizeMin;
			Vector2Int sizeMax = m_sizeMax;
			if (x < sizeMin.get_x() || y < sizeMin.get_y() || x >= sizeMax.get_x() || y >= sizeMax.get_y())
			{
				fightCellState = FightCellState.None;
				return false;
			}
			fightCellState = m_cellStates[y * m_width + x - m_delta];
			return true;
		}

		public IEnumerable<Coord> EnumerateCoords()
		{
			Vector2Int sizeMin = m_sizeMin;
			Vector2Int sizeMax = m_sizeMax;
			int xMin = sizeMin.get_x();
			int y2 = sizeMin.get_y();
			int xMax = sizeMax.get_x();
			int yMax = sizeMax.get_y();
			FightCellState[] cellStates = m_cellStates;
			int index = y2 * m_width + xMin - m_delta;
			int num;
			for (int y = y2; y < yMax; y = num)
			{
				for (int x = xMin; x < xMax; x = num)
				{
					switch (cellStates[index])
					{
					case FightCellState.Movement:
						yield return new Coord(x, y);
						break;
					default:
						throw new ArgumentOutOfRangeException();
					case FightCellState.None:
						break;
					}
					num = index + 1;
					index = num;
					num = x + 1;
				}
				num = y + 1;
			}
		}
	}
}
