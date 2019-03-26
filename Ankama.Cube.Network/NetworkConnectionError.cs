using System;

namespace Ankama.Cube.Network
{
	public class NetworkConnectionError : IConnectionError
	{
		public Exception exception
		{
			get;
		}

		public NetworkConnectionError(Exception exception)
		{
			this.exception = exception;
		}
	}
}
