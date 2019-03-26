using Ankama.Cube.Data;
using Ankama.Cube.UI.Components;
using Ankama.Cube.UI.DeckMaker;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ankama.Cube.UI.PlayerLayer
{
	public class DeckPresetButton : MonoBehaviour
	{
		private DeckSlot m_deckSlot;

		private Vector2 m_defaultSizeDelta;

		private RectTransform m_rectTransform;

		private Vector2 m_selectedSizeDelta;

		private AnimatedToggleButton m_toggle;

		[Header("Visual")]
		[SerializeField]
		private Image m_bg;

		[SerializeField]
		private Image m_deckIcon;

		[SerializeField]
		private GameObject m_equipedBG;

		[SerializeField]
		private Image m_slectedOutline;

		[SerializeField]
		private RawTextField m_presetName;

		[SerializeField]
		private Image m_invalidFeedback;

		[Header("Button")]
		[SerializeField]
		private AnimatedGraphicButton m_editButton;

		[Header("Visual")]
		[SerializeField]
		private GameObject m_bgDefaultVisual;

		[SerializeField]
		private Image m_btnIcon;

		[SerializeField]
		private Sprite m_btnIconSprite;

		public event Action<DeckSlot> OnSelectRequest;

		public unsafe void Initialise(DeckPresetPanel parent, Sprite sprite, ToggleGroup group, Action<DeckSlot> OnEdit)
		{
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Expected O, but got Unknown
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			m_bgDefaultVisual.SetActive(false);
			m_editButton.get_gameObject().SetActive(false);
			m_slectedOutline.set_enabled(false);
			m_deckIcon.set_sprite(sprite);
			m_toggle = this.GetComponent<AnimatedToggleButton>();
			m_toggle.onValueChanged.AddListener(new UnityAction<bool>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			_003C_003Ec__DisplayClass18_0 _003C_003Ec__DisplayClass18_;
			m_editButton.get_onClick().AddListener(new UnityAction((object)_003C_003Ec__DisplayClass18_, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_rectTransform = this.GetComponent<RectTransform>();
			m_defaultSizeDelta = m_rectTransform.get_sizeDelta();
			m_selectedSizeDelta = m_defaultSizeDelta;
			m_selectedSizeDelta.x *= 1.11f;
		}

		public void SetDefaultVisual()
		{
			m_bgDefaultVisual.SetActive(true);
			m_btnIcon.set_sprite(m_btnIconSprite);
		}

		private void BuildUI(bool equipped)
		{
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			string text = m_deckSlot.isAvailableEmptyDeckSlot ? RuntimeData.FormattedText(50970) : m_deckSlot.Name;
			m_presetName.SetText(text);
			m_presetName.color = new Color(1f, 1f, 1f, m_deckSlot.isAvailableEmptyDeckSlot ? 0.5f : 1f);
			m_deckIcon.set_color(new Color(1f, 1f, 1f, m_deckSlot.isAvailableEmptyDeckSlot ? 0.2f : 1f));
			m_invalidFeedback.get_gameObject().SetActive(m_deckSlot != null && !m_deckSlot.isAvailableEmptyDeckSlot && m_deckSlot.DeckInfo != null && !m_deckSlot.DeckInfo.IsValid());
			m_equipedBG.get_gameObject().SetActive(equipped);
		}

		public void SetEquipped(bool equipped)
		{
			m_equipedBG.get_gameObject().SetActive(equipped);
		}

		public void ForceSelect(bool selected = true)
		{
			m_toggle.set_isOn(selected);
		}

		private void OnValueChanged(bool selected)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			DOTweenModuleUI.DOSizeDelta(m_rectTransform, selected ? m_selectedSizeDelta : m_defaultSizeDelta, 0.1f, false);
			m_editButton.get_gameObject().SetActive(selected);
			m_bg.set_enabled(!selected);
			m_slectedOutline.set_enabled(selected);
			if (selected)
			{
				this.OnSelectRequest?.Invoke(m_deckSlot);
			}
		}

		public void Populate(DeckSlot slot, int equippedDeckId)
		{
			m_deckSlot = slot;
			BuildUI((m_deckSlot.Id ?? 0) == equippedDeckId);
		}

		public DeckSlot GetSlot()
		{
			return m_deckSlot;
		}

		public DeckPresetButton()
			: this()
		{
		}
	}
}
