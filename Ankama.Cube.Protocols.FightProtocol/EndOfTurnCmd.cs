using Google.Protobuf;
using Google.Protobuf.Reflection;
using System;
using System.Diagnostics;

namespace Ankama.Cube.Protocols.FightProtocol
{
	public sealed class EndOfTurnCmd : IMessage<EndOfTurnCmd>, IMessage, IEquatable<EndOfTurnCmd>, IDeepCloneable<EndOfTurnCmd>, ICustomDiagnosticMessage
	{
		private static readonly MessageParser<EndOfTurnCmd> _parser = new MessageParser<EndOfTurnCmd>((Func<EndOfTurnCmd>)(() => new EndOfTurnCmd()));

		private UnknownFieldSet _unknownFields;

		public const int TurnIndexFieldNumber = 1;

		private int turnIndex_;

		[DebuggerNonUserCode]
		public static MessageParser<EndOfTurnCmd> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => FightProtocolReflection.Descriptor.get_MessageTypes()[17];

		[DebuggerNonUserCode]
		MessageDescriptor Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public int TurnIndex
		{
			get
			{
				return turnIndex_;
			}
			set
			{
				turnIndex_ = value;
			}
		}

		[DebuggerNonUserCode]
		public EndOfTurnCmd()
		{
		}

		[DebuggerNonUserCode]
		public EndOfTurnCmd(EndOfTurnCmd other)
			: this()
		{
			turnIndex_ = other.turnIndex_;
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public EndOfTurnCmd Clone()
		{
			return new EndOfTurnCmd(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as EndOfTurnCmd);
		}

		[DebuggerNonUserCode]
		public bool Equals(EndOfTurnCmd other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (TurnIndex != other.TurnIndex)
			{
				return false;
			}
			return object.Equals(_unknownFields, other._unknownFields);
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (TurnIndex != 0)
			{
				num ^= TurnIndex.GetHashCode();
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
			if (TurnIndex != 0)
			{
				output.WriteRawTag((byte)8);
				output.WriteInt32(TurnIndex);
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
			if (TurnIndex != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(TurnIndex);
			}
			if (_unknownFields != null)
			{
				num += _unknownFields.CalculateSize();
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(EndOfTurnCmd other)
		{
			if (other != null)
			{
				if (other.TurnIndex != 0)
				{
					TurnIndex = other.TurnIndex;
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
					TurnIndex = input.ReadInt32();
				}
			}
		}

		public string ToDiagnosticString()
		{
			return "EndOfTurnCmd";
		}
	}
}
