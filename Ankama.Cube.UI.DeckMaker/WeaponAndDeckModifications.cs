using Ankama.Cube.TEMPFastEnterMatch.Player;
using System;
using System.Collections.Generic;

namespace Ankama.Cube.UI.DeckMaker
{
	public class WeaponAndDeckModifications
	{
		private int? m_selectedWeapon;

		private readonly Dictionary<int, int> m_selectedDecksPerWeapon = new Dictionary<int, int>();

		public bool hasModifications
		{
			get
			{
				if (!m_selectedWeapon.HasValue)
				{
					return m_selectedDecksPerWeapon.Count != 0;
				}
				return true;
			}
		}

		public int? selectedWeapon => m_selectedWeapon;

		public Dictionary<int, int> selectedDecksPerWeapon => m_selectedDecksPerWeapon;

		public event Action OnSelectedWeaponUpdated;

		public event Action OnSelectedDecksUpdated;

		public void Setup()
		{
			PlayerData.instance.OnSelectedWeaponUpdated += OnSelectedWeaponUpdate;
			PlayerData.instance.OnSelectedDecksUpdated += OnSelectedDecksUpdate;
		}

		public void Unregister()
		{
			PlayerData.instance.OnSelectedWeaponUpdated -= OnSelectedWeaponUpdate;
			PlayerData.instance.OnSelectedDecksUpdated -= OnSelectedDecksUpdate;
		}

		private void OnSelectedWeaponUpdate()
		{
			m_selectedWeapon = null;
			this.OnSelectedWeaponUpdated?.Invoke();
		}

		public void SetSelectedWeapon(int weapon)
		{
			if (PlayerData.instance.GetCurrentWeapon() == weapon)
			{
				m_selectedWeapon = null;
			}
			else
			{
				m_selectedWeapon = weapon;
			}
			this.OnSelectedWeaponUpdated?.Invoke();
		}

		public int GetSelectedWeapon()
		{
			return m_selectedWeapon ?? PlayerData.instance.GetCurrentWeapon();
		}

		private void OnSelectedDecksUpdate()
		{
			int selectedWeapon = GetSelectedWeapon();
			m_selectedDecksPerWeapon.Remove(selectedWeapon);
			this.OnSelectedDecksUpdated?.Invoke();
		}

		public void SetSelectedDeckForWeapon(int weapon, int deck)
		{
			if (PlayerData.instance.GetSelectedDeckByWeapon(weapon) == deck)
			{
				m_selectedDecksPerWeapon.Remove(weapon);
			}
			else
			{
				m_selectedDecksPerWeapon[weapon] = deck;
			}
			this.OnSelectedDecksUpdated?.Invoke();
		}

		public int GetSelectedDeckForWeapon(int weapon)
		{
			if (m_selectedDecksPerWeapon.TryGetValue(weapon, out int value))
			{
				return value;
			}
			return PlayerData.instance.GetSelectedDeckByWeapon(weapon);
		}
	}
}
