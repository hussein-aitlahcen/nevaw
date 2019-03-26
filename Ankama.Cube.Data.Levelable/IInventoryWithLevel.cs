using System.Collections;
using System.Collections.Generic;

namespace Ankama.Cube.Data.Levelable
{
	public interface IInventoryWithLevel : IEnumerable<int>, IEnumerable, ILevelProvider
	{
	}
}
