using Ankama.Cube.Data;
using UnityEngine;

namespace Ankama.Cube.Maps
{
	public interface IMapStateProvider
	{
		Vector2Int sizeMin
		{
			get;
		}

		Vector2Int sizeMax
		{
			get;
		}

		int GetCellIndex(int x, int y);

		FightCellState GetCellState(int index);
	}
}
