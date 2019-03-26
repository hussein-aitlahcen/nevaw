using Ankama.Cube.Network.Spin2.Layers;
using System;
using UnityEngine;

namespace Ankama.Cube.Network.Spin2
{
	public class SpinTransportLayer : TransformLayer<SpinProtocol.Message, byte[]>
	{
		public SpinTransportLayer(INetworkLayer<byte[]> child)
			: base(child)
		{
		}

		public override bool Write(SpinProtocol.Message input)
		{
			return base.child.Write(input.Serialize());
		}

		protected override void OnDataReceived(byte[] data)
		{
			switch (data[0])
			{
			case 0:
			{
				int num3 = data.Length - 1;
				byte[] array3 = new byte[num3];
				Buffer.BlockCopy(data, 1, array3, 0, num3);
				base.OnData(new SpinProtocol.RawApplicationMessage(array3));
				break;
			}
			case 1:
			{
				int num2 = data.Length - 1;
				byte[] array2 = new byte[num2];
				Buffer.BlockCopy(data, 1, array2, 0, num2);
				base.OnData(new SpinProtocol.PingMessage(array2));
				break;
			}
			case 2:
			{
				int num = data.Length - 1;
				byte[] array = new byte[num];
				Buffer.BlockCopy(data, 1, array, 0, num);
				base.OnData(new SpinProtocol.PongMessage(array));
				break;
			}
			case 3:
				base.OnData(SpinProtocol.HeartbeatMessage.instance);
				break;
			default:
				Debug.LogWarning((object)$"Unknownspin message type: {data[0]}");
				break;
			}
		}
	}
}
