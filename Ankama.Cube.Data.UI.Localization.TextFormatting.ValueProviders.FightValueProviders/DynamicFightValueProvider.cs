using Ankama.Cube.Fight.Entities;
using JetBrains.Annotations;
using System;

namespace Ankama.Cube.Data.UI.Localization.TextFormatting.ValueProviders.FightValueProviders
{
	public class DynamicFightValueProvider : AbstractDynamicFightValueProvider
	{
		[NotNull]
		private readonly IDynamicValueSource m_source;

		public DynamicFightValueProvider([NotNull] IDynamicValueSource source, int level)
			: base(source.dynamicValues, level)
		{
			m_source = source;
		}

		public override int GetDamageModifierValue()
		{
			return (m_source as IEntityWithAction)?.physicalDamageBoost ?? base.GetDamageModifierValue();
		}

		public override int GetHealModifierValue()
		{
			return (m_source as IEntityWithAction)?.physicalHealBoost ?? base.GetHealModifierValue();
		}

		public override Tuple<int, int> GetRange()
		{
			IEntityWithAction entityWithAction = m_source as IEntityWithAction;
			if (entityWithAction == null || !entityWithAction.hasRange)
			{
				return null;
			}
			return new Tuple<int, int>(entityWithAction.rangeMin, entityWithAction.rangeMax);
		}
	}
}
