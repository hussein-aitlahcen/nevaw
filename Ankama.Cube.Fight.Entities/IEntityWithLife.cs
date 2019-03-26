namespace Ankama.Cube.Fight.Entities
{
	public interface IEntityWithLife : IEntity
	{
		int baseLife
		{
			get;
		}

		int armoredLife
		{
			get;
		}

		int life
		{
			get;
		}

		int armor
		{
			get;
		}

		int resistance
		{
			get;
		}

		int hitLimit
		{
			get;
		}

		bool wounded
		{
			get;
		}

		bool hasArmor
		{
			get;
		}
	}
}
