using Ankama.Cube.Data.UI.Localization.TextFormatting;

namespace Ankama.Cube.Code.UI
{
	public struct TextData
	{
		public readonly bool isValid;

		public readonly int? textId;

		public readonly IValueProvider valueProvider;

		public TextData(int textId, params string[] textParams)
		{
			isValid = true;
			this.textId = textId;
			valueProvider = new IndexedValueProvider(textParams);
		}

		public static implicit operator TextData(int textId)
		{
			return new TextData(textId);
		}
	}
}
