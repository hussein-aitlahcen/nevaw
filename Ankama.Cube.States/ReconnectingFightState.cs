using Ankama.AssetManagement;
using Ankama.AssetManagement.StateManagement;
using Ankama.Cube.Fight;
using Ankama.Cube.Protocols.FightCommonProtocol;
using Ankama.Cube.Utility;

namespace Ankama.Cube.States
{
	public class ReconnectingFightState : StateContext
	{
		private ReconnectingFightFrame m_frame;

		protected override void Enable()
		{
			m_frame = new ReconnectingFightFrame
			{
				OnFightDataReceived = OnFightDataReceived
			};
			m_frame.RequestFightInfo();
		}

		protected override void Disable()
		{
			m_frame.Dispose();
		}

		private void OnFightDataReceived(FightInfo fightInfo)
		{
			StatesUtility.DoTransition(new FightState(fightInfo, hardResumed: true), StateManager.GetDefaultLayer().GetChildState());
		}

		public ReconnectingFightState()
			: this()
		{
		}
	}
}
