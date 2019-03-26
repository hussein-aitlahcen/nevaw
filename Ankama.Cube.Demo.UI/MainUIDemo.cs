using Ankama.Cube.UI;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace Ankama.Cube.Demo.UI
{
	public class MainUIDemo : AbstractUI
	{
		[SerializeField]
		private Button m_returnButton;

		[SerializeField]
		private StepIndicator m_stateIndicator1;

		[SerializeField]
		private StepIndicator m_stateIndicator2;

		[SerializeField]
		private StepIndicator m_stateIndicator3;

		[SerializeField]
		private PlayerAvatarDemo m_playerAvatar;

		[SerializeField]
		private PlayableDirector m_open;

		[SerializeField]
		private PlayableDirector m_close;

		[SerializeField]
		private PlayableDirector m_showPlayerAvatar;

		[SerializeField]
		private PlayableDirector m_hidePlayerAvatar;

		[SerializeField]
		private PlayableDirector m_showStepMenu;

		[SerializeField]
		private PlayableDirector m_hideStepMenu;

		[SerializeField]
		private PlayableDirector m_gotoVersus;

		[SerializeField]
		private PlayableDirector m_gotoFight;

		private int m_stateIndex = -1;

		public Action onReturn;

		public Button returnButton => m_returnButton;

		public PlayerAvatarDemo playerAvatar => m_playerAvatar;

		private unsafe void Start()
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Expected O, but got Unknown
			m_playerAvatar.get_gameObject().SetActive(false);
			m_returnButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		}

		public void SimulateReturnClick()
		{
			InputUtility.SimulateClickOn(m_returnButton);
		}

		public void SetStateIndex(int idx, bool tween)
		{
			if (m_stateIndex != idx)
			{
				switch (idx)
				{
				case 0:
					m_stateIndicator1.SetState(StepIndicator.State.Enable, tween);
					m_stateIndicator2.SetState(StepIndicator.State.Disable, tween);
					m_stateIndicator3.SetState(StepIndicator.State.Disable, tween);
					break;
				case 1:
					m_stateIndicator1.SetState(StepIndicator.State.Disable, tween);
					m_stateIndicator2.SetState(StepIndicator.State.Enable, tween);
					m_stateIndicator3.SetState(StepIndicator.State.Disable, tween);
					break;
				case 2:
					m_stateIndicator1.SetState(StepIndicator.State.Disable, tween);
					m_stateIndicator2.SetState(StepIndicator.State.Disable, tween);
					m_stateIndicator3.SetState(StepIndicator.State.Enable, tween);
					break;
				}
				m_stateIndex = idx;
			}
		}

		public void ShowPlayerAvatarAnim(bool value)
		{
			m_playerAvatar.get_gameObject().SetActive(true);
			if (value)
			{
				m_showPlayerAvatar.set_time(0.0);
				m_showPlayerAvatar.Play();
			}
			else
			{
				m_hidePlayerAvatar.set_time(0.0);
				m_hidePlayerAvatar.Play();
			}
		}

		public void ShowStepMenuAnim(bool value)
		{
			if (value)
			{
				m_showStepMenu.set_time(0.0);
				m_showStepMenu.Play();
			}
			else
			{
				m_hideStepMenu.set_time(0.0);
				m_hideStepMenu.Play();
			}
		}

		public IEnumerator GotoFightAnim()
		{
			yield return BaseOpenCloseUI.PlayDirector(m_gotoFight);
		}

		public void PlayVersusAnim()
		{
			m_gotoVersus.set_time(0.0);
			m_gotoVersus.Play();
		}

		public void Open()
		{
			m_open.set_time(0.0);
			m_open.Play();
		}

		public IEnumerator CloseCoroutine()
		{
			yield return BaseOpenCloseUI.PlayDirector(m_close);
		}
	}
}
