using Ankama.Cube.Data;
using Ankama.Cube.UI.Components;
using System;
using UnityEngine;

namespace Ankama.Cube.Demo.UI
{
	public class StatValue : AbstractStat
	{
		[SerializeField]
		private RawTextField m_valueText;

		[SerializeField]
		private StatData m_statData;

		public void Init(StatBoard.PlayerStatData statData, FightStatType type)
		{
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			int num = default(int);
			if (statData.playerStats.Stats.TryGetValue((int)type, ref num))
			{
				if (type == FightStatType.PlayTime)
				{
					TimeSpan timeSpan = TimeSpan.FromSeconds(num);
					string text = (timeSpan.Hours > 0) ? timeSpan.ToString("hh\\:mm\\:ss") : timeSpan.ToString("mm\\:ss");
					m_valueText.SetText(text);
				}
				else
				{
					m_valueText.SetText(num.ToString());
				}
			}
			else
			{
				m_valueText.SetText("-");
			}
			Color color = statData.playerStats.Titles.Contains((int)type) ? m_statData.bestValueColor : m_statData.neutralValueColor;
			m_valueText.color = color;
		}
	}
}
