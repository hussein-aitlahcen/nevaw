using Ankama.AssetManagement.InputManagement;
using Ankama.AssetManagement.StateManagement;
using Ankama.Cube.Protocols.CommonProtocol;
using Ankama.Cube.UI.Fight;
using JetBrains.Annotations;
using System;
using System.Collections;

namespace Ankama.Cube.States
{
	public class EndGameResultState : AbstractEndGameState
	{
		private FightEndedUI m_ui;

		private readonly FightResult m_end;

		public EndGameResultState(FightResult end)
		{
			m_end = end;
		}

		protected override IEnumerator Load()
		{
			UILoader<FightEndedUI> loader = new UILoader<FightEndedUI>(FightState.instance, this, GetSceneName(m_end), "core/scenes/ui/fight", disableOnLoad: true);
			yield return loader.Load();
			m_ui = loader.ui;
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

		private string GetSceneName(FightResult end)
		{
			switch (end)
			{
			case FightResult.Victory:
				return "FightEndedWinUI";
			case FightResult.Defeat:
				return "FightEndedLoseUI";
			case FightResult.Draw:
				return "FightEndedDrawUI";
			default:
				throw new ArgumentOutOfRangeException("end", end, null);
			}
		}

		protected unsafe override bool UseInput(InputState inputState)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			if ((int)((IntPtr)(void*)inputState).state != 1)
			{
				return this.UseInput(inputState);
			}
			int id = ((IntPtr)(void*)inputState).id;
			if (id == 2)
			{
				m_ui.DoContinueClick();
				return true;
			}
			return this.UseInput(inputState);
		}
	}
}
