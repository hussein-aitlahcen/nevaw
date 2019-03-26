using Google.Protobuf.Reflection;

namespace Ankama.Cube.Protocols.CommonProtocol
{
	public enum FightResult
	{
		[OriginalName("Draw")]
		Draw,
		[OriginalName("Victory")]
		Victory,
		[OriginalName("Defeat")]
		Defeat
	}
}
