namespace Ankama.Cube.Code.UI
{
	public struct RawTextData
	{
		public readonly bool isValid;

		public readonly string formattedText;

		public RawTextData(string formattedText)
		{
			isValid = true;
			this.formattedText = formattedText;
		}

		public RawTextData(int textId, params string[] textParams)
		{
			isValid = true;
			formattedText = RuntimeData.FormattedText(textId, textParams);
		}

		public static implicit operator RawTextData(int textId)
		{
			return new RawTextData(textId);
		}

		public static implicit operator RawTextData(string formattedText)
		{
			return new RawTextData(formattedText);
		}
	}
}
