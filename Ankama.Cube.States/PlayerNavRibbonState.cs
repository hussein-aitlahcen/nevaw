using Ankama.AssetManagement;
using Ankama.AssetManagement.InputManagement;
using Ankama.AssetManagement.StateManagement;
using Ankama.Cube.UI;
using Ankama.Cube.UI.PlayerLayer;
using System;
using System.Collections;

namespace Ankama.Cube.States
{
	public class PlayerNavRibbonState : LoadSceneStateContext, IStateUIChildPriority
	{
		private PlayerLayerNavRoot m_ui;

		public UIPriority uiChildPriority => UIPriority.Back;

		protected override IEnumerator Load()
		{
			UILoader<PlayerLayerNavRoot> loader = new UILoader<PlayerLayerNavRoot>(this, "PlayerLayer_NavRibbonCanvas", "core/scenes/ui/player", disableOnLoad: true);
			yield return loader.Load();
			m_ui = loader.ui;
			m_ui.get_gameObject().SetActive(true);
			m_ui.Initialise();
			m_ui.OnCloseAction = Quit;
		}

		protected override IEnumerator Update()
		{
			yield return m_ui.PlayEnterAnimation();
			_003C_003En__0();
		}

		public override bool AllowsTransition(StateContext nextState)
		{
			return true;
		}

		protected override IEnumerator Transition(StateTransitionInfo transitionInfo)
		{
			yield return m_ui.OnClose();
		}

		protected unsafe override bool UseInput(InputState inputState)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Invalid comparison between Unknown and I4
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			int id = ((IntPtr)(void*)inputState).id;
			if (id == 1)
			{
				if ((int)((IntPtr)(void*)inputState).state == 1)
				{
					PlayerIconRoot.instance.ReducePanel();
				}
				return true;
			}
			return this.UseInput(inputState);
		}

		private void Quit()
		{
			StateManager.SimulateInput(1, 1);
		}
	}
}
