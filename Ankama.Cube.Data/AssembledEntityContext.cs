using Ankama.Cube.Fight;
using Ankama.Cube.Fight.Entities;
using JetBrains.Annotations;

namespace Ankama.Cube.Data
{
	public sealed class AssembledEntityContext : DynamicValueFightContext
	{
		public readonly IEntityWithAssemblage assembling;

		public AssembledEntityContext([NotNull] FightStatus fightStatus, int playerId, IEntityWithAssemblage assembling, int level)
			: base(fightStatus, playerId, DynamicValueHolderType.AssembledEntity, level)
		{
			this.assembling = assembling;
		}
	}
}
