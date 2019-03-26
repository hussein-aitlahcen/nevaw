using Ankama.Cube.Data;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps.Objects;
using Ankama.Utilities;
using System.Collections;

namespace Ankama.Cube.Fight.Events
{
	public class PhysicalHealModifierChangedEvent : FightEvent, IRelatedToEntity
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

		public PhysicalHealModifierChangedEvent(int eventId, int? parentEventId, int concernedEntity, int valueBefore, int valueAfter)
			: base(FightEventData.Types.EventType.PhysicalHealModifierChanged, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.valueBefore = valueBefore;
			this.valueAfter = valueAfter;
		}

		public PhysicalHealModifierChangedEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.PhysicalHealModifierChanged, proto)
		{
			concernedEntity = proto.Int1;
			valueBefore = proto.Int2;
			valueAfter = proto.Int3;
		}

		public override void UpdateStatus(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out IEntityWithBoardPresence entityStatus))
			{
				entityStatus.SetCarac(CaracId.PhysicalHealModifier, valueAfter);
			}
			else
			{
				Log.Error(FightEventErrors.EntityNotFound<IEntityWithBoardPresence>(concernedEntity), 19, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\PhysicalHealModifierChangedEvent.cs");
			}
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out IEntityWithBoardPresence entityStatus))
			{
				IObjectWithAction objectWithAction;
				if ((objectWithAction = (entityStatus.view as IObjectWithAction)) != null)
				{
					objectWithAction.SetPhysicalHealBoost(valueAfter);
				}
				else
				{
					Log.Error(FightEventErrors.EntityHasIncompatibleView<IEntityWithBoardPresence>(entityStatus), 33, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\PhysicalHealModifierChangedEvent.cs");
				}
			}
			else
			{
				Log.Error(FightEventErrors.EntityNotFound<IEntityWithBoardPresence>(concernedEntity), 38, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\PhysicalHealModifierChangedEvent.cs");
			}
			yield break;
		}
	}
}
