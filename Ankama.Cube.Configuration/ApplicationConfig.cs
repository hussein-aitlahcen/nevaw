using Ankama.Utilities;
using JetBrains.Annotations;
using System;
using System.IO;
using UnityEngine;

namespace Ankama.Cube.Configuration
{
	public static class ApplicationConfig
	{
		private static readonly Options s_options;

		public static Action OnServerConfigLoaded;

		public static VersionChecker.Result versionCheckResult;

		public static ServerStatus serverStatus;

		private static int m_gameAppId;

		private static int m_chatAppId;

		public static string persistentArgsFile => Application.get_persistentDataPath() + "/commandline.txt";

		public static bool initialized
		{
			get;
			private set;
		}

		public static int GameAppId
		{
			get
			{
				if (m_gameAppId == -1)
				{
					throw new Exception("GameAppId not initialized");
				}
				return m_gameAppId;
			}
			private set
			{
				m_gameAppId = value;
			}
		}

		public static int ChatAppId
		{
			get
			{
				if (m_chatAppId == -1)
				{
					throw new Exception("ChatAppId not initialized");
				}
				return m_chatAppId;
			}
			private set
			{
				m_chatAppId = value;
			}
		}

		[NotNull]
		public static string gameServerHost
		{
			get;
			private set;
		}

		public static int gameServerPort
		{
			get;
			private set;
		}

		public static bool gameServerIsLocal
		{
			get;
			private set;
		}

		public static RemoteConfig.ServerProfile gameServerProfile
		{
			get;
			private set;
		}

		[NotNull]
		public static string bundlesUrl
		{
			get;
			private set;
		}

		[NotNull]
		public static string versionFileUrl
		{
			get;
			private set;
		}

		[NotNull]
		public static string haapiServerUrl
		{
			get;
			private set;
		}

		[NotNull]
		public static string langCode
		{
			get;
			private set;
		}

		public static bool debugMode
		{
			get;
			private set;
		}

		public static bool haapiAllowed
		{
			get;
			private set;
		}

		public static bool simulateDemo
		{
			get;
			private set;
		}

		public static bool showServerSelection
		{
			get;
			private set;
		}

		[NotNull]
		public static string configUrl
		{
			get;
			private set;
		}

		static ApplicationConfig()
		{
			s_options = new Options();
			versionCheckResult = VersionChecker.Result.None;
			serverStatus = ServerStatus.none;
			m_gameAppId = 22;
			m_chatAppId = 99;
			gameServerHost = string.Empty;
			gameServerPort = -1;
			gameServerIsLocal = false;
			bundlesUrl = string.Empty;
			versionFileUrl = string.Empty;
			haapiServerUrl = string.Empty;
			langCode = string.Empty;
			haapiAllowed = true;
			configUrl = string.Empty;
			s_options.Register("langCode", delegate(string v)
			{
				langCode = v.ToUpper();
			}, "FR|EN");
			s_options.Register("debug", delegate(bool v)
			{
				debugMode = v;
			});
			s_options.Register("no-haapi", delegate(bool v)
			{
				haapiAllowed = !v;
			});
			s_options.Register("forceValidVersion", delegate
			{
				versionCheckResult = VersionChecker.Result.Success;
			});
			s_options.Register("demo", delegate(bool v)
			{
				simulateDemo = v;
			});
			s_options.Register("showServerSelection", delegate(bool v)
			{
				showServerSelection = v;
			});
			s_options.Register("configUrl", delegate(string v)
			{
				configUrl = v.Trim('"', ' ', '\t');
			}, "file|url://path/to/config.json");
		}

		public static void Read()
		{
			ReadFromPersistentData();
			ReadFromCommandLine();
			if (showServerSelection)
			{
				initialized = true;
			}
		}

		private static void ReadFromPersistentData()
		{
			string persistentArgsFile = ApplicationConfig.persistentArgsFile;
			Log.Info("Read arguments from " + persistentArgsFile, 160, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Configuration\\ApplicationConfig.cs");
			try
			{
				if (File.Exists(persistentArgsFile))
				{
					ReadFromArguments(File.ReadAllLines(persistentArgsFile));
				}
			}
			catch (Exception ex)
			{
				Log.Warning((object)ex, 171, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Configuration\\ApplicationConfig.cs");
			}
		}

		private static void ReadFromCommandLine()
		{
			ReadFromArguments(Environment.GetCommandLineArgs());
		}

		private static void ReadFromArguments(string[] args)
		{
			for (int i = 0; i < args.Length; i++)
			{
				string text = args[i];
				if (!string.IsNullOrEmpty(text))
				{
					s_options.ParseArgument(text.Trim(), ref i);
				}
			}
		}

		public static string Usage()
		{
			return s_options.Usage();
		}

		public static void SetFromRemoteConfig(RemoteConfig config)
		{
			gameServerHost = config.gameServerHost;
			gameServerPort = config.gameServerPort;
			gameServerIsLocal = config.gameServerIsLocal;
			gameServerProfile = config.gameServerProfile;
			GameAppId = config.gameAppId;
			ChatAppId = config.chatAppId;
			bundlesUrl = config.bundlesUrl;
			versionFileUrl = config.versionFileUrl;
			haapiServerUrl = config.haapiServerUrl;
			initialized = true;
			OnServerConfigLoaded?.Invoke();
		}

		public static void SetServerInfo(ServerList.ServerInfo info)
		{
			gameServerHost = info.host;
			gameServerPort = info.port;
			gameServerIsLocal = info.isLocal;
			gameServerProfile = info.profile;
			GameAppId = info.gameAppId;
			ChatAppId = info.chatAppId;
			initialized = true;
			OnServerConfigLoaded?.Invoke();
		}

		public static bool IsVersionValid()
		{
			switch (versionCheckResult)
			{
			case VersionChecker.Result.Success:
			case VersionChecker.Result.PatchAvailable:
				return true;
			case VersionChecker.Result.UpdateNeeded:
			case VersionChecker.Result.VersionFileError:
			case VersionChecker.Result.RuntimeError:
				return false;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		public static void PrintConfig()
		{
			Log.Info($"Config: {{\ndebugMode: '{debugMode}'\nhaapiAllowed: '{haapiAllowed}'\nlangCode: '{langCode}'\nconfigUrl: '{configUrl}'\nbundlesUrl: '{bundlesUrl}'\nversionFileUrl: '{versionFileUrl}'\nhaapiServerUrl: '{haapiServerUrl}'\nGameAppId: '{GameAppId}'\nChatAppId: '{ChatAppId}'\ngameServerHost: '{gameServerHost}'\ngameServerPort: '{gameServerPort}'\ngameServerIsLocal: '{gameServerIsLocal}'\ngameServerProfile: '{gameServerProfile}'\n}}", 268, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Configuration\\ApplicationConfig.cs");
		}
	}
}
