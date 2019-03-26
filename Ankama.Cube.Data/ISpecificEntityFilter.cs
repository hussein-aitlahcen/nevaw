using Ankama.Cube.Fight.Entities;
using DataEditor;

namespace Ankama.Cube.Data
{
	public interface ISpecificEntityFilter : IEntityFilter, ITargetFilter, IEditableContent
	{
		bool ValidFor(IEntity entity);
	}
}
