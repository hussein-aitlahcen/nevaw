namespace Ankama.Cube.Data.UI.Localization
{
	public class PluralRulesFR : IPluralRules
	{
		public PluralCategory GetPluralForCardinal(int value)
		{
			if (value == 0 || value == 1)
			{
				return PluralCategory.one;
			}
			return PluralCategory.other;
		}

		public PluralCategory GetPluralForCardinal(float value)
		{
			if (value >= 0f && value <= 1f)
			{
				return PluralCategory.one;
			}
			return PluralCategory.other;
		}

		public PluralCategory GetPluralForOrdinal(int value)
		{
			if (value != 1)
			{
				return PluralCategory.other;
			}
			return PluralCategory.one;
		}

		public PluralCategory GetPluralForRange(PluralCategory min, PluralCategory max)
		{
			if (min == PluralCategory.one && max == PluralCategory.one)
			{
				return PluralCategory.one;
			}
			return PluralCategory.other;
		}
	}
}
