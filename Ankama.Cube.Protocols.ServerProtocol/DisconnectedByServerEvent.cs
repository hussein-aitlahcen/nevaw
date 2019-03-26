using Google.Protobuf;
using Google.Protobuf.Reflection;
using System;
using System.Diagnostics;

namespace Ankama.Cube.Protocols.ServerProtocol
{
	public sealed class DisconnectedByServerEvent : IMessage<DisconnectedByServerEvent>, IMessage, IEquatable<DisconnectedByServerEvent>, IDeepCloneable<DisconnectedByServerEvent>, ICustomDiagnosticMessage
	{
		[DebuggerNonUserCode]
		public static class Types
		{
			public enum Reason
			{
				[OriginalName("Unknown")]
				Unknown,
				[OriginalName("Error")]
				Error,
				[OriginalName("ServerIsStopping")]
				ServerIsStopping,
				[OriginalName("UnableToLoadAccount")]
				UnableToLoadAccount,
				[OriginalName("LoggedInAgainWithSameAccount")]
				LoggedInAgainWithSameAccount
			}
		}

		private static readonly MessageParser<DisconnectedByServerEvent> _parser = new MessageParser<DisconnectedByServerEvent>((Func<DisconnectedByServerEvent>)(() => new DisconnectedByServerEvent()));

		private UnknownFieldSet _unknownFields;

		public const int ReasonFieldNumber = 1;

		private Types.Reason reason_;

		[DebuggerNonUserCode]
		public static MessageParser<DisconnectedByServerEvent> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => ServerProtocolReflection.Descriptor.get_MessageTypes()[0];

		[DebuggerNonUserCode]
		MessageDescriptor Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public Types.Reason Reason
		{
			get
			{
				return reason_;
			}
			set
			{
				reason_ = value;
			}
		}

		[DebuggerNonUserCode]
		public DisconnectedByServerEvent()
		{
		}

		[DebuggerNonUserCode]
		public DisconnectedByServerEvent(DisconnectedByServerEvent other)
			: this()
		{
			reason_ = other.reason_;
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public DisconnectedByServerEvent Clone()
		{
			return new DisconnectedByServerEvent(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as DisconnectedByServerEvent);
		}

		[DebuggerNonUserCode]
		public bool Equals(DisconnectedByServerEvent other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (Reason != other.Reason)
			{
				return false;
			}
			return object.Equals(_unknownFields, other._unknownFields);
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (Reason != 0)
			{
				num ^= Reason.GetHashCode();
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
			if (Reason != 0)
			{
				output.WriteRawTag((byte)8);
				output.WriteEnum((int)Reason);
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
			if (Reason != 0)
			{
				num += 1 + CodedOutputStream.ComputeEnumSize((int)Reason);
			}
			if (_unknownFields != null)
			{
				num += _unknownFields.CalculateSize();
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(DisconnectedByServerEvent other)
		{
			if (other != null)
			{
				if (other.Reason != 0)
				{
					Reason = other.Reason;
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
					reason_ = (Types.Reason)input.ReadEnum();
				}
			}
		}

		public string ToDiagnosticString()
		{
			return "DisconnectedByServerEvent";
		}
	}
}
