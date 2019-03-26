using Ankama.Cube.Protocols.ServerProtocol;
using System;

namespace Ankama.Cube.Network
{
	public interface IConnection<T> where T : class
	{
		event Action<T> OnApplicationMessage;

		event Action<IConnectionError> OnOpenConnectionFailed;

		event Action OnConnectionOpened;

		event Action<IDisconnectionInfo> OnConnectionClosed;

		void Connect(string host, int port);

		void Disconnect();

		void DisconnectByServer(DisconnectedByServerEvent evt);

		void Write(T message);
	}
}
