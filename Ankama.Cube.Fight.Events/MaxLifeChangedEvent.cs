using Ankama.Cube.Data;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps;
using Ankama.Cube.Maps.Objects;
using Ankama.Cube.UI.Fight;
using Ankama.Utilities;
using System.Collections;

namespace Ankama.Cube.Fight.Events
{
	public class MaxLifeChangedEvent : FightEvent, IRelatedToEntity
	{
		public int concernedEntity
		{
			get;
			private set;
		}

		public int maxLifeBefore
		{
			get;
			private set;
		}

		public int maxLifeAfter
		{
			get;
			private set;
		}

		public MaxLifeChangedEvent(int eventId, int? parentEventId, int concernedEntity, int maxLifeBefore, int maxLifeAfter)
			: base(FightEventData.Types.EventType.MaxLifeChanged, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.maxLifeBefore = maxLifeBefore;
			this.maxLifeAfter = maxLifeAfter;
		}

		public MaxLifeChangedEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.MaxLifeChanged, proto)
		{
			concernedEntity = proto.Int1;
			maxLifeBefore = proto.Int2;
			maxLifeAfter = proto.Int3;
		}

		public override void UpdateStatus(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out IEntityWithLife entityStatus))
			{
				entityStatus.SetCarac(CaracId.LifeMax, maxLifeAfter);
			}
			else
			{
				Log.Error(FightEventErrors.EntityNotFound<IEntityWithLife>(concernedEntity), 21, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\MaxLifeChangedEvent.cs");
			}
			FightLogicExecutor.FireUpdateStatus(fightStatus.fightId, EventCategory.LifeArmorChanged);
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out IEntityWithBoardPresence entityStatus))
			{
				IObjectWithArmoredLife objectWithArmoredLife;
				if ((objectWithArmoredLife = (entityStatus.view as IObjectWithArmoredLife)) != null)
				{
					objectWithArmoredLife.SetBaseLife(maxLifeAfter);
				}
				else
				{
					Log.Error(FightEventErrors.EntityHasIncompatibleView<IObjectWithArmoredLife>(entityStatus), 37, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\MaxLifeChangedEvent.cs");
				}
				if (entityStatus.type == EntityType.Hero)
				{
					HeroStatus heroStatus = (HeroStatus)entityStatus;
					if (heroStatus.ownerId == fightStatus.localPlayerId)
					{
						FightMap current = FightMap.current;
						if (null != current)
						{
							current.SetLocalPlayerHeroLife(heroStatus.life, maxLifeAfter);
						}
					}
					if (fightStatus.TryGetEntity(heroStatus.ownerId, out PlayerStatus entityStatus2))
					{
						AbstractPlayerUIRework view = entityStatus2.view;
						if (null != view)
						{
							view.ChangeHeroBaseLifePoints(maxLifeAfter);
						}
					}
					else
					{
						Log.Error(FightEventErrors.PlayerNotFound(heroStatus.ownerId), 62, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\MaxLifeChangedEvent.cs");
					}
				}
			}
			else
			{
				Log.Error(FightEventErrors.EntityNotFound<IEntityWithBoardPresence>(concernedEntity), 68, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\MaxLifeChangedEvent.cs");
			}
			FightLogicExecutor.FireUpdateView(fightStatus.fightId, EventCategory.LifeArmorChanged);
			yield break;
		}
	}
}
