using Ankama.Cube.Data.Castable;

namespace Ankama.Cube.Data
{
	public class WeaponData : CastableWithLevelData<WeaponDefinition>
	{
		public WeaponData(WeaponDefinition definition, int level)
			: base(definition, level)
		{
		}
	}
}
