namespace Ankama.Cube.Network
{
	public class ConnectionInterruptedError : IConnectionError
	{
		public IDisconnectionInfo disconnectionInfo
		{
			get;
		}

		public ConnectionInterruptedError(IDisconnectionInfo disconnectionInfo)
		{
			this.disconnectionInfo = disconnectionInfo;
		}
	}
}
