using Ankama.Cube.Data;
using Ankama.Cube.Data.UI.Localization.TextFormatting;
using Ankama.Cube.Data.UI.Localization.TextFormatting.ValueProviders.FightValueProviders;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.UI.Components;
using JetBrains.Annotations;
using System;

namespace Ankama.Cube.UI.Fight.Windows
{
	public static class TooltipDataProviderFactory
	{
		private class CharacterTooltipDataProvider<T> : TooltipDataProvider<T>, ICharacterTooltipDataProvider, ITooltipDataProvider where T : CharacterDefinition, IDefinitionWithTooltip
		{
			public CharacterTooltipDataProvider(T definition, int level)
				: base(TooltipDataType.Character, definition, level)
			{
			}

			public ActionType GetActionType()
			{
				return m_definition.actionType;
			}

			public TooltipActionIcon GetActionIcon()
			{
				return TooltipWindowUtility.GetActionIcon(m_definition);
			}

			public bool TryGetActionValue(out int value)
			{
				if (m_definition.actionValue == null)
				{
					value = 0;
					return false;
				}
				value = m_definition.actionValue.GetValueWithLevel(m_valueProvider.level);
				return true;
			}

			public int GetLifeValue()
			{
				return m_definition.life.GetValueWithLevel(m_valueProvider.level);
			}

			public int GetMovementValue()
			{
				return m_definition.movementPoints.GetValueWithLevel(m_valueProvider.level);
			}
		}

		private class ObjectMechanismTooltipDataProvider : TooltipDataProvider<ObjectMechanismDefinition>, IObjectMechanismTooltipDataProvider, ITooltipDataProvider
		{
			public ObjectMechanismTooltipDataProvider(ObjectMechanismDefinition definition, int level)
				: base(TooltipDataType.ObjectMechanism, definition, level)
			{
			}

			public int GetArmorValue()
			{
				return m_definition.baseMecaLife.GetValueWithLevel(m_valueProvider.level);
			}
		}

		private class FloorMechanismTooltipDataProvider : TooltipDataProvider<FloorMechanismDefinition>, IFloorMechanismTooltipDataProvider, ITooltipDataProvider
		{
			public FloorMechanismTooltipDataProvider(FloorMechanismDefinition definition, int level)
				: base(TooltipDataType.FloorMechanism, definition, level)
			{
			}
		}

		private class SpellTooltipDataProvider : TooltipDataProvider<SpellDefinition>, ISpellTooltipDataProvider, ITooltipDataProvider
		{
			public SpellTooltipDataProvider(SpellDefinition definition, int level)
				: base(TooltipDataType.Spell, definition, level)
			{
			}

			public TooltipElementValues GetGaugeModifications()
			{
				return TooltipWindowUtility.GetTooltipElementValues(m_definition, null);
			}
		}

		private class ReserveTooltipDataProvider : TooltipDataProvider<ReserveDefinition>, ITextTooltipDataProvider, ITooltipDataProvider
		{
			public ReserveTooltipDataProvider(ReserveDefinition definition, int level)
				: base(TooltipDataType.Text, definition, level)
			{
			}
		}

		private class KeywordTooltipDataProvider : ITextTooltipDataProvider, ITooltipDataProvider
		{
			private readonly int m_titleKey;

			private readonly int m_descriptionKey;

			private readonly IFightValueProvider m_valueProvider;

			public TooltipDataType tooltipDataType => TooltipDataType.Text;

			public KeywordReference[] keywordReferences => null;

			public KeywordTooltipDataProvider(string keyword, IFightValueProvider valueProvider)
			{
				RuntimeData.TryGetTextKeyId("KEYWORD." + keyword + ".NAME", out m_titleKey);
				RuntimeData.TryGetTextKeyId("KEYWORD." + keyword + ".DESC", out m_descriptionKey);
				m_valueProvider = valueProvider;
			}

			public int GetTitleKey()
			{
				return m_titleKey;
			}

			public int GetDescriptionKey()
			{
				return m_descriptionKey;
			}

			public IFightValueProvider GetValueProvider()
			{
				return m_valueProvider;
			}
		}

		private abstract class TooltipDataProvider<T> : ITooltipDataProvider where T : class, IDefinitionWithTooltip
		{
			protected readonly T m_definition;

			protected readonly IFightValueProvider m_valueProvider;

			public TooltipDataType tooltipDataType
			{
				get;
			}

			public KeywordReference[] keywordReferences => m_definition.precomputedData.keywordReferences;

			protected TooltipDataProvider(TooltipDataType type, T definition, int level)
			{
				tooltipDataType = type;
				m_definition = definition;
				m_valueProvider = new FightValueProvider(definition, level);
			}

			public int GetTitleKey()
			{
				return m_definition.i18nNameId;
			}

			public int GetDescriptionKey()
			{
				return m_definition.i18nDescriptionId;
			}

			public IFightValueProvider GetValueProvider()
			{
				return m_valueProvider;
			}
		}

		[CanBeNull]
		public static ITooltipDataProvider Create(KeywordReference keywordReference, IFightValueProvider valueProvider)
		{
			if (keywordReference.type == ObjectReference.Type.None)
			{
				return new KeywordTooltipDataProvider(keywordReference.keyword, valueProvider);
			}
			IDefinitionWithTooltip @object = ObjectReference.GetObject(keywordReference.type, keywordReference.id);
			if (@object != null)
			{
				return Create(@object, valueProvider.level);
			}
			return null;
		}

		[CanBeNull]
		public static ITooltipDataProvider Create<T>(T definition, int level) where T : IDefinitionWithTooltip
		{
			SpellDefinition spellDefinition = definition as SpellDefinition;
			if (spellDefinition != null)
			{
				return Create(spellDefinition, level);
			}
			CompanionDefinition companionDefinition = definition as CompanionDefinition;
			if (companionDefinition != null)
			{
				return Create(companionDefinition, level);
			}
			SummoningDefinition summoningDefinition = definition as SummoningDefinition;
			if (summoningDefinition != null)
			{
				return Create(summoningDefinition, level);
			}
			WeaponDefinition weaponDefinition = definition as WeaponDefinition;
			if (weaponDefinition != null)
			{
				return Create(weaponDefinition, level);
			}
			FloorMechanismDefinition floorMechanismDefinition = definition as FloorMechanismDefinition;
			if (floorMechanismDefinition != null)
			{
				return Create(floorMechanismDefinition, level);
			}
			ObjectMechanismDefinition objectMechanismDefinition = definition as ObjectMechanismDefinition;
			if (objectMechanismDefinition != null)
			{
				return Create(objectMechanismDefinition, level);
			}
			ReserveDefinition reserveDefinition = definition as ReserveDefinition;
			if (reserveDefinition != null)
			{
				return Create(reserveDefinition, level);
			}
			throw new ArgumentOutOfRangeException();
		}

		private static ITooltipDataProvider Create(SpellDefinition spell, int level)
		{
			return new SpellTooltipDataProvider(spell, level);
		}

		private static ITooltipDataProvider Create(CompanionDefinition companion, int level)
		{
			return new CharacterTooltipDataProvider<CompanionDefinition>(companion, level);
		}

		private static ITooltipDataProvider Create(SummoningDefinition summoning, int level)
		{
			return new CharacterTooltipDataProvider<SummoningDefinition>(summoning, level);
		}

		private static ITooltipDataProvider Create(WeaponDefinition weapon, int level)
		{
			return new CharacterTooltipDataProvider<WeaponDefinition>(weapon, level);
		}

		private static ITooltipDataProvider Create(ObjectMechanismDefinition mechanism, int level)
		{
			return new ObjectMechanismTooltipDataProvider(mechanism, level);
		}

		private static ITooltipDataProvider Create(FloorMechanismDefinition mechanism, int level)
		{
			return new FloorMechanismTooltipDataProvider(mechanism, level);
		}

		private static ITooltipDataProvider Create(ReserveDefinition mechanism, int level)
		{
			return new ReserveTooltipDataProvider(mechanism, level);
		}
	}
}
