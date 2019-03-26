using Ankama.AssetManagement;
using Ankama.AssetManagement.StateManagement;
using Ankama.Cube.Audio.UI;
using Ankama.Cube.States;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ankama.Cube.UI
{
	public class ParametersUI : AbstractUI
	{
		[SerializeField]
		private Button m_parametersButton;

		[SerializeField]
		private Button m_bugReportButton;

		[SerializeField]
		private Button m_optionButton;

		[SerializeField]
		private Button m_quitButton;

		[SerializeField]
		private Button m_outMenuButton;

		[SerializeField]
		private CanvasGroup m_menu;

		[SerializeField]
		private float m_fadeDuration = 0.15f;

		[Header("Audio")]
		[SerializeField]
		private AudioEventUITriggerOnEvent m_openAudio;

		[SerializeField]
		private AudioEventUITriggerOnEvent m_closeAudio;

		public Action<bool> onShowMenu;

		public Action onOptionClick;

		public Action onQuitClick;

		private bool m_menuOpen;

		private Tween m_fadeTween;

		private unsafe void Start()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Expected O, but got Unknown
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Expected O, but got Unknown
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Expected O, but got Unknown
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Expected O, but got Unknown
			m_parametersButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_bugReportButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_optionButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_quitButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_outMenuButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_menu.get_gameObject().SetActive(false);
			m_menu.set_alpha(0f);
		}

		private void OnParametersClick()
		{
			ShowMenu(!m_menuOpen);
		}

		private void OnBugReportClick()
		{
			if (BugReportState.isReady)
			{
				StateLayer defaultLayer = default(StateLayer);
				if (!StateManager.TryGetLayer("OptionUI", ref defaultLayer))
				{
					defaultLayer = StateManager.GetDefaultLayer();
				}
				StateManager.SetActiveInputLayer(defaultLayer);
				BugReportState bugReportState = new BugReportState();
				bugReportState.Initialize();
				defaultLayer.GetChainEnd().SetChildState(bugReportState, 0);
			}
		}

		private void OnOptionClick()
		{
			ShowMenu(value: false);
			onOptionClick?.Invoke();
		}

		private void OnOutMenuClick()
		{
			ShowMenu(value: false);
		}

		private void OnQuitClick()
		{
			ShowMenu(value: false);
			onQuitClick?.Invoke();
		}

		private unsafe void ShowMenu(bool value)
		{
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Expected O, but got Unknown
			m_menuOpen = value;
			Tween fadeTween = m_fadeTween;
			if (fadeTween != null)
			{
				TweenExtensions.Kill(fadeTween, false);
			}
			if (m_menuOpen)
			{
				m_menu.get_gameObject().SetActive(true);
				m_fadeTween = DOTweenModuleUI.DOFade(m_menu, 1f, m_fadeDuration);
				m_openAudio.Trigger();
			}
			else
			{
				m_fadeTween = TweenSettingsExtensions.OnComplete<Tweener>(DOTweenModuleUI.DOFade(m_menu, 0f, m_fadeDuration), new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
				m_closeAudio.Trigger();
			}
			onShowMenu?.Invoke(value);
		}

		private void DeactivateMenu()
		{
			m_menu.get_gameObject().SetActive(false);
		}

		public void SimulateOptionClick()
		{
			InputUtility.SimulateClickOn(m_parametersButton);
		}
	}
}
