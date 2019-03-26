using Ankama.Cube.Data;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.UI.Fight;
using Ankama.Utilities;
using System.Collections;

namespace Ankama.Cube.Fight.Events
{
	public class ReservePointsChangedEvent : FightEvent, IRelatedToEntity
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

		public ReservePointsChangedEvent(int eventId, int? parentEventId, int concernedEntity, int valueBefore, int valueAfter)
			: base(FightEventData.Types.EventType.ReservePointsChanged, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.valueBefore = valueBefore;
			this.valueAfter = valueAfter;
		}

		public ReservePointsChangedEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.ReservePointsChanged, proto)
		{
			concernedEntity = proto.Int1;
			valueBefore = proto.Int2;
			valueAfter = proto.Int3;
		}

		public override void UpdateStatus(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out PlayerStatus entityStatus))
			{
				if (entityStatus.reservePoints != valueBefore)
				{
					Log.Warning($"The previous reserve points value ({entityStatus.reservePoints}) for player with id {concernedEntity} doesn't match the value in the event ({valueBefore}).", 17, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\ReservePointsChangedEvent.cs");
				}
				entityStatus.SetCarac(CaracId.ReservePoints, valueAfter);
				AbstractPlayerUIRework view = entityStatus.view;
				if (!(null != view))
				{
				}
			}
			else
			{
				Log.Error(FightEventErrors.PlayerNotFound(concernedEntity), 31, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\ReservePointsChangedEvent.cs");
			}
			FightLogicExecutor.FireUpdateStatus(fightStatus.fightId, EventCategory.ReserveChanged);
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out PlayerStatus entityStatus))
			{
				AbstractPlayerUIRework view = entityStatus.view;
				if (null != view)
				{
					view.ChangeReservePoints(valueAfter);
				}
			}
			else
			{
				Log.Error(FightEventErrors.PlayerNotFound(concernedEntity), 49, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\ReservePointsChangedEvent.cs");
			}
			FightLogicExecutor.FireUpdateView(fightStatus.fightId, EventCategory.ReserveChanged);
			yield break;
		}
	}
}
