using Newtonsoft.Json.Linq;
using System;
using System.Text;

namespace Ankama.Cube.Network.Spin2
{
	public static class SpinProtocol
	{
		public enum ConnectionErrors
		{
			NoneOrOtherOrUnknown,
			BadCredentials,
			InvalidAuthenticationInfo,
			SubscriptionRequired,
			AdminRightsRequired,
			AccountKnonwButBanned,
			AccountKnonwButBlocked,
			IpAddressRefused,
			BetaAccessRequired,
			ServerTimeout,
			ServerError,
			AccountsBackendError,
			NickNameRequired
		}

		public enum MessageType
		{
			Application,
			Ping,
			Pong,
			Heartbeat
		}

		public interface Message
		{
			MessageType messageType
			{
				get;
			}

			byte[] payload
			{
				get;
			}

			byte[] Serialize();
		}

		public abstract class MessageWithPayload : Message
		{
			public MessageType messageType
			{
				get;
			}

			public byte[] payload
			{
				get;
			}

			protected MessageWithPayload(MessageType messageType, byte[] payload)
			{
				this.messageType = messageType;
				this.payload = payload;
			}

			public byte[] Serialize()
			{
				int num = payload.Length;
				byte[] array = new byte[num + 1];
				array[0] = (byte)messageType;
				Buffer.BlockCopy(payload, 0, array, 1, num);
				return array;
			}
		}

		public class PingMessage : MessageWithPayload
		{
			public PingMessage(byte[] payload)
				: base(MessageType.Ping, payload)
			{
			}
		}

		public class PongMessage : MessageWithPayload
		{
			public PongMessage(byte[] payload)
				: base(MessageType.Pong, payload)
			{
			}
		}

		public class RawApplicationMessage : MessageWithPayload
		{
			public RawApplicationMessage(byte[] payload)
				: base(MessageType.Application, payload)
			{
			}
		}

		public class HeartbeatMessage : Message
		{
			public static readonly HeartbeatMessage instance = new HeartbeatMessage();

			private static readonly byte[] s_serialized = new byte[1]
			{
				3
			};

			public byte[] payload => new byte[0];

			public MessageType messageType => MessageType.Heartbeat;

			public byte[] Serialize()
			{
				return s_serialized;
			}
		}

		public static bool CheckAuthentication(byte[] jsonPayload, out ConnectionErrors optConnError)
		{
			JObject val = JObject.Parse(Encoding.UTF8.GetString(jsonPayload));
			if (val.Value<bool>((object)"success"))
			{
				optConnError = ConnectionErrors.NoneOrOtherOrUnknown;
				return true;
			}
			optConnError = (ConnectionErrors)val.Value<int>((object)"errCode");
			return false;
		}
	}
}
