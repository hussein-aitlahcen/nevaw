using Ankama.Cube.Data;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps.Objects;
using Ankama.Cube.Protocols.CommonProtocol;
using Ankama.Utilities;
using System;
using System.Collections;

namespace Ankama.Cube.Fight.Events
{
	public class TransformationEvent : FightEvent, IRelatedToEntity
	{
		public int concernedEntity
		{
			get;
			private set;
		}

		public int newEntityType
		{
			get;
			private set;
		}

		public int newEntityId
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

		public bool copyActionUsed
		{
			get;
			private set;
		}

		public TransformationEvent(int eventId, int? parentEventId, int concernedEntity, int newEntityType, int newEntityId, int entityDefId, int ownerId, CellCoord refCoord, int direction, int level, bool copyActionUsed)
			: base(FightEventData.Types.EventType.Transformation, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.newEntityType = newEntityType;
			this.newEntityId = newEntityId;
			this.entityDefId = entityDefId;
			this.ownerId = ownerId;
			this.refCoord = refCoord;
			this.direction = direction;
			this.level = level;
			this.copyActionUsed = copyActionUsed;
		}

		public TransformationEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.Transformation, proto)
		{
			concernedEntity = proto.Int1;
			newEntityType = proto.Int2;
			newEntityId = proto.Int3;
			entityDefId = proto.Int4;
			ownerId = proto.Int5;
			direction = proto.Int6;
			level = proto.Int7;
			refCoord = proto.CellCoord1;
			copyActionUsed = proto.Bool1;
		}

		public override void UpdateStatus(FightStatus fightStatus)
		{
			fightStatus.RemoveEntity(concernedEntity);
			if (fightStatus.TryGetEntity(ownerId, out PlayerStatus entityStatus))
			{
				EntityType newEntityType = (EntityType)this.newEntityType;
				EntityStatus entityStatus2;
				switch (newEntityType)
				{
				case EntityType.Hero:
					throw new Exception("[TransformationEvent] Heroes cannot be transformed.");
				case EntityType.Companion:
					entityStatus2 = CompanionAddedEvent.CreateCompanionStatus(newEntityId, entityDefId, level, entityStatus, refCoord);
					break;
				case EntityType.Summoning:
					entityStatus2 = SummoningAddedEvent.CreateSummoningStatus(newEntityId, entityDefId, level, entityStatus, refCoord);
					break;
				case EntityType.FloorMechanism:
					entityStatus2 = FloorMechanismAddedEvent.CreateFloorMechanismStatus(newEntityId, entityDefId, level, entityStatus, refCoord);
					break;
				case EntityType.ObjectMechanism:
					entityStatus2 = ObjectMechanismAddedEvent.CreateObjectMechanismStatus(newEntityId, entityDefId, level, entityStatus, refCoord);
					break;
				case EntityType.Global:
				case EntityType.Player:
				case EntityType.Team:
					Log.Error($"Transformation not handled for type {newEntityType}.", 48, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\TransformationEvent.cs");
					return;
				default:
					throw new ArgumentOutOfRangeException();
				}
				IEntityWithAction entityWithAction;
				if (copyActionUsed && fightStatus.TryGetEntity(concernedEntity, out IEntityWithAction entityStatus3) && (entityWithAction = (entityStatus2 as IEntityWithAction)) != null)
				{
					entityWithAction.actionUsed = entityStatus3.actionUsed;
					fightStatus.NotifyEntityPlayableStateChanged();
				}
				fightStatus.AddEntity(entityStatus2);
			}
			else
			{
				Log.Error(FightEventErrors.PlayerNotFound(ownerId), 72, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\TransformationEvent.cs");
			}
			FightLogicExecutor.FireUpdateStatus(fightStatus.fightId, EventCategory.EntityAddedOrRemoved);
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			Direction direction = Direction.SouthEast;
			if (fightStatus.TryGetEntity(concernedEntity, out IEntityWithBoardPresence entityStatus))
			{
				IsoObject view = entityStatus.view;
				if (null != view)
				{
					ICharacterObject characterObject;
					if ((characterObject = (view as ICharacterObject)) != null)
					{
						direction = characterObject.direction;
					}
					view.DetachFromCell();
					view.Destroy();
				}
				else
				{
					Log.Error(FightEventErrors.EntityHasNoView(entityStatus), 97, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\TransformationEvent.cs");
				}
			}
			else
			{
				Log.Error(FightEventErrors.EntityNotFound<IEntityWithBoardPresence>(concernedEntity), 102, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\TransformationEvent.cs");
			}
			if (fightStatus.TryGetEntity(newEntityId, out IEntityWithBoardPresence entityStatus2))
			{
				if (fightStatus.TryGetEntity(ownerId, out PlayerStatus entityStatus3))
				{
					EntityType newEntityType = (EntityType)this.newEntityType;
					switch (newEntityType)
					{
					case EntityType.Hero:
						throw new Exception("[TransformationEvent] Heroes cannot be transformed.");
					case EntityType.Companion:
						yield return CompanionAddedEvent.CreateCompanionCharacterObject(fightStatus, (CompanionStatus)entityStatus2, entityStatus3, refCoord.X, refCoord.Y, direction);
						break;
					case EntityType.Summoning:
						yield return SummoningAddedEvent.CreateSummoningCharacterObject(fightStatus, (SummoningStatus)entityStatus2, entityStatus3, refCoord.X, refCoord.Y, direction);
						break;
					case EntityType.FloorMechanism:
						yield return FloorMechanismAddedEvent.CreateFloorMechanismObject(fightStatus, (FloorMechanismStatus)entityStatus2, entityStatus3, refCoord.X, refCoord.Y);
						break;
					case EntityType.ObjectMechanism:
						yield return ObjectMechanismAddedEvent.CreateObjectMechanismObject(fightStatus, (ObjectMechanismStatus)entityStatus2, entityStatus3, refCoord.X, refCoord.Y);
						break;
					case EntityType.Global:
					case EntityType.Player:
					case EntityType.Team:
						Log.Error($"Transformation not handled for type {newEntityType}.", 134, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\TransformationEvent.cs");
						break;
					default:
						throw new ArgumentOutOfRangeException();
					}
				}
				else
				{
					Log.Error(FightEventErrors.PlayerNotFound(ownerId), 143, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\TransformationEvent.cs");
				}
			}
			else
			{
				Log.Error(FightEventErrors.EntityNotFound<IEntityWithBoardPresence>(newEntityId), 148, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\TransformationEvent.cs");
			}
			FightLogicExecutor.FireUpdateView(fightStatus.fightId, EventCategory.EntityAddedOrRemoved);
		}
	}
}
