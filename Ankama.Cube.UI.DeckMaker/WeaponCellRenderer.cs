using Ankama.AssetManagement.AssetReferences;
using Ankama.Cube.Data;
using Ankama.Cube.Data.UI.Localization.TextFormatting;
using Ankama.Cube.Data.UI.Localization.TextFormatting.ValueProviders.FightValueProviders;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.UI.Components;
using Ankama.Cube.Utility;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.UI.DeckMaker
{
	public class WeaponCellRenderer : WithTooltipCellRenderer<WeaponData, IWeaponDataCellRendererConfigurator>, ICharacterTooltipDataProvider, ITooltipDataProvider
	{
		[SerializeField]
		private Image m_weaponImage;

		[SerializeField]
		private TextField m_levelTextField;

		public override TooltipDataType tooltipDataType => TooltipDataType.Character;

		public override KeywordReference[] keywordReferences => m_value.definition.precomputedData.keywordReferences;

		protected override void SetValue(WeaponData val)
		{
			if (val != null)
			{
				m_valueProvider = new FightValueProvider(val.definition, val.level);
			}
			SetIllustration(val);
		}

		private void SetIllustration(WeaponData data)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			WeaponDefinition weaponDefinition = data?.definition;
			if (weaponDefinition != null)
			{
				AssetReference weaponIllustrationReference = weaponDefinition.GetWeaponIllustrationReference();
				if (weaponIllustrationReference.get_hasValue())
				{
					Main.monoBehaviour.StartCoroutine(weaponDefinition.LoadIllustrationAsync<Sprite>(AssetBundlesUtility.GetUICharacterResourcesBundleName(), weaponIllustrationReference, (Action<Sprite, string>)SetIllustrationCallback));
				}
			}
			else if (m_weaponImage != null)
			{
				m_weaponImage.set_enabled(false);
			}
		}

		private void SetLevel(int? level)
		{
			if (Object.op_Implicit(m_levelTextField))
			{
				m_levelTextField.get_gameObject().SetActive(level.HasValue);
				m_levelTextField.SetText(68066, new IndexedValueProvider(level.ToString()));
			}
		}

		private void SetIllustrationCallback(Sprite sprite, string loadedBundleName)
		{
			if (null != m_weaponImage)
			{
				m_weaponImage.set_sprite(sprite);
				m_weaponImage.set_enabled(null != sprite);
			}
		}

		protected override void Clear()
		{
			SetIllustration(null);
			SetLevel(null);
		}

		public override void OnConfiguratorUpdate(bool instant)
		{
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			base.OnConfiguratorUpdate(instant);
			float num = (m_configurator?.IsWeaponDataAvailable(m_value) ?? true) ? 1f : 0f;
			Color val = default(Color);
			val._002Ector(1f, num, 1f);
			if (instant)
			{
				m_weaponImage.set_color(val);
			}
			else
			{
				DOTweenModuleUI.DOColor(m_weaponImage, val, 0.2f);
			}
		}

		public override int GetTitleKey()
		{
			return m_value.definition.i18nNameId;
		}

		public override int GetDescriptionKey()
		{
			return m_value.definition.i18nDescriptionId;
		}

		public override IFightValueProvider GetValueProvider()
		{
			return m_valueProvider;
		}

		public ActionType GetActionType()
		{
			return m_value.definition.actionType;
		}

		public TooltipActionIcon GetActionIcon()
		{
			return TooltipWindowUtility.GetActionIcon(m_value.definition);
		}

		public bool TryGetActionValue(out int actionValue)
		{
			actionValue = 0;
			if (m_value == null)
			{
				return true;
			}
			ILevelOnlyDependant actionValue2 = m_value.definition.actionValue;
			if (actionValue2 == null)
			{
				return false;
			}
			actionValue = actionValue2.GetValueWithLevel(m_value.level);
			return true;
		}

		public int GetLifeValue()
		{
			if (m_value == null)
			{
				return 0;
			}
			return m_value.definition.life?.GetValueWithLevel(m_value.level) ?? 0;
		}

		public int GetMovementValue()
		{
			if (m_value == null)
			{
				return 0;
			}
			return m_value.definition.movementPoints?.GetValueWithLevel(m_value.level) ?? 0;
		}
	}
}
