using Ankama.Cube.Fight.Entities;
using Ankama.Cube.UI.Fight;
using Ankama.Cube.UI.Fight.Info;
using Ankama.Utilities;
using System.Collections;

namespace Ankama.Cube.Fight.Events
{
	public class BossLifeModificationEvent : FightEvent
	{
		public int valueBefore
		{
			get;
			private set;
		}

		public int valueAfter
		{
			get;
			private set;
		}

		public int sourceFightId
		{
			get;
			private set;
		}

		public int sourcePlayerId
		{
			get;
			private set;
		}

		public BossLifeModificationEvent(int eventId, int? parentEventId, int valueBefore, int valueAfter, int sourceFightId, int sourcePlayerId)
			: base(FightEventData.Types.EventType.BossLifeModification, eventId, parentEventId)
		{
			this.valueBefore = valueBefore;
			this.valueAfter = valueAfter;
			this.sourceFightId = sourceFightId;
			this.sourcePlayerId = sourcePlayerId;
		}

		public BossLifeModificationEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.BossLifeModification, proto)
		{
			valueBefore = proto.Int1;
			valueAfter = proto.Int2;
			sourceFightId = proto.Int3;
			sourcePlayerId = proto.Int4;
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			if (GameStatus.GetFightStatus(sourceFightId).TryGetEntity(sourcePlayerId, out PlayerStatus entityStatus))
			{
				FightUIRework instance = FightUIRework.instance;
				if (null != instance)
				{
					FightInfoMessage message = FightInfoMessage.BossPointEarn(MessageInfoRibbonGroup.MyID, valueBefore - valueAfter);
					instance.DrawScore(message, entityStatus.nickname);
				}
			}
			else
			{
				Log.Error(FightEventErrors.PlayerNotFound(sourcePlayerId, sourceFightId), 29, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\BossLifeModificationEvent.cs");
			}
			yield break;
		}
	}
}
