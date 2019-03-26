using Ankama.Cube.Audio.UI;
using Ankama.Cube.UI.Components;
using Ankama.Utilities;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Fight
{
	public class TurnFeedbackUI : MonoBehaviour
	{
		public enum Type
		{
			Player,
			PlayerTeam,
			Opponent,
			OpponentTeam,
			Boss
		}

		[SerializeField]
		private TextField m_text;

		[SerializeField]
		private TextField m_textEffect;

		[SerializeField]
		private Image m_picto;

		[SerializeField]
		private RawTextField m_playerName;

		[SerializeField]
		private PlayableDirector m_openCloseDirector;

		[SerializeField]
		private TurnFeedbackData m_data;

		[SerializeField]
		private AudioEventUITrigger m_playerTurnSoundTrigger;

		private Coroutine m_animCoroutine;

		private PlayableDirector m_playingDirector;

		private bool m_isAnimating;

		public bool isAnimating => m_isAnimating;

		private void OnDisable()
		{
			if (m_animCoroutine != null)
			{
				this.StopCoroutine(m_animCoroutine);
				m_animCoroutine = null;
			}
			m_isAnimating = false;
		}

		public void Show(Type type, string playerName, Action onComplete = null)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			if (GetData(type, out TurnFeedbackData.PlayerSideData data))
			{
				m_playerName.SetText(playerName);
				m_playerName.color = data.nameColor;
				m_text.SetText(data.messageKey);
				m_textEffect.SetText(data.messageKey);
				m_text.color = data.titleColor;
				m_textEffect.color = data.titleColor;
				m_picto.set_sprite(data.icon);
				m_playerTurnSoundTrigger.set_enabled(type == Type.Player);
				PlayAnim(m_openCloseDirector, onComplete);
			}
		}

		private bool GetData(Type type, out TurnFeedbackData.PlayerSideData data)
		{
			switch (type)
			{
			case Type.Player:
				data = m_data.player;
				return true;
			case Type.PlayerTeam:
				data = m_data.playerTeam;
				return true;
			case Type.Opponent:
				data = m_data.opponent;
				return true;
			case Type.OpponentTeam:
				data = m_data.opponentTeam;
				return true;
			case Type.Boss:
				data = m_data.boss;
				return true;
			default:
				Log.Error($"type not handled {type}", 95, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\FightRework\\TurnFeedback\\TurnFeedbackUI.cs");
				data = default(TurnFeedbackData.PlayerSideData);
				return false;
			}
		}

		private void PlayAnim(PlayableDirector director, Action onComplete = null)
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			m_isAnimating = true;
			this.get_gameObject().SetActive(true);
			if (m_animCoroutine != null)
			{
				this.StopCoroutine(m_animCoroutine);
			}
			if (m_playingDirector != null)
			{
				PlayableGraph playableGraph = m_playingDirector.get_playableGraph();
				if (playableGraph.IsValid())
				{
					playableGraph = m_playingDirector.get_playableGraph();
					if (!playableGraph.IsDone())
					{
						m_playingDirector.Stop();
					}
				}
			}
			m_playingDirector = director;
			director.set_time(0.0);
			director.Play();
			m_animCoroutine = this.StartCoroutine(AnimCoroutine(director, onComplete));
		}

		private IEnumerator AnimCoroutine(PlayableDirector director, Action onComplete = null)
		{
			PlayableGraph graph = director.get_playableGraph();
			while (graph.IsValid() && !graph.IsDone())
			{
				yield return null;
			}
			this.get_gameObject().SetActive(false);
			m_isAnimating = false;
			onComplete?.Invoke();
			m_animCoroutine = null;
		}

		public TurnFeedbackUI()
			: this()
		{
		}
	}
}
