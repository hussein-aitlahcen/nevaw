using System.Text;

namespace Ankama.Cube.Data.UI.Localization.TextFormatting
{
	public class DynamicValueParserRule : IParserRuleWithPrefix, IParserRule
	{
		public string prefix => "value";

		public bool TryFormat(ref StringReader reader, StringBuilder output, FormatterParams formatterParams)
		{
			if (reader.Read("value") && reader.ReadNext() == ':')
			{
				string name = reader.ReadWord().ToString();
				string value = formatterParams.valueProvider.GetValue(name);
				output.Append(value);
				return true;
			}
			return false;
		}
	}
}
