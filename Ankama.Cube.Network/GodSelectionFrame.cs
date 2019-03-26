using Ankama.Cube.Data;
using Ankama.Cube.Protocols.PlayerProtocol;
using System;

namespace Ankama.Cube.Network
{
	public class GodSelectionFrame : CubeMessageFrame
	{
		public event Action OnChangeGod;

		public GodSelectionFrame()
		{
			base.WhenReceiveEnqueue<ChangeGodResultEvent>((Action<ChangeGodResultEvent>)OnGodChanged);
		}

		public void ChangeGodRequest(God god)
		{
			ChangeGodCmd message = new ChangeGodCmd
			{
				God = (int)god
			};
			m_connection.Write(message);
		}

		private void OnGodChanged(ChangeGodResultEvent obj)
		{
			this.OnChangeGod?.Invoke();
		}
	}
}
