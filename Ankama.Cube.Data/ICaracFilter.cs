using System.Collections.Generic;

namespace Ankama.Cube.Data
{
	public interface ICaracFilter
	{
		IEnumerable<CaracId> Filter(IEnumerable<CaracId> entities, DynamicValueContext context);
	}
}
