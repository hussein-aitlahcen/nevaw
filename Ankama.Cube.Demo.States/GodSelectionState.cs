using Ankama.AssetManagement.InputManagement;
using Ankama.AssetManagement.StateManagement;
using Ankama.Cube.Data;
using Ankama.Cube.Demo.UI;
using Ankama.Cube.Network;
using Ankama.Cube.States;
using Ankama.Cube.TEMPFastEnterMatch.Player;
using Ankama.Cube.UI;
using System;
using System.Collections;

namespace Ankama.Cube.Demo.States
{
	public class GodSelectionState : BaseFightSelectionState, IStateUIChildPriority
	{
		public Action<God> onGodSelected;

		private GodSelectionUIDemo m_ui;

		private GodSelectionFrame m_frame;

		public UIPriority uiChildPriority => UIPriority.Back;

		protected override IEnumerator Load()
		{
			UILoader<GodSelectionUIDemo> loader = new UILoader<GodSelectionUIDemo>(this, "GodSelectionUIDemo", "demo/scenes/ui/godselection", disableOnLoad: true);
			yield return loader.Load();
			m_ui = loader.ui;
			m_ui.Init();
			m_ui.get_gameObject().SetActive(false);
		}

		protected override IEnumerator Update()
		{
			m_ui.get_gameObject().SetActive(true);
			yield return m_ui.OpenFrom(base.fromSide);
			m_ui.onSelect = OnSelectClick;
			onUIOpeningFinished?.Invoke();
		}

		public override bool AllowsTransition(StateContext nextState)
		{
			return true;
		}

		protected override IEnumerator Transition(StateTransitionInfo transitionInfo)
		{
			yield return m_ui.CloseTo(base.toSide);
		}

		protected override void Enable()
		{
			m_frame = new GodSelectionFrame();
			m_frame.OnChangeGod += OnChangeGod;
		}

		protected override void Disable()
		{
			m_ui.onSelect = null;
			m_frame.Dispose();
			m_frame = null;
		}

		protected unsafe override bool UseInput(InputState inputState)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			if ((int)((IntPtr)(void*)inputState).state != 1)
			{
				return this.UseInput(inputState);
			}
			switch (((IntPtr)(void*)inputState).id)
			{
			case 2:
			case 3:
				if (null != m_ui)
				{
					m_ui.SimulateSelectClick();
				}
				return true;
			case 6:
				if (null != m_ui)
				{
					m_ui.SimulateRightArrowClick();
				}
				return true;
			case 7:
				if (null != m_ui)
				{
					m_ui.SimulateLeftArrowClick();
				}
				return true;
			default:
				return this.UseInput(inputState);
			}
		}

		private void OnSelectClick(God god)
		{
			m_frame.ChangeGodRequest(god);
		}

		private void OnChangeGod()
		{
			onGodSelected?.Invoke(PlayerData.instance.god);
		}
	}
}
