using Ankama.Cube.Fight;
using JetBrains.Annotations;

namespace Ankama.Cube.Data
{
	public sealed class ReserveCompanionValueContext : DynamicValueFightContext
	{
		public ReserveCompanionValueContext([NotNull] FightStatus fightStatus, int playerId, int level)
			: base(fightStatus, playerId, DynamicValueHolderType.Companion, level)
		{
		}
	}
}
