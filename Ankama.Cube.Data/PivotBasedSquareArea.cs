using UnityEngine;

namespace Ankama.Cube.Data
{
	public sealed class PivotBasedSquareArea : Area
	{
		public readonly int sideSize;

		public PivotBasedSquareArea(Vector2Int pivot, int sideSize)
			: base(pivot)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			this.sideSize = sideSize;
			base.occupiedCoords = (Vector2Int[])new Vector2Int[sideSize * sideSize];
			for (int i = 0; i < sideSize; i++)
			{
				for (int j = 0; j < sideSize; j++)
				{
					base.occupiedCoords[j * sideSize + i] = pivot + new Vector2Int(i, j);
				}
			}
		}

		protected override bool AreaSpecificEquals(Area other)
		{
			return sideSize == ((PivotBasedSquareArea)other).sideSize;
		}

		public override Area GetCopy()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return new PivotBasedSquareArea(base.refCoord, sideSize);
		}

		public override Area GetAreaAt(Vector2Int position)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return new PivotBasedSquareArea(position, sideSize);
		}

		public override Area GetMovedArea(Vector2Int offset)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return new PivotBasedSquareArea(base.refCoord + offset, sideSize);
		}
	}
}
