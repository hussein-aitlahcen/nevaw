using System;
using System.Threading.Tasks;

namespace Ankama.Cube.Network.Spin2.Layers
{
	public interface INetworkLayer<TIn> : IDisposable
	{
		Action<TIn> OnData
		{
			set;
		}

		Action OnConnectionClosed
		{
			set;
		}

		Task ConnectAsync(string host, int port);

		bool Write(TIn input);
	}
}
