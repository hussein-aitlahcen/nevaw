using Ankama.Cube.Demo.States;
using Ankama.Cube.Protocols.CommonProtocol;
using Ankama.Cube.Protocols.FightCommonProtocol;
using System.Collections.Generic;

namespace Ankama.Cube.States
{
	public class FightEndedState : LoadSceneStateContext
	{
		private List<AbstractEndGameState> m_subStates = new List<AbstractEndGameState>();

		private int m_currentSubStateIndex = -1;

		public FightEndedState(FightResult end, GameStatistics gameStatistics, int fightTime)
		{
			if (gameStatistics == null)
			{
				m_subStates.Add(new EndGameResultState(end));
			}
			else
			{
				m_subStates.Add(new EndGameStatsStateDemo(end, gameStatistics, fightTime));
			}
		}

		protected override void Enable()
		{
			GotoSubState(0);
		}

		private void GotoSubState(int index)
		{
			if (m_subStates.Count == 0 || index >= m_subStates.Count)
			{
				LeaveFight();
				return;
			}
			if (m_currentSubStateIndex >= 0 && m_currentSubStateIndex < m_subStates.Count)
			{
				AbstractEndGameState abstractEndGameState = m_subStates[m_currentSubStateIndex];
				abstractEndGameState.onContinue = null;
				abstractEndGameState.allowTransition = true;
			}
			m_currentSubStateIndex = index;
			AbstractEndGameState abstractEndGameState2 = m_subStates[index];
			abstractEndGameState2.onContinue = OnSubStateContinue;
			this.SetChildState(abstractEndGameState2, 0);
		}

		private void OnSubStateContinue()
		{
			GotoSubState(m_currentSubStateIndex + 1);
		}

		private void LeaveFight()
		{
			FightState.instance.LeaveAndGotoMainState();
		}
	}
}
