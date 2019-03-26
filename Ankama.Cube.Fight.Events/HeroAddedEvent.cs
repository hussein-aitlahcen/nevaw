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
	public class HeroAddedEvent : FightEvent, IRelatedToEntity, ICharacterAdded
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

		public int gender
		{
			get;
			private set;
		}

		public int skinId
		{
			get;
			private set;
		}

		public HeroAddedEvent(int eventId, int? parentEventId, int concernedEntity, int entityDefId, int ownerId, CellCoord refCoord, int direction, int level, int gender, int skinId)
			: base(FightEventData.Types.EventType.HeroAdded, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.entityDefId = entityDefId;
			this.ownerId = ownerId;
			this.refCoord = refCoord;
			this.direction = direction;
			this.level = level;
			this.gender = gender;
			this.skinId = skinId;
		}

		public HeroAddedEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.HeroAdded, proto)
		{
			concernedEntity = proto.Int1;
			entityDefId = proto.Int2;
			ownerId = proto.Int3;
			direction = proto.Int4;
			level = proto.Int5;
			gender = proto.Int6;
			skinId = proto.Int7;
			refCoord = proto.CellCoord1;
		}

		public override void UpdateStatus(FightStatus fightStatus)
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			if (fightStatus.TryGetEntity(ownerId, out PlayerStatus entityStatus))
			{
				Gender gender = (Gender)this.gender;
				if (RuntimeData.weaponDefinitions.TryGetValue(entityDefId, out WeaponDefinition value))
				{
					HeroStatus heroStatus = HeroStatus.Create(concernedEntity, value, level, gender, entityStatus, (Vector2Int)refCoord);
					fightStatus.AddEntity(heroStatus);
					entityStatus.heroStatus = heroStatus;
					AbstractPlayerUIRework view = entityStatus.view;
					if (null != view)
					{
						view.SetHeroIllustration(value, gender);
						view.SetHeroStartLifePoints(heroStatus.baseLife, entityStatus.playerType);
						if (RuntimeData.reserveDefinitions.TryGetValue(value.god, out ReserveDefinition value2))
						{
							view.SetupReserve(heroStatus, value2);
						}
						else
						{
							Log.Error(FightEventErrors.DefinitionNotFound<ReserveDefinition>((int)value.god), 45, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\HeroAddedEvent.cs");
						}
					}
				}
				else
				{
					Log.Error(FightEventErrors.EntityCreationFailed<HeroStatus, WeaponDefinition>(concernedEntity, entityDefId), 51, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\HeroAddedEvent.cs");
				}
			}
			else
			{
				Log.Error(FightEventErrors.PlayerNotFound(ownerId), 56, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\HeroAddedEvent.cs");
			}
			FightLogicExecutor.FireUpdateStatus(fightStatus.fightId, EventCategory.EntityAddedOrRemoved);
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out HeroStatus heroStatus))
			{
				if (fightStatus.TryGetEntity(ownerId, out PlayerStatus ownerStatus))
				{
					WeaponDefinition weaponDefinition = (WeaponDefinition)heroStatus.definition;
					if (null != weaponDefinition)
					{
						HeroCharacterObject heroCharacterObject = FightObjectFactory.CreateHeroCharacterObject(weaponDefinition, refCoord.X, refCoord.Y, (Direction)direction);
						if (null != heroCharacterObject)
						{
							heroStatus.view = heroCharacterObject;
							yield return heroCharacterObject.LoadAnimationDefinitions(skinId, (Gender)gender);
							heroCharacterObject.Initialize(fightStatus, ownerStatus, heroStatus);
							UpdateAudioContext(ownerStatus, heroStatus.baseLife);
							yield return heroCharacterObject.Spawn();
						}
					}
				}
				else
				{
					Log.Error(FightEventErrors.PlayerNotFound(ownerId), 98, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\HeroAddedEvent.cs");
				}
			}
			else
			{
				Log.Error(FightEventErrors.EntityNotFound<HeroStatus>(concernedEntity), 103, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\HeroAddedEvent.cs");
			}
			FightLogicExecutor.FireUpdateView(fightStatus.fightId, EventCategory.EntityAddedOrRemoved);
		}

		private static void UpdateAudioContext(PlayerStatus ownerStatus, int life)
		{
			if (ownerStatus.isLocalPlayer)
			{
				FightMap current = FightMap.current;
				if (null != current)
				{
					current.SetLocalPlayerHeroLife(life, life);
				}
			}
		}
	}
}
