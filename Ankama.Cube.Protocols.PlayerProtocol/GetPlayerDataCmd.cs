using Google.Protobuf;
using Google.Protobuf.Reflection;
using System;
using System.Diagnostics;

namespace Ankama.Cube.Protocols.PlayerProtocol
{
	public sealed class GetPlayerDataCmd : IMessage<GetPlayerDataCmd>, IMessage, IEquatable<GetPlayerDataCmd>, IDeepCloneable<GetPlayerDataCmd>, ICustomDiagnosticMessage
	{
		private static readonly MessageParser<GetPlayerDataCmd> _parser = new MessageParser<GetPlayerDataCmd>((Func<GetPlayerDataCmd>)(() => new GetPlayerDataCmd()));

		private UnknownFieldSet _unknownFields;

		public const int AccountDataFieldNumber = 1;

		private bool accountData_;

		public const int OccupationFieldNumber = 2;

		private bool occupation_;

		public const int HeroDataFieldNumber = 3;

		private bool heroData_;

		public const int DecksFieldNumber = 4;

		private bool decks_;

		public const int CompanionsFieldNumber = 5;

		private bool companions_;

		public const int WeaponsFieldNumber = 6;

		private bool weapons_;

		[DebuggerNonUserCode]
		public static MessageParser<GetPlayerDataCmd> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => PlayerProtocolReflection.Descriptor.get_MessageTypes()[3];

		[DebuggerNonUserCode]
		MessageDescriptor Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public bool AccountData
		{
			get
			{
				return accountData_;
			}
			set
			{
				accountData_ = value;
			}
		}

		[DebuggerNonUserCode]
		public bool Occupation
		{
			get
			{
				return occupation_;
			}
			set
			{
				occupation_ = value;
			}
		}

		[DebuggerNonUserCode]
		public bool HeroData
		{
			get
			{
				return heroData_;
			}
			set
			{
				heroData_ = value;
			}
		}

		[DebuggerNonUserCode]
		public bool Decks
		{
			get
			{
				return decks_;
			}
			set
			{
				decks_ = value;
			}
		}

		[DebuggerNonUserCode]
		public bool Companions
		{
			get
			{
				return companions_;
			}
			set
			{
				companions_ = value;
			}
		}

		[DebuggerNonUserCode]
		public bool Weapons
		{
			get
			{
				return weapons_;
			}
			set
			{
				weapons_ = value;
			}
		}

		[DebuggerNonUserCode]
		public GetPlayerDataCmd()
		{
		}

		[DebuggerNonUserCode]
		public GetPlayerDataCmd(GetPlayerDataCmd other)
			: this()
		{
			accountData_ = other.accountData_;
			occupation_ = other.occupation_;
			heroData_ = other.heroData_;
			decks_ = other.decks_;
			companions_ = other.companions_;
			weapons_ = other.weapons_;
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public GetPlayerDataCmd Clone()
		{
			return new GetPlayerDataCmd(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as GetPlayerDataCmd);
		}

		[DebuggerNonUserCode]
		public bool Equals(GetPlayerDataCmd other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (AccountData != other.AccountData)
			{
				return false;
			}
			if (Occupation != other.Occupation)
			{
				return false;
			}
			if (HeroData != other.HeroData)
			{
				return false;
			}
			if (Decks != other.Decks)
			{
				return false;
			}
			if (Companions != other.Companions)
			{
				return false;
			}
			if (Weapons != other.Weapons)
			{
				return false;
			}
			return object.Equals(_unknownFields, other._unknownFields);
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (AccountData)
			{
				num ^= AccountData.GetHashCode();
			}
			if (Occupation)
			{
				num ^= Occupation.GetHashCode();
			}
			if (HeroData)
			{
				num ^= HeroData.GetHashCode();
			}
			if (Decks)
			{
				num ^= Decks.GetHashCode();
			}
			if (Companions)
			{
				num ^= Companions.GetHashCode();
			}
			if (Weapons)
			{
				num ^= Weapons.GetHashCode();
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
			if (AccountData)
			{
				output.WriteRawTag((byte)8);
				output.WriteBool(AccountData);
			}
			if (Occupation)
			{
				output.WriteRawTag((byte)16);
				output.WriteBool(Occupation);
			}
			if (HeroData)
			{
				output.WriteRawTag((byte)24);
				output.WriteBool(HeroData);
			}
			if (Decks)
			{
				output.WriteRawTag((byte)32);
				output.WriteBool(Decks);
			}
			if (Companions)
			{
				output.WriteRawTag((byte)40);
				output.WriteBool(Companions);
			}
			if (Weapons)
			{
				output.WriteRawTag((byte)48);
				output.WriteBool(Weapons);
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
			if (AccountData)
			{
				num += 2;
			}
			if (Occupation)
			{
				num += 2;
			}
			if (HeroData)
			{
				num += 2;
			}
			if (Decks)
			{
				num += 2;
			}
			if (Companions)
			{
				num += 2;
			}
			if (Weapons)
			{
				num += 2;
			}
			if (_unknownFields != null)
			{
				num += _unknownFields.CalculateSize();
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(GetPlayerDataCmd other)
		{
			if (other != null)
			{
				if (other.AccountData)
				{
					AccountData = other.AccountData;
				}
				if (other.Occupation)
				{
					Occupation = other.Occupation;
				}
				if (other.HeroData)
				{
					HeroData = other.HeroData;
				}
				if (other.Decks)
				{
					Decks = other.Decks;
				}
				if (other.Companions)
				{
					Companions = other.Companions;
				}
				if (other.Weapons)
				{
					Weapons = other.Weapons;
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
					AccountData = input.ReadBool();
					break;
				case 16u:
					Occupation = input.ReadBool();
					break;
				case 24u:
					HeroData = input.ReadBool();
					break;
				case 32u:
					Decks = input.ReadBool();
					break;
				case 40u:
					Companions = input.ReadBool();
					break;
				case 48u:
					Weapons = input.ReadBool();
					break;
				}
			}
		}

		public string ToDiagnosticString()
		{
			return "GetPlayerDataCmd";
		}
	}
}
