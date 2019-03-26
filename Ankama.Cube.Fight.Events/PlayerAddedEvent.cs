using Ankama.Cube.Data;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps;
using Ankama.Cube.UI.Fight;

namespace Ankama.Cube.Fight.Events
{
	public class PlayerAddedEvent : FightEvent, IRelatedToEntity
	{
		public int concernedEntity
		{
			get;
			private set;
		}

		public string name
		{
			get;
			private set;
		}

		public int teamId
		{
			get;
			private set;
		}

		public bool isLocalPlayer
		{
			get;
			private set;
		}

		public int baseActionPoints
		{
			get;
			private set;
		}

		public int index
		{
			get;
			private set;
		}

		public int teamIndex
		{
			get;
			private set;
		}

		public PlayerAddedEvent(int eventId, int? parentEventId, int concernedEntity, string name, int teamId, bool isLocalPlayer, int baseActionPoints, int index, int teamIndex)
			: base(FightEventData.Types.EventType.PlayerAdded, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.name = name;
			this.teamId = teamId;
			this.isLocalPlayer = isLocalPlayer;
			this.baseActionPoints = baseActionPoints;
			this.index = index;
			this.teamIndex = teamIndex;
		}

		public PlayerAddedEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.PlayerAdded, proto)
		{
			concernedEntity = proto.Int1;
			teamId = proto.Int2;
			baseActionPoints = proto.Int3;
			index = proto.Int4;
			teamIndex = proto.Int5;
			name = proto.String1;
			isLocalPlayer = proto.Bool1;
		}

		public override void UpdateStatus(FightStatus fightStatus)
		{
			PlayerType playerType = (PlayerType)(isLocalPlayer ? 13 : (((teamIndex == GameStatus.localPlayerTeamIndex) ? 1 : 2) | ((fightStatus == FightStatus.local) ? 4 : 0)));
			PlayerStatus playerStatus = new PlayerStatus(concernedEntity, fightStatus.fightId, index, teamId, teamIndex, name, playerType);
			fightStatus.AddEntity(playerStatus);
			playerStatus.SetCarac(CaracId.ActionPoints, baseActionPoints);
			if (isLocalPlayer)
			{
				fightStatus.localPlayerId = concernedEntity;
				CameraHandler current = CameraHandler.current;
				if (null != current)
				{
					DirectionAngle mapRotation = GameStatus.GetMapRotation(playerStatus);
					current.ChangeRotation(mapRotation);
				}
			}
			FightUIRework instance = FightUIRework.instance;
			if (null != instance)
			{
				AbstractPlayerUIRework abstractPlayerUIRework2 = playerStatus.view = ((!isLocalPlayer) ? ((AbstractPlayerUIRework)instance.AddPlayer(playerStatus)) : ((AbstractPlayerUIRework)instance.GetLocalPlayerUI(playerStatus)));
				abstractPlayerUIRework2.SetPlayerStatus(playerStatus);
				abstractPlayerUIRework2.SetPlayerName(playerStatus.nickname);
				abstractPlayerUIRework2.SetRankIcon(0);
				abstractPlayerUIRework2.SetActionPoints(baseActionPoints);
				abstractPlayerUIRework2.SetReservePoints(0);
				abstractPlayerUIRework2.SetElementaryPoints(0, 0, 0, 0);
			}
		}
	}
}
