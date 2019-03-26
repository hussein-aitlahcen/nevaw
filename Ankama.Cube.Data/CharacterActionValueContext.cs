using Ankama.Cube.Fight;
using Ankama.Cube.Fight.Entities;
using JetBrains.Annotations;

namespace Ankama.Cube.Data
{
	public sealed class CharacterActionValueContext : DynamicValueFightContext
	{
		public readonly ICharacterEntity relatedCharacterStatus;

		public int? relatedCharacterActionValue => relatedCharacterStatus.actionValue;

		public CharacterActionValueContext([NotNull] FightStatus fightStatus, [NotNull] ICharacterEntity relatedCharacterStatus)
			: base(fightStatus, relatedCharacterStatus.ownerId, DynamicValueHolderType.CharacterAction, relatedCharacterStatus.level)
		{
			this.relatedCharacterStatus = relatedCharacterStatus;
		}
	}
}
