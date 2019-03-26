using Google.Protobuf;
using Google.Protobuf.Reflection;
using System;
using System.Diagnostics;

namespace Ankama.Cube.Protocols.PlayerProtocol
{
	public sealed class PlayerPublicInfo : IMessage<PlayerPublicInfo>, IMessage, IEquatable<PlayerPublicInfo>, IDeepCloneable<PlayerPublicInfo>, ICustomDiagnosticMessage
	{
		private static readonly MessageParser<PlayerPublicInfo> _parser = new MessageParser<PlayerPublicInfo>((Func<PlayerPublicInfo>)(() => new PlayerPublicInfo()));

		private UnknownFieldSet _unknownFields;

		public const int NicknameFieldNumber = 1;

		private string nickname_ = "";

		public const int GodFieldNumber = 2;

		private int god_;

		public const int WeaponIdFieldNumber = 3;

		private int weaponId_;

		public const int SkinFieldNumber = 4;

		private int skin_;

		[DebuggerNonUserCode]
		public static MessageParser<PlayerPublicInfo> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => PlayerProtocolReflection.Descriptor.get_MessageTypes()[0];

		[DebuggerNonUserCode]
		MessageDescriptor Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public string Nickname
		{
			get
			{
				return nickname_;
			}
			set
			{
				nickname_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public int God
		{
			get
			{
				return god_;
			}
			set
			{
				god_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int WeaponId
		{
			get
			{
				return weaponId_;
			}
			set
			{
				weaponId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Skin
		{
			get
			{
				return skin_;
			}
			set
			{
				skin_ = value;
			}
		}

		[DebuggerNonUserCode]
		public PlayerPublicInfo()
		{
		}

		[DebuggerNonUserCode]
		public PlayerPublicInfo(PlayerPublicInfo other)
			: this()
		{
			nickname_ = other.nickname_;
			god_ = other.god_;
			weaponId_ = other.weaponId_;
			skin_ = other.skin_;
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public PlayerPublicInfo Clone()
		{
			return new PlayerPublicInfo(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as PlayerPublicInfo);
		}

		[DebuggerNonUserCode]
		public bool Equals(PlayerPublicInfo other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (Nickname != other.Nickname)
			{
				return false;
			}
			if (God != other.God)
			{
				return false;
			}
			if (WeaponId != other.WeaponId)
			{
				return false;
			}
			if (Skin != other.Skin)
			{
				return false;
			}
			return object.Equals(_unknownFields, other._unknownFields);
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (Nickname.Length != 0)
			{
				num ^= Nickname.GetHashCode();
			}
			if (God != 0)
			{
				num ^= God.GetHashCode();
			}
			if (WeaponId != 0)
			{
				num ^= WeaponId.GetHashCode();
			}
			if (Skin != 0)
			{
				num ^= Skin.GetHashCode();
			}
			if (_unknownFields != null)
			{
				num ^= ((object)_unknownFields).GetHashCode();
			}
			return num;
		}

		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (Nickname.Length != 0)
			{
				output.WriteRawTag((byte)10);
				output.WriteString(Nickname);
			}
			if (God != 0)
			{
				output.WriteRawTag((byte)16);
				output.WriteInt32(God);
			}
			if (WeaponId != 0)
			{
				output.WriteRawTag((byte)24);
				output.WriteInt32(WeaponId);
			}
			if (Skin != 0)
			{
				output.WriteRawTag((byte)32);
				output.WriteInt32(Skin);
			}
			if (_unknownFields != null)
			{
				_unknownFields.WriteTo(output);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (Nickname.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(Nickname);
			}
			if (God != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(God);
			}
			if (WeaponId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(WeaponId);
			}
			if (Skin != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Skin);
			}
			if (_unknownFields != null)
			{
				num += _unknownFields.CalculateSize();
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(PlayerPublicInfo other)
		{
			if (other != null)
			{
				if (other.Nickname.Length != 0)
				{
					Nickname = other.Nickname;
				}
				if (other.God != 0)
				{
					God = other.God;
				}
				if (other.WeaponId != 0)
				{
					WeaponId = other.WeaponId;
				}
				if (other.Skin != 0)
				{
					Skin = other.Skin;
				}
				_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
			}
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0)
			{
				switch (num)
				{
				default:
					_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
					break;
				case 10u:
					Nickname = input.ReadString();
					break;
				case 16u:
					God = input.ReadInt32();
					break;
				case 24u:
					WeaponId = input.ReadInt32();
					break;
				case 32u:
					Skin = input.ReadInt32();
					break;
				}
			}
		}

		public string ToDiagnosticString()
		{
			return "PlayerPublicInfo";
		}
	}
}
