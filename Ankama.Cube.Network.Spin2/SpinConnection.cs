using Ankama.Cube.Network.Spin2.Layers;
using Ankama.Cube.Protocols.ServerProtocol;
using Ankama.Utilities;
using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Ankama.Cube.Network.Spin2
{
	public class SpinConnection<T> : MonoBehaviour, IConnection<T> where T : class
	{
		private enum Status
		{
			Disconnected,
			Connecting,
			Connected,
			Destroyed
		}

		public const int DefaultMaximumMessageSize = 131072;

		public const int SpinHeaderSizeBytes = 4;

		public const bool SpinHeaderSizeContainsItself = true;

		public const bool SpinIsBigIndian = true;

		public const int ReceiveBufferSize = 8092;

		public const float HeartbeatDelay = 5f;

		private readonly SpinTransportLayer m_spinTransportLayer;

		private readonly ISpinCredentialsProvider m_credentialsProvider;

		private readonly ApplicationCodec<T> m_codec;

		private Status m_status;

		private TaskCompletionSource<IConnectionError> m_authenticationTaskSource;

		private TaskCompletionSource<IDisconnectionInfo> m_disconnectionTaskSource;

		private BlockingCollection<T> m_messagesFifo;

		private IConnectionError m_authenticationErrorToDispatch;

		private IDisconnectionInfo m_disconnectionInfoToDispatch;

		private float m_timeElapsedWithoutWriting;

		public event Action<T> OnApplicationMessage;

		public event Action<IConnectionError> OnOpenConnectionFailed;

		public event Action OnConnectionOpened;

		public event Action<IDisconnectionInfo> OnConnectionClosed;

		public SpinConnection(ISpinCredentialsProvider credentialsProvider, INetworkLayer<byte[]> underlyingTransportLayer, ApplicationCodec<T> codec, int maximumMessageSize = 131072)
			: this()
		{
			m_credentialsProvider = credentialsProvider;
			m_spinTransportLayer = new SpinTransportLayer(new FrameDelimiter(underlyingTransportLayer, maximumMessageSize, 4, includeHeaderSize: true));
			m_codec = codec;
			m_status = Status.Disconnected;
		}

		public void Connect(string host, int port)
		{
			if (m_status != 0)
			{
				throw new Exception($"Unable to connect SpinConnection: current status is {m_status}.");
			}
			ConnectAsync(host, port);
		}

		public void Disconnect()
		{
			if (m_status == Status.Connected || m_status == Status.Connecting)
			{
				DisconnectWithInfo(new ClientDisconnectionInfo());
			}
		}

		public void DisconnectByServer(DisconnectedByServerEvent evt)
		{
			if (m_status == Status.Connected || m_status == Status.Connecting)
			{
				DisconnectWithInfo(new ServerDisconnectionInfo(evt.Reason));
			}
		}

		public void Write(T message)
		{
			if (m_status == Status.Connected)
			{
				if (m_codec.TrySerialize(message, out byte[] result))
				{
					SendRawApplicationData(result);
				}
				else
				{
					Log.Error($"Unable to serialize application message {message}", 105, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Network\\Spin2\\SpinConnection.cs");
				}
			}
		}

		public void Update()
		{
			if (m_authenticationErrorToDispatch != null)
			{
				this.OnOpenConnectionFailed?.Invoke(m_authenticationErrorToDispatch);
				m_authenticationErrorToDispatch = null;
			}
			if (m_disconnectionInfoToDispatch != null)
			{
				this.OnConnectionClosed?.Invoke(m_disconnectionInfoToDispatch);
				m_disconnectionInfoToDispatch = null;
			}
			if (m_status == Status.Connected)
			{
				m_timeElapsedWithoutWriting += Time.get_deltaTime();
				if (m_timeElapsedWithoutWriting > 5f)
				{
					m_spinTransportLayer.Write(SpinProtocol.HeartbeatMessage.instance);
					m_timeElapsedWithoutWriting = 0f;
				}
			}
			if (m_messagesFifo != null)
			{
				T item;
				while (m_messagesFifo.TryTake(out item))
				{
					this.OnApplicationMessage?.Invoke(item);
				}
			}
		}

		private async void ConnectAsync(string host, int port)
		{
			try
			{
				m_status = Status.Connecting;
				m_messagesFifo = new BlockingCollection<T>();
				m_timeElapsedWithoutWriting = 0f;
				Log.Info("Requesting credentials...", 157, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Network\\Spin2\\SpinConnection.cs");
				ISpinCredentials credentials = await m_credentialsProvider.GetCredentials();
				if (m_status != Status.Destroyed)
				{
					Log.Info("Connecting with credentials...", 165, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Network\\Spin2\\SpinConnection.cs");
					await m_spinTransportLayer.ConnectAsync(host, port);
					if (m_status != Status.Destroyed)
					{
						m_spinTransportLayer.OnData = OnSpinMessage;
						m_spinTransportLayer.OnConnectionClosed = OnUnderlyingConnectionClosed;
						string s = credentials.CreateMessage();
						byte[] bytes = Encoding.UTF8.GetBytes(s);
						SendRawApplicationData(bytes);
						m_authenticationTaskSource = new TaskCompletionSource<IConnectionError>();
						IConnectionError connectionError = await m_authenticationTaskSource.Task;
						m_authenticationTaskSource = null;
						if (m_status != Status.Destroyed)
						{
							if (connectionError == null)
							{
								m_status = Status.Connected;
								this.OnConnectionOpened?.Invoke();
								m_disconnectionTaskSource = new TaskCompletionSource<IDisconnectionInfo>();
								IDisconnectionInfo info = await m_disconnectionTaskSource.Task;
								m_disconnectionTaskSource = null;
								if (m_status != Status.Destroyed)
								{
									DisconnectWithInfo(info);
								}
							}
							else
							{
								m_status = Status.Disconnected;
								m_spinTransportLayer.OnConnectionClosed = null;
								m_spinTransportLayer.Dispose();
								m_authenticationErrorToDispatch = connectionError;
							}
						}
					}
				}
			}
			catch (TaskCanceledException)
			{
				Log.Info($"Task canceled while connceting / connected to spin. Status: {m_status}", 215, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Network\\Spin2\\SpinConnection.cs");
				m_status = Status.Disconnected;
				m_spinTransportLayer.OnConnectionClosed = null;
				m_spinTransportLayer.Dispose();
			}
			catch (Exception ex2)
			{
				if (m_status != 0)
				{
					Log.Error($"Exception while connecting to Spin: {ex2}", 225, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Network\\Spin2\\SpinConnection.cs");
					m_status = Status.Disconnected;
					m_spinTransportLayer.OnConnectionClosed = null;
					m_spinTransportLayer.Dispose();
					m_authenticationErrorToDispatch = ((ex2 as IConnectionError) ?? new NetworkConnectionError(ex2));
				}
			}
		}

		private void DisconnectWithInfo(IDisconnectionInfo info)
		{
			switch (m_status)
			{
			case Status.Disconnected:
				break;
			case Status.Connecting:
				m_status = Status.Disconnected;
				m_authenticationTaskSource?.TrySetCanceled();
				m_spinTransportLayer.OnConnectionClosed = null;
				m_spinTransportLayer.Dispose();
				m_authenticationErrorToDispatch = new ConnectionInterruptedError(info);
				break;
			case Status.Connected:
				m_status = Status.Disconnected;
				m_disconnectionTaskSource?.TrySetCanceled();
				m_spinTransportLayer.OnConnectionClosed = null;
				m_spinTransportLayer.Dispose();
				m_disconnectionInfoToDispatch = info;
				break;
			}
		}

		private void SendRawApplicationData(byte[] data)
		{
			if (m_spinTransportLayer.Write(new SpinProtocol.RawApplicationMessage(data)))
			{
				m_timeElapsedWithoutWriting = 0f;
			}
		}

		private void OnSpinMessage(SpinProtocol.Message spinMessage)
		{
			switch (spinMessage.messageType)
			{
			case SpinProtocol.MessageType.Application:
				if (m_status == Status.Connected)
				{
					if (m_codec.TryDeserialize(spinMessage.payload, out T result))
					{
						m_messagesFifo.Add(result);
					}
					else
					{
						Log.Error("Unable to deserializer applicationMessage !!!", 280, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Network\\Spin2\\SpinConnection.cs");
					}
				}
				else if (m_status == Status.Connecting)
				{
					if (SpinProtocol.CheckAuthentication(spinMessage.payload, out SpinProtocol.ConnectionErrors optConnError))
					{
						Log.Info("Connection to SPIN succeded.", 288, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Network\\Spin2\\SpinConnection.cs");
						m_status = Status.Connected;
						m_authenticationTaskSource?.TrySetResult(null);
					}
					else
					{
						Log.Error($"Connection to SPIN2 failed with error {optConnError}", 294, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Network\\Spin2\\SpinConnection.cs");
						m_authenticationTaskSource?.TrySetResult(new SpinConnectionError(optConnError));
					}
				}
				break;
			case SpinProtocol.MessageType.Ping:
			{
				SpinProtocol.PongMessage input = new SpinProtocol.PongMessage((spinMessage as SpinProtocol.PingMessage)?.payload ?? new byte[0]);
				m_spinTransportLayer.Write(input);
				break;
			}
			case SpinProtocol.MessageType.Pong:
				Log.Info("PONG RECEIVED", 308, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Network\\Spin2\\SpinConnection.cs");
				break;
			}
		}

		private void OnUnderlyingConnectionClosed()
		{
			Log.Info("Underlying tcp connection closed.", 315, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Network\\Spin2\\SpinConnection.cs");
			switch (m_status)
			{
			case Status.Disconnected:
				break;
			case Status.Connecting:
				m_authenticationTaskSource?.TrySetResult(new ConnectionInterruptedError(new NetworkDisconnectionInfo()));
				break;
			case Status.Connected:
				m_disconnectionTaskSource?.TrySetResult(new NetworkDisconnectionInfo());
				break;
			}
		}

		private void OnApplicationQuit()
		{
			m_spinTransportLayer.OnConnectionClosed = null;
			m_disconnectionTaskSource?.TrySetCanceled();
			m_authenticationTaskSource?.TrySetCanceled();
			m_status = Status.Destroyed;
			m_spinTransportLayer.Dispose();
		}
	}
}
