using Google.Protobuf;
using Google.Protobuf.Reflection;
using System;
using System.Diagnostics;

namespace Ankama.Cube.Protocols.FightPreparationProtocol
{
	public sealed class LaunchMatchmakingResultEvent : IMessage<LaunchMatchmakingResultEvent>, IMessage, IEquatable<LaunchMatchmakingResultEvent>, IDeepCloneable<LaunchMatchmakingResultEvent>, ICustomDiagnosticMessage
	{
		[DebuggerNonUserCode]
		public static class Types
		{
			public enum Result
			{
				[OriginalName("OK")]
				Ok,
				[OriginalName("INTERNAL_ERROR")]
				InternalError,
				[OriginalName("VALID_DECK_NOT_FOUND")]
				ValidDeckNotFound,
				[OriginalName("ONLY_OWNER_CAN_LAUNCH")]
				OnlyOwnerCanLaunch,
				[OriginalName("GROUP_NOT_CREATED")]
				GroupNotCreated,
				[OriginalName("SOME_PLAYER_NOT_READY")]
				SomePlayerNotReady,
				[OriginalName("TOO_MANY_PLAYERS_FOR_FIGHT_DEFINITION")]
				TooManyPlayersForFightDefinition,
				[OriginalName("PLAYER_LEFT")]
				PlayerLeft
			}
		}

		private static readonly MessageParser<LaunchMatchmakingResultEvent> _parser = new MessageParser<LaunchMatchmakingResultEvent>((Func<LaunchMatchmakingResultEvent>)(() => new LaunchMatchmakingResultEvent()));

		private UnknownFieldSet _unknownFields;

		public const int FightDefIdFieldNumber = 1;

		private int fightDefId_;

		public const int ResultFieldNumber = 2;

		private Types.Result result_;

		[DebuggerNonUserCode]
		public static MessageParser<LaunchMatchmakingResultEvent> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => FightPreparationProtocolReflection.Descriptor.get_MessageTypes()[5];

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
		public Types.Result Result
		{
			get
			{
				return result_;
			}
			set
			{
				result_ = value;
			}
		}

		[DebuggerNonUserCode]
		public LaunchMatchmakingResultEvent()
		{
		}

		[DebuggerNonUserCode]
		public LaunchMatchmakingResultEvent(LaunchMatchmakingResultEvent other)
			: this()
		{
			fightDefId_ = other.fightDefId_;
			result_ = other.result_;
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public LaunchMatchmakingResultEvent Clone()
		{
			return new LaunchMatchmakingResultEvent(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as LaunchMatchmakingResultEvent);
		}

		[DebuggerNonUserCode]
		public bool Equals(LaunchMatchmakingResultEvent other)
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
			if (Result != other.Result)
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
			if (Result != 0)
			{
				num ^= Result.GetHashCode();
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
			if (FightDefId != 0)
			{
				output.WriteRawTag((byte)8);
				output.WriteInt32(FightDefId);
			}
			if (Result != 0)
			{
				output.WriteRawTag((byte)16);
				output.WriteEnum((int)Result);
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
			if (FightDefId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(FightDefId);
			}
			if (Result != 0)
			{
				num += 1 + CodedOutputStream.ComputeEnumSize((int)Result);
			}
			if (_unknownFields != null)
			{
				num += _unknownFields.CalculateSize();
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(LaunchMatchmakingResultEvent other)
		{
			if (other != null)
			{
				if (other.FightDefId != 0)
				{
					FightDefId = other.FightDefId;
				}
				if (other.Result != 0)
				{
					Result = other.Result;
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
					FightDefId = input.ReadInt32();
					break;
				case 16u:
					result_ = (Types.Result)input.ReadEnum();
					break;
				}
			}
		}

		public string ToDiagnosticString()
		{
			return "LaunchMatchmakingResultEvent";
		}
	}
}
