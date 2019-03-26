using Ankama.Cube.Utility;
using Ankama.Utilities;
using UnityEngine;

namespace Ankama.Cube.Demo
{
	public class InactivityHandler : MonoBehaviour
	{
		private const float MAX_INACTIVITY_TIME = 300f;

		private float m_lastActivityTime;

		public static InactivityHandler instance
		{
			get;
			private set;
		}

		public static void UpdateActivity()
		{
			instance.UpdateActivityTime();
		}

		private void Awake()
		{
			if (instance != null)
			{
				Log.Error("Another instance exist", 25, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Demo\\Code\\InactivityHandler.cs");
			}
			else
			{
				instance = this;
			}
		}

		private void OnDestroy()
		{
			if (!(instance != this))
			{
				instance = null;
			}
		}

		private void OnEnable()
		{
			m_lastActivityTime = Time.get_time();
		}

		private void Update()
		{
			if (Time.get_time() - m_lastActivityTime > 300f)
			{
				m_lastActivityTime = float.MaxValue;
				StatesUtility.GotoLoginState();
			}
		}

		private void UpdateActivityTime()
		{
			m_lastActivityTime = Time.get_time();
		}

		public InactivityHandler()
			: this()
		{
		}
	}
}
