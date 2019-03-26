using Ankama.AssetManagement.InputManagement;
using Ankama.AssetManagement.StateManagement;
using Ankama.Cube.Player;
using Ankama.Cube.UI;
using JetBrains.Annotations;
using System;
using System.Collections;

namespace Ankama.Cube.States
{
	public class OptionState : LoadSceneStateContext
	{
		private OptionUI m_ui;

		public Action onStateClosed;

		protected override IEnumerator Load()
		{
			UILoader<OptionUI> loader = new UILoader<OptionUI>(this, "OptionUI", "core/scenes/ui/option", disableOnLoad: true);
			yield return loader.Load();
			m_ui = loader.ui;
			m_ui.Initialise();
			yield return null;
			m_ui.get_gameObject().SetActive(true);
			yield return m_ui.OpenCoroutine();
		}

		protected override void Enable()
		{
			m_ui.onCloseClick = OnCloseClick;
		}

		protected override void Disable()
		{
			m_ui.onCloseClick = null;
			PlayerPreferences.Save();
		}

		public override bool AllowsTransition([CanBeNull] StateContext nextState)
		{
			return true;
		}

		protected override IEnumerator Transition([CanBeNull] StateTransitionInfo transitionInfo)
		{
			m_ui.onCloseClick = null;
			yield return m_ui.CloseCoroutine();
			onStateClosed?.Invoke();
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

		private void OnCloseClick()
		{
			this.get_parent().ClearChildState(0);
		}
	}
}
