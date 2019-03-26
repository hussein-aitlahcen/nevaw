using Ankama.AssetManagement.AssetReferences;
using Ankama.Cube.Data;
using Ankama.Cube.Data.UI.Localization.TextFormatting;
using Ankama.Cube.Data.UI.Localization.TextFormatting.ValueProviders.FightValueProviders;
using Ankama.Cube.Fight.Entities;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Fight.History
{
	public class HistoryReserveElement : HistoryAbstractElement, ITextTooltipDataProvider, ITooltipDataProvider
	{
		[SerializeField]
		private UISpriteTextRenderer m_valueText;

		private ReserveDefinition m_reserveDefinition;

		private FightValueProvider m_tooltipValueProvider;

		public override HistoryElementType type => HistoryElementType.Reserve;

		public override ITooltipDataProvider tooltipProvider => this;

		public TooltipDataType tooltipDataType => TooltipDataType.Text;

		public KeywordReference[] keywordReferences
		{
			get
			{
				if (!(null == m_reserveDefinition))
				{
					return m_reserveDefinition.precomputedData.keywordReferences;
				}
				return null;
			}
		}

		public void Set(PlayerStatus status, int valueBefore)
		{
			m_reserveDefinition = RuntimeDataHelper.GetReserveDefinitionFrom(status);
			HeroStatus heroStatus = status.heroStatus;
			m_tooltipValueProvider = new FightValueProvider(m_reserveDefinition, heroStatus.level);
			m_valueText.text = valueBefore.ToString();
			ApplyIllu(status.isLocalPlayer);
		}

		protected override bool HasIllu()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			if (null != m_reserveDefinition)
			{
				AssetReference illustrationReference = m_reserveDefinition.illustrationReference;
				return illustrationReference.get_hasValue();
			}
			return false;
		}

		protected override IEnumerator LoadIllu(Action<Sprite, string> loadEndCallback)
		{
			ReserveDefinition reserveDefinition = m_reserveDefinition;
			if (!(null == reserveDefinition))
			{
				yield return reserveDefinition.LoadIllustrationAsync<Sprite>(reserveDefinition.illustrationBundleName, reserveDefinition.illustrationReference, loadEndCallback);
			}
		}

		public int GetTitleKey()
		{
			if (!(null == m_reserveDefinition))
			{
				return m_reserveDefinition.i18nNameId;
			}
			return 0;
		}

		public int GetDescriptionKey()
		{
			if (!(null == m_reserveDefinition))
			{
				return m_reserveDefinition.i18nDescriptionId;
			}
			return 0;
		}

		public IFightValueProvider GetValueProvider()
		{
			return m_tooltipValueProvider;
		}
	}
}
