using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Protocols.FightCommonProtocol;
using Ankama.Cube.UI.Fight;
using Ankama.Utilities;
using System.Collections;
using System.Collections.Generic;

namespace Ankama.Cube.Fight.Events
{
	public class TeamsScoreModificationEvent : FightEvent
	{
		public int firstTeamScoreBefore
		{
			get;
			private set;
		}

		public int firstTeamScoreAfter
		{
			get;
			private set;
		}

		public int secondTeamScoreBefore
		{
			get;
			private set;
		}

		public int secondTeamScoreAfter
		{
			get;
			private set;
		}

		public TeamsScoreModificationReason reason
		{
			get;
			private set;
		}

		public int relatedFightId
		{
			get;
			private set;
		}

		public IReadOnlyList<int> relatedPlayersId
		{
			get;
			private set;
		}

		public TeamsScoreModificationEvent(int eventId, int? parentEventId, int firstTeamScoreBefore, int firstTeamScoreAfter, int secondTeamScoreBefore, int secondTeamScoreAfter, TeamsScoreModificationReason reason, int relatedFightId, IReadOnlyList<int> relatedPlayersId)
			: base(FightEventData.Types.EventType.TeamsScoreModification, eventId, parentEventId)
		{
			this.firstTeamScoreBefore = firstTeamScoreBefore;
			this.firstTeamScoreAfter = firstTeamScoreAfter;
			this.secondTeamScoreBefore = secondTeamScoreBefore;
			this.secondTeamScoreAfter = secondTeamScoreAfter;
			this.reason = reason;
			this.relatedFightId = relatedFightId;
			this.relatedPlayersId = relatedPlayersId;
		}

		public TeamsScoreModificationEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.TeamsScoreModification, proto)
		{
			firstTeamScoreBefore = proto.Int1;
			firstTeamScoreAfter = proto.Int2;
			secondTeamScoreBefore = proto.Int3;
			secondTeamScoreAfter = proto.Int4;
			relatedFightId = proto.Int5;
			reason = proto.TeamsScoreModificationReason1;
			relatedPlayersId = (IReadOnlyList<int>)proto.IntList1;
		}

		public override void UpdateStatus(FightStatus fightStatus)
		{
			FightScore score = GetScore();
			GameStatus.allyTeamPoints = score.myTeamScore.scoreAfter;
			GameStatus.opponentTeamPoints = score.opponentTeamScore.scoreAfter;
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			FightStatus fightStatus2 = GameStatus.GetFightStatus(relatedFightId);
			string playerName = GetPlayerName(fightStatus2);
			FightUIRework instance = FightUIRework.instance;
			if (null != instance)
			{
				FightScore score = GetScore();
				instance.SetScore(score, playerName, reason);
			}
			yield break;
		}

		private string GetPlayerName(FightStatus concernedFight)
		{
			IReadOnlyList<int> relatedPlayersId = this.relatedPlayersId;
			switch (relatedPlayersId.Count)
			{
			case 0:
				Log.Warning("No player was specified as the source of a team score modification.", 46, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\TeamsScoreModificationEvent.cs");
				return string.Empty;
			case 1:
				if (concernedFight.TryGetEntity(relatedPlayersId[0], out PlayerStatus entityStatus2))
				{
					return entityStatus2.nickname;
				}
				Log.Error($"Could not find player with id {relatedPlayersId[0]}.", 59, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\TeamsScoreModificationEvent.cs");
				return string.Empty;
			default:
				if (concernedFight.TryGetEntity(relatedPlayersId[0], out PlayerStatus entityStatus))
				{
					return "Team #" + entityStatus.teamIndex;
				}
				return string.Empty;
			}
		}

		private FightScore GetScore()
		{
			FightScore.Score score = new FightScore.Score(firstTeamScoreBefore, firstTeamScoreAfter);
			FightScore.Score score2 = new FightScore.Score(secondTeamScoreBefore, secondTeamScoreAfter);
			if (GameStatus.localPlayerTeamIndex != 0)
			{
				return new FightScore(score2, score);
			}
			return new FightScore(score, score2);
		}
	}
}
