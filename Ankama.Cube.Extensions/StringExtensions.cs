using System.Text;

namespace Ankama.Cube.Extensions
{
	public static class StringExtensions
	{
		public static string RemoveTags(this string s)
		{
			int num = s.IndexOf('<');
			if (num == -1)
			{
				return s;
			}
			int num2 = 0;
			StringBuilder stringBuilder = new StringBuilder();
			while (num != -1)
			{
				if (num2 < num)
				{
					stringBuilder.Append(s.Substring(num2, num - num2));
				}
				int num3 = s.IndexOf('>', num);
				num2 = ((num3 == -1) ? num : (num3 + 1));
				num = s.IndexOf('<', num3);
			}
			if (num2 != -1 && num2 < s.Length)
			{
				stringBuilder.Append(s.Substring(num2, s.Length - num2));
			}
			return stringBuilder.ToString();
		}
	}
}
