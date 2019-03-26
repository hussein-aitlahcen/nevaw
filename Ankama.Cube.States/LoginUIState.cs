using Ankama.AssetManagement.InputManagement;
using Ankama.Cube.Configuration;
using Ankama.Cube.Network;
using Ankama.Cube.Player;
using Ankama.Cube.TEMPFastEnterMatch.Auth;
using Ankama.Cube.TEMPFastEnterMatch.Auth.ConnectionMethod;
using Ankama.Cube.Utility;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Ankama.Cube.States
{
	public class LoginUIState : LoadSceneStateContext
	{
		private LoginUI m_ui;

		protected override IEnumerator Load()
		{
			UILoader<LoginUI> loader = new UILoader<LoginUI>(this, "LoginUI", "core/scenes/ui/login");
			yield return loader.Load();
			m_ui = loader.ui;
			m_ui.SetParams(PlayerPreferences.lastLogin, PlayerPreferences.lastPassword, PlayerPreferences.lastServer);
		}

		protected override void Enable()
		{
			m_ui.connect = TryToConnect;
			m_ui.onBackToGuest = OnBackToGuest;
			ConnectionHandler.instance.OnConnectionStatusChanged += OnConnectionStatusChanged;
		}

		protected unsafe override bool UseInput(InputState inputState)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			if ((int)((IntPtr)(void*)inputState).state != 1)
			{
				return this.UseInput(inputState);
			}
			switch (((IntPtr)(void*)inputState).id)
			{
			case 2:
			case 3:
				TryToConnect();
				return true;
			case 4:
				m_ui.SelectNext();
				return true;
			default:
				return this.UseInput(inputState);
			}
		}

		private void OnConnectionStatusChanged(ConnectionHandler.Status from, ConnectionHandler.Status to)
		{
			switch (to)
			{
			case ConnectionHandler.Status.Disconnected:
				if (null != m_ui)
				{
					m_ui.interactable = true;
				}
				break;
			case ConnectionHandler.Status.Connecting:
			case ConnectionHandler.Status.Connected:
				if (null != m_ui)
				{
					m_ui.interactable = false;
				}
				break;
			default:
				throw new ArgumentOutOfRangeException("to", to, null);
			}
		}

		protected override void Disable()
		{
			m_ui.connect = null;
			m_ui.onBackToGuest = null;
			m_ui.get_gameObject().SetActive(false);
			ConnectionHandler.instance.OnConnectionStatusChanged -= OnConnectionStatusChanged;
		}

		private void OnBackToGuest()
		{
			StatesUtility.DoTransition(new SelectLoginUIState(), this);
		}

		private void TryToConnect()
		{
			m_ui.interactable = false;
			PlayerPreferences.lastLogin = m_ui.login;
			PlayerPreferences.lastPassword = m_ui.password;
			ServerList.ServerInfo serverInfo = m_ui.serverList.GetServerInfo(m_ui.serverIndex);
			ApplicationConfig.SetServerInfo(serverInfo);
			ApplicationConfig.versionCheckResult = VersionChecker.Result.Success;
			PlayerPreferences.lastServer = serverInfo.displayName;
			PlayerPreferences.Save();
			DoConnect();
		}

		private void DoConnect()
		{
			ApplicationConfig.PrintConfig();
			ReinitConnections();
			m_ui.interactable = false;
			ConnectionHandler.instance.Connect();
		}

		private void ReinitConnections()
		{
			CredentialProvider.DeteteCredentialProviders();
			ConnectionHandler.instance.OnConnectionStatusChanged -= OnConnectionStatusChanged;
			ConnectionHandler.Destroy();
			if (ApplicationConfig.haapiAllowed)
			{
				HaapiManager.Initialize();
			}
			ConnectionHandler.Initialize();
			ConnectionHandler.instance.OnConnectionStatusChanged += OnConnectionStatusChanged;
		}

		private void OnNicknameResult(bool success, IList<string> suggests, string key, string text)
		{
		}

		private void OnNicknameRequest()
		{
		}
	}
}
