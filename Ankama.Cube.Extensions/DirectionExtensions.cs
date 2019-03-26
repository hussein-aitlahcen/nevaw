using Ankama.Cube.Data;
using JetBrains.Annotations;
using UnityEngine;

namespace Ankama.Cube.Extensions
{
	public static class DirectionExtensions
	{
		[PublicAPI]
		public static bool IsAxisAligned(this Direction value)
		{
			return (value & Direction.SouthEast) != Direction.East;
		}

		[PublicAPI]
		public static Direction TurnCounterClockwise45(this Direction value)
		{
			return (value + 7) & Direction.NorthEast;
		}

		[PublicAPI]
		public static Direction TurnCounterClockwise90(this Direction value)
		{
			return (value + 6) & Direction.NorthEast;
		}

		[PublicAPI]
		public static Direction TurnClockwise45(this Direction value)
		{
			return (value + 1) & Direction.NorthEast;
		}

		[PublicAPI]
		public static Direction TurnClockwise90(this Direction value)
		{
			return (value + 2) & Direction.NorthEast;
		}

		[PublicAPI]
		public static Direction Inverse(this Direction value)
		{
			return (value + 4) & Direction.NorthEast;
		}

		[PublicAPI]
		public static Direction Rotate(this Direction value, DirectionAngle angle)
		{
			return (Direction)(((int)value + (int)angle + 8) & 7);
		}

		[PublicAPI]
		public static Direction RotateInverse(this Direction value, DirectionAngle angle)
		{
			return (Direction)(((int)value - (int)angle - 8) & 7);
		}

		[PublicAPI]
		public static DirectionAngle DirectionAngleTo(this Direction from, Direction to)
		{
			return (DirectionAngle)(((to - from + 4) & 7) - 4);
		}

		[PublicAPI]
		public static Direction GetAxisAligned(this Direction value, Direction from)
		{
			int num = ((value - from + 4) & 7) - 4;
			int num2 = 2 * (num / 2);
			return (from + num2) & Direction.NorthEast;
		}

		[PublicAPI]
		public static Quaternion GetRotation(this Direction value)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			return Quaternion.AngleAxis(-45f * (float)(7 - value), Vector3.get_up());
		}

		[PublicAPI]
		public static Quaternion GetInverseRotation(this Direction value)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			return Quaternion.AngleAxis(45f * (float)(7 - value), Vector3.get_up());
		}
	}
}
