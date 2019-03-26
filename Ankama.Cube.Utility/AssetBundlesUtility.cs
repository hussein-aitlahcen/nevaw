using Ankama.Cube.Data;
using FMODUnity;
using JetBrains.Annotations;
using System;

namespace Ankama.Cube.Utility
{
	public static class AssetBundlesUtility
	{
		public const string AudioMasterBundleName = "core/audio/master";

		public const string DataBundleName = "core/data";

		public const string GameDataBundleName = "core/gamedata";

		public const string TextCollectionsBundleName = "core/localization";

		public const string FightMapWrapperBundleName = "core/scenes/maps/fight_maps";

		public const string HavreMapWrapperBundleName = "core/scenes/maps/havre_maps";

		public const string CommonUIBundleName = "core/ui/common";

		public const string FightUIBundleName = "core/scenes/ui/fight";

		public const string TransitionUIBundleName = "core/scenes/ui/transition";

		public const string ExampleUIBundleName = "core/scenes/ui/examples";

		public const string LoginUIBundleName = "core/scenes/ui/login";

		public const string MatchmakingUIBundleName = "core/scenes/ui/matchmaking";

		public const string PopupInfoUIBundleName = "core/scenes/ui/popup";

		public const string WorldUIBundleName = "core/scenes/ui/world";

		public const string DeckUIBundleName = "core/scenes/ui/deck";

		public const string PlayerUIBundleName = "core/scenes/ui/player";

		public const string OptionUIBundleName = "core/scenes/ui/option";

		public const string FightObjectFactoryBundleName = "core/factories/fight_object_factory";

		public const string MapObjectFactoryBundleName = "core/factories/map_object_factory";

		public const string CharacterResourcesBundleFolder = "core/characters/";

		public const string SpellEffectsBundleName = "core/spells/effects";

		public const string UIGodResourcesBundleName = "core/ui/gods";

		public const string UIMatchmakingIlluBundleName = "core/ui/matchmakingui";

		public const string UICharacterResourcesBundleFolder = "core/ui/characters/";

		public const string UICompanionResourcesBundleName = "core/ui/characters/companions";

		public const string UIWeaponResourcesBundleName = "core/ui/characters/heroes";

		public const string UIAnimationResourcesBundleName = "core/ui/characters/animatedcharacters";

		public const string UIWeaponMaterialButtonBundleName = "core/ui/characters/weaponbutton";

		public const string UIObjectMechanismResourcesBundleName = "core/ui/characters/objectmechanisms";

		public const string UISummoningResourcesBundleName = "core/ui/characters/summonings";

		public const string UISpellResourcesBundleName = "core/ui/spells";

		public static bool TryGetAudioBundleName([NotNull] string fileName, [NotNull] out string bundleName)
		{
			string[] array = fileName.Split(new char[1]
			{
				'_'
			});
			if (array.Length >= 2)
			{
				string text = array[0];
				if (text.Equals("Core", StringComparison.OrdinalIgnoreCase))
				{
					bundleName = "core/audio/" + string.Join("/", array, 1, Math.Max(1, array.Length - 2)).ToLowerInvariant();
					return true;
				}
				if (text.Equals("OpenWorld", StringComparison.OrdinalIgnoreCase))
				{
					bundleName = "openworld/audio/" + string.Join("/", array, 1, Math.Max(1, array.Length - 2)).ToLowerInvariant();
					return true;
				}
			}
			bundleName = string.Empty;
			return false;
		}

		[NotNull]
		public static string GetAudioBundleVariant()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Expected I4, but got Unknown
			FMODPlatform currentPlatform = FMODUtility.get_currentPlatform();
			switch ((int)currentPlatform)
			{
			case 0:
			case 2:
			case 22:
				throw new ArgumentException("fmodPlatform");
			case 1:
			case 3:
			case 8:
			case 9:
			case 10:
			case 19:
				return "desktop";
			case 4:
			case 5:
			case 6:
			case 11:
			case 12:
			case 13:
				return "mobile";
			case 7:
			case 14:
			case 15:
			case 16:
			case 17:
			case 18:
			case 20:
				throw new NotSupportedException();
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		public static string GetAnimatedCharacterDataBundle(BundleCategory bundleCategory)
		{
			return "core/characters/" + bundleCategory.GetBundleName();
		}

		public static string GetUICharacterResourcesBundleName()
		{
			return "core/ui/characters/heroes";
		}

		public static string GetUIAnimatedCharacterResourcesBundleName()
		{
			return "core/ui/characters/animatedcharacters";
		}

		public static string GetUICharacterResourcesBundleName(CompanionDefinition definition)
		{
			return "core/ui/characters/companions";
		}

		public static string GetUICharacterResourcesBundleName(SummoningDefinition definition)
		{
			return "core/ui/characters/summonings";
		}

		public static string GetUICharacterResourcesBundleName(ObjectMechanismDefinition definition)
		{
			return "core/ui/characters/objectmechanisms";
		}

		public static string GetFightMapAssetBundleName(int fightMapId)
		{
			return "core/scenes/maps/fight_maps";
		}

		public static string GetUIGodsResourcesBundleName()
		{
			return "core/ui/gods";
		}
	}
}
