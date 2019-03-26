using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ankama.Cube.UI.Components
{
	[RequireComponent(typeof(RectTransform))]
	public class Item : MonoBehaviour, DragNDropClient, IBeginDragHandler, IEventSystemHandler, IDragHandler, IEndDragHandler
	{
		[Header("Cell")]
		[SerializeField]
		protected GameObject m_prefab;

		[SerializeField]
		protected RectTransform m_content;

		[Header("Drag and drop")]
		[SerializeField]
		private bool m_enableDragAndDrop = true;

		[SerializeField]
		private bool m_removeOnDrag = true;

		[SerializeField]
		private bool m_insertOnDrop = true;

		[SerializeField]
		private Ease m_insertTweenEase = 7;

		[SerializeField]
		private float m_insertTweenDuration = 0.2f;

		private Type m_itemType;

		private CellRenderer m_cellRenderer;

		private object m_value;

		private CellRenderer m_cellRendererInstance;

		[NonSerialized]
		private bool m_initialized;

		private RectTransform m_rectTransform;

		private ICellRendererConfigurator m_cellRendererConfigurator;

		private IDragNDropValidator m_dragNDropValidator;

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

		public RectTransform rectTransform => m_rectTransform;

		public bool activeInHierarchy => this.get_gameObject().get_activeInHierarchy();

		public event Action<object, object> OnValueChange;

		private void Awake()
		{
			CheckInit();
			ItemDragNDropListener.instance.Register(this, m_itemType);
		}

		protected virtual void CheckInit()
		{
			if (m_initialized)
			{
				return;
			}
			m_rectTransform = this.GetComponent<RectTransform>();
			m_cellRenderer = m_prefab.GetComponent<CellRenderer>();
			if (m_cellRenderer == null)
			{
				Debug.LogWarningFormat("No valid CellRenderer found in the prefab {0} for list {1}", new object[2]
				{
					m_prefab.get_name(),
					this.get_name()
				});
				return;
			}
			m_itemType = m_cellRenderer.GetValueType();
			for (int num = m_content.get_transform().get_childCount() - 1; num >= 0; num--)
			{
				CellRenderer component = m_content.get_transform().GetChild(num).GetComponent<CellRenderer>();
				if (Object.op_Implicit(component))
				{
					Object.Destroy(component.get_gameObject());
				}
			}
			m_cellRendererInstance = InitializeRenderer();
			m_initialized = true;
		}

		public void SetValue<T>(T value)
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
			}
			else
			{
				SetValue((object)value);
			}
		}

		private void SetValue(object value)
		{
			CheckInit();
			object value2 = m_value;
			m_value = value;
			m_cellRendererInstance.SetValue(value);
			this.OnValueChange?.Invoke(value2, value);
		}

		public void UpdateConfigurator(bool instant = false)
		{
			if (Object.op_Implicit(m_cellRendererInstance))
			{
				m_cellRendererInstance.OnConfiguratorUpdate(instant);
			}
		}

		private CellRenderer InitializeRenderer()
		{
			CellRenderer cellRenderer = Object.Instantiate<CellRenderer>(m_cellRenderer);
			ConfigureCellRenderer(cellRenderer, instant: true, andUpdate: false);
			return cellRenderer;
		}

		private unsafe void ConfigureCellRenderer(CellRenderer cellRenderer, bool instant, bool andUpdate)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			RectTransform component = cellRenderer.GetComponent<RectTransform>();
			component.SetParent(m_content, true);
			if (instant)
			{
				component.set_anchorMin(Vector2.get_zero());
				component.set_anchorMax(Vector2.get_one());
				component.set_pivot(new Vector2(0.5f, 0.5f));
				component.set_sizeDelta(Vector2.get_zero());
				component.set_anchoredPosition3D(Vector3.get_zero());
				component.set_localScale(Vector3.get_one());
			}
			else
			{
				Vector3 localPosition = component.get_localPosition();
				component.set_localPosition(new Vector3(((IntPtr)(void*)localPosition).x, ((IntPtr)(void*)localPosition).y));
				TweenSettingsExtensions.SetEase<Tweener>(DOTweenModuleUI.DOAnchorMin(component, Vector2.get_zero(), m_insertTweenDuration, false), m_insertTweenEase);
				TweenSettingsExtensions.SetEase<Tweener>(DOTweenModuleUI.DOAnchorMax(component, Vector2.get_one(), m_insertTweenDuration, false), m_insertTweenEase);
				TweenSettingsExtensions.SetEase<Tweener>(DOTweenModuleUI.DOPivot(component, new Vector2(0.5f, 0.5f), m_insertTweenDuration), m_insertTweenEase);
				TweenSettingsExtensions.SetEase<Tweener>(DOTweenModuleUI.DOSizeDelta(component, Vector2.get_zero(), m_insertTweenDuration, false), m_insertTweenEase);
				TweenSettingsExtensions.SetEase<Tweener>(DOTweenModuleUI.DOAnchorPos3D(component, Vector3.get_zero(), m_insertTweenDuration, false), m_insertTweenEase);
				TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(component, 1f, m_insertTweenDuration), m_insertTweenEase);
			}
			cellRenderer.dragNDropClient = this;
			cellRenderer.SetConfigurator(m_cellRendererConfigurator, andUpdate);
		}

		public void SetCellRendererConfigurator(ICellRendererConfigurator configurator)
		{
			m_cellRendererConfigurator = configurator;
			CellRenderer cellRendererInstance = m_cellRendererInstance;
			if (!(cellRendererInstance == null))
			{
				cellRendererInstance.SetConfigurator(configurator);
			}
		}

		public void SetDragAndDropValidator(IDragNDropValidator validator)
		{
			m_dragNDropValidator = validator;
		}

		public void OnBeginDrag(PointerEventData evt)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			if (m_enableDragAndDrop && RectTransformUtility.RectangleContainsScreenPoint(m_rectTransform, evt.get_position(), evt.get_pressEventCamera()) && m_value != null && (m_dragNDropValidator == null || m_dragNDropValidator.IsValidDrag(m_value)))
			{
				ItemDragNDropListener.instance.OnBeginDrag(evt, m_cellRendererInstance);
				if (m_removeOnDrag)
				{
					SetValue(null);
				}
			}
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
			if (!m_enableDragAndDrop)
			{
				return true;
			}
			if (m_dragNDropValidator != null && !m_dragNDropValidator.IsValidDrop(m_value))
			{
				return true;
			}
			if (m_insertOnDrop)
			{
				if (Object.op_Implicit(m_cellRendererInstance.get_gameObject()))
				{
					Object.Destroy(m_cellRendererInstance.get_gameObject());
				}
				m_cellRendererInstance = cellRenderer;
				ConfigureCellRenderer(cellRenderer, instant: false, andUpdate: true);
				SetValue(cellRenderer.value);
				return false;
			}
			return true;
		}

		private void OnDestroy()
		{
			if (m_itemType != null)
			{
				ItemDragNDropListener.instance.UnRegister(this, m_itemType);
			}
		}

		public Item()
			: this()
		{
		}//IL_0017: Unknown result type (might be due to invalid IL or missing references)

	}
}
