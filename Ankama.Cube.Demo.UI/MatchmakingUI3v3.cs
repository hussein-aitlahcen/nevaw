using Ankama.Cube.Data;
using Ankama.Cube.Protocols.FightCommonProtocol;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Demo.UI
{
	public class MatchmakingUI3v3 : MatchmakingUIGroup
	{
		[SerializeField]
		private PlayerPanel[] m_opponentPanels;

		public void SetOpponents(IList<FightInfo.Types.Player> opponents)
		{
			for (int i = 0; i < opponents.Count && i < m_opponentPanels.Length; i++)
			{
				FightInfo.Types.Player player = opponents[i];
				Tuple<SquadDefinition, SquadFakeData> squadDataByWeaponId = GetSquadDataByWeaponId(player.WeaponId.Value);
				m_opponentPanels[i].Set(player.Name, player.Level, squadDataByWeaponId.Item2);
			}
		}
	}
}
