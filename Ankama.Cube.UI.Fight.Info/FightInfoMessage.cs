using Ankama.Cube.Fight;
using Ankama.Cube.Protocols.FightCommonProtocol;
using System;

namespace Ankama.Cube.UI.Fight.Info
{
	public struct FightInfoMessage
	{
		public readonly int id;

		public readonly MessageInfoRibbonGroup ribbonGroup;

		public readonly MessageInfoIconType iconType;

		public readonly int countValue;

		public FightInfoMessage(int id, MessageInfoRibbonGroup ribbonGroup, MessageInfoIconType iconType, int countValue = 0)
		{
			this.id = id;
			this.ribbonGroup = ribbonGroup;
			this.iconType = iconType;
			this.countValue = countValue;
		}

		public static FightInfoMessage HeroLowLife(MessageInfoRibbonGroup messageGroup)
		{
			return new FightInfoMessage(78839, messageGroup, MessageInfoIconType.HeroLowLife);
		}

		public static FightInfoMessage ReceivedCompanion(MessageInfoRibbonGroup messageGroup)
		{
			return new FightInfoMessage(16244, messageGroup, MessageInfoIconType.CompanionReceived);
		}

		public static FightInfoMessage HeroDeath(MessageInfoRibbonGroup messageGroup)
		{
			return new FightInfoMessage(12120, messageGroup, MessageInfoIconType.HeroDeath);
		}

		public static FightInfoMessage BossPointEarn(MessageInfoRibbonGroup messageGroup, int value)
		{
			return new FightInfoMessage(45919, messageGroup, MessageInfoIconType.CompanionKilled, value);
		}

		public static FightInfoMessage Score(FightScore score, TeamsScoreModificationReason reason)
		{
			int num = 0;
			MessageInfoRibbonGroup messageInfoRibbonGroup = MessageInfoRibbonGroup.DefaultID;
			if (score.myTeamScore.changed)
			{
				num = score.myTeamScore.delta;
				messageInfoRibbonGroup = MessageInfoRibbonGroup.MyID;
			}
			else if (score.opponentTeamScore.changed)
			{
				num = score.opponentTeamScore.delta;
				messageInfoRibbonGroup = MessageInfoRibbonGroup.OtherID;
			}
			switch (reason)
			{
			case TeamsScoreModificationReason.FirstVictory:
				return new FightInfoMessage(30898, messageInfoRibbonGroup, MessageInfoIconType.FirstWin, num);
			case TeamsScoreModificationReason.HeroDeath:
				return new FightInfoMessage(583, messageInfoRibbonGroup, MessageInfoIconType.Win, num);
			case TeamsScoreModificationReason.CompanionDeath:
				return new FightInfoMessage(72725, messageInfoRibbonGroup, MessageInfoIconType.CompanionKilled, num);
			default:
				throw new ArgumentOutOfRangeException("reason", reason, "Unhandled MessageInfoIconType");
			}
		}
	}
}
