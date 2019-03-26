using Ankama.Utilities;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Ankama.Cube.Network.Spin2.Layers
{
	public class TcpConnectionLayer : INetworkLayer<byte[]>, IDisposable
	{
		public const int ConnectionTimeoutMs = 10000;

		public const int ReceiveBufferSize = 8192;

		private Thread m_thread;

		private TcpClient m_tcpClient;

		private NetworkStream m_networkStream;

		private long m_threadExecutionUid;

		public Action OnConnectionClosed
		{
			private get;
			set;
		}

		public Action<byte[]> OnData
		{
			private get;
			set;
		}

		public async Task ConnectAsync(string host, int port)
		{
			m_tcpClient = new TcpClient(AddressFamily.InterNetworkV6)
			{
				Client = 
				{
					DualMode = true
				}
			};
			Task connectTask = m_tcpClient.ConnectAsync(host, port);
			Task timeoutTask = Task.Delay(10000);
			await Task.WhenAny(connectTask, timeoutTask);
			if (timeoutTask.Status == TaskStatus.RanToCompletion)
			{
				Dispose();
				throw new TimeoutException($"TcpConnection disposed: Unable to connect to {host}:{port} before timeout of {10000}ms");
			}
			if (connectTask.Status != TaskStatus.RanToCompletion)
			{
				Dispose();
				throw new UnableToConnectException(connectTask.Status, connectTask.Exception);
			}
			m_networkStream = m_tcpClient?.GetStream();
			Thread thread = new Thread(ThreadMain);
			thread.set_IsBackground(true);
			thread.Name = "TcpConnectionLayer-NetworkThread";
			m_thread = thread;
			m_thread.Start();
		}

		public bool Write(byte[] input)
		{
			m_networkStream.WriteAsync(input, 0, input.Length);
			return true;
		}

		private void ThreadMain()
		{
			long num = m_threadExecutionUid = DateTime.Now.Ticks;
			Log.Info(string.Format("Starting {0} receive thread (id {1})", "TcpConnectionLayer", num), 88, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Network\\Spin2\\Layers\\TcpConnectionLayer.cs");
			byte[] array = new byte[8192];
			try
			{
				while (m_networkStream != null && m_threadExecutionUid == num)
				{
					int num2 = m_networkStream.Read(array, 0, 8192);
					if (num2 <= 0)
					{
						break;
					}
					byte[] array2 = new byte[num2];
					Buffer.BlockCopy(array, 0, array2, 0, num2);
					OnData(array2);
				}
			}
			catch (IOException arg)
			{
				if (m_threadExecutionUid == num)
				{
					Log.Info($"IOException on socket: {arg}", 119, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Network\\Spin2\\Layers\\TcpConnectionLayer.cs");
				}
			}
			catch (Exception arg2)
			{
				if (m_threadExecutionUid == num)
				{
					Log.Error(string.Format("Unexpected Exception in {0}: {1}", "TcpConnectionLayer", arg2), 128, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Network\\Spin2\\Layers\\TcpConnectionLayer.cs");
				}
			}
			finally
			{
				Log.Info(string.Format("Exiting {0} receive thread (id {1})", "TcpConnectionLayer", num), 134, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Network\\Spin2\\Layers\\TcpConnectionLayer.cs");
				OnConnectionClosed?.Invoke();
			}
		}

		public void CloseAbruptly()
		{
			m_networkStream.Close();
		}

		public void Dispose()
		{
			m_threadExecutionUid = 0L;
			m_thread = null;
			try
			{
				if (m_networkStream != null)
				{
					m_networkStream.Dispose();
					m_networkStream = null;
				}
			}
			finally
			{
				if (m_tcpClient != null)
				{
					m_tcpClient.Dispose();
					m_tcpClient = null;
				}
			}
		}
	}
}
