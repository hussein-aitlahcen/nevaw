using Ankama.Cube.Data;
using JetBrains.Annotations;
using UnityEngine;

namespace Ankama.Cube.Fight.Entities
{
	public sealed class HeroStatus : CharacterStatus
	{
		public readonly Gender gender;

		public override EntityType type => EntityType.Hero;

		private HeroStatus(int id, int ownerId, int teamId, int teamIndex, int level, Gender gender)
			: base(id, ownerId, teamId, teamIndex, level)
		{
			this.gender = gender;
		}

		[NotNull]
		public static HeroStatus Create(int id, [NotNull] WeaponDefinition definition, int level, Gender gender, PlayerStatus owner, Vector2Int position)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			int id2 = owner.id;
			int teamId = owner.teamId;
			int teamIndex = owner.teamIndex;
			Area area = definition.areaDefinition.ToArea(position);
			HeroStatus obj = new HeroStatus(id, id2, teamId, teamIndex, level, gender)
			{
				area = area,
				definition = definition
			};
			CharacterStatus.InitializeStatus(obj, definition);
			return obj;
		}
	}
}
