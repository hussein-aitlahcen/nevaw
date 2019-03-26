using Ankama.Cube.Utility;

namespace Ankama.Cube.Data.UI.Localization.TextFormatting
{
	public struct StringReader
	{
		public readonly string text;

		public int position;

		public int limit
		{
			get;
			private set;
		}

		public int remaining => limit - position;

		public bool hasNext => position < limit;

		public char previous => text[position - 1];

		public char current => text[position];

		public char next => text[position + 1];

		public StringReader(string text)
		{
			this.text = text;
			position = 0;
			limit = text.Length;
		}

		public StringReader(SubString text)
		{
			this.text = text.originalText;
			position = text.startIndex;
			limit = text.endIndex;
		}

		public StringReader Copy()
		{
			StringReader result = new StringReader(text);
			result.position = position;
			result.limit = limit;
			return result;
		}

		public void SetLimit(int length)
		{
			limit = length;
		}

		public bool NextEquals(string value)
		{
			int length = value.Length;
			int num = position;
			for (int i = 0; i < length; i++)
			{
				if (num >= limit)
				{
					break;
				}
				if (text[num] != value[i])
				{
					return false;
				}
				num++;
			}
			return true;
		}

		public void ReadUntil(char c)
		{
			int num = position;
			while (hasNext && text[num] != c)
			{
				num++;
			}
			position = num;
		}

		public bool Read(string value)
		{
			int length = value.Length;
			int num = position;
			for (int i = 0; i < length; i++)
			{
				if (num >= limit)
				{
					break;
				}
				if (text[num] != value[i])
				{
					return false;
				}
				num++;
			}
			position = num;
			return true;
		}

		public bool Read(char test)
		{
			char num = text[position];
			position++;
			return num == test;
		}

		public void SkipSpaces()
		{
			int i;
			for (i = position; i < limit && char.IsWhiteSpace(text[i]); i++)
			{
			}
			position = i;
		}

		public char ReadNext()
		{
			char result = text[position];
			position++;
			return result;
		}

		public SubString ReadWord()
		{
			int i = position;
			SubString subString = new SubString(text, i, 0);
			for (; i < limit && (char.IsLetterOrDigit(text[i]) || text[i] == '.'); i++)
			{
			}
			subString.length = i - subString.startIndex;
			position = i;
			return subString;
		}

		public int ReadInt()
		{
			int i = position;
			SubString subString = new SubString(text, i, 0);
			for (; i < limit && char.IsDigit(text[i]); i++)
			{
			}
			if (i == position)
			{
				throw new TextFormatterException(text, i, "Cannot read a int");
			}
			subString.length = i - subString.startIndex;
			position = i;
			return int.Parse(subString.ToString());
		}

		public SubString ReadContent(char open, char close)
		{
			if (current != open)
			{
				throw new TextFormatterException(text, position, $"Cannot read a content between {open}{close}");
			}
			int num = position;
			int num2 = num + 1;
			int num3 = 1;
			while (num3 != 0 && num < limit - 1)
			{
				num++;
				char c = text[num];
				if (c == open)
				{
					num3++;
				}
				else if (c == close)
				{
					num3--;
				}
			}
			if (num3 != 0)
			{
				throw new TextFormatterException(text, num2, $"Not balanced {open} {close}");
			}
			position = num + 1;
			return new SubString(text, num2, num - num2);
		}

		public bool ReadTag(string tagName, ref SubString tagAttributes, ref SubString content)
		{
			int num = position;
			if (Read('<') && Read(tagName))
			{
				tagAttributes.startIndex = position;
				bool flag = false;
				while (hasNext)
				{
					if (current == '>')
					{
						flag = true;
						break;
					}
					position++;
				}
				if (flag)
				{
					tagAttributes.length = position - tagAttributes.startIndex;
					content.startIndex = position + 1;
					int num2 = 1;
					int startIndex = content.startIndex;
					try
					{
						while (num2 != 0 && position < limit - 1)
						{
							startIndex = position;
							if (Read('<'))
							{
								bool flag2 = false;
								if (current == '/')
								{
									position++;
									flag2 = true;
								}
								if (Read(tagName))
								{
									if (flag2)
									{
										if (Read('>'))
										{
											num2--;
										}
									}
									else
									{
										ReadUntil('>');
										if (current == '>')
										{
											num2++;
										}
									}
								}
							}
						}
					}
					catch
					{
					}
					if (num2 == 0)
					{
						content.length = startIndex - content.startIndex;
						return true;
					}
				}
			}
			position = num;
			return false;
		}
	}
}
