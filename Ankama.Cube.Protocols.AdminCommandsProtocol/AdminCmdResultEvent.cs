using Google.Protobuf;
using Google.Protobuf.Reflection;
using System;
using System.Diagnostics;

namespace Ankama.Cube.Protocols.AdminCommandsProtocol
{
	public sealed class AdminCmdResultEvent : IMessage<AdminCmdResultEvent>, IMessage, IEquatable<AdminCmdResultEvent>, IDeepCloneable<AdminCmdResultEvent>, ICustomDiagnosticMessage
	{
		private static readonly MessageParser<AdminCmdResultEvent> _parser = new MessageParser<AdminCmdResultEvent>((Func<AdminCmdResultEvent>)(() => new AdminCmdResultEvent()));

		private UnknownFieldSet _unknownFields;

		public const int IdFieldNumber = 1;

		private int id_;

		public const int SuccessFieldNumber = 2;

		private bool success_;

		public const int ResultFieldNumber = 3;

		private string result_ = "";

		[DebuggerNonUserCode]
		public static MessageParser<AdminCmdResultEvent> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => AdminCommandsProtocolReflection.Descriptor.get_MessageTypes()[1];

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
		public bool Success
		{
			get
			{
				return success_;
			}
			set
			{
				success_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string Result
		{
			get
			{
				return result_;
			}
			set
			{
				result_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public AdminCmdResultEvent()
		{
		}

		[DebuggerNonUserCode]
		public AdminCmdResultEvent(AdminCmdResultEvent other)
			: this()
		{
			id_ = other.id_;
			success_ = other.success_;
			result_ = other.result_;
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public AdminCmdResultEvent Clone()
		{
			return new AdminCmdResultEvent(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as AdminCmdResultEvent);
		}

		[DebuggerNonUserCode]
		public bool Equals(AdminCmdResultEvent other)
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
			if (Success != other.Success)
			{
				return false;
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
			if (Id != 0)
			{
				num ^= Id.GetHashCode();
			}
			if (Success)
			{
				num ^= Success.GetHashCode();
			}
			if (Result.Length != 0)
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
			if (Id != 0)
			{
				output.WriteRawTag((byte)8);
				output.WriteInt32(Id);
			}
			if (Success)
			{
				output.WriteRawTag((byte)16);
				output.WriteBool(Success);
			}
			if (Result.Length != 0)
			{
				output.WriteRawTag((byte)26);
				output.WriteString(Result);
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
			if (Success)
			{
				num += 2;
			}
			if (Result.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(Result);
			}
			if (_unknownFields != null)
			{
				num += _unknownFields.CalculateSize();
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(AdminCmdResultEvent other)
		{
			if (other != null)
			{
				if (other.Id != 0)
				{
					Id = other.Id;
				}
				if (other.Success)
				{
					Success = other.Success;
				}
				if (other.Result.Length != 0)
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
				switch (num)
				{
				default:
					_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
					break;
				case 8u:
					Id = input.ReadInt32();
					break;
				case 16u:
					Success = input.ReadBool();
					break;
				case 26u:
					Result = input.ReadString();
					break;
				}
			}
		}

		public string ToDiagnosticString()
		{
			return "AdminCmdResultEvent";
		}
	}
}
