using Ankama.Cube.Data;
using Ankama.Cube.Fight.Entities;
using Ankama.Utilities;
using System.Collections;

namespace Ankama.Cube.Fight.Events
{
	public class MagicalHealModifierChangedEvent : FightEvent, IRelatedToEntity
	{
		public int concernedEntity
		{
			get;
			private set;
		}

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

		public MagicalHealModifierChangedEvent(int eventId, int? parentEventId, int concernedEntity, int valueBefore, int valueAfter)
			: base(FightEventData.Types.EventType.MagicalHealModifierChanged, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.valueBefore = valueBefore;
			this.valueAfter = valueAfter;
		}

		public MagicalHealModifierChangedEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.MagicalHealModifierChanged, proto)
		{
			concernedEntity = proto.Int1;
			valueBefore = proto.Int2;
			valueAfter = proto.Int3;
		}

		public override void UpdateStatus(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out IEntity entityStatus))
			{
				entityStatus.SetCarac(CaracId.MagicalHealModifier, valueAfter);
			}
			else
			{
				Log.Error(FightEventErrors.EntityNotFound<IEntity>(concernedEntity), 18, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\MagicalHealModifierChangedEvent.cs");
			}
			FightLogicExecutor.FireUpdateStatus(fightStatus.fightId, EventCategory.HealModifierChanged);
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			FightLogicExecutor.FireUpdateView(fightStatus.fightId, EventCategory.HealModifierChanged);
			yield break;
		}
	}
}
