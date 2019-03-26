using Ankama.Cube.Maps;
using Ankama.Cube.Maps.Objects;
using System.Collections;

namespace Ankama.Cube.Fight.Events
{
	public class BossCastSpellEvent : FightEvent
	{
		public int spellId
		{
			get;
			private set;
		}

		public BossCastSpellEvent(int eventId, int? parentEventId, int spellId)
			: base(FightEventData.Types.EventType.BossCastSpell, eventId, parentEventId)
		{
			this.spellId = spellId;
		}

		public BossCastSpellEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.BossCastSpell, proto)
		{
			spellId = proto.Int1;
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			if (fightStatus == FightStatus.local)
			{
				FightMap current = FightMap.current;
				IBossSpell bossSpell;
				if (null != current && (bossSpell = (current.bossObject as IBossSpell)) != null)
				{
					yield return bossSpell.PlaySpellAnim(spellId);
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
