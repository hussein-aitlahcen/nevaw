using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Protocols.FightCommonProtocol;
using Ankama.Cube.UI.Fight;
using Ankama.Utilities;
using System.Collections;

namespace Ankama.Cube.Fight.Events
{
	public class CompanionReserveStateChangedEvent : FightEvent, IRelatedToEntity
	{
		private bool m_wasGiven;

		public int concernedEntity
		{
			get;
			private set;
		}

		public int companionDefId
		{
			get;
			private set;
		}

		public CompanionReserveState state
		{
			get;
			private set;
		}

		public bool hasPreviousState
		{
			get;
			private set;
		}

		public CompanionReserveState previousState
		{
			get;
			private set;
		}

		public CompanionReserveStateChangedEvent(int eventId, int? parentEventId, int concernedEntity, int companionDefId, CompanionReserveState state, bool hasPreviousState, CompanionReserveState previousState)
			: base(FightEventData.Types.EventType.CompanionReserveStateChanged, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.companionDefId = companionDefId;
			this.state = state;
			this.hasPreviousState = hasPreviousState;
			this.previousState = previousState;
		}

		public CompanionReserveStateChangedEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.CompanionReserveStateChanged, proto)
		{
			concernedEntity = proto.Int1;
			companionDefId = proto.Int2;
			state = proto.CompanionReserveState1;
			previousState = proto.CompanionReserveState2;
			hasPreviousState = proto.Bool1;
		}

		public override void UpdateStatus(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out PlayerStatus entityStatus))
			{
				if (!entityStatus.TryGetCompanion(companionDefId, out ReserveCompanionStatus companionStatus))
				{
					Log.Error(FightEventErrors.ReserveCompanionNotFound(companionDefId, concernedEntity), 19, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\CompanionReserveStateChangedEvent.cs");
				}
				m_wasGiven = companionStatus.isGiven;
				if (m_wasGiven && state == CompanionReserveState.Dead)
				{
					entityStatus.RemoveAdditionalCompanion(companionDefId);
				}
				else
				{
					companionStatus.SetState(state);
				}
				AbstractPlayerUIRework view = entityStatus.view;
				if (null != view)
				{
					if (m_wasGiven && state == CompanionReserveState.Dead)
					{
						view.RemoveAdditionalCompanionStatus(companionDefId);
					}
					else
					{
						view.ChangeCompanionStateStatus(companionDefId, state);
					}
				}
			}
			else
			{
				Log.Error(FightEventErrors.PlayerNotFound(concernedEntity), 48, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\CompanionReserveStateChangedEvent.cs");
			}
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out PlayerStatus entityStatus))
			{
				AbstractPlayerUIRework view = entityStatus.view;
				if (null != view)
				{
					if (m_wasGiven && state == CompanionReserveState.Dead)
					{
						yield return view.RemoveAdditionalCompanion(companionDefId);
					}
					else
					{
						yield return view.ChangeCompanionState(companionDefId, state);
					}
				}
			}
			else
			{
				Log.Error(FightEventErrors.PlayerNotFound(concernedEntity), 71, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\CompanionReserveStateChangedEvent.cs");
			}
		}
	}
}
