namespace Ankama.Cube.Fight.Events
{
	public class DiceThrownEvent : FightEvent, IRelatedToEntity
	{
		public int concernedEntity
		{
			get;
			private set;
		}

		public int diceValue
		{
			get;
			private set;
		}

		public DiceThrownEvent(int eventId, int? parentEventId, int concernedEntity, int diceValue)
			: base(FightEventData.Types.EventType.DiceThrown, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.diceValue = diceValue;
		}

		public DiceThrownEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.DiceThrown, proto)
		{
			concernedEntity = proto.Int1;
			diceValue = proto.Int2;
		}
	}
}
