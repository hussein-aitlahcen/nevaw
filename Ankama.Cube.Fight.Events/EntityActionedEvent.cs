using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps.Objects;
using Ankama.Utilities;
using System.Collections;

namespace Ankama.Cube.Fight.Events
{
	public class EntityActionedEvent : FightEvent, IRelatedToEntity
	{
		public int concernedEntity
		{
			get;
			private set;
		}

		public bool executedByPlayer
		{
			get;
			private set;
		}

		public EntityActionedEvent(int eventId, int? parentEventId, int concernedEntity, bool executedByPlayer)
			: base(FightEventData.Types.EventType.EntityActioned, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.executedByPlayer = executedByPlayer;
		}

		public EntityActionedEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.EntityActioned, proto)
		{
			concernedEntity = proto.Int1;
			executedByPlayer = proto.Bool1;
		}

		public override void UpdateStatus(FightStatus fightStatus)
		{
			if (executedByPlayer)
			{
				if (fightStatus.TryGetEntity(concernedEntity, out ICharacterEntity entityStatus))
				{
					entityStatus.actionUsed = true;
					fightStatus.NotifyEntityPlayableStateChanged();
				}
				else
				{
					Log.Error(FightEventErrors.EntityNotFound<CharacterStatus>(concernedEntity), 22, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\EntityActionedEvent.cs");
				}
			}
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			if (!executedByPlayer)
			{
				yield break;
			}
			if (fightStatus.TryGetEntity(concernedEntity, out IEntityWithBoardPresence entityStatus))
			{
				IObjectWithAction objectWithAction;
				if ((objectWithAction = (entityStatus.view as IObjectWithAction)) != null)
				{
					objectWithAction.SetActionUsed(actionUsed: true, turnEnded: false);
				}
				else
				{
					Log.Error(FightEventErrors.EntityHasIncompatibleView<IObjectWithAction>(entityStatus), 39, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\EntityActionedEvent.cs");
				}
			}
			else
			{
				Log.Error(FightEventErrors.EntityNotFound<IEntityWithBoardPresence>(concernedEntity), 44, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\EntityActionedEvent.cs");
			}
		}
	}
}
