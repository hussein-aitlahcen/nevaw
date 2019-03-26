using JetBrains.Annotations;
using System.Collections.Generic;

namespace Ankama.Cube.Data.UI.Localization.TextFormatting.ValueProviders.FightValueProviders
{
	public interface IDynamicValueSource
	{
		[CanBeNull]
		IReadOnlyList<ILevelOnlyDependant> dynamicValues
		{
			get;
		}
	}
}
