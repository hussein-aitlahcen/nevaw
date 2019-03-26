using Ankama.Cube.Audio.UI;
using Ankama.Cube.Protocols.CommonProtocol;
using Ankama.Cube.Protocols.FightCommonProtocol;
using Ankama.Cube.UI;
using Ankama.Cube.UI.Components;
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ankama.Cube.Demo.UI
{
	public class EndGameStatsUIDemo : BaseOpenCloseUI
	{
		[SerializeField]
		private Button m_quitButton;

		[SerializeField]
		private AbstractTextField m_victoryText;

		[SerializeField]
		private AbstractTextField m_defeatText;

		[SerializeField]
		private RawTextField m_gameTimeText;

		[SerializeField]
		private StatBoard m_statBoard;

		[SerializeField]
		private AudioEventUITriggerOnEnable m_winAudio;

		[SerializeField]
		private AudioEventUITriggerOnEnable m_looseAudio;

		public Action onContinueClick;

		private unsafe void Start()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			m_quitButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		}

		private void OnQuitClick()
		{
			onContinueClick?.Invoke();
		}

		public void DoContinueClick()
		{
			InputUtility.SimulateClickOn(m_quitButton);
		}

		public IEnumerator Init(FightResult endResult, GameStatistics gameStatistics, int fightTime)
		{
			switch (endResult)
			{
			case FightResult.Victory:
				m_victoryText.get_gameObject().SetActive(true);
				m_defeatText.get_gameObject().SetActive(false);
				m_winAudio.get_gameObject().SetActive(true);
				break;
			case FightResult.Draw:
			case FightResult.Defeat:
				m_victoryText.get_gameObject().SetActive(false);
				m_defeatText.get_gameObject().SetActive(true);
				m_looseAudio.get_gameObject().SetActive(true);
				break;
			}
			TimeSpan timeSpan = TimeSpan.FromSeconds(fightTime);
			m_gameTimeText.SetText("Temps de jeu : " + ((timeSpan.Hours > 0) ? timeSpan.ToString("hh\\:mm\\:ss") : timeSpan.ToString("mm\\:ss")));
			yield return m_statBoard.Init(gameStatistics);
		}

		public override IEnumerator OpenCoroutine()
		{
			Sequence boardSequence = m_statBoard.Open();
			yield return base.OpenCoroutine();
			while (TweenExtensions.IsActive(boardSequence) || TweenExtensions.IsActive(boardSequence))
			{
				yield return null;
			}
		}
	}
}
