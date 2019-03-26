using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Ankama.Cube.Protocols.FightCommonProtocol
{
	public sealed class FightInfo : IMessage<FightInfo>, IMessage, IEquatable<FightInfo>, IDeepCloneable<FightInfo>, ICustomDiagnosticMessage
	{
		[DebuggerNonUserCode]
		public static class Types
		{
			public sealed class Team : IMessage<Team>, IMessage, IEquatable<Team>, IDeepCloneable<Team>, ICustomDiagnosticMessage
			{
				private static readonly MessageParser<Team> _parser = new MessageParser<Team>((Func<Team>)(() => new Team()));

				private UnknownFieldSet _unknownFields;

				public const int PlayersFieldNumber = 1;

				private static readonly FieldCodec<Player> _repeated_players_codec = FieldCodec.ForMessage<Player>(10u, Player.Parser);

				private readonly RepeatedField<Player> players_ = new RepeatedField<Player>();

				[DebuggerNonUserCode]
				public static MessageParser<Team> Parser => _parser;

				[DebuggerNonUserCode]
				public static MessageDescriptor Descriptor => FightInfo.Descriptor.get_NestedTypes()[0];

				[DebuggerNonUserCode]
				MessageDescriptor Descriptor => Descriptor;

				[DebuggerNonUserCode]
				public RepeatedField<Player> Players => players_;

				[DebuggerNonUserCode]
				public Team()
				{
				}

				[DebuggerNonUserCode]
				public Team(Team other)
					: this()
				{
					players_ = other.players_.Clone();
					_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
				}

				[DebuggerNonUserCode]
				public Team Clone()
				{
					return new Team(this);
				}

				[DebuggerNonUserCode]
				public override bool Equals(object other)
				{
					return Equals(other as Team);
				}

				[DebuggerNonUserCode]
				public bool Equals(Team other)
				{
					if (other == null)
					{
						return false;
					}
					if (other == this)
					{
						return true;
					}
					if (!players_.Equals(other.players_))
					{
						return false;
					}
					return object.Equals(_unknownFields, other._unknownFields);
				}

				[DebuggerNonUserCode]
				public override int GetHashCode()
				{
					int num = 1;
					num ^= ((object)players_).GetHashCode();
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
					players_.WriteTo(output, _repeated_players_codec);
					if (_unknownFields != null)
					{
						_unknownFields.WriteTo(output);
					}
				}

				[DebuggerNonUserCode]
				public int CalculateSize()
				{
					int num = 0;
					num += players_.CalculateSize(_repeated_players_codec);
					if (_unknownFields != null)
					{
						num += _unknownFields.CalculateSize();
					}
					return num;
				}

				[DebuggerNonUserCode]
				public void MergeFrom(Team other)
				{
					if (other != null)
					{
						players_.Add((IEnumerable<Player>)other.players_);
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
							players_.AddEntriesFrom(input, _repeated_players_codec);
						}
					}
				}

				public string ToDiagnosticString()
				{
					return "Team";
				}
			}

			public sealed class Player : IMessage<Player>, IMessage, IEquatable<Player>, IDeepCloneable<Player>, ICustomDiagnosticMessage
			{
				private static readonly MessageParser<Player> _parser = new MessageParser<Player>((Func<Player>)(() => new Player()));

				private UnknownFieldSet _unknownFields;

				public const int NameFieldNumber = 1;

				private string name_ = "";

				public const int LevelFieldNumber = 2;

				private int level_;

				public const int WeaponIdFieldNumber = 3;

				private static readonly FieldCodec<int?> _single_weaponId_codec = FieldCodec.ForStructWrapper<int>(26u);

				private int? weaponId_;

				[DebuggerNonUserCode]
				public static MessageParser<Player> Parser => _parser;

				[DebuggerNonUserCode]
				public static MessageDescriptor Descriptor => FightInfo.Descriptor.get_NestedTypes()[1];

				[DebuggerNonUserCode]
				MessageDescriptor Descriptor => Descriptor;

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
				public int Level
				{
					get
					{
						return level_;
					}
					set
					{
						level_ = value;
					}
				}

				[DebuggerNonUserCode]
				public int? WeaponId
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
				public Player()
				{
				}

				[DebuggerNonUserCode]
				public Player(Player other)
					: this()
				{
					name_ = other.name_;
					level_ = other.level_;
					WeaponId = other.WeaponId;
					_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
				}

				[DebuggerNonUserCode]
				public Player Clone()
				{
					return new Player(this);
				}

				[DebuggerNonUserCode]
				public override bool Equals(object other)
				{
					return Equals(other as Player);
				}

				[DebuggerNonUserCode]
				public bool Equals(Player other)
				{
					if (other == null)
					{
						return false;
					}
					if (other == this)
					{
						return true;
					}
					if (Name != other.Name)
					{
						return false;
					}
					if (Level != other.Level)
					{
						return false;
					}
					if (WeaponId != other.WeaponId)
					{
						return false;
					}
					return object.Equals(_unknownFields, other._unknownFields);
				}

				[DebuggerNonUserCode]
				public override int GetHashCode()
				{
					int num = 1;
					if (Name.Length != 0)
					{
						num ^= Name.GetHashCode();
					}
					if (Level != 0)
					{
						num ^= Level.GetHashCode();
					}
					if (weaponId_.HasValue)
					{
						num ^= WeaponId.GetHashCode();
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
					if (Name.Length != 0)
					{
						output.WriteRawTag((byte)10);
						output.WriteString(Name);
					}
					if (Level != 0)
					{
						output.WriteRawTag((byte)16);
						output.WriteInt32(Level);
					}
					if (weaponId_.HasValue)
					{
						_single_weaponId_codec.WriteTagAndValue(output, WeaponId);
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
					if (Name.Length != 0)
					{
						num += 1 + CodedOutputStream.ComputeStringSize(Name);
					}
					if (Level != 0)
					{
						num += 1 + CodedOutputStream.ComputeInt32Size(Level);
					}
					if (weaponId_.HasValue)
					{
						num += _single_weaponId_codec.CalculateSizeWithTag(WeaponId);
					}
					if (_unknownFields != null)
					{
						num += _unknownFields.CalculateSize();
					}
					return num;
				}

				[DebuggerNonUserCode]
				public void MergeFrom(Player other)
				{
					if (other != null)
					{
						if (other.Name.Length != 0)
						{
							Name = other.Name;
						}
						if (other.Level != 0)
						{
							Level = other.Level;
						}
						if (other.weaponId_.HasValue && (!weaponId_.HasValue || other.WeaponId != 0))
						{
							WeaponId = other.WeaponId;
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
							Name = input.ReadString();
							break;
						case 16u:
							Level = input.ReadInt32();
							break;
						case 26u:
						{
							int? num2 = _single_weaponId_codec.Read(input);
							if (!weaponId_.HasValue || num2 != 0)
							{
								WeaponId = num2;
							}
							break;
						}
						}
					}
				}

				public string ToDiagnosticString()
				{
					return "Player";
				}
			}
		}

		private static readonly MessageParser<FightInfo> _parser = new MessageParser<FightInfo>((Func<FightInfo>)(() => new FightInfo()));

		private UnknownFieldSet _unknownFields;

		public const int FightDefIdFieldNumber = 1;

		private int fightDefId_;

		public const int FightMapIdFieldNumber = 2;

		private int fightMapId_;

		public const int FightTypeFieldNumber = 3;

		private int fightType_;

		public const int ConcurrentFightsCountFieldNumber = 4;

		private int concurrentFightsCount_;

		public const int OwnFightIdFieldNumber = 5;

		private int ownFightId_;

		public const int OwnTeamIndexFieldNumber = 6;

		private int ownTeamIndex_;

		public const int TeamsFieldNumber = 7;

		private static readonly FieldCodec<Types.Team> _repeated_teams_codec = FieldCodec.ForMessage<Types.Team>(58u, Types.Team.Parser);

		private readonly RepeatedField<Types.Team> teams_ = new RepeatedField<Types.Team>();

		[DebuggerNonUserCode]
		public static MessageParser<FightInfo> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => FightCommonProtocolReflection.Descriptor.get_MessageTypes()[2];

		[DebuggerNonUserCode]
		MessageDescriptor Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public int FightDefId
		{
			get
			{
				return fightDefId_;
			}
			set
			{
				fightDefId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int FightMapId
		{
			get
			{
				return fightMapId_;
			}
			set
			{
				fightMapId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int FightType
		{
			get
			{
				return fightType_;
			}
			set
			{
				fightType_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int ConcurrentFightsCount
		{
			get
			{
				return concurrentFightsCount_;
			}
			set
			{
				concurrentFightsCount_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int OwnFightId
		{
			get
			{
				return ownFightId_;
			}
			set
			{
				ownFightId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int OwnTeamIndex
		{
			get
			{
				return ownTeamIndex_;
			}
			set
			{
				ownTeamIndex_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<Types.Team> Teams => teams_;

		[DebuggerNonUserCode]
		public FightInfo()
		{
		}

		[DebuggerNonUserCode]
		public FightInfo(FightInfo other)
			: this()
		{
			fightDefId_ = other.fightDefId_;
			fightMapId_ = other.fightMapId_;
			fightType_ = other.fightType_;
			concurrentFightsCount_ = other.concurrentFightsCount_;
			ownFightId_ = other.ownFightId_;
			ownTeamIndex_ = other.ownTeamIndex_;
			teams_ = other.teams_.Clone();
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public FightInfo Clone()
		{
			return new FightInfo(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as FightInfo);
		}

		[DebuggerNonUserCode]
		public bool Equals(FightInfo other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (FightDefId != other.FightDefId)
			{
				return false;
			}
			if (FightMapId != other.FightMapId)
			{
				return false;
			}
			if (FightType != other.FightType)
			{
				return false;
			}
			if (ConcurrentFightsCount != other.ConcurrentFightsCount)
			{
				return false;
			}
			if (OwnFightId != other.OwnFightId)
			{
				return false;
			}
			if (OwnTeamIndex != other.OwnTeamIndex)
			{
				return false;
			}
			if (!teams_.Equals(other.teams_))
			{
				return false;
			}
			return object.Equals(_unknownFields, other._unknownFields);
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (FightDefId != 0)
			{
				num ^= FightDefId.GetHashCode();
			}
			if (FightMapId != 0)
			{
				num ^= FightMapId.GetHashCode();
			}
			if (FightType != 0)
			{
				num ^= FightType.GetHashCode();
			}
			if (ConcurrentFightsCount != 0)
			{
				num ^= ConcurrentFightsCount.GetHashCode();
			}
			if (OwnFightId != 0)
			{
				num ^= OwnFightId.GetHashCode();
			}
			if (OwnTeamIndex != 0)
			{
				num ^= OwnTeamIndex.GetHashCode();
			}
			num ^= ((object)teams_).GetHashCode();
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
			if (FightDefId != 0)
			{
				output.WriteRawTag((byte)8);
				output.WriteInt32(FightDefId);
			}
			if (FightMapId != 0)
			{
				output.WriteRawTag((byte)16);
				output.WriteInt32(FightMapId);
			}
			if (FightType != 0)
			{
				output.WriteRawTag((byte)24);
				output.WriteInt32(FightType);
			}
			if (ConcurrentFightsCount != 0)
			{
				output.WriteRawTag((byte)32);
				output.WriteInt32(ConcurrentFightsCount);
			}
			if (OwnFightId != 0)
			{
				output.WriteRawTag((byte)40);
				output.WriteInt32(OwnFightId);
			}
			if (OwnTeamIndex != 0)
			{
				output.WriteRawTag((byte)48);
				output.WriteInt32(OwnTeamIndex);
			}
			teams_.WriteTo(output, _repeated_teams_codec);
			if (_unknownFields != null)
			{
				_unknownFields.WriteTo(output);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (FightDefId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(FightDefId);
			}
			if (FightMapId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(FightMapId);
			}
			if (FightType != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(FightType);
			}
			if (ConcurrentFightsCount != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(ConcurrentFightsCount);
			}
			if (OwnFightId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(OwnFightId);
			}
			if (OwnTeamIndex != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(OwnTeamIndex);
			}
			num += teams_.CalculateSize(_repeated_teams_codec);
			if (_unknownFields != null)
			{
				num += _unknownFields.CalculateSize();
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(FightInfo other)
		{
			if (other != null)
			{
				if (other.FightDefId != 0)
				{
					FightDefId = other.FightDefId;
				}
				if (other.FightMapId != 0)
				{
					FightMapId = other.FightMapId;
				}
				if (other.FightType != 0)
				{
					FightType = other.FightType;
				}
				if (other.ConcurrentFightsCount != 0)
				{
					ConcurrentFightsCount = other.ConcurrentFightsCount;
				}
				if (other.OwnFightId != 0)
				{
					OwnFightId = other.OwnFightId;
				}
				if (other.OwnTeamIndex != 0)
				{
					OwnTeamIndex = other.OwnTeamIndex;
				}
				teams_.Add((IEnumerable<Types.Team>)other.teams_);
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
					FightDefId = input.ReadInt32();
					break;
				case 16u:
					FightMapId = input.ReadInt32();
					break;
				case 24u:
					FightType = input.ReadInt32();
					break;
				case 32u:
					ConcurrentFightsCount = input.ReadInt32();
					break;
				case 40u:
					OwnFightId = input.ReadInt32();
					break;
				case 48u:
					OwnTeamIndex = input.ReadInt32();
					break;
				case 58u:
					teams_.AddEntriesFrom(input, _repeated_teams_codec);
					break;
				}
			}
		}

		public string ToDiagnosticString()
		{
			return "FightInfo";
		}
	}
}
