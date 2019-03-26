using DataEditor;
using System.Collections.Generic;

namespace Ankama.Cube.Data
{
	public interface ICoordSelector : ITargetSelector, IEditableContent
	{
		IEnumerable<Coord> EnumerateCoords(DynamicValueContext context);
	}
}
