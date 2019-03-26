using Ankama.Cube.UI.Components;
using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ankama.Cube.Demo.UI
{
	public class MatchmakingPlayerElement : MonoBehaviour
	{
		public enum InviteState
		{
			Empty,
			Normal,
			Leaving,
			Invited_By_Me,
			Invite_Me
		}

		[SerializeField]
		private GameObject m_standardStuffParent;

		[SerializeField]
		private GameObject m_leaveStuffParent;

		[SerializeField]
		private Button m_inviteButton;

		[SerializeField]
		private Button m_cancelInviteButton;

		[SerializeField]
		private RawTextField m_name;

		[SerializeField]
		private CanvasGroup m_canvasGroup;

		private FightPlayerInfo m_player;

		private InviteState m_inviteState;

		public Action<MatchmakingPlayerElement, FightPlayerInfo> onInvite;

		public Action<MatchmakingPlayerElement, FightPlayerInfo> onCancelInvite;

		public InviteState state => m_inviteState;

		public FightPlayerInfo player => m_player;

		public bool interactable
		{
			set
			{
				m_canvasGroup.set_interactable(value);
			}
		}

		private unsafe void Start()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Expected O, but got Unknown
			m_inviteButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_cancelInviteButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		}

		public void SetState(InviteState state)
		{
			m_inviteState = state;
			switch (m_inviteState)
			{
			case InviteState.Empty:
				m_standardStuffParent.SetActive(true);
				m_leaveStuffParent.SetActive(false);
				m_inviteButton.get_gameObject().SetActive(false);
				m_cancelInviteButton.get_gameObject().SetActive(false);
				m_name.get_gameObject().SetActive(false);
				break;
			case InviteState.Leaving:
				m_standardStuffParent.SetActive(false);
				m_leaveStuffParent.SetActive(true);
				break;
			case InviteState.Normal:
				m_standardStuffParent.SetActive(true);
				m_leaveStuffParent.SetActive(false);
				m_inviteButton.set_interactable(true);
				m_inviteButton.get_gameObject().SetActive(true);
				m_cancelInviteButton.get_gameObject().SetActive(false);
				break;
			case InviteState.Invited_By_Me:
				m_standardStuffParent.SetActive(true);
				m_leaveStuffParent.SetActive(false);
				m_inviteButton.get_gameObject().SetActive(false);
				m_cancelInviteButton.get_gameObject().SetActive(true);
				break;
			case InviteState.Invite_Me:
				m_standardStuffParent.SetActive(true);
				m_leaveStuffParent.SetActive(false);
				m_inviteButton.set_interactable(false);
				m_inviteButton.get_gameObject().SetActive(true);
				m_cancelInviteButton.get_gameObject().SetActive(false);
				break;
			}
		}

		public void SetPlayer([CanBeNull] FightPlayerInfo player)
		{
			m_player = player;
			if (player != null)
			{
				m_name.SetText(player.Info.Nickname);
				m_name.get_gameObject().SetActive(true);
				SetState(InviteState.Normal);
			}
			else
			{
				SetState(InviteState.Empty);
			}
		}

		public MatchmakingPlayerElement()
			: this()
		{
		}
	}
}
