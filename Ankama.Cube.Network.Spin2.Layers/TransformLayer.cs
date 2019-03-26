using System;
using System.Threading.Tasks;

namespace Ankama.Cube.Network.Spin2.Layers
{
	public abstract class TransformLayer<TIn, TOut> : INetworkLayer<TIn>, IDisposable
	{
		protected INetworkLayer<TOut> child
		{
			get;
		}

		public Action<TIn> OnData
		{
			protected get;
			set;
		}

		public Action OnConnectionClosed
		{
			set
			{
				child.OnConnectionClosed = value;
			}
		}

		protected TransformLayer(INetworkLayer<TOut> child)
		{
			this.child = child;
			this.child.OnData = OnDataReceived;
		}

		public virtual async Task ConnectAsync(string host, int port)
		{
			await child.ConnectAsync(host, port);
		}

		public virtual void Dispose()
		{
			child.Dispose();
		}

		public abstract bool Write(TIn input);

		protected abstract void OnDataReceived(TOut data);
	}
}
