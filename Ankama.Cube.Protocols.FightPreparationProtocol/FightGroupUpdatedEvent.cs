using Ankama.Cube.Protocols.PlayerProtocol;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Ankama.Cube.Protocols.FightPreparationProtocol
{
	public sealed class FightGroupUpdatedEvent : IMessage<FightGroupUpdatedEvent>, IMessage, IEquatable<FightGroupUpdatedEvent>, IDeepCloneable<FightGroupUpdatedEvent>, ICustomDiagnosticMessage
	{
		private static readonly MessageParser<FightGroupUpdatedEvent> _parser = new MessageParser<FightGroupUpdatedEvent>((Func<FightGroupUpdatedEvent>)(() => new FightGroupUpdatedEvent()));

		private UnknownFieldSet _unknownFields;

		public const int GroupRemovedFieldNumber = 1;

		private bool groupRemoved_;

		public const int MembersFieldNumber = 2;

		private static readonly FieldCodec<PlayerPublicInfo> _repeated_members_codec = FieldCodec.ForMessage<PlayerPublicInfo>(18u, PlayerPublicInfo.Parser);

		private readonly RepeatedField<PlayerPublicInfo> members_ = new RepeatedField<PlayerPublicInfo>();

		[DebuggerNonUserCode]
		public static MessageParser<FightGroupUpdatedEvent> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => FightPreparationProtocolReflection.Descriptor.get_MessageTypes()[2];

		[DebuggerNonUserCode]
		MessageDescriptor Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public bool GroupRemoved
		{
			get
			{
				return groupRemoved_;
			}
			set
			{
				groupRemoved_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<PlayerPublicInfo> Members => members_;

		[DebuggerNonUserCode]
		public FightGroupUpdatedEvent()
		{
		}

		[DebuggerNonUserCode]
		public FightGroupUpdatedEvent(FightGroupUpdatedEvent other)
			: this()
		{
			groupRemoved_ = other.groupRemoved_;
			members_ = other.members_.Clone();
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public FightGroupUpdatedEvent Clone()
		{
			return new FightGroupUpdatedEvent(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as FightGroupUpdatedEvent);
		}

		[DebuggerNonUserCode]
		public bool Equals(FightGroupUpdatedEvent other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (GroupRemoved != other.GroupRemoved)
			{
				return false;
			}
			if (!members_.Equals(other.members_))
			{
				return false;
			}
			return object.Equals(_unknownFields, other._unknownFields);
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (GroupRemoved)
			{
				num ^= GroupRemoved.GetHashCode();
			}
			num ^= ((object)members_).GetHashCode();
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
			if (GroupRemoved)
			{
				output.WriteRawTag((byte)8);
				output.WriteBool(GroupRemoved);
			}
			members_.WriteTo(output, _repeated_members_codec);
			if (_unknownFields != null)
			{
				_unknownFields.WriteTo(output);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (GroupRemoved)
			{
				num += 2;
			}
			num += members_.CalculateSize(_repeated_members_codec);
			if (_unknownFields != null)
			{
				num += _unknownFields.CalculateSize();
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(FightGroupUpdatedEvent other)
		{
			if (other != null)
			{
				if (other.GroupRemoved)
				{
					GroupRemoved = other.GroupRemoved;
				}
				members_.Add((IEnumerable<PlayerPublicInfo>)other.members_);
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
					GroupRemoved = input.ReadBool();
					break;
				case 18u:
					members_.AddEntriesFrom(input, _repeated_members_codec);
					break;
				}
			}
		}

		public string ToDiagnosticString()
		{
			return "FightGroupUpdatedEvent";
		}
	}
}
