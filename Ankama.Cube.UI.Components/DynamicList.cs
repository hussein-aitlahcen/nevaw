using Ankama.Utilities;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Components
{
	[RequireComponent(typeof(ScrollRect))]
	public class DynamicList : MonoBehaviour, DragNDropClient, IBeginDragHandler, IEventSystemHandler, IDragHandler, IEndDragHandler
	{
		public enum CellAnimation
		{
			None,
			InsertionFromScratch,
			InsertionFromExternal
		}

		public class CellParams
		{
			public uint id;

			public object value;

			public int index = -1;

			public int itemIndex = -1;

			public CellAnimation cellAnimation;

			private readonly TweenableVector2 m_position = new TweenableVector2();

			private readonly TweenableVector2 m_size = new TweenableVector2();

			private readonly TweenableFloat m_scale = new TweenableFloat();

			private Vector2 m_pivot;

			private RectTransform m_rectTransform;

			private CellRenderer m_renderer;

			public bool removed;

			public bool filtered;

			public Rect actualRect;

			private bool m_moveAnimated;

			public Vector2 position
			{
				set
				{
					//IL_0006: Unknown result type (might be due to invalid IL or missing references)
					//IL_000b: Unknown result type (might be due to invalid IL or missing references)
					//IL_001a: Unknown result type (might be due to invalid IL or missing references)
					//IL_0041: Unknown result type (might be due to invalid IL or missing references)
					//IL_0042: Unknown result type (might be due to invalid IL or missing references)
					if (!(m_position.value == value))
					{
						m_position.SetValue(value);
						ComputeActualRect();
						if (!m_moveAnimated && Object.op_Implicit(m_rectTransform))
						{
							m_rectTransform.set_anchoredPosition3D(Vector2.op_Implicit(value));
						}
					}
				}
			}

			public CellRenderer renderer
			{
				get
				{
					return m_renderer;
				}
				set
				{
					//IL_007a: Unknown result type (might be due to invalid IL or missing references)
					//IL_007f: Unknown result type (might be due to invalid IL or missing references)
					//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
					if (!(m_renderer == value))
					{
						m_renderer = value;
						if (m_renderer == null)
						{
							m_rectTransform = null;
							return;
						}
						m_renderer.set_name($"cell {id}");
						m_renderer.SetValue(this.value);
						m_rectTransform = m_renderer.GetComponent<RectTransform>();
						m_rectTransform.set_anchoredPosition3D(Vector2.op_Implicit(m_position.currentValue));
						float currentValue = m_scale.currentValue;
						m_rectTransform.set_localScale(new Vector3(currentValue, currentValue, 1f));
					}
				}
			}

			public unsafe CellParams(uint id, [NotNull] CellRenderer cellRenderer, Vector2 size, CellAnimation anim = CellAnimation.None)
			{
				//IL_0083: Unknown result type (might be due to invalid IL or missing references)
				//IL_0093: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
				this.id = id;
				cellAnimation = anim;
				m_renderer = cellRenderer;
				m_renderer.set_name($"cell {id}");
				m_rectTransform = m_renderer.rectTransform;
				m_position.SetValue(m_rectTransform.get_anchoredPosition());
				m_size.SetValue(size);
				m_scale.SetValue(((IntPtr)(void*)m_rectTransform.get_localScale()).x);
				value = cellRenderer.value;
				ComputeActualRect();
			}

			public CellParams(uint id, object value, Vector2 size, CellAnimation anim = CellAnimation.None)
			{
				//IL_004a: Unknown result type (might be due to invalid IL or missing references)
				this.id = id;
				cellAnimation = anim;
				m_size.SetValue(size);
				m_scale.SetValue(1f);
				this.value = value;
			}

			private unsafe void ComputeActualRect()
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_0031: Unknown result type (might be due to invalid IL or missing references)
				//IL_0037: Unknown result type (might be due to invalid IL or missing references)
				//IL_0044: Unknown result type (might be due to invalid IL or missing references)
				//IL_004a: Unknown result type (might be due to invalid IL or missing references)
				Vector2 currentValue = m_position.currentValue;
				Vector2 currentValue2 = m_size.currentValue;
				actualRect.Set(((IntPtr)(void*)currentValue).x - ((IntPtr)(void*)currentValue2).x * 0.5f, ((IntPtr)(void*)currentValue).y - ((IntPtr)(void*)currentValue2).y * 0.5f, ((IntPtr)(void*)currentValue2).x, ((IntPtr)(void*)currentValue2).y);
			}

			public unsafe void StartInsertion()
			{
				//IL_0022: Unknown result type (might be due to invalid IL or missing references)
				if (cellAnimation == CellAnimation.InsertionFromExternal)
				{
					if (Object.op_Implicit(m_rectTransform))
					{
						m_scale.SetTweenValues(((IntPtr)(void*)m_rectTransform.get_localScale()).x, 1f);
					}
					else
					{
						m_scale.SetTweenValues(1f, 1f);
					}
				}
				else
				{
					if (Object.op_Implicit(m_renderer))
					{
						m_renderer.get_gameObject().SetActive(true);
					}
					m_scale.SetTweenValues(0f, 1f);
				}
				EvaluateScale(0f);
			}

			public void StartRemoval()
			{
				m_scale.SetTweenValues(1f, 0f);
				EvaluateScale(0f);
			}

			public void EvaluateScale(float percentage)
			{
				//IL_003a: Unknown result type (might be due to invalid IL or missing references)
				m_scale.Evaluate(percentage);
				if (Object.op_Implicit(m_rectTransform))
				{
					m_rectTransform.set_localScale(new Vector3(m_scale.currentValue, m_scale.currentValue, 1f));
				}
			}

			public void StartMove(Vector2 destination)
			{
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_002c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0031: Unknown result type (might be due to invalid IL or missing references)
				//IL_003a: Unknown result type (might be due to invalid IL or missing references)
				//IL_003f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0046: Unknown result type (might be due to invalid IL or missing references)
				//IL_0047: Unknown result type (might be due to invalid IL or missing references)
				m_moveAnimated = true;
				Vector2 startValue = (!m_position.init) ? destination : ((!(m_rectTransform == null)) ? m_rectTransform.get_anchoredPosition() : m_position.value);
				m_position.SetTweenValues(startValue, destination);
				EvaluateMove(0f);
			}

			public void EvaluateMove(float percentage)
			{
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0030: Unknown result type (might be due to invalid IL or missing references)
				m_position.Evaluate(percentage);
				ComputeActualRect();
				if (Object.op_Implicit(m_rectTransform))
				{
					m_rectTransform.set_anchoredPosition3D(Vector2.op_Implicit(m_position.currentValue));
				}
			}

			public void EndMove()
			{
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				m_position.Evaluate(1f);
				ComputeActualRect();
				m_moveAnimated = false;
				if (Object.op_Implicit(m_rectTransform))
				{
					m_rectTransform.set_anchoredPosition(m_position.currentValue);
				}
			}
		}

		public enum OnDragAction
		{
			None,
			Remove,
			SetToNull
		}

		public enum OnDropAction
		{
			None,
			InsertAt,
			Replace
		}

		private struct Item
		{
			internal object m_value;

			internal bool m_filtered;

			public Item(object value, bool filtered)
			{
				m_value = value;
				m_filtered = filtered;
			}
		}

		[Header("Elements")]
		[SerializeField]
		private RectTransform m_viewport;

		[SerializeField]
		protected RectTransform m_content;

		[Header("Cell renderer")]
		[SerializeField]
		protected GameObject m_prefab;

		[Header("Configuration")]
		[SerializeField]
		protected bool m_horizontal;

		[SerializeField]
		private bool m_displayEmptyCell;

		[SerializeField]
		protected Vector2Int m_cellSize;

		[SerializeField]
		protected Vector2Int m_margin;

		[Header("Drag and Drop")]
		[SerializeField]
		private bool m_enableDragAndDrop = true;

		[SerializeField]
		private OnDragAction m_onDragAction;

		[SerializeField]
		private OnDropAction m_onDropAction;

		[Header("Animation")]
		[SerializeField]
		protected Ease m_moveEase = 13;

		[SerializeField]
		protected float m_moveAnimationDuration = 0.2f;

		[SerializeField]
		protected Ease m_insertionEase = 30;

		[SerializeField]
		protected float m_insertionAnimationDuration = 0.15f;

		[SerializeField]
		protected Ease m_scrollEase = 10;

		[SerializeField]
		protected float m_scrollDuration = 0.15f;

		private ICellRendererConfigurator m_cellRendererConfigurator;

		private CellRendererFilter m_cellRendererFilter;

		private IDragNDropValidator m_dragNDropValidator;

		private Type m_itemType;

		private CellRenderer m_cellRenderer;

		private ScrollRect m_scrollRect;

		private int m_viewportWidth;

		private int m_viewportHeight;

		private List<Item> m_allItems;

		private List<object> m_items;

		private List<CellRenderer> m_rendererPool;

		private List<CellParams> m_cellParams;

		private readonly Dictionary<uint, CellParams> m_cellParamsById = new Dictionary<uint, CellParams>();

		private uint m_lastId;

		private int m_previousFirstCellIndex = -1;

		private int m_previousLastCellIndex = -1;

		private int m_rows;

		private int m_columns;

		private float m_horizontalLeeway;

		private float m_verticalLeeway;

		private float m_contentWidth;

		private float m_contentHeight;

		private int m_cellSizeX;

		private int m_cellSizeY;

		private Vector2 m_scrollPercentageVector;

		private float m_scrollPercentage;

		private Rect m_viewportBoundingBox;

		private float m_animationStartTime = -1f;

		private int m_animationStep;

		private float m_removeAnimationDuration;

		private readonly List<int> m_insertedCells = new List<int>();

		private readonly List<int> m_removedCells = new List<int>();

		private bool m_initialized;

		private Vector2 m_lastLayoutDimensions;

		private bool m_viewportChanged;

		private bool m_itemCountChanged;

		private bool m_resetScrollPosition;

		private float m_previousContentWidth;

		private float m_previousContentHeight;

		private float m_targetContentWidth;

		private float m_targetContentHeight;

		private bool m_needReLayout;

		public float scrollPercentage
		{
			get
			{
				return m_scrollPercentage;
			}
			private set
			{
				m_scrollPercentage = value;
				this.OnScrollPercentage?.Invoke(m_scrollPercentage);
			}
		}

		public bool enableDragAndDrop
		{
			get
			{
				return m_enableDragAndDrop;
			}
			set
			{
				m_enableDragAndDrop = value;
			}
		}

		private int m_scrollInPixels
		{
			get
			{
				if (m_horizontal)
				{
					return Mathf.RoundToInt(Mathf.Clamp01(m_scrollRect.get_horizontalNormalizedPosition()) * m_horizontalLeeway);
				}
				float num = Mathf.Clamp01(m_scrollRect.get_verticalNormalizedPosition());
				return Mathf.RoundToInt((1f - num) * m_verticalLeeway);
			}
			set
			{
				if (m_horizontal)
				{
					if (Mathf.Approximately(m_horizontalLeeway, 0f))
					{
						m_scrollRect.set_horizontalNormalizedPosition(0f);
					}
					else
					{
						m_scrollRect.set_horizontalNormalizedPosition((float)value / m_horizontalLeeway);
					}
				}
				else if (Mathf.Approximately(m_verticalLeeway, 0f))
				{
					m_scrollRect.set_verticalNormalizedPosition(1f);
				}
				else
				{
					m_scrollRect.set_verticalNormalizedPosition(1f - (float)value / m_verticalLeeway);
				}
			}
		}

		private float m_contentLength
		{
			get
			{
				if (!m_horizontal)
				{
					return m_contentHeight;
				}
				return m_contentWidth;
			}
		}

		private float m_viewportLength => m_horizontal ? m_viewportWidth : m_viewportHeight;

		public int currentPageIndex
		{
			get
			{
				CheckInit();
				float num = (m_viewportLength <= 0f) ? (-1f) : ((float)m_scrollInPixels / m_viewportLength);
				if (!((double)num <= 1E-06))
				{
					return Mathf.CeilToInt(num);
				}
				return 0;
			}
		}

		public int pagesCount => Mathf.Max(0, Mathf.CeilToInt(m_contentLength / m_viewportLength));

		public RectTransform rectTransform => m_viewport;

		public bool activeInHierarchy => this.get_gameObject().get_activeInHierarchy();

		public event Action OnSetValues;

		public event Action<object, int> OnInsertion;

		public event Action<object, int> OnRemoved;

		public event Action<object, object, int> OnValueChanged;

		public event Action<float> OnScrollPercentage;

		protected void Awake()
		{
			CheckInit();
			ItemDragNDropListener.instance.Register(this, m_itemType);
		}

		private unsafe void CheckInit()
		{
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			if (m_initialized)
			{
				return;
			}
			m_cellParams = ListPool<CellParams>.Get();
			m_rendererPool = ListPool<CellRenderer>.Get();
			m_items = ListPool<object>.Get();
			m_allItems = ListPool<Item>.Get();
			if (m_cellSize.get_x() <= 0 || m_cellSize.get_y() <= 0)
			{
				m_cellSize = new Vector2Int(10, 10);
			}
			m_cellSizeX = m_cellSize.get_x() + m_margin.get_x();
			m_cellSizeY = m_cellSize.get_y() + m_margin.get_y();
			m_scrollRect = this.GetComponent<ScrollRect>();
			m_scrollRect.get_onValueChanged().AddListener(new UnityAction<Vector2>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_scrollRect.set_horizontal(false);
			m_scrollRect.set_vertical(false);
			OnScrollRectValueChanged(new Vector2(m_scrollRect.get_horizontalNormalizedPosition(), m_scrollRect.get_verticalNormalizedPosition()));
			m_viewportChanged = true;
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
			CellRenderer[] componentsInChildren = this.get_gameObject().GetComponentsInChildren<CellRenderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Object.Destroy(componentsInChildren[i].get_gameObject());
			}
			m_initialized = true;
		}

		public void SetCellRendererConfigurator(ICellRendererConfigurator configurator)
		{
			CheckInit();
			m_cellRendererConfigurator = configurator;
			foreach (CellRenderer item in m_rendererPool)
			{
				item.SetConfigurator(configurator);
			}
			foreach (CellParams cellParam in m_cellParams)
			{
				CellRenderer renderer = cellParam.renderer;
				if (!(renderer == null))
				{
					renderer.SetConfigurator(configurator);
				}
			}
		}

		public void UpdateAllConfigurators(bool instant = false)
		{
			CheckInit();
			foreach (CellParams cellParam in m_cellParams)
			{
				CellRenderer renderer = cellParam.renderer;
				if (!(renderer == null))
				{
					renderer.OnConfiguratorUpdate(instant);
				}
			}
		}

		public void UpdateConfiguratorWithValue(object value, bool instant = false)
		{
			CheckInit();
			foreach (CellParams cellParam in m_cellParams)
			{
				CellRenderer renderer = cellParam.renderer;
				if (!(renderer == null) && renderer.value == value)
				{
					renderer.OnConfiguratorUpdate(instant);
				}
			}
		}

		public void SetValues<T>(IEnumerable<T> values) where T : class
		{
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
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
			FinishAnimation();
			m_lastId = 0u;
			m_items.Clear();
			m_allItems.Clear();
			m_cellParams.Clear();
			foreach (KeyValuePair<uint, CellParams> item in m_cellParamsById)
			{
				ReturnToPool(item.Value.renderer);
			}
			m_cellParamsById.Clear();
			foreach (T value in values)
			{
				bool flag = m_cellRendererFilter != null && !m_cellRendererFilter(value);
				m_allItems.Add(new Item(value, flag));
				if (!flag)
				{
					m_items.Add(value);
					m_cellParams.Add(new CellParams(m_lastId++, value, Vector2Int.op_Implicit(m_cellSize)));
				}
			}
			m_itemCountChanged = true;
			m_resetScrollPosition = true;
			UpdateAll();
			this.OnSetValues?.Invoke();
		}

		public void Insert<T>(int index, T value)
		{
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
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
			if (index < 0 || index > m_allItems.Count)
			{
				throw new ArgumentOutOfRangeException();
			}
			FinishAnimation();
			bool flag = m_cellRendererFilter != null && !m_cellRendererFilter(value);
			m_allItems.Insert(index, new Item(value, flag));
			if (!flag)
			{
				int num = 0;
				for (int i = 0; i < index; i++)
				{
					if (!m_allItems[i].m_filtered)
					{
						num++;
					}
				}
				m_items.Insert(num, value);
				CellParams item = new CellParams(m_lastId++, value, Vector2Int.op_Implicit(m_cellSize), CellAnimation.InsertionFromScratch);
				m_cellParams.Insert(num, item);
				m_insertedCells.Add(num);
				StartAnimation();
			}
			this.OnInsertion?.Invoke(value, index);
		}

		public void RemoveRange(int index, int count)
		{
			CheckInit();
			if (index < 0 || index + count > m_allItems.Count)
			{
				throw new ArgumentOutOfRangeException();
			}
			FinishAnimation();
			int num = 0;
			for (int i = 0; i < index; i++)
			{
				if (!m_allItems[i].m_filtered)
				{
					num++;
				}
			}
			for (int j = 0; j < count; j++)
			{
				int num2 = index + j;
				Item item = m_allItems[num2];
				if (!item.m_filtered)
				{
					m_cellParams[num].removed = true;
					m_removedCells.Add(num);
					num++;
				}
				this.OnRemoved?.Invoke(item.m_value, num2);
			}
			StartAnimation();
		}

		public void RemoveAt(int index)
		{
			CheckInit();
			if (index < 0 || index > m_allItems.Count)
			{
				throw new ArgumentOutOfRangeException();
			}
			FinishAnimation();
			Item item = m_allItems[index];
			if (item.m_filtered)
			{
				m_allItems.RemoveAt(index);
			}
			else
			{
				int num = 0;
				for (int i = 0; i < index; i++)
				{
					Item item2 = m_allItems[i];
					if (!item2.m_filtered && item2.m_value != item.m_value)
					{
						num++;
					}
				}
				m_cellParams[num].removed = true;
				m_removedCells.Add(num);
				StartAnimation();
			}
			this.OnRemoved?.Invoke(item.m_value, index);
		}

		public void SetFilter(CellRendererFilter filter)
		{
			m_cellRendererFilter = filter;
			UpdateFilter();
		}

		public void UpdateFilter()
		{
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			FinishAnimation();
			bool flag = false;
			int num = 0;
			int i = 0;
			for (int count = m_allItems.Count; i < count; i++)
			{
				Item item = m_allItems[i];
				bool filtered = item.m_filtered;
				bool flag2 = m_cellRendererFilter != null && !m_cellRendererFilter(item.m_value);
				if (filtered != flag2)
				{
					item.m_filtered = flag2;
					m_allItems[i] = item;
					if (flag2)
					{
						m_removedCells.Add(num);
						m_cellParams[num].filtered = true;
					}
					else
					{
						m_items.Insert(num, item.m_value);
						CellParams item2 = new CellParams(m_lastId++, item.m_value, Vector2Int.op_Implicit(m_cellSize), CellAnimation.InsertionFromScratch);
						m_cellParams.Insert(num, item2);
						m_insertedCells.Add(num - m_removedCells.Count);
					}
					flag = true;
				}
				if (!filtered || !flag2)
				{
					num++;
				}
			}
			if (flag)
			{
				StartAnimation();
			}
		}

		private void StartAnimation()
		{
			m_animationStep = 0;
			m_animationStartTime = Time.get_time();
			m_removeAnimationDuration = ((m_removedCells.Count == 0) ? 0f : m_insertionAnimationDuration);
			m_itemCountChanged = true;
			ComputeDimensions();
			ComputeCellPositions();
		}

		protected void Update()
		{
			UpdateAll();
		}

		private void UpdateAll()
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			UpdateAnimation();
			bool num = IsAnimating();
			if (m_viewportChanged)
			{
				Rect rect = m_viewport.get_rect();
				if (rect.get_size() == m_lastLayoutDimensions)
				{
					m_viewportChanged = false;
				}
			}
			if (!num && (m_itemCountChanged || m_viewportChanged))
			{
				ComputeDimensions();
				ComputeCellPositions();
			}
			if (m_resetScrollPosition)
			{
				m_scrollRect.set_horizontalNormalizedPosition(0f);
				m_scrollRect.set_verticalNormalizedPosition(1f);
				OnScrollRectValueChanged(new Vector2(0f, 1f));
				m_resetScrollPosition = false;
			}
			if (num)
			{
				AccurateReLayout();
			}
			else if (m_needReLayout)
			{
				FullReLayout();
			}
		}

		private bool IsAnimating()
		{
			return m_animationStartTime >= 0f;
		}

		private void UpdateAnimation(bool finish = false)
		{
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02de: Unknown result type (might be due to invalid IL or missing references)
			if (m_animationStartTime < 0f)
			{
				return;
			}
			float num = Time.get_time() - m_animationStartTime;
			bool flag = false;
			if (m_animationStep == 0)
			{
				int i = 0;
				for (int count = m_removedCells.Count; i < count; i++)
				{
					m_cellParams[m_removedCells[i]].StartRemoval();
				}
				m_animationStep++;
			}
			if (m_animationStep == 1)
			{
				if (m_removedCells.Count == 0)
				{
					m_animationStep++;
					flag = true;
				}
				else
				{
					float num2 = num;
					bool flag2 = false;
					if ((num2 >= m_insertionAnimationDuration) | finish)
					{
						flag2 = true;
						m_animationStep++;
						flag = true;
					}
					if (flag2)
					{
						FinishRemoveAnimation();
					}
					else
					{
						float percentage = EaseManager.Evaluate(m_insertionEase, null, num2, m_insertionAnimationDuration, 0f, 0f);
						int j = 0;
						for (int count2 = m_removedCells.Count; j < count2; j++)
						{
							m_cellParams[m_removedCells[j]].EvaluateScale(percentage);
						}
					}
				}
			}
			if (m_animationStep == 2)
			{
				if (flag)
				{
					ComputeDimensions();
				}
				float num3 = num - m_removeAnimationDuration;
				bool flag3 = false;
				if ((num3 >= m_moveAnimationDuration) | finish)
				{
					m_animationStep++;
					flag3 = true;
				}
				float num4 = EaseManager.Evaluate(m_moveEase, null, num3, m_moveAnimationDuration, 0f, 0f);
				int k = 0;
				for (int count3 = m_cellParams.Count; k < count3; k++)
				{
					if (flag3)
					{
						m_cellParams[k].EndMove();
					}
					else
					{
						m_cellParams[k].EvaluateMove(num4);
					}
				}
				if (flag3)
				{
					int l = 0;
					for (int count4 = m_insertedCells.Count; l < count4; l++)
					{
						m_cellParams[m_insertedCells[l]].StartInsertion();
					}
					m_contentWidth = m_targetContentWidth;
					m_contentHeight = m_targetContentHeight;
				}
				else
				{
					m_contentWidth = m_previousContentWidth + (m_targetContentWidth - m_previousContentWidth) * num4;
					m_contentHeight = m_previousContentHeight + (m_targetContentHeight - m_previousContentHeight) * num4;
				}
				UpdateContentSize();
			}
			if (m_animationStep == 3)
			{
				if (m_insertedCells.Count == 0)
				{
					m_animationStep++;
				}
				else
				{
					float num5 = num - m_moveAnimationDuration - m_removeAnimationDuration;
					bool flag4 = false;
					if ((num5 >= m_insertionAnimationDuration) | finish)
					{
						flag4 = true;
						m_animationStep++;
					}
					if (flag4)
					{
						FinishInsertAnimation();
					}
					else
					{
						float percentage2 = EaseManager.Evaluate(m_insertionEase, null, num5, m_insertionAnimationDuration, 0f, 0f);
						int m = 0;
						for (int count5 = m_insertedCells.Count; m < count5; m++)
						{
							m_cellParams[m_insertedCells[m]].EvaluateScale(percentage2);
						}
					}
				}
			}
			if (m_animationStep == 4)
			{
				m_animationStartTime = -1f;
				m_animationStep = 0;
			}
		}

		private void FinishAnimation()
		{
			UpdateAnimation(finish: true);
			AccurateReLayout();
		}

		private void FinishInsertAnimation()
		{
			int i = 0;
			for (int count = m_insertedCells.Count; i < count; i++)
			{
				CellParams cellParams = m_cellParams[m_insertedCells[i]];
				cellParams.cellAnimation = CellAnimation.None;
				cellParams.EvaluateScale(1f);
			}
			m_insertedCells.Clear();
		}

		private void FinishRemoveAnimation()
		{
			for (int num = m_removedCells.Count - 1; num >= 0; num--)
			{
				int index = m_removedCells[num];
				CellParams cellParams = m_cellParams[index];
				int itemIndex = cellParams.itemIndex;
				m_cellParamsById.Remove(cellParams.id);
				ReturnToPool(cellParams.renderer);
				m_cellParams.RemoveAt(index);
				m_items.RemoveAt(index);
				if (cellParams.removed)
				{
					m_allItems.RemoveAt(itemIndex);
				}
			}
			m_removedCells.Clear();
		}

		private void OnRectTransformDimensionsChange()
		{
			m_viewportChanged = true;
		}

		private void CheckEmptyCellParams()
		{
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			if (m_displayEmptyCell)
			{
				int num = (!m_horizontal) ? (Mathf.CeilToInt((float)m_items.Count / (float)m_columns) * m_columns) : (Mathf.CeilToInt((float)m_items.Count / (float)m_rows) * m_rows);
				int num2 = num + m_removedCells.Count;
				for (int i = m_cellParams.Count; i < num2; i++)
				{
					m_cellParams.Add(new CellParams(m_lastId++, (object)null, Vector2Int.op_Implicit(m_cellSize), CellAnimation.None));
				}
				for (int num3 = m_cellParams.Count - 1; num3 >= num2; num3--)
				{
					CellParams cellParams = m_cellParams[num3];
					ReturnToPool(cellParams.renderer);
					cellParams.renderer = null;
					m_cellParams.RemoveAt(num3);
					m_cellParamsById.Remove(cellParams.id);
				}
			}
		}

		private void ReturnToPool(CellRenderer cellRenderer)
		{
			if (!(cellRenderer == null))
			{
				cellRenderer.SetValue(null);
				m_rendererPool.Add(cellRenderer);
				cellRenderer.get_gameObject().SetActive(false);
			}
		}

		private CellRenderer GetFromPool(CellParams cellParams)
		{
			bool active = cellParams.cellAnimation != CellAnimation.InsertionFromScratch;
			if (m_rendererPool.Count == 0)
			{
				CellRenderer cellRenderer = Object.Instantiate<CellRenderer>(m_cellRenderer);
				InitCellRenderer(cellRenderer, andUpdate: false);
				cellRenderer.get_gameObject().SetActive(active);
				return cellRenderer;
			}
			CellRenderer cellRenderer2 = m_rendererPool[m_rendererPool.Count - 1];
			m_rendererPool.RemoveAt(m_rendererPool.Count - 1);
			cellRenderer2.get_gameObject().SetActive(active);
			return cellRenderer2;
		}

		protected virtual void InitCellRenderer(CellRenderer cellRenderer, bool andUpdate)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			RectTransform rectTransform = cellRenderer.rectTransform;
			rectTransform.SetParent(m_content, true);
			Vector3 localPosition = rectTransform.get_localPosition();
			rectTransform.set_sizeDelta(Vector2Int.op_Implicit(m_cellSize));
			rectTransform.set_anchorMin(Vector2.get_zero());
			rectTransform.set_anchorMax(Vector2.get_zero());
			rectTransform.set_pivot(new Vector2(0.5f, 0.5f));
			rectTransform.set_localPosition(localPosition);
			cellRenderer.dragNDropClient = this;
			cellRenderer.SetConfigurator(m_cellRendererConfigurator, andUpdate);
		}

		public unsafe IEnumerator ScrollToPage(int index, bool instant, TweenCallback onUpdate = null)
		{
			CheckInit();
			int pagesCount = this.pagesCount;
			if (index < 0 || index >= pagesCount)
			{
				yield break;
			}
			int num = Mathf.RoundToInt(m_viewportLength * (float)index);
			if (instant)
			{
				m_scrollInPixels = num;
				if (onUpdate != null)
				{
					onUpdate.Invoke();
				}
				yield break;
			}
			Tweener val = TweenSettingsExtensions.SetEase<Tweener>(DOTween.To(new DOGetter<int>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<int>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), Mathf.Clamp(num, 0, Mathf.RoundToInt(m_contentLength - m_viewportLength)), m_scrollDuration), m_scrollEase);
			if (onUpdate != null)
			{
				val = TweenSettingsExtensions.OnKill<Tweener>(TweenSettingsExtensions.OnUpdate<Tweener>(val, onUpdate), onUpdate);
			}
			yield return TweenExtensions.WaitForKill(val);
		}

		public bool HasPreviousPage()
		{
			CheckInit();
			return currentPageIndex > 0;
		}

		public bool HastNextPage()
		{
			CheckInit();
			return currentPageIndex < pagesCount - 1;
		}

		private unsafe void OnScrollRectValueChanged(Vector2 position)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			m_scrollPercentageVector.Set(((IntPtr)(void*)position).x, 1f - ((IntPtr)(void*)position).y);
			scrollPercentage = (m_horizontal ? m_scrollPercentageVector.x : m_scrollPercentageVector.y);
			if (m_initialized)
			{
				ComputeViewportBoundingBox();
				if (!IsAnimating() && !m_needReLayout)
				{
					FastReLayout();
				}
			}
		}

		private void ComputeDimensions()
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			CheckInit();
			if (m_viewportChanged)
			{
				Rect rect = m_viewport.get_rect();
				m_viewportWidth = Math.Max(1, (int)rect.get_width());
				rect = m_viewport.get_rect();
				m_viewportHeight = Math.Max(1, (int)rect.get_height());
				if (m_horizontal)
				{
					m_rows = Math.Max(1, m_viewportHeight / m_cellSizeY);
					m_columns = Math.Max(1, (int)Math.Ceiling((float)m_viewportWidth / (float)m_cellSizeX)) + 1;
					m_contentHeight = Math.Max(0, m_rows * m_cellSizeY);
				}
				else
				{
					m_columns = Math.Max(1, m_viewportWidth / m_cellSizeX);
					m_rows = Math.Max(1, (int)Math.Ceiling((float)m_viewportHeight / (float)m_cellSizeY)) + 1;
					m_contentWidth = Math.Max(0, m_columns * m_cellSizeX);
				}
			}
			bool flag = IsAnimating();
			if (m_viewportChanged || m_itemCountChanged)
			{
				float num = m_items.Count - m_removedCells.Count;
				if (flag)
				{
					Rect rect2 = m_content.get_rect();
					m_previousContentWidth = rect2.get_width();
					m_previousContentHeight = rect2.get_height();
					m_targetContentWidth = m_contentWidth;
					m_targetContentHeight = m_contentHeight;
				}
				if (m_horizontal)
				{
					float num2 = Mathf.Ceil(num / (float)m_rows);
					float num3 = Math.Max(0f, num2 * (float)m_cellSizeX);
					if (flag)
					{
						m_targetContentWidth = num3;
					}
					else
					{
						m_contentWidth = num3;
					}
				}
				else
				{
					float num4 = Mathf.Ceil(num / (float)m_columns);
					float num5 = Math.Max(0f, num4 * (float)m_cellSizeY);
					if (flag)
					{
						m_targetContentHeight = num5;
					}
					else
					{
						m_contentHeight = num5;
					}
				}
				UpdateContentSize();
			}
			if (!flag)
			{
				m_needReLayout = true;
			}
			m_viewportChanged = false;
			m_itemCountChanged = false;
		}

		private void UpdateContentSize()
		{
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			int scrollInPixels = m_scrollInPixels;
			m_horizontalLeeway = Mathf.Max(0f, m_contentWidth - (float)m_viewportWidth);
			m_verticalLeeway = Mathf.Max(0f, m_contentHeight - (float)m_viewportHeight);
			m_content.set_sizeDelta(new Vector2(m_contentWidth, m_contentHeight));
			m_scrollInPixels = scrollInPixels;
			ComputeViewportBoundingBox();
			CheckEmptyCellParams();
		}

		private void ComputeViewportBoundingBox()
		{
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			if (m_horizontal)
			{
				float num = m_horizontalLeeway * m_scrollPercentageVector.x;
				float num2 = m_verticalLeeway / 2f;
				m_viewportBoundingBox = new Rect(num, num2, (float)(m_viewportWidth + m_cellSizeX), (float)m_viewportHeight);
			}
			else
			{
				float num3 = m_horizontalLeeway / 2f;
				float num4 = m_verticalLeeway * (1f - m_scrollPercentageVector.y);
				m_viewportBoundingBox = new Rect(num3, num4, (float)m_viewportWidth, (float)(m_viewportHeight + m_cellSizeY));
			}
		}

		private void ComputeCellPositions()
		{
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			bool flag = IsAnimating();
			int num = 0;
			int i = 0;
			for (int count = m_allItems.Count; i < count; i++)
			{
				if (!m_allItems[i].m_filtered)
				{
					m_cellParams[num++].itemIndex = i;
				}
			}
			float num3 = flag ? m_targetContentHeight : m_contentHeight;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int j = 0;
			for (int count2 = m_cellParams.Count; j < count2; j++)
			{
				CellParams cellParams = m_cellParams[j];
				if (cellParams.removed || cellParams.filtered)
				{
					continue;
				}
				cellParams.index = num6++;
				float num8 = (float)m_cellSizeX * ((float)num4 + 0.5f);
				float num9 = (float)(int)num3 - (float)m_cellSizeY * ((float)num5 + 0.5f);
				if (flag)
				{
					cellParams.StartMove(new Vector2(num8, num9));
				}
				else
				{
					cellParams.position = new Vector2(num8, num9);
				}
				if (m_horizontal)
				{
					num5++;
					if (num5 >= m_rows)
					{
						num5 = 0;
						num4++;
					}
				}
				else
				{
					num4++;
					if (num4 >= m_columns)
					{
						num4 = 0;
						num5++;
					}
				}
			}
		}

		public void AccurateReLayout()
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			int i = 0;
			for (int count = m_cellParams.Count; i < count; i++)
			{
				CellParams cellParams = m_cellParams[i];
				CellParams value2;
				if (m_viewportBoundingBox.Overlaps(cellParams.actualRect))
				{
					if (!m_cellParamsById.TryGetValue(cellParams.id, out CellParams _))
					{
						cellParams.renderer = GetFromPool(cellParams);
						m_cellParamsById[cellParams.id] = cellParams;
					}
				}
				else if (m_cellParamsById.TryGetValue(cellParams.id, out value2))
				{
					ReturnToPool(value2.renderer);
					value2.renderer = null;
					m_cellParamsById.Remove(value2.id);
				}
			}
		}

		private void FullReLayout()
		{
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			int num = ToIndex(m_scrollPercentage);
			int num2 = Mathf.Clamp(num, 0, m_cellParams.Count);
			int num3 = num + m_rows * m_columns - 1;
			foreach (KeyValuePair<uint, CellParams> item in m_cellParamsById)
			{
				ReturnToPool(item.Value.renderer);
				item.Value.renderer = null;
			}
			m_cellParamsById.Clear();
			for (int i = num2; i <= num3 && i < m_cellParams.Count; i++)
			{
				CellParams cellParams = m_cellParams[i];
				cellParams.renderer = GetFromPool(cellParams);
				m_cellParamsById[cellParams.id] = cellParams;
			}
			m_previousFirstCellIndex = num2;
			m_previousLastCellIndex = num3;
			Rect rect = m_viewport.get_rect();
			m_lastLayoutDimensions = rect.get_size();
			m_needReLayout = false;
		}

		private void FastReLayout()
		{
			int num = ToIndex(m_scrollPercentage);
			int num2 = Mathf.Clamp(num, 0, m_cellParams.Count);
			int num3 = num + m_rows * m_columns - 1;
			for (int i = m_previousFirstCellIndex; i < Math.Min(num2, m_previousLastCellIndex + 1); i++)
			{
				if (i >= 0 && m_cellParams.Count > i)
				{
					CellParams cellParams = m_cellParams[i];
					ReturnToPool(cellParams.renderer);
					cellParams.renderer = null;
					m_cellParamsById.Remove(cellParams.id);
				}
			}
			for (int num4 = m_previousLastCellIndex; num4 > Math.Max(num3, m_previousFirstCellIndex - 1); num4--)
			{
				if (num4 >= 0 && m_cellParams.Count > num4)
				{
					CellParams cellParams2 = m_cellParams[num4];
					ReturnToPool(cellParams2.renderer);
					cellParams2.renderer = null;
					m_cellParamsById.Remove(cellParams2.id);
				}
			}
			for (int j = num2; j < Math.Min(m_previousFirstCellIndex, num3 + 1); j++)
			{
				if (j < m_cellParams.Count)
				{
					CellParams cellParams3 = m_cellParams[j];
					if (!m_cellParamsById.ContainsKey(cellParams3.id))
					{
						cellParams3.renderer = GetFromPool(cellParams3);
						m_cellParamsById[cellParams3.id] = cellParams3;
					}
				}
			}
			for (int k = Math.Max(num2, m_previousLastCellIndex + 1); k <= num3; k++)
			{
				if (k < m_cellParams.Count)
				{
					CellParams cellParams4 = m_cellParams[k];
					if (!m_cellParamsById.ContainsKey(cellParams4.id))
					{
						cellParams4.renderer = GetFromPool(cellParams4);
						m_cellParamsById[cellParams4.id] = cellParams4;
					}
				}
			}
			m_previousFirstCellIndex = num2;
			m_previousLastCellIndex = num3;
		}

		private int ToIndex(float percentage)
		{
			float num = Mathf.Clamp01(percentage);
			if (m_horizontal)
			{
				return (int)Math.Floor(m_horizontalLeeway * num / (float)m_cellSizeX) * m_rows;
			}
			return (int)Math.Floor(m_verticalLeeway * num / (float)m_cellSizeY) * m_columns;
		}

		public void SetDragAndDropValidator(IDragNDropValidator validator)
		{
			m_dragNDropValidator = validator;
		}

		public void OnDragOver(CellRenderer cellRenderer, PointerEventData evt)
		{
		}

		public bool OnDropOut(CellRenderer cellRenderer, PointerEventData evt)
		{
			bool enableDragAndDrop = m_enableDragAndDrop;
			return true;
		}

		public bool OnDrop(CellRenderer cellRenderer, PointerEventData evt)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			if (!m_enableDragAndDrop)
			{
				return true;
			}
			if (m_onDropAction == OnDropAction.None)
			{
				return true;
			}
			if (!RectTransformUtility.RectangleContainsScreenPoint(m_viewport, evt.get_position(), evt.get_pressEventCamera()))
			{
				return true;
			}
			int num = -1;
			int num2 = -1;
			foreach (KeyValuePair<uint, CellParams> item in m_cellParamsById)
			{
				if (RectTransformUtility.RectangleContainsScreenPoint(item.Value.renderer.rectTransform, evt.get_position(), evt.get_pressEventCamera()))
				{
					num = item.Value.index;
					num2 = item.Value.itemIndex;
					break;
				}
			}
			if (num == -1)
			{
				return true;
			}
			if (m_dragNDropValidator != null && !m_dragNDropValidator.IsValidDrop(cellRenderer.value))
			{
				return true;
			}
			FinishAnimation();
			InitCellRenderer(cellRenderer, andUpdate: true);
			cellRenderer.rectTransform.SetAsLastSibling();
			CellParams cellParams = new CellParams(m_lastId++, cellRenderer, Vector2Int.op_Implicit(m_cellSize), CellAnimation.InsertionFromExternal);
			bool flag = m_cellRendererFilter != null && !m_cellRendererFilter(cellRenderer.value);
			switch (m_onDropAction)
			{
			case OnDropAction.None:
				return true;
			case OnDropAction.Replace:
				m_cellParams[num].removed = true;
				m_removedCells.Add(num);
				m_allItems.Insert(num2 + 1, new Item(cellRenderer.value, flag));
				if (!flag)
				{
					m_items.Insert(num + 1, cellRenderer.value);
					m_cellParams.Insert(num + 1, cellParams);
					m_cellParamsById.Add(cellParams.id, cellParams);
					m_insertedCells.Add(num);
				}
				this.OnValueChanged?.Invoke(m_items[num], cellRenderer.value, num);
				StartAnimation();
				return false;
			case OnDropAction.InsertAt:
				m_allItems.Insert(num2, new Item(cellRenderer.value, flag));
				if (!flag)
				{
					m_items.Insert(num, cellRenderer.value);
					m_cellParams.Insert(num, cellParams);
					m_cellParamsById.Add(cellParams.id, cellParams);
					m_insertedCells.Add(num);
					StartAnimation();
				}
				this.OnInsertion?.Invoke(cellRenderer.value, num);
				return false;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		public void OnBeginDrag(PointerEventData evt)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			if (!m_enableDragAndDrop || !RectTransformUtility.RectangleContainsScreenPoint(m_viewport, evt.get_position(), evt.get_pressEventCamera()))
			{
				return;
			}
			int num = -1;
			foreach (KeyValuePair<uint, CellParams> item in m_cellParamsById)
			{
				if (RectTransformUtility.RectangleContainsScreenPoint(item.Value.renderer.rectTransform, evt.get_position(), evt.get_pressEventCamera()))
				{
					num = item.Value.index;
					break;
				}
			}
			if (num == -1)
			{
				return;
			}
			CellParams cellParams = m_cellParams[num];
			if (!(cellParams.renderer == null) && (m_dragNDropValidator == null || m_dragNDropValidator.IsValidDrag(cellParams.value)))
			{
				ItemDragNDropListener.instance.OnBeginDrag(evt, cellParams.renderer);
				switch (m_onDragAction)
				{
				case OnDragAction.None:
					break;
				case OnDragAction.Remove:
					RemoveAt(num);
					break;
				case OnDragAction.SetToNull:
				{
					object arg = m_items[num];
					bool filtered = m_cellRendererFilter != null && !m_cellRendererFilter(null);
					m_allItems[num] = new Item(null, filtered);
					m_items[num] = null;
					m_cellParams[num].value = null;
					m_cellParams[num].renderer.SetValue(null);
					this.OnValueChanged?.Invoke(arg, null, num);
					break;
				}
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		public List<CellParams> GetAllCells()
		{
			return m_cellParams;
		}

		public void OnDrag(PointerEventData eventData)
		{
			if (m_enableDragAndDrop)
			{
				ItemDragNDropListener.instance.OnDrag(eventData);
			}
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			if (m_enableDragAndDrop)
			{
				ItemDragNDropListener.instance.OnEndDrag(eventData);
			}
		}

		private void OnDestroy()
		{
			ItemDragNDropListener.instance.UnRegister(this, m_itemType);
			ListPool<object>.Release(m_items);
			ListPool<Item>.Release(m_allItems);
			ListPool<CellParams>.Release(m_cellParams);
			ListPool<CellRenderer>.Release(m_rendererPool);
		}

		public DynamicList()
			: this()
		{
		}//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)

	}
}
