using Ankama.Cube.Data;
using Ankama.Cube.Protocols.PlayerProtocol;
using Ankama.Cube.TEMPFastEnterMatch.Player;
using Ankama.Cube.UI.Components;
using Ankama.Cube.UI.Fight.Windows;
using DG.Tweening;
using Google.Protobuf.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ankama.Cube.UI.DeckMaker
{
	public class DeckDisplayer : CellRenderer<DeckSlot, IDeckDisplayConfigurator>, ListHighlightable, ISpellDataCellRendererConfigurator, ISpellCellRendererConfigurator, IWithTooltipCellRendererConfigurator, ICellRendererConfigurator, ICompanionDataCellRendererConfigurator, ICompanionCellRendererConfigurator, IWeaponDataCellRendererConfigurator
	{
		[SerializeField]
		private InputTextField m_nameTextField;

		[SerializeField]
		private DeckSpellList m_spellList;

		[SerializeField]
		private DeckCompanionList m_companionList;

		[SerializeField]
		private Image m_invalidDeckImage;

		[SerializeField]
		private CanvasGroup m_contentCanvasGroup;

		[SerializeField]
		private CanvasGroup m_saveCancelCanvasGroup;

		[SerializeField]
		private Animator m_editModeAnimator;

		[SerializeField]
		private float m_backgroundTweenDuration;

		[SerializeField]
		private Ease m_backgroundTweenEase;

		[SerializeField]
		private Button m_saveButton;

		[SerializeField]
		private Button m_cloneButton;

		[SerializeField]
		private Button m_deleteButton;

		private float m_highlightFactor;

		private bool m_contentVisible;

		private float m_visibilityFactor = 1f;

		private bool m_editMode;

		private DeckSlot m_previousValue;

		private FightTooltip m_fightTooltip;

		private TooltipPosition m_tooltipPosition;

		private DeckSlot m_uneditedValue;

		private bool m_settingValue;

		private DeckBuildingEventController m_eventController;

		public FightTooltip tooltip => m_fightTooltip;

		public TooltipPosition tooltipPosition => m_tooltipPosition;

		public DeckBuildingEventController eventController
		{
			set
			{
				m_eventController = value;
				m_spellList.eventController = value;
				m_companionList.eventController = value;
			}
		}

		public event Action<EditModeSelection> OnEditModeSelectionChanged;

		public float GetHighlightFactor()
		{
			return m_highlightFactor;
		}

		public void SetHighlightFactor(float factor)
		{
			if (!Mathf.Approximately(m_highlightFactor, factor))
			{
				m_highlightFactor = factor;
				bool flag = (double)factor >= 0.0001;
				if (flag != m_contentVisible)
				{
					m_contentVisible = flag;
					m_contentCanvasGroup.get_gameObject().SetActive(flag);
				}
				if (flag)
				{
					m_contentCanvasGroup.set_alpha(factor);
				}
			}
		}

		public float GetVisibilityFactor()
		{
			return m_visibilityFactor;
		}

		public void SetVisibilityFactor(float factor)
		{
			if (!Mathf.Approximately(m_visibilityFactor, factor))
			{
				m_visibilityFactor = factor;
				bool flag = Mathf.Approximately(factor, 0f);
				if (this.get_gameObject().get_activeSelf() && flag)
				{
					this.get_gameObject().SetActive(false);
				}
				else if (!this.get_gameObject().get_activeSelf() && !flag)
				{
					this.get_gameObject().SetActive(true);
				}
			}
		}

		protected unsafe void Awake()
		{
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Expected O, but got Unknown
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Expected O, but got Unknown
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Expected O, but got Unknown
			m_spellList.SetConfigurator(this);
			m_companionList.SetConfigurator(this);
			m_spellList.OnSpellChange += OnSpellChange;
			m_spellList.OnSelected += OnSpellSelected;
			m_companionList.OnCompanionChange += OnCompanionChange;
			m_companionList.OnSelected += OnCompanionSelected;
			m_nameTextField.onValueChanged.AddListener(new UnityAction<string>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_nameTextField.characterLimit = 30;
			SetHighlightFactor(1f);
			m_saveButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_deleteButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_cloneButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_saveCancelCanvasGroup.get_gameObject().SetActive(m_editMode);
			UpdateEditModeUI();
		}

		private void OnSpellSelected()
		{
			this.OnEditModeSelectionChanged?.Invoke(EditModeSelection.Spell);
			SetEditModeSelection(EditModeSelection.Spell);
		}

		private void OnCompanionSelected()
		{
			this.OnEditModeSelectionChanged?.Invoke(EditModeSelection.Companion);
			SetEditModeSelection(EditModeSelection.Companion);
		}

		protected override void SetValue(DeckSlot slot)
		{
			if (m_previousValue != null)
			{
				m_previousValue.OnModification -= OnDeckSlotModification;
			}
			if (slot != null)
			{
				slot.OnModification += OnDeckSlotModification;
			}
			m_previousValue = slot;
			SetValueInternal(slot);
		}

		public DeckSlot GetPreviousDeck()
		{
			return m_uneditedValue;
		}

		protected override void Clear()
		{
			SetValue(null);
		}

		public override void OnConfiguratorUpdate(bool instant)
		{
			eventController = m_configurator?.eventController;
			if (m_configurator != null)
			{
				SetTooltip(m_configurator.tooltip, m_configurator.tooltipPosition);
			}
		}

		private void SetValueInternal(DeckSlot slot)
		{
			if (slot == null)
			{
				HideAll();
				return;
			}
			if (!slot.HasDeckInfo)
			{
				HideAll();
				return;
			}
			UpdateInvalidDeck();
			DeckInfo deckInfo = slot.DeckInfo;
			bool editMode = m_editMode;
			if (!(m_nameTextField == null))
			{
				m_nameTextField.get_gameObject().SetActive(editMode);
				m_nameTextField.SetText(deckInfo.Name.Substring(0, Math.Min(deckInfo.Name.Length, 30)));
				RepeatedField<int> companions = deckInfo.Companions;
				RepeatedField<int> spells = deckInfo.Spells;
				int level = deckInfo.GetLevel(PlayerData.instance.weaponInventory);
				m_spellList.SetValues((IList<int>)spells, level);
				m_companionList.SetValues((IList<int>)companions, level);
				if (m_value.Preconstructed)
				{
					ItemDragNDropListener.instance.OnDragEndSuccessful += OnRequestValidation;
					ItemDragNDropListener.instance.OnDragEnd += OnDragFail;
				}
				m_deleteButton.set_interactable(!slot.isAvailableEmptyDeckSlot && !slot.Preconstructed);
				m_cloneButton.set_interactable(!slot.isAvailableEmptyDeckSlot && DeckUtility.GetRemainingSlotsForWeapon(deckInfo.Weapon) > 0);
				if (m_editMode)
				{
					m_uneditedValue = m_value?.Clone();
					m_saveButton.set_interactable(false);
				}
			}
		}

		private void OnDeckSlotModification(DeckSlot obj)
		{
			if (!m_settingValue)
			{
				SetValueInternal(obj);
			}
		}

		private void HideAll()
		{
			m_nameTextField.get_gameObject().SetActive(false);
			m_spellList.get_gameObject().SetActive(false);
			m_companionList.get_gameObject().SetActive(false);
		}

		private void UpdateEditModeUI()
		{
			m_nameTextField.get_gameObject().SetActive(true);
			m_nameTextField.interactable = m_editMode;
			if (m_contentCanvasGroup.get_gameObject().get_activeSelf())
			{
				m_spellList.SetEditMode(m_editMode, !m_value.Preconstructed);
				m_companionList.SetEditMode(m_editMode, !m_value.Preconstructed);
				m_editModeAnimator.Play(m_editMode ? "EditMode" : "SelectMode");
			}
		}

		public void SetTooltip(FightTooltip t, TooltipPosition tooltipPosition)
		{
			m_fightTooltip = t;
			m_tooltipPosition = tooltipPosition;
			UpdateAllChildren(instant: true);
		}

		private void UpdateAllChildren(bool instant)
		{
			m_spellList.UpdateConfigurator(instant);
			m_companionList.UpdateConfigurator(instant);
		}

		public unsafe Sequence EnterEditMode(EditModeSelection selection)
		{
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Expected O, but got Unknown
			if (m_editMode)
			{
				return null;
			}
			m_editMode = true;
			m_uneditedValue = m_value?.Clone();
			m_saveButton.set_interactable(false);
			m_saveCancelCanvasGroup.get_gameObject().SetActive(true);
			m_saveCancelCanvasGroup.set_alpha(0f);
			Sequence obj = DOTween.Sequence();
			TweenSettingsExtensions.Insert(obj, 0f, TweenSettingsExtensions.OnKill<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(DOTweenModuleUI.DOFade(m_saveCancelCanvasGroup, 1f, m_backgroundTweenDuration), m_backgroundTweenEase), new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/)));
			UpdateEditModeUI();
			SetEditModeSelection(selection);
			return obj;
		}

		public unsafe Sequence LeaveEditMode()
		{
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Expected O, but got Unknown
			if (!m_editMode)
			{
				return DOTween.Sequence();
			}
			m_editMode = false;
			m_uneditedValue = null;
			ItemDragNDropListener.instance.OnDragEndSuccessful -= OnRequestValidation;
			ItemDragNDropListener.instance.OnDragEnd -= OnDragFail;
			m_saveCancelCanvasGroup.get_gameObject().SetActive(true);
			m_saveCancelCanvasGroup.set_alpha(1f);
			UpdateEditModeUI();
			Sequence obj = DOTween.Sequence();
			TweenSettingsExtensions.Insert(obj, 0f, TweenSettingsExtensions.OnKill<Tweener>(DOTweenModuleUI.DOFade(m_saveCancelCanvasGroup, 0f, m_backgroundTweenDuration), new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/)));
			return obj;
		}

		private void CheckSaveButtonGroupVisibility()
		{
			if (m_editMode)
			{
				m_saveCancelCanvasGroup.get_gameObject().SetActive(true);
				m_saveCancelCanvasGroup.set_alpha(1f);
			}
			else
			{
				m_saveCancelCanvasGroup.get_gameObject().SetActive(false);
				m_saveCancelCanvasGroup.set_alpha(0f);
			}
		}

		public void SetEditModeSelection(EditModeSelection mode)
		{
			m_spellList.Select(mode == EditModeSelection.Spell);
			m_companionList.Select(mode == EditModeSelection.Companion);
		}

		private void OnNameChanged(string deckName)
		{
			m_settingValue = true;
			m_value?.SetName(deckName);
			m_saveButton.set_interactable(!DeckUtility.DecksAreEqual(m_value?.DeckInfo, m_uneditedValue?.DeckInfo));
			m_settingValue = false;
		}

		private void OnCompanionChange(CompanionData previousCompanionData, CompanionData companionData, int index)
		{
			m_settingValue = true;
			m_value?.SetCompanionAt((companionData != null) ? companionData.definition.get_id() : (-1), index);
			m_saveButton.set_interactable(!DeckUtility.DecksAreEqual(m_value?.DeckInfo, m_uneditedValue?.DeckInfo));
			UpdateInvalidDeck();
			m_settingValue = false;
		}

		private void OnRequestValidation()
		{
			if (m_value.Preconstructed)
			{
				m_eventController.OnClone(92537, 84166);
				ItemDragNDropListener.instance.CancelAll();
			}
		}

		private void OnDragFail()
		{
			if (m_value.Preconstructed && !m_value.DeckInfo.IsValid())
			{
				OnRequestValidation();
			}
		}

		private void OnSpellChange(SpellData previousSpellData, SpellData spellData, int index)
		{
			m_settingValue = true;
			m_value?.SetSpellAt((spellData != null) ? spellData.definition.get_id() : (-1), index);
			m_saveButton.set_interactable(!DeckUtility.DecksAreEqual(m_value?.DeckInfo, m_uneditedValue?.DeckInfo));
			UpdateInvalidDeck();
			m_settingValue = false;
		}

		private void UpdateInvalidDeck()
		{
			DeckSlot value = m_value;
			if (value == null || value.isAvailableEmptyDeckSlot)
			{
				m_invalidDeckImage.get_gameObject().SetActive(false);
			}
			else if (m_invalidDeckImage != null)
			{
				m_invalidDeckImage.get_gameObject().SetActive(!value.DeckInfo.IsValid());
			}
		}

		private void OnSave()
		{
			m_eventController?.OnSave();
		}

		private void OnCloneDeck()
		{
			m_eventController?.OnClone(92537, 0);
		}

		private void OnDelete()
		{
			m_eventController?.OnDelete();
		}

		public bool IsWeaponDataAvailable(WeaponData data)
		{
			return true;
		}

		public bool IsCompanionDataAvailable(CompanionData data)
		{
			return true;
		}

		public bool IsSpellDataAvailable(SpellData data)
		{
			return true;
		}

		public void OnCloneValidate()
		{
			ItemDragNDropListener.instance.OnDragEndSuccessful -= OnRequestValidation;
			ItemDragNDropListener.instance.OnDragEnd -= OnDragFail;
			m_nameTextField.selectable.Select();
		}
	}
}
