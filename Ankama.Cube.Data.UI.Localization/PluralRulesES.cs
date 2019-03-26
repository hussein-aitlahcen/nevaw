namespace Ankama.Cube.Data.UI.Localization
{
	public class PluralRulesES : IPluralRules
	{
		public PluralCategory GetPluralForCardinal(int value)
		{
			if (value != 1)
			{
				return PluralCategory.other;
			}
			return PluralCategory.one;
		}

		public PluralCategory GetPluralForCardinal(float value)
		{
			if (value != 1f)
			{
				return PluralCategory.other;
			}
			return PluralCategory.one;
		}

		public PluralCategory GetPluralForOrdinal(int value)
		{
			return PluralCategory.other;
		}

		public PluralCategory GetPluralForRange(PluralCategory min, PluralCategory max)
		{
			return PluralCategory.other;
		}
	}
}
