using System;

namespace Ankama.Cube.Data
{
	[Serializable]
	public enum EventCategory
	{
		Any = 1,
		ActionPointsChanged,
		ReserveChanged,
		LifeArmorChanged,
		ElementPointsChanged,
		ElementaryStateChanged,
		MovementPointsChanged,
		EntityMoved,
		EntityAddedOrRemoved,
		PropertyChanged,
		DamageModifierChanged,
		HealModifierChanged,
		SpellCostModification,
		SpellsMoved,
		PlaySpellForbiddenChanged
	}
}
