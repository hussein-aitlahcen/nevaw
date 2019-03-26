using Ankama.Cube.Data.Castable;

namespace Ankama.Cube.Data
{
	public class SpellData : CastableWithLevelData<SpellDefinition>
	{
		public SpellData(SpellDefinition definition, int level)
			: base(definition, level)
		{
		}
	}
}
