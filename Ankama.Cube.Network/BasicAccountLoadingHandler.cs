using Ankama.Cube.TEMPFastEnterMatch.Player;
using System;

namespace Ankama.Cube.Network
{
	public class BasicAccountLoadingHandler : IAccountLoadingHandler, IDisposable
	{
		private readonly PlayerDataFrame m_playerDataFrame;

		public event Action OnAccountLoaded;

		public BasicAccountLoadingHandler()
		{
			m_playerDataFrame = new PlayerDataFrame();
			m_playerDataFrame.OnPlayerAccountLoaded += OnPlayerAccountLoaded;
		}

		private void OnPlayerAccountLoaded(bool oldFightFound)
		{
			this.OnAccountLoaded?.Invoke();
		}

		public void LoadAccount()
		{
			PlayerData.Clean();
			m_playerDataFrame.GetPlayerInitialData();
		}

		public void Dispose()
		{
			m_playerDataFrame.Dispose();
		}
	}
}
