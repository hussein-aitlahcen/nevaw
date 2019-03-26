using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps.Objects;
using Ankama.Utilities;
using System.Collections;

namespace Ankama.Cube.Fight.Events
{
	public class EntityActionResetEvent : FightEvent, IRelatedToEntity
	{
		public int concernedEntity
		{
			get;
			private set;
		}

		public EntityActionResetEvent(int eventId, int? parentEventId, int concernedEntity)
			: base(FightEventData.Types.EventType.EntityActionReset, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
		}

		public EntityActionResetEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.EntityActionReset, proto)
		{
			concernedEntity = proto.Int1;
		}

		public override void UpdateStatus(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out CharacterStatus entityStatus))
			{
				entityStatus.actionUsed = false;
				fightStatus.NotifyEntityPlayableStateChanged();
			}
			else
			{
				Log.Error(FightEventErrors.EntityNotFound<CharacterStatus>(concernedEntity), 20, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\EntityActionResetEvent.cs");
			}
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out IEntityWithBoardPresence entityStatus))
			{
				IObjectWithAction objectWithAction;
				if ((objectWithAction = (entityStatus.view as IObjectWithAction)) != null)
				{
					objectWithAction.SetActionUsed(actionUsed: false, turnEnded: false);
				}
				else
				{
					Log.Error(FightEventErrors.EntityHasIncompatibleView<IObjectWithAction>(entityStatus), 34, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\EntityActionResetEvent.cs");
				}
			}
			else
			{
				Log.Error(FightEventErrors.EntityNotFound<IEntityWithBoardPresence>(concernedEntity), 39, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\EntityActionResetEvent.cs");
			}
			yield break;
		}
	}
}
