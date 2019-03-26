using System;

namespace Ankama.Cube.Data.UI.Localization.TextFormatting.ValueProviders.FightValueProviders
{
	public interface IFightValueProvider : IValueProvider
	{
		int level
		{
			get;
		}

		int GetValueInt(string name);

		int GetDamageModifierValue();

		int GetHealModifierValue();

		Tuple<int, int> GetRange();
	}
}
