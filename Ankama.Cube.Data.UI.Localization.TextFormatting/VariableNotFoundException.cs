namespace Ankama.Cube.Data.UI.Localization.TextFormatting
{
	public class VariableNotFoundException : TextFormatterException
	{
		public VariableNotFoundException(string text, int index, string variableName)
			: base(text, index, "Variable '" + variableName + "' could not be found.")
		{
		}
	}
}
