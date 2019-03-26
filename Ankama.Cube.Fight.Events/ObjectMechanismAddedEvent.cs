using Ankama.Cube.Data;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps.Objects;
using Ankama.Cube.Protocols.CommonProtocol;
using Ankama.Utilities;
using System.Collections;
using UnityEngine;

namespace Ankama.Cube.Fight.Events
{
	public class ObjectMechanismAddedEvent : FightEvent, IRelatedToEntity
	{
		public int concernedEntity
		{
			get;
			private set;
		}

		public int entityDefId
		{
			get;
			private set;
		}

		public int ownerId
		{
			get;
			private set;
		}

		public CellCoord refCoord
		{
			get;
			private set;
		}

		public int level
		{
			get;
			private set;
		}

		public ObjectMechanismAddedEvent(int eventId, int? parentEventId, int concernedEntity, int entityDefId, int ownerId, CellCoord refCoord, int level)
			: base(FightEventData.Types.EventType.ObjectMechanismAdded, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.entityDefId = entityDefId;
			this.ownerId = ownerId;
			this.refCoord = refCoord;
			this.level = level;
		}

		public ObjectMechanismAddedEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.ObjectMechanismAdded, proto)
		{
			concernedEntity = proto.Int1;
			entityDefId = proto.Int2;
			ownerId = proto.Int3;
			level = proto.Int4;
			refCoord = proto.CellCoord1;
		}

		public override void UpdateStatus(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(ownerId, out PlayerStatus entityStatus))
			{
				ObjectMechanismStatus objectMechanismStatus = CreateObjectMechanismStatus(concernedEntity, entityDefId, level, entityStatus, refCoord);
				if (objectMechanismStatus != null)
				{
					fightStatus.AddEntity(objectMechanismStatus);
				}
			}
			else
			{
				Log.Error(FightEventErrors.PlayerNotFound(ownerId), 25, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\ObjectMechanismAddedEvent.cs");
			}
			FightLogicExecutor.FireUpdateStatus(fightStatus.fightId, EventCategory.EntityAddedOrRemoved);
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out ObjectMechanismStatus entityStatus))
			{
				if (fightStatus.TryGetEntity(ownerId, out PlayerStatus entityStatus2))
				{
					yield return CreateObjectMechanismObject(fightStatus, entityStatus, entityStatus2, refCoord.X, refCoord.Y);
				}
				else
				{
					Log.Error(FightEventErrors.PlayerNotFound(ownerId), 44, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\ObjectMechanismAddedEvent.cs");
				}
			}
			else
			{
				Log.Error(FightEventErrors.EntityNotFound<ObjectMechanismStatus>(concernedEntity), 49, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\ObjectMechanismAddedEvent.cs");
			}
			FightLogicExecutor.FireUpdateView(fightStatus.fightId, EventCategory.EntityAddedOrRemoved);
		}

		public static ObjectMechanismStatus CreateObjectMechanismStatus(int id, int definitionId, int level, PlayerStatus playerStatus, CellCoord coord)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			if (RuntimeData.objectMechanismDefinitions.TryGetValue(definitionId, out ObjectMechanismDefinition value))
			{
				return ObjectMechanismStatus.Create(id, value, level, playerStatus, (Vector2Int)coord);
			}
			Log.Error(FightEventErrors.DefinitionNotFound<ObjectMechanismDefinition>(definitionId), 64, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\ObjectMechanismAddedEvent.cs");
			return null;
		}

		public static IEnumerator CreateObjectMechanismObject(FightStatus fightStatus, ObjectMechanismStatus objectMechanismStatus, PlayerStatus ownerStatus, int x, int y)
		{
			ObjectMechanismDefinition objectMechanismDefinition = (ObjectMechanismDefinition)objectMechanismStatus.definition;
			if (!(null == objectMechanismDefinition))
			{
				ObjectMechanismObject objectMechanismObject = FightObjectFactory.CreateObjectMechanismObject(objectMechanismDefinition, x, y);
				if (!(null == objectMechanismObject))
				{
					objectMechanismStatus.view = objectMechanismObject;
					objectMechanismObject.alliedWithLocalPlayer = (GameStatus.localPlayerTeamIndex == ownerStatus.teamIndex);
					yield return objectMechanismObject.LoadAnimationDefinitions(objectMechanismDefinition.defaultSkin.value);
					objectMechanismObject.Initialize(fightStatus, ownerStatus, objectMechanismStatus);
					yield return objectMechanismObject.Spawn();
				}
			}
		}
	}
}
