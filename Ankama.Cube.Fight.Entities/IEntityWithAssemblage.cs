using System.Collections.Generic;

namespace Ankama.Cube.Fight.Entities
{
	public interface IEntityWithAssemblage : IEntityWithBoardPresence, IEntity
	{
		IReadOnlyList<int> assemblingIds
		{
			get;
			set;
		}
	}
}
