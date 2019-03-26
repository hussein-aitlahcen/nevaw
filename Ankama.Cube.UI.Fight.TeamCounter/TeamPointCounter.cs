using Ankama.Cube.Fight;
using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Fight.TeamCounter
{
	public class TeamPointCounter : MonoBehaviour
	{
		[SerializeField]
		private UISpriteTextRenderer m_team0Score;

		[SerializeField]
		private UISpriteTextRenderer m_team1Score;

		[Header("Feedback")]
		[SerializeField]
		private TeamCounterFeedback m_team0Feedback;

		[SerializeField]
		private TeamCounterFeedback m_team1Feedback;

		[SerializeField]
		private UISpriteTextRenderer m_team0PointFeedback;

		[SerializeField]
		private UISpriteTextRenderer m_team1PointFeedback;

		[SerializeField]
		private TeamCounterFeedback m_team0CrownFeedback;

		[SerializeField]
		private TeamCounterFeedback m_team1CrownFeedback;

		private bool m_hasFirstVictory;

		public bool HasFinishedAFight()
		{
			return m_hasFirstVictory;
		}

		public void OnFirstVictory()
		{
			m_hasFirstVictory = true;
		}

		public void OnScoreChange(FightScore score)
		{
			string text = score.myTeamScore.scoreAfter.ToString();
			string text2 = score.opponentTeamScore.scoreAfter.ToString();
			m_team0Score.text = text;
			m_team1Score.text = text2;
			m_team0PointFeedback.text = text;
			m_team1PointFeedback.text = text2;
			if (score.myTeamScore.changed)
			{
				m_team0CrownFeedback.PlayFeedback();
				m_team0Feedback.PlayFeedback();
			}
			if (score.myTeamScore.changed)
			{
				m_team1CrownFeedback.PlayFeedback();
				m_team1Feedback.PlayFeedback();
			}
		}

		public void InitialiseScore(int score0, int score1)
		{
			m_team0Score.text = score0.ToString();
			m_team1Score.text = score1.ToString();
			m_team0CrownFeedback.SetOff();
			m_team1CrownFeedback.SetOff();
			m_team0Feedback.SetOff();
			m_team1Feedback.SetOff();
		}

		public TeamPointCounter()
			: this()
		{
		}
	}
}
