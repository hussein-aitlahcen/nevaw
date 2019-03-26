using System;

namespace Ankama.Cube.Data
{
	[Serializable]
	public enum MovementType
	{
		Run = 1,
		Action = 2,
		Teleport = 3,
		Charge = 4,
		Push = 6,
		Pull = 7,
		Dash = 8
	}
}
