using Ankama.Cube.Protocols.CommonProtocol;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using System;
using System.Diagnostics;

namespace Ankama.Cube.Protocols.PlayerProtocol
{
	public sealed class SaveDeckResultEvent : IMessage<SaveDeckResultEvent>, IMessage, IEquatable<SaveDeckResultEvent>, IDeepCloneable<SaveDeckResultEvent>, ICustomDiagnosticMessage
	{
		private static readonly MessageParser<SaveDeckResultEvent> _parser = new MessageParser<SaveDeckResultEvent>((Func<SaveDeckResultEvent>)(() => new SaveDeckResultEvent()));

		private UnknownFieldSet _unknownFields;

		public const int ResultFieldNumber = 1;

		private CmdResult result_;

		public const int DeckIdFieldNumber = 2;

		private int deckId_;

		[DebuggerNonUserCode]
		public static MessageParser<SaveDeckResultEvent> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => PlayerProtocolReflection.Descriptor.get_MessageTypes()[8];

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
		public int DeckId
		{
			get
			{
				return deckId_;
			}
			set
			{
				deckId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public SaveDeckResultEvent()
		{
		}

		[DebuggerNonUserCode]
		public SaveDeckResultEvent(SaveDeckResultEvent other)
			: this()
		{
			result_ = other.result_;
			deckId_ = other.deckId_;
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public SaveDeckResultEvent Clone()
		{
			return new SaveDeckResultEvent(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as SaveDeckResultEvent);
		}

		[DebuggerNonUserCode]
		public bool Equals(SaveDeckResultEvent other)
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
			if (DeckId != other.DeckId)
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
			if (DeckId != 0)
			{
				num ^= DeckId.GetHashCode();
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
			if (DeckId != 0)
			{
				output.WriteRawTag((byte)16);
				output.WriteInt32(DeckId);
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
			if (DeckId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(DeckId);
			}
			if (_unknownFields != null)
			{
				num += _unknownFields.CalculateSize();
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(SaveDeckResultEvent other)
		{
			if (other != null)
			{
				if (other.Result != 0)
				{
					Result = other.Result;
				}
				if (other.DeckId != 0)
				{
					DeckId = other.DeckId;
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
					result_ = (CmdResult)input.ReadEnum();
					break;
				case 16u:
					DeckId = input.ReadInt32();
					break;
				}
			}
		}

		public string ToDiagnosticString()
		{
			return "SaveDeckResultEvent";
		}
	}
}
