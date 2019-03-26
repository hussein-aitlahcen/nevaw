using UnityEngine;

namespace Ankama.Cube.Data
{
	public interface IMapDefinition
	{
		Vector3Int origin
		{
			get;
		}

		Vector2Int sizeMin
		{
			get;
		}

		Vector2Int sizeMax
		{
			get;
		}

		int regionCount
		{
			get;
		}

		FightMapRegionDefinition GetRegion(int index);

		int GetCellIndex(int x, int y);
	}
}
