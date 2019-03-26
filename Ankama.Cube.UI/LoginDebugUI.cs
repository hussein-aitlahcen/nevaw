using Ankama.Cube.Configuration;
using Ankama.Cube.Player;
using Ankama.Cube.UI.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Ankama.Cube.UI
{
	public class LoginDebugUI : MonoBehaviour
	{
		[SerializeField]
		private CustomDropdown m_serversDropdown;

		[SerializeField]
		private ServerList m_serverList;

		private void Awake()
		{
			this.get_gameObject().SetActive(ApplicationConfig.showServerSelection);
		}

		private void Start()
		{
			InitializeServerList();
		}

		private void OnSelectServer(int serverIndex)
		{
			ServerList.ServerInfo serverInfo = m_serverList.GetServerInfo(serverIndex);
			ApplicationConfig.SetServerInfo(serverInfo);
			ApplicationConfig.versionCheckResult = VersionChecker.Result.Success;
			PlayerPreferences.lastServer = serverInfo.displayName;
			PlayerPreferences.Save();
		}

		private unsafe void InitializeServerList()
		{
			List<string> list = (from s in m_serverList.GetAllServers()
				select s.displayName).ToList();
			m_serversDropdown.AddOptions(list);
			string defaultServer = PlayerPreferences.lastServer;
			int num = list.FindIndex((string s) => s == defaultServer);
			if (num < 0)
			{
				num = 0;
			}
			m_serversDropdown.value = num;
			OnSelectServer(num);
			m_serversDropdown.onValueChanged.AddListener(new UnityAction<int>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		}

		public LoginDebugUI()
			: this()
		{
		}
	}
}
