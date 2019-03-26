using System.Text;

namespace Ankama.Cube.Data.UI.Localization.TextFormatting
{
	public class VertivalTabParserRule : IParserRule
	{
		public bool TryFormat(ref StringReader input, StringBuilder output, FormatterParams parameters)
		{
			if (input.remaining >= 2 && input.NextEquals("\\v"))
			{
				output.AppendLine();
				input.position += 2;
				input.SkipSpaces();
				return true;
			}
			return false;
		}
	}
}
