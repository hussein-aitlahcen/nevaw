using Ankama.Cube.Extensions;
using UnityEngine;

namespace Ankama.Cube.Utility
{
	public static class GizmosUtility
	{
		public static void DrawArrow(Vector3 pos, Vector3 direction, float arrowLength, float arrowHeadLength = 0.3f, float arrowHeadAngle = 30f)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			Gizmos.DrawRay(pos, direction * arrowLength);
			Vector3 val = Quaternion.LookRotation(direction) * Quaternion.Euler(0f, 180f + arrowHeadAngle, 0f) * new Vector3(0f, 0f, 1f);
			Vector3 val2 = Quaternion.LookRotation(direction) * Quaternion.Euler(0f, 180f - arrowHeadAngle, 0f) * new Vector3(0f, 0f, 1f);
			Gizmos.DrawRay(pos + direction * arrowLength, val * arrowHeadLength);
			Gizmos.DrawRay(pos + direction * arrowLength, val2 * arrowHeadLength);
		}

		public static void DrawTransform(Transform transform)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			DrawArrow(transform.get_position(), transform.get_forward(), 4f);
			DrawArrow(transform.get_position(), transform.get_right(), 2f);
			DrawArrow(transform.get_position(), transform.get_up(), 2f);
		}

		public static void DrawTransform(Vector3 position, Quaternion rotation)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			DrawArrow(position, rotation * Vector3.get_forward(), 4f);
			DrawArrow(position, rotation * Vector3.get_right(), 2f);
			DrawArrow(position, rotation * Vector3.get_up(), 2f);
		}

		public static void DrawFrustrumPoints(Vector3[] frustrumPoints)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			Gizmos.DrawLine(frustrumPoints[0], frustrumPoints[1]);
			Gizmos.DrawLine(frustrumPoints[1], frustrumPoints[2]);
			Gizmos.DrawLine(frustrumPoints[2], frustrumPoints[3]);
			Gizmos.DrawLine(frustrumPoints[3], frustrumPoints[0]);
			Gizmos.DrawLine(frustrumPoints[4], frustrumPoints[5]);
			Gizmos.DrawLine(frustrumPoints[5], frustrumPoints[6]);
			Gizmos.DrawLine(frustrumPoints[6], frustrumPoints[7]);
			Gizmos.DrawLine(frustrumPoints[7], frustrumPoints[4]);
			Gizmos.DrawLine(frustrumPoints[0], frustrumPoints[4]);
			Gizmos.DrawLine(frustrumPoints[1], frustrumPoints[5]);
			Gizmos.DrawLine(frustrumPoints[2], frustrumPoints[6]);
			Gizmos.DrawLine(frustrumPoints[3], frustrumPoints[7]);
		}

		public static void DrawFrustrum(Camera cam)
		{
			Vector3[] points = (Vector3[])new Vector3[8];
			cam.GetFrustumPoints(ref points);
			DrawFrustrumPoints(points);
		}
	}
}
