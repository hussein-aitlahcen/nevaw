using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Ankama.Cube.Protocols.FightProtocol
{
	public sealed class FightSnapshotEvent : IMessage<FightSnapshotEvent>, IMessage, IEquatable<FightSnapshotEvent>, IDeepCloneable<FightSnapshotEvent>, ICustomDiagnosticMessage
	{
		private static readonly MessageParser<FightSnapshotEvent> _parser = new MessageParser<FightSnapshotEvent>((Func<FightSnapshotEvent>)(() => new FightSnapshotEvent()));

		private UnknownFieldSet _unknownFields;

		public const int FightsSnapshotsFieldNumber = 1;

		private static readonly FieldCodec<FightSnapshot> _repeated_fightsSnapshots_codec = FieldCodec.ForMessage<FightSnapshot>(10u, FightSnapshot.Parser);

		private readonly RepeatedField<FightSnapshot> fightsSnapshots_ = new RepeatedField<FightSnapshot>();

		public const int OwnFightIdFieldNumber = 2;

		private int ownFightId_;

		public const int OwnPlayerIdFieldNumber = 3;

		private int ownPlayerId_;

		public const int OwnSpellsIdsFieldNumber = 4;

		private static readonly FieldCodec<FightSnapshotSpell> _repeated_ownSpellsIds_codec = FieldCodec.ForMessage<FightSnapshotSpell>(34u, FightSnapshotSpell.Parser);

		private readonly RepeatedField<FightSnapshotSpell> ownSpellsIds_ = new RepeatedField<FightSnapshotSpell>();

		[DebuggerNonUserCode]
		public static MessageParser<FightSnapshotEvent> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => FightProtocolReflection.Descriptor.get_MessageTypes()[7];

		[DebuggerNonUserCode]
		MessageDescriptor Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public RepeatedField<FightSnapshot> FightsSnapshots => fightsSnapshots_;

		[DebuggerNonUserCode]
		public int OwnFightId
		{
			get
			{
				return ownFightId_;
			}
			set
			{
				ownFightId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int OwnPlayerId
		{
			get
			{
				return ownPlayerId_;
			}
			set
			{
				ownPlayerId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<FightSnapshotSpell> OwnSpellsIds => ownSpellsIds_;

		[DebuggerNonUserCode]
		public FightSnapshotEvent()
		{
		}

		[DebuggerNonUserCode]
		public FightSnapshotEvent(FightSnapshotEvent other)
			: this()
		{
			fightsSnapshots_ = other.fightsSnapshots_.Clone();
			ownFightId_ = other.ownFightId_;
			ownPlayerId_ = other.ownPlayerId_;
			ownSpellsIds_ = other.ownSpellsIds_.Clone();
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public FightSnapshotEvent Clone()
		{
			return new FightSnapshotEvent(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as FightSnapshotEvent);
		}

		[DebuggerNonUserCode]
		public bool Equals(FightSnapshotEvent other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (!fightsSnapshots_.Equals(other.fightsSnapshots_))
			{
				return false;
			}
			if (OwnFightId != other.OwnFightId)
			{
				return false;
			}
			if (OwnPlayerId != other.OwnPlayerId)
			{
				return false;
			}
			if (!ownSpellsIds_.Equals(other.ownSpellsIds_))
			{
				return false;
			}
			return object.Equals(_unknownFields, other._unknownFields);
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			num ^= ((object)fightsSnapshots_).GetHashCode();
			if (OwnFightId != 0)
			{
				num ^= OwnFightId.GetHashCode();
			}
			if (OwnPlayerId != 0)
			{
				num ^= OwnPlayerId.GetHashCode();
			}
			num ^= ((object)ownSpellsIds_).GetHashCode();
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
			fightsSnapshots_.WriteTo(output, _repeated_fightsSnapshots_codec);
			if (OwnFightId != 0)
			{
				output.WriteRawTag((byte)16);
				output.WriteInt32(OwnFightId);
			}
			if (OwnPlayerId != 0)
			{
				output.WriteRawTag((byte)24);
				output.WriteInt32(OwnPlayerId);
			}
			ownSpellsIds_.WriteTo(output, _repeated_ownSpellsIds_codec);
			if (_unknownFields != null)
			{
				_unknownFields.WriteTo(output);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			num += fightsSnapshots_.CalculateSize(_repeated_fightsSnapshots_codec);
			if (OwnFightId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(OwnFightId);
			}
			if (OwnPlayerId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(OwnPlayerId);
			}
			num += ownSpellsIds_.CalculateSize(_repeated_ownSpellsIds_codec);
			if (_unknownFields != null)
			{
				num += _unknownFields.CalculateSize();
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(FightSnapshotEvent other)
		{
			if (other != null)
			{
				fightsSnapshots_.Add((IEnumerable<FightSnapshot>)other.fightsSnapshots_);
				if (other.OwnFightId != 0)
				{
					OwnFightId = other.OwnFightId;
				}
				if (other.OwnPlayerId != 0)
				{
					OwnPlayerId = other.OwnPlayerId;
				}
				ownSpellsIds_.Add((IEnumerable<FightSnapshotSpell>)other.ownSpellsIds_);
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
				case 10u:
					fightsSnapshots_.AddEntriesFrom(input, _repeated_fightsSnapshots_codec);
					break;
				case 16u:
					OwnFightId = input.ReadInt32();
					break;
				case 24u:
					OwnPlayerId = input.ReadInt32();
					break;
				case 34u:
					ownSpellsIds_.AddEntriesFrom(input, _repeated_ownSpellsIds_codec);
					break;
				}
			}
		}

		public string ToDiagnosticString()
		{
			return "FightSnapshotEvent";
		}
	}
}
