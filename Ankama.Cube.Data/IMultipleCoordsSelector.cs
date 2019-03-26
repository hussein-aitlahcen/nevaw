using System.Collections.Generic;

namespace Ankama.Cube.Data
{
	public interface IMultipleCoordsSelector
	{
		IEnumerable<Coord> EnumerateCoords(DynamicValueContext context);
	}
}
