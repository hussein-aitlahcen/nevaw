namespace Ankama.Cube.Fight.Events
{
	public class BossTurnEndEvent : FightEvent
	{
		public BossTurnEndEvent(int eventId, int? parentEventId)
			: base(FightEventData.Types.EventType.BossTurnEnd, eventId, parentEventId)
		{
		}

		public BossTurnEndEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.BossTurnEnd, proto)
		{
		}

		public override bool CanBeGroupedWith(FightEvent other)
		{
			return false;
		}

		public override bool SynchronizeExecution()
		{
			return true;
		}
	}
}
