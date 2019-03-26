using Ankama.Cube.Data;
using Ankama.Cube.Data.UI.Localization.TextFormatting.ValueProviders.FightValueProviders;

namespace Ankama.Cube.Fight.Entities
{
	public interface ITooltipDataProvider
	{
		TooltipDataType tooltipDataType
		{
			get;
		}

		KeywordReference[] keywordReferences
		{
			get;
		}

		int GetTitleKey();

		int GetDescriptionKey();

		IFightValueProvider GetValueProvider();
	}
}
