using Ankama.Cube.Network.Spin2;
using Ankama.Cube.Network.Spin2.Layers;
using Ankama.Cube.TEMPFastEnterMatch.Auth.ConnectionMethod;
using Google.Protobuf;

namespace Ankama.Cube.Network
{
	public class CubeServerConnection : SpinConnection<IMessage>
	{
		public const int CubeMaximumMessageSize = 65536;

		private readonly TcpConnectionLayer m_tcpConnectionLayer;

		public CubeServerConnection()
			: this(new TcpConnectionLayer())
		{
		}

		private CubeServerConnection(TcpConnectionLayer tcpConnectionLayer)
			: base((ISpinCredentialsProvider)CredentialProvider.gameCredentialProvider, (INetworkLayer<byte[]>)tcpConnectionLayer, (ApplicationCodec<IMessage>)CubeApplicationCodec.instance, 65536)
		{
			m_tcpConnectionLayer = tcpConnectionLayer;
		}

		public void CloseAbruptly()
		{
			m_tcpConnectionLayer.CloseAbruptly();
		}
	}
}
