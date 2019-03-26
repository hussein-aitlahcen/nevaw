using System;

namespace Ankama.Cube.Data
{
	[Serializable]
	[RelatedToEvents(new EventCategory[]
	{
		EventCategory.PropertyChanged
	})]
	public enum PropertyId
	{
		Stun = 1,
		[RelatedToEvents(new EventCategory[]
		{
			EventCategory.PlaySpellForbiddenChanged
		})]
		PlaySpellForbidden = 2,
		CharacterActionForbidden = 3,
		Rooted = 4,
		Petrify = 6,
		Initiative = 7,
		Shield = 8,
		PhysicalCounter = 9,
		ElementaryStateProof = 10,
		PhysicalDamageProof = 11,
		MagicalDamageProof = 12,
		HealProof = 13,
		DamageProof = 14,
		PhysicalHealProof = 0xF,
		MagicalHealProof = 0x10,
		EntityVersion2 = 17,
		CanPassThrough = 18,
		Untargetable = 19,
		Unmovable = 20,
		DoubleDamageReceived = 21,
		MotivatingSight = 22,
		HealingSight = 23,
		AnimalLink = 24,
		Shadowed = 25,
		Frozen = 26,
		InvokeCopyOnDeath = 27,
		Agony = 28,
		SacredSight = 29,
		RepulsiveSight = 30,
		ProtectedSight = 0x1F,
		StoneSight = 0x20,
		AerialSight = 33,
		AutoResurrectCompanion = 34
	}
}
