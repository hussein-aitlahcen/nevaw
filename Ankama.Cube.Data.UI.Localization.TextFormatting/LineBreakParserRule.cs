using System.Text;

namespace Ankama.Cube.Data.UI.Localization.TextFormatting
{
	public class LineBreakParserRule : IParserRule
	{
		public bool TryFormat(ref StringReader input, StringBuilder output, FormatterParams parameters)
		{
			if (ReadBreakLine(ref input))
			{
				output.AppendLine();
				return true;
			}
			return false;
		}

		private bool ReadBreakLine(ref StringReader input)
		{
			if (input.current == '\n')
			{
				input.position++;
				return true;
			}
			if (input.remaining >= 2 && input.NextEquals("\\n"))
			{
				input.position += 2;
				return true;
			}
			return false;
		}
	}
}
