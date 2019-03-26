using System;
using System.IO;

namespace Ankama.Cube.Network.Spin2.Layers
{
	public static class StreamExtensions
	{
		public static void WriteInt(this Stream stream, int data, int bytesCount, bool bigIndian)
		{
			if (bytesCount <= 0 || bytesCount > 4)
			{
				throw new ArgumentOutOfRangeException("bytesCount", "should be in [1, 4]");
			}
			if (data > 1L << 8 * bytesCount - 1)
			{
				throw new ArgumentOutOfRangeException("data", "can't be encoded with " + bytesCount + " Bytes");
			}
			if (bigIndian)
			{
				for (int num = bytesCount - 1; num >= 0; num--)
				{
					int num2 = (data >> num * 8) & 0xFF;
					stream.WriteByte((byte)num2);
				}
			}
			else
			{
				for (int i = 0; i < bytesCount; i++)
				{
					int num3 = (data >> i * 8) & 0xFF;
					stream.WriteByte((byte)num3);
				}
			}
		}

		public static int ReadInt(this Stream stream, int bytesCount, bool bigIndian)
		{
			int num = 0;
			if (bigIndian)
			{
				for (int num2 = bytesCount - 1; num2 >= 0; num2--)
				{
					num += stream.ReadByte() << num2 * 8;
				}
			}
			else
			{
				for (int i = 0; i < bytesCount; i++)
				{
					num += (stream.ReadByte() & 0xFF) << i * 8;
				}
			}
			return num;
		}
	}
}
