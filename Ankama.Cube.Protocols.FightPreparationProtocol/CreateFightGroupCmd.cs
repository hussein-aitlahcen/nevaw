using Google.Protobuf;
using Google.Protobuf.Reflection;
using System;
using System.Diagnostics;

namespace Ankama.Cube.Protocols.FightPreparationProtocol
{
	public sealed class CreateFightGroupCmd : IMessage<CreateFightGroupCmd>, IMessage, IEquatable<CreateFightGroupCmd>, IDeepCloneable<CreateFightGroupCmd>, ICustomDiagnosticMessage
	{
		private static readonly MessageParser<CreateFightGroupCmd> _parser = new MessageParser<CreateFightGroupCmd>((Func<CreateFightGroupCmd>)(() => new CreateFightGroupCmd()));

		private UnknownFieldSet _unknownFields;

		[DebuggerNonUserCode]
		public static MessageParser<CreateFightGroupCmd> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => FightPreparationProtocolReflection.Descriptor.get_MessageTypes()[0];

		[DebuggerNonUserCode]
		MessageDescriptor Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public CreateFightGroupCmd()
		{
		}

		[DebuggerNonUserCode]
		public CreateFightGroupCmd(CreateFightGroupCmd other)
			: this()
		{
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public CreateFightGroupCmd Clone()
		{
			return new CreateFightGroupCmd(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as CreateFightGroupCmd);
		}

		[DebuggerNonUserCode]
		public bool Equals(CreateFightGroupCmd other)
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
		public void MergeFrom(CreateFightGroupCmd other)
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
			return "CreateFightGroupCmd";
		}
	}
}
