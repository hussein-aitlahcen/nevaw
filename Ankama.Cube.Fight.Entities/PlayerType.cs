using System;

namespace Ankama.Cube.Fight.Entities
{
	[Flags]
	public enum PlayerType
	{
		None = 0x0,
		Ally = 0x1,
		Opponent = 0x2,
		Local = 0x4,
		Player = 0xD
	}
}
