using Ankama.Cube.UI.Components;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ankama.Cube.UI.DeckMaker
{
	public abstract class CastableDnd : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField]
		protected RectTransform m_content;

		[SerializeField]
		protected RectTransform m_subContent;

		[SerializeField]
		protected CanvasGroup m_canvasGroup;

		private DndElementState m_elementState;

		private bool m_buttonPressed;

		private bool m_onTarget;

		private bool m_skipEndDragEvent;

		protected Tween m_animationTween;

		protected Transform m_contentParent;

		private Vector3 m_contentPosition;

		private Vector2 m_contentAnchorMin;

		private Vector2 m_contentAnchorMax;

		private Vector2 m_contentPivot;

		private Vector2 m_contentSizeDelta;

		private bool m_enableDnd;

		private DndCastBehaviour m_castBehaviour;

		private bool m_contentParametersInitialized;

		private bool m_draggingInternal;

		private bool m_wasOnTarget;

		public bool enableDnd
		{
			set
			{
				m_enableDnd = value;
			}
		}

		public DndCastBehaviour castBehaviour
		{
			set
			{
				m_castBehaviour = value;
			}
		}

		private Camera m_camera => UIManager.instance.GetCamera().camera;

		public bool SkipEndDragEvent
		{
			set
			{
				m_skipEndDragEvent = value;
			}
		}

		public event Func<bool> OnDragBeginRequest;

		public event Action<bool> OnDragBegin;

		public event Action<bool> OnDragEnd;

		public void OnEnterTarget()
		{
			if (!m_onTarget && (m_elementState == DndElementState.Drag || m_elementState == DndElementState.SimulatedDrag))
			{
				if (m_elementState == DndElementState.Drag)
				{
					m_elementState = DndElementState.DragTargeting;
				}
				if (m_elementState == DndElementState.SimulatedDrag)
				{
					m_elementState = DndElementState.SimulatedDragTargeting;
				}
				m_onTarget = true;
				Tween animationTween = m_animationTween;
				if (animationTween != null)
				{
					TweenExtensions.Kill(animationTween, false);
				}
				m_animationTween = OnEnterTargetTween();
			}
		}

		public void OnExitTarget()
		{
			if (m_onTarget && (m_elementState == DndElementState.DragTargeting || m_elementState == DndElementState.SimulatedDragTargeting))
			{
				if (m_elementState == DndElementState.DragTargeting)
				{
					m_elementState = DndElementState.Drag;
				}
				if (m_elementState == DndElementState.SimulatedDragTargeting)
				{
					m_elementState = DndElementState.SimulatedDrag;
				}
				m_onTarget = false;
				Tween animationTween = m_animationTween;
				if (animationTween != null)
				{
					TweenExtensions.Kill(animationTween, false);
				}
				m_animationTween = OnExitTargetTween();
			}
		}

		public void StartCast()
		{
			m_elementState = DndElementState.Casting;
		}

		public void CancelCast()
		{
			EndDrag(force: true, DndCastBehaviour.MoveBack);
		}

		public void DoneCasting()
		{
			EndDrag(force: true, m_castBehaviour);
		}

		private void Update()
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			bool flag = m_elementState == DndElementState.SimulatedDrag || m_elementState == DndElementState.SimulatedDragTargeting;
			if (flag)
			{
				DragNDropListener.instance.OnDrag(Vector2.op_Implicit(Input.get_mousePosition()), m_camera);
			}
			if (!m_skipEndDragEvent)
			{
				if (m_buttonPressed && InputUtility.GetPointerUp())
				{
					m_buttonPressed = false;
					EndDrag(force: false, DndCastBehaviour.MoveBack);
				}
				else if (flag && InputUtility.GetPointerDown())
				{
					m_buttonPressed = true;
				}
			}
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			m_draggingInternal = true;
			if (!DragNDropListener.instance.dragging && m_enableDnd && m_elementState == DndElementState.Idle && (this.OnDragBeginRequest == null || this.OnDragBeginRequest()))
			{
				m_elementState = DndElementState.Drag;
				BeginDrag();
				DragNDropListener.instance.OnBeginDrag(eventData.get_position(), m_camera, m_content);
			}
		}

		public void OnDrag(PointerEventData eventData)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			if (m_elementState == DndElementState.Drag || m_elementState == DndElementState.DragTargeting)
			{
				DragNDropListener.instance.OnDrag(eventData.get_position(), m_camera);
			}
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			m_draggingInternal = false;
			if (!m_skipEndDragEvent && (m_elementState == DndElementState.Drag || m_elementState == DndElementState.DragTargeting))
			{
				EndDrag(force: false, DndCastBehaviour.MoveBack);
			}
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			if (!m_draggingInternal && !DragNDropListener.instance.dragging && m_enableDnd && m_elementState == DndElementState.Idle && (this.OnDragBeginRequest == null || this.OnDragBeginRequest()))
			{
				m_elementState = DndElementState.SimulatedDrag;
				BeginDrag();
				DragNDropListener.instance.OnBeginDrag(eventData.get_position(), m_camera, m_content);
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (m_enableDnd && !DragNDropListener.instance.dragging)
			{
				Tween animationTween = m_animationTween;
				if (animationTween != null)
				{
					TweenExtensions.Kill(animationTween, false);
				}
				m_animationTween = OnPointerEnterTween();
			}
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (m_enableDnd && !DragNDropListener.instance.dragging)
			{
				Tween animationTween = m_animationTween;
				if (animationTween != null)
				{
					TweenExtensions.Kill(animationTween, false);
				}
				m_animationTween = OnPointerExitTween();
			}
		}

		protected unsafe void InitMove()
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			m_contentParent = m_content.get_parent();
			m_contentPosition = m_content.get_localPosition();
			m_contentAnchorMin = m_content.get_anchorMin();
			m_contentAnchorMax = m_content.get_anchorMax();
			m_contentPivot = m_content.get_pivot();
			m_contentSizeDelta = m_content.get_sizeDelta();
			m_contentParametersInitialized = true;
			Vector2 val = default(Vector2);
			val._002Ector(0.5f, 0f);
			Rect rect = m_content.get_rect();
			Vector2 val2 = val - m_content.get_pivot();
			Vector2 val3 = default(Vector2);
			val3._002Ector(rect.get_width() * ((IntPtr)(void*)val2).x, rect.get_height() * ((IntPtr)(void*)val2).y);
			m_content.set_anchorMin(new Vector2(0.5f, 0.5f));
			m_content.set_anchorMax(new Vector2(0.5f, 0.5f));
			m_content.set_pivot(val);
			m_content.set_sizeDelta(rect.get_size());
			m_content.set_anchoredPosition(new Vector2(m_contentPosition.x + ((IntPtr)(void*)val3).x, m_contentPosition.y + ((IntPtr)(void*)val3).y));
			Rect rect2 = m_subContent.get_rect();
			m_subContent.set_anchorMin(new Vector2(0.5f, 0.5f));
			m_subContent.set_anchorMax(new Vector2(0.5f, 0.5f));
			m_subContent.set_pivot(new Vector2(0.5f, 0.5f));
			m_subContent.set_anchoredPosition(new Vector2(0f, 0f));
			m_subContent.set_sizeDelta(rect2.get_size());
		}

		private void BeginDrag()
		{
			Tween animationTween = m_animationTween;
			if (animationTween != null)
			{
				TweenExtensions.Kill(animationTween, false);
			}
			InitMove();
			this.OnDragBegin?.Invoke(m_elementState == DndElementState.SimulatedDrag);
		}

		private unsafe void EndDrag(bool force, DndCastBehaviour behaviour)
		{
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Expected O, but got Unknown
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Expected O, but got Unknown
			if (m_elementState != DndElementState.Drag && m_elementState != DndElementState.SimulatedDrag && !force)
			{
				return;
			}
			DragNDropListener.instance.OnEndDrag();
			m_elementState = DndElementState.Idle;
			m_content.SetParent(m_contentParent);
			if (behaviour == DndCastBehaviour.MoveBack)
			{
				Rect rect = m_content.get_rect();
				Vector2 anchoredPosition = m_content.get_anchoredPosition();
				Vector2 val = m_contentPivot - m_content.get_pivot();
				Vector2 val2 = default(Vector2);
				val2._002Ector(rect.get_width() * ((IntPtr)(void*)val).x, rect.get_height() * ((IntPtr)(void*)val).y);
				m_content.set_anchorMin(m_contentAnchorMin);
				m_content.set_anchorMax(m_contentAnchorMax);
				m_content.set_sizeDelta(m_contentSizeDelta);
				m_content.set_pivot(m_contentPivot);
				m_content.set_anchoredPosition(new Vector2(((IntPtr)(void*)anchoredPosition).x + ((IntPtr)(void*)val2).x, ((IntPtr)(void*)anchoredPosition).y + ((IntPtr)(void*)val2).y));
				m_subContent.set_anchorMin(Vector2.get_zero());
				m_subContent.set_anchorMax(Vector2.get_one());
				m_subContent.set_sizeDelta(Vector2.get_zero());
				Tween animationTween = m_animationTween;
				if (animationTween != null)
				{
					TweenExtensions.Kill(animationTween, false);
				}
				Sequence val3 = DOTween.Sequence();
				TweenSettingsExtensions.Insert(val3, 0f, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMove(m_content, m_contentPosition, 0.3f, true), 18));
				TweenSettingsExtensions.Insert(val3, 0f, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(m_content, Vector3.get_one(), 0.3f), 27));
				TweenSettingsExtensions.Insert(val3, 0f, DOTweenModuleUI.DOAnchorPos(m_subContent, Vector2.op_Implicit(Vector3.get_zero()), 0.3f, false));
				TweenSettingsExtensions.Insert(val3, 0f, ShortcutExtensions.DOLocalRotate(m_subContent, Vector3.get_zero(), 0.3f, 0));
				m_animationTween = TweenSettingsExtensions.OnKill<Sequence>(val3, new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			}
			else
			{
				m_animationTween = TweenSettingsExtensions.OnKill<Tweener>(DOTweenModuleUI.DOFade(m_canvasGroup, 0f, 0.3f), new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			}
			m_wasOnTarget = m_onTarget;
			m_buttonPressed = false;
			m_onTarget = false;
		}

		private void OnEndDragEnd()
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			this.OnDragEnd?.Invoke(m_wasOnTarget);
			m_content.set_localPosition(m_contentPosition);
			m_content.set_sizeDelta(m_contentSizeDelta);
			m_content.set_localScale(Vector3.get_one());
			m_subContent.set_localRotation(Quaternion.Euler(Vector3.get_zero()));
			m_subContent.set_anchoredPosition(Vector2.get_zero());
		}

		public void ResetContentPosition()
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			m_content.get_gameObject().SetActive(true);
			m_canvasGroup.set_alpha(1f);
			if (m_contentParametersInitialized)
			{
				m_contentParametersInitialized = false;
				m_content.set_anchorMin(m_contentAnchorMin);
				m_content.set_anchorMax(m_contentAnchorMax);
				m_content.set_pivot(m_contentPivot);
				m_content.set_localPosition(m_contentPosition);
				m_content.set_sizeDelta(m_contentSizeDelta);
				m_content.set_localScale(Vector3.get_one());
				m_subContent.set_localRotation(Quaternion.Euler(Vector3.get_zero()));
				m_subContent.set_anchorMin(Vector2.get_zero());
				m_subContent.set_anchorMax(Vector2.get_one());
				m_subContent.set_anchoredPosition(Vector2.get_zero());
				m_subContent.set_sizeDelta(Vector2.get_zero());
			}
		}

		protected abstract Tween OnPointerEnterTween();

		protected abstract Tween OnPointerExitTween();

		protected abstract Tween OnEnterTargetTween();

		protected abstract Tween OnExitTargetTween();

		protected CastableDnd()
			: this()
		{
		}
	}
}
