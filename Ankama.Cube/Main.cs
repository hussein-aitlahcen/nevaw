using Ankama.AssetManagement;
using Ankama.AssetManagement.InputManagement;
using Ankama.Cube.Audio;
using Ankama.Cube.Code.UI;
using Ankama.Cube.Configuration;
using Ankama.Cube.Network;
using Ankama.Cube.Player;
using Ankama.Cube.SRP;
using Ankama.Cube.States;
using Ankama.Cube.Utility;
using Ankama.Utilities;
using System;
using System.Collections;
using UnityEngine;

namespace Ankama.Cube
{
	public class Main : MonoBehaviour
	{
		public enum InitializationFailure
		{
			RuntimeDataInitialisation,
			BootConfigInitialisation,
			ApplicationConfigInitialisation,
			UnvalidVersion,
			ServerStatusError,
			ServerStatusMaintenance,
			RuntimeDataLoad,
			AssetManagerInitialisation
		}

		private bool m_serverStatusChecked;

		public static MonoBehaviour monoBehaviour
		{
			get;
			private set;
		}

		public static void Quit()
		{
			Application.Quit();
		}

		private void Awake()
		{
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Expected O, but got Unknown
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Expected O, but got Unknown
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Expected O, but got Unknown
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Expected O, but got Unknown
			string text = "START version: 0.1.0.6045 " + DateTime.Now.ToShortDateString();
			Log.Info(text + "\n" + new string('-', 35 + text.Length), 40, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Main.cs");
			Device.LogInfo();
			Object.DontDestroyOnLoad(this.get_gameObject());
			PlayerPreferences.Load();
			ApplicationConfig.Read();
			QualityManager.Load();
			if (PlayerPreferences.graphicPresetIndex == -1)
			{
				PlayerPreferences.graphicPresetIndex = QualityManager.GetQualityPresetIndex();
				PlayerPreferences.Save();
			}
			QualityManager.SetQualityPresetIndex(PlayerPreferences.graphicPresetIndex);
			monoBehaviour = this;
			StateManager.RegisterInputDefinition(new InputKeyCodeDefinition(27, 1, 0, 0.4f, 0.1f));
			StateManager.RegisterInputDefinition(new InputKeyCodeDefinition(13, 2, 0, 0.4f, 0.1f));
			StateManager.RegisterInputDefinition(new InputKeyCodeDefinition(271, 3, 0, 0.4f, 0.1f));
			StateManager.RegisterInputDefinition(new InputKeyCodeDefinition(9, 4, 0, 0.4f, 0.1f));
		}

		private IEnumerator Start()
		{
			return Initialize();
		}

		private void Update()
		{
			Device.CheckScreenStateChanged();
		}

		private void OnDestroy()
		{
			LauncherConnection.Release();
			RuntimeData.Release();
			StateManager.UnregisterInputDefinition(1);
			StateManager.UnregisterInputDefinition(2);
			StateManager.UnregisterInputDefinition(3);
			StateManager.UnregisterInputDefinition(4);
		}

		private IEnumerator Initialize()
		{
			yield return LauncherConnection.InitializeConnection();
			if (!ApplicationStarter.InitializeAssetManager())
			{
				Log.Error("InitializeAssetManager: misssing AssetReferenceMap", 100, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Main.cs");
				Quit();
			}
			if (!ApplicationStarter.InitializeRuntimeData())
			{
				Log.Error("InitializeAssetManager: missing LocalizedTextData or BootTextCollection", 107, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Main.cs");
				Quit();
			}
			SplashState splashState = new SplashState();
			StateManager.GetDefaultLayer().SetChildState(splashState, 0);
			yield return ApplicationStarter.ReadBootConfig();
			if (!BootConfig.initialized)
			{
				yield return GoToCatastrophicFailureState(InitializationFailure.BootConfigInitialisation);
				yield break;
			}
			if (ApplicationConfig.showServerSelection)
			{
				ApplicationConfig.versionCheckResult = VersionChecker.Result.Success;
			}
			else
			{
				string text = ApplicationConfig.configUrl;
				if (string.IsNullOrEmpty(text))
				{
					text = BootConfig.remoteConfigUrl;
				}
				Log.Info("configUrl=" + text, 136, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Main.cs");
				yield return ApplicationStarter.ReadRemoteConfig(text);
				if (!ApplicationConfig.initialized)
				{
					yield return GoToInitializationFailedState(InitializationFailure.ApplicationConfigInitialisation);
					yield break;
				}
			}
			ApplicationConfig.PrintConfig();
			if (ApplicationConfig.versionCheckResult == VersionChecker.Result.None)
			{
				yield return ApplicationStarter.ReadVersion(ApplicationConfig.versionFileUrl);
			}
			if (!ApplicationConfig.IsVersionValid())
			{
				yield return GoToInitializationFailedState(InitializationFailure.UnvalidVersion);
				yield break;
			}
			this.StartCoroutine(CheckServerStatus());
			if (ApplicationConfig.haapiAllowed)
			{
				HaapiManager.Initialize();
			}
			string bundlesUrl = ApplicationConfig.bundlesUrl;
			bool patchAvailable = ApplicationConfig.versionCheckResult == VersionChecker.Result.PatchAvailable;
			yield return ApplicationStarter.ConfigureAssetManager(bundlesUrl, patchAvailable);
			if (!AssetManager.get_isReady())
			{
				yield return GoToInitializationFailedState(InitializationFailure.AssetManagerInitialisation);
				yield break;
			}
			if (patchAvailable)
			{
				yield return ApplicationStarter.CheckPatch();
			}
			yield return AudioManager.Load();
			PlayerPreferences.InitializeAudioPreference();
			yield return RuntimeData.Load();
			if (!RuntimeData.isReady)
			{
				yield return GoToInitializationFailedState(InitializationFailure.RuntimeDataInitialisation);
				yield break;
			}
			ConnectionHandler.Initialize();
			while (!m_serverStatusChecked)
			{
				yield return null;
			}
			StatesUtility.GotoLoginState();
		}

		private IEnumerator CheckServerStatus()
		{
			ServerStatus.StatusCode code = ApplicationConfig.serverStatus.code;
			if (ApplicationConfig.serverStatus.code == ServerStatus.StatusCode.Error)
			{
				yield return GoToInitializationFailedState(InitializationFailure.ServerStatusError);
			}
			else if (ApplicationConfig.serverStatus.code == ServerStatus.StatusCode.Maintenance)
			{
				yield return GoToInitializationFailedState(InitializationFailure.ServerStatusMaintenance);
			}
			else if (ApplicationConfig.serverStatus.code == ServerStatus.StatusCode.MaintenanceExpected)
			{
				m_serverStatusChecked = false;
				string text = ApplicationConfig.serverStatus.maintenanceStartTimeUtc.ToLocalTime().ToShortTimeString();
				string text2 = ApplicationConfig.serverStatus.maintenanceEstimatedEndTimeUtc.ToLocalTime().ToShortTimeString();
				PopupInfoManager.Show(StateManager.GetDefaultLayer().GetChainEnd(), new PopupInfo
				{
					title = 75142,
					message = new RawTextData(85153, text, text2),
					buttons = new ButtonData[1]
					{
						new ButtonData(27169, delegate
						{
							m_serverStatusChecked = true;
						})
					},
					selectedButton = 1,
					style = PopupStyle.Warning
				});
			}
			else
			{
				m_serverStatusChecked = true;
			}
		}

		private static IEnumerator GoToInitializationFailedState(InitializationFailure cause)
		{
			Log.Error("Switching to initialization failed state.", 280, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Main.cs");
			yield return AudioManager.Unload();
			yield return RuntimeData.Unload();
			if (AssetManagerError.op_Implicit(RuntimeData.error) != 0)
			{
				yield return GoToCatastrophicFailureState(InitializationFailure.RuntimeDataLoad);
				yield break;
			}
			yield return ApplicationStarter.ConfigureLocalAssetManager();
			if (!AssetManager.get_isReady())
			{
				yield return GoToCatastrophicFailureState(cause);
				yield break;
			}
			yield return AudioManager.Load();
			yield return RuntimeData.LoadOffline();
			InitializationFailedState initializationFailedState = new InitializationFailedState(cause);
			StateManager.GetDefaultLayer().GetChainEnd().SetChildState(initializationFailedState, 0);
		}

		private static IEnumerator GoToCatastrophicFailureState(InitializationFailure cause)
		{
			CatastrophicFailureState catastrophicFailureState = new CatastrophicFailureState(cause);
			StateManager.GetDefaultLayer().GetChainEnd().SetChildState(catastrophicFailureState, 0);
			yield break;
		}

		public Main()
			: this()
		{
		}
	}
}
