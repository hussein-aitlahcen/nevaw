using Google.Protobuf;
using Google.Protobuf.Reflection;
using System;
using System.Diagnostics;

namespace Ankama.Cube.Protocols.PlayerProtocol
{
	public sealed class RemoveDeckCmd : IMessage<RemoveDeckCmd>, IMessage, IEquatable<RemoveDeckCmd>, IDeepCloneable<RemoveDeckCmd>, ICustomDiagnosticMessage
	{
		private static readonly MessageParser<RemoveDeckCmd> _parser = new MessageParser<RemoveDeckCmd>((Func<RemoveDeckCmd>)(() => new RemoveDeckCmd()));

		private UnknownFieldSet _unknownFields;

		public const int IdFieldNumber = 1;

		private int id_;

		[DebuggerNonUserCode]
		public static MessageParser<RemoveDeckCmd> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => PlayerProtocolReflection.Descriptor.get_MessageTypes()[9];

		[DebuggerNonUserCode]
		MessageDescriptor Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public int Id
		{
			get
			{
				return id_;
			}
			set
			{
				id_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RemoveDeckCmd()
		{
		}

		[DebuggerNonUserCode]
		public RemoveDeckCmd(RemoveDeckCmd other)
			: this()
		{
			id_ = other.id_;
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public RemoveDeckCmd Clone()
		{
			return new RemoveDeckCmd(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as RemoveDeckCmd);
		}

		[DebuggerNonUserCode]
		public bool Equals(RemoveDeckCmd other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (Id != other.Id)
			{
				return false;
			}
			return object.Equals(_unknownFields, other._unknownFields);
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (Id != 0)
			{
				num ^= Id.GetHashCode();
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
			if (Id != 0)
			{
				output.WriteRawTag((byte)8);
				output.WriteInt32(Id);
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
			if (Id != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Id);
			}
			if (_unknownFields != null)
			{
				num += _unknownFields.CalculateSize();
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(RemoveDeckCmd other)
		{
			if (other != null)
			{
				if (other.Id != 0)
				{
					Id = other.Id;
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
					Id = input.ReadInt32();
				}
			}
		}

		public string ToDiagnosticString()
		{
			return "RemoveDeckCmd";
		}
	}
}
