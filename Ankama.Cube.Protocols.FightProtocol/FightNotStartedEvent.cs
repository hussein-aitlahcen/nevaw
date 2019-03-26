using Google.Protobuf;
using Google.Protobuf.Reflection;
using System;
using System.Diagnostics;

namespace Ankama.Cube.Protocols.FightProtocol
{
	public sealed class FightNotStartedEvent : IMessage<FightNotStartedEvent>, IMessage, IEquatable<FightNotStartedEvent>, IDeepCloneable<FightNotStartedEvent>, ICustomDiagnosticMessage
	{
		private static readonly MessageParser<FightNotStartedEvent> _parser = new MessageParser<FightNotStartedEvent>((Func<FightNotStartedEvent>)(() => new FightNotStartedEvent()));

		private UnknownFieldSet _unknownFields;

		[DebuggerNonUserCode]
		public static MessageParser<FightNotStartedEvent> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => FightProtocolReflection.Descriptor.get_MessageTypes()[1];

		[DebuggerNonUserCode]
		MessageDescriptor Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public FightNotStartedEvent()
		{
		}

		[DebuggerNonUserCode]
		public FightNotStartedEvent(FightNotStartedEvent other)
			: this()
		{
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public FightNotStartedEvent Clone()
		{
			return new FightNotStartedEvent(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as FightNotStartedEvent);
		}

		[DebuggerNonUserCode]
		public bool Equals(FightNotStartedEvent other)
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
		public void MergeFrom(FightNotStartedEvent other)
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
			return "FightNotStartedEvent";
		}
	}
}
