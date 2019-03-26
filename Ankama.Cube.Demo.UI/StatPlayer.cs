using Ankama.Cube.Data;
using Ankama.Cube.Protocols.FightCommonProtocol;
using Ankama.Cube.UI.Components;
using Ankama.Cube.Utility;
using Google.Protobuf.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.Demo.UI
{
	public class StatPlayer : AbstractStat
	{
		[SerializeField]
		private Transform m_illuContainer;

		[SerializeField]
		private ImageLoader m_illuImageLoader;

		[SerializeField]
		private RawTextField m_nameText;

		[SerializeField]
		private Image m_crownImage;

		[SerializeField]
		private RawTextField m_bestPlayerText;

		[SerializeField]
		private List<Image> m_titleIcons;

		[SerializeField]
		private Image m_deadIcon;

		[SerializeField]
		private StatData m_statData;

		public IEnumerator Init(StatBoard.PlayerStatData statData)
		{
			m_nameText.SetText(statData.name);
			Color color = statData.ally ? m_statData.allyColor : m_statData.opponentColor;
			m_nameText.color = color;
			m_bestPlayerText.color = color;
			m_crownImage.set_color(color);
			GameStatistics.Types.PlayerStats playerStats = statData.playerStats;
			bool flag = statData.playerStats.Titles.Contains(0);
			m_bestPlayerText.get_gameObject().SetActive(flag);
			float num = flag ? m_statData.illuMvpScale : m_statData.illuNeutralScale;
			m_illuContainer.set_localScale(Vector3.get_one() * num);
			int num2 = default(int);
			if (playerStats.Stats.TryGetValue(12, ref num2))
			{
				m_deadIcon.get_gameObject().SetActive(num2 > 0);
			}
			else
			{
				m_deadIcon.get_gameObject().SetActive(true);
			}
			yield return LoadIllu(statData.weaponDefinition);
		}

		private void SetTitles(RepeatedField<int> titles)
		{
			int num = 0;
			for (int i = 0; i < titles.get_Count(); i++)
			{
				FightStatType fightStatType = (FightStatType)titles.get_Item(i);
				if (fightStatType != 0)
				{
					if (i >= m_titleIcons.Count)
					{
						Image val = Object.Instantiate<Image>(m_titleIcons[0]);
						val.get_transform().SetParent(m_titleIcons[0].get_transform().get_parent());
						m_titleIcons.Add(val);
					}
					Image obj = m_titleIcons[i];
					obj.get_gameObject().SetActive(true);
					obj.set_sprite(m_statData.GetIcon(fightStatType));
					num++;
				}
			}
			for (int j = num; j < m_titleIcons.Count; j++)
			{
				m_titleIcons[j].get_gameObject().SetActive(false);
			}
		}

		private IEnumerator LoadIllu(WeaponDefinition weaponDefinition)
		{
			m_illuImageLoader.Setup(weaponDefinition.GetIllustrationReference(), AssetBundlesUtility.GetUICharacterResourcesBundleName());
			yield break;
		}
	}
}
