using Ankama.Cube.Fight.Entities;
using DataEditor;
using System.Collections.Generic;

namespace Ankama.Cube.Data
{
	public interface IEntitySelector : ITargetSelector, IEditableContent
	{
		IEnumerable<IEntity> EnumerateEntities(DynamicValueContext context);
	}
}
