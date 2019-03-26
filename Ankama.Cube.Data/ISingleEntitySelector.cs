using Ankama.Cube.Fight.Entities;
using DataEditor;

namespace Ankama.Cube.Data
{
	public interface ISingleEntitySelector : IEntitySelector, ITargetSelector, IEditableContent, ISingleTargetSelector
	{
		bool TryGetEntity<T>(DynamicValueContext context, out T entity) where T : class, IEntity;
	}
}
