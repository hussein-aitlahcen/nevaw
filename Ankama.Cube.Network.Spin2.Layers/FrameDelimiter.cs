using System;
using UnityEngine;

namespace Ankama.Cube.Network.Spin2.Layers
{
	public class FrameDelimiter : TransformLayer<byte[], byte[]>
	{
		private readonly ByteBuffer m_receiveBuffer;

		private readonly ByteBuffer m_sendBuffer;

		private readonly int m_headerSize;

		private readonly int m_maximumMessageSize;

		private readonly bool m_includeHeaderSize;

		public FrameDelimiter(INetworkLayer<byte[]> child, int maximumMessageSize, int headerSize, bool includeHeaderSize, int receiveBufferSize = 8092, bool bigIndian = true)
			: base(child)
		{
			int num;
			switch (headerSize)
			{
			default:
				throw new ArgumentOutOfRangeException("headerSize", $"HeaderSize should be between 1 and 4. Found {headerSize}");
			case 1:
			case 2:
			case 3:
				num = 1 << 8 * headerSize;
				break;
			case 4:
				num = int.MaxValue;
				break;
			}
			int num2 = num;
			if (maximumMessageSize >= num2)
			{
				throw new ArgumentOutOfRangeException("maximumMessageSize", string.Format("cannot encode {0} with a headerSize of {1}. {2} should be < {3}", "maximumMessageSize", headerSize, "maximumMessageSize", num2));
			}
			m_maximumMessageSize = maximumMessageSize;
			m_includeHeaderSize = includeHeaderSize;
			m_headerSize = headerSize;
			m_receiveBuffer = new ByteBuffer(receiveBufferSize, bigIndian);
			m_sendBuffer = new ByteBuffer(1024, bigIndian);
		}

		public override bool Write(byte[] input)
		{
			int num = input.Length;
			if (num == 0)
			{
				Debug.LogWarning((object)"FrameDelimiter: unable to write an empty byte array");
				return false;
			}
			ByteBuffer sendBuffer = m_sendBuffer;
			sendBuffer.Clear();
			sendBuffer.WriteInt(m_includeHeaderSize ? (num + m_headerSize) : num, m_headerSize);
			sendBuffer.Write(input);
			return base.child.Write(sendBuffer.ReadAll());
		}

		protected override void OnDataReceived(byte[] data)
		{
			ByteBuffer receiveBuffer = m_receiveBuffer;
			receiveBuffer.Write(data);
			int num;
			while (true)
			{
				if (receiveBuffer.remaining >= m_headerSize)
				{
					num = receiveBuffer.PeekInt(m_headerSize);
					if (num < 0 || num > m_maximumMessageSize)
					{
						break;
					}
					int num2 = num + ((!m_includeHeaderSize) ? m_headerSize : 0);
					if (receiveBuffer.remaining >= num2)
					{
						receiveBuffer.Skip(m_headerSize);
						int length = num2 - m_headerSize;
						base.OnData(receiveBuffer.ReadBytes(length));
						receiveBuffer.CompactIfNeeded();
						continue;
					}
					return;
				}
				return;
			}
			throw new ArgumentException($"Frame too large received: frame size is {num} but maximumMessageSize is {m_maximumMessageSize}");
		}
	}
}
