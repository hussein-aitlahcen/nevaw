using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ankama.Cube.Data.UI.Localization.TextFormatting
{
	public class ParserRulesGroupByFirstLetter : IParserRule
	{
		private readonly char[] m_firstLetters;

		private readonly IParserRuleWithPrefix[][] m_rulesByLetters;

		public ParserRulesGroupByFirstLetter(params IParserRuleWithPrefix[] rules)
		{
			Dictionary<char, IGrouping<char, IParserRuleWithPrefix>> dictionary = (from r in rules
				group r by r.prefix[0]).ToDictionary((IGrouping<char, IParserRuleWithPrefix> g) => g.Key);
			int count = dictionary.Count;
			m_firstLetters = new char[count];
			m_rulesByLetters = new IParserRuleWithPrefix[count][];
			int num = 0;
			foreach (KeyValuePair<char, IGrouping<char, IParserRuleWithPrefix>> item in dictionary)
			{
				m_firstLetters[num] = item.Key;
				m_rulesByLetters[num] = item.Value.ToArray();
				num++;
			}
		}

		public bool TryFormat(ref StringReader input, StringBuilder output, FormatterParams formatterParams)
		{
			char current = input.current;
			char[] firstLetters = m_firstLetters;
			int num = firstLetters.Length;
			for (int i = 0; i < num; i++)
			{
				if (firstLetters[i] != current)
				{
					continue;
				}
				IParserRuleWithPrefix[] array = m_rulesByLetters[i];
				int num2 = array.Length;
				for (int j = 0; j < num2; j++)
				{
					StringReader input2 = input;
					if (array[j].TryFormat(ref input2, output, formatterParams))
					{
						input = input2;
						return true;
					}
				}
			}
			return false;
		}
	}
}
