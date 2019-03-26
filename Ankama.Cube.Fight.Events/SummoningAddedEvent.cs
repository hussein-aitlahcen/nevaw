using Ankama.Cube.Data;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps.Objects;
using Ankama.Cube.Protocols.CommonProtocol;
using Ankama.Utilities;
using System.Collections;
using UnityEngine;

namespace Ankama.Cube.Fight.Events
{
	public class SummoningAddedEvent : FightEvent, IRelatedToEntity, ICharacterAdded
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

		public SummoningAddedEvent(int eventId, int? parentEventId, int concernedEntity, int entityDefId, int ownerId, CellCoord refCoord, int direction, int level)
			: base(FightEventData.Types.EventType.SummoningAdded, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.entityDefId = entityDefId;
			this.ownerId = ownerId;
			this.refCoord = refCoord;
			this.direction = direction;
			this.level = level;
		}

		public SummoningAddedEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.SummoningAdded, proto)
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
			if (!fightStatus.TryGetEntity(ownerId, out PlayerStatus entityStatus))
			{
				Log.Error(string.Format("Could not find a {0} entity with id {1}.", "PlayerStatus", ownerId), 17, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\SummoningAddedEvent.cs");
				return;
			}
			SummoningStatus summoningStatus = CreateSummoningStatus(concernedEntity, entityDefId, level, entityStatus, refCoord);
			if (summoningStatus != null)
			{
				fightStatus.AddEntity(summoningStatus);
				FightLogicExecutor.FireUpdateStatus(fightStatus.fightId, EventCategory.EntityAddedOrRemoved);
			}
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out SummoningStatus entityStatus))
			{
				if (fightStatus.TryGetEntity(ownerId, out PlayerStatus entityStatus2))
				{
					yield return CreateSummoningCharacterObject(fightStatus, entityStatus, entityStatus2, refCoord.X, refCoord.Y, (Direction)direction);
				}
				else
				{
					Log.Error(FightEventErrors.PlayerNotFound(ownerId), 44, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\SummoningAddedEvent.cs");
				}
			}
			else
			{
				Log.Error(FightEventErrors.EntityNotFound<SummoningStatus>(concernedEntity), 49, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\SummoningAddedEvent.cs");
			}
			FightLogicExecutor.FireUpdateView(fightStatus.fightId, EventCategory.EntityAddedOrRemoved);
		}

		public static SummoningStatus CreateSummoningStatus(int id, int definitionId, int level, PlayerStatus playerStatus, CellCoord coord)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			if (RuntimeData.summoningDefinitions.TryGetValue(definitionId, out SummoningDefinition value))
			{
				return SummoningStatus.Create(id, value, level, playerStatus, (Vector2Int)coord);
			}
			Log.Error(FightEventErrors.DefinitionNotFound<SummoningDefinition>(definitionId), 64, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\SummoningAddedEvent.cs");
			return null;
		}

		public static IEnumerator CreateSummoningCharacterObject(FightStatus fightStatus, SummoningStatus summoningStatus, PlayerStatus ownerStatus, int x, int y, Direction direction)
		{
			SummoningDefinition summoningDefinition = (SummoningDefinition)summoningStatus.definition;
			if (!(null == summoningDefinition))
			{
				SummoningCharacterObject summoningCharacterObject = FightObjectFactory.CreateSummoningCharacterObject(summoningDefinition, x, y, direction);
				if (!(null == summoningCharacterObject))
				{
					summoningStatus.view = summoningCharacterObject;
					yield return summoningCharacterObject.LoadAnimationDefinitions(summoningDefinition.defaultSkin.value);
					summoningCharacterObject.Initialize(fightStatus, ownerStatus, summoningStatus);
					yield return summoningCharacterObject.Spawn();
				}
			}
		}
	}
}
