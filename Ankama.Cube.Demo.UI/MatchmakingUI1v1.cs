using Ankama.Cube.Data;
using Ankama.Cube.Protocols.FightCommonProtocol;
using Ankama.Cube.Protocols.PlayerProtocol;
using Ankama.Cube.TEMPFastEnterMatch.Player;
using System;
using UnityEngine;

namespace Ankama.Cube.Demo.UI
{
	public class MatchmakingUI1v1 : AbstractMatchmakingUI
	{
		[SerializeField]
		private PlayerPanel m_playerPanel;

		[SerializeField]
		private PlayerPanel m_opponentPanel;

		private void Start()
		{
			PlayerInvitationUI playerInvitationPanel = m_playerInvitationPanel;
			playerInvitationPanel.onInvitationSend = (Action<FightPlayerInfo>)Delegate.Combine(playerInvitationPanel.onInvitationSend, (Action<FightPlayerInfo>)delegate
			{
				UpdateInactivityTime();
			});
			PlayerInvitationUI playerInvitationPanel2 = m_playerInvitationPanel;
			playerInvitationPanel2.onInvitationCanceled = (Action<FightPlayerInfo>)Delegate.Combine(playerInvitationPanel2.onInvitationCanceled, (Action<FightPlayerInfo>)delegate
			{
				UpdateInactivityTime();
			});
		}

		public override void Init()
		{
			PlayerData instance = PlayerData.instance;
			int currentDeckId = instance.currentDeckId;
			Tuple<SquadDefinition, SquadFakeData> squadDataByDeckId = GetSquadDataByDeckId(currentDeckId);
			int level = 6;
			if (instance.TryGetDeckById(currentDeckId, out DeckInfo deckInfo))
			{
				level = deckInfo.GetLevel(instance.weaponInventory);
			}
			m_playerPanel.Set(instance.nickName, level, squadDataByDeckId.Item2);
			m_playerInvitationPanel.Init();
		}

		public void SetOpponent(FightInfo.Types.Player opponent)
		{
			Tuple<SquadDefinition, SquadFakeData> squadDataByWeaponId = GetSquadDataByWeaponId(opponent.WeaponId.Value);
			m_opponentPanel.Set(opponent.Name, opponent.Level, squadDataByWeaponId.Item2);
		}
	}
}
