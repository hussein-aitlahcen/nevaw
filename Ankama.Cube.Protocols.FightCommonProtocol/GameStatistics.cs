using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Ankama.Cube.Protocols.FightCommonProtocol
{
	public sealed class GameStatistics : IMessage<GameStatistics>, IMessage, IEquatable<GameStatistics>, IDeepCloneable<GameStatistics>, ICustomDiagnosticMessage
	{
		[DebuggerNonUserCode]
		public static class Types
		{
			public sealed class PlayerStats : IMessage<PlayerStats>, IMessage, IEquatable<PlayerStats>, IDeepCloneable<PlayerStats>, ICustomDiagnosticMessage
			{
				private static readonly MessageParser<PlayerStats> _parser = new MessageParser<PlayerStats>((Func<PlayerStats>)(() => new PlayerStats()));

				private UnknownFieldSet _unknownFields;

				public const int PlayerIdFieldNumber = 1;

				private int playerId_;

				public const int FightIdFieldNumber = 2;

				private int fightId_;

				public const int StatsFieldNumber = 3;

				private static readonly Codec<int, int> _map_stats_codec = new Codec<int, int>(FieldCodec.ForInt32(8u), FieldCodec.ForInt32(16u), 26u);

				private readonly MapField<int, int> stats_ = new MapField<int, int>();

				public const int TitlesFieldNumber = 4;

				private static readonly FieldCodec<int> _repeated_titles_codec = FieldCodec.ForInt32(34u);

				private readonly RepeatedField<int> titles_ = new RepeatedField<int>();

				[DebuggerNonUserCode]
				public static MessageParser<PlayerStats> Parser => _parser;

				[DebuggerNonUserCode]
				public static MessageDescriptor Descriptor => GameStatistics.Descriptor.get_NestedTypes()[0];

				[DebuggerNonUserCode]
				MessageDescriptor Descriptor => Descriptor;

				[DebuggerNonUserCode]
				public int PlayerId
				{
					get
					{
						return playerId_;
					}
					set
					{
						playerId_ = value;
					}
				}

				[DebuggerNonUserCode]
				public int FightId
				{
					get
					{
						return fightId_;
					}
					set
					{
						fightId_ = value;
					}
				}

				[DebuggerNonUserCode]
				public MapField<int, int> Stats => stats_;

				[DebuggerNonUserCode]
				public RepeatedField<int> Titles => titles_;

				[DebuggerNonUserCode]
				public PlayerStats()
				{
				}

				[DebuggerNonUserCode]
				public PlayerStats(PlayerStats other)
					: this()
				{
					playerId_ = other.playerId_;
					fightId_ = other.fightId_;
					stats_ = other.stats_.Clone();
					titles_ = other.titles_.Clone();
					_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
				}

				[DebuggerNonUserCode]
				public PlayerStats Clone()
				{
					return new PlayerStats(this);
				}

				[DebuggerNonUserCode]
				public override bool Equals(object other)
				{
					return Equals(other as PlayerStats);
				}

				[DebuggerNonUserCode]
				public bool Equals(PlayerStats other)
				{
					if (other == null)
					{
						return false;
					}
					if (other == this)
					{
						return true;
					}
					if (PlayerId != other.PlayerId)
					{
						return false;
					}
					if (FightId != other.FightId)
					{
						return false;
					}
					if (!Stats.Equals(other.Stats))
					{
						return false;
					}
					if (!titles_.Equals(other.titles_))
					{
						return false;
					}
					return object.Equals(_unknownFields, other._unknownFields);
				}

				[DebuggerNonUserCode]
				public override int GetHashCode()
				{
					int num = 1;
					if (PlayerId != 0)
					{
						num ^= PlayerId.GetHashCode();
					}
					if (FightId != 0)
					{
						num ^= FightId.GetHashCode();
					}
					num ^= ((object)Stats).GetHashCode();
					num ^= ((object)titles_).GetHashCode();
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
					if (PlayerId != 0)
					{
						output.WriteRawTag((byte)8);
						output.WriteInt32(PlayerId);
					}
					if (FightId != 0)
					{
						output.WriteRawTag((byte)16);
						output.WriteInt32(FightId);
					}
					stats_.WriteTo(output, _map_stats_codec);
					titles_.WriteTo(output, _repeated_titles_codec);
					if (_unknownFields != null)
					{
						_unknownFields.WriteTo(output);
					}
				}

				[DebuggerNonUserCode]
				public int CalculateSize()
				{
					int num = 0;
					if (PlayerId != 0)
					{
						num += 1 + CodedOutputStream.ComputeInt32Size(PlayerId);
					}
					if (FightId != 0)
					{
						num += 1 + CodedOutputStream.ComputeInt32Size(FightId);
					}
					num += stats_.CalculateSize(_map_stats_codec);
					num += titles_.CalculateSize(_repeated_titles_codec);
					if (_unknownFields != null)
					{
						num += _unknownFields.CalculateSize();
					}
					return num;
				}

				[DebuggerNonUserCode]
				public void MergeFrom(PlayerStats other)
				{
					if (other != null)
					{
						if (other.PlayerId != 0)
						{
							PlayerId = other.PlayerId;
						}
						if (other.FightId != 0)
						{
							FightId = other.FightId;
						}
						stats_.Add((IDictionary<int, int>)other.stats_);
						titles_.Add((IEnumerable<int>)other.titles_);
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
							PlayerId = input.ReadInt32();
							break;
						case 16u:
							FightId = input.ReadInt32();
							break;
						case 26u:
							stats_.AddEntriesFrom(input, _map_stats_codec);
							break;
						case 32u:
						case 34u:
							titles_.AddEntriesFrom(input, _repeated_titles_codec);
							break;
						}
					}
				}

				public string ToDiagnosticString()
				{
					return "PlayerStats";
				}
			}
		}

		private static readonly MessageParser<GameStatistics> _parser = new MessageParser<GameStatistics>((Func<GameStatistics>)(() => new GameStatistics()));

		private UnknownFieldSet _unknownFields;

		public const int PlayerStatsFieldNumber = 1;

		private static readonly FieldCodec<Types.PlayerStats> _repeated_playerStats_codec = FieldCodec.ForMessage<Types.PlayerStats>(10u, Types.PlayerStats.Parser);

		private readonly RepeatedField<Types.PlayerStats> playerStats_ = new RepeatedField<Types.PlayerStats>();

		[DebuggerNonUserCode]
		public static MessageParser<GameStatistics> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => FightCommonProtocolReflection.Descriptor.get_MessageTypes()[3];

		[DebuggerNonUserCode]
		MessageDescriptor Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public RepeatedField<Types.PlayerStats> PlayerStats => playerStats_;

		[DebuggerNonUserCode]
		public GameStatistics()
		{
		}

		[DebuggerNonUserCode]
		public GameStatistics(GameStatistics other)
			: this()
		{
			playerStats_ = other.playerStats_.Clone();
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public GameStatistics Clone()
		{
			return new GameStatistics(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as GameStatistics);
		}

		[DebuggerNonUserCode]
		public bool Equals(GameStatistics other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (!playerStats_.Equals(other.playerStats_))
			{
				return false;
			}
			return object.Equals(_unknownFields, other._unknownFields);
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			num ^= ((object)playerStats_).GetHashCode();
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
			playerStats_.WriteTo(output, _repeated_playerStats_codec);
			if (_unknownFields != null)
			{
				_unknownFields.WriteTo(output);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			num += playerStats_.CalculateSize(_repeated_playerStats_codec);
			if (_unknownFields != null)
			{
				num += _unknownFields.CalculateSize();
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(GameStatistics other)
		{
			if (other != null)
			{
				playerStats_.Add((IEnumerable<Types.PlayerStats>)other.playerStats_);
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
					playerStats_.AddEntriesFrom(input, _repeated_playerStats_codec);
				}
			}
		}

		public string ToDiagnosticString()
		{
			return "GameStatistics";
		}
	}
}
