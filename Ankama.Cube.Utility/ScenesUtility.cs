using Ankama.Utilities;
using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ankama.Cube.Utility
{
	public static class ScenesUtility
	{
		public const string FightMapWrapperSceneName = "FightMapWrapper";

		public const string HavreMapWrapperSceneName = "HavreMapWrapper";

		public const string FightUISceneName = "FightUI";

		public const string FightUIReworkSceneName = "FightUIRework";

		public const string WorldUISceneName = "WorldUI";

		public const string FightEndedWinUISceneName = "FightEndedWinUI";

		public const string FightEndedLoseUISceneName = "FightEndedLoseUI";

		public const string FightEndedDrawUISceneName = "FightEndedDrawUI";

		public const string PopupInfoUISceneName = "PopupInfoUI";

		public const string PlayerUIMainState = "PlayerLayerUI";

		public const string PlayerUINavState = "PlayerLayer_NavRibbonCanvas";

		public const string ParametersUISceneName = "ParametersUI";

		public const string OptionUISceneName = "OptionUI";

		public const string BugReportUISceneName = "BugReportUI";

		public const string DeckUISceneName = "DeckUI";

		public const string DeckMainUISceneName = "PlayerLayer_DeckCanvas";

		public const string ProfileUISceneName = "PlayerLayer_ProfilCanvas";

		public const string GodSelectionUISceneName = "GodSelectionUI";

		public const string UIZaap_Pvp = "UIZaap_PVP";

		public const string UIZaap_1v1Loaing = "MatchmakingUI_1v1";

		public static string GetFightMapSceneName(int id)
		{
			return "FightMap_" + id;
		}

		public static string GetHavreMapSceneName(int id)
		{
			switch (id)
			{
			case 0:
				return "HavreDimension_Amakna";
			case 1:
				return "HavreDimension_Bonta";
			case 2:
				return "HavreDimension_Brakmar";
			case 3:
				return "HavreDimension_Sufokia";
			case 4:
				return "HavreDimension_Astrub";
			default:
				return "HavreDimension_Amakna";
			}
		}

		public static T GetSceneRoot<T>(Scene scene) where T : Component
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			GameObject sceneRoot = GetSceneRoot(scene);
			if (null == sceneRoot)
			{
				return default(T);
			}
			T componentInChildren = sceneRoot.GetComponentInChildren<T>();
			if (null == (object)componentInChildren)
			{
				Log.Error("Cannot find component of type '" + typeof(T).Name + "' in root object of scene named '" + scene.get_name() + "'.", 61, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Utility\\ScenesUtility.cs");
			}
			return componentInChildren;
		}

		[CanBeNull]
		public static T GetComponentInRootGameObjects<T>(Scene scene) where T : Component
		{
			int rootCount = scene.get_rootCount();
			if (rootCount == 0)
			{
				return default(T);
			}
			List<GameObject> list = ListPool<GameObject>.Get(scene.get_rootCount());
			scene.GetRootGameObjects(list);
			for (int i = 0; i < rootCount; i++)
			{
				T component = list[i].GetComponent<T>();
				if (null != (object)component)
				{
					ListPool<GameObject>.Release(list);
					return component;
				}
			}
			ListPool<GameObject>.Release(list);
			return default(T);
		}

		[CanBeNull]
		private static GameObject GetSceneRoot(Scene scene)
		{
			int rootCount = scene.get_rootCount();
			if (rootCount == 0)
			{
				Log.Error("Scene named '" + scene.get_name() + "' is empty.", 96, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Utility\\ScenesUtility.cs");
				return null;
			}
			List<GameObject> list = ListPool<GameObject>.Get(rootCount);
			scene.GetRootGameObjects(list);
			if (rootCount > 1)
			{
				Log.Warning("Scene named '" + scene.get_name() + "' has more than one root GameObject.", 105, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Utility\\ScenesUtility.cs");
			}
			for (int i = 0; i < rootCount; i++)
			{
				GameObject val = list[i];
				if (val.get_name().Equals("root"))
				{
					ListPool<GameObject>.Release(list);
					return val;
				}
			}
			Log.Error("Scene named '" + scene.get_name() + "' does not have a root GameObject named 'root'.", 117, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Utility\\ScenesUtility.cs");
			ListPool<GameObject>.Release(list);
			return null;
		}
	}
}
