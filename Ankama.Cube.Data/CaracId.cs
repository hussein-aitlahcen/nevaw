using System;

namespace Ankama.Cube.Data
{
	[Serializable]
	public enum CaracId
	{
		[RelatedToEvents(new EventCategory[]
		{
			EventCategory.LifeArmorChanged
		})]
		Life = 1,
		[RelatedToEvents(new EventCategory[]
		{
			EventCategory.MovementPointsChanged
		})]
		MovementPoints = 2,
		[RelatedToEvents(new EventCategory[]
		{
			EventCategory.ActionPointsChanged
		})]
		ActionPoints = 3,
		Resistance = 5,
		[RelatedToEvents(new EventCategory[]
		{
			EventCategory.LifeArmorChanged
		})]
		Armor = 6,
		HitLimit = 7,
		RangeMin = 8,
		RangeMax = 9,
		DamageReflection = 10,
		[RelatedToEvents(new EventCategory[]
		{
			EventCategory.ElementPointsChanged
		})]
		FirePoints = 11,
		[RelatedToEvents(new EventCategory[]
		{
			EventCategory.ElementPointsChanged
		})]
		WaterPoints = 12,
		[RelatedToEvents(new EventCategory[]
		{
			EventCategory.ElementPointsChanged
		})]
		EarthPoints = 13,
		[RelatedToEvents(new EventCategory[]
		{
			EventCategory.ElementPointsChanged
		})]
		AirPoints = 14,
		[RelatedToEvents(new EventCategory[]
		{
			EventCategory.DamageModifierChanged
		})]
		PhysicalDamageModifier = 0xF,
		[RelatedToEvents(new EventCategory[]
		{
			EventCategory.HealModifierChanged
		})]
		PhysicalHealModifier = 0x10,
		[RelatedToEvents(new EventCategory[]
		{
			EventCategory.DamageModifierChanged
		})]
		MagicalDamageModifier = 17,
		[RelatedToEvents(new EventCategory[]
		{
			EventCategory.HealModifierChanged
		})]
		MagicalHealModifier = 18,
		[RelatedToEvents(new EventCategory[]
		{
			EventCategory.ReserveChanged
		})]
		ReservePoints = 19,
		FloatingCounterRipostingSword = 20,
		FloatingCounterBleedingSword = 21,
		FloatingCounterFlyingFist = 22,
		FloatingCounterPoisoningSkull = 23,
		FloatingCounterHealingSkull = 24,
		FloatingCounterDamagingSkull = 25,
		FloatingCounterPunishingDagger = 26,
		FloatingCounterCorbakSouls = 27,
		FloatingCounterWildBoarSouls = 28,
		FloatingCounterRegenerationMaster = 29,
		FloatingCounterChibiDial = 30,
		FloatingCounterAvengerHourglass = 0x1F,
		FloatingCounterNocturiansMaster = 33,
		FloatingCounterSight = 34,
		FloatingCounterTattooedFists = 35,
		FloatingCounterTattooedEyes = 36,
		LifeMax = 0x20
	}
}
