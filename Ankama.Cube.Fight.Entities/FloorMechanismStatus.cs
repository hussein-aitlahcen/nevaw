using Ankama.Cube.Data;
using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Fight.Entities
{
	public class FloorMechanismStatus : MechanismStatus, IEntityWithAssemblage, IEntityWithBoardPresence, IEntity
	{
		public override EntityType type => EntityType.FloorMechanism;

		public override bool blocksMovement => false;

		public IReadOnlyList<int> assemblingIds
		{
			get;
			set;
		}

		public int? activationValue => ((FloorMechanismDefinition)base.definition).activationValue?.GetValueWithLevel(base.level);

		private FloorMechanismStatus(int id, int ownerId, int teamId, int teamIndex, int level)
			: base(id, ownerId, teamId, teamIndex, level)
		{
		}

		[NotNull]
		public static FloorMechanismStatus Create(int id, [NotNull] FloorMechanismDefinition definition, int level, PlayerStatus owner, Vector2Int position)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			int id2 = owner.id;
			int teamId = owner.teamId;
			int teamIndex = owner.teamIndex;
			Area area = definition.areaDefinition.ToArea(position);
			return new FloorMechanismStatus(id, id2, teamId, teamIndex, level)
			{
				area = area,
				definition = definition
			};
		}
	}
}
