using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Ankama.Cube.Protocols.PlayerProtocol
{
	public sealed class PlayerDataEvent : IMessage<PlayerDataEvent>, IMessage, IEquatable<PlayerDataEvent>, IDeepCloneable<PlayerDataEvent>, ICustomDiagnosticMessage
	{
		[DebuggerNonUserCode]
		public static class Types
		{
			public sealed class AccountData : IMessage<AccountData>, IMessage, IEquatable<AccountData>, IDeepCloneable<AccountData>, ICustomDiagnosticMessage
			{
				private static readonly MessageParser<AccountData> _parser = new MessageParser<AccountData>((Func<AccountData>)(() => new AccountData()));

				private UnknownFieldSet _unknownFields;

				public const int HashFieldNumber = 1;

				private int hash_;

				public const int NickNameFieldNumber = 2;

				private static readonly FieldCodec<string> _single_nickName_codec = FieldCodec.ForClassWrapper<string>(18u);

				private string nickName_;

				public const int AdminFieldNumber = 3;

				private bool admin_;

				public const int AccountTypeFieldNumber = 4;

				private string accountType_ = "";

				[DebuggerNonUserCode]
				public static MessageParser<AccountData> Parser => _parser;

				[DebuggerNonUserCode]
				public static MessageDescriptor Descriptor => PlayerDataEvent.Descriptor.get_NestedTypes()[0];

				[DebuggerNonUserCode]
				MessageDescriptor Descriptor => Descriptor;

				[DebuggerNonUserCode]
				public int Hash
				{
					get
					{
						return hash_;
					}
					set
					{
						hash_ = value;
					}
				}

				[DebuggerNonUserCode]
				public string NickName
				{
					get
					{
						return nickName_;
					}
					set
					{
						nickName_ = value;
					}
				}

				[DebuggerNonUserCode]
				public bool Admin
				{
					get
					{
						return admin_;
					}
					set
					{
						admin_ = value;
					}
				}

				[DebuggerNonUserCode]
				public string AccountType
				{
					get
					{
						return accountType_;
					}
					set
					{
						accountType_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
					}
				}

				[DebuggerNonUserCode]
				public AccountData()
				{
				}

				[DebuggerNonUserCode]
				public AccountData(AccountData other)
					: this()
				{
					hash_ = other.hash_;
					NickName = other.NickName;
					admin_ = other.admin_;
					accountType_ = other.accountType_;
					_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
				}

				[DebuggerNonUserCode]
				public AccountData Clone()
				{
					return new AccountData(this);
				}

				[DebuggerNonUserCode]
				public override bool Equals(object other)
				{
					return Equals(other as AccountData);
				}

				[DebuggerNonUserCode]
				public bool Equals(AccountData other)
				{
					if (other == null)
					{
						return false;
					}
					if (other == this)
					{
						return true;
					}
					if (Hash != other.Hash)
					{
						return false;
					}
					if (NickName != other.NickName)
					{
						return false;
					}
					if (Admin != other.Admin)
					{
						return false;
					}
					if (AccountType != other.AccountType)
					{
						return false;
					}
					return object.Equals(_unknownFields, other._unknownFields);
				}

				[DebuggerNonUserCode]
				public override int GetHashCode()
				{
					int num = 1;
					if (Hash != 0)
					{
						num ^= Hash.GetHashCode();
					}
					if (nickName_ != null)
					{
						num ^= NickName.GetHashCode();
					}
					if (Admin)
					{
						num ^= Admin.GetHashCode();
					}
					if (AccountType.Length != 0)
					{
						num ^= AccountType.GetHashCode();
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
					if (Hash != 0)
					{
						output.WriteRawTag((byte)8);
						output.WriteInt32(Hash);
					}
					if (nickName_ != null)
					{
						_single_nickName_codec.WriteTagAndValue(output, NickName);
					}
					if (Admin)
					{
						output.WriteRawTag((byte)24);
						output.WriteBool(Admin);
					}
					if (AccountType.Length != 0)
					{
						output.WriteRawTag((byte)34);
						output.WriteString(AccountType);
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
					if (Hash != 0)
					{
						num += 1 + CodedOutputStream.ComputeInt32Size(Hash);
					}
					if (nickName_ != null)
					{
						num += _single_nickName_codec.CalculateSizeWithTag(NickName);
					}
					if (Admin)
					{
						num += 2;
					}
					if (AccountType.Length != 0)
					{
						num += 1 + CodedOutputStream.ComputeStringSize(AccountType);
					}
					if (_unknownFields != null)
					{
						num += _unknownFields.CalculateSize();
					}
					return num;
				}

				[DebuggerNonUserCode]
				public void MergeFrom(AccountData other)
				{
					if (other != null)
					{
						if (other.Hash != 0)
						{
							Hash = other.Hash;
						}
						if (other.nickName_ != null && (nickName_ == null || other.NickName != ""))
						{
							NickName = other.NickName;
						}
						if (other.Admin)
						{
							Admin = other.Admin;
						}
						if (other.AccountType.Length != 0)
						{
							AccountType = other.AccountType;
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
							Hash = input.ReadInt32();
							break;
						case 18u:
						{
							string text = _single_nickName_codec.Read(input);
							if (nickName_ == null || text != "")
							{
								NickName = text;
							}
							break;
						}
						case 24u:
							Admin = input.ReadBool();
							break;
						case 34u:
							AccountType = input.ReadString();
							break;
						}
					}
				}

				public string ToDiagnosticString()
				{
					return "AccountData";
				}
			}

			public sealed class DecksData : IMessage<DecksData>, IMessage, IEquatable<DecksData>, IDeepCloneable<DecksData>, ICustomDiagnosticMessage
			{
				private static readonly MessageParser<DecksData> _parser = new MessageParser<DecksData>((Func<DecksData>)(() => new DecksData()));

				private UnknownFieldSet _unknownFields;

				public const int CustomDecksFieldNumber = 1;

				private static readonly FieldCodec<DeckInfo> _repeated_customDecks_codec = FieldCodec.ForMessage<DeckInfo>(10u, DeckInfo.Parser);

				private readonly RepeatedField<DeckInfo> customDecks_ = new RepeatedField<DeckInfo>();

				public const int SelectedDecksFieldNumber = 2;

				private static readonly Codec<int, int> _map_selectedDecks_codec = new Codec<int, int>(FieldCodec.ForInt32(8u), FieldCodec.ForInt32(16u), 18u);

				private readonly MapField<int, int> selectedDecks_ = new MapField<int, int>();

				[DebuggerNonUserCode]
				public static MessageParser<DecksData> Parser => _parser;

				[DebuggerNonUserCode]
				public static MessageDescriptor Descriptor => PlayerDataEvent.Descriptor.get_NestedTypes()[1];

				[DebuggerNonUserCode]
				MessageDescriptor Descriptor => Descriptor;

				[DebuggerNonUserCode]
				public RepeatedField<DeckInfo> CustomDecks => customDecks_;

				[DebuggerNonUserCode]
				public MapField<int, int> SelectedDecks => selectedDecks_;

				[DebuggerNonUserCode]
				public DecksData()
				{
				}

				[DebuggerNonUserCode]
				public DecksData(DecksData other)
					: this()
				{
					customDecks_ = other.customDecks_.Clone();
					selectedDecks_ = other.selectedDecks_.Clone();
					_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
				}

				[DebuggerNonUserCode]
				public DecksData Clone()
				{
					return new DecksData(this);
				}

				[DebuggerNonUserCode]
				public override bool Equals(object other)
				{
					return Equals(other as DecksData);
				}

				[DebuggerNonUserCode]
				public bool Equals(DecksData other)
				{
					if (other == null)
					{
						return false;
					}
					if (other == this)
					{
						return true;
					}
					if (!customDecks_.Equals(other.customDecks_))
					{
						return false;
					}
					if (!SelectedDecks.Equals(other.SelectedDecks))
					{
						return false;
					}
					return object.Equals(_unknownFields, other._unknownFields);
				}

				[DebuggerNonUserCode]
				public override int GetHashCode()
				{
					int num = 1;
					num ^= ((object)customDecks_).GetHashCode();
					num ^= ((object)SelectedDecks).GetHashCode();
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
					customDecks_.WriteTo(output, _repeated_customDecks_codec);
					selectedDecks_.WriteTo(output, _map_selectedDecks_codec);
					if (_unknownFields != null)
					{
						_unknownFields.WriteTo(output);
					}
				}

				[DebuggerNonUserCode]
				public int CalculateSize()
				{
					int num = 0;
					num += customDecks_.CalculateSize(_repeated_customDecks_codec);
					num += selectedDecks_.CalculateSize(_map_selectedDecks_codec);
					if (_unknownFields != null)
					{
						num += _unknownFields.CalculateSize();
					}
					return num;
				}

				[DebuggerNonUserCode]
				public void MergeFrom(DecksData other)
				{
					if (other != null)
					{
						customDecks_.Add((IEnumerable<DeckInfo>)other.customDecks_);
						selectedDecks_.Add((IDictionary<int, int>)other.selectedDecks_);
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
							customDecks_.AddEntriesFrom(input, _repeated_customDecks_codec);
							break;
						case 18u:
							selectedDecks_.AddEntriesFrom(input, _map_selectedDecks_codec);
							break;
						}
					}
				}

				public string ToDiagnosticString()
				{
					return "DecksData";
				}
			}

			public sealed class OccupationData : IMessage<OccupationData>, IMessage, IEquatable<OccupationData>, IDeepCloneable<OccupationData>, ICustomDiagnosticMessage
			{
				private static readonly MessageParser<OccupationData> _parser = new MessageParser<OccupationData>((Func<OccupationData>)(() => new OccupationData()));

				private UnknownFieldSet _unknownFields;

				public const int InFightFieldNumber = 1;

				private bool inFight_;

				[DebuggerNonUserCode]
				public static MessageParser<OccupationData> Parser => _parser;

				[DebuggerNonUserCode]
				public static MessageDescriptor Descriptor => PlayerDataEvent.Descriptor.get_NestedTypes()[2];

				[DebuggerNonUserCode]
				MessageDescriptor Descriptor => Descriptor;

				[DebuggerNonUserCode]
				public bool InFight
				{
					get
					{
						return inFight_;
					}
					set
					{
						inFight_ = value;
					}
				}

				[DebuggerNonUserCode]
				public OccupationData()
				{
				}

				[DebuggerNonUserCode]
				public OccupationData(OccupationData other)
					: this()
				{
					inFight_ = other.inFight_;
					_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
				}

				[DebuggerNonUserCode]
				public OccupationData Clone()
				{
					return new OccupationData(this);
				}

				[DebuggerNonUserCode]
				public override bool Equals(object other)
				{
					return Equals(other as OccupationData);
				}

				[DebuggerNonUserCode]
				public bool Equals(OccupationData other)
				{
					if (other == null)
					{
						return false;
					}
					if (other == this)
					{
						return true;
					}
					if (InFight != other.InFight)
					{
						return false;
					}
					return object.Equals(_unknownFields, other._unknownFields);
				}

				[DebuggerNonUserCode]
				public override int GetHashCode()
				{
					int num = 1;
					if (InFight)
					{
						num ^= InFight.GetHashCode();
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
					if (InFight)
					{
						output.WriteRawTag((byte)8);
						output.WriteBool(InFight);
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
					if (InFight)
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
				public void MergeFrom(OccupationData other)
				{
					if (other != null)
					{
						if (other.InFight)
						{
							InFight = other.InFight;
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
						if (num != 8)
						{
							_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
						}
						else
						{
							InFight = input.ReadBool();
						}
					}
				}

				public string ToDiagnosticString()
				{
					return "OccupationData";
				}
			}

			public sealed class HeroData : IMessage<HeroData>, IMessage, IEquatable<HeroData>, IDeepCloneable<HeroData>, ICustomDiagnosticMessage
			{
				private static readonly MessageParser<HeroData> _parser = new MessageParser<HeroData>((Func<HeroData>)(() => new HeroData()));

				private UnknownFieldSet _unknownFields;

				public const int InfoFieldNumber = 1;

				private HeroInfo info_;

				[DebuggerNonUserCode]
				public static MessageParser<HeroData> Parser => _parser;

				[DebuggerNonUserCode]
				public static MessageDescriptor Descriptor => PlayerDataEvent.Descriptor.get_NestedTypes()[3];

				[DebuggerNonUserCode]
				MessageDescriptor Descriptor => Descriptor;

				[DebuggerNonUserCode]
				public HeroInfo Info
				{
					get
					{
						return info_;
					}
					set
					{
						info_ = value;
					}
				}

				[DebuggerNonUserCode]
				public HeroData()
				{
				}

				[DebuggerNonUserCode]
				public HeroData(HeroData other)
					: this()
				{
					info_ = ((other.info_ != null) ? other.info_.Clone() : null);
					_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
				}

				[DebuggerNonUserCode]
				public HeroData Clone()
				{
					return new HeroData(this);
				}

				[DebuggerNonUserCode]
				public override bool Equals(object other)
				{
					return Equals(other as HeroData);
				}

				[DebuggerNonUserCode]
				public bool Equals(HeroData other)
				{
					if (other == null)
					{
						return false;
					}
					if (other == this)
					{
						return true;
					}
					if (!object.Equals(Info, other.Info))
					{
						return false;
					}
					return object.Equals(_unknownFields, other._unknownFields);
				}

				[DebuggerNonUserCode]
				public override int GetHashCode()
				{
					int num = 1;
					if (info_ != null)
					{
						num ^= Info.GetHashCode();
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
					if (info_ != null)
					{
						output.WriteRawTag((byte)10);
						output.WriteMessage(Info);
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
					if (info_ != null)
					{
						num += 1 + CodedOutputStream.ComputeMessageSize(Info);
					}
					if (_unknownFields != null)
					{
						num += _unknownFields.CalculateSize();
					}
					return num;
				}

				[DebuggerNonUserCode]
				public void MergeFrom(HeroData other)
				{
					if (other == null)
					{
						return;
					}
					if (other.info_ != null)
					{
						if (info_ == null)
						{
							info_ = new HeroInfo();
						}
						Info.MergeFrom(other.Info);
					}
					_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
				}

				[DebuggerNonUserCode]
				public void MergeFrom(CodedInputStream input)
				{
					uint num;
					while ((num = input.ReadTag()) != 0)
					{
						if (num != 10)
						{
							_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
							continue;
						}
						if (info_ == null)
						{
							info_ = new HeroInfo();
						}
						input.ReadMessage(info_);
					}
				}

				public string ToDiagnosticString()
				{
					return "HeroData";
				}
			}

			public sealed class CompanionData : IMessage<CompanionData>, IMessage, IEquatable<CompanionData>, IDeepCloneable<CompanionData>, ICustomDiagnosticMessage
			{
				private static readonly MessageParser<CompanionData> _parser = new MessageParser<CompanionData>((Func<CompanionData>)(() => new CompanionData()));

				private UnknownFieldSet _unknownFields;

				public const int CompanionsFieldNumber = 1;

				private static readonly FieldCodec<int> _repeated_companions_codec = FieldCodec.ForInt32(10u);

				private readonly RepeatedField<int> companions_ = new RepeatedField<int>();

				[DebuggerNonUserCode]
				public static MessageParser<CompanionData> Parser => _parser;

				[DebuggerNonUserCode]
				public static MessageDescriptor Descriptor => PlayerDataEvent.Descriptor.get_NestedTypes()[4];

				[DebuggerNonUserCode]
				MessageDescriptor Descriptor => Descriptor;

				[DebuggerNonUserCode]
				public RepeatedField<int> Companions => companions_;

				[DebuggerNonUserCode]
				public CompanionData()
				{
				}

				[DebuggerNonUserCode]
				public CompanionData(CompanionData other)
					: this()
				{
					companions_ = other.companions_.Clone();
					_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
				}

				[DebuggerNonUserCode]
				public CompanionData Clone()
				{
					return new CompanionData(this);
				}

				[DebuggerNonUserCode]
				public override bool Equals(object other)
				{
					return Equals(other as CompanionData);
				}

				[DebuggerNonUserCode]
				public bool Equals(CompanionData other)
				{
					if (other == null)
					{
						return false;
					}
					if (other == this)
					{
						return true;
					}
					if (!companions_.Equals(other.companions_))
					{
						return false;
					}
					return object.Equals(_unknownFields, other._unknownFields);
				}

				[DebuggerNonUserCode]
				public override int GetHashCode()
				{
					int num = 1;
					num ^= ((object)companions_).GetHashCode();
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
					companions_.WriteTo(output, _repeated_companions_codec);
					if (_unknownFields != null)
					{
						_unknownFields.WriteTo(output);
					}
				}

				[DebuggerNonUserCode]
				public int CalculateSize()
				{
					int num = 0;
					num += companions_.CalculateSize(_repeated_companions_codec);
					if (_unknownFields != null)
					{
						num += _unknownFields.CalculateSize();
					}
					return num;
				}

				[DebuggerNonUserCode]
				public void MergeFrom(CompanionData other)
				{
					if (other != null)
					{
						companions_.Add((IEnumerable<int>)other.companions_);
						_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
					}
				}

				[DebuggerNonUserCode]
				public void MergeFrom(CodedInputStream input)
				{
					uint num;
					while ((num = input.ReadTag()) != 0)
					{
						if (num != 8 && num != 10)
						{
							_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
						}
						else
						{
							companions_.AddEntriesFrom(input, _repeated_companions_codec);
						}
					}
				}

				public string ToDiagnosticString()
				{
					return "CompanionData";
				}
			}

			public sealed class WeaponLevelsData : IMessage<WeaponLevelsData>, IMessage, IEquatable<WeaponLevelsData>, IDeepCloneable<WeaponLevelsData>, ICustomDiagnosticMessage
			{
				private static readonly MessageParser<WeaponLevelsData> _parser = new MessageParser<WeaponLevelsData>((Func<WeaponLevelsData>)(() => new WeaponLevelsData()));

				private UnknownFieldSet _unknownFields;

				public const int WeaponLevelsFieldNumber = 1;

				private static readonly Codec<int, int> _map_weaponLevels_codec = new Codec<int, int>(FieldCodec.ForInt32(8u), FieldCodec.ForInt32(16u), 10u);

				private readonly MapField<int, int> weaponLevels_ = new MapField<int, int>();

				[DebuggerNonUserCode]
				public static MessageParser<WeaponLevelsData> Parser => _parser;

				[DebuggerNonUserCode]
				public static MessageDescriptor Descriptor => PlayerDataEvent.Descriptor.get_NestedTypes()[5];

				[DebuggerNonUserCode]
				MessageDescriptor Descriptor => Descriptor;

				[DebuggerNonUserCode]
				public MapField<int, int> WeaponLevels => weaponLevels_;

				[DebuggerNonUserCode]
				public WeaponLevelsData()
				{
				}

				[DebuggerNonUserCode]
				public WeaponLevelsData(WeaponLevelsData other)
					: this()
				{
					weaponLevels_ = other.weaponLevels_.Clone();
					_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
				}

				[DebuggerNonUserCode]
				public WeaponLevelsData Clone()
				{
					return new WeaponLevelsData(this);
				}

				[DebuggerNonUserCode]
				public override bool Equals(object other)
				{
					return Equals(other as WeaponLevelsData);
				}

				[DebuggerNonUserCode]
				public bool Equals(WeaponLevelsData other)
				{
					if (other == null)
					{
						return false;
					}
					if (other == this)
					{
						return true;
					}
					if (!WeaponLevels.Equals(other.WeaponLevels))
					{
						return false;
					}
					return object.Equals(_unknownFields, other._unknownFields);
				}

				[DebuggerNonUserCode]
				public override int GetHashCode()
				{
					int num = 1;
					num ^= ((object)WeaponLevels).GetHashCode();
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
					weaponLevels_.WriteTo(output, _map_weaponLevels_codec);
					if (_unknownFields != null)
					{
						_unknownFields.WriteTo(output);
					}
				}

				[DebuggerNonUserCode]
				public int CalculateSize()
				{
					int num = 0;
					num += weaponLevels_.CalculateSize(_map_weaponLevels_codec);
					if (_unknownFields != null)
					{
						num += _unknownFields.CalculateSize();
					}
					return num;
				}

				[DebuggerNonUserCode]
				public void MergeFrom(WeaponLevelsData other)
				{
					if (other != null)
					{
						weaponLevels_.Add((IDictionary<int, int>)other.weaponLevels_);
						_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
					}
				}

				[DebuggerNonUserCode]
				public void MergeFrom(CodedInputStream input)
				{
					uint num;
					while ((num = input.ReadTag()) != 0)
					{
						if (num != 10)
						{
							_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
						}
						else
						{
							weaponLevels_.AddEntriesFrom(input, _map_weaponLevels_codec);
						}
					}
				}

				public string ToDiagnosticString()
				{
					return "WeaponLevelsData";
				}
			}

			public sealed class SelectedWeaponsData : IMessage<SelectedWeaponsData>, IMessage, IEquatable<SelectedWeaponsData>, IDeepCloneable<SelectedWeaponsData>, ICustomDiagnosticMessage
			{
				private static readonly MessageParser<SelectedWeaponsData> _parser = new MessageParser<SelectedWeaponsData>((Func<SelectedWeaponsData>)(() => new SelectedWeaponsData()));

				private UnknownFieldSet _unknownFields;

				public const int SelectedWeaponsFieldNumber = 1;

				private static readonly Codec<int, int> _map_selectedWeapons_codec = new Codec<int, int>(FieldCodec.ForInt32(8u), FieldCodec.ForInt32(16u), 10u);

				private readonly MapField<int, int> selectedWeapons_ = new MapField<int, int>();

				[DebuggerNonUserCode]
				public static MessageParser<SelectedWeaponsData> Parser => _parser;

				[DebuggerNonUserCode]
				public static MessageDescriptor Descriptor => PlayerDataEvent.Descriptor.get_NestedTypes()[6];

				[DebuggerNonUserCode]
				MessageDescriptor Descriptor => Descriptor;

				[DebuggerNonUserCode]
				public MapField<int, int> SelectedWeapons => selectedWeapons_;

				[DebuggerNonUserCode]
				public SelectedWeaponsData()
				{
				}

				[DebuggerNonUserCode]
				public SelectedWeaponsData(SelectedWeaponsData other)
					: this()
				{
					selectedWeapons_ = other.selectedWeapons_.Clone();
					_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
				}

				[DebuggerNonUserCode]
				public SelectedWeaponsData Clone()
				{
					return new SelectedWeaponsData(this);
				}

				[DebuggerNonUserCode]
				public override bool Equals(object other)
				{
					return Equals(other as SelectedWeaponsData);
				}

				[DebuggerNonUserCode]
				public bool Equals(SelectedWeaponsData other)
				{
					if (other == null)
					{
						return false;
					}
					if (other == this)
					{
						return true;
					}
					if (!SelectedWeapons.Equals(other.SelectedWeapons))
					{
						return false;
					}
					return object.Equals(_unknownFields, other._unknownFields);
				}

				[DebuggerNonUserCode]
				public override int GetHashCode()
				{
					int num = 1;
					num ^= ((object)SelectedWeapons).GetHashCode();
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
					selectedWeapons_.WriteTo(output, _map_selectedWeapons_codec);
					if (_unknownFields != null)
					{
						_unknownFields.WriteTo(output);
					}
				}

				[DebuggerNonUserCode]
				public int CalculateSize()
				{
					int num = 0;
					num += selectedWeapons_.CalculateSize(_map_selectedWeapons_codec);
					if (_unknownFields != null)
					{
						num += _unknownFields.CalculateSize();
					}
					return num;
				}

				[DebuggerNonUserCode]
				public void MergeFrom(SelectedWeaponsData other)
				{
					if (other != null)
					{
						selectedWeapons_.Add((IDictionary<int, int>)other.selectedWeapons_);
						_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
					}
				}

				[DebuggerNonUserCode]
				public void MergeFrom(CodedInputStream input)
				{
					uint num;
					while ((num = input.ReadTag()) != 0)
					{
						if (num != 10)
						{
							_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
						}
						else
						{
							selectedWeapons_.AddEntriesFrom(input, _map_selectedWeapons_codec);
						}
					}
				}

				public string ToDiagnosticString()
				{
					return "SelectedWeaponsData";
				}
			}

			public sealed class DeckIncrementalUpdateData : IMessage<DeckIncrementalUpdateData>, IMessage, IEquatable<DeckIncrementalUpdateData>, IDeepCloneable<DeckIncrementalUpdateData>, ICustomDiagnosticMessage
			{
				[DebuggerNonUserCode]
				public static class Types
				{
					public sealed class SelectedDeckPerWeapon : IMessage<SelectedDeckPerWeapon>, IMessage, IEquatable<SelectedDeckPerWeapon>, IDeepCloneable<SelectedDeckPerWeapon>, ICustomDiagnosticMessage
					{
						private static readonly MessageParser<SelectedDeckPerWeapon> _parser = new MessageParser<SelectedDeckPerWeapon>((Func<SelectedDeckPerWeapon>)(() => new SelectedDeckPerWeapon()));

						private UnknownFieldSet _unknownFields;

						public const int WeaponIdFieldNumber = 1;

						private int weaponId_;

						public const int DeckIdFieldNumber = 2;

						private static readonly FieldCodec<int?> _single_deckId_codec = FieldCodec.ForStructWrapper<int>(18u);

						private int? deckId_;

						[DebuggerNonUserCode]
						public static MessageParser<SelectedDeckPerWeapon> Parser => _parser;

						[DebuggerNonUserCode]
						public static MessageDescriptor Descriptor => DeckIncrementalUpdateData.Descriptor.get_NestedTypes()[0];

						[DebuggerNonUserCode]
						MessageDescriptor Descriptor => Descriptor;

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
						public int? DeckId
						{
							get
							{
								return deckId_;
							}
							set
							{
								deckId_ = value;
							}
						}

						[DebuggerNonUserCode]
						public SelectedDeckPerWeapon()
						{
						}

						[DebuggerNonUserCode]
						public SelectedDeckPerWeapon(SelectedDeckPerWeapon other)
							: this()
						{
							weaponId_ = other.weaponId_;
							DeckId = other.DeckId;
							_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
						}

						[DebuggerNonUserCode]
						public SelectedDeckPerWeapon Clone()
						{
							return new SelectedDeckPerWeapon(this);
						}

						[DebuggerNonUserCode]
						public override bool Equals(object other)
						{
							return Equals(other as SelectedDeckPerWeapon);
						}

						[DebuggerNonUserCode]
						public bool Equals(SelectedDeckPerWeapon other)
						{
							if (other == null)
							{
								return false;
							}
							if (other == this)
							{
								return true;
							}
							if (WeaponId != other.WeaponId)
							{
								return false;
							}
							if (DeckId != other.DeckId)
							{
								return false;
							}
							return object.Equals(_unknownFields, other._unknownFields);
						}

						[DebuggerNonUserCode]
						public override int GetHashCode()
						{
							int num = 1;
							if (WeaponId != 0)
							{
								num ^= WeaponId.GetHashCode();
							}
							if (deckId_.HasValue)
							{
								num ^= DeckId.GetHashCode();
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
							if (WeaponId != 0)
							{
								output.WriteRawTag((byte)8);
								output.WriteInt32(WeaponId);
							}
							if (deckId_.HasValue)
							{
								_single_deckId_codec.WriteTagAndValue(output, DeckId);
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
							if (WeaponId != 0)
							{
								num += 1 + CodedOutputStream.ComputeInt32Size(WeaponId);
							}
							if (deckId_.HasValue)
							{
								num += _single_deckId_codec.CalculateSizeWithTag(DeckId);
							}
							if (_unknownFields != null)
							{
								num += _unknownFields.CalculateSize();
							}
							return num;
						}

						[DebuggerNonUserCode]
						public void MergeFrom(SelectedDeckPerWeapon other)
						{
							if (other != null)
							{
								if (other.WeaponId != 0)
								{
									WeaponId = other.WeaponId;
								}
								if (other.deckId_.HasValue && (!deckId_.HasValue || other.DeckId != 0))
								{
									DeckId = other.DeckId;
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
									WeaponId = input.ReadInt32();
									break;
								case 18u:
								{
									int? num2 = _single_deckId_codec.Read(input);
									if (!deckId_.HasValue || num2 != 0)
									{
										DeckId = num2;
									}
									break;
								}
								}
							}
						}

						public string ToDiagnosticString()
						{
							return "SelectedDeckPerWeapon";
						}
					}
				}

				private static readonly MessageParser<DeckIncrementalUpdateData> _parser = new MessageParser<DeckIncrementalUpdateData>((Func<DeckIncrementalUpdateData>)(() => new DeckIncrementalUpdateData()));

				private UnknownFieldSet _unknownFields;

				public const int DeckUpdatedFieldNumber = 1;

				private static readonly FieldCodec<DeckInfo> _repeated_deckUpdated_codec = FieldCodec.ForMessage<DeckInfo>(10u, DeckInfo.Parser);

				private readonly RepeatedField<DeckInfo> deckUpdated_ = new RepeatedField<DeckInfo>();

				public const int DeckRemovedIdFieldNumber = 2;

				private static readonly FieldCodec<int> _repeated_deckRemovedId_codec = FieldCodec.ForInt32(18u);

				private readonly RepeatedField<int> deckRemovedId_ = new RepeatedField<int>();

				public const int DeckSelectionsUpdatedFieldNumber = 3;

				private static readonly FieldCodec<Types.SelectedDeckPerWeapon> _repeated_deckSelectionsUpdated_codec = FieldCodec.ForMessage<Types.SelectedDeckPerWeapon>(26u, Types.SelectedDeckPerWeapon.Parser);

				private readonly RepeatedField<Types.SelectedDeckPerWeapon> deckSelectionsUpdated_ = new RepeatedField<Types.SelectedDeckPerWeapon>();

				[DebuggerNonUserCode]
				public static MessageParser<DeckIncrementalUpdateData> Parser => _parser;

				[DebuggerNonUserCode]
				public static MessageDescriptor Descriptor => PlayerDataEvent.Descriptor.get_NestedTypes()[7];

				[DebuggerNonUserCode]
				MessageDescriptor Descriptor => Descriptor;

				[DebuggerNonUserCode]
				public RepeatedField<DeckInfo> DeckUpdated => deckUpdated_;

				[DebuggerNonUserCode]
				public RepeatedField<int> DeckRemovedId => deckRemovedId_;

				[DebuggerNonUserCode]
				public RepeatedField<Types.SelectedDeckPerWeapon> DeckSelectionsUpdated => deckSelectionsUpdated_;

				[DebuggerNonUserCode]
				public DeckIncrementalUpdateData()
				{
				}

				[DebuggerNonUserCode]
				public DeckIncrementalUpdateData(DeckIncrementalUpdateData other)
					: this()
				{
					deckUpdated_ = other.deckUpdated_.Clone();
					deckRemovedId_ = other.deckRemovedId_.Clone();
					deckSelectionsUpdated_ = other.deckSelectionsUpdated_.Clone();
					_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
				}

				[DebuggerNonUserCode]
				public DeckIncrementalUpdateData Clone()
				{
					return new DeckIncrementalUpdateData(this);
				}

				[DebuggerNonUserCode]
				public override bool Equals(object other)
				{
					return Equals(other as DeckIncrementalUpdateData);
				}

				[DebuggerNonUserCode]
				public bool Equals(DeckIncrementalUpdateData other)
				{
					if (other == null)
					{
						return false;
					}
					if (other == this)
					{
						return true;
					}
					if (!deckUpdated_.Equals(other.deckUpdated_))
					{
						return false;
					}
					if (!deckRemovedId_.Equals(other.deckRemovedId_))
					{
						return false;
					}
					if (!deckSelectionsUpdated_.Equals(other.deckSelectionsUpdated_))
					{
						return false;
					}
					return object.Equals(_unknownFields, other._unknownFields);
				}

				[DebuggerNonUserCode]
				public override int GetHashCode()
				{
					int num = 1;
					num ^= ((object)deckUpdated_).GetHashCode();
					num ^= ((object)deckRemovedId_).GetHashCode();
					num ^= ((object)deckSelectionsUpdated_).GetHashCode();
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
					deckUpdated_.WriteTo(output, _repeated_deckUpdated_codec);
					deckRemovedId_.WriteTo(output, _repeated_deckRemovedId_codec);
					deckSelectionsUpdated_.WriteTo(output, _repeated_deckSelectionsUpdated_codec);
					if (_unknownFields != null)
					{
						_unknownFields.WriteTo(output);
					}
				}

				[DebuggerNonUserCode]
				public int CalculateSize()
				{
					int num = 0;
					num += deckUpdated_.CalculateSize(_repeated_deckUpdated_codec);
					num += deckRemovedId_.CalculateSize(_repeated_deckRemovedId_codec);
					num += deckSelectionsUpdated_.CalculateSize(_repeated_deckSelectionsUpdated_codec);
					if (_unknownFields != null)
					{
						num += _unknownFields.CalculateSize();
					}
					return num;
				}

				[DebuggerNonUserCode]
				public void MergeFrom(DeckIncrementalUpdateData other)
				{
					if (other != null)
					{
						deckUpdated_.Add((IEnumerable<DeckInfo>)other.deckUpdated_);
						deckRemovedId_.Add((IEnumerable<int>)other.deckRemovedId_);
						deckSelectionsUpdated_.Add((IEnumerable<Types.SelectedDeckPerWeapon>)other.deckSelectionsUpdated_);
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
							deckUpdated_.AddEntriesFrom(input, _repeated_deckUpdated_codec);
							break;
						case 16u:
						case 18u:
							deckRemovedId_.AddEntriesFrom(input, _repeated_deckRemovedId_codec);
							break;
						case 26u:
							deckSelectionsUpdated_.AddEntriesFrom(input, _repeated_deckSelectionsUpdated_codec);
							break;
						}
					}
				}

				public string ToDiagnosticString()
				{
					return "DeckIncrementalUpdateData";
				}
			}
		}

		private static readonly MessageParser<PlayerDataEvent> _parser = new MessageParser<PlayerDataEvent>((Func<PlayerDataEvent>)(() => new PlayerDataEvent()));

		private UnknownFieldSet _unknownFields;

		public const int AccountFieldNumber = 1;

		private Types.AccountData account_;

		public const int CompanionDataFieldNumber = 2;

		private Types.CompanionData companionData_;

		public const int WeaponLevelsDataFieldNumber = 3;

		private Types.WeaponLevelsData weaponLevelsData_;

		public const int SelectedWeaponsDataFieldNumber = 4;

		private Types.SelectedWeaponsData selectedWeaponsData_;

		public const int DecksFieldNumber = 5;

		private Types.DecksData decks_;

		public const int OccupationFieldNumber = 6;

		private Types.OccupationData occupation_;

		public const int HeroFieldNumber = 7;

		private Types.HeroData hero_;

		public const int DecksUpdatesFieldNumber = 8;

		private Types.DeckIncrementalUpdateData decksUpdates_;

		[DebuggerNonUserCode]
		public static MessageParser<PlayerDataEvent> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => PlayerProtocolReflection.Descriptor.get_MessageTypes()[4];

		[DebuggerNonUserCode]
		MessageDescriptor Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public Types.AccountData Account
		{
			get
			{
				return account_;
			}
			set
			{
				account_ = value;
			}
		}

		[DebuggerNonUserCode]
		public Types.CompanionData CompanionData
		{
			get
			{
				return companionData_;
			}
			set
			{
				companionData_ = value;
			}
		}

		[DebuggerNonUserCode]
		public Types.WeaponLevelsData WeaponLevelsData
		{
			get
			{
				return weaponLevelsData_;
			}
			set
			{
				weaponLevelsData_ = value;
			}
		}

		[DebuggerNonUserCode]
		public Types.SelectedWeaponsData SelectedWeaponsData
		{
			get
			{
				return selectedWeaponsData_;
			}
			set
			{
				selectedWeaponsData_ = value;
			}
		}

		[DebuggerNonUserCode]
		public Types.DecksData Decks
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
		public Types.OccupationData Occupation
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
		public Types.HeroData Hero
		{
			get
			{
				return hero_;
			}
			set
			{
				hero_ = value;
			}
		}

		[DebuggerNonUserCode]
		public Types.DeckIncrementalUpdateData DecksUpdates
		{
			get
			{
				return decksUpdates_;
			}
			set
			{
				decksUpdates_ = value;
			}
		}

		[DebuggerNonUserCode]
		public PlayerDataEvent()
		{
		}

		[DebuggerNonUserCode]
		public PlayerDataEvent(PlayerDataEvent other)
			: this()
		{
			account_ = ((other.account_ != null) ? other.account_.Clone() : null);
			companionData_ = ((other.companionData_ != null) ? other.companionData_.Clone() : null);
			weaponLevelsData_ = ((other.weaponLevelsData_ != null) ? other.weaponLevelsData_.Clone() : null);
			selectedWeaponsData_ = ((other.selectedWeaponsData_ != null) ? other.selectedWeaponsData_.Clone() : null);
			decks_ = ((other.decks_ != null) ? other.decks_.Clone() : null);
			occupation_ = ((other.occupation_ != null) ? other.occupation_.Clone() : null);
			hero_ = ((other.hero_ != null) ? other.hero_.Clone() : null);
			decksUpdates_ = ((other.decksUpdates_ != null) ? other.decksUpdates_.Clone() : null);
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public PlayerDataEvent Clone()
		{
			return new PlayerDataEvent(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as PlayerDataEvent);
		}

		[DebuggerNonUserCode]
		public bool Equals(PlayerDataEvent other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (!object.Equals(Account, other.Account))
			{
				return false;
			}
			if (!object.Equals(CompanionData, other.CompanionData))
			{
				return false;
			}
			if (!object.Equals(WeaponLevelsData, other.WeaponLevelsData))
			{
				return false;
			}
			if (!object.Equals(SelectedWeaponsData, other.SelectedWeaponsData))
			{
				return false;
			}
			if (!object.Equals(Decks, other.Decks))
			{
				return false;
			}
			if (!object.Equals(Occupation, other.Occupation))
			{
				return false;
			}
			if (!object.Equals(Hero, other.Hero))
			{
				return false;
			}
			if (!object.Equals(DecksUpdates, other.DecksUpdates))
			{
				return false;
			}
			return object.Equals(_unknownFields, other._unknownFields);
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (account_ != null)
			{
				num ^= Account.GetHashCode();
			}
			if (companionData_ != null)
			{
				num ^= CompanionData.GetHashCode();
			}
			if (weaponLevelsData_ != null)
			{
				num ^= WeaponLevelsData.GetHashCode();
			}
			if (selectedWeaponsData_ != null)
			{
				num ^= SelectedWeaponsData.GetHashCode();
			}
			if (decks_ != null)
			{
				num ^= Decks.GetHashCode();
			}
			if (occupation_ != null)
			{
				num ^= Occupation.GetHashCode();
			}
			if (hero_ != null)
			{
				num ^= Hero.GetHashCode();
			}
			if (decksUpdates_ != null)
			{
				num ^= DecksUpdates.GetHashCode();
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
			if (account_ != null)
			{
				output.WriteRawTag((byte)10);
				output.WriteMessage(Account);
			}
			if (companionData_ != null)
			{
				output.WriteRawTag((byte)18);
				output.WriteMessage(CompanionData);
			}
			if (weaponLevelsData_ != null)
			{
				output.WriteRawTag((byte)26);
				output.WriteMessage(WeaponLevelsData);
			}
			if (selectedWeaponsData_ != null)
			{
				output.WriteRawTag((byte)34);
				output.WriteMessage(SelectedWeaponsData);
			}
			if (decks_ != null)
			{
				output.WriteRawTag((byte)42);
				output.WriteMessage(Decks);
			}
			if (occupation_ != null)
			{
				output.WriteRawTag((byte)50);
				output.WriteMessage(Occupation);
			}
			if (hero_ != null)
			{
				output.WriteRawTag((byte)58);
				output.WriteMessage(Hero);
			}
			if (decksUpdates_ != null)
			{
				output.WriteRawTag((byte)66);
				output.WriteMessage(DecksUpdates);
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
			if (account_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(Account);
			}
			if (companionData_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(CompanionData);
			}
			if (weaponLevelsData_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(WeaponLevelsData);
			}
			if (selectedWeaponsData_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(SelectedWeaponsData);
			}
			if (decks_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(Decks);
			}
			if (occupation_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(Occupation);
			}
			if (hero_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(Hero);
			}
			if (decksUpdates_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(DecksUpdates);
			}
			if (_unknownFields != null)
			{
				num += _unknownFields.CalculateSize();
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(PlayerDataEvent other)
		{
			if (other == null)
			{
				return;
			}
			if (other.account_ != null)
			{
				if (account_ == null)
				{
					account_ = new Types.AccountData();
				}
				Account.MergeFrom(other.Account);
			}
			if (other.companionData_ != null)
			{
				if (companionData_ == null)
				{
					companionData_ = new Types.CompanionData();
				}
				CompanionData.MergeFrom(other.CompanionData);
			}
			if (other.weaponLevelsData_ != null)
			{
				if (weaponLevelsData_ == null)
				{
					weaponLevelsData_ = new Types.WeaponLevelsData();
				}
				WeaponLevelsData.MergeFrom(other.WeaponLevelsData);
			}
			if (other.selectedWeaponsData_ != null)
			{
				if (selectedWeaponsData_ == null)
				{
					selectedWeaponsData_ = new Types.SelectedWeaponsData();
				}
				SelectedWeaponsData.MergeFrom(other.SelectedWeaponsData);
			}
			if (other.decks_ != null)
			{
				if (decks_ == null)
				{
					decks_ = new Types.DecksData();
				}
				Decks.MergeFrom(other.Decks);
			}
			if (other.occupation_ != null)
			{
				if (occupation_ == null)
				{
					occupation_ = new Types.OccupationData();
				}
				Occupation.MergeFrom(other.Occupation);
			}
			if (other.hero_ != null)
			{
				if (hero_ == null)
				{
					hero_ = new Types.HeroData();
				}
				Hero.MergeFrom(other.Hero);
			}
			if (other.decksUpdates_ != null)
			{
				if (decksUpdates_ == null)
				{
					decksUpdates_ = new Types.DeckIncrementalUpdateData();
				}
				DecksUpdates.MergeFrom(other.DecksUpdates);
			}
			_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
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
					if (account_ == null)
					{
						account_ = new Types.AccountData();
					}
					input.ReadMessage(account_);
					break;
				case 18u:
					if (companionData_ == null)
					{
						companionData_ = new Types.CompanionData();
					}
					input.ReadMessage(companionData_);
					break;
				case 26u:
					if (weaponLevelsData_ == null)
					{
						weaponLevelsData_ = new Types.WeaponLevelsData();
					}
					input.ReadMessage(weaponLevelsData_);
					break;
				case 34u:
					if (selectedWeaponsData_ == null)
					{
						selectedWeaponsData_ = new Types.SelectedWeaponsData();
					}
					input.ReadMessage(selectedWeaponsData_);
					break;
				case 42u:
					if (decks_ == null)
					{
						decks_ = new Types.DecksData();
					}
					input.ReadMessage(decks_);
					break;
				case 50u:
					if (occupation_ == null)
					{
						occupation_ = new Types.OccupationData();
					}
					input.ReadMessage(occupation_);
					break;
				case 58u:
					if (hero_ == null)
					{
						hero_ = new Types.HeroData();
					}
					input.ReadMessage(hero_);
					break;
				case 66u:
					if (decksUpdates_ == null)
					{
						decksUpdates_ = new Types.DeckIncrementalUpdateData();
					}
					input.ReadMessage(decksUpdates_);
					break;
				}
			}
		}

		public string ToDiagnosticString()
		{
			return "PlayerDataEvent";
		}
	}
}
