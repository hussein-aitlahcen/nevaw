using Ankama.Cube.Data;
using JetBrains.Annotations;
using System;
using UnityEngine;

namespace Ankama.Cube.Extensions
{
	public static class DirectionAngleExtensions
	{
		[PublicAPI]
		public static DirectionAngle Inverse(this DirectionAngle value)
		{
			return (DirectionAngle)(0 - value);
		}

		[PublicAPI]
		public static Quaternion GetRotation(this DirectionAngle value)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			return Quaternion.AngleAxis(45f * (float)value, Vector3.get_up());
		}

		[PublicAPI]
		public static Quaternion GetInverseRotation(this DirectionAngle value)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			return Quaternion.AngleAxis(-45f * (float)value, Vector3.get_up());
		}

		[PublicAPI]
		public static DirectionAngle Add(this DirectionAngle value, DirectionAngle shift)
		{
			int num = (int)value + (int)shift;
			return (DirectionAngle)(num + num / -5 * 8);
		}

		[PublicAPI]
		public static DirectionAngle Substract(this DirectionAngle value, DirectionAngle shift)
		{
			int num = value - shift;
			return (DirectionAngle)(num + num / -5 * 8);
		}

		[PublicAPI]
		public static DirectionAngle Multiply(this DirectionAngle value, int times)
		{
			int num = (int)value * times;
			int num2 = num % (8 * Math.Sign(num));
			return (DirectionAngle)(num2 + num2 / -5 * 8);
		}
	}
}
