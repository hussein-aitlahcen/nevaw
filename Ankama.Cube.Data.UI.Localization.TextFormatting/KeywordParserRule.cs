using Ankama.Cube.Utility;
using System.Text;

namespace Ankama.Cube.Data.UI.Localization.TextFormatting
{
	public class KeywordParserRule : IParserRule
	{
		public bool TryFormat(ref StringReader reader, StringBuilder output, FormatterParams formatterParams)
		{
			if (reader.current == '#')
			{
				reader.position++;
				Format(reader, output, formatterParams);
				return true;
			}
			return false;
		}

		private static void Format(StringReader reader, StringBuilder output, FormatterParams formatterParams)
		{
			SubString subString = reader.ReadWord();
			reader.SkipSpaces();
			string value;
			SubString text = (reader.current == '[') ? reader.ReadContent('[', ']') : (RuntimeData.TryGetText($"KEYWORD.{subString}.NAME", out value) ? ((SubString)value) : subString);
			output.BeginKeyWord();
			formatterParams.formatter.AppendFormat(text, output, formatterParams);
			output.EndKeyWord();
		}
	}
}
