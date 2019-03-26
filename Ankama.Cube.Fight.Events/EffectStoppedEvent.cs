namespace Ankama.Cube.Fight.Events
{
	public class EffectStoppedEvent : FightEvent
	{
		public EffectStoppedEvent(int eventId, int? parentEventId)
			: base(FightEventData.Types.EventType.EffectStopped, eventId, parentEventId)
		{
		}

		public EffectStoppedEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.EffectStopped, proto)
		{
		}
	}
}
