using Ankama.Cube.Data;
using Ankama.Cube.Protocols.PlayerProtocol;
using Ankama.Cube.UI.DeckMaker;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.UI.PlayerLayer
{
	public class DeckPresetPanel : MonoBehaviour
	{
		[Header("Button")]
		[SerializeField]
		private Transform m_editButtonTransform;

		[Header("Presets")]
		[SerializeField]
		private Transform m_customPresetRoot;

		[SerializeField]
		private List<Sprite> m_deckIcons;

		[SerializeField]
		private DeckPresetButton m_defaultRibbon;

		private WeaponDefinition m_definition;

		private List<DeckPresetButton> m_presets;

		private DeckSlot m_selectedSlot;

		private ToggleGroup m_toggleGroup;

		private WeaponAndDeckModifications m_modifications;

		public event Action<DeckSlot> OnSelectionChange;

		public void InitialiseUI(WeaponAndDeckModifications modifications, Action<DeckSlot> OnEditSlot)
		{
			m_modifications = modifications;
			if (m_toggleGroup == null)
			{
				m_toggleGroup = this.get_gameObject().GetComponent<ToggleGroup>();
				if (m_toggleGroup == null)
				{
					m_toggleGroup = this.get_gameObject().AddComponent<ToggleGroup>();
				}
			}
			m_presets = new List<DeckPresetButton>();
			m_presets.Add(m_defaultRibbon);
			m_defaultRibbon.Initialise(this, m_deckIcons[0], m_toggleGroup, OnEditSlot);
			for (int i = 1; i < 4; i++)
			{
				DeckPresetButton deckPresetButton = Object.Instantiate<DeckPresetButton>(m_defaultRibbon, m_customPresetRoot);
				deckPresetButton.OnSelectRequest += OnSelectionChanged;
				m_presets.Add(deckPresetButton);
				deckPresetButton.Initialise(this, m_deckIcons[i], m_toggleGroup, OnEditSlot);
			}
			m_defaultRibbon.SetDefaultVisual();
			m_defaultRibbon.OnSelectRequest += OnSelectionChanged;
			m_editButtonTransform.SetAsLastSibling();
		}

		private void OnSelectionChanged(DeckSlot deckSlot)
		{
			m_selectedSlot = deckSlot;
			this.OnSelectionChange?.Invoke(deckSlot);
		}

		public void OnEquippedDeckUpdate()
		{
			int selectedDeckForWeapon = m_modifications.GetSelectedDeckForWeapon(m_definition.get_id());
			foreach (DeckPresetButton preset in m_presets)
			{
				bool equipped = preset.GetSlot() != null && preset.GetSlot().Id == selectedDeckForWeapon;
				preset.SetEquipped(equipped);
			}
		}

		public void Populate(List<DeckSlot> deckSlots, WeaponDefinition weapon)
		{
			m_definition = weapon;
			int index = 0;
			int selectedDeckForWeapon = m_modifications.GetSelectedDeckForWeapon(weapon.get_id());
			for (int i = 0; i < deckSlots.Count; i++)
			{
				m_presets[i].ForceSelect(selected: false);
				DeckSlot deckSlot = deckSlots[i];
				if (deckSlot.DeckInfo != null && deckSlot.DeckInfo.Id.HasValue && deckSlot.DeckInfo.Id.Value == selectedDeckForWeapon)
				{
					index = i;
				}
				DeckInfo deckInfo = deckSlot.DeckInfo;
				int num;
				bool isAvailableEmptyDeckSlot;
				if (deckInfo == null || !deckInfo.Id.HasValue)
				{
					num = 1;
				}
				else
					isAvailableEmptyDeckSlot = deckSlot.isAvailableEmptyDeckSlot;
				m_presets[i].Populate(deckSlot, selectedDeckForWeapon);
			}
			m_presets[index].ForceSelect();
		}

		public void Unload()
		{
			foreach (DeckPresetButton preset in m_presets)
			{
				preset.OnSelectRequest -= OnSelectionChanged;
			}
		}

		public DeckSlot GetSelectedDeck()
		{
			return m_selectedSlot;
		}

		public DeckPresetPanel()
			: this()
		{
		}
	}
}
