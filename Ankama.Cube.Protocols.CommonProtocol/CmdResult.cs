using Google.Protobuf.Reflection;

namespace Ankama.Cube.Protocols.CommonProtocol
{
	public enum CmdResult
	{
		[OriginalName("Failed")]
		Failed,
		[OriginalName("Success")]
		Success
	}
}
