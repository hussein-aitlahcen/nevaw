using Ankama.Cube.Protocols.CommonProtocol;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	public struct Coord : IEquatable<Coord>, IEquatable<Vector2Int>
	{
		public readonly int x;

		public readonly int y;

		public Coord(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public Coord(Coord copy)
		{
			x = copy.x;
			y = copy.y;
		}

		public Coord(Vector2Int vector)
		{
			x = vector.get_x();
			y = vector.get_y();
		}

		public override bool Equals(object obj)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			if (obj == null)
			{
				return false;
			}
			if (!(obj is Coord) || !Equals((Coord)obj))
			{
				if (obj is Vector2Int)
				{
					return Equals((Vector2Int)obj);
				}
				return false;
			}
			return true;
		}

		public override int GetHashCode()
		{
			return (x * 397) ^ y;
		}

		public bool Equals(Coord other)
		{
			if (x == other.x)
			{
				return y == other.y;
			}
			return false;
		}

		public bool IsAlignedWith(Coord other)
		{
			if (x != other.x)
			{
				return y == other.y;
			}
			return true;
		}

		public IEnumerable<Coord> StraightPathUntil(Coord other)
		{
			if (Equals(other))
			{
				yield break;
			}
			if (x == other.x)
			{
				int increment2 = Math.Sign(other.y - y);
				for (int newY2 = y + increment2; newY2 != other.y; newY2 += increment2)
				{
					yield return new Coord(x, newY2);
				}
			}
			else if (y == other.y)
			{
				int increment2 = Math.Sign(other.x - x);
				for (int newY2 = x + increment2; newY2 != other.x; newY2 += increment2)
				{
					yield return new Coord(newY2, y);
				}
			}
		}

		public static bool operator ==(Coord value, Coord other)
		{
			if (value.x == other.x)
			{
				return value.y == other.y;
			}
			return false;
		}

		public static bool operator !=(Coord value, Coord other)
		{
			if (value.x == other.x)
			{
				return value.y != other.y;
			}
			return true;
		}

		public bool Equals(Vector2Int other)
		{
			if (x == other.get_x())
			{
				return y == other.get_y();
			}
			return false;
		}

		public static explicit operator Vector2Int(Coord value)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2Int(value.x, value.y);
		}

		public static bool operator ==(Coord value, Vector2Int vector)
		{
			if (value.x == vector.get_x())
			{
				return value.y == vector.get_y();
			}
			return false;
		}

		public static bool operator !=(Coord value, Vector2Int vector)
		{
			if (value.x == vector.get_x())
			{
				return value.y != vector.get_y();
			}
			return true;
		}

		[Pure]
		public CellCoord ToCellCoord()
		{
			return new CellCoord
			{
				X = x,
				Y = y
			};
		}
	}
}
