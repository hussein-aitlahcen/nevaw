using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ankama.Cube.UI.Components
{
	public class DragNDropListener : MonoBehaviour
	{
		[SerializeField]
		private Canvas m_canvas;

		[SerializeField]
		private RectTransform m_content;

		[SerializeField]
		private Camera m_camera;

		[SerializeField]
		private float m_scaleFactor = 1.2f;

		[SerializeField]
		private Ease m_scaleEase = 27;

		[SerializeField]
		private float m_scaleTweenDuration = 0.2f;

		[SerializeField]
		private Ease m_moveEase = 7;

		[SerializeField]
		private float m_moveTweenDuration = 0.2f;

		private RectTransform m_dragObject;

		private PointerEventData m_lastEvent;

		private Tween m_tweenViewPosition;

		private Vector2? m_snapScreenPosition;

		private Vector2 m_previousPosition;

		public bool dragging
		{
			get;
			private set;
		}

		public static DragNDropListener instance
		{
			get;
			private set;
		}

		public event Action OnDragBegin;

		public event Action OnDragEnd;

		private void Awake()
		{
			instance = this;
			Object.DontDestroyOnLoad(this.get_gameObject());
			m_canvas.get_gameObject().SetActive(false);
			m_camera.get_gameObject().SetActive(false);
		}

		public void CancelAll()
		{
		}

		private void UpdateCamera()
		{
			if (dragging)
			{
				float lastDepth = UIManager.instance.lastDepth;
				int lastSortingOrder = UIManager.instance.lastSortingOrder;
				m_canvas.set_sortingOrder(lastSortingOrder);
				m_canvas.set_planeDistance(lastDepth);
				m_camera.set_nearClipPlane(lastDepth);
				m_camera.set_farClipPlane(lastDepth + 1f);
				m_canvas.get_gameObject().SetActive(true);
				m_camera.get_gameObject().SetActive(true);
			}
			else
			{
				m_canvas.get_gameObject().SetActive(false);
				m_camera.get_gameObject().SetActive(false);
			}
		}

		public void SnapDragToScreenPosition(Vector2 position)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			m_snapScreenPosition = position;
		}

		public void SnapDragToWorldPosition(Camera cam, Vector3 position)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			m_snapScreenPosition = RectTransformUtility.WorldToScreenPoint(cam, position);
		}

		public void CancelSnapDrag()
		{
			m_snapScreenPosition = null;
		}

		public void OnBeginDrag(Vector2 screenPosition, Camera cam, RectTransform dragObject)
		{
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			if (m_dragObject != null)
			{
				return;
			}
			dragging = true;
			UpdateCamera();
			m_dragObject = dragObject;
			m_dragObject.SetParent(m_content, true);
			m_dragObject.set_anchoredPosition3D(Vector2.op_Implicit(m_dragObject.get_anchoredPosition()));
			TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(m_dragObject, m_scaleFactor, m_scaleTweenDuration), m_scaleEase);
			Vector2 val = ((_003F?)m_snapScreenPosition) ?? screenPosition;
			Vector2 val2 = default(Vector2);
			if (RectTransformUtility.ScreenPointToLocalPointInRectangle(m_content, val, cam, ref val2))
			{
				m_previousPosition = val2;
				Tween tweenViewPosition = m_tweenViewPosition;
				if (tweenViewPosition != null)
				{
					TweenExtensions.Kill(tweenViewPosition, false);
				}
				m_tweenViewPosition = TweenSettingsExtensions.SetEase<Tweener>(DOTweenModuleUI.DOAnchorPos3D(m_dragObject, Vector2.op_Implicit(val2), m_moveTweenDuration, false), m_moveEase);
			}
			this.OnDragBegin?.Invoke();
		}

		public unsafe void OnDrag(Vector2 screenPosition, Camera cam)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Expected O, but got Unknown
			if (!dragging)
			{
				return;
			}
			Vector2 val = ((_003F?)m_snapScreenPosition) ?? screenPosition;
			Vector2 val2 = default(Vector2);
			if (RectTransformUtility.ScreenPointToLocalPointInRectangle(m_content, val, cam, ref val2) && !(m_previousPosition == val2))
			{
				m_previousPosition = val2;
				Tween tweenViewPosition = m_tweenViewPosition;
				if (tweenViewPosition != null)
				{
					TweenExtensions.Kill(tweenViewPosition, false);
				}
				m_tweenViewPosition = TweenSettingsExtensions.SetEase<Tweener>(DOTweenModuleUI.DOAnchorPos3D(m_dragObject, Vector2.op_Implicit(val2), m_moveTweenDuration, false), m_moveEase);
				TweenSettingsExtensions.OnKill<Tween>(m_tweenViewPosition, new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			}
		}

		public void OnEndDrag()
		{
			if (dragging)
			{
				Tween tweenViewPosition = m_tweenViewPosition;
				if (tweenViewPosition != null)
				{
					TweenExtensions.Kill(tweenViewPosition, false);
				}
				dragging = false;
				m_dragObject = null;
				UpdateCamera();
				this.OnDragEnd?.Invoke();
			}
		}

		private void TweenPositionCompleteCallback()
		{
			m_tweenViewPosition = null;
		}

		public DragNDropListener()
			: this()
		{
		}//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)

	}
}
