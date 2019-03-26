namespace Ankama.Cube.Data.UI.Localization
{
	public class PluralRulesEN : IPluralRules
	{
		public PluralCategory GetPluralForCardinal(int value)
		{
			if (value == 1)
			{
				return PluralCategory.one;
			}
			return PluralCategory.other;
		}

		public PluralCategory GetPluralForCardinal(float value)
		{
			return PluralCategory.other;
		}

		public PluralCategory GetPluralForOrdinal(int value)
		{
			int num = value % 10;
			int num2 = value % 100;
			if (num == 1 && num2 != 11)
			{
				return PluralCategory.one;
			}
			if (num == 2 && num2 != 12)
			{
				return PluralCategory.two;
			}
			if (num == 3 && num2 != 13)
			{
				return PluralCategory.few;
			}
			return PluralCategory.other;
		}

		public PluralCategory GetPluralForRange(PluralCategory min, PluralCategory max)
		{
			return PluralCategory.other;
		}
	}
}
