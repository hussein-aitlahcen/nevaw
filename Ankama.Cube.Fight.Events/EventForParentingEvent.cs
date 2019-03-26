namespace Ankama.Cube.Fight.Events
{
	public class EventForParentingEvent : FightEvent
	{
		public EventForParentingEvent(int eventId, int? parentEventId)
			: base(FightEventData.Types.EventType.EventForParenting, eventId, parentEventId)
		{
		}

		public EventForParentingEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.EventForParenting, proto)
		{
		}
	}
}
