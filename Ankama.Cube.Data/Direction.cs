using System;

namespace Ankama.Cube.Data
{
	[Serializable]
	public enum Direction
	{
		None = -1,
		East,
		SouthEast,
		South,
		SouthWest,
		West,
		NorthWest,
		North,
		NorthEast
	}
}
