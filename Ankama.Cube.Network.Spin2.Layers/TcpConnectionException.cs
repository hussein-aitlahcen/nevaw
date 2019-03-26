using System;

namespace Ankama.Cube.Network.Spin2.Layers
{
	public abstract class TcpConnectionException : Exception
	{
		protected TcpConnectionException(string message)
			: base(message)
		{
		}
	}
}
