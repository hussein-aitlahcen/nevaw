using Ankama.Cube.Data;
using JetBrains.Annotations;
using UnityEngine;

namespace Ankama.Cube.Fight.Entities
{
	public class ObjectMechanismStatus : MechanismStatus, IEntityWithLife, IEntity, IEntityTargetableByAction, IEntityWithBoardPresence
	{
		public override EntityType type => EntityType.ObjectMechanism;

		public int baseLife => 0;

		public int life => GetCarac(CaracId.Life);

		public bool hasArmor => GetCarac(CaracId.Armor) > 0;

		public int armor => GetCarac(CaracId.Armor);

		public int armoredLife => life + armor;

		public int resistance => GetCarac(CaracId.Resistance);

		public int hitLimit => GetCarac(CaracId.HitLimit);

		public bool wounded => life < baseLife;

		public override bool blocksMovement => true;

		public ObjectMechanismStatus(int id, int ownerId, int teamId, int teamIndex, int level)
			: base(id, ownerId, teamId, teamIndex, level)
		{
		}

		[NotNull]
		public static ObjectMechanismStatus Create(int id, [NotNull] ObjectMechanismDefinition definition, int level, PlayerStatus owner, Vector2Int position)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			int id2 = owner.id;
			int teamId = owner.teamId;
			int teamIndex = owner.teamIndex;
			Area area = definition.areaDefinition.ToArea(position);
			int valueWithLevel = definition.baseMecaLife.GetValueWithLevel(level);
			ObjectMechanismStatus objectMechanismStatus = new ObjectMechanismStatus(id, id2, teamId, teamIndex, level);
			objectMechanismStatus.area = area;
			objectMechanismStatus.definition = definition;
			objectMechanismStatus.SetCarac(CaracId.Life, valueWithLevel);
			return objectMechanismStatus;
		}
	}
}
