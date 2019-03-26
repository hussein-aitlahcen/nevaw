using DataEditor;
using System.Collections.Generic;

namespace Ankama.Cube.Data
{
	[RelatedToEvents(new EventCategory[]
	{
		EventCategory.EntityAddedOrRemoved,
		EventCategory.EntityMoved
	})]
	public interface ICoordFilter : ITargetFilter, IEditableContent
	{
		IEnumerable<Coord> Filter(IEnumerable<Coord> coords, DynamicValueContext context);
	}
}
