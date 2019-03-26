using Google.Protobuf;
using Google.Protobuf.Reflection;
using System;
using System.Diagnostics;

namespace Ankama.Cube.Protocols.FightPreparationProtocol
{
	public sealed class LaunchMatchmakingCmd : IMessage<LaunchMatchmakingCmd>, IMessage, IEquatable<LaunchMatchmakingCmd>, IDeepCloneable<LaunchMatchmakingCmd>, ICustomDiagnosticMessage
	{
		private static readonly MessageParser<LaunchMatchmakingCmd> _parser = new MessageParser<LaunchMatchmakingCmd>((Func<LaunchMatchmakingCmd>)(() => new LaunchMatchmakingCmd()));

		private UnknownFieldSet _unknownFields;

		public const int FightDefIdFieldNumber = 1;

		private int fightDefId_;

		[DebuggerNonUserCode]
		public static MessageParser<LaunchMatchmakingCmd> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => FightPreparationProtocolReflection.Descriptor.get_MessageTypes()[3];

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
		public LaunchMatchmakingCmd()
		{
		}

		[DebuggerNonUserCode]
		public LaunchMatchmakingCmd(LaunchMatchmakingCmd other)
			: this()
		{
			fightDefId_ = other.fightDefId_;
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public LaunchMatchmakingCmd Clone()
		{
			return new LaunchMatchmakingCmd(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as LaunchMatchmakingCmd);
		}

		[DebuggerNonUserCode]
		public bool Equals(LaunchMatchmakingCmd other)
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
		public void MergeFrom(LaunchMatchmakingCmd other)
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
			return "LaunchMatchmakingCmd";
		}
	}
}
