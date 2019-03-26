namespace Ankama.Cube.Fight.Entities
{
	public interface IEntityWithOwner : IEntityWithTeam, IEntity
	{
		int ownerId
		{
			get;
		}
	}
}
