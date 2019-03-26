using UnityEngine;

namespace Ankama.Cube.Data
{
	public sealed class CircleArea : Area
	{
		public readonly int radius;

		public CircleArea(Vector2Int center, int radius)
			: base(center)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			this.radius = radius;
			int num = 0;
			int num2 = 4 * radius + 1;
			base.occupiedCoords = (Vector2Int[])new Vector2Int[num2];
			for (int i = -radius; i <= 0; i++)
			{
				int num3 = radius + i;
				for (int j = -radius - i; j <= num3; j++)
				{
					base.occupiedCoords[num] = center + new Vector2Int(i, j);
					num++;
				}
			}
			for (int k = 1; k <= radius; k++)
			{
				int num4 = radius - k;
				for (int l = -radius + k; l <= num4; l++)
				{
					base.occupiedCoords[num] = center + new Vector2Int(k, l);
					num++;
				}
			}
		}

		protected override bool AreaSpecificEquals(Area other)
		{
			return radius == ((CircleArea)other).radius;
		}

		public override Area GetCopy()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return new CircleArea(base.refCoord, radius);
		}

		public override Area GetAreaAt(Vector2Int position)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return new CircleArea(position, radius);
		}

		public override Area GetMovedArea(Vector2Int offset)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return new CircleArea(base.refCoord + offset, radius);
		}
	}
}
