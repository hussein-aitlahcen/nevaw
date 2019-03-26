using Ankama.Cube.Utility;
using System.Collections.Concurrent;
using System.Text;

namespace Ankama.Cube.Data.UI.Localization.TextFormatting
{
	public class TextFormatter
	{
		private static readonly ConcurrentStack<StringBuilder> s_stringBuildersPool = new ConcurrentStack<StringBuilder>();

		private readonly IParserRule[] m_parserRules;

		public IPluralRules pluralRules
		{
			get;
			set;
		} = new PluralRulesEN();


		public TextFormatter(IParserRule[] parserRules)
		{
			m_parserRules = parserRules;
		}

		public string Format(string pattern, FormatterParams formatterParams)
		{
			StringBuilder stringBuilder = GetStringBuilder();
			StringReader reader = new StringReader(pattern);
			AppendFormat(ref reader, stringBuilder, formatterParams);
			string result = stringBuilder.ToString();
			ReleaseStringBuilder(stringBuilder);
			return result;
		}

		public void AppendFormat(SubString text, StringBuilder output, FormatterParams formatterParams)
		{
			StringReader reader = new StringReader(text.originalText);
			reader.position = text.startIndex;
			reader.SetLimit(text.endIndex);
			AppendFormat(ref reader, output, formatterParams);
		}

		private void AppendFormat(ref StringReader reader, StringBuilder writer, FormatterParams formatterParams)
		{
			IParserRule[] parserRules = m_parserRules;
			int num = m_parserRules.Length;
			while (reader.hasNext)
			{
				bool flag = false;
				for (int i = 0; i < num; i++)
				{
					if (parserRules[i].TryFormat(ref reader, writer, formatterParams))
					{
						flag = true;
						break;
					}
				}
				IParserRule[] additionnalRules = formatterParams.additionnalRules;
				if (additionnalRules != null)
				{
					for (int j = 0; j < additionnalRules.Length; j++)
					{
						if (additionnalRules[j].TryFormat(ref reader, writer, formatterParams))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					writer.Append(reader.ReadNext());
				}
			}
		}

		private static StringBuilder GetStringBuilder()
		{
			if (s_stringBuildersPool.TryPop(out StringBuilder result))
			{
				result.Clear();
				return result;
			}
			return new StringBuilder(512);
		}

		private static void ReleaseStringBuilder(StringBuilder sb)
		{
			s_stringBuildersPool.Push(sb);
		}
	}
}
