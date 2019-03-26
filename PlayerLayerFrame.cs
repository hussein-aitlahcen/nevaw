using Ankama.Cube.Network;
using Ankama.Cube.Protocols.PlayerProtocol;
using System;

public class PlayerLayerFrame : CubeMessageFrame
{
	public Action<ChangeGodResultEvent> onGodChangeResult;

	public PlayerLayerFrame()
	{
		base.WhenReceiveEnqueue<ChangeGodResultEvent>((Action<ChangeGodResultEvent>)OnGodChange);
	}

	private void OnGodChange(ChangeGodResultEvent result)
	{
		onGodChangeResult?.Invoke(result);
	}
}
