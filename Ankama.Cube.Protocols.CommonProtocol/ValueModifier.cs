using Google.Protobuf.Reflection;

namespace Ankama.Cube.Protocols.CommonProtocol
{
	public enum ValueModifier
	{
		[OriginalName("VALUE_MODIFIER_NOT_USED")]
		NotUsed,
		[OriginalName("ADD")]
		Add,
		[OriginalName("SET")]
		Set
	}
}
