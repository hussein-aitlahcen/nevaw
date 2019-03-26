using System.Text;

namespace Ankama.Cube.Data.UI.Localization.TextFormatting
{
	public static class KeywordUtility
	{
		public static void BeginKeyWord(this StringBuilder sb)
		{
			sb.Append("<uppercase><b><color=#3FD5D3>");
		}

		public static void EndKeyWord(this StringBuilder sb)
		{
			sb.Append("</color></b></uppercase>");
		}
	}
}
