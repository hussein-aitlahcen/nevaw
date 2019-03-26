using System;
using System.Collections.Generic;

namespace Ankama.Cube.Data.UI.Localization.TextFormatting.ValueProviders.FightValueProviders
{
	public class AbstractDynamicFightValueProvider : IFightValueProvider, IValueProvider
	{
		public int level
		{
			get;
		}

		public IReadOnlyList<ILevelOnlyDependant> dynamicValues
		{
			get;
		}

		protected AbstractDynamicFightValueProvider(IReadOnlyList<ILevelOnlyDependant> dynamicValues, int level)
		{
			this.dynamicValues = dynamicValues;
			this.level = level;
		}

		public string GetValue(string name)
		{
			return GetValueInt(name).ToString();
		}

		public int GetValueInt(string name)
		{
			return dynamicValues.GetValueInt(name, level);
		}

		public virtual int GetDamageModifierValue()
		{
			return 0;
		}

		public virtual int GetHealModifierValue()
		{
			return 0;
		}

		public virtual Tuple<int, int> GetRange()
		{
			return null;
		}
	}
}
