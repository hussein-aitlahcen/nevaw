using Ankama.AssetManagement.InputManagement;
using Ankama.AssetManagement.StateManagement;
using Ankama.Cube.Data;
using Ankama.Cube.Network;
using Ankama.Cube.UI.GodSelection;
using System;
using System.Collections;

namespace Ankama.Cube.States
{
	public class GodSelectionState : LoadSceneStateContext
	{
		private GodSelectionFrame m_frame;

		private GodSelectionRoot m_ui;

		public Action onDisable;

		public Action onTransition;

		protected override IEnumerator Load()
		{
			UILoader<GodSelectionRoot> loader = new UILoader<GodSelectionRoot>(this, "GodSelectionUI", "core/scenes/ui/player", disableOnLoad: true);
			yield return loader.Load();
			m_ui = loader.ui;
			m_ui.Initialise();
			m_ui.get_gameObject().SetActive(true);
		}

		protected override IEnumerator Update()
		{
			yield return m_ui.PlayEnterAnimation();
		}

		protected override void Enable()
		{
			m_frame = new GodSelectionFrame();
			m_ui.onGodSelected = OnGodChangeRequest;
			m_ui.onCloseClick = CloseState;
		}

		protected override void Disable()
		{
			m_ui.onGodSelected = null;
			m_ui.onCloseClick = null;
			m_frame.Dispose();
		}

		protected override IEnumerator Unload()
		{
			onDisable?.Invoke();
			return this.Unload();
		}

		protected override IEnumerator Transition(StateTransitionInfo transitionInfo)
		{
			onTransition?.Invoke();
			yield return m_ui.CloseUI();
			yield return _003C_003En__0(transitionInfo);
		}

		public override bool AllowsTransition(StateContext nextState)
		{
			return true;
		}

		private void OnGodChangeRequest(God value)
		{
			m_frame.ChangeGodRequest(value);
		}

		private void CloseState()
		{
			this.get_parent().ClearChildState(0);
		}

		protected unsafe override bool UseInput(InputState inputState)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Invalid comparison between Unknown and I4
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			int id = ((IntPtr)(void*)inputState).id;
			if (id == 1)
			{
				if ((int)((IntPtr)(void*)inputState).state == 1 && null != m_ui)
				{
					m_ui.SimulateCloseClick();
				}
				return true;
			}
			return this.UseInput(inputState);
		}
	}
}
