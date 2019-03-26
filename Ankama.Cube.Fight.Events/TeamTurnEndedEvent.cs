using Ankama.Cube.UI.Fight;
using System.Collections;

namespace Ankama.Cube.Fight.Events
{
	public class TeamTurnEndedEvent : FightEvent
	{
		public int turnIndex
		{
			get;
			private set;
		}

		public int teamIndex
		{
			get;
			private set;
		}

		public TeamTurnEndedEvent(int eventId, int? parentEventId, int turnIndex, int teamIndex)
			: base(FightEventData.Types.EventType.TeamTurnEnded, eventId, parentEventId)
		{
			this.turnIndex = turnIndex;
			this.teamIndex = teamIndex;
		}

		public TeamTurnEndedEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.TeamTurnEnded, proto)
		{
			turnIndex = proto.Int1;
			teamIndex = proto.Int2;
		}

		public override void UpdateStatus(FightStatus fightStatus)
		{
			FightUIRework instance = FightUIRework.instance;
			if (null != instance)
			{
				instance.EndTurn();
			}
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			FightUIRework instance = FightUIRework.instance;
			if (null != instance)
			{
				instance.ShowEndOfTurn();
			}
			yield break;
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
