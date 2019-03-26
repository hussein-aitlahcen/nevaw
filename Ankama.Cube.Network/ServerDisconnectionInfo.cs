using Ankama.Cube.Protocols.ServerProtocol;

namespace Ankama.Cube.Network
{
	public class ServerDisconnectionInfo : IDisconnectionInfo
	{
		public DisconnectedByServerEvent.Types.Reason reason
		{
			get;
		}

		public ServerDisconnectionInfo(DisconnectedByServerEvent.Types.Reason reason)
		{
			this.reason = reason;
		}
	}
}
