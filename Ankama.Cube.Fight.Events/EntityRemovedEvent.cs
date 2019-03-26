using Ankama.Cube.Data;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps.Objects;
using Ankama.Cube.UI.Fight;
using Ankama.Cube.UI.Fight.Info;
using Ankama.Utilities;
using System;
using System.Collections;

namespace Ankama.Cube.Fight.Events
{
	public class EntityRemovedEvent : FightEvent, IRelatedToEntity
	{
		public int concernedEntity
		{
			get;
			private set;
		}

		public int reason
		{
			get;
			private set;
		}

		public EntityRemovedEvent(int eventId, int? parentEventId, int concernedEntity, int reason)
			: base(FightEventData.Types.EventType.EntityRemoved, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.reason = reason;
		}

		public EntityRemovedEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.EntityRemoved, proto)
		{
			concernedEntity = proto.Int1;
			reason = proto.Int2;
		}

		public override void UpdateStatus(FightStatus fightStatus)
		{
			fightStatus.RemoveEntity(concernedEntity);
			FightLogicExecutor.FireUpdateStatus(fightStatus.fightId, EventCategory.EntityAddedOrRemoved);
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out IEntityWithBoardPresence entityStatus))
			{
				IsoObject view = entityStatus.view;
				if (null != view)
				{
					switch (reason)
					{
					case 2:
					case 4:
					case 6:
					case 8:
					case 9:
					{
						ICharacterObject characterObject2;
						if ((characterObject2 = (view as ICharacterObject)) != null)
						{
							yield return characterObject2.Die();
						}
						HeroStatus heroStatus;
						if ((heroStatus = (entityStatus as HeroStatus)) == null)
						{
							break;
						}
						if (fightStatus.TryGetEntity(heroStatus.ownerId, out PlayerStatus entityStatus2))
						{
							AbstractPlayerUIRework view2 = entityStatus2.view;
							if (null != view2)
							{
								view2.ChangeHeroLifePoints(0);
							}
							if (GameStatus.fightType == FightType.BossFight)
							{
								FightUIRework instance = FightUIRework.instance;
								if (null != instance)
								{
									FightInfoMessage message = FightInfoMessage.HeroDeath(MessageInfoRibbonGroup.MyID);
									instance.DrawInfoMessage(message, entityStatus2.nickname);
								}
							}
						}
						else
						{
							Log.Error(FightEventErrors.PlayerNotFound(heroStatus.ownerId), 68, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\EntityRemovedEvent.cs");
						}
						break;
					}
					case 7:
					{
						IObjectWithActivation objectWithActivation;
						if ((objectWithActivation = (view as IObjectWithActivation)) != null)
						{
							yield return objectWithActivation.WaitForActivationEnd();
						}
						else
						{
							Log.Error(FightEventErrors.EntityHasIncompatibleView<IObjectWithActivation>(entityStatus), 82, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\EntityRemovedEvent.cs");
						}
						break;
					}
					case 5:
					{
						ICharacterObject characterObject;
						if ((characterObject = (view as ICharacterObject)) != null)
						{
							yield return characterObject.Die();
						}
						else
						{
							Log.Error(FightEventErrors.EntityHasIncompatibleView<ICharacterObject>(entityStatus), 95, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\EntityRemovedEvent.cs");
						}
						break;
					}
					case 3:
						throw new ArgumentException("Transformations should not trigger an EntityRemovedEvent.");
					default:
						throw new ArgumentOutOfRangeException($"EntityRemovedReason not handled: {reason}");
					case 1:
						break;
					}
					view.DetachFromCell();
					view.Destroy();
				}
				else
				{
					Log.Error(FightEventErrors.EntityHasNoView(entityStatus), 112, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\EntityRemovedEvent.cs");
				}
			}
			else
			{
				Log.Error(FightEventErrors.EntityNotFound<IEntityWithBoardPresence>(concernedEntity), 117, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\EntityRemovedEvent.cs");
			}
			FightLogicExecutor.FireUpdateView(fightStatus.fightId, EventCategory.EntityAddedOrRemoved);
		}
	}
}
