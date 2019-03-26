using Ankama.AssetManagement.InputManagement;
using Ankama.AssetManagement.StateManagement;
using Ankama.Cube.States;
using Ankama.Cube.Utility;
using System;
using System.Collections;

namespace Ankama.Cube.UI.Zaap
{
	public class UIZaapState : LoadSceneStateContext
	{
		private UIZaapPVPSelection m_ui;

		public Action onDisable;

		public Action onTransition;

		protected override IEnumerator Load()
		{
			UILoader<UIZaapPVPSelection> loader = new UILoader<UIZaapPVPSelection>(this, "UIZaap_PVP", "core/scenes/maps/havre_maps", disableOnLoad: true);
			yield return loader.Load();
			m_ui = loader.ui;
			m_ui.get_gameObject().SetActive(true);
			yield return m_ui.LoadAssets();
		}

		protected override void Enable()
		{
			m_ui.onCloseClick = OnCloseClick;
			m_ui.onPlayRequested = OnPlayRequested;
		}

		protected override void Disable()
		{
			m_ui.UnloadAsset();
			DisableUIEvents();
		}

		protected override IEnumerator Unload()
		{
			onDisable?.Invoke();
			return this.Unload();
		}

		private void DisableUIEvents()
		{
			m_ui.onPlayRequested = null;
			m_ui.onCloseClick = null;
		}

		protected override IEnumerator Update()
		{
			yield return m_ui.PlayEnterAnimation();
		}

		protected override IEnumerator Transition(StateTransitionInfo transitionInfo)
		{
			DisableUIEvents();
			onTransition?.Invoke();
			if (transitionInfo != null && transitionInfo.get_stateContext() != null && transitionInfo.get_stateContext() is UIPVPLoadingState)
			{
				yield return m_ui.PlayTransitionToVersusAnimation();
			}
			else
			{
				yield return m_ui.PlayCloseAnimation();
			}
			yield return _003C_003En__0(transitionInfo);
		}

		public override bool AllowsTransition(StateContext nextState)
		{
			return true;
		}

		private void OnCloseClick()
		{
			this.get_parent().ClearChildState(0);
		}

		private void OnPlayRequested(int fightDefinitionId, int? forcedLevel)
		{
			UIPVPLoadingState uIPVPLoadingState = new UIPVPLoadingState();
			uIPVPLoadingState.SetGameMode(fightDefinitionId);
			this.get_parent().SetChildState(uIPVPLoadingState, 0);
		}

		private void OnDebugMatchmakingRequested()
		{
			StatesUtility.GotoMatchMakingState();
		}

		protected unsafe override bool UseInput(InputState inputState)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Invalid comparison between Unknown and I4
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			int id = ((IntPtr)(void*)inputState).id;
			if (id == 1)
			{
				if ((int)((IntPtr)(void*)inputState).state == 1)
				{
					this.get_parent().ClearChildState(0);
				}
				return true;
			}
			return this.UseInput(inputState);
		}
	}
}
