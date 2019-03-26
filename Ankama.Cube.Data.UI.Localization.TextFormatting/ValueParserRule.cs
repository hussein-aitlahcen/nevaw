using Ankama.Cube.Utility;
using System.Text;

namespace Ankama.Cube.Data.UI.Localization.TextFormatting
{
	public class ValueParserRule : IParserRule
	{
		public bool TryFormat(ref StringReader reader, StringBuilder output, FormatterParams formatterParams)
		{
			reader.SkipSpaces();
			SubString subString = reader.ReadWord();
			if (subString.length == 0)
			{
				return false;
			}
			reader.SkipSpaces();
			if (reader.hasNext)
			{
				return false;
			}
			string value = formatterParams.valueProvider.GetValue(subString.ToString());
			output.Append(value);
			return true;
		}
	}
}
