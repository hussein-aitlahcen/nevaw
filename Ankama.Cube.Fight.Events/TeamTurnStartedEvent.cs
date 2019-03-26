using Ankama.Cube.Data;
using Ankama.Cube.Maps;
using Ankama.Cube.UI.Fight;
using System;
using System.Collections;

namespace Ankama.Cube.Fight.Events
{
	public class TeamTurnStartedEvent : FightEvent
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

		public int turnDuration
		{
			get;
			private set;
		}

		public TeamTurnStartedEvent(int eventId, int? parentEventId, int turnIndex, int teamIndex, int turnDuration)
			: base(FightEventData.Types.EventType.TeamTurnStarted, eventId, parentEventId)
		{
			this.turnIndex = turnIndex;
			this.teamIndex = teamIndex;
			this.turnDuration = turnDuration;
		}

		public TeamTurnStartedEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.TeamTurnStarted, proto)
		{
			turnIndex = proto.Int1;
			teamIndex = proto.Int2;
			turnDuration = proto.Int3;
		}

		public override void UpdateStatus(FightStatus fightStatus)
		{
			fightStatus.turnIndex = turnIndex;
			if (fightStatus == FightStatus.local)
			{
				FightUIRework instance = FightUIRework.instance;
				if (null != instance)
				{
					bool isLocalPlayerTeam = GameStatus.localPlayerTeamIndex == teamIndex;
					instance.StartTurn(turnIndex, turnDuration, isLocalPlayerTeam);
				}
			}
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			if (fightStatus != FightStatus.local)
			{
				yield break;
			}
			FightUIRework instance = FightUIRework.instance;
			if (null != instance)
			{
				switch (GameStatus.fightType)
				{
				case FightType.BossFight:
					if (GameStatus.localPlayerTeamIndex == teamIndex)
					{
						TurnFeedbackUI.Type type2 = fightStatus.isEnded ? TurnFeedbackUI.Type.PlayerTeam : TurnFeedbackUI.Type.Player;
						yield return instance.ShowTurnFeedback(type2, 61373);
					}
					else
					{
						yield return instance.ShowTurnFeedback(TurnFeedbackUI.Type.Boss, 30091);
					}
					break;
				case FightType.TeamVersus:
					if (GameStatus.localPlayerTeamIndex == teamIndex)
					{
						TurnFeedbackUI.Type type = fightStatus.isEnded ? TurnFeedbackUI.Type.PlayerTeam : TurnFeedbackUI.Type.Player;
						yield return instance.ShowTurnFeedback(type, 61373);
					}
					else
					{
						yield return instance.ShowTurnFeedback(TurnFeedbackUI.Type.OpponentTeam, 30091);
					}
					break;
				default:
					throw new ArgumentOutOfRangeException();
				case FightType.Versus:
					break;
				}
			}
			FightMap current = FightMap.current;
			if (null != current)
			{
				current.SetTurnIndex(turnIndex);
			}
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
