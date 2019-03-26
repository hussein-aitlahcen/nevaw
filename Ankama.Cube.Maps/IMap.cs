using Ankama.Cube.Data;
using Ankama.Cube.Maps.Objects;
using JetBrains.Annotations;
using UnityEngine;

namespace Ankama.Cube.Maps
{
	public interface IMap
	{
		[NotNull]
		CellObject GetCellObject(int x, int y);

		bool TryGetCellObject(int x, int y, out CellObject cellObject);

		Vector2Int GetCellCoords(Vector3 worldPosition);

		void AddArea(Area area);

		void MoveArea(Area from, Area to);

		void RemoveArea(Area area);

		MapCellIndicator GetCellIndicator(int x, int y);
	}
}
