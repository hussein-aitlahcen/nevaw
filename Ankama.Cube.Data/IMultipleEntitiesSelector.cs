using Ankama.Cube.Fight.Entities;
using System.Collections.Generic;

namespace Ankama.Cube.Data
{
	public interface IMultipleEntitiesSelector
	{
		IEnumerable<IEntity> EnumerateEntities(DynamicValueContext context);
	}
}
