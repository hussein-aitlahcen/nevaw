namespace Ankama.Cube.Data.UI.Localization
{
	public class PluralRulesRU : IPluralRules
	{
		public PluralCategory GetPluralForCardinal(int value)
		{
			int num = value % 10;
			int num2 = value % 100;
			if (num2 >= 11 && num2 <= 14)
			{
				return PluralCategory.many;
			}
			switch (num)
			{
			case 1:
				return PluralCategory.one;
			case 2:
			case 3:
			case 4:
				return PluralCategory.few;
			default:
				return PluralCategory.many;
			}
		}

		public PluralCategory GetPluralForCardinal(float value)
		{
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
			return max;
		}
	}
}
