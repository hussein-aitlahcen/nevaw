using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Components
{
	[RequireComponent(typeof(RectMask2D))]
	public abstract class List : MonoBehaviour, DragNDropClient, IBeginDragHandler, IEventSystemHandler, IDragHandler, IEndDragHandler
	{
		[Header("Cell")]
		[SerializeField]
		protected GameObject m_prefab;

		[Header("List configuration")]
		[SerializeField]
		protected bool m_horizontal;

		[SerializeField]
		protected Vector2Int m_cellSize;

		private RectTransform m_rectTransform;

		private Type m_itemType;

		private CellRenderer m_cellRenderer;

		protected int m_totalWidth;

		protected int m_totalHeight;

		private readonly List<CellRenderer> m_elementPool = new List<CellRenderer>();

		protected readonly List<object> m_items = new List<object>();

		protected readonly Dictionary<int, CellRenderer> m_elementsByItem = new Dictionary<int, CellRenderer>();

		protected bool m_needFullReLayout;

		protected int m_extraLineCount = 1;

		[NonSerialized]
		private bool m_initialized;

		public float scrollPercentage
		{
			protected get;
			set;
		}

		public RectTransform rectTransform => m_rectTransform;

		public bool activeInHierarchy => this.get_gameObject().get_activeInHierarchy();

		public void SetValues<T>(IEnumerable<T> values) where T : class
		{
			CheckInit();
			if (!m_itemType.IsAssignableFrom(typeof(T)))
			{
				Debug.LogWarningFormat("Wrong value type set in list {0}. Expected : {1}. Got {2}", new object[3]
				{
					this.get_name(),
					m_itemType.Name,
					typeof(T).Name
				});
				return;
			}
			m_items.Clear();
			m_items.AddRange(values);
			ComputeDimensions();
		}

		protected virtual void Awake()
		{
			CheckInit();
		}

		protected virtual void CheckInit()
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			if (!m_initialized)
			{
				m_rectTransform = this.GetComponent<RectTransform>();
				if (m_cellSize.get_x() <= 0 || m_cellSize.get_y() <= 0)
				{
					m_cellSize = new Vector2Int(10, 10);
				}
				m_cellRenderer = m_prefab.GetComponent<CellRenderer>();
				if (m_cellRenderer == null)
				{
					Debug.LogWarningFormat("No valid ItemElement found in the prefab {0} for list {1}", new object[2]
					{
						m_prefab.get_name(),
						this.get_name()
					});
					return;
				}
				m_itemType = m_cellRenderer.GetValueType();
				ItemDragNDropListener.instance.Register(this, m_itemType);
				m_initialized = true;
			}
		}

		private void OnRectTransformDimensionsChange()
		{
			ComputeDimensions();
		}

		protected virtual void ComputeDimensions()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			CheckInit();
			Rect rect = m_rectTransform.get_rect();
			m_totalWidth = Math.Max(1, (int)rect.get_width());
			m_totalHeight = Math.Max(1, (int)rect.get_height());
		}

		protected void ReturnToPool(CellRenderer cellRenderer)
		{
			if (!(cellRenderer == null))
			{
				m_elementPool.Add(cellRenderer);
				cellRenderer.get_gameObject().SetActive(false);
			}
		}

		protected CellRenderer GetFromPool()
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			if (m_elementPool.Count == 0)
			{
				CellRenderer cellRenderer = Object.Instantiate<CellRenderer>(m_cellRenderer);
				RectTransform component = cellRenderer.GetComponent<RectTransform>();
				component.SetParent(this.get_transform(), false);
				component.set_sizeDelta(Vector2Int.op_Implicit(m_cellSize));
				SetCellRectTransformAnchors(component);
				cellRenderer.dragNDropClient = this;
				return cellRenderer;
			}
			CellRenderer cellRenderer2 = m_elementPool[m_elementPool.Count - 1];
			m_elementPool.RemoveAt(m_elementPool.Count - 1);
			cellRenderer2.get_gameObject().SetActive(true);
			return cellRenderer2;
		}

		protected abstract void SetCellRectTransformAnchors(RectTransform rectTransform);

		public void OnBeginDrag(PointerEventData evt)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			if (RectTransformUtility.RectangleContainsScreenPoint(m_rectTransform, evt.get_position(), evt.get_pressEventCamera()))
			{
				foreach (KeyValuePair<int, CellRenderer> item in m_elementsByItem)
				{
					CellRenderer value = item.Value;
					if (RectTransformUtility.RectangleContainsScreenPoint(value.rectTransform, evt.get_position(), evt.get_pressEventCamera()))
					{
						if (m_items.IndexOf(value.value) != -1)
						{
							ItemDragNDropListener.instance.OnBeginDrag(evt, value);
						}
						break;
					}
				}
			}
		}

		public void OnDrag(PointerEventData eventData)
		{
			ItemDragNDropListener.instance.OnDrag(eventData);
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			ItemDragNDropListener.instance.OnEndDrag(eventData);
		}

		public void OnDragOver(CellRenderer cellRenderer, PointerEventData evt)
		{
		}

		public bool OnDropOut(CellRenderer cellRenderer, PointerEventData evt)
		{
			return true;
		}

		public bool OnDrop(CellRenderer cellRenderer, PointerEventData evt)
		{
			return true;
		}

		private void OnDestroy()
		{
			ItemDragNDropListener.instance.UnRegister(this, m_itemType);
		}

		protected List()
			: this()
		{
		}
	}
}
