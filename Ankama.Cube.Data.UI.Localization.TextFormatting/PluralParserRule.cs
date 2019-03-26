using Ankama.Cube.Utility;
using System.Text;

namespace Ankama.Cube.Data.UI.Localization.TextFormatting
{
	public class PluralParserRule : IParserRule
	{
		private class ReplaceNumber : IParserRule
		{
			public readonly string value;

			public ReplaceNumber(string value)
			{
				this.value = value;
			}

			public bool TryFormat(ref StringReader input, StringBuilder output, FormatterParams formatterParams)
			{
				if (input.current == '#')
				{
					output.Append(value);
					input.position++;
					return true;
				}
				return false;
			}
		}

		private static readonly string[] PluralFormStrings = new string[6]
		{
			"zero",
			"one",
			"two",
			"few",
			"many",
			"other"
		};

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
				if (reader.Read("plural"))
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
			if (string.IsNullOrEmpty(value))
			{
				return;
			}
			PluralCategory pluralForOrdinal = formatterParams.formatter.pluralRules.GetPluralForOrdinal(int.Parse(value));
			int position = reader.position;
			while (reader.hasNext)
			{
				reader.SkipSpaces();
				bool num = reader.Read(PluralFormStrings[(int)pluralForOrdinal]);
				if (!num)
				{
					reader.ReadUntil('[');
				}
				SubString text = reader.ReadContent('[', ']');
				if (num)
				{
					Append(text, output, formatterParams, value);
					return;
				}
			}
			reader.position = position;
			while (reader.hasNext)
			{
				reader.SkipSpaces();
				bool num2 = reader.Read("other");
				if (!num2)
				{
					reader.ReadUntil('[');
				}
				SubString text2 = reader.ReadContent('[', ']');
				if (num2)
				{
					Append(text2, output, formatterParams, value);
					return;
				}
			}
			throw new TextFormatterException(reader.text, reader.position, "'other' content is not defined");
		}

		private static void Append(SubString text, StringBuilder output, FormatterParams formatterParams, string value)
		{
			text.Trim();
			formatterParams.additionnalRules = new IParserRule[1]
			{
				new ReplaceNumber(value)
			};
			formatterParams.formatter.AppendFormat(text, output, formatterParams);
		}
	}
}
