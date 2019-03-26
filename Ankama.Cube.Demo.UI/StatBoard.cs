using Ankama.Cube.Data;
using Ankama.Cube.Fight;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Protocols.FightCommonProtocol;
using Ankama.Cube.UI.Components;
using DG.Tweening;
using Google.Protobuf.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.Demo.UI
{
	public class StatBoard : MonoBehaviour
	{
		public struct PlayerStatData
		{
			public bool ally;

			public string name;

			public WeaponDefinition weaponDefinition;

			public GameStatistics.Types.PlayerStats playerStats;
		}

		public class PlayerStatDataComparer : IComparer<PlayerStatData>
		{
			public int Compare(PlayerStatData x, PlayerStatData y)
			{
				return x.playerStats.FightId.CompareTo(y.playerStats.FightId);
			}
		}

		[SerializeField]
		private StatPlayerLine m_playerLine;

		[SerializeField]
		private List<StatValueLine> m_statLines;

		[SerializeField]
		private Image m_versusImage;

		[SerializeField]
		private GameObject m_versusBloom;

		[SerializeField]
		private CanvasGroup m_teamScoreLine;

		[SerializeField]
		private RawTextField m_allyTeamScore;

		[SerializeField]
		private RawTextField m_opponentTeamScore;

		[SerializeField]
		private StatData m_statData;

		private Sequence m_openTweenSequence;

		private List<PlayerStatData> m_allies = new List<PlayerStatData>();

		private List<PlayerStatData> m_opponents = new List<PlayerStatData>();

		private List<FightStatType> m_availableStats = new List<FightStatType>();

		public IEnumerator Init(GameStatistics gameStatistics)
		{
			m_allies.Clear();
			m_opponents.Clear();
			m_availableStats.Clear();
			RepeatedField<GameStatistics.Types.PlayerStats> playerStats = gameStatistics.PlayerStats;
			for (int i = 0; i < playerStats.get_Count(); i++)
			{
				GameStatistics.Types.PlayerStats playerStats2 = playerStats.get_Item(i);
				for (int j = 0; j < playerStats2.Titles.get_Count(); j++)
				{
					FightStatType item = (FightStatType)playerStats2.Titles.get_Item(j);
					if (!m_availableStats.Contains(item))
					{
						m_availableStats.Add(item);
					}
				}
				if (GameStatus.GetFightStatus(playerStats2.FightId).TryGetEntity(playerStats2.PlayerId, out PlayerStatus entityStatus) && entityStatus.heroStatus != null)
				{
					PlayerStatData playerStatData = default(PlayerStatData);
					playerStatData.name = entityStatus.nickname;
					playerStatData.weaponDefinition = (WeaponDefinition)entityStatus.heroStatus.definition;
					playerStatData.playerStats = playerStats2;
					PlayerStatData item2 = playerStatData;
					if (GameStatus.localPlayerTeamIndex == entityStatus.teamIndex)
					{
						item2.ally = true;
						m_allies.Add(item2);
					}
					else
					{
						item2.ally = false;
						m_opponents.Add(item2);
					}
				}
			}
			PlayerStatDataComparer comparer = new PlayerStatDataComparer();
			m_allies.Sort(comparer);
			m_opponents.Sort(comparer);
			if (GameStatus.fightType == FightType.TeamVersus)
			{
				m_teamScoreLine.get_gameObject().SetActive(true);
				m_allyTeamScore.SetText(GameStatus.allyTeamPoints.ToString());
				m_opponentTeamScore.SetText(GameStatus.opponentTeamPoints.ToString());
			}
			else
			{
				m_teamScoreLine.get_gameObject().SetActive(false);
			}
			bool displayOpponent = m_opponents.Count > 0 || GameStatus.fightType != FightType.BossFight;
			m_versusImage.get_gameObject().SetActive(displayOpponent);
			m_versusBloom.get_gameObject().SetActive(displayOpponent);
			yield return m_playerLine.Init(m_allies, m_opponents, displayOpponent);
			m_statLines[0].Init(m_allies, m_opponents, FightStatType.TotalDamageDealt, displayOpponent);
			m_statLines[1].Init(m_allies, m_opponents, FightStatType.TotalDamageSustained, displayOpponent);
			m_statLines[2].Init(m_allies, m_opponents, FightStatType.PlayTime, displayOpponent);
			m_statLines[3].Init(m_allies, m_opponents, FightStatType.TotalKills, displayOpponent);
			InitOptionnalLine(m_statLines[4], FightStatType.CompanionGiven, displayOpponent);
			InitOptionnalLine(m_statLines[5], FightStatType.BudgetPointsDiff, displayOpponent);
			InitOptionnalLine(m_statLines[6], FightStatType.BudgetPointsWon, displayOpponent);
		}

		private void InitOptionnalLine(StatValueLine line, FightStatType type, bool displayOpponent)
		{
			if (m_availableStats.Contains(type))
			{
				line.Init(m_allies, m_opponents, type, displayOpponent);
				line.get_gameObject().SetActive(true);
			}
			else
			{
				line.get_gameObject().SetActive(false);
			}
		}

		public Sequence Open()
		{
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			if (m_openTweenSequence != null && TweenExtensions.IsActive(m_openTweenSequence))
			{
				TweenExtensions.Kill(m_openTweenSequence, false);
			}
			m_openTweenSequence = DOTween.Sequence();
			int num = m_statLines.Count + 2;
			float num2 = Mathf.Min(m_statData.openBoardDuration, m_statData.openBoardLineTweenDuration);
			float num3 = (m_statData.openBoardDuration - num2) / (float)num;
			float openBoardDelay = m_statData.openBoardDelay;
			m_playerLine.canvasGroup.set_alpha(0f);
			TweenSettingsExtensions.Insert(m_openTweenSequence, openBoardDelay, TweenSettingsExtensions.SetEase<Tweener>(DOTweenModuleUI.DOFade(m_playerLine.canvasGroup, 1f, num2), m_statData.openBoardLineTweenEase));
			openBoardDelay += num3;
			for (int i = 0; i < m_statLines.Count; i++)
			{
				StatValueLine statValueLine = m_statLines[i];
				statValueLine.canvasGroup.set_alpha(0f);
				TweenSettingsExtensions.Insert(m_openTweenSequence, openBoardDelay, TweenSettingsExtensions.SetEase<Tweener>(DOTweenModuleUI.DOFade(statValueLine.canvasGroup, 1f, num2), m_statData.openBoardLineTweenEase));
				openBoardDelay += num3;
			}
			m_teamScoreLine.set_alpha(0f);
			TweenSettingsExtensions.Insert(m_openTweenSequence, openBoardDelay, TweenSettingsExtensions.SetEase<Tweener>(DOTweenModuleUI.DOFade(m_teamScoreLine, 1f, num2), m_statData.openBoardLineTweenEase));
			return m_openTweenSequence;
		}

		public StatBoard()
			: this()
		{
		}
	}
}
