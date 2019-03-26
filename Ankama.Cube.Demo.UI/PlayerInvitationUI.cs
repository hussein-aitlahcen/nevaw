using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Demo.UI
{
	public class PlayerInvitationUI : MonoBehaviour
	{
		[SerializeField]
		private CanvasGroup m_playersCanvasGroup;

		[SerializeField]
		private List<MatchmakingPlayerElement> m_playersElements;

		public Action<bool> onSearchingValueChanged;

		public Action<FightPlayerInfo> onInvitationSend;

		public Action<FightPlayerInfo> onInvitationCanceled;

		public Action<FightPlayerInfo> onInvitationDeclined;

		public Action<FightPlayerInfo> onInvitationAccepted;

		public void Init()
		{
			for (int i = 0; i < m_playersElements.Count; i++)
			{
				MatchmakingPlayerElement matchmakingPlayerElement = m_playersElements[i];
				matchmakingPlayerElement.SetPlayer(null);
				matchmakingPlayerElement.SetState(MatchmakingPlayerElement.InviteState.Empty);
			}
		}

		private void Start()
		{
			UpdateInviteResearch();
			for (int i = 0; i < m_playersElements.Count; i++)
			{
				MatchmakingPlayerElement matchmakingPlayerElement = m_playersElements[i];
				matchmakingPlayerElement.onInvite = (Action<MatchmakingPlayerElement, FightPlayerInfo>)Delegate.Combine(matchmakingPlayerElement.onInvite, new Action<MatchmakingPlayerElement, FightPlayerInfo>(OnSendInvitationClick));
				matchmakingPlayerElement.onCancelInvite = (Action<MatchmakingPlayerElement, FightPlayerInfo>)Delegate.Combine(matchmakingPlayerElement.onCancelInvite, new Action<MatchmakingPlayerElement, FightPlayerInfo>(OnCancelInvitationClick));
			}
		}

		private void UpdateInviteResearch()
		{
			int num = 0;
			for (int i = 0; i < m_playersElements.Count; i++)
			{
				MatchmakingPlayerElement matchmakingPlayerElement = m_playersElements[i];
				if (matchmakingPlayerElement.state == MatchmakingPlayerElement.InviteState.Invited_By_Me || matchmakingPlayerElement.state == MatchmakingPlayerElement.InviteState.Invite_Me)
				{
					num++;
				}
			}
			onSearchingValueChanged?.Invoke(num > 0);
		}

		public void SetInvitationsLocked(bool value)
		{
			m_playersCanvasGroup.set_interactable(!value);
		}

		private void OnSendInvitationClick(MatchmakingPlayerElement element, FightPlayerInfo player)
		{
			element.SetState(MatchmakingPlayerElement.InviteState.Invited_By_Me);
			UpdateInviteResearch();
			onInvitationSend?.Invoke(player);
		}

		private void OnCancelInvitationClick(MatchmakingPlayerElement element, FightPlayerInfo player)
		{
			element.SetState(MatchmakingPlayerElement.InviteState.Normal);
			UpdateInviteResearch();
			onInvitationCanceled?.Invoke(player);
		}

		public void ReceiveInvitationResult(long uid, bool result)
		{
			if (!result)
			{
				MatchmakingPlayerElement matchmakingPlayerElement = FindElement(uid);
				if (matchmakingPlayerElement != null && matchmakingPlayerElement.state == MatchmakingPlayerElement.InviteState.Invited_By_Me)
				{
					FightPlayerInfo player = matchmakingPlayerElement.player;
					MatchmakingPopupHandler.instance.ShowMessage(MatchmakingPopupHandler.MessageType.InvitationFail, player);
					matchmakingPlayerElement.SetState(MatchmakingPlayerElement.InviteState.Normal);
					UpdateInviteResearch();
				}
			}
		}

		private void OnInvitationAccepted(FightPlayerInfo player)
		{
			onInvitationAccepted?.Invoke(player);
		}

		private void OnInvitationDeclined(FightPlayerInfo player)
		{
			MatchmakingPlayerElement matchmakingPlayerElement = FindElement(player.Uid);
			if (matchmakingPlayerElement != null)
			{
				matchmakingPlayerElement.SetState(MatchmakingPlayerElement.InviteState.Normal);
				UpdateInviteResearch();
			}
			onInvitationDeclined?.Invoke(player);
		}

		public void AddPlayer(FightPlayerInfo player)
		{
			MatchmakingPlayerElement matchmakingPlayerElement = FirstFreeElement();
			if (matchmakingPlayerElement != null)
			{
				matchmakingPlayerElement.SetPlayer(player);
				matchmakingPlayerElement.SetState(MatchmakingPlayerElement.InviteState.Normal);
			}
			UpdateInviteResearch();
		}

		public void RemovePlayer(long uid)
		{
			MatchmakingPlayerElement matchmakingPlayerElement = FindElement(uid);
			if (matchmakingPlayerElement != null)
			{
				matchmakingPlayerElement.SetPlayer(null);
				matchmakingPlayerElement.SetState(MatchmakingPlayerElement.InviteState.Leaving);
			}
			UpdateInviteResearch();
		}

		public void RemoveAllPlayers()
		{
			for (int i = 0; i < m_playersElements.Count; i++)
			{
				MatchmakingPlayerElement matchmakingPlayerElement = m_playersElements[i];
				matchmakingPlayerElement.SetPlayer(null);
				matchmakingPlayerElement.SetState(MatchmakingPlayerElement.InviteState.Empty);
			}
			MatchmakingPopupHandler.instance.RemoveAllStackedMessages();
			UpdateInviteResearch();
		}

		public void InvitationReceiveFrom(FightPlayerInfo player)
		{
			MatchmakingPlayerElement matchmakingPlayerElement = FindElement(player.Uid);
			if (matchmakingPlayerElement != null)
			{
				matchmakingPlayerElement.SetState(MatchmakingPlayerElement.InviteState.Invite_Me);
				UpdateInviteResearch();
			}
			MatchmakingPopupHandler.instance.ShowMessage(MatchmakingPopupHandler.MessageType.InvitationReceived, player, delegate
			{
				OnInvitationDeclined(player);
			}, delegate
			{
				OnInvitationAccepted(player);
			});
		}

		public void InvitationCanceledFrom(long uid)
		{
			MatchmakingPopupHandler.instance.RemoveInvitationMessageFrom(uid);
			MatchmakingPlayerElement matchmakingPlayerElement = FindElement(uid);
			if (matchmakingPlayerElement != null)
			{
				matchmakingPlayerElement.SetState(MatchmakingPlayerElement.InviteState.Normal);
				UpdateInviteResearch();
			}
		}

		public void RemoveAllReceivedInvitations()
		{
			MatchmakingPopupHandler.instance.RemoveAllMessageOfType(MatchmakingPopupHandler.MessageType.InvitationReceived);
			for (int i = 0; i < m_playersElements.Count; i++)
			{
				MatchmakingPlayerElement matchmakingPlayerElement = m_playersElements[i];
				if (matchmakingPlayerElement.state == MatchmakingPlayerElement.InviteState.Invite_Me)
				{
					matchmakingPlayerElement.SetState(MatchmakingPlayerElement.InviteState.Normal);
				}
			}
		}

		public void InvitationAnswerFrom(long uid, bool value)
		{
			if (!value)
			{
				MatchmakingPlayerElement matchmakingPlayerElement = FindElement(uid);
				if (matchmakingPlayerElement != null)
				{
					MatchmakingPopupHandler.instance.ShowMessage(MatchmakingPopupHandler.MessageType.InvitationDeclined, matchmakingPlayerElement.player);
					matchmakingPlayerElement.SetState(MatchmakingPlayerElement.InviteState.Normal);
					UpdateInviteResearch();
				}
			}
		}

		private MatchmakingPlayerElement FirstFreeElement()
		{
			for (int i = 0; i < m_playersElements.Count; i++)
			{
				MatchmakingPlayerElement matchmakingPlayerElement = m_playersElements[i];
				if (matchmakingPlayerElement.state == MatchmakingPlayerElement.InviteState.Empty || matchmakingPlayerElement.state == MatchmakingPlayerElement.InviteState.Leaving)
				{
					return matchmakingPlayerElement;
				}
			}
			MatchmakingPlayerElement matchmakingPlayerElement2 = Object.Instantiate<MatchmakingPlayerElement>(m_playersElements[0]);
			matchmakingPlayerElement2.get_gameObject().SetActive(true);
			matchmakingPlayerElement2.get_transform().SetParent(m_playersElements[0].get_transform().get_parent(), false);
			matchmakingPlayerElement2.SetState(MatchmakingPlayerElement.InviteState.Empty);
			m_playersElements.Add(matchmakingPlayerElement2);
			return matchmakingPlayerElement2;
		}

		private MatchmakingPlayerElement FindElement(long uid)
		{
			for (int num = m_playersElements.Count - 1; num >= 0; num--)
			{
				MatchmakingPlayerElement matchmakingPlayerElement = m_playersElements[num];
				if (matchmakingPlayerElement.player != null && matchmakingPlayerElement.player.Uid == uid)
				{
					return matchmakingPlayerElement;
				}
			}
			return null;
		}

		public PlayerInvitationUI()
			: this()
		{
		}
	}
}
