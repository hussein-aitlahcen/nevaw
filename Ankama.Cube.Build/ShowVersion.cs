using Ankama.Cube.Configuration;
using Ankama.Cube.UI.Components;
using System;
using System.Collections;
using UnityEngine;

namespace Ankama.Cube.Build
{
	public sealed class ShowVersion : MonoBehaviour
	{
		[SerializeField]
		private RawTextField m_textVersion;

		[SerializeField]
		private GameObject m_versionDemo;

		private void Awake()
		{
			if (null != m_textVersion)
			{
				m_textVersion.set_enabled(false);
			}
		}

		private IEnumerator Start()
		{
			while (!RuntimeData.isReady)
			{
				yield return null;
			}
			bool simulateDemo = ApplicationConfig.simulateDemo;
			if (null != m_textVersion)
			{
				m_textVersion.set_enabled(!simulateDemo);
				ApplicationConfig.OnServerConfigLoaded = (Action)Delegate.Combine(ApplicationConfig.OnServerConfigLoaded, new Action(RefreshText));
				RefreshText();
			}
			if (null != m_versionDemo)
			{
				m_versionDemo.SetActive(simulateDemo);
			}
		}

		public void RefreshText()
		{
			string str = ApplicationConfig.gameServerIsLocal ? "-internal" : "";
			m_textVersion.SetText("0.1.0.6045" + str + GetServerProfile());
		}

		private static string GetServerProfile()
		{
			switch (ApplicationConfig.gameServerProfile)
			{
			case RemoteConfig.ServerProfile.Development:
				return "-dev";
			case RemoteConfig.ServerProfile.Beta:
				return "-alpha";
			case RemoteConfig.ServerProfile.Production:
				return "";
			default:
				return "";
			}
		}

		public ShowVersion()
			: this()
		{
		}
	}
}
