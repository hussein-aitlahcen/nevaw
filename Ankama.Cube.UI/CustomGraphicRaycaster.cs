using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ankama.Cube.UI
{
	[AddComponentMenu("Event/Custom Graphic Raycaster")]
	[RequireComponent(typeof(Canvas))]
	public class CustomGraphicRaycaster : BaseRaycaster
	{
		private Canvas m_Canvas;

		[NonSerialized]
		private List<Graphic> m_RaycastResults = new List<Graphic>();

		[NonSerialized]
		private static readonly List<Graphic> s_SortedGraphics = new List<Graphic>();

		public override int sortOrderPriority
		{
			get
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				if ((int)canvas.get_renderMode() == 0)
				{
					return canvas.get_sortingOrder();
				}
				return this.get_sortOrderPriority();
			}
		}

		public override int renderOrderPriority
		{
			get
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Invalid comparison between Unknown and I4
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				if ((int)canvas.get_renderMode() == 1)
				{
					return -Mathf.RoundToInt(canvas.get_rootCanvas().get_planeDistance());
				}
				if ((int)canvas.get_renderMode() == 0)
				{
					return canvas.get_rootCanvas().get_renderOrder();
				}
				return this.get_renderOrderPriority();
			}
		}

		private Canvas canvas
		{
			get
			{
				if (m_Canvas != null)
				{
					return m_Canvas;
				}
				m_Canvas = this.GetComponent<Canvas>();
				return m_Canvas;
			}
		}

		public override Camera eventCamera
		{
			get
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Invalid comparison between Unknown and I4
				if ((int)canvas.get_renderMode() == 0 || ((int)canvas.get_renderMode() == 1 && canvas.get_worldCamera() == null))
				{
					return null;
				}
				if (!(canvas.get_worldCamera() != null))
				{
					return Camera.get_main();
				}
				return canvas.get_worldCamera();
			}
		}

		protected CustomGraphicRaycaster()
			: this()
		{
		}

		public unsafe override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0203: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			//IL_029f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
			if (canvas == null)
			{
				return;
			}
			IList<Graphic> graphicsForCanvas = GraphicRegistry.GetGraphicsForCanvas(canvas);
			if (graphicsForCanvas == null || graphicsForCanvas.Count == 0)
			{
				return;
			}
			Camera eventCamera = this.get_eventCamera();
			int num = ((int)canvas.get_renderMode() != 0 && !(eventCamera == null)) ? eventCamera.get_targetDisplay() : canvas.get_targetDisplay();
			Vector3 val = Display.RelativeMouseAt(Vector2.op_Implicit(eventData.get_position()));
			if (val != Vector3.get_zero())
			{
				if ((int)((IntPtr)(void*)val).z != num)
				{
					return;
				}
			}
			else
			{
				val = Vector2.op_Implicit(eventData.get_position());
			}
			Vector2 val2 = default(Vector2);
			if (eventCamera == null)
			{
				float num2 = Screen.get_width();
				float num3 = Screen.get_height();
				if (num > 0 && num < Display.displays.Length)
				{
					num2 = Display.displays[num].get_systemWidth();
					num3 = Display.displays[num].get_systemHeight();
				}
				val2._002Ector(((IntPtr)(void*)val).x / num2, ((IntPtr)(void*)val).y / num3);
			}
			else
			{
				val2 = Vector2.op_Implicit(eventCamera.ScreenToViewportPoint(val));
			}
			if (((IntPtr)(void*)val2).x < 0f || ((IntPtr)(void*)val2).x > 1f || ((IntPtr)(void*)val2).y < 0f || ((IntPtr)(void*)val2).y > 1f)
			{
				return;
			}
			float num4 = float.MaxValue;
			Ray val3 = default(Ray);
			if (eventCamera != null)
			{
				val3 = eventCamera.ScreenPointToRay(val);
			}
			m_RaycastResults.Clear();
			Raycast(canvas, eventCamera, Vector2.op_Implicit(val), graphicsForCanvas, m_RaycastResults);
			int count = m_RaycastResults.Count;
			for (int i = 0; i < count; i++)
			{
				GameObject gameObject = m_RaycastResults[i].get_gameObject();
				if (1 == 0)
				{
					continue;
				}
				float num5 = 0f;
				if (eventCamera == null || (int)canvas.get_renderMode() == 0)
				{
					num5 = 0f;
				}
				else
				{
					Transform transform = gameObject.get_transform();
					Vector3 forward = transform.get_forward();
					num5 = Vector3.Dot(forward, transform.get_position() - eventCamera.get_transform().get_position()) / Vector3.Dot(forward, val3.get_direction());
					if (num5 < 0f)
					{
						continue;
					}
				}
				if (!(num5 >= num4))
				{
					RaycastResult val4 = default(RaycastResult);
					val4.set_gameObject(gameObject);
					val4.module = this;
					val4.distance = num5;
					val4.screenPosition = Vector2.op_Implicit(val);
					val4.index = resultAppendList.Count;
					val4.depth = m_RaycastResults[i].get_depth();
					val4.sortingLayer = canvas.get_sortingLayerID();
					val4.sortingOrder = canvas.get_sortingOrder();
					RaycastResult item = val4;
					resultAppendList.Add(item);
				}
			}
		}

		private unsafe static void Raycast(Canvas canvas, Camera eventCamera, Vector2 pointerPosition, IList<Graphic> foundGraphics, List<Graphic> results)
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			int count = foundGraphics.Count;
			for (int i = 0; i < count; i++)
			{
				Graphic val = foundGraphics[i];
				if (val.get_depth() != -1 && val.get_raycastTarget() && !val.get_canvasRenderer().get_cull() && RectTransformUtility.RectangleContainsScreenPoint(val.get_rectTransform(), pointerPosition, eventCamera) && (!(eventCamera != null) || !(((IntPtr)(void*)eventCamera.WorldToScreenPoint(val.get_rectTransform().get_position())).z > eventCamera.get_farClipPlane())) && val.Raycast(pointerPosition, eventCamera))
				{
					s_SortedGraphics.Add(val);
				}
			}
			s_SortedGraphics.Sort((Graphic g1, Graphic g2) => g2.get_depth().CompareTo(g1.get_depth()));
			count = s_SortedGraphics.Count;
			for (int j = 0; j < count; j++)
			{
				results.Add(s_SortedGraphics[j]);
			}
			s_SortedGraphics.Clear();
		}
	}
}
