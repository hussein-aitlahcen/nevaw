using JetBrains.Annotations;

namespace Ankama.Cube.Data
{
	public static class PropertiesUtility
	{
		[PublicAPI]
		public static readonly PropertyId[] propertiesWhichPreventVoluntaryMove = new PropertyId[4]
		{
			PropertyId.Rooted,
			PropertyId.Stun,
			PropertyId.Petrify,
			PropertyId.Shadowed
		};

		[PublicAPI]
		public static readonly PropertyId[] propertiesWhichPreventMoveEffects = new PropertyId[1]
		{
			PropertyId.Unmovable
		};

		[PublicAPI]
		public static readonly PropertyId[] propertiesWhichPreventAction = new PropertyId[4]
		{
			PropertyId.CharacterActionForbidden,
			PropertyId.Stun,
			PropertyId.Petrify,
			PropertyId.Shadowed
		};

		[PublicAPI]
		public static readonly PropertyId[] propertiesWhichPreventMagicalDamage = new PropertyId[3]
		{
			PropertyId.DamageProof,
			PropertyId.MagicalDamageProof,
			PropertyId.Petrify
		};

		[PublicAPI]
		public static readonly PropertyId[] propertiesWhichPreventPhysicalDamage = new PropertyId[3]
		{
			PropertyId.DamageProof,
			PropertyId.PhysicalDamageProof,
			PropertyId.Petrify
		};

		[PublicAPI]
		public static readonly PropertyId[] propertiesWhichPreventMagicalHeal = new PropertyId[3]
		{
			PropertyId.HealProof,
			PropertyId.MagicalHealProof,
			PropertyId.Petrify
		};

		[PublicAPI]
		public static readonly PropertyId[] propertiesWhichPreventPhysicalHeal = new PropertyId[3]
		{
			PropertyId.HealProof,
			PropertyId.PhysicalHealProof,
			PropertyId.Petrify
		};

		[PublicAPI]
		public static bool PreventsAction(PropertyId property)
		{
			PropertyId[] array = propertiesWhichPreventAction;
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				if (array[i] == property)
				{
					return true;
				}
			}
			return false;
		}

		public static bool IsSightProperty(PropertyId property)
		{
			PropertyId[] values = SightPropertyId.values;
			int num = values.Length;
			for (int i = 0; i < num; i++)
			{
				if (values[i] == property)
				{
					return true;
				}
			}
			return false;
		}
	}
}
