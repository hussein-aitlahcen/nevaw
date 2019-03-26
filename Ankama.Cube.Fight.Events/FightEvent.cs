using System.Collections;
using System.Collections.Generic;

namespace Ankama.Cube.Fight.Events
{
	public abstract class FightEvent
	{
		public readonly FightEventData.Types.EventType eventType;

		public readonly int eventId;

		public readonly int? parentEventId;

		public FightEvent parentEvent
		{
			get;
			private set;
		}

		public FightEvent firstChildEvent
		{
			get;
			private set;
		}

		public FightEvent nextSiblingEvent
		{
			get;
			private set;
		}

		protected FightEvent(FightEventData.Types.EventType eventType, int eventId, int? parentEventId)
		{
			this.eventType = eventType;
			this.eventId = eventId;
			this.parentEventId = parentEventId;
		}

		protected FightEvent(FightEventData.Types.EventType eventType, FightEventData proto)
		{
			this.eventType = eventType;
			eventId = proto.EventId;
			parentEventId = proto.ParentEventId;
		}

		public void AddChildEvent(FightEvent fightEvent)
		{
			fightEvent.parentEvent = this;
			if (firstChildEvent == null)
			{
				firstChildEvent = fightEvent;
				return;
			}
			FightEvent fightEvent2 = firstChildEvent;
			FightEvent fightEvent3;
			do
			{
				fightEvent3 = fightEvent2;
				fightEvent2 = fightEvent3.nextSiblingEvent;
			}
			while (fightEvent2 != null);
			fightEvent3.nextSiblingEvent = fightEvent;
		}

		public virtual bool IsInvisible()
		{
			return false;
		}

		public virtual bool CanBeGroupedWith(FightEvent other)
		{
			if (eventType == other.eventType)
			{
				return parentEventId == other.parentEventId;
			}
			return false;
		}

		public virtual bool SynchronizeExecution()
		{
			return false;
		}

		public IEnumerable<FightEvent> EnumerateChildren()
		{
			for (FightEvent childEvent = firstChildEvent; childEvent != null; childEvent = childEvent.nextSiblingEvent)
			{
				yield return childEvent;
			}
		}

		public virtual void UpdateStatus(FightStatus fightStatus)
		{
		}

		public virtual IEnumerator UpdateView(FightStatus fightStatus)
		{
			yield break;
		}
	}
}
