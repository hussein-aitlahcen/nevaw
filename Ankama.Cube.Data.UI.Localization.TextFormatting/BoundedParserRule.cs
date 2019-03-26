using Ankama.Cube.Utility;
using System.Collections.Generic;
using System.Text;

namespace Ankama.Cube.Data.UI.Localization.TextFormatting
{
	public class BoundedParserRule : IParserRule
	{
		private readonly List<IParserRule> m_rules;

		public readonly char open;

		public readonly char close;

		public BoundedParserRule(char open = '{', char close = '}', params IParserRule[] rules)
		{
			this.open = open;
			this.close = close;
			m_rules = new List<IParserRule>(rules);
		}

		public bool TryFormat(ref StringReader input, StringBuilder output, FormatterParams formatterParams)
		{
			if (input.current != open)
			{
				return false;
			}
			StringReader stringReader = input.Copy();
			SubString subString = stringReader.ReadContent(open, close);
			subString.Trim();
			int position = stringReader.position;
			List<IParserRule> rules = m_rules;
			int count = rules.Count;
			for (int i = 0; i < count; i++)
			{
				StringReader input2 = new StringReader(input.text);
				input2.position = subString.startIndex;
				input2.SetLimit(subString.endIndex);
				if (rules[i].TryFormat(ref input2, output, formatterParams))
				{
					input.position = position;
					return true;
				}
			}
			return false;
		}
	}
}
