using System;

namespace Ankama.Cube.Data.UI.Localization.TextFormatting
{
	public class TextFormatterException : Exception
	{
		public TextFormatterException(string text, int index)
			: base($"{text} at:{index}")
		{
		}

		public TextFormatterException(string text, int index, string message)
			: base($"{text} at:{index} : {message}")
		{
		}

		public TextFormatterException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
