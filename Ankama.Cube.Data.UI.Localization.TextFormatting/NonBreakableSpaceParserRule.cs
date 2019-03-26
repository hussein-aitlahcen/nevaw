using System.Text;

namespace Ankama.Cube.Data.UI.Localization.TextFormatting
{
	public class NonBreakableSpaceParserRule : IParserRule
	{
		public const char NonBreakableSpace = '\u00a0';

		public bool TryFormat(ref StringReader input, StringBuilder output, FormatterParams parameters)
		{
			if (input.remaining >= 2 && input.NextEquals("\\_"))
			{
				output.Append('\u00a0');
				input.position += 2;
				return true;
			}
			return false;
		}
	}
}
