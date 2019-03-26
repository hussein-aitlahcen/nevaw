using Ankama.Cube.Data;
using Ankama.Cube.Protocols.PlayerProtocol;
using Ankama.Cube.TEMPFastEnterMatch.Player;
using Ankama.Cube.UI.Components;
using Ankama.Cube.UI.Components.Tooltip;
using Ankama.Cube.UI.Fight.Windows;
using Ankama.Utilities;
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Ankama.Cube.UI.DeckMaker
{
	public class DeckUI : AbstractUI, IDeckDisplayConfigurator, IWithTooltipCellRendererConfigurator, ICellRendererConfigurator
	{
		[SerializeField]
		private GameObject m_inputBlocker;

		[SerializeField]
		private DeckEditModeUI m_editModeUI;

		[SerializeField]
		private DeckDisplayer m_deck;

		[SerializeField]
		private GenericTooltipWindow m_genericTooltip;

		[SerializeField]
		private FightTooltip m_fightTooltip;

		[SerializeField]
		private TooltipPosition m_tooltipPosition;

		[SerializeField]
		private CanvasGroup m_mainCanvasGroup;

		[Header("Sounds")]
		[SerializeField]
		private UnityEvent m_openCanvas;

		[SerializeField]
		private UnityEvent m_closeCanvas;

		private DeckSlot m_selectedSlot;

		private DeckBuildingEventController m_eventController;

		private bool m_inEdition;

		private bool m_isOpen;

		public DeckBuildingEventController eventController
		{
			get
			{
				return m_eventController;
			}
			set
			{
				m_eventController = value;
				m_deck.eventController = m_eventController;
			}
		}

		public FightTooltip tooltip => m_fightTooltip;

		public TooltipPosition tooltipPosition => m_tooltipPosition;

		protected override void Awake()
		{
			base.Awake();
			m_inputBlocker.SetActive(false);
			m_deck.get_gameObject().SetActive(false);
			m_deck.SetConfigurator(this, andUpdate: false);
			m_deck.OnEditModeSelectionChanged += OnEditModeSelectionChanged;
			m_editModeUI.SetTooltip(m_fightTooltip, m_tooltipPosition);
			m_mainCanvasGroup.set_alpha(0f);
			m_mainCanvasGroup.get_gameObject().SetActive(false);
		}

		public void SetValue(DeckSlot selectedValue)
		{
			m_selectedSlot = selectedValue;
			m_deck.SetValue(selectedValue);
		}

		public DeckSlot GetSelectedSlot()
		{
			return m_selectedSlot;
		}

		public void RemoveCurrent(int weapon)
		{
			DeckInfo deckInfo = new DeckInfo().FillEmptySlotsCopy();
			deckInfo.Name = RuntimeData.FormattedText(92537);
			deckInfo.God = (int)PlayerData.instance.god;
			deckInfo.Weapon = weapon;
			DeckSlot value = m_selectedSlot = new DeckSlot(deckInfo);
			m_deck.SetValue(value);
			m_eventController.OnDeckSlotSelectionChange(m_selectedSlot);
		}

		public IEnumerator GotoEdit(EditModeSelection selection)
		{
			m_inEdition = true;
			Sequence val = DOTween.Sequence();
			if (!(m_deck == null))
			{
				m_deck.get_gameObject().SetActive(true);
				m_mainCanvasGroup.get_gameObject().SetActive(true);
				TweenSettingsExtensions.Insert(val, 0f, DOTweenModuleUI.DOFade(m_mainCanvasGroup, 1f, 0.2f));
				TweenSettingsExtensions.Insert(val, 0f, m_deck.EnterEditMode(selection));
				TweenSettingsExtensions.Insert(val, 0f, m_editModeUI.Display(selection, m_selectedSlot));
				m_openCanvas.Invoke();
				m_isOpen = true;
				yield return TweenExtensions.WaitForKill(val);
			}
		}

		public unsafe IEnumerator GotoSelectMode()
		{
			if (m_inEdition)
			{
				m_inEdition = false;
				Sequence val = DOTween.Sequence();
				TweenSettingsExtensions.Insert(val, 0f, DOTweenModuleUI.DOFade(m_mainCanvasGroup, 0f, 0.2f));
				TweenSettingsExtensions.Insert(val, 0f, m_deck.LeaveEditMode());
				TweenSettingsExtensions.Insert(val, 0f, m_editModeUI.Hide());
				m_closeCanvas.Invoke();
				m_inputBlocker.SetActive(false);
				TweenSettingsExtensions.InsertCallback(val, TweenExtensions.Duration(val, true), new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
				m_isOpen = false;
				m_fightTooltip.Hide();
				yield return TweenExtensions.WaitForKill(val);
			}
		}

		private void OnGotoSelectModeEnd()
		{
			m_deck.get_gameObject().SetActive(false);
			m_mainCanvasGroup.get_gameObject().SetActive(false);
		}

		private void OnEditModeSelectionChanged(EditModeSelection obj)
		{
			m_editModeUI.SetEditModeSelection(obj);
		}

		public DeckSlot OnCloneCanceled()
		{
			DeckSlot deckSlot = m_deck.GetPreviousDeck().Clone();
			this.StartCoroutine(CloneCanceledAnimation(deckSlot));
			return deckSlot;
		}

		private IEnumerator CloneCanceledAnimation(DeckSlot previousSLot)
		{
			SetValue(previousSLot);
			yield return null;
			m_editModeUI.RefreshList(previousSLot);
		}

		public void OnCloneValidate(DeckSlot newSlot)
		{
			this.StartCoroutine(CloneAnimation(newSlot));
		}

		private IEnumerator CloneAnimation(DeckSlot newSlot)
		{
			Sequence sequence2 = DOTween.Sequence();
			TweenSettingsExtensions.Insert(sequence2, 0f, DOTweenModuleUI.DOFade(m_mainCanvasGroup, 0f, 0.2f));
			TweenSettingsExtensions.Insert(sequence2, 0f, m_deck.LeaveEditMode());
			TweenSettingsExtensions.Insert(sequence2, 0f, m_editModeUI.Hide());
			yield return TweenExtensions.Play<Sequence>(sequence2);
			while (TweenExtensions.IsPlaying(sequence2))
			{
				yield return null;
			}
			yield return (object)new WaitForTime(0.1f);
			m_selectedSlot = newSlot;
			m_deck.SetValue(m_selectedSlot);
			sequence2 = DOTween.Sequence();
			TweenSettingsExtensions.Append(sequence2, DOTweenModuleUI.DOFade(m_mainCanvasGroup, 1f, 0.2f));
			TweenSettingsExtensions.Append(sequence2, m_deck.EnterEditMode(m_editModeUI.GetCurrentMode()));
			TweenSettingsExtensions.Append(sequence2, m_editModeUI.Display(m_editModeUI.GetCurrentMode(), m_selectedSlot));
			TweenExtensions.Play<Sequence>(sequence2);
			m_deck.OnCloneValidate();
		}

		private void OnDeckSlotSelectionChangedRequest(DeckSlot obj)
		{
			eventController?.OnDeckSlotSelectionChange(obj);
		}

		private void OnCancel()
		{
			m_eventController?.OnCancel();
		}

		public bool IsOpen()
		{
			return m_isOpen;
		}
	}
}
