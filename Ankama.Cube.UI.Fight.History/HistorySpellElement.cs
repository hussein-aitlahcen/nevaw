using Ankama.AssetManagement.AssetReferences;
using Ankama.Cube.Data;
using Ankama.Cube.Data.UI.Localization.TextFormatting;
using Ankama.Cube.Data.UI.Localization.TextFormatting.ValueProviders.FightValueProviders;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.UI.Components;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Fight.History
{
	public class HistorySpellElement : HistoryAbstractElement, ISpellTooltipDataProvider, ITooltipDataProvider
	{
		[SerializeField]
		private UISpriteTextRenderer m_costText;

		private SpellDefinition m_definition;

		private FightValueProvider m_tooltipValueProvider;

		private TooltipElementValues m_tooltipElementValues;

		public override HistoryElementType type => HistoryElementType.Spell;

		public override ITooltipDataProvider tooltipProvider => this;

		public TooltipDataType tooltipDataType => TooltipDataType.Spell;

		public KeywordReference[] keywordReferences => m_definition.precomputedData.keywordReferences;

		public void Set(SpellStatus status, DynamicValueContext context, int cost)
		{
			m_definition = status.definition;
			m_tooltipValueProvider = new FightValueProvider(status);
			m_tooltipElementValues = TooltipWindowUtility.GetTooltipElementValues(m_definition, context);
			m_costText.text = cost.ToString();
			ApplyIllu(status.ownerPlayer.isLocalPlayer);
		}

		protected override bool HasIllu()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			if (m_definition != null)
			{
				AssetReference illustrationReference = m_definition.illustrationReference;
				return illustrationReference.get_hasValue();
			}
			return false;
		}

		protected override IEnumerator LoadIllu(Action<Sprite, string> loadEndCallback)
		{
			SpellDefinition definition = m_definition;
			yield return definition.LoadIllustrationAsync<Sprite>(definition.illustrationBundleName, definition.illustrationReference, loadEndCallback);
		}

		public TooltipElementValues GetGaugeModifications()
		{
			return m_tooltipElementValues;
		}

		public int GetTitleKey()
		{
			return m_definition.i18nNameId;
		}

		public int GetDescriptionKey()
		{
			return m_definition.i18nDescriptionId;
		}

		public IFightValueProvider GetValueProvider()
		{
			return m_tooltipValueProvider;
		}
	}
}
