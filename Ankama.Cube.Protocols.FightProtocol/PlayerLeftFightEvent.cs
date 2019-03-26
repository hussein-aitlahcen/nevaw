using Google.Protobuf;
using Google.Protobuf.Reflection;
using System;
using System.Diagnostics;

namespace Ankama.Cube.Protocols.FightProtocol
{
	public sealed class PlayerLeftFightEvent : IMessage<PlayerLeftFightEvent>, IMessage, IEquatable<PlayerLeftFightEvent>, IDeepCloneable<PlayerLeftFightEvent>, ICustomDiagnosticMessage
	{
		private static readonly MessageParser<PlayerLeftFightEvent> _parser = new MessageParser<PlayerLeftFightEvent>((Func<PlayerLeftFightEvent>)(() => new PlayerLeftFightEvent()));

		private UnknownFieldSet _unknownFields;

		public const int FightIdFieldNumber = 1;

		private int fightId_;

		public const int PlayerIdFieldNumber = 2;

		private int playerId_;

		[DebuggerNonUserCode]
		public static MessageParser<PlayerLeftFightEvent> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => FightProtocolReflection.Descriptor.get_MessageTypes()[10];

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
		public int PlayerId
		{
			get
			{
				return playerId_;
			}
			set
			{
				playerId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public PlayerLeftFightEvent()
		{
		}

		[DebuggerNonUserCode]
		public PlayerLeftFightEvent(PlayerLeftFightEvent other)
			: this()
		{
			fightId_ = other.fightId_;
			playerId_ = other.playerId_;
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public PlayerLeftFightEvent Clone()
		{
			return new PlayerLeftFightEvent(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as PlayerLeftFightEvent);
		}

		[DebuggerNonUserCode]
		public bool Equals(PlayerLeftFightEvent other)
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
			if (PlayerId != other.PlayerId)
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
			if (PlayerId != 0)
			{
				num ^= PlayerId.GetHashCode();
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
			if (FightId != 0)
			{
				output.WriteRawTag((byte)8);
				output.WriteInt32(FightId);
			}
			if (PlayerId != 0)
			{
				output.WriteRawTag((byte)16);
				output.WriteInt32(PlayerId);
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
			if (FightId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(FightId);
			}
			if (PlayerId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(PlayerId);
			}
			if (_unknownFields != null)
			{
				num += _unknownFields.CalculateSize();
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(PlayerLeftFightEvent other)
		{
			if (other != null)
			{
				if (other.FightId != 0)
				{
					FightId = other.FightId;
				}
				if (other.PlayerId != 0)
				{
					PlayerId = other.PlayerId;
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
					FightId = input.ReadInt32();
					break;
				case 16u:
					PlayerId = input.ReadInt32();
					break;
				}
			}
		}

		public string ToDiagnosticString()
		{
			return "PlayerLeftFightEvent";
		}
	}
}
