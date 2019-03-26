using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ankama.Cube.UI.Components
{
	public class ItemDragNDropListener : MonoBehaviour
	{
		private readonly Dictionary<Type, List<DragNDropClient>> m_clients = new Dictionary<Type, List<DragNDropClient>>();

		private CellRenderer m_currentRenderer;

		private DragNDropClient m_sourceClient;

		private CellRenderer m_copy;

		private RectTransform m_copyTransform;

		private PointerEventData m_lastEvent;

		private List<DragNDropClient> m_candidates;

		private Tween m_tweenViewPosition;

		private Tween m_tweenDestroy;

		public bool dragging
		{
			get;
			private set;
		}

		public static ItemDragNDropListener instance
		{
			get;
			private set;
		}

		public object DraggedValue
		{
			get
			{
				if (!(m_currentRenderer == null))
				{
					return m_currentRenderer.value;
				}
				return null;
			}
		}

		public event Action OnDragBegin;

		public event Action OnDragEnd;

		public event Action OnDragEndSuccessful;

		private void Awake()
		{
			instance = this;
			Object.DontDestroyOnLoad(this.get_gameObject());
		}

		public void Register(DragNDropClient client, Type type)
		{
			if (!m_clients.TryGetValue(type, out List<DragNDropClient> value))
			{
				value = new List<DragNDropClient>();
				m_clients.Add(type, value);
			}
			value.Add(client);
		}

		public void UnRegister(DragNDropClient client, Type type)
		{
			if (!m_clients.TryGetValue(type, out List<DragNDropClient> value))
			{
				return;
			}
			if (client != null)
			{
				value.Remove(client);
				return;
			}
			for (int num = value.Count - 1; num >= 0; num--)
			{
				if (value[num] == null)
				{
					value.RemoveAt(num);
				}
			}
		}

		public void CancelAll()
		{
		}

		public unsafe void OnBeginDrag(PointerEventData eventData, CellRenderer cellRenderer)
		{
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			Tween tweenDestroy = m_tweenDestroy;
			if (tweenDestroy != null)
			{
				TweenExtensions.Kill(tweenDestroy, false);
			}
			if (!(m_currentRenderer != null))
			{
				dragging = true;
				m_currentRenderer = cellRenderer;
				m_sourceClient = cellRenderer.dragNDropClient;
				m_copy = cellRenderer.Clone();
				m_copy.dragNDropClient = null;
				m_copyTransform = m_copy.rectTransform;
				Vector3 localPosition = m_copyTransform.get_localPosition();
				Rect rect = m_copyTransform.get_rect();
				Vector2 val = default(Vector2);
				val._002Ector(0.5f, 0.5f);
				Vector2 pivot = m_copyTransform.get_pivot();
				m_copyTransform.set_anchorMin(new Vector2(0.5f, 0.5f));
				m_copyTransform.set_anchorMax(new Vector2(0.5f, 0.5f));
				m_copyTransform.set_pivot(val);
				m_copyTransform.set_sizeDelta(new Vector2(rect.get_width(), rect.get_height()));
				Vector3 val2 = Vector2.op_Implicit(val - pivot);
				Vector3 val3 = Vector2.op_Implicit(new Vector2(rect.get_width() * ((IntPtr)(void*)val2).x, rect.get_height() * ((IntPtr)(void*)val2).y));
				Vector3 localPosition2 = localPosition + val3;
				m_copyTransform.set_localPosition(localPosition2);
				DragNDropListener.instance.OnBeginDrag(eventData.get_position(), eventData.get_pressEventCamera(), m_copyTransform);
				m_clients.TryGetValue(cellRenderer.GetValueType(), out m_candidates);
				this.OnDragBegin?.Invoke();
			}
		}

		public void OnDrag(PointerEventData eventData)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			if (!dragging)
			{
				return;
			}
			DragNDropListener.instance.OnDrag(eventData.get_position(), eventData.get_pressEventCamera());
			int num = 0;
			int count = m_candidates.Count;
			DragNDropClient dragNDropClient;
			while (true)
			{
				if (num < count)
				{
					dragNDropClient = m_candidates[num];
					if (dragNDropClient.activeInHierarchy && RectTransformUtility.RectangleContainsScreenPoint(dragNDropClient.rectTransform, eventData.get_position(), eventData.get_pressEventCamera()))
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			dragNDropClient.OnDragOver(m_copy, eventData);
		}

		public unsafe void OnEndDrag(PointerEventData eventData)
		{
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Expected O, but got Unknown
			if (!dragging)
			{
				return;
			}
			m_lastEvent = eventData;
			bool flag = false;
			bool flag2 = true;
			bool flag3 = true;
			bool flag4 = true;
			int i = 0;
			for (int count = m_candidates.Count; i < count; i++)
			{
				DragNDropClient dragNDropClient = m_candidates[i];
				if (dragNDropClient.activeInHierarchy && RectTransformUtility.RectangleContainsScreenPoint(dragNDropClient.rectTransform, eventData.get_position(), eventData.get_pressEventCamera()))
				{
					flag2 = dragNDropClient.OnDrop(m_copy, eventData);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				flag2 = m_sourceClient.OnDropOut(m_copy, eventData);
				flag3 = !flag2;
			}
			else
			{
				this.OnDragEndSuccessful?.Invoke();
			}
			if (flag2)
			{
				if (flag3)
				{
					Object.Destroy(m_copy.get_gameObject());
				}
				else
				{
					m_tweenDestroy = m_copy.DestroySequence();
					if (m_tweenDestroy == null)
					{
						Object.Destroy(m_copy.get_gameObject());
					}
					else
					{
						TweenSettingsExtensions.OnKill<Tween>(m_tweenDestroy, new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
						flag4 = false;
					}
				}
			}
			if (flag4)
			{
				EndDragAction();
			}
		}

		private void OnDestroySequenceEnd()
		{
			Object.Destroy(m_copy.get_gameObject());
			EndDragAction();
		}

		private void EndDragAction()
		{
			dragging = false;
			m_currentRenderer = null;
			m_sourceClient = null;
			m_copy = null;
			m_tweenDestroy = null;
			DragNDropListener.instance.OnEndDrag();
			this.OnDragEnd?.Invoke();
		}

		public ItemDragNDropListener()
			: this()
		{
		}
	}
}
