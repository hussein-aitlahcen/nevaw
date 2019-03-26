using Ankama.Cube.Data;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.UI.Fight;
using Ankama.Cube.UI.Fight.Info;
using Ankama.Utilities;
using System.Collections;

namespace Ankama.Cube.Fight.Events
{
	public class CompanionReceivedEvent : FightEvent, IRelatedToEntity
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

		public int fightId
		{
			get;
			private set;
		}

		public int fromFightId
		{
			get;
			private set;
		}

		public int fromPlayerId
		{
			get;
			private set;
		}

		public CompanionReceivedEvent(int eventId, int? parentEventId, int concernedEntity, int companionDefId, int companionLevel, int fightId, int fromFightId, int fromPlayerId)
			: base(FightEventData.Types.EventType.CompanionReceived, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.companionDefId = companionDefId;
			this.companionLevel = companionLevel;
			this.fightId = fightId;
			this.fromFightId = fromFightId;
			this.fromPlayerId = fromPlayerId;
		}

		public CompanionReceivedEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.CompanionReceived, proto)
		{
			concernedEntity = proto.Int1;
			companionDefId = proto.Int2;
			companionLevel = proto.Int3;
			fightId = proto.Int4;
			fromFightId = proto.Int5;
			fromPlayerId = proto.Int6;
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			if (fightStatus != FightStatus.local)
			{
				yield break;
			}
			FightUIRework instance = FightUIRework.instance;
			if (!(null != instance))
			{
				yield break;
			}
			if (GameStatus.GetFightStatus(fromFightId).TryGetEntity(fromPlayerId, out PlayerStatus entityStatus))
			{
				if (RuntimeData.companionDefinitions.TryGetValue(companionDefId, out CompanionDefinition value))
				{
					PlayerStatus localPlayer = fightStatus.GetLocalPlayer();
					if (concernedEntity == localPlayer.id)
					{
						FightInfoMessage message = FightInfoMessage.ReceivedCompanion(MessageInfoRibbonGroup.MyID);
						instance.DrawInfoMessage(message, entityStatus.nickname, RuntimeData.FormattedText(value.i18nNameId));
					}
				}
				else
				{
					Log.Error(FightEventErrors.DefinitionNotFound<CompanionDefinition>(companionDefId), 37, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\CompanionReceivedEvent.cs");
				}
			}
			else
			{
				Log.Error(FightEventErrors.PlayerNotFound(fromPlayerId, fromFightId), 42, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\CompanionReceivedEvent.cs");
			}
		}
	}
}
