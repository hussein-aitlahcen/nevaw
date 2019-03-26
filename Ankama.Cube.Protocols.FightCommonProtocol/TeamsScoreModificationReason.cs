using Google.Protobuf.Reflection;

namespace Ankama.Cube.Protocols.FightCommonProtocol
{
	public enum TeamsScoreModificationReason
	{
		[OriginalName("FIRST_VICTORY")]
		FirstVictory,
		[OriginalName("HERO_DEATH")]
		HeroDeath,
		[OriginalName("COMPANION_DEATH")]
		CompanionDeath,
		[OriginalName("HERO_LIFE_MODIFIED")]
		HeroLifeModified
	}
}
