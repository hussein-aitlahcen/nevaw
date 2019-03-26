using System;

namespace Ankama.Cube.Data
{
	[Serializable]
	public enum DirectionAngle
	{
		CounterClockwise180 = -4,
		CounterClockwise135,
		CounterClockwise90,
		CounterClockwise45,
		None,
		Clockwise45,
		Clockwise90,
		Clockwise135,
		Clockwise180
	}
}
