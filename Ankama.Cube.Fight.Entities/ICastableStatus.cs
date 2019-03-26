using Ankama.Cube.Data;

namespace Ankama.Cube.Fight.Entities
{
	public interface ICastableStatus
	{
		int level
		{
			get;
		}

		ICastableDefinition GetDefinition();
	}
}
