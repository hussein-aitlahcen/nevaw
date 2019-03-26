namespace Ankama.Cube.Data.Levelable
{
	public interface ILevelProvider
	{
		bool TryGetLevel(int id, out int level);
	}
}
