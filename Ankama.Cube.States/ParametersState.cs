using Ankama.AssetManagement;
using Ankama.AssetManagement.InputManagement;
using Ankama.AssetManagement.StateManagement;
using Ankama.Cube.Code.UI;
using Ankama.Cube.UI;
using System;
using System.Collections;

namespace Ankama.Cube.States
{
	public class ParametersState : LoadSceneStateContext
	{
		private ParametersUI m_ui;

		protected override IEnumerator Load()
		{
			UILoader<ParametersUI> loader = new UILoader<ParametersUI>(this, "ParametersUI", "core/scenes/ui/option");
			yield return loader.Load();
			m_ui = loader.ui;
		}

		protected override void Enable()
		{
			m_ui.onShowMenu = OnShowMenu;
			m_ui.onOptionClick = OnOptionClick;
			m_ui.onQuitClick = OnQuitClick;
		}

		protected override void Disable()
		{
			m_ui.onShowMenu = null;
			m_ui.onOptionClick = null;
			m_ui.onQuitClick = null;
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
					m_ui.SimulateOptionClick();
				}
				return true;
			}
			return this.UseInput(inputState);
		}

		private void OnShowMenu(bool value)
		{
			if (value)
			{
				SetFocusOnLayer("OptionUI");
			}
			else
			{
				SetFocusOnLayer("PlayerUI");
			}
		}

		private void OnOptionClick()
		{
			if (!this.HasPendingStates() && !this.HasChildState())
			{
				SetFocusOnLayer("OptionUI");
				OptionState optionState = new OptionState();
				optionState.onStateClosed = OnOptionClosed;
				this.SetChildState(optionState, 0);
			}
		}

		private void OnQuitClick()
		{
			PopupInfoManager.Show(this, new PopupInfo
			{
				message = 75182,
				buttons = new ButtonData[2]
				{
					new ButtonData(9912, Main.Quit, closeOnClick: true, ButtonStyle.Negative),
					new ButtonData(68421)
				},
				selectedButton = 2,
				style = PopupStyle.Normal,
				useBlur = true
			});
		}

		private void OnOptionClosed()
		{
			SetFocusOnLayer("PlayerUI");
		}

		private void SetFocusOnLayer(string layerName)
		{
			StateLayer activeInputLayer = default(StateLayer);
			if (StateManager.TryGetLayer(layerName, ref activeInputLayer))
			{
				StateManager.SetActiveInputLayer(activeInputLayer);
			}
		}
	}
}
