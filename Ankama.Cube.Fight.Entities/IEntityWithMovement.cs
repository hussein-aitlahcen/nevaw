namespace Ankama.Cube.Fight.Entities
{
	public interface IEntityWithMovement : IEntityWithBoardPresence, IEntity
	{
		int baseMovementPoints
		{
			get;
		}

		int movementPoints
		{
			get;
		}

		bool canMove
		{
			get;
		}
	}
}
