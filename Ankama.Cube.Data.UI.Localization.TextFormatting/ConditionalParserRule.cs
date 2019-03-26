using Ankama.Cube.Utility;
using System.Text;

namespace Ankama.Cube.Data.UI.Localization.TextFormatting
{
	public class ConditionalParserRule : IParserRuleWithPrefix, IParserRule
	{
		public const string transfer = "transfer";

		public const string deckbuild = "deckbuild";

		public string prefix => "<if";

		public bool TryFormat(ref StringReader input, StringBuilder output, FormatterParams formatterParams)
		{
			SubString tagAttributes = new SubString(input.text, input.position, 0);
			SubString content = new SubString(input.text, input.position, 0);
			if (!input.ReadTag("if", ref tagAttributes, ref content))
			{
				return false;
			}
			if (GetCondition(tagAttributes).IsValidFor(formatterParams.context))
			{
				formatterParams.formatter.AppendFormat(content, output, formatterParams);
			}
			return true;
		}

		private static KeywordCondition GetCondition(SubString attributes)
		{
			KeywordCondition keywordCondition = (KeywordCondition)0;
			attributes.Trim();
			StringReader stringReader = new StringReader(attributes);
			while (stringReader.hasNext)
			{
				if (stringReader.NextEquals("transfer"))
				{
					stringReader.position += "transfer".Length;
					keywordCondition |= KeywordCondition.Transfer;
				}
				else if (stringReader.NextEquals("deckbuild"))
				{
					stringReader.position += "deckbuild".Length;
					keywordCondition |= KeywordCondition.EditDeck;
				}
				else
				{
					stringReader.position++;
				}
			}
			return keywordCondition;
		}
	}
}
