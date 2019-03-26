using Google.Protobuf.Reflection;

namespace Ankama.Cube.Protocols.CommonProtocol
{
	public enum DamageReductionType
	{
		[OriginalName("UNKNOWN")]
		Unknown,
		[OriginalName("SHIELD")]
		Shield,
		[OriginalName("COUNTER")]
		Counter,
		[OriginalName("PROTECTOR")]
		Protector,
		[OriginalName("REFLECTION")]
		Reflection,
		[OriginalName("DAMAGE_PROOF")]
		DamageProof,
		[OriginalName("RESISTANCE")]
		Resistance,
		[OriginalName("PETRIFICATION")]
		Petrification
	}
}
