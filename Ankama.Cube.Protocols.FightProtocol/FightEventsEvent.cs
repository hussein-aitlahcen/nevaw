using Ankama.Cube.Fight.Events;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Ankama.Cube.Protocols.FightProtocol
{
	public sealed class FightEventsEvent : IMessage<FightEventsEvent>, IMessage, IEquatable<FightEventsEvent>, IDeepCloneable<FightEventsEvent>, ICustomDiagnosticMessage
	{
		private static readonly MessageParser<FightEventsEvent> _parser = new MessageParser<FightEventsEvent>((Func<FightEventsEvent>)(() => new FightEventsEvent()));

		private UnknownFieldSet _unknownFields;

		public const int FightIdFieldNumber = 1;

		private int fightId_;

		public const int EventsFieldNumber = 2;

		private static readonly FieldCodec<FightEventData> _repeated_events_codec = FieldCodec.ForMessage<FightEventData>(18u, FightEventData.Parser);

		private readonly RepeatedField<FightEventData> events_ = new RepeatedField<FightEventData>();

		[DebuggerNonUserCode]
		public static MessageParser<FightEventsEvent> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => FightProtocolReflection.Descriptor.get_MessageTypes()[19];

		[DebuggerNonUserCode]
		MessageDescriptor Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public int FightId
		{
			get
			{
				return fightId_;
			}
			set
			{
				fightId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<FightEventData> Events => events_;

		[DebuggerNonUserCode]
		public FightEventsEvent()
		{
		}

		[DebuggerNonUserCode]
		public FightEventsEvent(FightEventsEvent other)
			: this()
		{
			fightId_ = other.fightId_;
			events_ = other.events_.Clone();
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public FightEventsEvent Clone()
		{
			return new FightEventsEvent(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as FightEventsEvent);
		}

		[DebuggerNonUserCode]
		public bool Equals(FightEventsEvent other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (FightId != other.FightId)
			{
				return false;
			}
			if (!events_.Equals(other.events_))
			{
				return false;
			}
			return object.Equals(_unknownFields, other._unknownFields);
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (FightId != 0)
			{
				num ^= FightId.GetHashCode();
			}
			num ^= ((object)events_).GetHashCode();
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
			if (FightId != 0)
			{
				output.WriteRawTag((byte)8);
				output.WriteInt32(FightId);
			}
			events_.WriteTo(output, _repeated_events_codec);
			if (_unknownFields != null)
			{
				_unknownFields.WriteTo(output);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (FightId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(FightId);
			}
			num += events_.CalculateSize(_repeated_events_codec);
			if (_unknownFields != null)
			{
				num += _unknownFields.CalculateSize();
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(FightEventsEvent other)
		{
			if (other != null)
			{
				if (other.FightId != 0)
				{
					FightId = other.FightId;
				}
				events_.Add((IEnumerable<FightEventData>)other.events_);
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
					FightId = input.ReadInt32();
					break;
				case 18u:
					events_.AddEntriesFrom(input, _repeated_events_codec);
					break;
				}
			}
		}

		public string ToDiagnosticString()
		{
			return "FightEventsEvent";
		}
	}
}
