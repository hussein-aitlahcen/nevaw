using JetBrains.Annotations;
using System;
using UnityEngine;

namespace Ankama.Cube.Extensions
{
	public static class Vector2Extensions
	{
		[Pure]
		public unsafe static float DistanceTo(this Vector2 from, Vector2 to)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			return Mathf.Abs(((IntPtr)(void*)to).x - ((IntPtr)(void*)from).x) + Mathf.Abs(((IntPtr)(void*)to).y - ((IntPtr)(void*)from).y);
		}

		[Pure]
		public unsafe static Vector2Int ToInt(this Vector2 value)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2Int((int)((IntPtr)(void*)value).x, (int)((IntPtr)(void*)value).y);
		}

		[Pure]
		public unsafe static Vector2Int RoundToInt(this Vector2 value)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2Int(Mathf.RoundToInt(((IntPtr)(void*)value).x), Mathf.RoundToInt(((IntPtr)(void*)value).y));
		}

		[Pure]
		public unsafe static Vector2Int FloorToInt(this Vector2 value)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2Int(Mathf.FloorToInt(((IntPtr)(void*)value).x), Mathf.FloorToInt(((IntPtr)(void*)value).y));
		}

		[Pure]
		public unsafe static Vector2Int CeilToInt(this Vector2 value)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2Int(Mathf.CeilToInt(((IntPtr)(void*)value).x), Mathf.CeilToInt(((IntPtr)(void*)value).y));
		}

		[Pure]
		public unsafe static Vector2 Clamp(this Vector2 value, float min, float max)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2(Mathf.Clamp(((IntPtr)(void*)value).x, min, max), Mathf.Clamp(((IntPtr)(void*)value).y, min, max));
		}

		[Pure]
		public unsafe static Vector2 Clamp(this Vector2 value, Vector2 min, Vector2 max)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2(Mathf.Clamp(((IntPtr)(void*)value).x, ((IntPtr)(void*)min).x, ((IntPtr)(void*)max).x), Mathf.Clamp(((IntPtr)(void*)value).y, ((IntPtr)(void*)min).y, ((IntPtr)(void*)max).y));
		}

		[Pure]
		public unsafe static Vector2 Rotate(this Vector2 v, float degrees)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			float num = degrees * (MathF.PI / 180f);
			float num2 = Mathf.Sin(num);
			float num3 = Mathf.Cos(num);
			float x = ((IntPtr)(void*)v).x;
			float y = ((IntPtr)(void*)v).y;
			return new Vector2(num3 * x - num2 * y, num2 * x + num3 * y);
		}

		[Pure]
		public unsafe static Vector2 Inverse(this Vector2 v)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2(1f / ((IntPtr)(void*)v).x, 1f / ((IntPtr)(void*)v).y);
		}

		[Pure]
		public unsafe static Vector2 Abs(this Vector2 v)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2(Mathf.Abs(((IntPtr)(void*)v).x), Mathf.Abs(((IntPtr)(void*)v).y));
		}
	}
}
