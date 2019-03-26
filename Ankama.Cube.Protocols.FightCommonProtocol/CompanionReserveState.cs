using Google.Protobuf.Reflection;

namespace Ankama.Cube.Protocols.FightCommonProtocol
{
	public enum CompanionReserveState
	{
		[OriginalName("IDLE")]
		Idle,
		[OriginalName("IN_FIGHT")]
		InFight,
		[OriginalName("GIVEN")]
		Given,
		[OriginalName("DEAD")]
		Dead
	}
}
