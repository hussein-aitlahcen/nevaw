using Ankama.AssetManagement.InputManagement;
using Ankama.AssetManagement.StateManagement;
using Ankama.Cube.Demo.UI;
using Ankama.Cube.Protocols.CommonProtocol;
using Ankama.Cube.Protocols.FightCommonProtocol;
using Ankama.Cube.States;
using JetBrains.Annotations;
using System;
using System.Collections;

namespace Ankama.Cube.Demo.States
{
	public class EndGameStatsStateDemo : AbstractEndGameState
	{
		private EndGameStatsUIDemo m_ui;

		private readonly GameStatistics m_gameStatistics;

		private FightResult m_endResult;

		private FightInfo m_fightInfo;

		private int m_fightTime;

		public EndGameStatsStateDemo(FightResult endResult, GameStatistics gameStatistics, int fightTime)
		{
			m_gameStatistics = gameStatistics;
			m_endResult = endResult;
			m_fightTime = fightTime;
		}

		protected override IEnumerator Load()
		{
			UILoader<EndGameStatsUIDemo> loader = new UILoader<EndGameStatsUIDemo>(FightState.instance, this, "EndGameStats", "demo/scenes/ui/fight", disableOnLoad: true);
			yield return loader.Load();
			m_ui = loader.ui;
			yield return m_ui.Init(m_endResult, m_gameStatistics, m_fightTime);
		}

		protected override IEnumerator Update()
		{
			m_ui.get_gameObject().SetActive(true);
			yield return m_ui.OpenCoroutine();
			m_ui.onContinueClick = OnContinueClick;
		}

		protected override IEnumerator Transition([CanBeNull] StateTransitionInfo transitionInfo)
		{
			m_ui.onContinueClick = null;
			yield return m_ui.CloseCoroutine();
			m_ui.get_gameObject().SetActive(false);
		}

		protected override void Disable()
		{
			m_ui.onContinueClick = null;
		}

		private void OnContinueClick()
		{
			m_ui.onContinueClick = null;
			onContinue?.Invoke();
		}

		protected unsafe override bool UseInput(InputState inputState)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			if ((int)((IntPtr)(void*)inputState).state != 1)
			{
				return this.UseInput(inputState);
			}
			int id = ((IntPtr)(void*)inputState).id;
			if (id == 2)
			{
				if (null != m_ui)
				{
					m_ui.DoContinueClick();
				}
				return true;
			}
			return this.UseInput(inputState);
		}
	}
}
