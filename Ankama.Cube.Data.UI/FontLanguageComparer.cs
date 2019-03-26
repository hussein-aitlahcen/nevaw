using System.Collections.Generic;

namespace Ankama.Cube.Data.UI
{
	public class FontLanguageComparer : IEqualityComparer<FontLanguage>
	{
		public static FontLanguageComparer instance;

		static FontLanguageComparer()
		{
			instance = new FontLanguageComparer();
		}

		public bool Equals(FontLanguage x, FontLanguage y)
		{
			return x == y;
		}

		public int GetHashCode(FontLanguage obj)
		{
			return (int)obj;
		}
	}
}
