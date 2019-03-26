using System;

namespace Ankama.Cube.Data
{
	[Serializable]
	public enum Element
	{
		[RelatedToEvents(new EventCategory[]
		{
			EventCategory.ElementPointsChanged
		})]
		Fire = 1,
		[RelatedToEvents(new EventCategory[]
		{
			EventCategory.ElementPointsChanged
		})]
		Water,
		[RelatedToEvents(new EventCategory[]
		{
			EventCategory.ElementPointsChanged
		})]
		Earth,
		[RelatedToEvents(new EventCategory[]
		{
			EventCategory.ElementPointsChanged
		})]
		Air,
		[RelatedToEvents(new EventCategory[]
		{
			EventCategory.ElementPointsChanged
		})]
		Multi
	}
}
