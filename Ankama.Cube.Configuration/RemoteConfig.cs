using JetBrains.Annotations;
using System;
using UnityEngine;

namespace Ankama.Cube.Configuration
{
	public class RemoteConfig
	{
		public enum ServerProfile
		{
			None,
			[UsedImplicitly]
			Development,
			[UsedImplicitly]
			Beta,
			[UsedImplicitly]
			Production
		}

		public string gameServerDisplayName
		{
			get;
			private set;
		} = string.Empty;


		public string gameServerHost
		{
			get;
			private set;
		} = string.Empty;


		public int gameServerPort
		{
			get;
			private set;
		} = -1;


		public int gameAppId
		{
			get;
			private set;
		} = -1;


		public int chatAppId
		{
			get;
			private set;
		} = -1;


		public bool gameServerIsLocal
		{
			get;
			private set;
		}

		public ServerProfile gameServerProfile
		{
			get;
			private set;
		}

		public string bundlesUrl
		{
			get;
			private set;
		} = string.Empty;


		public string versionFileUrl
		{
			get;
			private set;
		} = string.Empty;


		public string haapiServerUrl
		{
			get;
			private set;
		} = string.Empty;


		public static RemoteConfig From(ConfigReader reader)
		{
			if (reader == null)
			{
				return null;
			}
			RemoteConfig remoteConfig = new RemoteConfig();
			remoteConfig.gameServerHost = reader.GetString("gameServerHost");
			remoteConfig.gameServerPort = reader.GetInt("gameServerPort", -1);
			remoteConfig.gameAppId = reader.GetInt("gameAppId", -1);
			remoteConfig.chatAppId = reader.GetInt("chatAppId", -1);
			remoteConfig.gameServerIsLocal = reader.GetBool("gameServerIsLocal");
			remoteConfig.gameServerProfile = reader.GetEnum("gameServerProfile", ServerProfile.None);
			remoteConfig.versionFileUrl = ReplaceVars(reader.GetUrl("versionFileUrl"));
			remoteConfig.haapiServerUrl = reader.GetString("haapiServerUrl");
			if (reader.HasProperty("gameServerDisplayName"))
			{
				remoteConfig.gameServerDisplayName = reader.GetString("gameServerDisplayName");
			}
			else
			{
				remoteConfig.gameServerDisplayName = remoteConfig.gameServerProfile.ToString();
			}
			remoteConfig.bundlesUrl = ReplaceVars(reader.GetUrl("bundlesUrl"));
			return remoteConfig;
		}

		public static string ReplaceVars(string txt)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected I4, but got Unknown
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Invalid comparison between Unknown and I4
			RuntimePlatform platform = Application.get_platform();
			string newValue;
			switch ((int)platform)
			{
			default:
				if ((int)platform != 11)
				{
					goto case 3;
				}
				newValue = "android";
				break;
			case 0:
			case 1:
				newValue = "macosx";
				break;
			case 2:
			case 7:
				newValue = "windows";
				break;
			case 8:
				newValue = "ios";
				break;
			case 3:
			case 4:
			case 5:
			case 6:
				throw new ArgumentOutOfRangeException("platform", "Unsupported platform.");
			}
			return txt.Replace("$%7Bplatform%7D", newValue).Replace("$%7Bversion%7D", "0.1.0.6045").Replace("$%7Bbuild%7D", 6045.ToString());
		}
	}
}
