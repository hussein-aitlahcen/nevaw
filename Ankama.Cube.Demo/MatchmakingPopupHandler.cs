using Ankama.AssetManagement.StateManagement;
using Ankama.Cube.Code.UI;
using Ankama.Cube.Utility;
using System;
using System.Collections.Generic;

namespace Ankama.Cube.Demo
{
	public class MatchmakingPopupHandler
	{
		public enum MessageType
		{
			InvitationReceived,
			InvitationDeclined,
			InvitationFail,
			PlayerLeaveGroup,
			JoinGroupFail
		}

		private struct Message
		{
			public readonly MessageType type;

			public readonly FightPlayerInfo player;

			public readonly int popupInfoId;

			public Message(MessageType type, FightPlayerInfo player, int popupInfoId)
			{
				this.type = type;
				this.player = player;
				this.popupInfoId = popupInfoId;
			}
		}

		private readonly StateContext m_stateContext;

		private readonly List<Message> m_messageStack = new List<Message>();

		public static MatchmakingPopupHandler instance
		{
			get;
			private set;
		}

		public static void Init(StateContext state)
		{
			if (MatchmakingPopupHandler.instance == null)
			{
				MatchmakingPopupHandler.instance = new MatchmakingPopupHandler(state);
				PopupInfoManager instance = Singleton<PopupInfoManager>.instance;
				instance.onPopupBeginClosing = (Action<int>)Delegate.Combine(instance.onPopupBeginClosing, new Action<int>(MatchmakingPopupHandler.instance.OnPopupClosing));
			}
		}

		public static void Dispose()
		{
			if (MatchmakingPopupHandler.instance != null)
			{
				PopupInfoManager instance = Singleton<PopupInfoManager>.instance;
				instance.onPopupBeginClosing = (Action<int>)Delegate.Remove(instance.onPopupBeginClosing, new Action<int>(MatchmakingPopupHandler.instance.OnPopupClosing));
				MatchmakingPopupHandler.instance.RemoveAllStackedMessages();
				MatchmakingPopupHandler.instance = null;
			}
		}

		public MatchmakingPopupHandler(StateContext state)
		{
			m_stateContext = state;
		}

		private void OnPopupClosing(int id)
		{
			int num = m_messageStack.Count - 1;
			while (true)
			{
				if (num >= 0)
				{
					if (m_messageStack[num].popupInfoId == id)
					{
						break;
					}
					num--;
					continue;
				}
				return;
			}
			m_messageStack.RemoveAt(num);
		}

		public void RemoveAllStackedMessages()
		{
			for (int num = m_messageStack.Count - 1; num >= 0; num--)
			{
				PopupInfoManager.RemoveById(m_messageStack[num].popupInfoId);
			}
			m_messageStack.Clear();
		}

		public void ShowMessage(MessageType type, FightPlayerInfo player, Action noAction = null, Action yesAction = null)
		{
			string text = (player != null) ? player.Info.Nickname : "Player";
			PopupInfo info;
			int popupInfoId;
			switch (type)
			{
			case MessageType.InvitationReceived:
			{
				StateContext stateContext5 = m_stateContext;
				info = new PopupInfo
				{
					message = new RawTextData(22824, text),
					buttons = new ButtonData[2]
					{
						new ButtonData(9912, yesAction),
						new ButtonData(68421, noAction, closeOnClick: true, ButtonStyle.Negative)
					},
					selectedButton = 1,
					style = PopupStyle.Normal,
					useBlur = true
				};
				popupInfoId = PopupInfoManager.Show(stateContext5, info);
				break;
			}
			case MessageType.InvitationDeclined:
			{
				StateContext stateContext4 = m_stateContext;
				info = new PopupInfo
				{
					message = new RawTextData(32703, text),
					buttons = new ButtonData[1]
					{
						new ButtonData(27169)
					},
					selectedButton = 1,
					style = PopupStyle.Normal,
					useBlur = true
				};
				popupInfoId = PopupInfoManager.Show(stateContext4, info);
				break;
			}
			case MessageType.InvitationFail:
			{
				StateContext stateContext3 = m_stateContext;
				info = new PopupInfo
				{
					message = new RawTextData(18236, text),
					buttons = new ButtonData[1]
					{
						new ButtonData(27169)
					},
					style = PopupStyle.Error,
					useBlur = true
				};
				popupInfoId = PopupInfoManager.Show(stateContext3, info);
				break;
			}
			case MessageType.PlayerLeaveGroup:
			{
				StateContext stateContext2 = m_stateContext;
				info = new PopupInfo
				{
					message = new RawTextData(34361, text),
					buttons = new ButtonData[1]
					{
						new ButtonData(27169)
					},
					style = PopupStyle.Normal,
					useBlur = true
				};
				popupInfoId = PopupInfoManager.Show(stateContext2, info);
				break;
			}
			case MessageType.JoinGroupFail:
			{
				StateContext stateContext = m_stateContext;
				info = new PopupInfo
				{
					message = new RawTextData(80127),
					buttons = new ButtonData[1]
					{
						new ButtonData(27169)
					},
					style = PopupStyle.Normal,
					useBlur = true
				};
				popupInfoId = PopupInfoManager.Show(stateContext, info);
				break;
			}
			default:
				throw new ArgumentOutOfRangeException("type", type, null);
			}
			m_messageStack.Add(new Message(type, player, popupInfoId));
		}

		public void RemoveAllMessageOfType(MessageType type)
		{
			List<Message> messageStack = m_messageStack;
			for (int num = messageStack.Count - 1; num >= 0; num--)
			{
				Message message = messageStack[num];
				if (message.type == type)
				{
					messageStack.RemoveAt(num);
					PopupInfoManager.RemoveById(message.popupInfoId);
				}
			}
		}

		public void RemoveInvitationMessageFrom(long playerId)
		{
			List<Message> messageStack = m_messageStack;
			int num = messageStack.Count - 1;
			Message message;
			while (true)
			{
				if (num >= 0)
				{
					message = messageStack[num];
					if (message.type == MessageType.InvitationReceived && message.player.Uid == playerId)
					{
						break;
					}
					num--;
					continue;
				}
				return;
			}
			messageStack.RemoveAt(num);
			PopupInfoManager.RemoveById(message.popupInfoId);
		}
	}
}
