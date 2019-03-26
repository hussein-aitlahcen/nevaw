using System.Text;

namespace Ankama.Cube.Data.UI.Localization.TextFormatting
{
	public class EscapedBraceParserRule : IParserRule
	{
		public bool TryFormat(ref StringReader input, StringBuilder output, FormatterParams parameters)
		{
			if (input.remaining >= 2 && input.current == '\\')
			{
				char next = input.next;
				if (next == '{' || next == '}')
				{
					output.Append(next);
					input.position += 2;
					return true;
				}
			}
			return false;
		}
	}
}
