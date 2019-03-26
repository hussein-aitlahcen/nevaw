using Ankama.Cube.Fight.Entities;
using JetBrains.Annotations;
using System;

namespace Ankama.Cube.Data.UI.Localization.TextFormatting.ValueProviders.FightValueProviders
{
	public class CastableFightValueProvider : AbstractDynamicFightValueProvider
	{
		[NotNull]
		private readonly ICastableStatus m_source;

		public CastableFightValueProvider([NotNull] ICastableStatus source)
			: base(source.GetDefinition().precomputedData.dynamicValueReferences, source.level)
		{
			m_source = source;
		}

		public override Tuple<int, int> GetRange()
		{
			ActionRange actionRange = (m_source.GetDefinition() as CharacterDefinition)?.actionRange;
			if (actionRange == null)
			{
				return null;
			}
			return new Tuple<int, int>(actionRange.min.GetValueWithLevel(base.level), actionRange.max.GetValueWithLevel(base.level));
		}
	}
}
