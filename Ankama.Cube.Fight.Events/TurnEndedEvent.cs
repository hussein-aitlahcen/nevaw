using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps;
using Ankama.Cube.Maps.Objects;
using Ankama.Cube.UI.Fight;
using Ankama.Utilities;
using System;
using System.Collections;

namespace Ankama.Cube.Fight.Events
{
	public class TurnEndedEvent : FightEvent, IRelatedToEntity
	{
		public int concernedEntity
		{
			get;
			private set;
		}

		public int turnIndex
		{
			get;
			private set;
		}

		public TurnEndedEvent(int eventId, int? parentEventId, int concernedEntity, int turnIndex)
			: base(FightEventData.Types.EventType.TurnEnded, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.turnIndex = turnIndex;
		}

		public TurnEndedEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.TurnEnded, proto)
		{
			concernedEntity = proto.Int1;
			turnIndex = proto.Int2;
		}

		public override void UpdateStatus(FightStatus fightStatus)
		{
			fightStatus.currentTurnPlayerId = 0;
			if (fightStatus != FightStatus.local || fightStatus.localPlayerId != concernedEntity)
			{
				return;
			}
			switch (FightCastManager.currentCastType)
			{
			case FightCastManager.CurrentCastType.Spell:
				FightCastManager.StopCastingSpell(cancelled: true);
				break;
			case FightCastManager.CurrentCastType.Companion:
				FightCastManager.StopInvokingCompanion(cancelled: true);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case FightCastManager.CurrentCastType.None:
				break;
			}
			FightMap current = FightMap.current;
			if (null != current)
			{
				current.SetNoInteractionPhase();
			}
			FightUIRework instance = FightUIRework.instance;
			if (null != instance)
			{
				instance.EndLocalPlayerTurn();
			}
			if (fightStatus.TryGetEntity(concernedEntity, out PlayerStatus entityStatus))
			{
				AbstractPlayerUIRework view = entityStatus.view;
				if (null != view)
				{
					view.SetUIInteractable(interactable: false);
				}
			}
			else
			{
				Log.Error(FightEventErrors.PlayerNotFound(concernedEntity), 60, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\TurnEndedEvent.cs");
			}
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			foreach (CharacterStatus item in fightStatus.EnumerateEntities((CharacterStatus c) => c.ownerId == concernedEntity))
			{
				IObjectWithAction objectWithAction;
				if ((objectWithAction = (item.view as IObjectWithAction)) != null)
				{
					objectWithAction.SetActionUsed(actionUsed: true, turnEnded: true);
				}
			}
			yield break;
		}
	}
}
