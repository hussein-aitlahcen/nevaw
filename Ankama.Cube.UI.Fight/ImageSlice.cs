using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Fight
{
	public class ImageSlice : Image
	{
		private static readonly Vector3[] s_Xy = (Vector3[])new Vector3[4];

		private static readonly Vector3[] s_Uv = (Vector3[])new Vector3[4];

		[SerializeField]
		[Range(0f, 1f)]
		private float m_fillStart;

		private Sprite activeSprite
		{
			get
			{
				if (!(this.get_overrideSprite() == null))
				{
					return this.get_overrideSprite();
				}
				return this.get_sprite();
			}
		}

		public float fillStart
		{
			get
			{
				return m_fillStart;
			}
			set
			{
				float num = Mathf.Clamp01(value);
				if (!EqualityComparer<float>.Default.Equals(m_fillStart, num))
				{
					m_fillStart = num;
					this.SetVerticesDirty();
				}
			}
		}

		protected override void OnPopulateMesh(VertexHelper toFill)
		{
			if (activeSprite == null)
			{
				this.OnPopulateMesh(toFill);
			}
			else
			{
				GenerateFilledSprite(toFill, this.get_preserveAspect());
			}
		}

		private unsafe Vector4 GetDrawingDimensions(bool shouldPreserveAspect)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			Vector4 val = (!(activeSprite == null)) ? DataUtility.GetPadding(activeSprite) : Vector4.get_zero();
			_003F val2;
			if (activeSprite == null)
			{
				val2 = Vector2.get_zero();
			}
			else
			{
				Rect rect = activeSprite.get_rect();
				float width = rect.get_width();
				rect = activeSprite.get_rect();
				val2 = new Vector2(width, rect.get_height());
			}
			Vector2 val3 = val2;
			Rect pixelAdjustedRect = this.GetPixelAdjustedRect();
			int num = Mathf.RoundToInt(((IntPtr)(void*)val3).x);
			int num2 = Mathf.RoundToInt(((IntPtr)(void*)val3).y);
			Vector4 val4 = default(Vector4);
			val4._002Ector(((IntPtr)(void*)val).x / (float)num, ((IntPtr)(void*)val).y / (float)num2, ((float)num - ((IntPtr)(void*)val).z) / (float)num, ((float)num2 - ((IntPtr)(void*)val).w) / (float)num2);
			if (shouldPreserveAspect && (double)val3.get_sqrMagnitude() > 0.0)
			{
				float num3 = ((IntPtr)(void*)val3).x / ((IntPtr)(void*)val3).y;
				float num4 = pixelAdjustedRect.get_width() / pixelAdjustedRect.get_height();
				if ((double)num3 > (double)num4)
				{
					float height = pixelAdjustedRect.get_height();
					pixelAdjustedRect.set_height(pixelAdjustedRect.get_width() * (1f / num3));
					pixelAdjustedRect.set_y(pixelAdjustedRect.get_y() + (height - pixelAdjustedRect.get_height()) * ((IntPtr)(void*)this.get_rectTransform().get_pivot()).y);
				}
				else
				{
					float width2 = pixelAdjustedRect.get_width();
					pixelAdjustedRect.set_width(pixelAdjustedRect.get_height() * num3);
					pixelAdjustedRect.set_x(pixelAdjustedRect.get_x() + (width2 - pixelAdjustedRect.get_width()) * ((IntPtr)(void*)this.get_rectTransform().get_pivot()).x);
				}
			}
			val4._002Ector(pixelAdjustedRect.get_x() + pixelAdjustedRect.get_width() * ((IntPtr)(void*)val4).x, pixelAdjustedRect.get_y() + pixelAdjustedRect.get_height() * ((IntPtr)(void*)val4).y, pixelAdjustedRect.get_x() + pixelAdjustedRect.get_width() * ((IntPtr)(void*)val4).z, pixelAdjustedRect.get_y() + pixelAdjustedRect.get_height() * ((IntPtr)(void*)val4).w);
			return val4;
		}

		private unsafe void GenerateFilledSprite(VertexHelper toFill, bool preserveAspect)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Invalid comparison between Unknown and I4
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_0212: Unknown result type (might be due to invalid IL or missing references)
			//IL_0218: Unknown result type (might be due to invalid IL or missing references)
			//IL_021e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0223: Unknown result type (might be due to invalid IL or missing references)
			//IL_0228: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_023f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_0254: Unknown result type (might be due to invalid IL or missing references)
			//IL_025a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0260: Unknown result type (might be due to invalid IL or missing references)
			//IL_0265: Unknown result type (might be due to invalid IL or missing references)
			//IL_026a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0277: Unknown result type (might be due to invalid IL or missing references)
			//IL_027c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0281: Unknown result type (might be due to invalid IL or missing references)
			//IL_028f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0294: Unknown result type (might be due to invalid IL or missing references)
			//IL_0299: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02be: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
			toFill.Clear();
			if ((double)this.get_fillAmount() < 0.001)
			{
				return;
			}
			Vector4 drawingDimensions = GetDrawingDimensions(preserveAspect);
			Vector4 val = (!(activeSprite != null)) ? Vector4.get_zero() : DataUtility.GetOuterUV(activeSprite);
			UIVertex.simpleVert.color = Color32.op_Implicit(this.get_color());
			float num = ((IntPtr)(void*)val).x;
			float num2 = ((IntPtr)(void*)val).y;
			float num3 = ((IntPtr)(void*)val).z;
			float num4 = ((IntPtr)(void*)val).w;
			float num5 = Mathf.Min(this.get_fillAmount(), 1f - m_fillStart);
			if ((int)this.get_fillMethod() == 0 || (int)this.get_fillMethod() == 1)
			{
				if ((int)this.get_fillMethod() == 0)
				{
					float num6 = num3 - num;
					float num7 = ((IntPtr)(void*)drawingDimensions).z - ((IntPtr)(void*)drawingDimensions).x;
					if (this.get_fillOrigin() == 0)
					{
						drawingDimensions.x = ((IntPtr)(void*)drawingDimensions).x + num7 * fillStart;
						drawingDimensions.z = ((IntPtr)(void*)drawingDimensions).x + num7 * num5;
						num += num6 * fillStart;
						num3 = num + num6 * num5;
					}
					else
					{
						drawingDimensions.z = ((IntPtr)(void*)drawingDimensions).z - num7 * fillStart;
						drawingDimensions.x = ((IntPtr)(void*)drawingDimensions).z - num7 * num5;
						num3 -= num6 * fillStart;
						num = num3 - num6 * num5;
					}
				}
				else
				{
					float num8 = num4 - num2;
					float num9 = ((IntPtr)(void*)drawingDimensions).w - ((IntPtr)(void*)drawingDimensions).y;
					if (this.get_fillOrigin() == 0)
					{
						drawingDimensions.y = ((IntPtr)(void*)drawingDimensions).y + num9 * fillStart;
						drawingDimensions.w = ((IntPtr)(void*)drawingDimensions).y + num9 * num5;
						num2 += num8 * fillStart;
						num4 = num2 + num8 * num5;
					}
					else
					{
						drawingDimensions.w = ((IntPtr)(void*)drawingDimensions).w - num9 * fillStart;
						drawingDimensions.y = ((IntPtr)(void*)drawingDimensions).w - num9 * num5;
						num4 -= num8 * fillStart;
						num2 = num4 - num8 * num5;
					}
				}
			}
			s_Xy[0] = Vector2.op_Implicit(new Vector2(((IntPtr)(void*)drawingDimensions).x, ((IntPtr)(void*)drawingDimensions).y));
			s_Xy[1] = Vector2.op_Implicit(new Vector2(((IntPtr)(void*)drawingDimensions).x, ((IntPtr)(void*)drawingDimensions).w));
			s_Xy[2] = Vector2.op_Implicit(new Vector2(((IntPtr)(void*)drawingDimensions).z, ((IntPtr)(void*)drawingDimensions).w));
			s_Xy[3] = Vector2.op_Implicit(new Vector2(((IntPtr)(void*)drawingDimensions).z, ((IntPtr)(void*)drawingDimensions).y));
			s_Uv[0] = Vector2.op_Implicit(new Vector2(num, num2));
			s_Uv[1] = Vector2.op_Implicit(new Vector2(num, num4));
			s_Uv[2] = Vector2.op_Implicit(new Vector2(num3, num4));
			s_Uv[3] = Vector2.op_Implicit(new Vector2(num3, num2));
			AddQuad(toFill, s_Xy, Color32.op_Implicit(this.get_color()), s_Uv);
		}

		private static void AddQuad(VertexHelper vertexHelper, Vector3[] quadPositions, Color32 color, Vector3[] quadUVs)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			int currentVertCount = vertexHelper.get_currentVertCount();
			for (int i = 0; i < 4; i++)
			{
				vertexHelper.AddVert(quadPositions[i], color, Vector2.op_Implicit(quadUVs[i]));
			}
			vertexHelper.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
			vertexHelper.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount);
		}

		public ImageSlice()
			: this()
		{
		}
	}
}
