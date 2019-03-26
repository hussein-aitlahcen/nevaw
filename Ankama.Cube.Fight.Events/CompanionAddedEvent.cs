using Ankama.Cube.Data;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps;
using Ankama.Cube.Maps.Objects;
using Ankama.Cube.Protocols.CommonProtocol;
using Ankama.Cube.UI.Fight;
using Ankama.Utilities;
using System.Collections;
using UnityEngine;

namespace Ankama.Cube.Fight.Events
{
	public class CompanionAddedEvent : FightEvent, IRelatedToEntity, ICharacterAdded
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

		public int direction
		{
			get;
			private set;
		}

		public int level
		{
			get;
			private set;
		}

		public CompanionAddedEvent(int eventId, int? parentEventId, int concernedEntity, int entityDefId, int ownerId, CellCoord refCoord, int direction, int level)
			: base(FightEventData.Types.EventType.CompanionAdded, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.entityDefId = entityDefId;
			this.ownerId = ownerId;
			this.refCoord = refCoord;
			this.direction = direction;
			this.level = level;
		}

		public CompanionAddedEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.CompanionAdded, proto)
		{
			concernedEntity = proto.Int1;
			entityDefId = proto.Int2;
			ownerId = proto.Int3;
			direction = proto.Int4;
			level = proto.Int5;
			refCoord = proto.CellCoord1;
		}

		public override void UpdateStatus(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(ownerId, out PlayerStatus entityStatus))
			{
				CompanionStatus companionStatus = CreateCompanionStatus(concernedEntity, entityDefId, level, entityStatus, refCoord);
				if (companionStatus != null)
				{
					fightStatus.AddEntity(companionStatus);
				}
				if (entityStatus.isLocalPlayer)
				{
					FightCastManager.CheckCompanionInvoked(entityDefId);
				}
			}
			else
			{
				Log.Error(FightEventErrors.PlayerNotFound(ownerId), 32, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\CompanionAddedEvent.cs");
			}
			FightLogicExecutor.FireUpdateStatus(fightStatus.fightId, EventCategory.EntityAddedOrRemoved);
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out CompanionStatus companionStatus))
			{
				if (fightStatus.TryGetEntity(ownerId, out PlayerStatus ownerStatus))
				{
					if (!ownerStatus.isLocalPlayer)
					{
						FightMap current = FightMap.current;
						if (null != current)
						{
							if (current.TryGetCellObject(refCoord.X, refCoord.Y, out CellObject cellObject))
							{
								ReserveCompanionStatus reserveCompanion = new ReserveCompanionStatus(ownerStatus, (CompanionDefinition)companionStatus.definition, level);
								yield return FightUIRework.ShowPlayingCompanion(reserveCompanion, cellObject);
							}
							else
							{
								Log.Error(FightEventErrors.InvalidPosition(refCoord), 59, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\CompanionAddedEvent.cs");
							}
						}
					}
					yield return CreateCompanionCharacterObject(fightStatus, companionStatus, ownerStatus, refCoord.X, refCoord.Y, (Direction)direction);
				}
				else
				{
					Log.Error(FightEventErrors.PlayerNotFound(ownerId), 70, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\CompanionAddedEvent.cs");
				}
			}
			else
			{
				Log.Error(FightEventErrors.EntityNotFound<CompanionStatus>(concernedEntity), 75, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\CompanionAddedEvent.cs");
			}
			FightLogicExecutor.FireUpdateView(fightStatus.fightId, EventCategory.EntityAddedOrRemoved);
		}

		public static CompanionStatus CreateCompanionStatus(int id, int definitionId, int level, PlayerStatus playerStatus, CellCoord coord)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			if (RuntimeData.companionDefinitions.TryGetValue(definitionId, out CompanionDefinition value))
			{
				return CompanionStatus.Create(id, value, level, playerStatus, (Vector2Int)coord);
			}
			Log.Error(FightEventErrors.EntityCreationFailed<CompanionStatus, CompanionDefinition>(id, definitionId), 90, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\CompanionAddedEvent.cs");
			return null;
		}

		public static IEnumerator CreateCompanionCharacterObject(FightStatus fightStatus, CompanionStatus companionStatus, PlayerStatus ownerStatus, int x, int y, Direction direction)
		{
			CompanionDefinition companionDefinition = (CompanionDefinition)companionStatus.definition;
			if (!(null == companionDefinition))
			{
				CompanionCharacterObject companionCharacterObject = FightObjectFactory.CreateCompanionCharacterObject(companionDefinition, x, y, direction);
				if (!(null == companionCharacterObject))
				{
					companionStatus.view = companionCharacterObject;
					yield return companionCharacterObject.LoadAnimationDefinitions(companionDefinition.defaultSkin.value);
					companionCharacterObject.Initialize(fightStatus, ownerStatus, companionStatus);
					yield return companionCharacterObject.Spawn();
				}
			}
		}
	}
}
