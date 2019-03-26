using Ankama.Cube.Data.Castable;

namespace Ankama.Cube.Data
{
	public class CompanionData : CastableWithLevelData<CompanionDefinition>
	{
		public CompanionData(CompanionDefinition definition, int level)
			: base(definition, level)
		{
		}
	}
}
