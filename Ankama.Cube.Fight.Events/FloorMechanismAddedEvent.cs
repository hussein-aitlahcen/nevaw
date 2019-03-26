using Ankama.Cube.Data;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps.Objects;
using Ankama.Cube.Protocols.CommonProtocol;
using Ankama.Utilities;
using System.Collections;
using UnityEngine;

namespace Ankama.Cube.Fight.Events
{
	public class FloorMechanismAddedEvent : FightEvent, IRelatedToEntity
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

		public FloorMechanismAddedEvent(int eventId, int? parentEventId, int concernedEntity, int entityDefId, int ownerId, CellCoord refCoord, int level)
			: base(FightEventData.Types.EventType.FloorMechanismAdded, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.entityDefId = entityDefId;
			this.ownerId = ownerId;
			this.refCoord = refCoord;
			this.level = level;
		}

		public FloorMechanismAddedEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.FloorMechanismAdded, proto)
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
				FloorMechanismStatus floorMechanismStatus = CreateFloorMechanismStatus(concernedEntity, entityDefId, level, entityStatus, refCoord);
				if (floorMechanismStatus != null)
				{
					fightStatus.AddEntity(floorMechanismStatus);
				}
			}
			else
			{
				Log.Error(FightEventErrors.PlayerNotFound(ownerId), 25, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\FloorMechanismAddedEvent.cs");
			}
			FightLogicExecutor.FireUpdateStatus(fightStatus.fightId, EventCategory.EntityAddedOrRemoved);
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out FloorMechanismStatus entityStatus))
			{
				if (fightStatus.TryGetEntity(ownerId, out PlayerStatus entityStatus2))
				{
					yield return CreateFloorMechanismObject(fightStatus, entityStatus, entityStatus2, refCoord.X, refCoord.Y);
				}
				else
				{
					Log.Error(FightEventErrors.PlayerNotFound(ownerId), 43, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\FloorMechanismAddedEvent.cs");
				}
			}
			else
			{
				Log.Error(FightEventErrors.EntityNotFound<FloorMechanismStatus>(concernedEntity), 48, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\FloorMechanismAddedEvent.cs");
			}
			FightLogicExecutor.FireUpdateView(fightStatus.fightId, EventCategory.EntityAddedOrRemoved);
		}

		public static FloorMechanismStatus CreateFloorMechanismStatus(int id, int definitionId, int level, PlayerStatus playerStatus, CellCoord coord)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			if (RuntimeData.floorMechanismDefinitions.TryGetValue(definitionId, out FloorMechanismDefinition value))
			{
				return FloorMechanismStatus.Create(id, value, level, playerStatus, (Vector2Int)coord);
			}
			Log.Error(FightEventErrors.EntityCreationFailed<FloorMechanismStatus, FloorMechanismDefinition>(id, definitionId), 63, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\FloorMechanismAddedEvent.cs");
			return null;
		}

		public static IEnumerator CreateFloorMechanismObject(FightStatus fightStatus, FloorMechanismStatus floorMechanismStatus, PlayerStatus ownerStatus, int x, int y)
		{
			FloorMechanismDefinition floorMechanismDefinition = (FloorMechanismDefinition)floorMechanismStatus.definition;
			if (!(null == floorMechanismDefinition))
			{
				FloorMechanismObject floorMechanismObject = FightObjectFactory.CreateFloorMechanismObject(floorMechanismDefinition, x, y);
				if (!(null == floorMechanismObject))
				{
					floorMechanismStatus.view = floorMechanismObject;
					floorMechanismObject.alliedWithLocalPlayer = (GameStatus.localPlayerTeamIndex == floorMechanismStatus.teamIndex);
					yield return floorMechanismObject.LoadAnimationDefinitions(floorMechanismDefinition.defaultSkin.value);
					floorMechanismObject.Initialize(fightStatus, ownerStatus, floorMechanismStatus);
					yield return floorMechanismObject.Spawn();
				}
			}
		}
	}
}
