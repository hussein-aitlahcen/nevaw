using Ankama.Cube.Data.UI.Localization.TextFormatting.ValueProviders.FightValueProviders;
using Ankama.Cube.Fight.Entities;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace Ankama.Cube.Data.UI.Localization.TextFormatting
{
	public class FightValueProvider : IFightValueProvider, IValueProvider
	{
		private readonly int m_damageModifier;

		private readonly int m_healModifier;

		private readonly Tuple<int, int> m_range;

		private readonly IReadOnlyList<ILevelOnlyDependant> m_dynamicValues;

		public int level
		{
			get;
		}

		public FightValueProvider([NotNull] ICastableStatus castableStatus)
			: this(new CastableFightValueProvider(castableStatus))
		{
		}

		public FightValueProvider(AbstractDynamicFightValueProvider dynamicProvider)
		{
			level = dynamicProvider.level;
			m_dynamicValues = dynamicProvider.dynamicValues;
			m_damageModifier = dynamicProvider.GetDamageModifierValue();
			m_healModifier = dynamicProvider.GetHealModifierValue();
			m_range = dynamicProvider.GetRange();
		}

		public FightValueProvider([NotNull] IDefinitionWithPrecomputedData definition, int level)
		{
			this.level = level;
			m_damageModifier = 0;
			m_healModifier = 0;
			m_range = CreateRange(definition, level);
			m_dynamicValues = definition.precomputedData.dynamicValueReferences;
		}

		public int GetValueInt(string name)
		{
			return m_dynamicValues.GetValueInt(name, level);
		}

		public int GetDamageModifierValue()
		{
			return m_damageModifier;
		}

		public int GetHealModifierValue()
		{
			return m_healModifier;
		}

		public Tuple<int, int> GetRange()
		{
			return m_range;
		}

		public string GetValue(string name)
		{
			return GetValueInt(name).ToString();
		}

		private static Tuple<int, int> CreateRange(IDefinitionWithPrecomputedData definition, int level)
		{
			CharacterDefinition characterDefinition = definition as CharacterDefinition;
			if (characterDefinition == null || characterDefinition.actionRange == null)
			{
				return null;
			}
			int valueWithLevel = characterDefinition.actionRange.min.GetValueWithLevel(level);
			int valueWithLevel2 = characterDefinition.actionRange.max.GetValueWithLevel(level);
			return new Tuple<int, int>(valueWithLevel, valueWithLevel2);
		}
	}
}
