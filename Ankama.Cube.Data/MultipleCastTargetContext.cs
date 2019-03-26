using Ankama.Cube.Fight;

namespace Ankama.Cube.Data
{
	public sealed class MultipleCastTargetContext : CastTargetContext
	{
		public MultipleCastTargetContext(FightStatus fightStatus, int playerId, DynamicValueHolderType type, int spellDefinitionId, int level, int instanceId, int expectedTargetCount)
			: base(fightStatus, playerId, type, spellDefinitionId, level, instanceId, expectedTargetCount)
		{
		}
	}
}
