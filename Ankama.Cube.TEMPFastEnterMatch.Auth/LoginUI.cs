using Ankama.Cube.Configuration;
using Ankama.Cube.Extensions;
using Ankama.Cube.Player;
using Ankama.Cube.TEMPFastEnterMatch.Auth.ConnectionMethod;
using Ankama.Cube.UI;
using Ankama.Cube.UI.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ankama.Cube.TEMPFastEnterMatch.Auth
{
	public class LoginUI : AbstractUI
	{
		public enum StartState
		{
			MatchMaking,
			HavreDimension
		}

		[SerializeField]
		private InputTextField m_login;

		[SerializeField]
		private InputTextField m_password;

		[SerializeField]
		private Button m_loginButton;

		[SerializeField]
		private Button m_backToGuestButton;

		[SerializeField]
		private CustomDropdown m_serversDropdown;

		[SerializeField]
		private CustomDropdown m_startStateDropDown;

		[SerializeField]
		private ServerList m_serverList;

		[NonSerialized]
		public Action connect;

		[NonSerialized]
		public Action onBackToGuest;

		private int m_selectedIndex;

		public ServerList serverList => m_serverList;

		public string login => m_login.GetText();

		public string password => m_password.GetText();

		public int serverIndex => m_serversDropdown.value;

		protected unsafe override void Awake()
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Expected O, but got Unknown
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected O, but got Unknown
			m_backToGuestButton.get_gameObject().SetActive(CredentialProvider.HasGuestMode());
			m_loginButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_backToGuestButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_startStateDropDown.AddOptions((from s in EnumUtility.GetValues<StartState>()
				select s.ToString()).ToList());
			m_startStateDropDown.value = PlayerPreferences.startState;
			m_startStateDropDown.onValueChanged.AddListener(new UnityAction<int>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			PlayerPreferences.useGuest = false;
		}

		public void SelectNext()
		{
			m_selectedIndex = (m_selectedIndex + 1) % 2;
			switch (m_selectedIndex)
			{
			case 0:
				m_login.selectable.Select();
				break;
			case 1:
				m_password.selectable.Select();
				break;
			}
		}

		private void OnBackToGuestClick()
		{
			onBackToGuest?.Invoke();
		}

		public void SetParams(string login, string password, string serverId)
		{
			m_login.SetText(PlayerPreferences.lastLogin);
			m_password.SetText(PlayerPreferences.lastPassword);
			InitializeServerList(serverId);
			m_login.selectable.Select();
		}

		private void OnLoginButtonClicked()
		{
			PlayerPreferences.lastLogin = m_login.GetText();
			PlayerPreferences.lastPassword = m_password.GetText();
			PlayerPreferences.Save();
			connect?.Invoke();
		}

		private void OnSelectServer(int serverIndex)
		{
			ServerList.ServerInfo serverInfo = m_serverList.GetServerInfo(serverIndex);
			ApplicationConfig.SetServerInfo(serverInfo);
			ApplicationConfig.versionCheckResult = VersionChecker.Result.Success;
			PlayerPreferences.lastServer = serverInfo.displayName;
			PlayerPreferences.Save();
		}

		private void OnStartStateChanged(int stateIndex)
		{
			PlayerPreferences.startState = stateIndex;
			PlayerPreferences.Save();
		}

		private void InitializeServerList(string defaultServer)
		{
			List<string> list = (from s in m_serverList.GetAllServers()
				select s.displayName).ToList();
			m_serversDropdown.AddOptions(list);
			int value = Math.Max(0, list.FindIndex((string s) => s == defaultServer));
			m_serversDropdown.value = value;
		}
	}
}
