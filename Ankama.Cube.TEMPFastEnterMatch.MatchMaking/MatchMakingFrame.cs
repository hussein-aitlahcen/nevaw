using Ankama.Cube.Data;
using Ankama.Cube.Network;
using Ankama.Cube.Protocols.FightCommonProtocol;
using Ankama.Cube.Protocols.FightPreparationProtocol;
using Ankama.Cube.Protocols.FightProtocol;
using Ankama.Cube.Protocols.PlayerProtocol;
using System;

namespace Ankama.Cube.TEMPFastEnterMatch.MatchMaking
{
	public class MatchMakingFrame : CubeMessageFrame
	{
		public Action<FightInfo> onGameCreated;

		public Action onGameCanceled;

		public Action onGameError;

		private int m_lastFightDefIdRequested;

		public event Action<ChangeGodResultEvent> OnChangedGod;

		public event Action<SelectDeckAndWeaponResultEvent> OnSelectedWeaponAndDeck;

		public MatchMakingFrame()
		{
			base.WhenReceiveEnqueue<FightStartedEvent>((Action<FightStartedEvent>)OnFightStartedEvent);
			base.WhenReceiveEnqueue<FightNotStartedEvent>((Action<FightNotStartedEvent>)OnFightNotStartedEvent);
			base.WhenReceiveEnqueue<ChangeGodResultEvent>((Action<ChangeGodResultEvent>)OnChangeGodResultEvent);
			base.WhenReceiveEnqueue<LaunchMatchmakingResultEvent>((Action<LaunchMatchmakingResultEvent>)OnLaunchMatchmakingResultEvent);
			base.WhenReceiveEnqueue<MatchmakingStartedEvent>((Action<MatchmakingStartedEvent>)OnMatchmakingStartedEvent);
			base.WhenReceiveEnqueue<MatchmakingStoppedEvent>((Action<MatchmakingStoppedEvent>)OnMatchmakingStoppedEvent);
			base.WhenReceiveEnqueue<MatchmakingSuccessEvent>((Action<MatchmakingSuccessEvent>)OnMatchmakingSuccessEvent);
			base.WhenReceiveEnqueue<SelectDeckAndWeaponResultEvent>((Action<SelectDeckAndWeaponResultEvent>)OnSelectDeckAndWeaponResultHandler);
			base.WhenReceiveEnqueue<FightGroupUpdatedEvent>((Action<FightGroupUpdatedEvent>)OnFightGroupUpdatedEvent);
		}

		private void OnMatchmakingSuccessEvent(MatchmakingSuccessEvent obj)
		{
		}

		private void OnMatchmakingStoppedEvent(MatchmakingStoppedEvent obj)
		{
		}

		private void OnMatchmakingStartedEvent(MatchmakingStartedEvent obj)
		{
		}

		public void SendCancelGame()
		{
			m_connection.Write(new LeaveFightGroupCmd());
		}

		public void SendToggleMatchmaking()
		{
			m_connection.Write(new LaunchMatchmakingCmd
			{
				FightDefId = m_lastFightDefIdRequested
			});
		}

		private void OnLaunchMatchmakingResultEvent(LaunchMatchmakingResultEvent evt)
		{
		}

		public void SendForceFightVersusAI()
		{
			m_connection.Write(new ForceMatchmakingAgainstAICmd());
		}

		public void SendCreateGame(int fightDefId, int deckId, int? forcedLevel)
		{
			m_lastFightDefIdRequested = fightDefId;
			m_connection.Write(new CreateFightGroupCmd());
			m_connection.Write(new LaunchMatchmakingCmd
			{
				FightDefId = fightDefId
			});
		}

		private void OnFightNotStartedEvent(FightNotStartedEvent evt)
		{
			onGameError?.Invoke();
		}

		private void OnFightStartedEvent(FightStartedEvent evt)
		{
			onGameCreated?.Invoke(evt.FightInfo);
		}

		private void OnFightGroupUpdatedEvent(FightGroupUpdatedEvent evt)
		{
			if (evt.GroupRemoved)
			{
				onGameCanceled?.Invoke();
			}
		}

		private void OnChangeGodResultEvent(ChangeGodResultEvent obj)
		{
			this.OnChangedGod?.Invoke(obj);
		}

		private void OnSelectDeckAndWeaponResultHandler(SelectDeckAndWeaponResultEvent obj)
		{
			this.OnSelectedWeaponAndDeck?.Invoke(obj);
		}

		public void UpdatePlayerGod(God god)
		{
			ChangeGodCmd message = new ChangeGodCmd
			{
				God = (int)god
			};
			m_connection.Write(message);
		}

		public void SelectDeckAndWeapon(int weaponId, int? deckId)
		{
			SelectDeckAndWeaponCmd selectDeckAndWeaponCmd = new SelectDeckAndWeaponCmd();
			selectDeckAndWeaponCmd.SelectedWeapon = weaponId;
			selectDeckAndWeaponCmd.SelectedDecks.Add(new SelectDeckInfo
			{
				WeaponId = weaponId,
				DeckId = deckId
			});
			SelectDeckAndWeaponCmd message = selectDeckAndWeaponCmd;
			m_connection.Write(message);
		}
	}
}
