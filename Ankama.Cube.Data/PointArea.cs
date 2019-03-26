using Ankama.Cube.Extensions;
using UnityEngine;

namespace Ankama.Cube.Data
{
	public sealed class PointArea : Area
	{
		public PointArea(Vector2Int point)
			: base(point)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			base.occupiedCoords = (Vector2Int[])new Vector2Int[1]
			{
				point
			};
		}

		protected override bool AreaSpecificEquals(Area other)
		{
			return true;
		}

		public override Area GetCopy()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return new PointArea(base.refCoord);
		}

		public override Area GetAreaAt(Vector2Int position)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return new PointArea(position);
		}

		public override Area GetMovedArea(Vector2Int offset)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return new PointArea(base.refCoord + offset);
		}

		public override void Move(Vector2Int offset)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			base.refCoord += offset;
			base.occupiedCoords[0] = base.refCoord;
		}

		public override void MoveTo(Vector2Int newRefCoords)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			base.refCoord = newRefCoords;
			base.occupiedCoords[0] = newRefCoords;
		}

		public override bool Intersects(Area other)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			Vector2Int[] occupiedCoords = other.occupiedCoords;
			int num = occupiedCoords.Length;
			for (int i = 0; i < num; i++)
			{
				if (base.refCoord == occupiedCoords[i])
				{
					return true;
				}
			}
			return false;
		}

		public override bool Contains(Area other)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			Vector2Int[] occupiedCoords = other.occupiedCoords;
			if (occupiedCoords.Length > 1)
			{
				return false;
			}
			return occupiedCoords[0] == base.refCoord;
		}

		public override int MinDistanceWith(Area other)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			Vector2Int[] occupiedCoords = other.occupiedCoords;
			int num = occupiedCoords.Length;
			int num2 = int.MaxValue;
			for (int i = 0; i < num; i++)
			{
				int num3 = base.refCoord.DistanceTo(occupiedCoords[i]);
				if (num3 < num2)
				{
					num2 = num3;
				}
			}
			return num2;
		}

		public override int MinDistanceWith(Vector2Int other)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return base.refCoord.DistanceTo(other);
		}

		public override bool DistanceValid(Area other, int distance)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			Vector2Int[] occupiedCoords = other.occupiedCoords;
			int num = occupiedCoords.Length;
			for (int i = 0; i < num; i++)
			{
				if (base.refCoord.DistanceTo(occupiedCoords[i]) == distance)
				{
					return true;
				}
			}
			return false;
		}

		public override bool Intersects(Vector2Int other)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return base.refCoord == other;
		}
	}
}
