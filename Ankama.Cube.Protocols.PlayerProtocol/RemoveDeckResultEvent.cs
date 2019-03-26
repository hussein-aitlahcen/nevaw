using Ankama.Cube.Protocols.CommonProtocol;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using System;
using System.Diagnostics;

namespace Ankama.Cube.Protocols.PlayerProtocol
{
	public sealed class RemoveDeckResultEvent : IMessage<RemoveDeckResultEvent>, IMessage, IEquatable<RemoveDeckResultEvent>, IDeepCloneable<RemoveDeckResultEvent>, ICustomDiagnosticMessage
	{
		private static readonly MessageParser<RemoveDeckResultEvent> _parser = new MessageParser<RemoveDeckResultEvent>((Func<RemoveDeckResultEvent>)(() => new RemoveDeckResultEvent()));

		private UnknownFieldSet _unknownFields;

		public const int ResultFieldNumber = 1;

		private CmdResult result_;

		[DebuggerNonUserCode]
		public static MessageParser<RemoveDeckResultEvent> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => PlayerProtocolReflection.Descriptor.get_MessageTypes()[10];

		[DebuggerNonUserCode]
		MessageDescriptor Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public CmdResult Result
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
		public RemoveDeckResultEvent()
		{
		}

		[DebuggerNonUserCode]
		public RemoveDeckResultEvent(RemoveDeckResultEvent other)
			: this()
		{
			result_ = other.result_;
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public RemoveDeckResultEvent Clone()
		{
			return new RemoveDeckResultEvent(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as RemoveDeckResultEvent);
		}

		[DebuggerNonUserCode]
		public bool Equals(RemoveDeckResultEvent other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
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
			if (Result != 0)
			{
				output.WriteRawTag((byte)8);
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
		public void MergeFrom(RemoveDeckResultEvent other)
		{
			if (other != null)
			{
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
				if (num != 8)
				{
					_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
				}
				else
				{
					result_ = (CmdResult)input.ReadEnum();
				}
			}
		}

		public string ToDiagnosticString()
		{
			return "RemoveDeckResultEvent";
		}
	}
}
