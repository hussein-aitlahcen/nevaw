using Ankama.Cube.Data;
using Ankama.Cube.Protocols.CommonProtocol;
using JetBrains.Annotations;
using System;
using UnityEngine;

namespace Ankama.Cube.Extensions
{
	public static class Vector2IntExtensions
	{
		[Pure]
		public static int DistanceTo(this Vector2Int from, Vector2Int to)
		{
			return Mathf.Abs(to.get_x() - from.get_x()) + Mathf.Abs(to.get_y() - from.get_y());
		}

		[Pure]
		public static bool IsAdjacentTo(this Vector2Int from, Vector2Int to)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return from.DistanceTo(to) == 1;
		}

		[Pure]
		public static Direction? GetStrictDirection4To(this Vector2Int from, Vector2Int to)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			Vector2Int val = to - from;
			if (val.get_x() > 0 && val.get_y() == 0)
			{
				return Direction.NorthEast;
			}
			if (val.get_x() < 0 && val.get_y() == 0)
			{
				return Direction.SouthWest;
			}
			if (val.get_x() == 0 && val.get_y() > 0)
			{
				return Direction.NorthWest;
			}
			if (val.get_x() == 0 && val.get_y() < 0)
			{
				return Direction.SouthEast;
			}
			return null;
		}

		[Pure]
		public static Direction GetDirectionTo(this Vector2Int from, Vector2Int to)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			Vector2Int val = to - from;
			if (val == Vector2Int.get_zero())
			{
				return Direction.None;
			}
			int num = Mathf.RoundToInt(Mathf.Atan2((float)val.get_y(), (float)val.get_x()) / (MathF.PI / 4f));
			return (Direction)((7 - num) & 7);
		}

		[Pure]
		public static CellCoord ToCellCoord(this Vector2Int value)
		{
			return new CellCoord
			{
				X = value.get_x(),
				Y = value.get_y()
			};
		}

		[Pure]
		public unsafe static Vector2Int Rotate(this Vector2Int value, Quaternion rotation)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val = default(Vector3);
			val._002Ector((float)value.get_x(), 0f, (float)value.get_y());
			val = rotation * val;
			return new Vector2Int(Mathf.RoundToInt(((IntPtr)(void*)val).x), Mathf.RoundToInt(((IntPtr)(void*)val).z));
		}
	}
}
