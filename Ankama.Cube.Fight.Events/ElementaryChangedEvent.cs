using Ankama.Cube.Data;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps.Objects;
using Ankama.Utilities;
using System.Collections;

namespace Ankama.Cube.Fight.Events
{
	public class ElementaryChangedEvent : FightEvent, IRelatedToEntity
	{
		public int concernedEntity
		{
			get;
			private set;
		}

		public int elementaryState
		{
			get;
			private set;
		}

		public ElementaryChangedEvent(int eventId, int? parentEventId, int concernedEntity, int elementaryState)
			: base(FightEventData.Types.EventType.ElementaryChanged, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.elementaryState = elementaryState;
		}

		public ElementaryChangedEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.ElementaryChanged, proto)
		{
			concernedEntity = proto.Int1;
			elementaryState = proto.Int2;
		}

		public override void UpdateStatus(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out IEntityWithElementaryState entityStatus))
			{
				entityStatus.ChangeElementaryState((ElementaryStates)elementaryState);
			}
			else
			{
				Log.Error(FightEventErrors.EntityNotFound<IEntityWithElementaryState>(concernedEntity), 19, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\ElementaryChangedEvent.cs");
			}
			FightLogicExecutor.FireUpdateStatus(fightStatus.fightId, EventCategory.ElementaryStateChanged);
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out IEntityWithBoardPresence entityStatus))
			{
				IObjectWithElementaryState objectWithElementaryState;
				if ((objectWithElementaryState = (entityStatus.view as IObjectWithElementaryState)) != null)
				{
					objectWithElementaryState.SetElementaryState((ElementaryStates)elementaryState);
				}
				else
				{
					Log.Error(FightEventErrors.EntityHasIncompatibleView<IObjectWithElementaryState>(entityStatus), 35, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\ElementaryChangedEvent.cs");
				}
			}
			else
			{
				Log.Error(FightEventErrors.EntityNotFound<IEntityWithBoardPresence>(concernedEntity), 40, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\ElementaryChangedEvent.cs");
			}
			FightLogicExecutor.FireUpdateView(fightStatus.fightId, EventCategory.ElementaryStateChanged);
			yield break;
		}
	}
}
