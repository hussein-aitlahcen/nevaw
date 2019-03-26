using Ankama.Cube.Data;
using Ankama.Cube.UI.Components;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ankama.Cube.UI.DeckMaker
{
	public class DeckCompanionList : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField]
		private Animator m_listSelectionAnimator;

		[SerializeField]
		private Animator m_editModeAnimator;

		[SerializeField]
		private CanvasGroup m_canvasGroup;

		[SerializeField]
		private CanvasGroup m_blockerCanvasGroup;

		[SerializeField]
		private List<Item> m_companionItems;

		[Header("Tween")]
		[SerializeField]
		private float m_minScale = 0.985f;

		[SerializeField]
		private float m_tweenDuration = 0.25f;

		private bool m_selected;

		private bool m_editable;

		private bool m_editMode;

		private readonly List<CompanionData> m_items = new List<CompanionData>();

		private Tween m_currentTween;

		private DeckBuildingEventController m_eventController;

		public DeckBuildingEventController eventController
		{
			set
			{
				m_eventController = value;
			}
		}

		public event Action OnSelected;

		public event Action<CompanionData, CompanionData, int> OnCompanionChange;

		private void Awake()
		{
			m_canvasGroup.set_alpha(0f);
			int i = 0;
			for (int count = m_companionItems.Count; i < count; i++)
			{
				int index = i;
				m_companionItems[i].OnValueChange += delegate(object previous, object value)
				{
					OnValueChanged(previous, value, index);
				};
			}
		}

		public void SetValues(IList<int> companionIds, int level)
		{
			m_items.Clear();
			int i = 0;
			for (int count = companionIds.Count; i < count; i++)
			{
				int key = companionIds[i];
				if (RuntimeData.companionDefinitions.TryGetValue(key, out CompanionDefinition value))
				{
					CompanionData companionData = new CompanionData(value, level);
					m_items.Add(companionData);
					m_companionItems[i].SetValue(companionData);
				}
				else
				{
					m_items.Add(null);
					m_companionItems[i].SetValue<CompanionData>(null);
				}
			}
			int j = m_items.Count;
			for (int num = 4; j < num; j++)
			{
				m_items.Add(null);
				m_companionItems[j].SetValue<CompanionData>(null);
			}
		}

		private void OnValueChanged(object previousValue, object value, int index)
		{
			CompanionData companionData = (CompanionData)value;
			m_items[index] = companionData;
			this.OnCompanionChange?.Invoke((CompanionData)previousValue, companionData, index);
		}

		public void SetEditMode(bool editMode, bool editable)
		{
			m_editMode = editMode;
			m_editable = editable;
			if (!editMode)
			{
				Select(select: false);
				Fade(visible: true);
			}
			else
			{
				Fade(m_selected);
			}
			m_editModeAnimator.Play(editMode ? "EditMode" : "SelectMode");
		}

		private unsafe Sequence Sequence()
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			Tween currentTween = m_currentTween;
			if (currentTween != null)
			{
				TweenExtensions.Kill(currentTween, true);
			}
			return m_currentTween = TweenSettingsExtensions.OnKill<Sequence>(DOTween.Sequence(), new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		}

		private void OnTweenKill()
		{
			m_currentTween = null;
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (!ItemDragNDropListener.instance.dragging)
			{
				SetEnableVisual(m_selected);
			}
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (!ItemDragNDropListener.instance.dragging)
			{
				SeDisableVisual(m_selected);
			}
		}

		private void SeDisableVisual(bool selected)
		{
			if (m_editMode && !selected)
			{
				Sequence val = Sequence();
				if (m_editMode)
				{
					TweenSettingsExtensions.Insert(val, 0f, Fade(visible: false));
				}
			}
		}

		private void SetEnableVisual(bool selected)
		{
			if (m_editMode && !selected)
			{
				Sequence obj = Sequence();
				TweenSettingsExtensions.Insert(obj, 0f, ShortcutExtensions.DOScale(m_canvasGroup.get_transform(), 1f, m_tweenDuration));
				TweenSettingsExtensions.Insert(obj, 0f, Fade(visible: true));
			}
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			if (m_editMode)
			{
				Select(select: true);
				this.OnSelected?.Invoke();
			}
			else
			{
				m_eventController?.OnEdit(EditModeSelection.Companion);
			}
		}

		public void Select(bool select, bool fullAnimation = true)
		{
			if (m_selected == select)
			{
				return;
			}
			m_selected = select;
			int i = 0;
			for (int count = m_companionItems.Count; i < count; i++)
			{
				m_companionItems[i].enableDragAndDrop = m_editMode;
			}
			if (select)
			{
				m_listSelectionAnimator.SetBool("Selected", true);
				if (fullAnimation)
				{
					Sequence val = Sequence();
					TweenSettingsExtensions.Insert(val, 0f, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(m_canvasGroup.get_transform(), m_minScale, 0.05f), 3));
					TweenSettingsExtensions.Append(val, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(m_canvasGroup.get_transform(), 1f, 0.1f), 3));
					if (m_editMode)
					{
						TweenSettingsExtensions.Insert(val, 0f, Fade(visible: true));
					}
				}
				return;
			}
			m_listSelectionAnimator.SetBool("Selected", false);
			if (fullAnimation)
			{
				Sequence val2 = Sequence();
				if (m_editMode)
				{
					TweenSettingsExtensions.Insert(val2, 0f, Fade(visible: false));
				}
			}
		}

		private Tween Fade(bool visible)
		{
			return DOTweenModuleUI.DOFade(m_blockerCanvasGroup, visible ? 0f : 1f, 0f);
		}

		public void SetConfigurator(ICellRendererConfigurator configurator)
		{
			int i = 0;
			for (int count = m_companionItems.Count; i < count; i++)
			{
				m_companionItems[i].SetCellRendererConfigurator(configurator);
			}
		}

		public void UpdateConfigurator(bool instant)
		{
			int i = 0;
			for (int count = m_companionItems.Count; i < count; i++)
			{
				m_companionItems[i].UpdateConfigurator(instant);
			}
		}

		public DeckCompanionList()
			: this()
		{
		}
	}
}
