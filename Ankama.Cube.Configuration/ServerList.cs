using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Configuration
{
	[CreateAssetMenu]
	public class ServerList : ScriptableObject
	{
		[Serializable]
		public struct ServerInfo
		{
			[SerializeField]
			private string m_displayName;

			[SerializeField]
			private string m_host;

			[SerializeField]
			private int m_port;

			[SerializeField]
			private bool m_isLocal;

			[SerializeField]
			private RemoteConfig.ServerProfile m_profile;

			[SerializeField]
			private int m_gameAppId;

			[SerializeField]
			private int m_chatAppId;

			public string displayName => m_displayName;

			public string host => m_host;

			public int port => m_port;

			public bool isLocal => m_isLocal;

			public RemoteConfig.ServerProfile profile => m_profile;

			public int gameAppId => m_gameAppId;

			public int chatAppId => m_chatAppId;
		}

		[SerializeField]
		private ServerInfo[] m_servers;

		public IReadOnlyList<ServerInfo> GetAllServers()
		{
			return m_servers;
		}

		public ServerInfo GetServerInfo(int index)
		{
			return m_servers[index];
		}

		public ServerList()
			: this()
		{
		}
	}
}
