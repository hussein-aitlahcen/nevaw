namespace Ankama.Cube.Data.UI.Localization.TextFormatting
{
	public struct FormatterParams
	{
		public readonly TextFormatter formatter;

		public IValueProvider valueProvider;

		public IParserRule[] additionnalRules;

		public KeywordContext context;

		public FormatterParams(TextFormatter formatter, IValueProvider valueProvider)
		{
			this.formatter = formatter;
			this.valueProvider = valueProvider;
			additionnalRules = null;
			context = KeywordContext.FightSolo;
		}
	}
}
