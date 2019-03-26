using Ankama.Cube.Data;
using Ankama.Cube.Data.Castable;
using Ankama.Cube.Data.UI.Localization.TextFormatting;
using Ankama.Cube.Extensions;
using Ankama.Cube.TEMPFastEnterMatch.Player;
using Ankama.Cube.UI.Components;
using Ankama.Cube.UI.Fight.Windows;
using Ankama.Utilities;
using DataEditor;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Ankama.Cube.UI.DeckMaker
{
	public class DeckEditModeUI : MonoBehaviour, ISpellDataCellRendererConfigurator, ISpellCellRendererConfigurator, IWithTooltipCellRendererConfigurator, ICellRendererConfigurator, ICompanionDataCellRendererConfigurator, ICompanionCellRendererConfigurator, IWeaponDataCellRendererConfigurator, IDragNDropValidator
	{
		private class WeaponDataComparer : Comparer<WeaponData>
		{
			public override int Compare(WeaponData x, WeaponData y)
			{
				if (x == y)
				{
					return 0;
				}
				if (x == null)
				{
					return 1;
				}
				if (y == null)
				{
					return -1;
				}
				return x.definition.get_id() - y.definition.get_id();
			}
		}

		private class CompanionDataComparer : Comparer<CompanionData>
		{
			public override int Compare(CompanionData x, CompanionData y)
			{
				if (x == y)
				{
					return 0;
				}
				if (x == null)
				{
					return 1;
				}
				if (y == null)
				{
					return -1;
				}
				CaracId caracId = CaracId.Armor;
				int value = int.MaxValue;
				CaracId caracId2 = CaracId.Armor;
				int value2 = int.MaxValue;
				foreach (Cost item in x.definition.cost)
				{
					ElementPointsCost elementPointsCost = item as ElementPointsCost;
					if (elementPointsCost != null)
					{
						caracId = elementPointsCost.element;
						elementPointsCost.value.GetValue(null, out value);
						break;
					}
				}
				foreach (Cost item2 in y.definition.cost)
				{
					ElementPointsCost elementPointsCost2 = item2 as ElementPointsCost;
					if (elementPointsCost2 != null)
					{
						caracId2 = elementPointsCost2.element;
						elementPointsCost2.value.GetValue(null, out value2);
						break;
					}
				}
				if (CaracId.Armor != caracId && caracId2 != CaracId.Armor)
				{
					if (caracId != caracId2)
					{
						return caracId - caracId2;
					}
					return value - value2;
				}
				return x.definition.get_id() - y.definition.get_id();
			}
		}

		private class SpellDataComparer : Comparer<SpellData>
		{
			public override int Compare(SpellData x, SpellData y)
			{
				if (x == y)
				{
					return 0;
				}
				if (x == null)
				{
					return 1;
				}
				if (y == null)
				{
					return -1;
				}
				int num = x.definition.element.CompareTo(y.definition.element);
				if (num != 0)
				{
					return num;
				}
				int value = int.MaxValue;
				int value2 = int.MaxValue;
				foreach (Cost cost in x.definition.costs)
				{
					(cost as ActionPointsCost)?.value.GetValue(null, out value);
				}
				foreach (Cost cost2 in y.definition.costs)
				{
					(cost2 as ActionPointsCost)?.value.GetValue(null, out value2);
				}
				if (value != value2)
				{
					return value - value2;
				}
				return x.definition.get_id() - y.definition.get_id();
			}
		}

		[SerializeField]
		private RectTransform m_spellsTransform;

		[SerializeField]
		private DynamicList m_spellsList;

		[SerializeField]
		private RectTransform m_companionsTransform;

		[SerializeField]
		private DynamicList m_companionsList;

		[SerializeField]
		private RectTransform m_inPoint;

		[SerializeField]
		private RectTransform m_outPoint;

		[SerializeField]
		private Ease m_moveTweenEase;

		[SerializeField]
		private float m_moveTweenDuration;

		[SerializeField]
		private Ease m_fadeTweenEase;

		[SerializeField]
		private float m_fadeTweenDuration;

		[SerializeField]
		private CanvasGroup m_canvasGroup;

		[SerializeField]
		private CanvasGroup m_spellListCanvasGroup;

		[SerializeField]
		private CanvasGroup m_companionsCanvasGroup;

		[Header("Filters")]
		[SerializeField]
		private DeckEditToggleParent m_filterParent;

		[Header("Sounds")]
		[SerializeField]
		private UnityEvent m_openCanvasEvent;

		[SerializeField]
		private UnityEvent m_onSwapEditMode;

		[SerializeField]
		private UnityEvent m_onGrabSpell;

		[SerializeField]
		private UnityEvent m_onDropSpell;

		private SpellDataComparer m_spellComparer;

		private CompanionDataComparer m_companionDataComparer;

		private CanvasGroup m_currentListCanvasGroup;

		private CanvasGroup m_nextListCanvasGroup;

		private RectTransform m_currentDisplayedTransform;

		private RectTransform m_nextDisplayedTransform;

		private EditModeSelection? m_currentMode;

		private EditModeSelection? m_previousValideMode;

		private List<SpellData> m_allSpells;

		private List<CompanionData> m_allCompanions;

		private WeaponDefinition m_currentWeapon;

		private DeckSlot m_slot;

		private HashSet<CaracId> m_caracIdsEnableToggle;

		private HashSet<Element> m_elementsEnableToggle;

		private FightTooltip m_tooltip;

		private TooltipPosition m_tooltipPosition;

		public FightTooltip tooltip => m_tooltip;

		public TooltipPosition tooltipPosition => m_tooltipPosition;

		private unsafe void Awake()
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			m_filterParent.Initialise(OnFilterChange, ((IntPtr)(void*)m_outPoint.get_anchoredPosition()).y - ((IntPtr)(void*)m_inPoint.get_anchoredPosition()).y);
			this.get_gameObject().SetActive(false);
			m_spellsList.SetCellRendererConfigurator(this);
			m_companionsList.SetCellRendererConfigurator(this);
			m_spellsList.SetDragAndDropValidator(this);
			m_companionsList.SetDragAndDropValidator(this);
			InitPoints(m_spellsTransform);
			InitPoints(m_companionsTransform);
			InitLists();
			m_canvasGroup.set_alpha(0f);
			DragNDropListener.instance.OnDragBegin += OnDragBegin;
			DragNDropListener.instance.OnDragEnd += OnDragEnd;
		}

		private void OnDestroy()
		{
			DragNDropListener.instance.OnDragBegin -= OnDragBegin;
			DragNDropListener.instance.OnDragEnd -= OnDragEnd;
		}

		private void InitPoints(RectTransform rectTransform)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			rectTransform.get_gameObject().SetActive(false);
			rectTransform.set_anchorMin(m_outPoint.get_anchorMin());
			rectTransform.set_anchorMax(m_outPoint.get_anchorMax());
			rectTransform.set_pivot(m_outPoint.get_pivot());
			rectTransform.set_anchoredPosition(m_outPoint.get_anchoredPosition());
		}

		public void InitLists()
		{
			God god = PlayerData.instance.god;
			List<SpellData> list = new List<SpellData>();
			int level = 1;
			m_spellComparer = new SpellDataComparer();
			m_companionDataComparer = new CompanionDataComparer();
			foreach (SpellDefinition value in RuntimeData.spellDefinitions.Values)
			{
				if (value.god == god && value.spellType == SpellType.Normal)
				{
					list.Add(new SpellData(value, level));
				}
			}
			list.Sort(m_spellComparer);
			m_allSpells = list;
			List<CompanionData> list2 = new List<CompanionData>();
			foreach (CompanionDefinition value2 in RuntimeData.companionDefinitions.Values)
			{
				if (PlayerData.instance.companionInventory.Contains(value2.get_id()))
				{
					list2.Add(new CompanionData(value2, level));
				}
			}
			list2.Sort(m_companionDataComparer);
			m_allCompanions = list2;
			OnFilterChange();
			m_spellsList.SetValues(m_allSpells);
			m_companionsList.SetValues(m_allCompanions);
		}

		public void RefreshList(DeckSlot slot)
		{
			SetSlot(slot);
			m_filterParent.OnEditModeChange(m_currentMode.Value);
			OnFilterChange();
			UpdateAllChildren(instant: true);
			m_spellsList.AccurateReLayout();
			m_spellsList.UpdateAllConfigurators();
			m_companionsList.UpdateAllConfigurators();
			UpdateLists();
		}

		private void UpdateLists()
		{
			UpdateCompanionList();
			UpdateSpellList();
		}

		private void UpdateCompanionList()
		{
			m_companionsList.UpdateFilter();
		}

		private void UpdateSpellList()
		{
			m_spellsList.UpdateFilter();
		}

		public void SetSlot(DeckSlot slot)
		{
			m_slot = slot;
		}

		public unsafe Tween Display(EditModeSelection selection, DeckSlot slot)
		{
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Expected O, but got Unknown
			m_slot = slot;
			if (m_slot != null)
			{
				m_slot.OnModification += OnSlotModification;
			}
			OnFilterChange();
			UpdateAllChildren(instant: true);
			m_canvasGroup.set_alpha(0f);
			m_spellsTransform.get_gameObject().SetActive(selection == EditModeSelection.Spell);
			m_companionsTransform.get_gameObject().SetActive(selection == EditModeSelection.Companion);
			this.get_gameObject().SetActive(true);
			Sequence val = DOTween.Sequence();
			TweenSettingsExtensions.Insert(val, 0f, TweenSettingsExtensions.SetEase<Tweener>(DOTweenModuleUI.DOFade(m_canvasGroup, 1f, m_fadeTweenDuration), m_fadeTweenEase));
			Tween val2 = CreateEditModeSelectionTween(selection);
			if (val2 != null)
			{
				TweenSettingsExtensions.Insert(val, 0f, val2);
			}
			val.onComplete = new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/);
			return val;
		}

		public unsafe Tween Hide()
		{
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Expected O, but got Unknown
			if (m_slot != null)
			{
				m_slot.OnModification -= OnSlotModification;
			}
			Sequence val = DOTween.Sequence();
			TweenSettingsExtensions.Insert(val, 0f, TweenSettingsExtensions.SetEase<Tweener>(DOTweenModuleUI.DOFade(m_canvasGroup, 0f, m_fadeTweenDuration), m_fadeTweenEase));
			Tween val2 = CreateEditModeSelectionTween(null);
			if (val2 != null)
			{
				TweenSettingsExtensions.Insert(val, 0f, val2);
			}
			TweenSettingsExtensions.InsertCallback(val, TweenExtensions.Duration(val, true), new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			return val;
		}

		private void OnHideEnd()
		{
			this.get_gameObject().SetActive(false);
		}

		private void OnSlotModification(DeckSlot obj)
		{
			UpdateAllChildren(instant: false);
			UpdateCompanionList();
		}

		public void SetEditModeSelection(EditModeSelection? selection)
		{
			Main.monoBehaviour.StartCoroutine(SetEditModeSelectionCoroutine(selection));
			m_openCanvasEvent.Invoke();
		}

		private IEnumerator SetEditModeSelectionCoroutine(EditModeSelection? selection)
		{
			Tween val = CreateEditModeSelectionTween(selection);
			if (val != null)
			{
				yield return TweenExtensions.WaitForKill(val);
			}
		}

		private Tween CreateEditModeSelectionTween(EditModeSelection? selection)
		{
			if (m_currentMode == selection)
			{
				return null;
			}
			m_currentMode = selection;
			if (selection.HasValue)
			{
				m_previousValideMode = selection;
			}
			switch (selection)
			{
			case EditModeSelection.Spell:
				m_nextDisplayedTransform = m_spellsTransform;
				m_nextListCanvasGroup = m_spellListCanvasGroup;
				UpdateSpellList();
				break;
			case EditModeSelection.Companion:
				m_nextDisplayedTransform = m_companionsTransform;
				m_nextListCanvasGroup = m_companionsCanvasGroup;
				UpdateCompanionList();
				break;
			case null:
				m_nextDisplayedTransform = null;
				break;
			default:
				throw new ArgumentOutOfRangeException("selection", selection, null);
			}
			return TweenLists();
		}

		private unsafe Tween TweenLists()
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Expected O, but got Unknown
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Expected O, but got Unknown
			Tween val = null;
			Tween val2 = null;
			if (Object.op_Implicit(m_currentDisplayedTransform))
			{
				Sequence obj = DOTween.Sequence();
				TweenSettingsExtensions.Insert(obj, 0f, TweenSettingsExtensions.SetEase<Tweener>(DOTweenModuleUI.DOAnchorPos(m_currentDisplayedTransform, m_outPoint.get_anchoredPosition(), m_moveTweenDuration, false), m_moveTweenEase));
				TweenSettingsExtensions.SetEase<Sequence>(TweenSettingsExtensions.Insert(obj, 0f, DOTweenModuleUI.DOFade(m_currentListCanvasGroup, 0f, m_fadeTweenDuration)), m_fadeTweenEase);
				TweenSettingsExtensions.Insert(obj, 0f, m_filterParent.TweenOut(m_moveTweenDuration, m_moveTweenEase));
				val = obj;
			}
			if (Object.op_Implicit(m_nextDisplayedTransform))
			{
				m_onSwapEditMode.Invoke();
				m_nextListCanvasGroup.set_alpha(0f);
				m_nextDisplayedTransform.get_gameObject().SetActive(true);
				Sequence obj2 = DOTween.Sequence();
				TweenSettingsExtensions.Insert(obj2, 0f, TweenSettingsExtensions.SetEase<Tweener>(DOTweenModuleUI.DOAnchorPos(m_nextDisplayedTransform, m_inPoint.get_anchoredPosition(), m_moveTweenDuration, false), m_moveTweenEase));
				TweenSettingsExtensions.SetEase<Sequence>(TweenSettingsExtensions.Insert(obj2, 0f, DOTweenModuleUI.DOFade(m_nextListCanvasGroup, 1f, m_fadeTweenDuration)), m_fadeTweenEase);
				TweenSettingsExtensions.Insert(obj2, 0f, m_filterParent.TweenIn(m_moveTweenDuration, m_moveTweenEase));
				val2 = obj2;
			}
			if (m_currentDisplayedTransform == null && m_nextDisplayedTransform == null)
			{
				return null;
			}
			TweenSettingsExtensions.OnStart<Tween>(val2, new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			Sequence val3 = DOTween.Sequence();
			if (val != null)
			{
				TweenSettingsExtensions.Append(val3, val);
			}
			if (val2 != null)
			{
				if (val != null)
				{
					TweenSettingsExtensions.Insert(val3, m_moveTweenDuration / 2f, val2);
				}
				else
				{
					TweenSettingsExtensions.Append(val3, val2);
				}
			}
			TweenSettingsExtensions.OnKill<Sequence>(val3, new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			return val3;
		}

		private void OnTweenMiddle()
		{
			if (m_currentDisplayedTransform != null)
			{
				m_currentDisplayedTransform.get_gameObject().SetActive(false);
			}
			if (m_nextDisplayedTransform != null)
			{
				m_nextDisplayedTransform.get_gameObject().SetActive(true);
			}
			if (m_currentMode.HasValue)
			{
				m_filterParent.OnEditModeChange(m_currentMode.Value);
			}
		}

		private void OnTweenEnd()
		{
			m_currentListCanvasGroup = m_nextListCanvasGroup;
			m_currentDisplayedTransform = m_nextDisplayedTransform;
			m_nextDisplayedTransform = null;
			m_nextListCanvasGroup = null;
		}

		private void OnFilterChange()
		{
			HashSet<CaracId> hashSet = new HashSet<CaracId>();
			HashSet<Element> hashSet2 = new HashSet<Element>();
			m_caracIdsEnableToggle = new HashSet<CaracId>();
			m_elementsEnableToggle = new HashSet<Element>();
			if (m_slot != null)
			{
				foreach (DeckEditToggleFilter item in m_filterParent.ToggleFilter)
				{
					if (item.IsEnabled())
					{
						m_caracIdsEnableToggle.Add(item.GetElement());
						m_elementsEnableToggle.Add(item.GetSpellElement());
					}
					hashSet.Add(item.GetElement());
					hashSet2.Add(item.GetSpellElement());
				}
				if (!m_caracIdsEnableToggle.Any())
				{
					m_caracIdsEnableToggle = hashSet;
					m_elementsEnableToggle = hashSet2;
				}
				RefreshValue();
			}
		}

		private void RefreshValue()
		{
			int value = PlayerData.instance.GetCurrentWeaponLevel().Value;
			God god = PlayerData.instance.god;
			List<SpellData> list = new List<SpellData>();
			foreach (SpellDefinition value2 in RuntimeData.spellDefinitions.Values)
			{
				if (value2.god == god && value2.spellType == SpellType.Normal)
				{
					SpellData spellData = new SpellData(value2, value);
					if (IsSpellValid(spellData))
					{
						list.Add(spellData);
					}
				}
			}
			list.Sort(m_spellComparer);
			m_allSpells = list;
			List<CompanionData> list2 = new List<CompanionData>();
			foreach (CompanionDefinition value3 in RuntimeData.companionDefinitions.Values)
			{
				if (PlayerData.instance.companionInventory.Contains(value3.get_id()))
				{
					CompanionData obj = new CompanionData(value3, value);
					if (IsCompanionValid(obj))
					{
						list2.Add(new CompanionData(value3, value));
					}
				}
			}
			list2.Sort(m_companionDataComparer);
			m_allCompanions = list2;
			if (m_currentMode.HasValue)
			{
				m_spellsList.SetValues(list);
				m_companionsList.SetValues(list2);
			}
		}

		private bool IsCompanionValid(object obj)
		{
			CompanionData companionData = obj as CompanionData;
			if (companionData == null)
			{
				return false;
			}
			IReadOnlyList<Cost> cost = companionData.definition.cost;
			int i = 0;
			for (int count = cost.Count; i < count; i++)
			{
				ElementPointsCost elementPointsCost = cost[i] as ElementPointsCost;
				if (elementPointsCost != null && !m_caracIdsEnableToggle.Contains(elementPointsCost.element))
				{
					return false;
				}
			}
			return ValidateSearchText(companionData, companionData.definition);
		}

		private bool IsSpellValid(object obj)
		{
			SpellData spellData = obj as SpellData;
			if (spellData == null)
			{
				return false;
			}
			bool flag = false;
			bool flag2 = false;
			Element element = spellData.definition.element;
			flag = m_elementsEnableToggle.Contains(element);
			IReadOnlyList<GaugeValue> gaugeToModifyOnSpellPlay = spellData.definition.gaugeToModifyOnSpellPlay;
			int i = 0;
			for (int count = gaugeToModifyOnSpellPlay.Count; i < count; i++)
			{
				GaugeValue gaugeValue = gaugeToModifyOnSpellPlay[i];
				flag2 |= (gaugeValue != null && m_caracIdsEnableToggle.Contains(gaugeValue.element));
			}
			if (flag | flag2)
			{
				return ValidateSearchText(spellData, spellData.definition);
			}
			return false;
		}

		private bool ValidateSearchText<T, D>(T data, D definition) where T : CastableWithLevelData<D> where D : EditableData, IDefinitionWithTooltip
		{
			string textFilter = m_filterParent.GetTextFilter();
			if (textFilter.Length == 0)
			{
				return true;
			}
			if (!RuntimeData.TryGetText(definition.i18nNameId, out string value))
			{
				return true;
			}
			if (StringExtensions.ContainsIgnoreDiacritics(value, textFilter, StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
			if (StringExtensions.ContainsIgnoreDiacritics(RuntimeData.FormattedText(definition.i18nDescriptionId, new FightValueProvider(definition, data.level)).RemoveTags(), textFilter, StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
			return false;
		}

		private void UpdateAllChildren(bool instant)
		{
			m_spellsList.UpdateAllConfigurators(instant);
			m_companionsList.UpdateAllConfigurators(instant);
		}

		public void SetTooltip(FightTooltip tooltip, TooltipPosition tooltipPosition)
		{
			m_tooltip = tooltip;
			m_tooltipPosition = tooltipPosition;
			UpdateAllChildren(instant: true);
		}

		public bool IsWeaponDataAvailable(WeaponData data)
		{
			return true;
		}

		public bool IsCompanionDataAvailable(CompanionData data)
		{
			if (m_slot == null)
			{
				return false;
			}
			if (data == null)
			{
				return false;
			}
			if (m_slot.Companions.Contains(data.definition.get_id()))
			{
				return false;
			}
			if (!ItemDragNDropListener.instance.dragging)
			{
				return true;
			}
			CompanionData companionData = ItemDragNDropListener.instance.DraggedValue as CompanionData;
			return companionData == null || companionData.definition.get_id() != data.definition.get_id();
		}

		public bool IsSpellDataAvailable(SpellData data)
		{
			if (m_slot == null || data == null)
			{
				return false;
			}
			if (m_slot.Spells.Contains(data.definition.get_id()))
			{
				return false;
			}
			if (!ItemDragNDropListener.instance.dragging)
			{
				return true;
			}
			SpellData spellData = ItemDragNDropListener.instance.DraggedValue as SpellData;
			return spellData == null || spellData.definition.get_id() != data.definition.get_id();
		}

		private void OnDragBegin()
		{
			m_onGrabSpell.Invoke();
			if (m_currentMode == EditModeSelection.Spell)
			{
				m_spellsList.UpdateAllConfigurators(instant: true);
			}
			if (m_currentMode == EditModeSelection.Companion)
			{
				m_companionsList.UpdateAllConfigurators(instant: true);
			}
		}

		private void OnDragEnd()
		{
			m_onDropSpell.Invoke();
			if (m_currentMode == EditModeSelection.Spell)
			{
				m_spellsList.UpdateAllConfigurators();
			}
			if (m_currentMode == EditModeSelection.Companion)
			{
				m_companionsList.UpdateAllConfigurators();
			}
		}

		public bool IsValidDrag(object value)
		{
			CompanionData companionData = value as CompanionData;
			if (companionData != null)
			{
				return IsCompanionDataAvailable(companionData);
			}
			SpellData spellData = value as SpellData;
			if (spellData != null)
			{
				return IsSpellDataAvailable(spellData);
			}
			return true;
		}

		public bool IsValidDrop(object value)
		{
			return false;
		}

		public EditModeSelection GetCurrentMode()
		{
			return m_previousValideMode.Value;
		}

		public DeckEditModeUI()
			: this()
		{
		}
	}
}
