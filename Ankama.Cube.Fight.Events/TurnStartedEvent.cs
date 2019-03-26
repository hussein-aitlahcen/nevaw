using Ankama.Cube.Data;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps;
using Ankama.Cube.Maps.Objects;
using Ankama.Cube.UI.Fight;
using Ankama.Utilities;
using System;
using System.Collections;

namespace Ankama.Cube.Fight.Events
{
	public class TurnStartedEvent : FightEvent, IRelatedToEntity
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

		public TurnStartedEvent(int eventId, int? parentEventId, int concernedEntity, int turnIndex)
			: base(FightEventData.Types.EventType.TurnStarted, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.turnIndex = turnIndex;
		}

		public TurnStartedEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.TurnStarted, proto)
		{
			concernedEntity = proto.Int1;
			turnIndex = proto.Int2;
		}

		public override void UpdateStatus(FightStatus fightStatus)
		{
			fightStatus.currentTurnPlayerId = concernedEntity;
			foreach (CharacterStatus item in fightStatus.EnumerateEntities((CharacterStatus c) => c.ownerId == concernedEntity))
			{
				item.actionUsed = false;
			}
			fightStatus.NotifyEntityPlayableStateChanged();
			if (fightStatus != FightStatus.local || fightStatus.localPlayerId != concernedEntity)
			{
				return;
			}
			FightMap current = FightMap.current;
			if (null != current)
			{
				current.SetMovementPhase();
			}
			FightUIRework instance = FightUIRework.instance;
			if (null != instance)
			{
				instance.StartLocalPlayerTurn();
			}
			if (fightStatus.TryGetEntity(concernedEntity, out PlayerStatus entityStatus))
			{
				AbstractPlayerUIRework view = entityStatus.view;
				if (null != view)
				{
					view.SetUIInteractable(interactable: true);
				}
			}
			else
			{
				Log.Error(FightEventErrors.PlayerNotFound(concernedEntity), 55, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\TurnStartedEvent.cs");
			}
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			foreach (CharacterStatus item in fightStatus.EnumerateEntities((CharacterStatus c) => c.ownerId == concernedEntity))
			{
				IObjectWithAction objectWithAction;
				if ((objectWithAction = (item.view as IObjectWithAction)) != null)
				{
					objectWithAction.SetActionUsed(actionUsed: false, turnEnded: false);
				}
			}
			if (fightStatus != FightStatus.local)
			{
				yield break;
			}
			FightUIRework instance = FightUIRework.instance;
			if (!(null != instance))
			{
				yield break;
			}
			switch (GameStatus.fightType)
			{
			case FightType.BossFight:
			case FightType.TeamVersus:
				break;
			case FightType.Versus:
				if (fightStatus.TryGetEntity(concernedEntity, out PlayerStatus entityStatus))
				{
					if (entityStatus.isLocalPlayer)
					{
						yield return instance.ShowTurnFeedback(TurnFeedbackUI.Type.Player, 61373);
					}
					else
					{
						yield return instance.ShowTurnFeedback(TurnFeedbackUI.Type.Opponent, 30091);
					}
				}
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
	}
}
