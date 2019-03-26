using Ankama.Cube.Data;
using Ankama.Cube.Maps.Objects;

namespace Ankama.Cube.Fight.Entities
{
	public interface IEntityWithBoardPresence : IEntity
	{
		Area area
		{
			get;
		}

		IsoObject view
		{
			get;
			set;
		}

		bool blocksMovement
		{
			get;
		}
	}
}
