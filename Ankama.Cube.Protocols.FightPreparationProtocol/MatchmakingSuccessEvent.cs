using Google.Protobuf;
using Google.Protobuf.Reflection;
using System;
using System.Diagnostics;

namespace Ankama.Cube.Protocols.FightPreparationProtocol
{
	public sealed class MatchmakingSuccessEvent : IMessage<MatchmakingSuccessEvent>, IMessage, IEquatable<MatchmakingSuccessEvent>, IDeepCloneable<MatchmakingSuccessEvent>, ICustomDiagnosticMessage
	{
		private static readonly MessageParser<MatchmakingSuccessEvent> _parser = new MessageParser<MatchmakingSuccessEvent>((Func<MatchmakingSuccessEvent>)(() => new MatchmakingSuccessEvent()));

		private UnknownFieldSet _unknownFields;

		public const int FightDefIdFieldNumber = 1;

		private int fightDefId_;

		[DebuggerNonUserCode]
		public static MessageParser<MatchmakingSuccessEvent> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => FightPreparationProtocolReflection.Descriptor.get_MessageTypes()[7];

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
		public MatchmakingSuccessEvent()
		{
		}

		[DebuggerNonUserCode]
		public MatchmakingSuccessEvent(MatchmakingSuccessEvent other)
			: this()
		{
			fightDefId_ = other.fightDefId_;
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public MatchmakingSuccessEvent Clone()
		{
			return new MatchmakingSuccessEvent(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as MatchmakingSuccessEvent);
		}

		[DebuggerNonUserCode]
		public bool Equals(MatchmakingSuccessEvent other)
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
			if (_unknownFields != null)
			{
				num += _unknownFields.CalculateSize();
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(MatchmakingSuccessEvent other)
		{
			if (other != null)
			{
				if (other.FightDefId != 0)
				{
					FightDefId = other.FightDefId;
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
					FightDefId = input.ReadInt32();
				}
			}
		}

		public string ToDiagnosticString()
		{
			return "MatchmakingSuccessEvent";
		}
	}
}
