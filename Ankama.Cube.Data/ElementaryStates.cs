using System;

namespace Ankama.Cube.Data
{
	[Serializable]
	[RelatedToEvents(new EventCategory[]
	{
		EventCategory.ElementaryStateChanged
	})]
	public enum ElementaryStates
	{
		None = 1,
		Muddy,
		Oiled,
		Ventilated,
		Wet
	}
}
