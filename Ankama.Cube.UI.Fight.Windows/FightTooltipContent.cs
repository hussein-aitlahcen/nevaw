using Ankama.Cube.Data;
using Ankama.Cube.Data.UI.Localization.TextFormatting;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.UI.Components;
using Ankama.Utilities;
using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Fight.Windows
{
	[Serializable]
	public struct FightTooltipContent
	{
		[Serializable]
		private struct PropertyIcon
		{
			[SerializeField]
			private GameObject m_imageContainer;

			[SerializeField]
			private GameObject m_textContainer;

			[SerializeField]
			private Image m_image;

			[SerializeField]
			private UISpriteTextRenderer m_spriteTextRenderer;

			public void SetActive(bool value)
			{
				m_imageContainer.SetActive(value);
				m_textContainer.SetActive(value);
			}

			public void SetValue(int value)
			{
				m_spriteTextRenderer.text = value.ToString();
			}

			public void SetModificationValue(int value)
			{
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_004e: Unknown result type (might be due to invalid IL or missing references)
				if (value == 0)
				{
					m_image.set_color(new Color(0.5f, 0.5f, 0.5f, 1f));
					m_spriteTextRenderer.set_enabled(false);
				}
				else
				{
					m_image.set_color(new Color(1f, 1f, 1f, 1f));
					m_spriteTextRenderer.text = ToStringExtensions.ToStringSigned(value);
					m_spriteTextRenderer.set_enabled(true);
				}
			}
		}

		[SerializeField]
		private GameObject m_iconsContainer;

		[Header("Texts")]
		[SerializeField]
		private TextField m_title;

		[SerializeField]
		private TextField m_description;

		[Header("Spell Elements")]
		[SerializeField]
		private PropertyIcon m_air;

		[SerializeField]
		private PropertyIcon m_earth;

		[SerializeField]
		private PropertyIcon m_fire;

		[SerializeField]
		private PropertyIcon m_water;

		[SerializeField]
		private PropertyIcon m_reserve;

		[Header("Character Properties")]
		[SerializeField]
		private PropertyIcon m_action;

		[SerializeField]
		private PropertyIcon m_life;

		[SerializeField]
		private PropertyIcon m_movement;

		[Header("Mechanism Properties")]
		[SerializeField]
		private PropertyIcon m_armor;

		[Header("Action Icons")]
		[SerializeField]
		private GameObject m_closeCombatAttackIcon;

		[SerializeField]
		private GameObject m_rangedAttackIcon;

		[SerializeField]
		private GameObject m_healIcon;

		public void Setup()
		{
			m_title.SetText(0);
			m_description.SetText(0);
			SetCharacterPropertiesVisibility(value: false);
			SetSpellElementsVisibility(value: false);
		}

		public void Initialize([NotNull] ITooltipDataProvider dataProvider)
		{
			IValueProvider valueProvider = dataProvider.GetValueProvider();
			m_title.SetText(dataProvider.GetTitleKey(), valueProvider);
			m_description.SetText(dataProvider.GetDescriptionKey(), valueProvider);
			switch (dataProvider.tooltipDataType)
			{
			case TooltipDataType.Character:
				SetCharacterPropertiesVisibility(value: true);
				SetMechanismPropertiesVisibility(value: false);
				SetSpellElementsVisibility(value: false);
				SetIconsVisibility(value: true);
				InitializeProperties((ICharacterTooltipDataProvider)dataProvider);
				break;
			case TooltipDataType.ObjectMechanism:
				SetCharacterPropertiesVisibility(value: false);
				SetMechanismPropertiesVisibility(value: true);
				SetSpellElementsVisibility(value: false);
				SetIconsVisibility(value: true);
				InitializeProperties((IObjectMechanismTooltipDataProvider)dataProvider);
				break;
			case TooltipDataType.FloorMechanism:
				SetCharacterPropertiesVisibility(value: false);
				SetMechanismPropertiesVisibility(value: false);
				SetSpellElementsVisibility(value: false);
				SetIconsVisibility(value: false);
				break;
			case TooltipDataType.Spell:
				SetCharacterPropertiesVisibility(value: false);
				SetMechanismPropertiesVisibility(value: false);
				SetSpellElementsVisibility(value: true);
				SetIconsVisibility(value: true);
				InitializeProperties((ISpellTooltipDataProvider)dataProvider);
				break;
			case TooltipDataType.Text:
				SetCharacterPropertiesVisibility(value: false);
				SetMechanismPropertiesVisibility(value: false);
				SetSpellElementsVisibility(value: false);
				SetIconsVisibility(value: false);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		private void InitializeProperties(ICharacterTooltipDataProvider dataProvider)
		{
			TooltipActionIcon actionIcon = dataProvider.GetActionIcon();
			m_closeCombatAttackIcon.SetActive(actionIcon == TooltipActionIcon.AttackCloseCombat);
			m_rangedAttackIcon.SetActive(actionIcon == TooltipActionIcon.AttackRanged);
			m_healIcon.SetActive(actionIcon == TooltipActionIcon.Heal);
			switch (dataProvider.GetActionType())
			{
			case ActionType.Attack:
				if (dataProvider.TryGetActionValue(out int value2))
				{
					m_action.SetValue(value2);
				}
				break;
			case ActionType.Heal:
				if (dataProvider.TryGetActionValue(out int value))
				{
					m_action.SetValue(value);
				}
				break;
			case ActionType.None:
				m_action.SetActive(value: false);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			m_life.SetValue(dataProvider.GetLifeValue());
			m_movement.SetValue(dataProvider.GetMovementValue());
		}

		private void InitializeProperties(IObjectMechanismTooltipDataProvider dataProvider)
		{
			m_armor.SetValue(dataProvider.GetArmorValue());
		}

		private void InitializeProperties(ISpellTooltipDataProvider dataProvider)
		{
			TooltipElementValues gaugeModifications = dataProvider.GetGaugeModifications();
			m_air.SetModificationValue(gaugeModifications.air);
			m_earth.SetModificationValue(gaugeModifications.earth);
			m_fire.SetModificationValue(gaugeModifications.fire);
			m_water.SetModificationValue(gaugeModifications.water);
			m_reserve.SetModificationValue(gaugeModifications.reserve);
		}

		private void SetSpellElementsVisibility(bool value)
		{
			m_air.SetActive(value);
			m_earth.SetActive(value);
			m_fire.SetActive(value);
			m_water.SetActive(value);
			m_reserve.SetActive(value);
		}

		private void SetCharacterPropertiesVisibility(bool value)
		{
			m_action.SetActive(value);
			m_life.SetActive(value);
			m_movement.SetActive(value);
		}

		private void SetMechanismPropertiesVisibility(bool value)
		{
			m_armor.SetActive(value);
		}

		private void SetIconsVisibility(bool value)
		{
			m_iconsContainer.SetActive(value);
		}
	}
}
