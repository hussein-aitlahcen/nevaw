using Ankama.AssetManagement;
using Ankama.Cube.Code.UI;
using Ankama.Cube.Configuration;
using Ankama.Cube.Network.Spin2;
using Ankama.Cube.Player;
using Ankama.Cube.TEMPFastEnterMatch.Auth;
using Ankama.Cube.TEMPFastEnterMatch.Auth.ConnectionMethod;
using Ankama.Cube.TEMPFastEnterMatch.Player;
using Ankama.Cube.UI.NicknameRequest;
using Ankama.Cube.Utility;
using Ankama.Utilities;
using Google.Protobuf;
using System;
using UnityEngine;

namespace Ankama.Cube.Network
{
	public class ConnectionHandler
	{
		public enum Status
		{
			Disconnected,
			Connecting,
			Connected
		}

		public delegate void ConnectionStatusChangedHandler(Status from, Status to);

		private static ConnectionHandler s_instance;

		private readonly CubeServerConnection m_connection;

		private BasicAccountLoadingHandler m_accountLoadingHandler;

		private GlobalFrame m_globalFrame;

		private Coroutine m_currentReconnectionCoroutine;

		private bool m_reconnecting;

		private int m_count;

		private Status m_currentStatus;

		private bool m_autoReconnect;

		private int m_currentPopupId = -1;

		public static ConnectionHandler instance => s_instance;

		public Status status => m_currentStatus;

		public bool autoReconnect
		{
			get
			{
				return m_autoReconnect;
			}
			set
			{
				m_autoReconnect = value;
			}
		}

		public static bool Initialized => s_instance != null;

		public IConnection<IMessage> connection => m_connection;

		public event ConnectionStatusChangedHandler OnConnectionStatusChanged;

		public static void Initialize()
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			if (s_instance == null)
			{
				Log.Info("Adding CubeServerConnection to scene.", 69, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Network\\ConnectionHandler.cs");
				CubeServerConnection cubeServerConnection = new GameObject("CubeServerConnection").AddComponent<CubeServerConnection>();
				Object.DontDestroyOnLoad(cubeServerConnection);
				s_instance = new ConnectionHandler(cubeServerConnection);
			}
		}

		public static void Destroy()
		{
			if (s_instance != null)
			{
				Log.Info("Destroy connection handler", 81, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Network\\ConnectionHandler.cs");
				s_instance.m_connection.Disconnect();
				Object.Destroy(s_instance.m_connection);
				s_instance = null;
			}
		}

		private ConnectionHandler(CubeServerConnection connection)
		{
			m_connection = connection;
			m_connection.OnConnectionOpened += OnConnectionOpened;
			m_connection.OnConnectionClosed += OnConnectionClosed;
			m_connection.OnOpenConnectionFailed += OnOpenConnectionFailed;
		}

		private void UpdateStatus(Status newStatus)
		{
			if (m_globalFrame != null && newStatus == Status.Disconnected)
			{
				m_globalFrame.Dispose();
				m_globalFrame = null;
			}
			this.OnConnectionStatusChanged?.Invoke(m_currentStatus, newStatus);
			m_currentStatus = newStatus;
			m_reconnecting = (m_currentStatus == Status.Connecting);
		}

		public void Connect()
		{
			if (m_globalFrame != null)
			{
				m_globalFrame.Dispose();
			}
			m_globalFrame = new GlobalFrame();
			UpdateStatus(Status.Connecting);
			Log.Info($"Connecting to {ApplicationConfig.gameServerHost}:{ApplicationConfig.gameServerPort}", 122, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Network\\ConnectionHandler.cs");
			m_connection.Connect(ApplicationConfig.gameServerHost, ApplicationConfig.gameServerPort);
		}

		public void Disconnect()
		{
			m_connection.Disconnect();
		}

		private void Reconnect()
		{
			if (!m_reconnecting)
			{
				m_count++;
				m_reconnecting = true;
				Log.Info($"[ConnectionHandler] Trying to reconnect. Retry {m_count}.", 142, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Network\\ConnectionHandler.cs");
				CreateReconnectionPopup();
				Connect();
			}
		}

		private void StopReconnection()
		{
			m_connection.Disconnect();
			ReleaseReconnection();
			PlayerPreferences.autoLogin = false;
			StatesUtility.GotoLoginState();
		}

		private void OnOpenConnectionFailed(IConnectionError obj)
		{
			UpdateStatus(Status.Disconnected);
			SpinConnectionError spinConnectionError = obj as SpinConnectionError;
			if (spinConnectionError != null)
			{
				string formattedText = TextCollectionUtility.AuthenticationErrorKeys.GetFormattedText(spinConnectionError.error);
				switch (spinConnectionError.error)
				{
				case SpinProtocol.ConnectionErrors.NickNameRequired:
					RequestNickname();
					break;
				case SpinProtocol.ConnectionErrors.BadCredentials:
				case SpinProtocol.ConnectionErrors.InvalidAuthenticationInfo:
				case SpinProtocol.ConnectionErrors.SubscriptionRequired:
				case SpinProtocol.ConnectionErrors.AdminRightsRequired:
				case SpinProtocol.ConnectionErrors.AccountKnonwButBanned:
				case SpinProtocol.ConnectionErrors.AccountKnonwButBlocked:
				case SpinProtocol.ConnectionErrors.BetaAccessRequired:
					CreateDisconnectedPopup(formattedText, DisconnectionStrategy.ReturnToLoginAndChangeAccount);
					break;
				case SpinProtocol.ConnectionErrors.ServerTimeout:
				case SpinProtocol.ConnectionErrors.ServerError:
				case SpinProtocol.ConnectionErrors.AccountsBackendError:
					CreateDisconnectedPopup(formattedText, DisconnectionStrategy.ReturnToLogin);
					break;
				case SpinProtocol.ConnectionErrors.IpAddressRefused:
					CreateDisconnectedPopup(formattedText, DisconnectionStrategy.QuitApplication);
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
				return;
			}
			ConnectionInterruptedError connectionInterruptedError = obj as ConnectionInterruptedError;
			if (connectionInterruptedError != null)
			{
				ServerDisconnectionInfo serverDisconnectionInfo = connectionInterruptedError.disconnectionInfo as ServerDisconnectionInfo;
				if (serverDisconnectionInfo != null)
				{
					Log.Info($"Disconnection occured during authentication {connectionInterruptedError.disconnectionInfo}", 209, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Network\\ConnectionHandler.cs");
					string formattedText2 = TextCollectionUtility.DisconnectionReasonKeys.GetFormattedText(serverDisconnectionInfo.reason);
					CreateDisconnectedPopup(formattedText2, DisconnectionStrategy.ReturnToLogin);
				}
				else if (connectionInterruptedError.disconnectionInfo is NetworkDisconnectionInfo)
				{
					if (m_autoReconnect)
					{
						Reconnect();
						return;
					}
					Log.Info($"Disconnection occured during authentication {connectionInterruptedError.disconnectionInfo}", 224, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Network\\ConnectionHandler.cs");
					string cause = RuntimeData.FormattedText(94930);
					CreateDisconnectedPopup(cause, DisconnectionStrategy.ReturnToLogin);
				}
				return;
			}
			NetworkConnectionError networkConnectionError = obj as NetworkConnectionError;
			if (networkConnectionError != null)
			{
				if (m_autoReconnect)
				{
					Reconnect();
					return;
				}
				Log.Info($"Error occured during authentication {networkConnectionError.exception}", 244, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Network\\ConnectionHandler.cs");
				string cause2 = RuntimeData.FormattedText(34942);
				CreateDisconnectedPopup(cause2, DisconnectionStrategy.ReturnToLogin);
			}
			else
			{
				Log.Info($"Error while connecting: {obj}", 251, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Network\\ConnectionHandler.cs");
				string cause3 = RuntimeData.FormattedText(36698, obj.ToString());
				CreateDisconnectedPopup(cause3, DisconnectionStrategy.ReturnToLogin);
			}
		}

		private void OnConnectionOpened()
		{
			m_count = 0;
			UpdateStatus(Status.Connected);
			if (m_accountLoadingHandler == null)
			{
				m_accountLoadingHandler = new BasicAccountLoadingHandler();
			}
			m_accountLoadingHandler.LoadAccount();
			ReleaseReconnection();
		}

		private void OnConnectionClosed(IDisconnectionInfo obj)
		{
			UpdateStatus(Status.Disconnected);
			ServerDisconnectionInfo serverDisconnectionInfo = obj as ServerDisconnectionInfo;
			if (serverDisconnectionInfo != null)
			{
				string formattedText = TextCollectionUtility.DisconnectionReasonKeys.GetFormattedText(serverDisconnectionInfo.reason);
				CreateDisconnectedPopup(formattedText, DisconnectionStrategy.ReturnToLogin);
			}
			else if (!(obj is ClientDisconnectionInfo))
			{
				if (obj is NetworkDisconnectionInfo)
				{
					Reconnect();
					return;
				}
				Log.Error($"Connection closed for unknown reason: {obj}. Leaving application.", 298, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Network\\ConnectionHandler.cs");
				string cause = RuntimeData.FormattedText(36698, obj.ToString());
				CreateDisconnectedPopup(cause, DisconnectionStrategy.QuitApplication);
			}
		}

		private void CreateDisconnectedPopup(string cause, DisconnectionStrategy errorStrategy)
		{
			bool flag = false;
			AutoConnectLevel autoConnectLevel = CredentialProvider.gameCredentialProvider.AutoConnectLevel();
			Action onClick;
			switch (errorStrategy)
			{
			case DisconnectionStrategy.ReturnToLogin:
				if (autoConnectLevel == AutoConnectLevel.Mandatory)
				{
					onClick = delegate
					{
						CredentialProvider.gameCredentialProvider.CleanCredentials();
						Main.Quit();
					};
					flag = true;
				}
				else
				{
					onClick = delegate
					{
						PlayerPreferences.autoLogin = false;
						StatesUtility.GotoLoginState();
					};
				}
				break;
			case DisconnectionStrategy.ReturnToLoginAndChangeAccount:
				if (autoConnectLevel == AutoConnectLevel.Mandatory)
				{
					onClick = delegate
					{
						CredentialProvider.gameCredentialProvider.CleanCredentials();
						Main.Quit();
					};
					flag = true;
				}
				else
				{
					onClick = delegate
					{
						CredentialProvider.gameCredentialProvider.CleanCredentials();
						StatesUtility.GotoLoginState();
					};
				}
				break;
			case DisconnectionStrategy.QuitApplication:
				onClick = Main.Quit;
				flag = true;
				break;
			default:
				throw new ArgumentOutOfRangeException("errorStrategy", errorStrategy, null);
			}
			ButtonData[] buttons = (!flag) ? new ButtonData[2]
			{
				new ButtonData(27169, onClick),
				new ButtonData(75192, Main.Quit)
			} : new ButtonData[1]
			{
				new ButtonData(75192, onClick)
			};
			PopupInfoManager.ClearAllMessages();
			PopupInfo popupInfo = default(PopupInfo);
			popupInfo.title = 20267;
			popupInfo.message = cause;
			popupInfo.buttons = buttons;
			popupInfo.selectedButton = 1;
			PopupInfoManager.ShowApplicationError(popupInfo);
		}

		private void CreateReconnectionPopup()
		{
			if (m_currentPopupId < 0)
			{
				RawTextData message;
				ButtonData[] buttons;
				if (CredentialProvider.gameCredentialProvider.AutoConnectLevel() == AutoConnectLevel.Mandatory)
				{
					message = 34942;
					buttons = new ButtonData[1]
					{
						new ButtonData(75192, delegate
						{
							CredentialProvider.gameCredentialProvider.CleanCredentials();
							Main.Quit();
						})
					};
				}
				else
				{
					message = 30166;
					buttons = new ButtonData[1]
					{
						new ButtonData(59515, delegate
						{
							StopReconnection();
							m_currentPopupId = -1;
						})
					};
				}
				m_currentPopupId = PopupInfoManager.Show(StateManager.GetDefaultLayer().GetChainEnd(), new PopupInfo
				{
					title = 20267,
					message = message,
					buttons = buttons,
					selectedButton = 1,
					style = PopupStyle.Error
				});
			}
		}

		private void RequestNickname()
		{
			NicknameRequestState nicknameRequestState = new NicknameRequestState();
			nicknameRequestState.OnSuccess += OnNicknameUpdated;
			StateManager.GetDefaultLayer().GetChainEnd().SetChildState(nicknameRequestState, 0);
		}

		private void OnNicknameUpdated(string nickname)
		{
			PlayerData.instance?.UpdateNickname(nickname);
		}

		private void ReleaseReconnection()
		{
			if (m_currentPopupId >= 0)
			{
				PopupInfoManager.RemoveById(m_currentPopupId);
				m_currentPopupId = -1;
			}
			m_reconnecting = false;
			m_count = 0;
		}

		public void Dispose()
		{
			if (m_globalFrame != null)
			{
				m_globalFrame.Dispose();
				m_globalFrame = null;
			}
			m_connection.Disconnect();
			ReleaseReconnection();
		}
	}
}
