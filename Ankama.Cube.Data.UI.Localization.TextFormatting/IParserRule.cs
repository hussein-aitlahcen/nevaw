using System.Text;

namespace Ankama.Cube.Data.UI.Localization.TextFormatting
{
	public interface IParserRule
	{
		bool TryFormat(ref StringReader input, StringBuilder output, FormatterParams formatterParams);
	}
}
