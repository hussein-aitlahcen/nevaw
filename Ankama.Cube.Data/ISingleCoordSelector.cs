using DataEditor;

namespace Ankama.Cube.Data
{
	public interface ISingleCoordSelector : ICoordSelector, ITargetSelector, IEditableContent, ISingleTargetSelector
	{
		bool TryGetCoord(DynamicValueContext context, out Coord coord);
	}
}
