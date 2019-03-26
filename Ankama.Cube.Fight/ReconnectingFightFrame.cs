using Ankama.Cube.Network;
using Ankama.Cube.Protocols.FightCommonProtocol;
using Ankama.Cube.Protocols.FightProtocol;
using System;

namespace Ankama.Cube.Fight
{
	public sealed class ReconnectingFightFrame : CubeMessageFrame
	{
		public Action<FightInfo> OnFightDataReceived;

		public ReconnectingFightFrame()
		{
			base.WhenReceiveEnqueue<FightInfoEvent>((Action<FightInfoEvent>)OnFightInfo);
		}

		private void OnFightInfo(FightInfoEvent msg)
		{
			OnFightDataReceived?.Invoke(msg.FightInfo);
		}

		public void RequestFightInfo()
		{
			m_connection.Write(new GetFightInfoCmd());
		}
	}
}
