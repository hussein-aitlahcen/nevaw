using Ankama.Cube.Data;

namespace Ankama.Cube.Fight.Entities
{
	public interface IEntityWithElementaryState : IEntity
	{
		void ChangeElementaryState(ElementaryStates elemState);

		bool HasElementaryState(ElementaryStates elemState);
	}
}
