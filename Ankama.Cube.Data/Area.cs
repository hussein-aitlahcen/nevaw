using Ankama.Cube.Extensions;
using System;
using UnityEngine;

namespace Ankama.Cube.Data
{
	public abstract class Area
	{
		public Vector2Int refCoord
		{
			get;
			protected set;
		}

		public Vector2Int[] occupiedCoords
		{
			get;
			protected set;
		}

		protected Area(Vector2Int refCoord)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			this.refCoord = refCoord;
		}

		public abstract Area GetCopy();

		public abstract Area GetAreaAt(Vector2Int position);

		public abstract Area GetMovedArea(Vector2Int offset);

		protected abstract bool AreaSpecificEquals(Area other);

		protected bool Equals(Area other)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			Vector2Int refCoord = this.refCoord;
			if (refCoord.Equals(other.refCoord) && other.GetType() == GetType())
			{
				return AreaSpecificEquals(other);
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (this == obj)
			{
				return true;
			}
			if (obj.GetType() == GetType())
			{
				return Equals((Area)obj);
			}
			return false;
		}

		public override int GetHashCode()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			Vector2Int refCoord = this.refCoord;
			return ((object)refCoord).GetHashCode();
		}

		public virtual void Move(Vector2Int offset)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			refCoord += offset;
			Vector2Int[] occupiedCoords = this.occupiedCoords;
			int num = occupiedCoords.Length;
			for (int i = 0; i < num; i++)
			{
				ref Vector2Int reference = ref occupiedCoords[i];
				reference += offset;
			}
		}

		public virtual void MoveTo(Vector2Int newRefCoords)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			Vector2Int val = newRefCoords - refCoord;
			refCoord = newRefCoords;
			Vector2Int[] occupiedCoords = this.occupiedCoords;
			int num = occupiedCoords.Length;
			for (int i = 0; i < num; i++)
			{
				ref Vector2Int reference = ref occupiedCoords[i];
				reference += val;
			}
		}

		public virtual bool Intersects(Area other)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			Vector2Int[] occupiedCoords = this.occupiedCoords;
			int num = occupiedCoords.Length;
			Vector2Int[] occupiedCoords2 = other.occupiedCoords;
			int num2 = occupiedCoords2.Length;
			for (int i = 0; i < num2; i++)
			{
				Vector2Int val = occupiedCoords2[i];
				for (int j = 0; j < num; j++)
				{
					if (occupiedCoords[j] == val)
					{
						return true;
					}
				}
			}
			return false;
		}

		public virtual bool Contains(Coord coord)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			Vector2Int val = default(Vector2Int);
			val._002Ector(coord.x, coord.y);
			Vector2Int[] occupiedCoords = this.occupiedCoords;
			int num = this.occupiedCoords.Length;
			for (int i = 0; i < num; i++)
			{
				if (occupiedCoords[i] == val)
				{
					return true;
				}
			}
			return false;
		}

		public virtual bool Contains(Area other)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			Vector2Int[] occupiedCoords = this.occupiedCoords;
			int num = occupiedCoords.Length;
			Vector2Int[] occupiedCoords2 = other.occupiedCoords;
			int num2 = occupiedCoords2.Length;
			for (int i = 0; i < num2; i++)
			{
				Vector2Int val = occupiedCoords2[i];
				bool flag = false;
				for (int j = 0; j < num; j++)
				{
					if (val == occupiedCoords[j])
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		public virtual int MinDistanceWith(Area other)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			Vector2Int[] occupiedCoords = this.occupiedCoords;
			int num = occupiedCoords.Length;
			Vector2Int[] occupiedCoords2 = other.occupiedCoords;
			int num2 = occupiedCoords2.Length;
			int num3 = int.MaxValue;
			for (int i = 0; i < num; i++)
			{
				Vector2Int from = occupiedCoords[i];
				for (int j = 0; j < num2; j++)
				{
					int num4 = from.DistanceTo(occupiedCoords2[j]);
					if (num4 < num3)
					{
						num3 = num4;
					}
				}
			}
			return num3;
		}

		public virtual int MinDistanceWith(Vector2Int other)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			Vector2Int[] occupiedCoords = this.occupiedCoords;
			int num = occupiedCoords.Length;
			int num2 = int.MaxValue;
			for (int i = 0; i < num; i++)
			{
				int num3 = occupiedCoords[i].DistanceTo(other);
				if (num3 < num2)
				{
					num2 = num3;
				}
			}
			return num2;
		}

		public virtual int MinSquaredDistanceWith(Area other)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			Vector2Int[] occupiedCoords = this.occupiedCoords;
			int num = occupiedCoords.Length;
			Vector2Int[] occupiedCoords2 = other.occupiedCoords;
			int num2 = occupiedCoords2.Length;
			int num3 = int.MaxValue;
			for (int i = 0; i < num; i++)
			{
				Vector2Int val = occupiedCoords[i];
				for (int j = 0; j < num2; j++)
				{
					Vector2Int val2 = occupiedCoords2[j];
					int num4 = Math.Max(Math.Abs(val.get_x() - val2.get_x()), Math.Abs(val.get_y() - val2.get_y()));
					if (num4 < num3)
					{
						num3 = num4;
					}
				}
			}
			return num3;
		}

		public virtual int MinSquaredDistanceWith(Vector2Int other)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			Vector2Int[] occupiedCoords = this.occupiedCoords;
			int num = occupiedCoords.Length;
			int num2 = int.MaxValue;
			for (int i = 0; i < num; i++)
			{
				Vector2Int val = occupiedCoords[i];
				int num3 = Math.Max(Math.Abs(val.get_x() - other.get_x()), Math.Abs(val.get_y() - other.get_y()));
				if (num3 < num2)
				{
					num2 = num3;
				}
			}
			return num2;
		}

		public virtual bool IsAlignedWith(Area other)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			Vector2Int[] occupiedCoords = this.occupiedCoords;
			int num = occupiedCoords.Length;
			Vector2Int[] occupiedCoords2 = other.occupiedCoords;
			int num2 = occupiedCoords2.Length;
			for (int i = 0; i < num; i++)
			{
				Vector2Int val = occupiedCoords[i];
				for (int j = 0; j < num2; j++)
				{
					Vector2Int val2 = occupiedCoords2[j];
					if (val.get_x() == val2.get_x() || val.get_y() == val2.get_y())
					{
						return true;
					}
				}
			}
			return false;
		}

		public virtual bool IsAlignedWith(Vector2Int other)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			Vector2Int[] occupiedCoords = this.occupiedCoords;
			int num = occupiedCoords.Length;
			for (int i = 0; i < num; i++)
			{
				Vector2Int val = occupiedCoords[i];
				if (val.get_x() == other.get_x() || val.get_y() == other.get_y())
				{
					return true;
				}
			}
			return false;
		}

		public virtual bool DistanceValid(Area other, int distance)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			Vector2Int[] occupiedCoords = this.occupiedCoords;
			int num = occupiedCoords.Length;
			Vector2Int[] occupiedCoords2 = other.occupiedCoords;
			int num2 = occupiedCoords2.Length;
			for (int i = 0; i < num; i++)
			{
				Vector2Int from = occupiedCoords[i];
				for (int j = 0; j < num2; j++)
				{
					if (from.DistanceTo(occupiedCoords2[j]) == distance)
					{
						return true;
					}
				}
			}
			return false;
		}

		public virtual bool Intersects(Vector2Int other)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			Vector2Int[] occupiedCoords = this.occupiedCoords;
			int num = occupiedCoords.Length;
			for (int i = 0; i < num; i++)
			{
				if (occupiedCoords[i] == other)
				{
					return true;
				}
			}
			return false;
		}

		public Direction? GetStrictDirection4To(Area other)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			Vector2Int[] occupiedCoords = other.occupiedCoords;
			int num = occupiedCoords.Length;
			for (int i = 0; i < num; i++)
			{
				Direction? strictDirection4To = GetStrictDirection4To(occupiedCoords[i]);
				if (strictDirection4To.HasValue)
				{
					return strictDirection4To;
				}
			}
			return null;
		}

		public Direction? GetStrictDirection4To(Vector2Int otherCoord)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			Vector2Int[] occupiedCoords = this.occupiedCoords;
			int num = occupiedCoords.Length;
			for (int i = 0; i < num; i++)
			{
				Direction? strictDirection4To = occupiedCoords[i].GetStrictDirection4To(otherCoord);
				if (strictDirection4To.HasValue)
				{
					return strictDirection4To.Value;
				}
			}
			return null;
		}
	}
}
