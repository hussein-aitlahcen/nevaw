using Ankama.AssetManagement.AssetReferences;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class WeaponDefinition : CharacterDefinition, IDefinitionWithTooltip, IDefinitionWithDescription, IDefinitionWithPrecomputedData
	{
		[LocalizedString("WEAPON_{id}_NAME", "Weapons", 1)]
		[SerializeField]
		private int m_i18nNameId;

		[LocalizedString("WEAPON_{id}_DESCRIPTION", "Weapons", 3)]
		[SerializeField]
		private int m_i18nDescriptionId;

		private God m_god;

		private ILevelOnlyDependant m_playerActionPoints;

		private ILevelOnlyDependant m_maxMechanismsOnBoard;

		private ILevelOnlyDependant m_maxSummoningsOnBoard;

		private List<Id<SpellDefinition>> m_spells;

		private Id<SquadDefinition> m_defaultDeck;

		[SerializeField]
		private AssetReference m_illustrationReference;

		[SerializeField]
		private AssetReference m_femaleIllustrationReference;

		[SerializeField]
		private AssetReference m_fullMaleIllustrationReference;

		[SerializeField]
		private AssetReference m_fullFemaleIllustrationReference;

		[SerializeField]
		private AssetReference m_weaponIllustrationReference;

		[SerializeField]
		private AssetReference m_UIMatchmakingIlluReference;

		[SerializeField]
		private AssetReference m_uiAnimatedCharacterReference;

		[SerializeField]
		private AssetReference m_uiWeaponButtonMatReference;

		[SerializeField]
		private Color m_uiWeaponButtonShineColor;

		[SerializeField]
		private Color m_deckBuildingBackgroundColor;

		[SerializeField]
		private Color m_deckBuildingBackgroundColor2;

		public int i18nNameId => m_i18nNameId;

		public int i18nDescriptionId => m_i18nDescriptionId;

		public God god => m_god;

		public ILevelOnlyDependant playerActionPoints => m_playerActionPoints;

		public ILevelOnlyDependant maxMechanismsOnBoard => m_maxMechanismsOnBoard;

		public ILevelOnlyDependant maxSummoningsOnBoard => m_maxSummoningsOnBoard;

		public IReadOnlyList<Id<SpellDefinition>> spells => m_spells;

		public Id<SquadDefinition> defaultDeck => m_defaultDeck;

		public Color deckBuildingBackgroundColor => m_deckBuildingBackgroundColor;

		public Color deckBuildingBackgroundColor2 => m_deckBuildingBackgroundColor2;

		public Color deckBuildingWeaponShine => m_uiWeaponButtonShineColor;

		public override void PopulateFromJson(JObject jsonObject)
		{
			base.PopulateFromJson(jsonObject);
			m_god = (God)Serialization.JsonTokenValue<int>(jsonObject, "god", 0);
			m_playerActionPoints = ILevelOnlyDependantUtils.FromJsonProperty(jsonObject, "playerActionPoints");
			m_maxMechanismsOnBoard = ILevelOnlyDependantUtils.FromJsonProperty(jsonObject, "maxMechanismsOnBoard");
			m_maxSummoningsOnBoard = ILevelOnlyDependantUtils.FromJsonProperty(jsonObject, "maxSummoningsOnBoard");
			JArray val = Serialization.JsonArray(jsonObject, "spells");
			m_spells = new List<Id<SpellDefinition>>((val != null) ? val.get_Count() : 0);
			if (val != null)
			{
				foreach (JToken item2 in val)
				{
					Id<SpellDefinition> item = Serialization.JsonTokenIdValue<SpellDefinition>(item2);
					m_spells.Add(item);
				}
			}
			m_defaultDeck = Serialization.JsonTokenIdValue<SquadDefinition>(jsonObject, "defaultDeck");
		}

		public AssetReference GetWeaponIllustrationReference()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return m_weaponIllustrationReference;
		}

		public AssetReference GetIlluMatchmakingReference()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return m_UIMatchmakingIlluReference;
		}

		public AssetReference GetIllustrationReference(Gender gender = Gender.Male)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			switch (gender)
			{
			case Gender.Male:
				return m_illustrationReference;
			case Gender.Female:
				return m_femaleIllustrationReference;
			default:
				throw new ArgumentOutOfRangeException("gender", gender, null);
			}
		}

		public AssetReference GetFullIllustrationReference(Gender gender = Gender.Male)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			switch (gender)
			{
			case Gender.Male:
				return m_fullMaleIllustrationReference;
			case Gender.Female:
				return m_fullFemaleIllustrationReference;
			default:
				throw new ArgumentOutOfRangeException("gender", gender, null);
			}
		}

		public AssetReference GetUIAnimatedCharacterReference(Gender gender = Gender.Male)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			switch (gender)
			{
			case Gender.Male:
				return m_uiAnimatedCharacterReference;
			case Gender.Female:
				return m_uiAnimatedCharacterReference;
			default:
				throw new ArgumentOutOfRangeException("gender", gender, null);
			}
		}

		public AssetReference GetUIWeaponButtonReference()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return m_uiWeaponButtonMatReference;
		}
	}
}
