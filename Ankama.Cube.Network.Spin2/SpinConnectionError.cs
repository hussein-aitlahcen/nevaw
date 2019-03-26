using System;

namespace Ankama.Cube.Network.Spin2
{
	public class SpinConnectionError : Exception, IConnectionError
	{
		public SpinProtocol.ConnectionErrors error
		{
			get;
		}

		public SpinConnectionError(SpinProtocol.ConnectionErrors error)
		{
			this.error = error;
		}
	}
}
