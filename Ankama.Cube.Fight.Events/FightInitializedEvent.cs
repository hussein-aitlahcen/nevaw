using Ankama.Cube.Maps;
using System.Collections;

namespace Ankama.Cube.Fight.Events
{
	public class FightInitializedEvent : FightEvent
	{
		public FightInitializedEvent(int eventId, int? parentEventId)
			: base(FightEventData.Types.EventType.FightInitialized, eventId, parentEventId)
		{
		}

		public FightInitializedEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.FightInitialized, proto)
		{
		}

		public override void UpdateStatus(FightStatus fightStatus)
		{
			if (fightStatus == FightStatus.local)
			{
				FightLogicExecutor.fightInitialized = true;
			}
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			if (fightStatus != FightStatus.local)
			{
				yield break;
			}
			CameraHandler current = CameraHandler.current;
			if (null != current)
			{
				FightMap current2 = FightMap.current;
				if (null != current2)
				{
					yield return current.FocusOnMapRegion(current2.definition, fightStatus.fightId, current.cinematicControlParameters);
				}
			}
		}
	}
}
