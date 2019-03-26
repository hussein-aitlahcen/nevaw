namespace Ankama.Cube.Fight.Events
{
	public class EntityProtectionRemovedEvent : FightEvent, IRelatedToEntity
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

		public EntityProtectionRemovedEvent(int eventId, int? parentEventId, int concernedEntity, int protectorId)
			: base(FightEventData.Types.EventType.EntityProtectionRemoved, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.protectorId = protectorId;
		}

		public EntityProtectionRemovedEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.EntityProtectionRemoved, proto)
		{
			concernedEntity = proto.Int1;
			protectorId = proto.Int2;
		}
	}
}
