using Ankama.Cube.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.Demo.UI
{
	public class StatValueLine : AbstractStatLine<StatValue>
	{
		public void Init(List<StatBoard.PlayerStatData> allies, List<StatBoard.PlayerStatData> opponents, FightStatType type, bool displayOpponents)
		{
			InitList(m_alliesStats, m_alliesGroup, allies, type);
			if (displayOpponents)
			{
				m_opponentsGroup.get_gameObject().SetActive(true);
				InitList(m_opponentStats, m_opponentsGroup, opponents, type);
			}
			else
			{
				m_opponentsGroup.get_gameObject().SetActive(false);
			}
		}

		private void InitList(List<StatValue> stats, LayoutGroup group, List<StatBoard.PlayerStatData> statDatas, FightStatType type)
		{
			int num = 0;
			for (int i = 0; i < statDatas.Count; i++)
			{
				StatBoard.PlayerStatData statData = statDatas[i];
				if (i >= stats.Count)
				{
					StatValue statValue = Object.Instantiate<StatValue>(stats[0]);
					statValue.get_transform().SetParent(group.get_transform());
					stats.Add(statValue);
				}
				StatValue statValue2 = stats[i];
				statValue2.Init(statData, type);
				statValue2.get_gameObject().SetActive(true);
				num++;
			}
			for (int j = num; j < stats.Count; j++)
			{
				stats[j].get_gameObject().SetActive(false);
			}
		}
	}
}
