using JetBrains.Annotations;
using System;
using UnityEngine;

namespace Ankama.Cube.Extensions
{
	public static class BoundsExtension
	{
		public unsafe static void GetPoints(this Bounds bounds, ref Vector3[] points)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			Vector3 extents = bounds.get_extents();
			Vector3 center = bounds.get_center();
			float x = ((IntPtr)(void*)extents).x;
			float y = ((IntPtr)(void*)extents).y;
			float z = ((IntPtr)(void*)extents).z;
			points[0] = center + new Vector3(0f - x, 0f - y, 0f - z);
			points[1] = center + new Vector3(0f - x, y, 0f - z);
			points[2] = center + new Vector3(x, y, 0f - z);
			points[3] = center + new Vector3(x, 0f - y, 0f - z);
			points[4] = center + new Vector3(0f - x, 0f - y, z);
			points[5] = center + new Vector3(0f - x, y, z);
			points[6] = center + new Vector3(x, y, z);
			points[7] = center + new Vector3(x, 0f - y, z);
		}

		public unsafe static Bounds GetScreenBounds(this Bounds worldBounds, [NotNull] Camera camera)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			Vector3 min = worldBounds.get_min();
			Vector3 max = worldBounds.get_max();
			Vector3 val = camera.WorldToScreenPoint(min);
			Vector3 val2 = camera.WorldToScreenPoint(max);
			Vector3 val3 = camera.WorldToScreenPoint(new Vector3(((IntPtr)(void*)min).x, 0f, ((IntPtr)(void*)max).z));
			Vector3 val4 = camera.WorldToScreenPoint(new Vector3(((IntPtr)(void*)max).x, 0f, ((IntPtr)(void*)min).z));
			float num = Mathf.Min(new float[4]
			{
				((IntPtr)(void*)val).x,
				((IntPtr)(void*)val2).x,
				((IntPtr)(void*)val3).x,
				((IntPtr)(void*)val4).x
			});
			float num2 = Mathf.Max(new float[4]
			{
				((IntPtr)(void*)val).x,
				((IntPtr)(void*)val2).x,
				((IntPtr)(void*)val3).x,
				((IntPtr)(void*)val4).x
			});
			float num3 = Mathf.Min(new float[4]
			{
				((IntPtr)(void*)val).y,
				((IntPtr)(void*)val2).y,
				((IntPtr)(void*)val3).y,
				((IntPtr)(void*)val4).y
			});
			float num4 = Mathf.Max(new float[4]
			{
				((IntPtr)(void*)val).y,
				((IntPtr)(void*)val2).y,
				((IntPtr)(void*)val3).y,
				((IntPtr)(void*)val4).y
			});
			Bounds result = default(Bounds);
			result.SetMinMax(new Vector3(num, num3, 0f), new Vector3(num2, num4, 0f));
			return result;
		}

		public static bool Contains(this Bounds bounds, Bounds value)
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			if (bounds.Contains(value.get_min()))
			{
				return bounds.Contains(value.get_max());
			}
			return false;
		}

		public static Vector3 ProjectPoint(this Bounds bounds, Vector3 point, Vector3 origin)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			if (bounds.Contains(point))
			{
				return point;
			}
			Ray val = default(Ray);
			val._002Ector(origin, origin - point);
			float num = default(float);
			if (!bounds.IntersectRay(val, ref num))
			{
				return bounds.ClosestPoint(point);
			}
			return val.GetPoint(num);
		}
	}
}
