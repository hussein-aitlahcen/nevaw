using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.Demo.UI
{
	[RequireComponent(typeof(Image))]
	public class PanelMeshEffect : BaseMeshEffect
	{
		private static Vector3[] s_vertexs = (Vector3[])new Vector3[4];

		[SerializeField]
		private Image m_image;

		[SerializeField]
		private float m_diagonal;

		public unsafe override void ModifyMesh(VertexHelper vh)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			if (this.IsActive() && !(m_image == null))
			{
				Color color = m_image.get_color();
				Sprite sprite = m_image.get_sprite();
				vh.Clear();
				Rect pixelAdjustedRect = m_image.GetPixelAdjustedRect();
				float pixelsPerUnit = m_image.get_pixelsPerUnit();
				Rect rect = sprite.get_rect();
				float num = rect.get_width() / pixelsPerUnit;
				rect = sprite.get_rect();
				float num2 = rect.get_height() / pixelsPerUnit;
				object obj = (object)(this.get_transform() as RectTransform);
				float num3 = ((IntPtr)(void*)obj.get_pivot()).x * num;
				float num4 = ((IntPtr)(void*)obj.get_pivot()).y * num2;
				Vector4 val = default(Vector4);
				val._002Ector(0f - num3, 0f - num4, num - num3, num2 - num4);
				Vector4 val2 = default(Vector4);
				val2._002Ector(Math.Max(((IntPtr)(void*)val).x, pixelAdjustedRect.get_xMin()), ((IntPtr)(void*)val).y, Math.Min(((IntPtr)(void*)val).z, pixelAdjustedRect.get_xMax()), ((IntPtr)(void*)val).w);
				s_vertexs = (Vector3[])new Vector3[4]
				{
					new Vector3(((IntPtr)(void*)val2).x, ((IntPtr)(void*)val2).y),
					new Vector3(((IntPtr)(void*)val2).x + m_diagonal, ((IntPtr)(void*)val2).w),
					new Vector3(((IntPtr)(void*)val2).z, ((IntPtr)(void*)val2).w),
					new Vector3(((IntPtr)(void*)val2).z - m_diagonal, ((IntPtr)(void*)val2).y)
				};
				Vector2 val3 = default(Vector2);
				for (int i = 0; i < 4; i++)
				{
					val3._002Ector((s_vertexs[i].x - ((IntPtr)(void*)val).x) / num, (s_vertexs[i].y - ((IntPtr)(void*)val).y) / num2);
					vh.AddVert(s_vertexs[i], Color32.op_Implicit(color), val3);
				}
				vh.AddTriangle(0, 1, 2);
				vh.AddTriangle(2, 3, 0);
			}
		}

		public PanelMeshEffect()
			: this()
		{
		}
	}
}
