using Ankama.Cube.Data;
using JetBrains.Annotations;
using UnityEngine;

namespace Ankama.Cube.Fight.Entities
{
	public sealed class CompanionStatus : CharacterStatus
	{
		public override EntityType type => EntityType.Companion;

		private CompanionStatus(int id, int ownerId, int teamId, int teamIndex, int level)
			: base(id, ownerId, teamId, teamIndex, level)
		{
		}

		[NotNull]
		public static CompanionStatus Create(int id, [NotNull] CompanionDefinition definition, int level, PlayerStatus owner, Vector2Int position)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			int id2 = owner.id;
			int teamId = owner.teamId;
			int teamIndex = owner.teamIndex;
			Area area = definition.areaDefinition.ToArea(position);
			CompanionStatus obj = new CompanionStatus(id, id2, teamId, teamIndex, level)
			{
				area = area,
				definition = definition
			};
			CharacterStatus.InitializeStatus(obj, definition);
			return obj;
		}
	}
}
