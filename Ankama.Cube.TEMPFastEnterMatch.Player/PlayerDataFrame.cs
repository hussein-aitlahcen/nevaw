using Ankama.Cube.Network;
using Ankama.Cube.Protocols.PlayerProtocol;
using Ankama.Utilities;
using Google.Protobuf.Collections;
using System;
using System.Collections.Generic;

namespace Ankama.Cube.TEMPFastEnterMatch.Player
{
	public class PlayerDataFrame : CubeMessageFrame
	{
		public event PlayerAccountLoaded OnPlayerAccountLoaded;

		public PlayerDataFrame()
		{
			base.WhenReceiveEnqueue<PlayerDataEvent>((Action<PlayerDataEvent>)OnPlayerDataEvent);
		}

		private void OnPlayerDataEvent(PlayerDataEvent msg)
		{
			if (msg.Account != null)
			{
				Log.Info("Player account loaded : " + msg.Account.NickName, 26, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\TEMPFastEnterMatch\\Player\\PlayerDataFrame.cs");
				bool pendingFightFound = msg.Occupation?.InFight ?? false;
				PlayerData.Init(msg.Account.Hash, msg.Account.NickName, msg.Account.AccountType, msg.Account.Admin, pendingFightFound);
				this.OnPlayerAccountLoaded?.Invoke(pendingFightFound);
			}
			if (msg.Decks != null)
			{
				PlayerData instance = PlayerData.instance;
				instance.SetAllDecks((IEnumerable<DeckInfo>)msg.Decks.CustomDecks);
				foreach (KeyValuePair<int, int> selectedDeck in msg.Decks.SelectedDecks)
				{
					instance.AddOrUpdateSelectedDeck(selectedDeck.Key, selectedDeck.Value);
				}
			}
			else if (msg.DecksUpdates != null)
			{
				PlayerData instance2 = PlayerData.instance;
				foreach (DeckInfo item in msg.DecksUpdates.DeckUpdated)
				{
					instance2.AddOrUpdateDeck(item);
				}
				foreach (int item2 in msg.DecksUpdates.DeckRemovedId)
				{
					instance2.RemoveDeck(item2);
				}
				foreach (PlayerDataEvent.Types.DeckIncrementalUpdateData.Types.SelectedDeckPerWeapon item3 in msg.DecksUpdates.DeckSelectionsUpdated)
				{
					if (item3.DeckId.HasValue)
					{
						PlayerData.instance.AddOrUpdateSelectedDeck(item3.WeaponId, item3.DeckId.Value);
					}
					else
					{
						PlayerData.instance.RemoveSelectedDeck(item3.WeaponId);
					}
				}
			}
			if (msg.CompanionData != null)
			{
				RepeatedField<int> companions = msg.CompanionData.Companions;
				PlayerData.instance.UpdateCompanionData(companions);
			}
			if (msg.WeaponLevelsData != null)
			{
				MapField<int, int> weaponLevels = msg.WeaponLevelsData.WeaponLevels;
				PlayerData.instance.UpdateWeaponsLevelsData(weaponLevels);
			}
			if (msg.SelectedWeaponsData != null)
			{
				MapField<int, int> selectedWeapons = msg.SelectedWeaponsData.SelectedWeapons;
				PlayerData.instance.UpdateSelectedWeaponsData(selectedWeapons);
			}
			if (msg.Hero != null)
			{
				PlayerData.instance.UpdatePlayerHeroInfo(msg.Hero.Info);
			}
		}

		public void GetPlayerInitialData()
		{
			m_connection.Write(new GetPlayerDataCmd
			{
				Occupation = true,
				AccountData = true,
				Decks = true,
				HeroData = true,
				Companions = true,
				Weapons = true
			});
		}

		public override void Dispose()
		{
			PlayerData.Clean();
			base.Dispose();
		}
	}
}
