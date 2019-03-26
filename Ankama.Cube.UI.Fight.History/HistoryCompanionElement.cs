using Ankama.AssetManagement.AssetReferences;
using Ankama.Cube.Data;
using Ankama.Cube.Data.UI.Localization.TextFormatting;
using Ankama.Cube.Data.UI.Localization.TextFormatting.ValueProviders.FightValueProviders;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.UI.Components;
using System;
using System.Collections;
using UnityEngine;

namespace Ankama.Cube.UI.Fight.History
{
	public class HistoryCompanionElement : HistoryAbstractElement, ICharacterTooltipDataProvider, ITooltipDataProvider
	{
		private CompanionDefinition m_definition;

		private FightValueProvider m_tooltipValueProvider;

		private int m_movementPoints;

		private int m_life;

		private int? m_actionValue;

		public override HistoryElementType type => HistoryElementType.Companion;

		public override ITooltipDataProvider tooltipProvider => this;

		public TooltipDataType tooltipDataType => TooltipDataType.Character;

		public KeywordReference[] keywordReferences => m_definition.precomputedData.keywordReferences;

		public void Set(ReserveCompanionStatus companion)
		{
			ReserveCompanionValueContext reserveCompanionValueContext = companion.CreateValueContext();
			m_definition = companion.definition;
			m_tooltipValueProvider = new FightValueProvider(companion);
			m_life = m_definition.life.GetValueWithLevel(reserveCompanionValueContext.level);
			m_movementPoints = m_definition.movementPoints.GetValueWithLevel(reserveCompanionValueContext.level);
			m_actionValue = ExtractActionValue(m_definition, reserveCompanionValueContext);
			ApplyIllu(companion.currentPlayer.isLocalPlayer);
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
			CompanionDefinition definition = m_definition;
			yield return definition.LoadIllustrationAsync<Sprite>(definition.illustrationBundleName, definition.illustrationReference, loadEndCallback);
		}

		public ActionType GetActionType()
		{
			return m_definition.actionType;
		}

		public TooltipActionIcon GetActionIcon()
		{
			return TooltipWindowUtility.GetActionIcon(m_definition);
		}

		public bool TryGetActionValue(out int value)
		{
			if (m_actionValue.HasValue)
			{
				value = m_actionValue.Value;
				return true;
			}
			value = 0;
			return false;
		}

		public int GetLifeValue()
		{
			return m_life;
		}

		public int GetMovementValue()
		{
			return m_movementPoints;
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

		private static int? ExtractActionValue(CompanionDefinition definition, DynamicValueContext context)
		{
			return definition.actionValue?.GetValueWithLevel(context.level);
		}
	}
}
