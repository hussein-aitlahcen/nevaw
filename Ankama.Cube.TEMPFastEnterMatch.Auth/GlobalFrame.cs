using Ankama.Cube.Network;
using Ankama.Cube.Protocols.ServerProtocol;
using System;

namespace Ankama.Cube.TEMPFastEnterMatch.Auth
{
	public class GlobalFrame : CubeMessageFrame
	{
		public GlobalFrame()
		{
			base.WhenReceiveEnqueue<DisconnectedByServerEvent>((Action<DisconnectedByServerEvent>)OnDisconnectedByServer);
		}

		private void OnDisconnectedByServer(DisconnectedByServerEvent evt)
		{
			m_connection.DisconnectByServer(evt);
		}
	}
}
