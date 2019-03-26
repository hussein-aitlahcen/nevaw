using JetBrains.Annotations;
using System.Text;

namespace Ankama.Cube.Utility
{
	public struct SubString
	{
		public readonly string originalText;

		public int startIndex;

		public int length;

		public int endIndex => startIndex + length;

		public SubString(string originalText)
		{
			this = new SubString(originalText, 0, originalText.Length);
		}

		public SubString(string originalText, int startIndex, int length)
		{
			this.originalText = originalText;
			this.startIndex = startIndex;
			this.length = length;
		}

		[PublicAPI]
		public void Trim(params char[] trimChars)
		{
			TrimStart(trimChars);
			TrimEnd(trimChars);
		}

		[PublicAPI]
		public void TrimStart(params char[] trimChars)
		{
			int i = startIndex;
			int endIndex = this.endIndex;
			if (trimChars == null || trimChars.Length == 0)
			{
				for (; i < endIndex && char.IsWhiteSpace(originalText[i]); i++)
				{
				}
			}
			else
			{
				for (; i < endIndex && Contains(trimChars, originalText[i]); i++)
				{
				}
			}
			startIndex = i;
			length = endIndex - i;
		}

		[PublicAPI]
		public void TrimEnd(params char[] trimChars)
		{
			int num = startIndex;
			int num2 = endIndex - 1;
			if (trimChars == null || trimChars.Length == 0)
			{
				while (num2 >= num && char.IsWhiteSpace(originalText[num2]))
				{
					num2--;
				}
			}
			else
			{
				while (num2 >= num && Contains(trimChars, originalText[num2]))
				{
					num2--;
				}
			}
			length = num2 - startIndex + 1;
		}

		[PublicAPI]
		public void WriteTo(StringBuilder sb)
		{
			if (length > 0)
			{
				sb.Append(originalText, startIndex, length);
			}
		}

		[PublicAPI]
		public override string ToString()
		{
			if (length <= 0)
			{
				return string.Empty;
			}
			return originalText.Substring(startIndex, length);
		}

		public static explicit operator SubString(string text)
		{
			return new SubString(text, 0, text.Length);
		}

		private static bool Contains(char[] array, char ch)
		{
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				if (array[i] == ch)
				{
					return true;
				}
			}
			return false;
		}
	}
}
