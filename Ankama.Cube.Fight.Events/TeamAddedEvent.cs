namespace Ankama.Cube.Fight.Events
{
	public class TeamAddedEvent : FightEvent, IRelatedToEntity
	{
		public int concernedEntity
		{
			get;
			private set;
		}

		public int teamIndex
		{
			get;
			private set;
		}

		public bool isLocalTeam
		{
			get;
			private set;
		}

		public TeamAddedEvent(int eventId, int? parentEventId, int concernedEntity, int teamIndex, bool isLocalTeam)
			: base(FightEventData.Types.EventType.TeamAdded, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.teamIndex = teamIndex;
			this.isLocalTeam = isLocalTeam;
		}

		public TeamAddedEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.TeamAdded, proto)
		{
			concernedEntity = proto.Int1;
			teamIndex = proto.Int2;
			isLocalTeam = proto.Bool1;
		}

		public override void UpdateStatus(FightStatus fightStatus)
		{
			if (isLocalTeam)
			{
				GameStatus.localPlayerTeamIndex = teamIndex;
			}
		}
	}
}
