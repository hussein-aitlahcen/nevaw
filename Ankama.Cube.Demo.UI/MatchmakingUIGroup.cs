using Ankama.Cube.Configuration;
using Ankama.Cube.Data;
using Ankama.Cube.Protocols.PlayerProtocol;
using Ankama.Cube.TEMPFastEnterMatch.Player;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ankama.Cube.Demo.UI
{
	public abstract class MatchmakingUIGroup : AbstractMatchmakingUI
	{
		[SerializeField]
		private PlayerPanel[] m_playerPanels;

		[SerializeField]
		private GameObject m_matchmakingButtonComponent;

		[SerializeField]
		private Button m_launchMatchmakingButton;

		[SerializeField]
		private Button m_cancelMatchmakingButton;

		private List<FightPlayerInfo> m_allies = new List<FightPlayerInfo>();

		private unsafe void Start()
		{
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Expected O, but got Unknown
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Expected O, but got Unknown
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
			m_launchMatchmakingButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_cancelMatchmakingButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_matchmakingButtonComponent.SetActive(ApplicationConfig.debugMode);
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
			m_playerPanels[0].Set(instance.nickName, level, squadDataByDeckId.Item2);
			for (int i = 1; i < m_playerPanels.Length; i++)
			{
				m_playerPanels[i].SetEmpty();
			}
			m_launchMatchmakingButton.get_gameObject().SetActive(true);
			m_cancelMatchmakingButton.get_gameObject().SetActive(false);
			m_playerInvitationPanel.Init();
			m_allies.Clear();
			m_playerInvitationPanel.SetInvitationsLocked(value: false);
		}

		public void OnJoinGroupFail(long inviterId)
		{
			MatchmakingPopupHandler.instance.ShowMessage(MatchmakingPopupHandler.MessageType.JoinGroupFail, null);
		}

		public void OnGroupLeft(long oldGroup)
		{
			for (int num = m_allies.Count - 1; num >= 0; num--)
			{
				FightPlayerInfo fightPlayerInfo = m_allies[num];
				RemoveAlly(fightPlayerInfo);
				m_allies.Remove(fightPlayerInfo);
			}
		}

		public void OnGroupModified(long playerId, IList<FightPlayerInfo> players)
		{
			for (int num = m_allies.Count - 1; num >= 0; num--)
			{
				FightPlayerInfo fightPlayerInfo = m_allies[num];
				if (!PlayerListContain(players, fightPlayerInfo.Uid))
				{
					RemoveAlly(fightPlayerInfo);
					m_allies.Remove(fightPlayerInfo);
				}
			}
			for (int i = 0; i < players.Count; i++)
			{
				FightPlayerInfo fightPlayerInfo2 = players[i];
				if (playerId != fightPlayerInfo2.Uid && !PlayerListContain(m_allies, fightPlayerInfo2.Uid))
				{
					AddAlly(fightPlayerInfo2);
					m_allies.Add(fightPlayerInfo2);
				}
			}
			m_playerInvitationPanel.RemoveAllReceivedInvitations();
			m_playerInvitationPanel.SetInvitationsLocked(m_allies.Count + 1 >= m_playerPanels.Length);
			InactivityHandler.UpdateActivity();
		}

		private bool PlayerListContain(IList<FightPlayerInfo> players, long id)
		{
			for (int i = 0; i < players.Count; i++)
			{
				FightPlayerInfo fightPlayerInfo = players[i];
				if (id == fightPlayerInfo.Uid)
				{
					return true;
				}
			}
			return false;
		}

		private void RemoveAlly(FightPlayerInfo player)
		{
			MatchmakingPopupHandler.instance.ShowMessage(MatchmakingPopupHandler.MessageType.PlayerLeaveGroup, player);
			int num = 1;
			while (true)
			{
				if (num < m_playerPanels.Length)
				{
					if (m_playerPanels[num].playerId == player.Uid)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			m_playerPanels[num].SetEmpty(tween: true);
		}

		private void AddAlly(FightPlayerInfo player)
		{
			int num = 1;
			while (true)
			{
				if (num < m_playerPanels.Length)
				{
					if (m_playerPanels[num].isEmpty)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			Tuple<SquadDefinition, SquadFakeData> squadDataByWeaponId = GetSquadDataByWeaponId(player.WeaponId.Value);
			m_playerPanels[num].Set(player, player.Level, squadDataByWeaponId.Item2, tween: true);
		}

		private void OnLaunchMatchmakingButtonClick()
		{
			m_launchMatchmakingButton.get_gameObject().SetActive(false);
			m_cancelMatchmakingButton.get_gameObject().SetActive(true);
			m_playerInvitationPanel.SetInvitationsLocked(value: true);
			onLaunchMatchmakingClick?.Invoke();
		}

		private void OnCancelMatchmakingButtonClick()
		{
			m_launchMatchmakingButton.get_gameObject().SetActive(true);
			m_cancelMatchmakingButton.get_gameObject().SetActive(false);
			m_playerInvitationPanel.SetInvitationsLocked(m_allies.Count + 1 >= m_playerPanels.Length);
			onCancelMatchmakingClick?.Invoke();
		}
	}
}
