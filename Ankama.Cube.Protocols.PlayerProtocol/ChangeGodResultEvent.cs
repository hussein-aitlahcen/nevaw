using Ankama.Cube.Protocols.CommonProtocol;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using System;
using System.Diagnostics;

namespace Ankama.Cube.Protocols.PlayerProtocol
{
	public sealed class ChangeGodResultEvent : IMessage<ChangeGodResultEvent>, IMessage, IEquatable<ChangeGodResultEvent>, IDeepCloneable<ChangeGodResultEvent>, ICustomDiagnosticMessage
	{
		private static readonly MessageParser<ChangeGodResultEvent> _parser = new MessageParser<ChangeGodResultEvent>((Func<ChangeGodResultEvent>)(() => new ChangeGodResultEvent()));

		private UnknownFieldSet _unknownFields;

		public const int ResultFieldNumber = 1;

		private CmdResult result_;

		[DebuggerNonUserCode]
		public static MessageParser<ChangeGodResultEvent> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => PlayerProtocolReflection.Descriptor.get_MessageTypes()[6];

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
		public ChangeGodResultEvent()
		{
		}

		[DebuggerNonUserCode]
		public ChangeGodResultEvent(ChangeGodResultEvent other)
			: this()
		{
			result_ = other.result_;
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public ChangeGodResultEvent Clone()
		{
			return new ChangeGodResultEvent(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as ChangeGodResultEvent);
		}

		[DebuggerNonUserCode]
		public bool Equals(ChangeGodResultEvent other)
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
		public void MergeFrom(ChangeGodResultEvent other)
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
			return "ChangeGodResultEvent";
		}
	}
}
