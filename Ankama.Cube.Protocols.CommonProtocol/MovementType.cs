using Google.Protobuf.Reflection;

namespace Ankama.Cube.Protocols.CommonProtocol
{
	public enum MovementType
	{
		[OriginalName("MOVEMENT_TYPE_NOT_USED")]
		NotUsed,
		[OriginalName("WALK")]
		Walk,
		[OriginalName("RUN")]
		Run,
		[OriginalName("TELEPORT")]
		Teleport,
		[OriginalName("SLIDE")]
		Slide,
		[OriginalName("ATTACK")]
		Attack
	}
}
