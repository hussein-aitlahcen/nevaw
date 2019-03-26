using Ankama.Cube.Data;
using Ankama.Cube.Fight.Entities;
using Ankama.Utilities;
using System.Collections;

namespace Ankama.Cube.Fight.Events
{
	public class MagicalDamageModifierChangedEvent : FightEvent, IRelatedToEntity
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

		public MagicalDamageModifierChangedEvent(int eventId, int? parentEventId, int concernedEntity, int valueBefore, int valueAfter)
			: base(FightEventData.Types.EventType.MagicalDamageModifierChanged, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.valueBefore = valueBefore;
			this.valueAfter = valueAfter;
		}

		public MagicalDamageModifierChangedEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.MagicalDamageModifierChanged, proto)
		{
			concernedEntity = proto.Int1;
			valueBefore = proto.Int2;
			valueAfter = proto.Int3;
		}

		public override void UpdateStatus(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out IEntity entityStatus))
			{
				entityStatus.SetCarac(CaracId.MagicalDamageModifier, valueAfter);
			}
			else
			{
				Log.Error(FightEventErrors.EntityNotFound<IEntity>(concernedEntity), 18, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\MagicalDamageModifierChangedEvent.cs");
			}
			FightLogicExecutor.FireUpdateStatus(fightStatus.fightId, EventCategory.DamageModifierChanged);
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			FightLogicExecutor.FireUpdateView(fightStatus.fightId, EventCategory.DamageModifierChanged);
			yield break;
		}
	}
}
