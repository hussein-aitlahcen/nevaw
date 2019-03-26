namespace Ankama.Cube.Fight.Events
{
	public class ReserveUsedEvent : FightEvent, IRelatedToEntity
	{
		public int concernedEntity
		{
			get;
			private set;
		}

		public int valueBefore
		{
			get;
			private set;
		}

		public ReserveUsedEvent(int eventId, int? parentEventId, int concernedEntity, int valueBefore)
			: base(FightEventData.Types.EventType.ReserveUsed, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.valueBefore = valueBefore;
		}

		public ReserveUsedEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.ReserveUsed, proto)
		{
			concernedEntity = proto.Int1;
			valueBefore = proto.Int2;
		}
	}
}
