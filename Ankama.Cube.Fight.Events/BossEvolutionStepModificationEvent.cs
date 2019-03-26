using Ankama.Cube.Maps;
using Ankama.Cube.Maps.Objects;
using System.Collections;

namespace Ankama.Cube.Fight.Events
{
	public class BossEvolutionStepModificationEvent : FightEvent
	{
		public int valueBefore
		{
			get;
			private set;
		}

		public int valueAfter
		{
			get;
			private set;
		}

		public BossEvolutionStepModificationEvent(int eventId, int? parentEventId, int valueBefore, int valueAfter)
			: base(FightEventData.Types.EventType.BossEvolutionStepModification, eventId, parentEventId)
		{
			this.valueBefore = valueBefore;
			this.valueAfter = valueAfter;
		}

		public BossEvolutionStepModificationEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.BossEvolutionStepModification, proto)
		{
			valueBefore = proto.Int1;
			valueAfter = proto.Int2;
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			if (fightStatus != FightStatus.local)
			{
				yield break;
			}
			FightMap current = FightMap.current;
			if (null != current)
			{
				current.SetBossEvolutionStep(valueAfter);
				IBossEvolution bossEvolution;
				if ((bossEvolution = (current.bossObject as IBossEvolution)) != null)
				{
					yield return bossEvolution.PlayLevelChangeAnim(valueBefore, valueAfter);
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
