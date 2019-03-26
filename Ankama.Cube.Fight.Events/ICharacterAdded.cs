using Ankama.Cube.Protocols.CommonProtocol;

namespace Ankama.Cube.Fight.Events
{
	public interface ICharacterAdded
	{
		int entityDefId
		{
			get;
		}

		int ownerId
		{
			get;
		}

		CellCoord refCoord
		{
			get;
		}

		int direction
		{
			get;
		}

		int level
		{
			get;
		}
	}
}
