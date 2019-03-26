using Ankama.Cube.Data;
using Ankama.Cube.Data.Levelable;
using Ankama.Cube.Protocols.PlayerProtocol;
using Ankama.Utilities;
using DataEditor;
using Google.Protobuf.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.TEMPFastEnterMatch.Player
{
	public class PlayerData
	{
		public delegate void PlayerDataInitialized(bool pendingFightFound);

		private static bool s_initialized;

		private static PlayerData s_instance;

		private readonly Dictionary<int, DeckInfo> m_decks = new Dictionary<int, DeckInfo>();

		private readonly Dictionary<int, int> m_selectedDecks = new Dictionary<int, int>();

		private readonly Dictionary<int, int> m_selectedWeapons = new Dictionary<int, int>();

		private long m_hash;

		private string m_nickName;

		private string m_accountType;

		private bool m_admin;

		private God m_god;

		private Gender m_gender;

		private Id<CharacterSkinDefinition> m_skin;

		private readonly InventoryWithLevel m_weaponInventory;

		private readonly HashSet<int> m_companionInventory = new HashSet<int>();

		private int m_havreMapSceneIndex;

		private int m_currentDeckId;

		public static PlayerData instance => s_instance;

		public long hash => m_hash;

		public string nickName => m_nickName;

		public string accountType => m_accountType;

		public bool admin => m_admin;

		public God god => m_god;

		public Gender gender => m_gender;

		public Id<CharacterSkinDefinition> Skin => m_skin;

		public IInventoryWithLevel weaponInventory => m_weaponInventory;

		public HashSet<int> companionInventory => m_companionInventory;

		public int havreMapSceneIndex => m_havreMapSceneIndex;

		public int currentDeckId => m_currentDeckId;

		public static event PlayerDataInitialized OnPlayerDataInitialized;

		public event Action<string> OnNicknameUpdated;

		public event Action OnPlayerHeroInfoUpdated;

		public event Action OnPlayerHeroSkinChanged;

		public event Action OnPlayerGodChanged;

		public event Action OnDeckListUpdated;

		public event Action OnCompanionsUpdated;

		public event Action<int> OnRequestVisualWeaponUpdated;

		public event Action OnSelectedWeaponUpdated;

		public event Action OnWeaponsLevelsUpdated;

		public event Action OnSelectedWeaponsUpdated;

		public event Action OnSelectedDecksUpdated;

		public event Action OnSelectedDeckUpdated;

		public static void Init(long hash, string nickName, string accountType = "ANKAMA", bool admin = false, bool pendingFightFound = false)
		{
			if (!s_initialized)
			{
				s_instance = new PlayerData(hash, nickName, accountType, admin);
				s_instance.m_havreMapSceneIndex = Random.Range(0, 5);
				Log.Info($"PlayerData Initialized for {nickName}. pending fight found: {pendingFightFound}", 44, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\TEMPFastEnterMatch\\Player\\PlayerData.cs");
				PlayerData.OnPlayerDataInitialized?.Invoke(pendingFightFound);
				s_initialized = true;
			}
			else
			{
				Log.Error("PlayerData already Initialized for " + nickName, 49, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\TEMPFastEnterMatch\\Player\\PlayerData.cs");
			}
		}

		public static void Clean()
		{
			s_initialized = false;
			s_instance = null;
		}

		private PlayerData(long hash, string nickName, string accountType = "ANKAMA", bool admin = false)
		{
			m_hash = hash;
			m_nickName = nickName;
			m_accountType = accountType;
			m_admin = admin;
			m_weaponInventory = new InventoryWithLevel(1);
		}

		public void UpdateNickname(string nickname)
		{
			m_nickName = nickname;
			this.OnNicknameUpdated?.Invoke(m_nickName);
		}

		public void SetAllDecks(IEnumerable<DeckInfo> decksInfos)
		{
			m_decks.Clear();
			foreach (DeckInfo decksInfo in decksInfos)
			{
				if (decksInfo.Id.HasValue)
				{
					int value = decksInfo.Id.Value;
					DeckInfo value2 = decksInfo.EnsureDataConsistency();
					m_decks.Add(value, value2);
				}
			}
			this.OnDeckListUpdated?.Invoke();
		}

		public void AddOrUpdateDeck(DeckInfo info)
		{
			if (!info.Id.HasValue)
			{
				Log.Error("Unable to Add Or Update deck: no Id provided", 127, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\TEMPFastEnterMatch\\Player\\PlayerData.cs");
				return;
			}
			int value = info.Id.Value;
			DeckInfo value2 = info.EnsureDataConsistency();
			if (m_decks.ContainsKey(value))
			{
				m_decks[value] = value2;
			}
			else
			{
				m_decks.Add(value, value2);
			}
			this.OnDeckListUpdated?.Invoke();
		}

		public void RemoveDeck(int id)
		{
			m_decks.Remove(id);
			this.OnDeckListUpdated?.Invoke();
		}

		public IEnumerable<DeckInfo> GetDecks()
		{
			return m_decks.Values;
		}

		public bool TryGetDeckById(int id, out DeckInfo deckInfo)
		{
			if (id < 0)
			{
				foreach (KeyValuePair<int, WeaponDefinition> weaponDefinition in RuntimeData.weaponDefinitions)
				{
					SquadDefinition value;
					if (!(weaponDefinition.Value.defaultDeck == null) && RuntimeData.squadDefinitions.TryGetValue(weaponDefinition.Value.defaultDeck.value, out value) && value.get_id() == -id)
					{
						DeckInfo deckInfo2 = value.ToDeckInfo();
						deckInfo2.Id = id;
						deckInfo = deckInfo2;
						return true;
					}
				}
				deckInfo = null;
				return false;
			}
			return m_decks.TryGetValue(id, out deckInfo);
		}

		public void UpdatePlayerHeroInfo(HeroInfo info)
		{
			int num = (m_skin != null) ? m_skin.value : (-1);
			God god = m_god;
			m_god = (God)info.God;
			m_gender = (Gender)info.Gender;
			m_skin = new Id<CharacterSkinDefinition>(info.Skin);
			if (num != info.Skin)
			{
				this.OnPlayerHeroSkinChanged?.Invoke();
			}
			if (god != m_god)
			{
				this.OnPlayerGodChanged?.Invoke();
			}
			this.OnPlayerHeroInfoUpdated?.Invoke();
			RefreshCurrentDeck();
		}

		public int GetCurrentWeapon()
		{
			if (m_selectedWeapons.TryGetValue((int)god, out int value))
			{
				return value;
			}
			return 0;
		}

		public void AddOrUpdateSelectedDeck(int weaponId, int deckId)
		{
			if (m_selectedDecks.ContainsKey(weaponId))
			{
				m_selectedDecks[weaponId] = deckId;
			}
			else
			{
				m_selectedDecks.Add(weaponId, deckId);
			}
			this.OnSelectedDecksUpdated?.Invoke();
			RefreshCurrentDeck();
		}

		public void RemoveSelectedDeck(int weaponId)
		{
			m_selectedDecks.Remove(weaponId);
			this.OnSelectedDecksUpdated?.Invoke();
			RefreshCurrentDeck();
		}

		public int GetSelectedDeckByWeapon(int weaponId)
		{
			if (m_selectedDecks.TryGetValue(weaponId, out int value))
			{
				return value;
			}
			if (RuntimeData.weaponDefinitions.TryGetValue(weaponId, out WeaponDefinition value2))
			{
				Id<SquadDefinition> defaultDeck = value2.defaultDeck;
				if (defaultDeck == null)
				{
					return 0;
				}
				return -defaultDeck.value;
			}
			return 0;
		}

		private void RefreshCurrentDeck()
		{
			if (m_selectedWeapons.TryGetValue((int)m_god, out int value))
			{
				int selectedDeckByWeapon = GetSelectedDeckByWeapon(value);
				bool num = selectedDeckByWeapon != m_currentDeckId;
				m_currentDeckId = selectedDeckByWeapon;
				if (num)
				{
					this.OnSelectedDeckUpdated?.Invoke();
				}
			}
		}

		public void UpdateWeaponsLevelsData(MapField<int, int> levels)
		{
			m_weaponInventory.UpdateAllLevels((IDictionary<int, int>)levels);
			this.OnWeaponsLevelsUpdated?.Invoke();
		}

		public void UpdateSelectedWeaponsData(MapField<int, int> selectedWeapons)
		{
			m_selectedWeapons.Clear();
			foreach (KeyValuePair<int, int> selectedWeapon in selectedWeapons)
			{
				m_selectedWeapons.Add(selectedWeapon.Key, selectedWeapon.Value);
			}
			this.OnSelectedWeaponUpdated?.Invoke();
			RefreshCurrentDeck();
		}

		public void UpdateCompanionData(RepeatedField<int> companions)
		{
			m_companionInventory.Clear();
			int i = 0;
			for (int count = companions.get_Count(); i < count; i++)
			{
				companionInventory.Add(companions.get_Item(i));
			}
			this.OnCompanionsUpdated?.Invoke();
		}

		public int? GetCurrentWeaponLevel()
		{
			if (weaponInventory.TryGetLevel(GetCurrentWeapon(), out int level))
			{
				return level;
			}
			return null;
		}

		public void RequestWeaponChange(int weaponId)
		{
			this.OnRequestVisualWeaponUpdated?.Invoke(weaponId);
		}
	}
}
