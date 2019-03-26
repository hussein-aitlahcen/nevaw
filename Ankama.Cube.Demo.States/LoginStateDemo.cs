using Ankama.AssetManagement;
using Ankama.AssetManagement.InputManagement;
using Ankama.AssetManagement.StateManagement;
using Ankama.Cube.Code.UI;
using Ankama.Cube.Configuration;
using Ankama.Cube.Demo.UI;
using Ankama.Cube.Network;
using Ankama.Cube.Player;
using Ankama.Cube.States;
using Ankama.Cube.TEMPFastEnterMatch.Player;
using Ankama.Cube.UI;
using JetBrains.Annotations;
using System;
using System.Collections;
using UnityEngine;

namespace Ankama.Cube.Demo.States
{
	public class LoginStateDemo : LoadSceneStateContext
	{
		private LoginUIDemo m_ui;

		protected override IEnumerator Load()
		{
			UILoader<LoginUIDemo> loader = new UILoader<LoginUIDemo>(this, "LoginUIDemo", "demo/scenes/ui/login", disableOnLoad: true);
			yield return loader.Load();
			m_ui = loader.ui;
			m_ui.get_gameObject().SetActive(false);
		}

		protected override IEnumerator Update()
		{
			m_ui.get_gameObject().SetActive(true);
			yield return m_ui.OpenCoroutine();
			yield return _003C_003En__0();
		}

		protected override void Enable()
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Expected O, but got Unknown
			m_ui.onConnect = Connect;
			if (ApplicationConfig.debugMode)
			{
				StateManager.RegisterInputDefinition(new InputKeyCodeDefinition(293, 5, 0, 0.4f, 0.1f));
			}
			ConnectionHandler.instance.OnConnectionStatusChanged += OnConnectionStatusChanged;
		}

		public override bool AllowsTransition([CanBeNull] StateContext nextState)
		{
			return nextState is MainStateDemo;
		}

		protected override IEnumerator Transition([CanBeNull] StateTransitionInfo transitionInfo)
		{
			yield return m_ui.CloseCoroutine();
		}

		protected override void Disable()
		{
			m_ui.onConnect = null;
			m_ui.get_gameObject().SetActive(false);
			if (ApplicationConfig.debugMode)
			{
				StateManager.UnregisterInputDefinition(5);
			}
			ConnectionHandler.instance.OnConnectionStatusChanged -= OnConnectionStatusChanged;
		}

		protected unsafe override bool UseInput(InputState inputState)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
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
					m_ui.DoClickSelected();
				}
				return true;
			case 4:
				if (null != m_ui)
				{
					m_ui.SelectNext();
				}
				return true;
			case 5:
				if (null != m_ui)
				{
					LoginDebugUI componentInChildren = m_ui.GetComponentInChildren<LoginDebugUI>(true);
					componentInChildren.get_gameObject().SetActive(!componentInChildren.get_gameObject().get_activeSelf());
				}
				return true;
			default:
				return this.UseInput(inputState);
			}
		}

		private void LockUI(bool value)
		{
			m_ui.canvasGroup.set_interactable(!value);
		}

		private void Connect(bool asGuest, string login)
		{
			if (string.IsNullOrEmpty(login))
			{
				PopupInfoManager.Show(StateManager.GetDefaultLayer().GetChainEnd(), new PopupInfo
				{
					message = 98703,
					buttons = new ButtonData[1]
					{
						new ButtonData(27169)
					},
					selectedButton = 1,
					style = PopupStyle.Normal
				});
				return;
			}
			LockUI(value: true);
			PlayerPreferences.lastLogin = login;
			if (string.IsNullOrWhiteSpace(PlayerPreferences.lastPassword))
			{
				PlayerPreferences.lastPassword = "pass";
			}
			PlayerPreferences.Save();
			ConnectionHandler.instance.Connect();
		}

		private void OnPlayerDataInitialized(bool pendigFightFound)
		{
			PlayerData.OnPlayerDataInitialized -= OnPlayerDataInitialized;
			this.get_parent().SetChildState(new MainStateDemo(), 0);
		}

		public void OnConnectionStatusChanged(ConnectionHandler.Status from, ConnectionHandler.Status to)
		{
			if (Object.op_Implicit(m_ui))
			{
				switch (to)
				{
				case ConnectionHandler.Status.Disconnected:
					LockUI(value: false);
					break;
				case ConnectionHandler.Status.Connected:
					PlayerData.OnPlayerDataInitialized += OnPlayerDataInitialized;
					LockUI(value: true);
					break;
				case ConnectionHandler.Status.Connecting:
					LockUI(value: true);
					break;
				default:
					throw new ArgumentOutOfRangeException("to", to, null);
				}
			}
		}
	}
}
