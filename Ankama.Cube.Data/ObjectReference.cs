using System;

namespace Ankama.Cube.Data
{
	public static class ObjectReference
	{
		public enum Type
		{
			None,
			Spell,
			Companion,
			Summoning,
			FloorMechanism,
			ObjectMechanism,
			Weapon
		}

		public static IDefinitionWithTooltip GetObject(Type type, int id)
		{
			switch (type)
			{
			case Type.Spell:
				return GetSpell(id);
			case Type.Companion:
				return GetCompanion(id);
			case Type.Summoning:
				return GetSummoning(id);
			case Type.ObjectMechanism:
				return GetObjectMechanism(id);
			case Type.FloorMechanism:
				return GetFloorMechanism(id);
			case Type.Weapon:
				return GetWeapon(id);
			default:
				throw new ArgumentOutOfRangeException("type", type, null);
			}
		}

		public static SpellDefinition GetSpell(int spellId)
		{
			return RuntimeData.spellDefinitions[spellId];
		}

		public static CompanionDefinition GetCompanion(int companionId)
		{
			return RuntimeData.companionDefinitions[companionId];
		}

		public static SummoningDefinition GetSummoning(int summoningId)
		{
			return RuntimeData.summoningDefinitions[summoningId];
		}

		public static ObjectMechanismDefinition GetObjectMechanism(int objectMechanismId)
		{
			return RuntimeData.objectMechanismDefinitions[objectMechanismId];
		}

		public static FloorMechanismDefinition GetFloorMechanism(int floorMechanismId)
		{
			return RuntimeData.floorMechanismDefinitions[floorMechanismId];
		}

		public static WeaponDefinition GetWeapon(int weaponId)
		{
			return RuntimeData.weaponDefinitions[weaponId];
		}

		public static GodDefinition GetGod(string godName)
		{
			God key = (God)Enum.Parse(typeof(God), godName);
			RuntimeData.godDefinitions.TryGetValue(key, out GodDefinition value);
			return value;
		}
	}
}
