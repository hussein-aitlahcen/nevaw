using Ankama.Cube.Data;
using Ankama.Cube.Protocols.PlayerProtocol;
using Ankama.Cube.TEMPFastEnterMatch.Player;
using Ankama.Cube.UI;
using Ankama.Cube.UI.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ankama.Cube.TEMPFastEnterMatch.MatchMaking
{
	public class MatchMakingUI : AbstractUI
	{
		[SerializeField]
		private MatchMakingButton m_defaultButton;

		[SerializeField]
		private CustomDropdown m_godChoiceDropdown;

		[SerializeField]
		private CustomDropdown m_weaponsDropdown;

		[SerializeField]
		private CustomDropdown m_decksDropDown;

		[SerializeField]
		private Button m_returnButton;

		[SerializeField]
		private CustomDropdown m_forceLevelDropdown;

		[SerializeField]
		private List<int> m_availableFightDefIds;

		private readonly List<God> m_playableGods = new List<God>();

		private MatchMakingButton m_waitingButton;

		public Action<int, int?> onPlayRequested;

		public Action onForceAiRequested;

		public Action onReturnClicked;

		public Action onCancelRequested;

		private List<DeckInfo> m_deckList;

		private List<WeaponDefinition> m_weaponList;

		private int? ForcedLevel
		{
			get
			{
				if (m_forceLevelDropdown.value == 0)
				{
					return null;
				}
				return m_forceLevelDropdown.value;
			}
		}

		public event Action<God> onGodSelectedChanged;

		public event Action<int> onSelectedWeaponChanged;

		public event Action<int> onSelectedDeckChanged;

		protected unsafe override void Awake()
		{
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Expected O, but got Unknown
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Expected O, but got Unknown
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Expected O, but got Unknown
			m_defaultButton.get_gameObject().SetActive(false);
			foreach (int availableFightDefId in m_availableFightDefIds)
			{
				MatchMakingButton btn = Object.Instantiate<MatchMakingButton>(m_defaultButton, m_defaultButton.get_transform().get_parent());
				btn.fightDefId = availableFightDefId;
				btn.StopWait();
				btn.get_gameObject().SetActive(true);
				_003C_003Ec__DisplayClass26_0 _003C_003Ec__DisplayClass26_;
				btn.button.get_onClick().AddListener(new UnityAction((object)_003C_003Ec__DisplayClass26_, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
				btn.forceAiBUtton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			}
			m_returnButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_godChoiceDropdown.onValueChanged.AddListener(new UnityAction<int>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_weaponsDropdown.onValueChanged.AddListener(new UnityAction<int>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_decksDropDown.onValueChanged.AddListener(new UnityAction<int>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			InitGodsChoice();
		}

		private void OnForceVersusAiButtonClicked()
		{
			if (m_waitingButton != null)
			{
				onForceAiRequested?.Invoke();
			}
		}

		private void OnPlayButtonClicked(MatchMakingButton btn)
		{
			if (m_waitingButton != null)
			{
				onCancelRequested?.Invoke();
				m_waitingButton.StopWait();
				m_waitingButton = null;
			}
			else
			{
				btn.StartWait();
				onPlayRequested?.Invoke(btn.fightDefId, ForcedLevel);
				m_waitingButton = btn;
			}
		}

		private void OnSelectWeapon(int weaponIndex)
		{
			WeaponDefinition weaponDefinition = m_weaponList[weaponIndex];
			this.onSelectedWeaponChanged?.Invoke(weaponDefinition.get_id());
		}

		private void OnSelectDeck(int deckIndex)
		{
			DeckInfo deckInfo = m_deckList[deckIndex];
			this.onSelectedDeckChanged?.Invoke(deckInfo.Id ?? 0);
		}

		private void OnGodSelected(int god)
		{
			this.onGodSelectedChanged?.Invoke(m_playableGods[god]);
		}

		private void OnReturnClicked()
		{
			if (m_waitingButton != null)
			{
				onCancelRequested?.Invoke();
				m_waitingButton = null;
			}
			onReturnClicked?.Invoke();
		}

		public void OnGodChanged()
		{
			InitGodsChoice();
		}

		public void OnWeaponChanged(int weaponId)
		{
			InitWeapons(weaponId);
		}

		private void InitGodsChoice()
		{
			m_playableGods.Clear();
			List<string> list = new List<string>();
			God god = PlayerData.instance.god;
			int num = -1;
			int num2 = 0;
			foreach (GodDefinition value in RuntimeData.godDefinitions.Values)
			{
				if (value.playable)
				{
					m_playableGods.Add(value.god);
					list.Add(RuntimeData.FormattedText(value.i18nNameId));
					if (value.god == god)
					{
						num = num2;
					}
					num2++;
				}
			}
			m_godChoiceDropdown.ClearOptions();
			m_godChoiceDropdown.AddOptions(list);
			m_godChoiceDropdown.value = ((num >= 0) ? num : 0);
			int currentWeapon = PlayerData.instance.GetCurrentWeapon();
			InitWeapons(currentWeapon);
		}

		private void InitWeapons(int selectedWeaponId)
		{
			m_weaponList = new List<WeaponDefinition>();
			m_weaponsDropdown.ClearOptions();
			God god = PlayerData.instance.god;
			foreach (int item in PlayerData.instance.weaponInventory)
			{
				if (RuntimeData.weaponDefinitions.TryGetValue(item, out WeaponDefinition value) && value.god == god)
				{
					m_weaponList.Add(value);
				}
			}
			int value2 = m_weaponList.FindIndex((WeaponDefinition definition) => definition.get_id() == selectedWeaponId);
			if (m_weaponsDropdown != null)
			{
				m_weaponsDropdown.AddOptions((from sd in m_weaponList
					select sd.get_displayName()).ToList());
				m_weaponsDropdown.value = value2;
			}
			InitDecks();
		}

		private void InitDecks()
		{
			m_deckList = new List<DeckInfo>();
			m_decksDropDown.ClearOptions();
			God god = PlayerData.instance.god;
			int currentWeapon = PlayerData.instance.GetCurrentWeapon();
			if (RuntimeData.weaponDefinitions.TryGetValue(currentWeapon, out WeaponDefinition value))
			{
				if (RuntimeData.squadDefinitions.TryGetValue(value.defaultDeck.value, out SquadDefinition value2))
				{
					DeckInfo deckInfo = value2.ToDeckInfo();
					deckInfo.Id = -value2.get_id();
					m_deckList.Add(deckInfo);
				}
				foreach (DeckInfo deck in PlayerData.instance.GetDecks())
				{
					if (deck.Weapon == currentWeapon)
					{
						m_deckList.Add(deck);
					}
				}
				int selectedDeckId = PlayerData.instance.GetSelectedDeckByWeapon(value.get_id());
				int num = -1;
				num = m_deckList.FindIndex((DeckInfo deck) => deck.Id == selectedDeckId);
				if (m_decksDropDown != null)
				{
					m_decksDropDown.AddOptions((from sd in m_deckList
						select sd.Name).ToList());
					m_decksDropDown.value = num;
				}
			}
		}

		public void SetCurrentWeapon(int weaponId)
		{
			if (m_weaponsDropdown != null)
			{
				m_weaponsDropdown.value = m_weaponList.FindIndex((WeaponDefinition weapon) => weapon.get_id() == weaponId);
			}
		}

		public void SetCurrentDeck(DeckInfo deckInfo)
		{
			if (m_decksDropDown != null)
			{
				m_decksDropDown.value = m_deckList.IndexOf(deckInfo);
			}
		}
	}
}
