using Ankama.Cube.Fight.Entities;
using Ankama.Cube.UI.Fight;
using Ankama.Utilities;
using System.Collections;

namespace Ankama.Cube.Fight.Events
{
	public class CompanionGivenEvent : FightEvent, IRelatedToEntity
	{
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

		public int companionLevel
		{
			get;
			private set;
		}

		public int toFightId
		{
			get;
			private set;
		}

		public int toPlayerId
		{
			get;
			private set;
		}

		public CompanionGivenEvent(int eventId, int? parentEventId, int concernedEntity, int companionDefId, int companionLevel, int toFightId, int toPlayerId)
			: base(FightEventData.Types.EventType.CompanionGiven, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.companionDefId = companionDefId;
			this.companionLevel = companionLevel;
			this.toFightId = toFightId;
			this.toPlayerId = toPlayerId;
		}

		public CompanionGivenEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.CompanionGiven, proto)
		{
			concernedEntity = proto.Int1;
			companionDefId = proto.Int2;
			companionLevel = proto.Int3;
			toFightId = proto.Int4;
			toPlayerId = proto.Int5;
		}

		public override void UpdateStatus(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out PlayerStatus entityStatus))
			{
				if (entityStatus.TryGetCompanion(companionDefId, out ReserveCompanionStatus companionStatus))
				{
					if (FightLogicExecutor.GetFightStatus(toFightId).TryGetEntity(toPlayerId, out PlayerStatus entityStatus2))
					{
						entityStatus2.AddAdditionalCompanion(companionStatus);
						AbstractPlayerUIRework view = entityStatus2.view;
						if (null != view)
						{
							view.AddAdditionalCompanionStatus(entityStatus, companionDefId, companionStatus.level);
						}
					}
					else
					{
						Log.Error(FightEventErrors.PlayerNotFound(toPlayerId, toFightId), 30, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\CompanionGivenEvent.cs");
					}
				}
				else
				{
					Log.Error(FightEventErrors.ReserveCompanionNotFound(companionDefId, concernedEntity), 35, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\CompanionGivenEvent.cs");
				}
			}
			else
			{
				Log.Error(FightEventErrors.PlayerNotFound(concernedEntity), 40, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\CompanionGivenEvent.cs");
			}
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out PlayerStatus entityStatus))
			{
				if (entityStatus.TryGetCompanion(companionDefId, out ReserveCompanionStatus companionStatus))
				{
					if (FightLogicExecutor.GetFightStatus(toFightId).TryGetEntity(toPlayerId, out PlayerStatus entityStatus2))
					{
						AbstractPlayerUIRework view = entityStatus2.view;
						if (null != view)
						{
							yield return view.AddAdditionalCompanion(entityStatus, companionDefId, companionStatus.level);
						}
					}
					else
					{
						Log.Error(FightEventErrors.PlayerNotFound(toPlayerId, toFightId), 62, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\CompanionGivenEvent.cs");
					}
				}
				else
				{
					Log.Error(FightEventErrors.ReserveCompanionNotFound(companionDefId, concernedEntity), 67, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\CompanionGivenEvent.cs");
				}
			}
			else
			{
				Log.Error(FightEventErrors.PlayerNotFound(concernedEntity), 72, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\CompanionGivenEvent.cs");
			}
		}
	}
}
