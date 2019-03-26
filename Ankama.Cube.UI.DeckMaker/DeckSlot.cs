using Ankama.Cube.Data;
using Ankama.Cube.Protocols.PlayerProtocol;
using Ankama.Cube.TEMPFastEnterMatch.Player;
using System;
using System.Collections.Generic;

namespace Ankama.Cube.UI.DeckMaker
{
	public class DeckSlot
	{
		private DeckInfo m_deckInfo;

		private readonly int m_slotId;

		private bool m_preconstructed;

		private static int s_slotId;

		public DeckInfo DeckInfo => m_deckInfo;

		public bool HasDeckInfo => m_deckInfo != null;

		public bool Preconstructed => m_preconstructed;

		public int SlotId => m_slotId;

		public int? Id => m_deckInfo?.Id;

		public string Name => m_deckInfo?.Name;

		public int? Weapon => m_deckInfo?.Weapon;

		public int? God => m_deckInfo?.God;

		public IList<int> Spells => (IList<int>)(m_deckInfo?.Spells);

		public IList<int> Companions => (IList<int>)(m_deckInfo?.Companions);

		public bool isAvailableEmptyDeckSlot
		{
			get
			{
				if (!HasDeckInfo || !Id.HasValue)
				{
					return !m_preconstructed;
				}
				return false;
			}
		}

		public int Level
		{
			get
			{
				if (m_deckInfo == null)
				{
					return 0;
				}
				return m_deckInfo.GetLevel(PlayerData.instance.weaponInventory);
			}
		}

		public event Action<DeckSlot> OnModification;

		public DeckSlot(DeckInfo deckInfo, bool preconstructed = false)
		{
			m_deckInfo = deckInfo;
			m_slotId = ++s_slotId;
			m_preconstructed = preconstructed;
		}

		public void SetDeckInfo(DeckInfo info)
		{
			m_deckInfo = info;
			this.OnModification?.Invoke(this);
		}

		public void SetName(string name)
		{
			if (m_deckInfo != null && !string.Equals(name, m_deckInfo.Name))
			{
				m_deckInfo.Name = name;
				this.OnModification?.Invoke(this);
			}
		}

		public void SetId(int? id)
		{
			if (m_deckInfo != null && m_deckInfo.Id != id)
			{
				m_deckInfo.Id = id;
				this.OnModification?.Invoke(this);
			}
		}

		public void SetWeapon(int? weapon)
		{
			if (m_deckInfo != null && m_deckInfo.Weapon != weapon)
			{
				m_deckInfo.Weapon = (weapon ?? 0);
				this.OnModification?.Invoke(this);
			}
		}

		public void SetCompanionAt(int companion, int index)
		{
			if (m_deckInfo != null && m_deckInfo.Companions.get_Item(index) != companion)
			{
				m_deckInfo.Companions.set_Item(index, companion);
				this.OnModification?.Invoke(this);
			}
		}

		public void SetSpellAt(int spell, int index)
		{
			if (m_deckInfo != null && m_deckInfo.Spells.get_Item(index) != spell)
			{
				m_deckInfo.Spells.set_Item(index, spell);
				this.OnModification?.Invoke(this);
			}
		}

		public DeckSlot Clone(bool keepPreconstructed = true)
		{
			DeckSlot deckSlot = new DeckSlot(m_deckInfo?.Clone());
			if (keepPreconstructed)
			{
				deckSlot.m_preconstructed = m_preconstructed;
			}
			return deckSlot;
		}
	}
}
