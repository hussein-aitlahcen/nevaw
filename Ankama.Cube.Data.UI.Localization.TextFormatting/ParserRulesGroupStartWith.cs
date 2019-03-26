namespace Ankama.Cube.Data.UI.Localization.TextFormatting
{
	public class ParserRulesGroupStartWith : AbstractParserRulesGroup, IParserRuleWithPrefix, IParserRule
	{
		private readonly string starts;

		public string prefix => starts;

		public ParserRulesGroupStartWith(string starts, params IParserRule[] rules)
			: base(rules)
		{
			this.starts = starts;
		}

		protected override bool MatchGroup(StringReader input)
		{
			return input.NextEquals(starts);
		}
	}
}
