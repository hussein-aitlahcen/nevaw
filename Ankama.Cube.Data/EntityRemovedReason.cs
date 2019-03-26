using System;

namespace Ankama.Cube.Data
{
	[Serializable]
	public enum EntityRemovedReason
	{
		None = 1,
		Death,
		Transformation,
		PlayerLeft,
		Resurrection3V3,
		EntitySummonedAtSamePlace,
		Activated,
		OwnerDead,
		CompanionReturned
	}
}
