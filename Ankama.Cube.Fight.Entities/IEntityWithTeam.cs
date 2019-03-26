namespace Ankama.Cube.Fight.Entities
{
	public interface IEntityWithTeam : IEntity
	{
		int teamId
		{
			get;
		}

		int teamIndex
		{
			get;
		}
	}
}
