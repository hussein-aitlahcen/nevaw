using Google.Protobuf;
using Google.Protobuf.Reflection;
using System;
using System.Diagnostics;

namespace Ankama.Cube.Protocols.FightProtocol
{
	public sealed class GetFightInfoCmd : IMessage<GetFightInfoCmd>, IMessage, IEquatable<GetFightInfoCmd>, IDeepCloneable<GetFightInfoCmd>, ICustomDiagnosticMessage
	{
		private static readonly MessageParser<GetFightInfoCmd> _parser = new MessageParser<GetFightInfoCmd>((Func<GetFightInfoCmd>)(() => new GetFightInfoCmd()));

		private UnknownFieldSet _unknownFields;

		[DebuggerNonUserCode]
		public static MessageParser<GetFightInfoCmd> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => FightProtocolReflection.Descriptor.get_MessageTypes()[4];

		[DebuggerNonUserCode]
		MessageDescriptor Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public GetFightInfoCmd()
		{
		}

		[DebuggerNonUserCode]
		public GetFightInfoCmd(GetFightInfoCmd other)
			: this()
		{
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public GetFightInfoCmd Clone()
		{
			return new GetFightInfoCmd(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as GetFightInfoCmd);
		}

		[DebuggerNonUserCode]
		public bool Equals(GetFightInfoCmd other)
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
		public void MergeFrom(GetFightInfoCmd other)
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
			return "GetFightInfoCmd";
		}
	}
}
