using Ankama.Cube.UI.Fight.Info;
using JetBrains.Annotations;
using System.Collections.Generic;

namespace Ankama.Cube.Data.UI.Localization.TextFormatting
{
	public class FightInfoValueProvider : IValueProvider
	{
		[NotNull]
		private readonly FightInfoMessageRibbon InfoRoot;

		public FightInfoValueProvider(FightInfoMessageRibbon parent)
		{
			InfoRoot = parent;
		}

		public string GetValue(string name)
		{
			if (int.TryParse(name, out int result))
			{
				IReadOnlyList<string> parameter = InfoRoot.GetParameter();
				if (result >= 0 && result < parameter.Count)
				{
					return parameter[result];
				}
			}
			return null;
		}
	}
}
