using Google.Protobuf;
using Google.Protobuf.Reflection;
using System;
using System.Diagnostics;

namespace Ankama.Cube.Protocols.FightProtocol
{
	public sealed class LeaveCmd : IMessage<LeaveCmd>, IMessage, IEquatable<LeaveCmd>, IDeepCloneable<LeaveCmd>, ICustomDiagnosticMessage
	{
		private static readonly MessageParser<LeaveCmd> _parser = new MessageParser<LeaveCmd>((Func<LeaveCmd>)(() => new LeaveCmd()));

		private UnknownFieldSet _unknownFields;

		[DebuggerNonUserCode]
		public static MessageParser<LeaveCmd> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => FightProtocolReflection.Descriptor.get_MessageTypes()[3];

		[DebuggerNonUserCode]
		MessageDescriptor Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public LeaveCmd()
		{
		}

		[DebuggerNonUserCode]
		public LeaveCmd(LeaveCmd other)
			: this()
		{
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public LeaveCmd Clone()
		{
			return new LeaveCmd(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as LeaveCmd);
		}

		[DebuggerNonUserCode]
		public bool Equals(LeaveCmd other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			return object.Equals(_unknownFields, other._unknownFields);
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
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
			if (_unknownFields != null)
			{
				_unknownFields.WriteTo(output);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (_unknownFields != null)
			{
				num += _unknownFields.CalculateSize();
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(LeaveCmd other)
		{
			if (other != null)
			{
				_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
			}
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0)
			{
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
			}
		}

		public string ToDiagnosticString()
		{
			return "LeaveCmd";
		}
	}
}
