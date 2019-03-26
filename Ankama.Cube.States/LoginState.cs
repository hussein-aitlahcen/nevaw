using Ankama.AssetManagement;
using Ankama.AssetManagement.StateManagement;
using Ankama.Cube.Configuration;
using Ankama.Cube.Network;
using Ankama.Cube.Player;
using Ankama.Cube.TEMPFastEnterMatch.Auth;
using Ankama.Cube.TEMPFastEnterMatch.Auth.ConnectionMethod;
using Ankama.Cube.TEMPFastEnterMatch.Player;
using Ankama.Cube.Utility;
using Ankama.Launcher;
using Ankama.Utilities;
using System.Collections;
using UnityEngine;

namespace Ankama.Cube.States
{
	public class LoginState : StateContext
	{
		protected override IEnumerator Load()
		{
			ConnectionHandler.instance.autoReconnect = false;
			PlayerData.OnPlayerDataInitialized += OnPlayerDataInitialized;
			yield return AwaitLauncherConnection();
			yield return OpenLoginOrAutoConnect();
		}

		protected override void Enable()
		{
		}

		protected override void Disable()
		{
		}

		protected override IEnumerator Unload()
		{
			ConnectionHandler.instance.autoReconnect = true;
			PlayerData.OnPlayerDataInitialized -= OnPlayerDataInitialized;
			return this.Unload();
		}

		private void OnPlayerDataInitialized(bool pendingFightFound)
		{
			PlayerData.OnPlayerDataInitialized -= OnPlayerDataInitialized;
			if (pendingFightFound)
			{
				Log.Info("Pending fight found. Going to fight mode", 58, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\States\\LoginState.cs");
				StatesUtility.DoTransition(new ReconnectingFightState(), StateManager.GetDefaultLayer().GetChildState());
				return;
			}
			LoginUI.StartState startState = (LoginUI.StartState)PlayerPreferences.startState;
			switch (startState)
			{
			case LoginUI.StartState.HavreDimension:
				StatesUtility.GotoDimensionState();
				break;
			case LoginUI.StartState.MatchMaking:
				StatesUtility.GotoMatchMakingState();
				break;
			default:
				Log.Warning($"Start in state '{startState}' unknown: goto Dimension state", 77, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\States\\LoginState.cs");
				StatesUtility.GotoDimensionState();
				break;
			}
		}

		private void OnPlayerAccountLoadingErrorEvent()
		{
		}

		private void OnAccountReady()
		{
		}

		private IEnumerator OpenLoginOrAutoConnect()
		{
			if (CredentialProvider.gameCredentialProvider.HasGuestMode() && CredentialProvider.gameCredentialProvider.LoginUIType() == LoginUIType.Guest)
			{
				return OpenLoginUICoroutine();
			}
			return OpenSelectLoginUI();
		}

		private IEnumerator Connect()
		{
			RemoteConfig.ServerProfile gameServerProfile = ApplicationConfig.gameServerProfile;
			ConnectionHandler.instance.Connect();
			yield break;
		}

		private IEnumerator OpenZaapRequiredState()
		{
			this.SetChildState(new ZaapRequiredState(), 0);
			yield break;
		}

		private IEnumerator OpenSelectLoginUI()
		{
			this.SetChildState(new SelectLoginUIState(), 0);
			yield break;
		}

		private IEnumerator OpenLoginUICoroutine()
		{
			this.SetChildState(new LoginUIState(), 0);
			yield break;
		}

		private static IEnumerator AwaitLauncherConnection()
		{
			ILauncherLink conn = LauncherConnection.instance;
			if (conn == null)
			{
				yield return null;
				yield break;
			}
			WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
			while (conn.get_opening())
			{
				yield return waitForEndOfFrame;
			}
		}

		public LoginState()
			: this()
		{
		}
	}
}
