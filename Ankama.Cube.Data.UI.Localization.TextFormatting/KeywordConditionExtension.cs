namespace Ankama.Cube.Data.UI.Localization.TextFormatting
{
	public static class KeywordConditionExtension
	{
		public static bool IsValidFor(this KeywordCondition c, KeywordContext context)
		{
			return ((int)context & (int)c) == (int)c;
		}
	}
}
