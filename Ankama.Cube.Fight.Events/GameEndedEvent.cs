using Ankama.Cube.Protocols.CommonProtocol;
using Ankama.Cube.Protocols.FightCommonProtocol;
using Ankama.Cube.States;
using Ankama.Cube.UI.Fight;
using Ankama.Utilities;
using JetBrains.Annotations;
using System.Collections;

namespace Ankama.Cube.Fight.Events
{
	public class GameEndedEvent : FightEvent
	{
		public FightResult result
		{
			get;
			private set;
		}

		[CanBeNull]
		public GameStatistics gameStats
		{
			get;
			private set;
		}

		public int fightDuration
		{
			get;
			private set;
		}

		public GameEndedEvent(int eventId, int? parentEventId, FightResult result, GameStatistics gameStats, int fightDuration)
			: base(FightEventData.Types.EventType.GameEnded, eventId, parentEventId)
		{
			this.result = result;
			this.gameStats = gameStats;
			this.fightDuration = fightDuration;
		}

		public GameEndedEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.GameEnded, proto)
		{
			result = proto.FightResult1;
			gameStats = proto.GameStatistics1;
			fightDuration = proto.Int1;
		}

		public override void UpdateStatus(FightStatus fightStatus)
		{
			GameStatus.hasEnded = true;
			FightUIRework instance = FightUIRework.instance;
			if (null != instance)
			{
				instance.SetResignButtonEnabled(value: false);
			}
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			FightState instance = FightState.instance;
			if (instance != null)
			{
				instance.GotoFightEndState(result, gameStats, fightDuration);
			}
			else
			{
				Log.Error("Could not find fight state.", 30, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\GameEndedEvent.cs");
			}
			yield break;
		}
	}
}
