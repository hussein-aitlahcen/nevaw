using System.Collections.Generic;
using System.Text;

namespace Ankama.Cube.Data.UI.Localization.TextFormatting
{
	public abstract class AbstractParserRulesGroup : IParserRule
	{
		private readonly List<IParserRule> m_rules;

		protected AbstractParserRulesGroup(params IParserRule[] rules)
		{
			m_rules = new List<IParserRule>(rules);
		}

		public bool TryFormat(ref StringReader input, StringBuilder output, FormatterParams formatterParams)
		{
			if (!MatchGroup(input))
			{
				return false;
			}
			List<IParserRule> rules = m_rules;
			int count = rules.Count;
			for (int i = 0; i < count; i++)
			{
				if (rules[i].TryFormat(ref input, output, formatterParams))
				{
					return true;
				}
			}
			return false;
		}

		protected abstract bool MatchGroup(StringReader input);
	}
}
