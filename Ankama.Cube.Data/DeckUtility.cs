using Ankama.Cube.Data.Levelable;
using Ankama.Cube.Protocols.PlayerProtocol;
using Ankama.Cube.TEMPFastEnterMatch.Player;
using DataEditor;
using Google.Protobuf.Collections;
using System;
using System.Collections.Generic;

namespace Ankama.Cube.Data
{
	public static class DeckUtility
	{
		public static int GetRemainingSlotsForWeapon(int weapon)
		{
			int num = 0;
			foreach (DeckInfo deck in PlayerData.instance.GetDecks())
			{
				if (deck.Weapon == weapon)
				{
					num++;
				}
			}
			return Math.Max(0, 3 - num);
		}

		public static DeckInfo FindValidDeckForGod(God god)
		{
			foreach (DeckInfo deck in PlayerData.instance.GetDecks())
			{
				if (deck.God == (int)god && deck.IsValid())
				{
					return deck;
				}
			}
			return null;
		}

		public static bool IsValid(this DeckInfo info)
		{
			if (info == null)
			{
				return false;
			}
			RepeatedField<int> spells = info.Spells;
			int count = spells.get_Count();
			if (count < 8)
			{
				return false;
			}
			for (int i = 0; i < count; i++)
			{
				if (spells.get_Item(i) <= 0)
				{
					return false;
				}
			}
			RepeatedField<int> companions = info.Companions;
			int count2 = info.Companions.get_Count();
			if (count2 < 4)
			{
				return false;
			}
			for (int j = 0; j < count2; j++)
			{
				if (companions.get_Item(j) <= 0)
				{
					return false;
				}
			}
			int? num = info.Weapon;
			if (!num.HasValue || !PlayerData.instance.weaponInventory.TryGetLevel(num.Value, out int _))
			{
				return false;
			}
			return true;
		}

		public static int GetLevel(this DeckInfo info, ILevelProvider weaponLevelProvider)
		{
			int level = 0;
			weaponLevelProvider.TryGetLevel(info.Weapon, out level);
			return Math.Max(1, level);
		}

		public static DeckInfo EnsureDataConsistency(this DeckInfo deck)
		{
			RepeatedField<int> spells = deck.Spells;
			for (int num = spells.get_Count() - 1; num >= 0; num--)
			{
				int key = spells.get_Item(num);
				if (!RuntimeData.spellDefinitions.TryGetValue(key, out SpellDefinition _))
				{
					spells.RemoveAt(num);
				}
			}
			RepeatedField<int> companions = deck.Companions;
			for (int num2 = companions.get_Count() - 1; num2 >= 0; num2--)
			{
				int key2 = companions.get_Item(num2);
				if (!RuntimeData.companionDefinitions.TryGetValue(key2, out CompanionDefinition _))
				{
					companions.RemoveAt(num2);
				}
			}
			if (!RuntimeData.weaponDefinitions.TryGetValue(deck.Weapon, out WeaponDefinition _))
			{
				deck.Weapon = 0;
			}
			return deck;
		}

		public static DeckInfo ToDeckInfo(this SquadDefinition definition)
		{
			if (!RuntimeData.weaponDefinitions.TryGetValue(definition.weapon.value, out WeaponDefinition value))
			{
				return null;
			}
			DeckInfo deckInfo = new DeckInfo
			{
				Name = RuntimeData.FormattedText(63105),
				God = (int)value.god,
				Weapon = value.get_id()
			};
			IReadOnlyList<Id<CompanionDefinition>> companions = definition.companions;
			int i = 0;
			for (int count = companions.Count; i < count; i++)
			{
				if (RuntimeData.companionDefinitions.TryGetValue(companions[i].value, out CompanionDefinition _))
				{
					deckInfo.Companions.Add(companions[i].value);
				}
			}
			IReadOnlyList<Id<SpellDefinition>> spells = definition.spells;
			int j = 0;
			for (int count2 = spells.Count; j < count2; j++)
			{
				if (RuntimeData.spellDefinitions.TryGetValue(spells[j].value, out SpellDefinition _))
				{
					deckInfo.Spells.Add(spells[j].value);
				}
			}
			return deckInfo;
		}

		public static DeckInfo FillEmptySlotsCopy(this DeckInfo clone, int defaultValue = -1)
		{
			int i = clone.Companions.get_Count();
			for (int num = 4; i < num; i++)
			{
				clone.Companions.Add(defaultValue);
			}
			int j = clone.Spells.get_Count();
			for (int num2 = 8; j < num2; j++)
			{
				clone.Spells.Add(defaultValue);
			}
			return clone;
		}

		public static DeckInfo TrimCopy(this DeckInfo info, int defaultValue = -1)
		{
			DeckInfo deckInfo = new DeckInfo
			{
				Id = info.Id,
				Name = info.Name,
				God = info.God,
				Weapon = info.Weapon
			};
			RepeatedField<int> companions = info.Companions;
			int i = 0;
			for (int count = companions.get_Count(); i < count; i++)
			{
				if (companions.get_Item(i) != defaultValue)
				{
					deckInfo.Companions.Add(companions.get_Item(i));
				}
			}
			RepeatedField<int> spells = info.Spells;
			int j = 0;
			for (int count2 = spells.get_Count(); j < count2; j++)
			{
				if (spells.get_Item(j) != defaultValue)
				{
					deckInfo.Spells.Add(spells.get_Item(j));
				}
			}
			return deckInfo;
		}

		public static bool DecksAreEqual(DeckInfo deck, DeckInfo deck2)
		{
			return deck?.Equals(deck2) ?? (deck2 == null);
		}
	}
}
