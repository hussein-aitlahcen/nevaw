using System.Collections;

namespace Ankama.Cube.Fight.Events
{
	public class BossReserveModificationEvent : FightEvent
	{
		public int valueBefore
		{
			get;
			private set;
		}

		public int valueAfter
		{
			get;
			private set;
		}

		public BossReserveModificationEvent(int eventId, int? parentEventId, int valueBefore, int valueAfter)
			: base(FightEventData.Types.EventType.BossReserveModification, eventId, parentEventId)
		{
			this.valueBefore = valueBefore;
			this.valueAfter = valueAfter;
		}

		public BossReserveModificationEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.BossReserveModification, proto)
		{
			valueBefore = proto.Int1;
			valueAfter = proto.Int2;
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			FightStatus local = FightStatus.local;
			yield break;
		}
	}
}
