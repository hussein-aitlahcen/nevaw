namespace Ankama.Cube.Fight.Events
{
	public class EntityProtectionAddedEvent : FightEvent, IRelatedToEntity
	{
		public int concernedEntity
		{
			get;
			private set;
		}

		public int protectorId
		{
			get;
			private set;
		}

		public int fixedValue
		{
			get;
			private set;
		}

		public int percentsValues
		{
			get;
			private set;
		}

		public EntityProtectionAddedEvent(int eventId, int? parentEventId, int concernedEntity, int protectorId, int fixedValue, int percentsValues)
			: base(FightEventData.Types.EventType.EntityProtectionAdded, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.protectorId = protectorId;
			this.fixedValue = fixedValue;
			this.percentsValues = percentsValues;
		}

		public EntityProtectionAddedEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.EntityProtectionAdded, proto)
		{
			concernedEntity = proto.Int1;
			protectorId = proto.Int2;
			fixedValue = proto.Int3;
			percentsValues = proto.Int4;
		}
	}
}
