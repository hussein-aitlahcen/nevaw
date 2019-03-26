using Google.Protobuf;
using Google.Protobuf.Reflection;
using System;
using System.Diagnostics;

namespace Ankama.Cube.Protocols.PlayerProtocol
{
	public sealed class HeroInfo : IMessage<HeroInfo>, IMessage, IEquatable<HeroInfo>, IDeepCloneable<HeroInfo>, ICustomDiagnosticMessage
	{
		private static readonly MessageParser<HeroInfo> _parser = new MessageParser<HeroInfo>((Func<HeroInfo>)(() => new HeroInfo()));

		private UnknownFieldSet _unknownFields;

		public const int GodFieldNumber = 1;

		private int god_;

		public const int GenderFieldNumber = 2;

		private int gender_;

		public const int WeaponIdFieldNumber = 3;

		private int weaponId_;

		public const int SkinFieldNumber = 4;

		private int skin_;

		[DebuggerNonUserCode]
		public static MessageParser<HeroInfo> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => PlayerProtocolReflection.Descriptor.get_MessageTypes()[2];

		[DebuggerNonUserCode]
		MessageDescriptor Descriptor => Descriptor;

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
		public int Gender
		{
			get
			{
				return gender_;
			}
			set
			{
				gender_ = value;
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
		public HeroInfo()
		{
		}

		[DebuggerNonUserCode]
		public HeroInfo(HeroInfo other)
			: this()
		{
			god_ = other.god_;
			gender_ = other.gender_;
			weaponId_ = other.weaponId_;
			skin_ = other.skin_;
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public HeroInfo Clone()
		{
			return new HeroInfo(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as HeroInfo);
		}

		[DebuggerNonUserCode]
		public bool Equals(HeroInfo other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (God != other.God)
			{
				return false;
			}
			if (Gender != other.Gender)
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
			if (God != 0)
			{
				num ^= God.GetHashCode();
			}
			if (Gender != 0)
			{
				num ^= Gender.GetHashCode();
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
			if (God != 0)
			{
				output.WriteRawTag((byte)8);
				output.WriteInt32(God);
			}
			if (Gender != 0)
			{
				output.WriteRawTag((byte)16);
				output.WriteInt32(Gender);
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
			if (God != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(God);
			}
			if (Gender != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Gender);
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
		public void MergeFrom(HeroInfo other)
		{
			if (other != null)
			{
				if (other.God != 0)
				{
					God = other.God;
				}
				if (other.Gender != 0)
				{
					Gender = other.Gender;
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
				case 8u:
					God = input.ReadInt32();
					break;
				case 16u:
					Gender = input.ReadInt32();
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
			return "HeroInfo";
		}
	}
}
