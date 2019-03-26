using Ankama.Cube.Fight;

namespace Ankama.Cube.Data
{
	public sealed class TwoCastTargetContext : CastTargetContext
	{
		public TwoCastTargetContext(FightStatus fightStatus, int playerId, DynamicValueHolderType type, int spellDefinitionId, int level, int instanceId)
			: base(fightStatus, playerId, type, spellDefinitionId, level, instanceId, 2)
		{
		}
	}
}
