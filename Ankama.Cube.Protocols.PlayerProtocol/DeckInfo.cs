using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Ankama.Cube.Protocols.PlayerProtocol
{
	public sealed class DeckInfo : IMessage<DeckInfo>, IMessage, IEquatable<DeckInfo>, IDeepCloneable<DeckInfo>, ICustomDiagnosticMessage
	{
		private static readonly MessageParser<DeckInfo> _parser = new MessageParser<DeckInfo>((Func<DeckInfo>)(() => new DeckInfo()));

		private UnknownFieldSet _unknownFields;

		public const int IdFieldNumber = 1;

		private static readonly FieldCodec<int?> _single_id_codec = FieldCodec.ForStructWrapper<int>(10u);

		private int? id_;

		public const int NameFieldNumber = 2;

		private string name_ = "";

		public const int GodFieldNumber = 3;

		private int god_;

		public const int WeaponFieldNumber = 4;

		private int weapon_;

		public const int CompanionsFieldNumber = 5;

		private static readonly FieldCodec<int> _repeated_companions_codec = FieldCodec.ForInt32(42u);

		private readonly RepeatedField<int> companions_ = new RepeatedField<int>();

		public const int SpellsFieldNumber = 6;

		private static readonly FieldCodec<int> _repeated_spells_codec = FieldCodec.ForInt32(50u);

		private readonly RepeatedField<int> spells_ = new RepeatedField<int>();

		public const int SummoningsFieldNumber = 7;

		private static readonly FieldCodec<int> _repeated_summonings_codec = FieldCodec.ForInt32(58u);

		private readonly RepeatedField<int> summonings_ = new RepeatedField<int>();

		[DebuggerNonUserCode]
		public static MessageParser<DeckInfo> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => PlayerProtocolReflection.Descriptor.get_MessageTypes()[1];

		[DebuggerNonUserCode]
		MessageDescriptor Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public int? Id
		{
			get
			{
				return id_;
			}
			set
			{
				id_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string Name
		{
			get
			{
				return name_;
			}
			set
			{
				name_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
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
		public int Weapon
		{
			get
			{
				return weapon_;
			}
			set
			{
				weapon_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<int> Companions => companions_;

		[DebuggerNonUserCode]
		public RepeatedField<int> Spells => spells_;

		[DebuggerNonUserCode]
		public RepeatedField<int> Summonings => summonings_;

		[DebuggerNonUserCode]
		public DeckInfo()
		{
		}

		[DebuggerNonUserCode]
		public DeckInfo(DeckInfo other)
			: this()
		{
			Id = other.Id;
			name_ = other.name_;
			god_ = other.god_;
			weapon_ = other.weapon_;
			companions_ = other.companions_.Clone();
			spells_ = other.spells_.Clone();
			summonings_ = other.summonings_.Clone();
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public DeckInfo Clone()
		{
			return new DeckInfo(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as DeckInfo);
		}

		[DebuggerNonUserCode]
		public bool Equals(DeckInfo other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (Id != other.Id)
			{
				return false;
			}
			if (Name != other.Name)
			{
				return false;
			}
			if (God != other.God)
			{
				return false;
			}
			if (Weapon != other.Weapon)
			{
				return false;
			}
			if (!companions_.Equals(other.companions_))
			{
				return false;
			}
			if (!spells_.Equals(other.spells_))
			{
				return false;
			}
			if (!summonings_.Equals(other.summonings_))
			{
				return false;
			}
			return object.Equals(_unknownFields, other._unknownFields);
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (id_.HasValue)
			{
				num ^= Id.GetHashCode();
			}
			if (Name.Length != 0)
			{
				num ^= Name.GetHashCode();
			}
			if (God != 0)
			{
				num ^= God.GetHashCode();
			}
			if (Weapon != 0)
			{
				num ^= Weapon.GetHashCode();
			}
			num ^= ((object)companions_).GetHashCode();
			num ^= ((object)spells_).GetHashCode();
			num ^= ((object)summonings_).GetHashCode();
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
			if (id_.HasValue)
			{
				_single_id_codec.WriteTagAndValue(output, Id);
			}
			if (Name.Length != 0)
			{
				output.WriteRawTag((byte)18);
				output.WriteString(Name);
			}
			if (God != 0)
			{
				output.WriteRawTag((byte)24);
				output.WriteInt32(God);
			}
			if (Weapon != 0)
			{
				output.WriteRawTag((byte)32);
				output.WriteInt32(Weapon);
			}
			companions_.WriteTo(output, _repeated_companions_codec);
			spells_.WriteTo(output, _repeated_spells_codec);
			summonings_.WriteTo(output, _repeated_summonings_codec);
			if (_unknownFields != null)
			{
				_unknownFields.WriteTo(output);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (id_.HasValue)
			{
				num += _single_id_codec.CalculateSizeWithTag(Id);
			}
			if (Name.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(Name);
			}
			if (God != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(God);
			}
			if (Weapon != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Weapon);
			}
			num += companions_.CalculateSize(_repeated_companions_codec);
			num += spells_.CalculateSize(_repeated_spells_codec);
			num += summonings_.CalculateSize(_repeated_summonings_codec);
			if (_unknownFields != null)
			{
				num += _unknownFields.CalculateSize();
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(DeckInfo other)
		{
			if (other != null)
			{
				if (other.id_.HasValue && (!id_.HasValue || other.Id != 0))
				{
					Id = other.Id;
				}
				if (other.Name.Length != 0)
				{
					Name = other.Name;
				}
				if (other.God != 0)
				{
					God = other.God;
				}
				if (other.Weapon != 0)
				{
					Weapon = other.Weapon;
				}
				companions_.Add((IEnumerable<int>)other.companions_);
				spells_.Add((IEnumerable<int>)other.spells_);
				summonings_.Add((IEnumerable<int>)other.summonings_);
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
				{
					int? num2 = _single_id_codec.Read(input);
					if (!id_.HasValue || num2 != 0)
					{
						Id = num2;
					}
					break;
				}
				case 18u:
					Name = input.ReadString();
					break;
				case 24u:
					God = input.ReadInt32();
					break;
				case 32u:
					Weapon = input.ReadInt32();
					break;
				case 40u:
				case 42u:
					companions_.AddEntriesFrom(input, _repeated_companions_codec);
					break;
				case 48u:
				case 50u:
					spells_.AddEntriesFrom(input, _repeated_spells_codec);
					break;
				case 56u:
				case 58u:
					summonings_.AddEntriesFrom(input, _repeated_summonings_codec);
					break;
				}
			}
		}

		public string ToDiagnosticString()
		{
			return "DeckInfo";
		}
	}
}
