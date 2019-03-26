using Ankama.Cube.Data;
using System.Collections.Generic;

namespace Ankama.Cube.Fight.Entities
{
	public interface IEntityWithFamilies : IEntityWithTeam, IEntity
	{
		IReadOnlyList<Family> families
		{
			get;
		}
	}
}
