using Ankama.Cube.Utility;
using System.Text;

namespace Ankama.Cube.Data.UI.Localization.TextFormatting
{
	public class SelectParserRule : IParserRule
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
			if (reader.Read(','))
			{
				reader.SkipSpaces();
				if (reader.Read("select"))
				{
					reader.SkipSpaces();
					if (reader.Read(','))
					{
						string value = formatterParams.valueProvider.GetValue(subString.ToString());
						FormatSelect(value, reader, output, formatterParams);
						return true;
					}
				}
			}
			return false;
		}

		private void FormatSelect(string value, StringReader reader, StringBuilder output, FormatterParams formatterParams)
		{
			bool num;
			SubString text;
			do
			{
				if (reader.hasNext)
				{
					reader.SkipSpaces();
					num = reader.Read(value);
					text = reader.ReadContent('[', ']');
					continue;
				}
				return;
			}
			while (!num);
			text.Trim();
			formatterParams.formatter.AppendFormat(text, output, formatterParams);
		}
	}
}
