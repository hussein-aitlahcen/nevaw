using Ankama.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.UI.Fight.NotificationWindow
{
	public class NotificationWindowManager : MonoBehaviour
	{
		[SerializeField]
		private RectTransform m_windowsContainerTransform;

		[SerializeField]
		private NotificationWindow m_prefab;

		private static NotificationWindowManager s_manager;

		private List<NotificationWindow> m_windows = new List<NotificationWindow>();

		private void Awake()
		{
			s_manager = this;
		}

		private void OnDestroy()
		{
			s_manager = null;
		}

		private void _AddNotification(string notification)
		{
			NotificationWindow notificationWindow = Object.Instantiate<NotificationWindow>(m_prefab, m_windowsContainerTransform);
			notificationWindow.Open(notification);
			notificationWindow.OnClosed += OnClosed;
		}

		private void OnClosed(NotificationWindow window)
		{
			Object.Destroy(window);
		}

		public static void DisplayNotification(string notification)
		{
			if (s_manager == null)
			{
				Log.Error("Impossible to add a notification: s_manager == null", 47, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\FightRework\\NotificationWindow\\NotificationWindowManager.cs");
			}
			else
			{
				s_manager._AddNotification(notification);
			}
		}

		public NotificationWindowManager()
			: this()
		{
		}
	}
}
