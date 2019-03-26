using Ankama.Cube.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Components
{
	[AddComponentMenu("UI/Textfield Dropdown", 35)]
	[RequireComponent(typeof(RectTransform))]
	public class TextFieldDropdown : Selectable, IPointerClickHandler, IEventSystemHandler, ISubmitHandler, ICancelHandler
	{
		protected internal class DropdownItem : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, ICancelHandler
		{
			[SerializeField]
			private RawTextField m_Text;

			[SerializeField]
			private Image m_Image;

			[SerializeField]
			private RectTransform m_RectTransform;

			[SerializeField]
			private Toggle m_Toggle;

			public RawTextField text
			{
				get
				{
					return m_Text;
				}
				set
				{
					m_Text = value;
				}
			}

			public Image image
			{
				get
				{
					return m_Image;
				}
				set
				{
					m_Image = value;
				}
			}

			public RectTransform rectTransform
			{
				get
				{
					return m_RectTransform;
				}
				set
				{
					m_RectTransform = value;
				}
			}

			public Toggle toggle
			{
				get
				{
					return m_Toggle;
				}
				set
				{
					m_Toggle = value;
				}
			}

			public virtual void OnPointerEnter(PointerEventData eventData)
			{
				EventSystem.get_current().SetSelectedGameObject(this.get_gameObject());
			}

			public virtual void OnCancel(BaseEventData eventData)
			{
				Dropdown componentInParent = this.GetComponentInParent<Dropdown>();
				if (Object.op_Implicit(componentInParent))
				{
					componentInParent.Hide();
				}
			}

			public DropdownItem()
				: this()
			{
			}
		}

		[SerializeField]
		private RectTransform m_Template;

		[SerializeField]
		private RawTextField m_CaptionText;

		[SerializeField]
		private Image m_CaptionImage;

		[Space]
		[SerializeField]
		private RawTextField m_ItemText;

		[SerializeField]
		private Image m_ItemImage;

		[Space]
		[SerializeField]
		private int m_Value;

		[Space]
		[SerializeField]
		private OptionDataList m_Options = new OptionDataList();

		[Space]
		[SerializeField]
		private DropdownEvent m_OnValueChanged = new DropdownEvent();

		private GameObject m_Dropdown;

		private GameObject m_Blocker;

		private List<DropdownItem> m_Items = new List<DropdownItem>();

		private bool validTemplate;

		private static OptionData s_NoOptionData = new OptionData();

		public RectTransform template
		{
			get
			{
				return m_Template;
			}
			set
			{
				m_Template = value;
				RefreshShownValue();
			}
		}

		public RawTextField captionText
		{
			get
			{
				return m_CaptionText;
			}
			set
			{
				m_CaptionText = value;
				RefreshShownValue();
			}
		}

		public Image captionImage
		{
			get
			{
				return m_CaptionImage;
			}
			set
			{
				m_CaptionImage = value;
				RefreshShownValue();
			}
		}

		public RawTextField itemText
		{
			get
			{
				return m_ItemText;
			}
			set
			{
				m_ItemText = value;
				RefreshShownValue();
			}
		}

		public Image itemImage
		{
			get
			{
				return m_ItemImage;
			}
			set
			{
				m_ItemImage = value;
				RefreshShownValue();
			}
		}

		public List<OptionData> options
		{
			get
			{
				return m_Options.get_options();
			}
			set
			{
				m_Options.set_options(value);
				RefreshShownValue();
			}
		}

		public DropdownEvent onValueChanged
		{
			get
			{
				return m_OnValueChanged;
			}
			set
			{
				m_OnValueChanged = value;
			}
		}

		public int value
		{
			get
			{
				return m_Value;
			}
			set
			{
				if (!Application.get_isPlaying() || (value != m_Value && options.Count != 0))
				{
					m_Value = Mathf.Clamp(value, 0, options.Count - 1);
					RefreshShownValue();
					UISystemProfilerApi.AddMarker("Dropdown.value", this);
					m_OnValueChanged.Invoke(m_Value);
				}
			}
		}

		protected override void Awake()
		{
			if (Object.op_Implicit(m_CaptionImage))
			{
				m_CaptionImage.set_enabled(m_CaptionImage.get_sprite() != null);
			}
			if (Object.op_Implicit(m_Template))
			{
				m_Template.get_gameObject().SetActive(false);
			}
		}

		public void RefreshShownValue()
		{
			OptionData val = s_NoOptionData;
			if (options.Count > 0)
			{
				val = options[Mathf.Clamp(m_Value, 0, options.Count - 1)];
			}
			if (Object.op_Implicit(m_CaptionText))
			{
				if (val != null && val.get_text() != null)
				{
					m_CaptionText.SetText(val.get_text());
				}
				else
				{
					m_CaptionText.SetText("");
				}
			}
			if (Object.op_Implicit(m_CaptionImage))
			{
				if (val != null)
				{
					m_CaptionImage.set_sprite(val.get_image());
				}
				else
				{
					m_CaptionImage.set_sprite(null);
				}
				m_CaptionImage.set_enabled(m_CaptionImage.get_sprite() != null);
			}
		}

		public void AddOptions(List<OptionData> options)
		{
			this.options.AddRange(options);
			RefreshShownValue();
		}

		public void AddOptions(List<string> options)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Expected O, but got Unknown
			for (int i = 0; i < options.Count; i++)
			{
				this.options.Add(new OptionData(options[i]));
			}
			RefreshShownValue();
		}

		public void AddOptions(List<Sprite> options)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Expected O, but got Unknown
			for (int i = 0; i < options.Count; i++)
			{
				this.options.Add(new OptionData(options[i]));
			}
			RefreshShownValue();
		}

		public void ClearOptions()
		{
			options.Clear();
			RefreshShownValue();
		}

		private void SetupTemplate()
		{
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Expected O, but got Unknown
			validTemplate = false;
			if (!Object.op_Implicit(m_Template))
			{
				Debug.LogError((object)"The dropdown template is not assigned. The template needs to be assigned and must have a child GameObject with a Toggle component serving as the item.", this);
				return;
			}
			GameObject gameObject = m_Template.get_gameObject();
			gameObject.SetActive(true);
			Toggle componentInChildren = m_Template.GetComponentInChildren<Toggle>();
			validTemplate = true;
			if (!Object.op_Implicit(componentInChildren) || componentInChildren.get_transform() == template)
			{
				validTemplate = false;
				Debug.LogError((object)"The dropdown template is not valid. The template must have a child GameObject with a Toggle component serving as the item.", template);
			}
			else if (!(componentInChildren.get_transform().get_parent() is RectTransform))
			{
				validTemplate = false;
				Debug.LogError((object)"The dropdown template is not valid. The child GameObject with a Toggle component (the item) must have a RectTransform on its parent.", template);
			}
			else if (itemText != null && !itemText.get_transform().IsChildOf(componentInChildren.get_transform()))
			{
				validTemplate = false;
				Debug.LogError((object)"The dropdown template is not valid. The Item Text must be on the item GameObject or children of it.", template);
			}
			else if (itemImage != null && !itemImage.get_transform().IsChildOf(componentInChildren.get_transform()))
			{
				validTemplate = false;
				Debug.LogError((object)"The dropdown template is not valid. The Item Image must be on the item GameObject or children of it.", template);
			}
			if (!validTemplate)
			{
				gameObject.SetActive(false);
				return;
			}
			DropdownItem dropdownItem = componentInChildren.get_gameObject().AddComponent<DropdownItem>();
			dropdownItem.text = m_ItemText;
			dropdownItem.image = m_ItemImage;
			dropdownItem.toggle = componentInChildren;
			dropdownItem.rectTransform = componentInChildren.get_transform();
			Canvas orAddComponent = TextFieldDropdown.GetOrAddComponent<Canvas>(gameObject);
			orAddComponent.set_overrideSorting(true);
			orAddComponent.set_sortingOrder(30000);
			TextFieldDropdown.GetOrAddComponent<CustomGraphicRaycaster>(gameObject);
			TextFieldDropdown.GetOrAddComponent<CanvasGroup>(gameObject);
			gameObject.SetActive(false);
			validTemplate = true;
		}

		private static T GetOrAddComponent<T>(GameObject go) where T : Component
		{
			T val = go.GetComponent<T>();
			if (!Object.op_Implicit((object)val))
			{
				val = go.AddComponent<T>();
			}
			return val;
		}

		public virtual void OnPointerClick(PointerEventData eventData)
		{
			Show();
		}

		public virtual void OnSubmit(BaseEventData eventData)
		{
			Show();
		}

		public virtual void OnCancel(BaseEventData eventData)
		{
			Hide();
		}

		public unsafe void Show()
		{
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_023e: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0251: Unknown result type (might be due to invalid IL or missing references)
			//IL_029d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02de: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0303: Unknown result type (might be due to invalid IL or missing references)
			//IL_0311: Unknown result type (might be due to invalid IL or missing references)
			//IL_0319: Unknown result type (might be due to invalid IL or missing references)
			//IL_031e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0328: Unknown result type (might be due to invalid IL or missing references)
			//IL_032d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0344: Unknown result type (might be due to invalid IL or missing references)
			//IL_034f: Unknown result type (might be due to invalid IL or missing references)
			//IL_035c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0385: Unknown result type (might be due to invalid IL or missing references)
			//IL_038a: Unknown result type (might be due to invalid IL or missing references)
			//IL_039f: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_042e: Unknown result type (might be due to invalid IL or missing references)
			//IL_043d: Unknown result type (might be due to invalid IL or missing references)
			//IL_044b: Unknown result type (might be due to invalid IL or missing references)
			//IL_045a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0468: Unknown result type (might be due to invalid IL or missing references)
			//IL_0472: Unknown result type (might be due to invalid IL or missing references)
			//IL_0479: Unknown result type (might be due to invalid IL or missing references)
			//IL_0493: Unknown result type (might be due to invalid IL or missing references)
			//IL_049c: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
			if (!this.IsActive() || !this.IsInteractable() || m_Dropdown != null)
			{
				return;
			}
			if (!validTemplate)
			{
				SetupTemplate();
				if (!validTemplate)
				{
					return;
				}
			}
			Canvas rootCanvas = this.get_gameObject().GetRootCanvas();
			if (rootCanvas == null)
			{
				return;
			}
			m_Template.get_gameObject().SetActive(true);
			m_Dropdown = CreateDropdownList(m_Template.get_gameObject());
			m_Dropdown.set_name("Dropdown List");
			m_Dropdown.SetActive(true);
			RectTransform val = m_Dropdown.get_transform() as RectTransform;
			val.SetParent(m_Template.get_transform().get_parent(), false);
			DropdownItem componentInChildren = m_Dropdown.GetComponentInChildren<DropdownItem>();
			RectTransform val2 = componentInChildren.rectTransform.get_parent().get_gameObject().get_transform() as RectTransform;
			componentInChildren.rectTransform.get_gameObject().SetActive(true);
			Rect rect = val2.get_rect();
			Rect rect2 = componentInChildren.rectTransform.get_rect();
			Vector2 val3 = rect2.get_min() - rect.get_min() + Vector2.op_Implicit(componentInChildren.rectTransform.get_localPosition());
			Vector2 val4 = rect2.get_max() - rect.get_max() + Vector2.op_Implicit(componentInChildren.rectTransform.get_localPosition());
			Vector2 size = rect2.get_size();
			m_Items.Clear();
			Toggle val5 = null;
			for (int i = 0; i < options.Count; i++)
			{
				OptionData data = options[i];
				DropdownItem item = AddItem(data, value == i, componentInChildren, m_Items);
				if (!(item == null))
				{
					item.toggle.set_isOn(value == i);
					_003C_003Ec__DisplayClass49_0 _003C_003Ec__DisplayClass49_;
					item.toggle.onValueChanged.AddListener(new UnityAction<bool>((object)_003C_003Ec__DisplayClass49_, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
					if (item.toggle.get_isOn())
					{
						item.toggle.Select();
					}
					if (val5 != null)
					{
						Navigation navigation = val5.get_navigation();
						Navigation navigation2 = item.toggle.get_navigation();
						navigation.set_mode(4);
						navigation2.set_mode(4);
						navigation.set_selectOnDown(item.toggle);
						navigation.set_selectOnRight(item.toggle);
						navigation2.set_selectOnLeft(val5);
						navigation2.set_selectOnUp(val5);
						val5.set_navigation(navigation);
						item.toggle.set_navigation(navigation2);
					}
					val5 = item.toggle;
				}
			}
			Vector2 sizeDelta = val2.get_sizeDelta();
			sizeDelta.y = ((IntPtr)(void*)size).y * (float)m_Items.Count + ((IntPtr)(void*)val3).y - ((IntPtr)(void*)val4).y;
			val2.set_sizeDelta(sizeDelta);
			Rect rect3 = val.get_rect();
			float height = rect3.get_height();
			rect3 = val2.get_rect();
			float num = height - rect3.get_height();
			if (num > 0f)
			{
				val.set_sizeDelta(new Vector2(((IntPtr)(void*)val.get_sizeDelta()).x, ((IntPtr)(void*)val.get_sizeDelta()).y - num));
			}
			Vector3[] array = (Vector3[])new Vector3[4];
			val.GetWorldCorners(array);
			RectTransform val6 = rootCanvas.get_transform() as RectTransform;
			Rect rect4 = val6.get_rect();
			for (int j = 0; j < 2; j++)
			{
				bool flag = false;
				int num2 = 0;
				while (num2 < 4)
				{
					Vector3 val7 = val6.InverseTransformPoint(array[num2]);
					float num3 = val7.get_Item(j);
					Vector2 val8 = rect4.get_min();
					if (!(num3 < val8.get_Item(j)))
					{
						float num4 = val7.get_Item(j);
						val8 = rect4.get_max();
						if (!(num4 > val8.get_Item(j)))
						{
							num2++;
							continue;
						}
					}
					flag = true;
					break;
				}
				if (flag)
				{
					RectTransformUtility.FlipLayoutOnAxis(val, j, false, false);
				}
			}
			for (int k = 0; k < m_Items.Count; k++)
			{
				RectTransform rectTransform = m_Items[k].rectTransform;
				rectTransform.set_anchorMin(new Vector2(((IntPtr)(void*)rectTransform.get_anchorMin()).x, 0f));
				rectTransform.set_anchorMax(new Vector2(((IntPtr)(void*)rectTransform.get_anchorMax()).x, 0f));
				rectTransform.set_anchoredPosition(new Vector2(((IntPtr)(void*)rectTransform.get_anchoredPosition()).x, ((IntPtr)(void*)val3).y + ((IntPtr)(void*)size).y * (float)(m_Items.Count - 1 - k) + ((IntPtr)(void*)size).y * ((IntPtr)(void*)rectTransform.get_pivot()).y));
				rectTransform.set_sizeDelta(new Vector2(((IntPtr)(void*)rectTransform.get_sizeDelta()).x, ((IntPtr)(void*)size).y));
			}
			m_Template.get_gameObject().SetActive(false);
			componentInChildren.get_gameObject().SetActive(false);
			m_Blocker = CreateBlocker(rootCanvas);
		}

		protected unsafe virtual GameObject CreateBlocker(Canvas rootCanvas)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Expected O, but got Unknown
			//IL_00ad: Expected O, but got Unknown
			GameObject val = new GameObject("Blocker");
			RectTransform obj = val.AddComponent<RectTransform>();
			obj.SetParent(rootCanvas.get_transform(), false);
			obj.set_anchorMin(Vector2.op_Implicit(Vector3.get_zero()));
			obj.set_anchorMax(Vector2.op_Implicit(Vector3.get_one()));
			obj.set_sizeDelta(Vector2.get_zero());
			Canvas obj2 = val.AddComponent<Canvas>();
			obj2.set_overrideSorting(true);
			Canvas component = m_Dropdown.GetComponent<Canvas>();
			obj2.set_sortingLayerID(component.get_sortingLayerID());
			obj2.set_sortingOrder(component.get_sortingOrder() - 1);
			val.AddComponent<CustomGraphicRaycaster>();
			val.AddComponent<Image>().set_color(Color.get_clear());
			val.AddComponent<Button>().get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			return val;
		}

		protected virtual void DestroyBlocker(GameObject blocker)
		{
			Object.Destroy(blocker);
		}

		protected virtual GameObject CreateDropdownList(GameObject template)
		{
			return Object.Instantiate<GameObject>(template);
		}

		protected virtual void DestroyDropdownList(GameObject dropdownList)
		{
			Object.Destroy(dropdownList);
		}

		protected virtual DropdownItem CreateItem(DropdownItem itemTemplate)
		{
			return Object.Instantiate<DropdownItem>(itemTemplate);
		}

		private DropdownItem AddItem(OptionData data, bool selected, DropdownItem itemTemplate, List<DropdownItem> items)
		{
			DropdownItem dropdownItem = CreateItem(itemTemplate);
			dropdownItem.rectTransform.SetParent(itemTemplate.rectTransform.get_parent(), false);
			dropdownItem.get_gameObject().SetActive(true);
			dropdownItem.get_gameObject().set_name("Item " + items.Count + ((data.get_text() != null) ? (": " + data.get_text()) : ""));
			if (dropdownItem.toggle != null)
			{
				dropdownItem.toggle.set_isOn(false);
			}
			if (Object.op_Implicit(dropdownItem.text))
			{
				dropdownItem.text.SetText(data.get_text());
			}
			if (Object.op_Implicit(dropdownItem.image))
			{
				dropdownItem.image.set_sprite(data.get_image());
				dropdownItem.image.set_enabled(dropdownItem.image.get_sprite() != null);
			}
			items.Add(dropdownItem);
			return dropdownItem;
		}

		public void Hide()
		{
			if (m_Dropdown != null)
			{
				DestroyDropdownList(m_Dropdown);
			}
			m_Dropdown = null;
			if (m_Blocker != null)
			{
				DestroyBlocker(m_Blocker);
			}
			m_Blocker = null;
			this.Select();
		}

		private void OnSelectItem(Toggle toggle)
		{
			if (!toggle.get_isOn())
			{
				toggle.set_isOn(true);
			}
			int num = -1;
			Transform transform = toggle.get_transform();
			Transform parent = transform.get_parent();
			for (int i = 0; i < parent.get_childCount(); i++)
			{
				if (parent.GetChild(i) == transform)
				{
					num = i - 1;
					break;
				}
			}
			if (num >= 0)
			{
				value = num;
				Hide();
			}
		}

		public TextFieldDropdown()
			: this()
		{
		}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown

	}
}
