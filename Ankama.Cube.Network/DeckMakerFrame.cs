using Ankama.Cube.Data;
using Ankama.Cube.Protocols.PlayerProtocol;
using System;
using System.Collections.Generic;

namespace Ankama.Cube.Network
{
	public class DeckMakerFrame : CubeMessageFrame
	{
		public Action<RemoveDeckResultEvent> onRemoveConfigResult;

		public Action<SaveDeckResultEvent> onSaveConfigResult;

		public Action<SelectDeckAndWeaponResultEvent> onSelectDeckAndWeaponResult;

		public DeckMakerFrame()
		{
			base.WhenReceiveEnqueue<SaveDeckResultEvent>((Action<SaveDeckResultEvent>)OnSaveConfigResult);
			base.WhenReceiveEnqueue<RemoveDeckResultEvent>((Action<RemoveDeckResultEvent>)OnRemoveConfigResult);
			base.WhenReceiveEnqueue<SelectDeckAndWeaponResultEvent>((Action<SelectDeckAndWeaponResultEvent>)OnDeckAndWeaponChangeResult);
		}

		private void OnRemoveConfigResult(RemoveDeckResultEvent evt)
		{
			onRemoveConfigResult?.Invoke(evt);
		}

		private void OnSaveConfigResult(SaveDeckResultEvent evt)
		{
			onSaveConfigResult?.Invoke(evt);
		}

		private void OnDeckAndWeaponChangeResult(SelectDeckAndWeaponResultEvent evt)
		{
			onSelectDeckAndWeaponResult?.Invoke(evt);
		}

		public void SendSaveSquadRequest(int? id, string name, Family god, int weapon, IReadOnlyList<int> companions, IReadOnlyList<int> spells)
		{
			DeckInfo deckInfo = new DeckInfo
			{
				Id = id,
				Name = name,
				Weapon = weapon,
				God = (int)god
			};
			deckInfo.Companions.AddRange((IEnumerable<int>)companions);
			deckInfo.Spells.AddRange((IEnumerable<int>)spells);
			SaveDeckCmd message = new SaveDeckCmd
			{
				Info = deckInfo
			};
			m_connection.Write(message);
		}

		public void SendRemoveSquadRequest(int id)
		{
			RemoveDeckCmd message = new RemoveDeckCmd
			{
				Id = id
			};
			m_connection.Write(message);
		}

		public void SendSelectDecksAndWeapon(int? weaponId, Dictionary<int, int> selectedDecksPerWeapon)
		{
			SelectDeckAndWeaponCmd selectDeckAndWeaponCmd = new SelectDeckAndWeaponCmd
			{
				SelectedWeapon = weaponId
			};
			foreach (KeyValuePair<int, int> item in selectedDecksPerWeapon)
			{
				int? deckId = null;
				if (item.Value >= 0)
				{
					deckId = item.Value;
				}
				selectDeckAndWeaponCmd.SelectedDecks.Add(new SelectDeckInfo
				{
					WeaponId = item.Key,
					DeckId = deckId
				});
			}
			m_connection.Write(selectDeckAndWeaponCmd);
		}
	}
}
