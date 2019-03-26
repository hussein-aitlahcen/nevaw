using Ankama.AssetManagement.AssetReferences;
using Ankama.Cube.Data;
using Ankama.Cube.UI.Components;
using Ankama.Cube.Utility;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ankama.Cube.UI.DeckMaker
{
	public class DeckSlotMiniature : CellRenderer<DeckSlot, IDeckSlotCellRendererConfigurator>
	{
		private WeaponDefinition m_definition;

		private bool m_isOn = true;

		private DeckSlot m_previousValue;

		[SerializeField]
		private Image m_illustration;

		[SerializeField]
		private Button m_button;

		[SerializeField]
		private Image m_invalidDeck;

		[SerializeField]
		private Sprite m_emptySprite;

		private unsafe void Awake()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			m_button.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		}

		protected override void SetValue(DeckSlot deckSlot)
		{
			if (m_previousValue != null)
			{
				m_previousValue.OnModification -= OnModification;
			}
			if (deckSlot != null)
			{
				deckSlot.OnModification += OnModification;
			}
			m_previousValue = deckSlot;
		}

		private void SetValueInternal(DeckSlot slot)
		{
			WeaponDefinition definition = m_definition;
			GetWeaponDefinition(slot, out WeaponDefinition definition2);
			m_definition = definition2;
			if (definition != m_definition)
			{
				UpdateIllustration();
			}
			UpdateInvalidDeck();
		}

		private void UpdateInvalidDeck()
		{
			DeckSlot value = m_value;
			if (value == null || value.isAvailableEmptyDeckSlot)
			{
				m_invalidDeck.get_gameObject().SetActive(false);
			}
			else
			{
				m_invalidDeck.get_gameObject().SetActive(!value.DeckInfo.IsValid());
			}
		}

		private void GetWeaponDefinition(DeckSlot slot, out WeaponDefinition definition)
		{
			definition = null;
			if (slot != null && slot.Id.HasValue)
			{
				int? weapon = slot.Weapon;
				if (weapon.HasValue)
				{
					RuntimeData.weaponDefinitions.TryGetValue(weapon.Value, out definition);
				}
			}
		}

		private void UpdateIllustration()
		{
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			m_illustration.set_enabled(false);
			if (m_definition == null)
			{
				m_illustration.set_sprite(m_emptySprite);
				m_illustration.set_enabled(true);
				return;
			}
			AssetReference illustrationReference = m_definition.GetIllustrationReference();
			if (illustrationReference.get_hasValue())
			{
				Main.monoBehaviour.StartCoroutine(m_definition.LoadIllustrationAsync<Sprite>(AssetBundlesUtility.GetUICharacterResourcesBundleName(), illustrationReference, (Action<Sprite, string>)UpdateIllustrationCallback));
			}
		}

		private void UpdateIllustrationCallback(Sprite sprite, string loadedBundleName)
		{
			if (null != m_illustration)
			{
				m_illustration.set_sprite(sprite);
				m_illustration.set_enabled(null != sprite);
			}
		}

		protected override void Clear()
		{
			SetValue(null);
		}

		private void OnModification(DeckSlot obj)
		{
			SetValueInternal(obj);
		}

		private void SetIsOn(bool isOn, bool instant)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			if (m_isOn != isOn)
			{
				m_isOn = isOn;
				float num = isOn ? 1.2f : 1f;
				Color val = isOn ? Color.get_white() : Color.get_gray();
				if (instant)
				{
					m_illustration.get_transform().set_localScale(new Vector3(num, num, 1f));
					m_illustration.set_color(val);
					m_invalidDeck.get_transform().set_localScale(new Vector3(num, num, 1f));
					m_invalidDeck.set_color(val);
				}
				else
				{
					ShortcutExtensions.DOScale(m_illustration.get_transform(), num, 0.15f);
					DOTweenModuleUI.DOColor(m_illustration, val, 0.15f);
					ShortcutExtensions.DOScale(m_invalidDeck.get_transform(), num, 0.15f);
					DOTweenModuleUI.DOColor(m_invalidDeck, val, 0.15f);
				}
			}
		}

		private void OnButtonClicked()
		{
			m_configurator?.OnClicked(m_value);
		}

		public override void OnConfiguratorUpdate(bool instant)
		{
			DeckSlot value = m_value;
			bool isOn = (m_configurator?.currentSlot)?.SlotId == value?.SlotId;
			SetIsOn(isOn, instant);
			SetValueInternal(value);
		}
	}
}
