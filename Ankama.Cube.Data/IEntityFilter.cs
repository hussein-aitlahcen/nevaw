using Ankama.Cube.Fight.Entities;
using DataEditor;
using System.Collections.Generic;

namespace Ankama.Cube.Data
{
	[RelatedToEvents(new EventCategory[]
	{
		EventCategory.EntityAddedOrRemoved
	})]
	public interface IEntityFilter : ITargetFilter, IEditableContent
	{
		IEnumerable<IEntity> Filter(IEnumerable<IEntity> entities, DynamicValueContext context);
	}
}
