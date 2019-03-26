namespace Ankama.Cube.Data.UI.Localization
{
	public interface IPluralRules
	{
		PluralCategory GetPluralForCardinal(int value);

		PluralCategory GetPluralForCardinal(float value);

		PluralCategory GetPluralForOrdinal(int value);

		PluralCategory GetPluralForRange(PluralCategory min, PluralCategory max);
	}
}
