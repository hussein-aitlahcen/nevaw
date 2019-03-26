using Google.Protobuf;
using Google.Protobuf.Reflection;
using System;
using System.Diagnostics;

namespace Ankama.Cube.Protocols.PlayerProtocol
{
	public sealed class ChangeGodCmd : IMessage<ChangeGodCmd>, IMessage, IEquatable<ChangeGodCmd>, IDeepCloneable<ChangeGodCmd>, ICustomDiagnosticMessage
	{
		private static readonly MessageParser<ChangeGodCmd> _parser = new MessageParser<ChangeGodCmd>((Func<ChangeGodCmd>)(() => new ChangeGodCmd()));

		private UnknownFieldSet _unknownFields;

		public const int GodFieldNumber = 1;

		private int god_;

		[DebuggerNonUserCode]
		public static MessageParser<ChangeGodCmd> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => PlayerProtocolReflection.Descriptor.get_MessageTypes()[5];

		[DebuggerNonUserCode]
		MessageDescriptor Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public int God
		{
			get
			{
				return god_;
			}
			set
			{
				god_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ChangeGodCmd()
		{
		}

		[DebuggerNonUserCode]
		public ChangeGodCmd(ChangeGodCmd other)
			: this()
		{
			god_ = other.god_;
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public ChangeGodCmd Clone()
		{
			return new ChangeGodCmd(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as ChangeGodCmd);
		}

		[DebuggerNonUserCode]
		public bool Equals(ChangeGodCmd other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (God != other.God)
			{
				return false;
			}
			return object.Equals(_unknownFields, other._unknownFields);
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (God != 0)
			{
				num ^= God.GetHashCode();
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
			if (God != 0)
			{
				output.WriteRawTag((byte)8);
				output.WriteInt32(God);
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
			if (God != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(God);
			}
			if (_unknownFields != null)
			{
				num += _unknownFields.CalculateSize();
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(ChangeGodCmd other)
		{
			if (other != null)
			{
				if (other.God != 0)
				{
					God = other.God;
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
					God = input.ReadInt32();
				}
			}
		}

		public string ToDiagnosticString()
		{
			return "ChangeGodCmd";
		}
	}
}
