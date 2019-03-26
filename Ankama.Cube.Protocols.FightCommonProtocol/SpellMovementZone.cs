using Google.Protobuf.Reflection;

namespace Ankama.Cube.Protocols.FightCommonProtocol
{
	public enum SpellMovementZone
	{
		[OriginalName("NOWHERE")]
		Nowhere,
		[OriginalName("HAND")]
		Hand,
		[OriginalName("DECK")]
		Deck
	}
}
