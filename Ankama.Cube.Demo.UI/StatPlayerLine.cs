using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.Demo.UI
{
	public class StatPlayerLine : AbstractStatLine<StatPlayer>
	{
		public IEnumerator Init(List<StatBoard.PlayerStatData> allies, List<StatBoard.PlayerStatData> opponents, bool displayOpponents)
		{
			yield return InitList(m_alliesStats, m_alliesGroup, allies);
			if (displayOpponents)
			{
				m_opponentsGroup.get_gameObject().SetActive(true);
				yield return InitList(m_opponentStats, m_opponentsGroup, opponents);
			}
			else
			{
				m_opponentsGroup.get_gameObject().SetActive(false);
			}
		}

		private IEnumerator InitList(List<StatPlayer> stats, LayoutGroup group, List<StatBoard.PlayerStatData> statDatas)
		{
			int statDataCount = 0;
			for (int i = 0; i < statDatas.Count; i++)
			{
				StatBoard.PlayerStatData statData = statDatas[i];
				if (i >= stats.Count)
				{
					StatPlayer statPlayer = Object.Instantiate<StatPlayer>(stats[0]);
					statPlayer.get_transform().SetParent(group.get_transform());
					stats.Add(statPlayer);
				}
				StatPlayer stat = stats[i];
				yield return stat.Init(statData);
				stat.get_gameObject().SetActive(true);
				statDataCount++;
			}
			for (int j = statDataCount; j < stats.Count; j++)
			{
				stats[j].get_gameObject().SetActive(false);
			}
		}
	}
}
