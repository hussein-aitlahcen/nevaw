using UnityEngine;

namespace Ankama.Cube.Data
{
	public sealed class CenteredSquareArea : Area
	{
		public readonly int radius;

		public CenteredSquareArea(Vector2Int center, int radius)
			: base(center)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			this.radius = radius;
			int num = 2 * radius + 1;
			base.occupiedCoords = (Vector2Int[])new Vector2Int[num * num];
			for (int i = -radius; i <= radius; i++)
			{
				for (int j = -radius; j <= radius; j++)
				{
					base.occupiedCoords[j * num + i] = center + new Vector2Int(i, j);
				}
			}
		}

		protected override bool AreaSpecificEquals(Area other)
		{
			return radius == ((CenteredSquareArea)other).radius;
		}

		public override Area GetCopy()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return new CenteredSquareArea(base.refCoord, radius);
		}

		public override Area GetAreaAt(Vector2Int position)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return new CenteredSquareArea(position, radius);
		}

		public override Area GetMovedArea(Vector2Int offset)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return new CenteredSquareArea(base.refCoord + offset, radius);
		}
	}
}
