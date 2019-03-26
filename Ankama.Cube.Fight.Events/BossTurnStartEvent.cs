using Ankama.Cube.UI.Fight;
using System.Collections;

namespace Ankama.Cube.Fight.Events
{
	public class BossTurnStartEvent : FightEvent
	{
		public BossTurnStartEvent(int eventId, int? parentEventId)
			: base(FightEventData.Types.EventType.BossTurnStart, eventId, parentEventId)
		{
		}

		public BossTurnStartEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.BossTurnStart, proto)
		{
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			if (fightStatus == FightStatus.local)
			{
				FightUIRework instance = FightUIRework.instance;
				if (null != instance)
				{
					int i18nNameId = GameStatus.fightDefinition.i18nNameId;
					yield return instance.ShowTurnFeedback(TurnFeedbackUI.Type.Boss, i18nNameId);
				}
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
