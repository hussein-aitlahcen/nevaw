using Ankama.Cube.Network.Spin2.Layers;
using Ankama.Cube.Protocols;
using Google.Protobuf;
using System;
using System.IO;
using UnityEngine;

namespace Ankama.Cube.Network
{
	public class CubeApplicationCodec : ApplicationCodec<IMessage>
	{
		public static CubeApplicationCodec instance = new CubeApplicationCodec();

		public const bool BigIndian = true;

		private CubeApplicationCodec()
		{
		}

		public bool TrySerialize(IMessage m, out byte[] result)
		{
			Type type = ((object)m).GetType();
			if (ProtocolMap.identifiers.TryGetValue(type, out int value))
			{
				MemoryStream memoryStream = new MemoryStream();
				memoryStream.WriteInt(value, 4, bigIndian: true);
				byte[] array = MessageExtensions.ToByteArray(m);
				memoryStream.Write(array, 0, array.Length);
				result = memoryStream.ToArray();
				return true;
			}
			Debug.LogError((object)("Unable to serialize message " + type.Name + ": no known id for it"));
			result = new byte[0];
			return false;
		}

		public bool TryDeserialize(byte[] data, out IMessage result)
		{
			MemoryStream memoryStream = new MemoryStream(data);
			int num = memoryStream.ReadInt(4, bigIndian: true);
			if (ProtocolMap.parsers.TryGetValue(num, out MessageParser value))
			{
				result = value.ParseFrom((Stream)memoryStream);
				return true;
			}
			Debug.LogError((object)$"Unable to serialize message with ID {num}: no known parser for it");
			result = null;
			return false;
		}
	}
}
