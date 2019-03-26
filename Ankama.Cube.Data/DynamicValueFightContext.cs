using Ankama.Cube.Fight;
using JetBrains.Annotations;

namespace Ankama.Cube.Data
{
	public abstract class DynamicValueFightContext : DynamicValueContext
	{
		public readonly FightStatus fightStatus;

		public readonly int playerId;

		protected DynamicValueFightContext([NotNull] FightStatus fightStatus, int playerId, DynamicValueHolderType type, int level)
			: base(type, level)
		{
			this.fightStatus = fightStatus;
			this.playerId = playerId;
		}
	}
}
