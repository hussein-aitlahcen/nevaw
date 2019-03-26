using Ankama.Cube.TEMPFastEnterMatch.Network;
using Google.Protobuf;

namespace Ankama.Cube.Network
{
	public abstract class CubeMessageFrame : MessageFrame<IMessage>
	{
		protected CubeMessageFrame()
			: base(ConnectionHandler.instance.connection)
		{
		}
	}
}
