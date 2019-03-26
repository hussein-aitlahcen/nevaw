using Ankama.AssetManagement;
using Ankama.AssetManagement.StateManagement;
using Ankama.Cube.Utility;
using Ankama.Utilities;
using System;
using System.Collections.Generic;

namespace Ankama.Cube.Code.UI
{
	public class PopupInfoManager : Singleton<PopupInfoManager>
	{
		public struct StackedMessage
		{
			public int id;

			public PopupInfo info;

			public StateContext parentState;

			public StackedMessage(int id, PopupInfo info, StateContext parentState)
			{
				this.id = id;
				this.info = info;
				this.parentState = parentState;
			}
		}

		private List<StackedMessage> m_messages = new List<StackedMessage>();

		private StackedMessage m_displayedMessage;

		private PopupInfoState m_popupState;

		private int m_stackedId;

		public Action<int> onPopupDisplayed;

		public Action<int> onPopupBeginClosing;

		public bool isPopupDisplaying
		{
			get
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Invalid comparison between Unknown and I4
				if (m_popupState != null)
				{
					return (int)m_popupState.get_loadState() != 9;
				}
				return false;
			}
		}

		public static int Show(StateContext parentState, PopupInfo info)
		{
			return Singleton<PopupInfoManager>.instance.Add(parentState, info);
		}

		public static void RemoveById(int id)
		{
			Singleton<PopupInfoManager>.instance.Remove(id);
		}

		public static void ClearAllMessages()
		{
			Singleton<PopupInfoManager>.instance.ClearAll();
		}

		private int Add(StateContext parentState, PopupInfo info)
		{
			if (parentState is PopupInfoState)
			{
				parentState = parentState.get_parent();
			}
			m_stackedId++;
			m_messages.Add(new StackedMessage(m_stackedId, info, parentState));
			if (m_messages.Count == 1)
			{
				Singleton<SceneEventListener>.instance.AddUpdateListener(Update);
			}
			return m_stackedId;
		}

		private void Remove(int id)
		{
			Log.Info($"Remove {id}", 76, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\PopupInfo\\PopupInfoManager.cs");
			for (int num = m_messages.Count - 1; num >= 0; num--)
			{
				if (m_messages[num].id == id)
				{
					m_messages.RemoveAt(num);
					break;
				}
			}
			if (isPopupDisplaying && m_displayedMessage.id == id)
			{
				m_popupState.Close();
			}
		}

		private void ClearAll()
		{
			m_messages.Clear();
			if (isPopupDisplaying)
			{
				m_popupState.Close();
			}
		}

		private void Update()
		{
			if (m_messages.Count == 0)
			{
				Singleton<SceneEventListener>.instance.RemoveUpdateListener(Update);
			}
			if (m_messages.Count > 0 && !isPopupDisplaying)
			{
				DisplayStackedMessage();
			}
		}

		private void DisplayStackedMessage()
		{
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Invalid comparison between Unknown and I4
			if (m_messages.Count != 0 && !isPopupDisplaying)
			{
				m_displayedMessage = m_messages[0];
				m_messages.RemoveAt(0);
				StateContext parentState = m_displayedMessage.parentState;
				if (parentState != null && (int)parentState.get_loadState() <= 4)
				{
					m_popupState = new PopupInfoState(m_displayedMessage.info);
					parentState.SetChildState(m_popupState, 0);
					PopupInfoState popupState = m_popupState;
					popupState.onClose = (Action)Delegate.Combine(popupState.onClose, (Action)delegate
					{
						onPopupBeginClosing?.Invoke(m_displayedMessage.id);
					});
					onPopupDisplayed?.Invoke(m_displayedMessage.id);
				}
			}
		}

		public static void ShowApplicationError(PopupInfo popupInfo)
		{
			StateLayer val = default(StateLayer);
			if (!StateManager.TryGetLayer("application", ref val))
			{
				val = StateManager.AddLayer("application");
			}
			StateManager.SetActiveInputLayer(val);
			popupInfo.style = PopupStyle.Error;
			Show(val, popupInfo);
		}
	}
}
