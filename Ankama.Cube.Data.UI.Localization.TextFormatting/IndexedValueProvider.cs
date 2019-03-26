using JetBrains.Annotations;

namespace Ankama.Cube.Data.UI.Localization.TextFormatting
{
	public class IndexedValueProvider : IValueProvider
	{
		[NotNull]
		private readonly string[] m_args;

		public IndexedValueProvider([NotNull] params string[] args)
		{
			m_args = args;
		}

		public string GetValue(string name)
		{
			int result;
			if (int.TryParse(name, out result) && result >= 0 && result < m_args.Length)
			{
				return m_args[result];
			}
			return null;
		}
	}
}
