using Ankama.Cube.UI;
using DG.Tweening;
using System;
using UnityEngine;

namespace Ankama.Cube.Maps
{
	public class ZaapObject : MonoBehaviour
	{
		public enum ZaapState
		{
			Normal,
			Highlight,
			Clicked,
			Open
		}

		[SerializeField]
		private ZaapAnimData m_animData;

		[SerializeField]
		private Transform m_positionToReach;

		[SerializeField]
		private Portal m_portalFX;

		private bool m_interactable = true;

		private bool m_mouseOver;

		private ZaapState m_state;

		public Action<ZaapObject> onClick;

		public Action<ZaapObject> onPortalBeginOpen;

		public Action<ZaapObject> onPortalEndOpen;

		public ZaapState state => m_state;

		public Vector3 destination => m_positionToReach.get_position();

		public Vector3 outsideDestination => m_positionToReach.get_position() - m_positionToReach.get_right();

		public Vector3 destinationLookAt => m_positionToReach.get_right();

		public bool waitingForCharacterToReach
		{
			get;
			set;
		}

		public bool interactable
		{
			get
			{
				return m_interactable;
			}
			set
			{
				m_interactable = value;
				if (!value && m_state == ZaapState.Highlight)
				{
					m_state = ZaapState.Normal;
					m_portalFX.SetState(m_state);
				}
			}
		}

		private void Awake()
		{
			m_portalFX.SetState(m_state);
		}

		public unsafe void UpdateCharacterPos(Vector3 worldPos)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			if (interactable && m_state == ZaapState.Clicked && Vector2.Distance(new Vector2(((IntPtr)(void*)worldPos).x, ((IntPtr)(void*)worldPos).z), new Vector2(((IntPtr)(void*)m_positionToReach.get_position()).x, ((IntPtr)(void*)m_positionToReach.get_position()).z)) < 0.05f)
			{
				OpenPortal();
			}
		}

		public void ClosePortal()
		{
			m_state = ZaapState.Normal;
			m_portalFX.SetState(m_state);
		}

		public void OnClickOutside()
		{
			m_state = ZaapState.Normal;
			m_portalFX.SetState(m_state);
		}

		private void OnMouseUpAsButton()
		{
			if (interactable && !InputUtility.IsMouseOverUI)
			{
				if (m_state == ZaapState.Clicked)
				{
					OpenPortal();
					return;
				}
				m_state = ZaapState.Clicked;
				m_portalFX.SetState(m_state);
				onClick?.Invoke(this);
			}
		}

		private unsafe void OpenPortal()
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Expected O, but got Unknown
			m_state = ZaapState.Open;
			m_portalFX.SetState(m_state);
			onPortalBeginOpen?.Invoke(this);
			DOVirtual.DelayedCall(m_animData.openCallbackDelay, new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), true);
		}

		private void OpenCallback()
		{
			onPortalEndOpen?.Invoke(this);
		}

		private void OnMouseOver()
		{
			if (m_mouseOver && InputUtility.IsMouseOverUI)
			{
				OnMouseExit();
			}
			else if (!m_mouseOver && !InputUtility.IsMouseOverUI)
			{
				OnMouseEnter();
			}
		}

		private void OnMouseEnter()
		{
			if (!InputUtility.IsMouseOverUI)
			{
				m_mouseOver = true;
				if (interactable && m_state == ZaapState.Normal)
				{
					m_state = ZaapState.Highlight;
					m_portalFX.SetState(m_state);
				}
			}
		}

		private void OnMouseExit()
		{
			m_mouseOver = false;
			if (interactable && m_state == ZaapState.Highlight)
			{
				m_state = ZaapState.Normal;
				m_portalFX.SetState(m_state);
			}
		}

		public ZaapObject()
			: this()
		{
		}
	}
}
