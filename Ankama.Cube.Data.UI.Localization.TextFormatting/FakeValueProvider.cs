namespace Ankama.Cube.Data.UI.Localization.TextFormatting
{
	public class FakeValueProvider : IValueProvider
	{
		public string GetValue(string name)
		{
			return name;
		}
	}
}
