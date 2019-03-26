using Ankama.Cube.Data;
using Ankama.Cube.Maps.Objects;
using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Maps
{
	public class MapVirtualGrid
	{
		private struct Cell
		{
			public readonly CellObject cellObject;

			public Area area;

			public Cell(CellObject cellObject)
			{
				this.cellObject = cellObject;
				area = null;
			}
		}

		private readonly IMapDefinition m_mapDefinition;

		private readonly Cell[] m_virtualCells;

		public MapVirtualGrid(IMapDefinition mapDefinition, CellObject[] cells)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			m_mapDefinition = mapDefinition;
			Vector2Int val = mapDefinition.sizeMax - mapDefinition.sizeMin;
			m_virtualCells = new Cell[val.get_x() * val.get_y()];
			int num = cells.Length;
			for (int i = 0; i < num; i++)
			{
				CellObject cellObject = cells[i];
				Vector2Int coords = cellObject.coords;
				int cellIndex = mapDefinition.GetCellIndex(coords.get_x(), coords.get_y());
				m_virtualCells[cellIndex] = new Cell(cellObject);
			}
		}

		public bool IsReferenceCell(Vector2Int coords)
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			int cellIndex = m_mapDefinition.GetCellIndex(coords.get_x(), coords.get_y());
			Area area = m_virtualCells[cellIndex].area;
			if (area != null)
			{
				return area.refCoord == coords;
			}
			return true;
		}

		public CellObject GetReferenceCell(Vector2Int coords)
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			IMapDefinition mapDefinition = m_mapDefinition;
			int cellIndex = mapDefinition.GetCellIndex(coords.get_x(), coords.get_y());
			Cell cell = m_virtualCells[cellIndex];
			Area area = cell.area;
			if (area == null)
			{
				return cell.cellObject;
			}
			Vector2Int refCoord = area.refCoord;
			cellIndex = mapDefinition.GetCellIndex(refCoord.get_x(), refCoord.get_y());
			return m_virtualCells[cellIndex].cellObject;
		}

		public bool TryGetReferenceCell(Vector2Int coords, out CellObject referenceCell)
		{
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			IMapDefinition mapDefinition = m_mapDefinition;
			int cellIndex = mapDefinition.GetCellIndex(coords.get_x(), coords.get_y());
			Cell cell = m_virtualCells[cellIndex];
			Area area = cell.area;
			if (area == null)
			{
				referenceCell = cell.cellObject;
				return true;
			}
			Vector2Int refCoord = area.refCoord;
			if (refCoord == coords)
			{
				cellIndex = mapDefinition.GetCellIndex(refCoord.get_x(), refCoord.get_y());
				referenceCell = m_virtualCells[cellIndex].cellObject;
				return true;
			}
			referenceCell = null;
			return false;
		}

		public void GetReferenceCellsNoAlloc(List<CellObject> outCells)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			int num = m_virtualCells.Length;
			for (int i = 0; i < num; i++)
			{
				Cell cell = m_virtualCells[i];
				CellObject cellObject = cell.cellObject;
				if (!(null == cellObject))
				{
					Area area = cell.area;
					if (area == null || area.refCoord == cellObject.coords)
					{
						outCells.Add(cellObject);
					}
				}
			}
		}

		public void GetAreaCellsNoAlloc(Vector2Int coords, List<CellObject> outCells)
		{
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			IMapDefinition mapDefinition = m_mapDefinition;
			int cellIndex = mapDefinition.GetCellIndex(coords.get_x(), coords.get_y());
			Cell cell = m_virtualCells[cellIndex];
			Area area = cell.area;
			if (area == null)
			{
				CellObject cellObject = cell.cellObject;
				if (null != cellObject)
				{
					outCells.Add(cellObject);
				}
				return;
			}
			Vector2Int[] occupiedCoords = area.occupiedCoords;
			int num = occupiedCoords.Length;
			for (int i = 0; i < num; i++)
			{
				Vector2Int val = occupiedCoords[i];
				cellIndex = mapDefinition.GetCellIndex(val.get_x(), val.get_y());
				outCells.Add(m_virtualCells[cellIndex].cellObject);
			}
		}

		public void GetLinkedCellsNoAlloc(Vector2Int coords, List<CellObject> outCells)
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			IMapDefinition mapDefinition = m_mapDefinition;
			int cellIndex = mapDefinition.GetCellIndex(coords.get_x(), coords.get_y());
			Area area = m_virtualCells[cellIndex].area;
			if (area == null || area.refCoord != coords)
			{
				return;
			}
			Vector2Int[] occupiedCoords = area.occupiedCoords;
			int num = occupiedCoords.Length;
			for (int i = 0; i < num; i++)
			{
				Vector2Int val = occupiedCoords[i];
				if (!(val == coords))
				{
					cellIndex = mapDefinition.GetCellIndex(val.get_x(), val.get_y());
					outCells.Add(m_virtualCells[cellIndex].cellObject);
				}
			}
		}

		public void AddArea([NotNull] Area area)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			IMapDefinition mapDefinition = m_mapDefinition;
			Vector2Int[] occupiedCoords = area.occupiedCoords;
			int num = occupiedCoords.Length;
			for (int i = 0; i < num; i++)
			{
				Vector2Int val = occupiedCoords[i];
				int cellIndex = mapDefinition.GetCellIndex(val.get_x(), val.get_y());
				m_virtualCells[cellIndex].area = area;
			}
		}

		public void MoveArea([NotNull] Area from, [NotNull] Area to)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			IMapDefinition mapDefinition = m_mapDefinition;
			Vector2Int[] occupiedCoords = from.occupiedCoords;
			int num = occupiedCoords.Length;
			for (int i = 0; i < num; i++)
			{
				Vector2Int val = occupiedCoords[i];
				int cellIndex = mapDefinition.GetCellIndex(val.get_x(), val.get_y());
				m_virtualCells[cellIndex].area = null;
			}
			Vector2Int[] occupiedCoords2 = to.occupiedCoords;
			int num2 = occupiedCoords2.Length;
			for (int j = 0; j < num2; j++)
			{
				Vector2Int val2 = occupiedCoords2[j];
				int cellIndex2 = mapDefinition.GetCellIndex(val2.get_x(), val2.get_y());
				m_virtualCells[cellIndex2].area = to;
			}
		}

		public void RemoveArea([NotNull] Area area)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			IMapDefinition mapDefinition = m_mapDefinition;
			Vector2Int[] occupiedCoords = area.occupiedCoords;
			int num = occupiedCoords.Length;
			for (int i = 0; i < num; i++)
			{
				Vector2Int val = occupiedCoords[i];
				int cellIndex = mapDefinition.GetCellIndex(val.get_x(), val.get_y());
				m_virtualCells[cellIndex].area = null;
			}
		}
	}
}
