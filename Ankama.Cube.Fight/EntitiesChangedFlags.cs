using System;

namespace Ankama.Cube.Fight
{
	[Flags]
	public enum EntitiesChangedFlags
	{
		None = 0x0,
		Added = 0x1,
		Removed = 0x2,
		AreaMoved = 0x4,
		PlayableState = 0x8
	}
}
