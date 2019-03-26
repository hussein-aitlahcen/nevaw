using Ankama.Cube.Data;
using Ankama.Cube.UI.Components;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ankama.Cube.UI.DeckMaker
{
	public class DeckSpellList : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField]
		private List<Item> m_spellItems;

		[SerializeField]
		private Animator m_listSelectionAnimator;

		[SerializeField]
		private Animator m_editModeAnimator;

		[SerializeField]
		private CanvasGroup m_canvasGroup;

		[SerializeField]
		private CanvasGroup m_blockerCanvasGroup;

		[Header("Tween")]
		[SerializeField]
		private float m_minScale = 0.95f;

		[SerializeField]
		private float m_tweenDuration = 0.15f;

		private readonly List<SpellData> m_items = new List<SpellData>();

		private bool m_selected;

		private bool m_editable;

		private bool m_editMode;

		private Tween m_currentTween;

		private DeckBuildingEventController m_eventController;

		private ICellRendererConfigurator m_configurator;

		public DeckBuildingEventController eventController
		{
			set
			{
				m_eventController = value;
			}
		}

		public event Action<SpellData, SpellData, int> OnSpellChange;

		public event Action OnSelected;

		private void Awake()
		{
			m_canvasGroup.set_alpha(0f);
			int i = 0;
			for (int count = m_spellItems.Count; i < count; i++)
			{
				int index = i;
				Item item = m_spellItems[i];
				item.SetCellRendererConfigurator(m_configurator);
				item.OnValueChange += delegate(object previous, object value)
				{
					OnValueChanged(previous, value, index);
				};
			}
		}

		public void SetValues(IList<int> spellIds, int level)
		{
			m_items.Clear();
			int i = 0;
			for (int count = spellIds.Count; i < count; i++)
			{
				int key = spellIds[i];
				if (RuntimeData.spellDefinitions.TryGetValue(key, out SpellDefinition value))
				{
					SpellData spellData = new SpellData(value, level);
					m_items.Add(spellData);
					m_spellItems[i].SetValue(spellData);
				}
				else
				{
					m_items.Add(null);
					m_spellItems[i].SetValue<SpellData>(null);
				}
			}
			int j = m_items.Count;
			for (int num = 8; j < num; j++)
			{
				m_items.Add(null);
				m_spellItems[j].SetValue<SpellData>(null);
			}
		}

		private void OnValueChanged(object previousValue, object value, int index)
		{
			SpellData spellData = (SpellData)value;
			m_items[index] = spellData;
			this.OnSpellChange?.Invoke((SpellData)previousValue, spellData, index);
			DeckEditItemPointerListener componentInChildren = m_spellItems[index].get_gameObject().GetComponentInChildren<DeckEditItemPointerListener>();
			if (componentInChildren != null)
			{
				componentInChildren.RemoveComponent();
			}
		}

		public void SetEditMode(bool editMode, bool editable)
		{
			m_editMode = editMode;
			m_editable = editable;
			Select(editMode);
			if (!editMode)
			{
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
			if (!ItemDragNDropListener.instance.dragging && !m_selected)
			{
				SetEnableVisual(m_selected);
			}
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (!ItemDragNDropListener.instance.dragging && !m_selected)
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
				m_eventController?.OnEdit(EditModeSelection.Spell);
			}
		}

		public void Select(bool select, bool fullAnimation = true)
		{
			if (m_selected != select)
			{
				m_selected = select;
				int i = 0;
				for (int count = m_spellItems.Count; i < count; i++)
				{
					m_spellItems[i].enableDragAndDrop = m_editMode;
				}
				PlaySelectAnimation(select, fullAnimation);
			}
		}

		private void PlaySelectAnimation(bool select, bool fullAnimation = true)
		{
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
			m_configurator = configurator;
			int i = 0;
			for (int count = m_spellItems.Count; i < count; i++)
			{
				m_spellItems[i].SetCellRendererConfigurator(configurator);
			}
		}

		public void UpdateConfigurator(bool instant)
		{
			int i = 0;
			for (int count = m_spellItems.Count; i < count; i++)
			{
				m_spellItems[i].UpdateConfigurator(instant);
			}
		}

		public DeckSpellList()
			: this()
		{
		}
	}
}
