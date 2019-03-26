using Ankama.Cube.States;
using Ankama.Utilities;
using System.Collections;

namespace Ankama.Cube.Fight.Events
{
	public class TurnSynchronizationEvent : FightEvent
	{
		public int tmp
		{
			get;
			private set;
		}

		public TurnSynchronizationEvent(int eventId, int? parentEventId, int tmp)
			: base(FightEventData.Types.EventType.TurnSynchronization, eventId, parentEventId)
		{
			this.tmp = tmp;
		}

		public TurnSynchronizationEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.TurnSynchronization, proto)
		{
			tmp = proto.Int1;
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			FightState instance = FightState.instance;
			if (instance == null)
			{
				Log.Error("Could not find fight state.", 17, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\TurnSynchronizationEvent.cs");
				yield break;
			}
			FightFrame frame = instance.frame;
			if (frame == null)
			{
				Log.Error("Could not retrieve fight frame in fight state.", 24, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\TurnSynchronizationEvent.cs");
			}
			else
			{
				frame.SendPlayerReady();
			}
		}
	}
}
