using Ankama.Cube.Data;
using Ankama.Cube.Fight;
using Ankama.Cube.UI;
using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ankama.Cube.Maps
{
	public class FightMapInputHandler
	{
		private readonly Camera m_camera;

		private readonly CameraHandler m_cameraHandler;

		private readonly Collider m_collider;

		private bool m_forceUpdate;

		private bool m_mouseButtonIsDown;

		private Vector3 m_previousMousePosition;

		public Vector2Int? targetCell
		{
			get;
			private set;
		}

		public bool pressedMouseButton
		{
			get;
			private set;
		}

		public bool releasedMouseButton
		{
			get;
			private set;
		}

		public bool clickedMouseButton
		{
			get;
			private set;
		}

		public Vector2Int? mouseButtonPressLocation
		{
			get;
			private set;
		}

		public Vector2Int? mouseButtonReleaseLocation
		{
			get;
			private set;
		}

		public FightMapInputHandler([NotNull] Collider collider, [CanBeNull] CameraHandler cameraHandler)
		{
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			if (null == collider)
			{
				throw new ArgumentNullException("collider");
			}
			m_camera = ((null == cameraHandler) ? Camera.get_main() : cameraHandler.camera);
			m_cameraHandler = cameraHandler;
			m_collider = collider;
			m_previousMousePosition = InputUtility.pointerPosition;
			if (cameraHandler != null)
			{
				cameraHandler.onMoved = (Action<CameraHandler>)Delegate.Combine(cameraHandler.onMoved, new Action<CameraHandler>(ForceUpdate));
				cameraHandler.onZoomChanged = (Action<CameraHandler>)Delegate.Combine(cameraHandler.onZoomChanged, new Action<CameraHandler>(ForceUpdate));
			}
		}

		public void SetDirty()
		{
			m_forceUpdate = true;
		}

		private void ForceUpdate(CameraHandler cameraHandler)
		{
			m_forceUpdate = true;
		}

		public unsafe bool Update(IMapDefinition mapDefinition)
		{
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_021e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0223: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			//IL_0271: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_033a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0341: Unknown result type (might be due to invalid IL or missing references)
			EventSystem current = EventSystem.get_current();
			if (null != current && current.IsPointerOverGameObject())
			{
				if (InputUtility.GetPointerUp())
				{
					int clickedMouseButton;
					if (m_mouseButtonIsDown)
					{
						Vector2Int? mouseButtonPressLocation = this.mouseButtonPressLocation;
						Vector2Int? targetCell = this.targetCell;
						clickedMouseButton = ((mouseButtonPressLocation.HasValue == targetCell.HasValue && (!mouseButtonPressLocation.HasValue || mouseButtonPressLocation.GetValueOrDefault() == targetCell.GetValueOrDefault())) ? 1 : 0);
					}
					else
					{
						clickedMouseButton = 0;
					}
					this.clickedMouseButton = ((byte)clickedMouseButton != 0);
					pressedMouseButton = false;
					releasedMouseButton = true;
					this.mouseButtonPressLocation = null;
					mouseButtonReleaseLocation = this.targetCell;
					m_mouseButtonIsDown = false;
				}
				else
				{
					this.clickedMouseButton = false;
					pressedMouseButton = false;
					releasedMouseButton = false;
				}
				if (this.targetCell.HasValue)
				{
					this.targetCell = null;
					return true;
				}
				return false;
			}
			bool result = false;
			Vector3 pointerPosition = InputUtility.pointerPosition;
			bool flag = pointerPosition != m_previousMousePosition;
			if (null != m_cameraHandler && m_cameraHandler.userHasControl)
			{
				Rect pixelRect = m_camera.get_pixelRect();
				if (pixelRect.Contains(pointerPosition))
				{
					if (InputUtility.GetTertiaryDown())
					{
						FightStatus local = FightStatus.local;
						if (local != null)
						{
							m_cameraHandler.StartFocusOnMapRegion(mapDefinition, local.fightId);
						}
						else
						{
							m_cameraHandler.StartFocusOnMapRegion(mapDefinition, 0);
						}
					}
					else if (InputUtility.IsSecondaryDown())
					{
						if (flag)
						{
							m_cameraHandler.Pan(Vector2.op_Implicit(pointerPosition), Vector2.op_Implicit(m_previousMousePosition));
						}
					}
					else
					{
						float y = ((IntPtr)(void*)Input.get_mouseScrollDelta()).y;
						if (Math.Abs(y) > float.Epsilon)
						{
							m_cameraHandler.TweenZoom(y);
						}
					}
				}
			}
			if (flag || m_forceUpdate)
			{
				if (null != m_camera)
				{
					Ray val = m_camera.ScreenPointToRay(pointerPosition);
					RaycastHit val2 = default(RaycastHit);
					if (m_collider.Raycast(val, ref val2, m_camera.get_farClipPlane()))
					{
						Vector3 val3 = val2.get_point() - m_collider.get_transform().get_position();
						int num = Mathf.RoundToInt(((IntPtr)(void*)val3).x);
						int num2 = Mathf.RoundToInt(((IntPtr)(void*)val3).z);
						Vector2Int val4 = default(Vector2Int);
						val4._002Ector(num, num2);
						if (!this.targetCell.HasValue || this.targetCell.Value != val4)
						{
							this.targetCell = val4;
							result = true;
						}
					}
					else if (this.targetCell.HasValue)
					{
						this.targetCell = null;
						result = true;
					}
				}
				m_forceUpdate = false;
				m_previousMousePosition = pointerPosition;
			}
			if (InputUtility.GetPointerDown())
			{
				pressedMouseButton = true;
				releasedMouseButton = false;
				this.clickedMouseButton = false;
				this.mouseButtonPressLocation = this.targetCell;
				mouseButtonReleaseLocation = null;
				m_mouseButtonIsDown = true;
			}
			else if (InputUtility.GetPointerUp())
			{
				int clickedMouseButton2;
				if (m_mouseButtonIsDown)
				{
					Vector2Int? targetCell = this.mouseButtonPressLocation;
					Vector2Int? mouseButtonPressLocation = this.targetCell;
					clickedMouseButton2 = ((targetCell.HasValue == mouseButtonPressLocation.HasValue && (!targetCell.HasValue || targetCell.GetValueOrDefault() == mouseButtonPressLocation.GetValueOrDefault())) ? 1 : 0);
				}
				else
				{
					clickedMouseButton2 = 0;
				}
				this.clickedMouseButton = ((byte)clickedMouseButton2 != 0);
				pressedMouseButton = false;
				releasedMouseButton = true;
				this.mouseButtonPressLocation = null;
				mouseButtonReleaseLocation = this.targetCell;
				m_mouseButtonIsDown = false;
			}
			else
			{
				this.clickedMouseButton = false;
				pressedMouseButton = false;
				releasedMouseButton = false;
			}
			return result;
		}
	}
}
