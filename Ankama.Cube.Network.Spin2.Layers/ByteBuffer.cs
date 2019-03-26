using System;
using System.IO;

namespace Ankama.Cube.Network.Spin2.Layers
{
	public class ByteBuffer
	{
		private readonly MemoryStream m_stream;

		private readonly bool m_bigIndian;

		private int m_readPosition;

		public bool isEmpty => m_stream.Length <= m_readPosition;

		public int remaining => (int)m_stream.Length - m_readPosition;

		public ByteBuffer(int capacity = 8092, bool bigIndian = true)
		{
			m_stream = new MemoryStream(capacity);
			m_bigIndian = bigIndian;
		}

		private void PrepareForRead()
		{
			m_stream.Position = m_readPosition;
		}

		private void PrepareForWrite()
		{
			m_stream.Position = m_stream.Length;
		}

		public void Write(byte[] buffer)
		{
			PrepareForWrite();
			Write(buffer, 0, buffer.Length);
		}

		public void Write(byte[] buffer, int offset, int dataLength)
		{
			PrepareForWrite();
			m_stream.Position = m_stream.Length;
			m_stream.Write(buffer, offset, dataLength);
		}

		public void WriteInt(int data, int bytesCount)
		{
			PrepareForWrite();
			m_stream.Position = m_stream.Length;
			m_stream.WriteInt(data, bytesCount, m_bigIndian);
		}

		public int ReadInt(int bytesCount)
		{
			int result = PeekInt(bytesCount);
			m_readPosition += bytesCount;
			return result;
		}

		public int PeekInt(int bytesCount)
		{
			PrepareForRead();
			if (m_readPosition + bytesCount > m_stream.Length)
			{
				throw new IndexOutOfRangeException($"Trying to read too many Bytes. end({m_readPosition}+{bytesCount}) > Length({m_stream.Length})");
			}
			return m_stream.ReadInt(bytesCount, m_bigIndian);
		}

		public byte[] ReadAll()
		{
			return ReadBytes(remaining);
		}

		public byte[] ReadBytes(int length)
		{
			byte[] result = PeekBytes(length);
			m_readPosition += length;
			return result;
		}

		public byte[] PeekBytes(int length)
		{
			PrepareForRead();
			if (m_readPosition + length > m_stream.Length)
			{
				throw new IndexOutOfRangeException($"Trying to read too many Bytes. end({m_readPosition}+{length}) > Length({m_stream.Length})");
			}
			byte[] array = new byte[length];
			m_stream.Position = m_readPosition;
			m_stream.Read(array, 0, length);
			return array;
		}

		public void Skip(int bytesCount)
		{
			int num = m_readPosition + bytesCount;
			if (num > m_stream.Length)
			{
				throw new IndexOutOfRangeException($"Can't skip {bytesCount} bytes: readPosition is {m_readPosition} and length is {m_stream.Length}");
			}
			m_readPosition = num;
		}

		public void Clear()
		{
			m_readPosition = 0;
			m_stream.SetLength(0L);
		}

		public void CompactIfNeeded()
		{
			if (m_readPosition != 0)
			{
				int remaining = this.remaining;
				if (remaining == 0)
				{
					Clear();
				}
				int capacity = m_stream.Capacity;
				if (m_readPosition > capacity / 4)
				{
					byte[] buffer = m_stream.GetBuffer();
					Buffer.BlockCopy(buffer, m_readPosition, buffer, 0, remaining);
					m_readPosition = 0;
					m_stream.SetLength(remaining);
				}
			}
		}

		public void Compact()
		{
			if (m_readPosition != 0)
			{
				int remaining = this.remaining;
				if (remaining == 0)
				{
					Clear();
				}
				byte[] buffer = m_stream.GetBuffer();
				Buffer.BlockCopy(buffer, m_readPosition, buffer, 0, remaining);
				m_readPosition = 0;
				m_stream.SetLength(remaining);
			}
		}
	}
}
