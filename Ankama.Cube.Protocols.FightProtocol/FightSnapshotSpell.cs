using Google.Protobuf;
using Google.Protobuf.Reflection;
using System;
using System.Diagnostics;

namespace Ankama.Cube.Protocols.FightProtocol
{
	public sealed class FightSnapshotSpell : IMessage<FightSnapshotSpell>, IMessage, IEquatable<FightSnapshotSpell>, IDeepCloneable<FightSnapshotSpell>, ICustomDiagnosticMessage
	{
		private static readonly MessageParser<FightSnapshotSpell> _parser = new MessageParser<FightSnapshotSpell>((Func<FightSnapshotSpell>)(() => new FightSnapshotSpell()));

		private UnknownFieldSet _unknownFields;

		public const int SpellDefIdFieldNumber = 1;

		private int spellDefId_;

		public const int SpellInstanceIdFieldNumber = 2;

		private int spellInstanceId_;

		[DebuggerNonUserCode]
		public static MessageParser<FightSnapshotSpell> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => FightProtocolReflection.Descriptor.get_MessageTypes()[8];

		[DebuggerNonUserCode]
		MessageDescriptor Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public int SpellDefId
		{
			get
			{
				return spellDefId_;
			}
			set
			{
				spellDefId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int SpellInstanceId
		{
			get
			{
				return spellInstanceId_;
			}
			set
			{
				spellInstanceId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public FightSnapshotSpell()
		{
		}

		[DebuggerNonUserCode]
		public FightSnapshotSpell(FightSnapshotSpell other)
			: this()
		{
			spellDefId_ = other.spellDefId_;
			spellInstanceId_ = other.spellInstanceId_;
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public FightSnapshotSpell Clone()
		{
			return new FightSnapshotSpell(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as FightSnapshotSpell);
		}

		[DebuggerNonUserCode]
		public bool Equals(FightSnapshotSpell other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (SpellDefId != other.SpellDefId)
			{
				return false;
			}
			if (SpellInstanceId != other.SpellInstanceId)
			{
				return false;
			}
			return object.Equals(_unknownFields, other._unknownFields);
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (SpellDefId != 0)
			{
				num ^= SpellDefId.GetHashCode();
			}
			if (SpellInstanceId != 0)
			{
				num ^= SpellInstanceId.GetHashCode();
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
			if (SpellDefId != 0)
			{
				output.WriteRawTag((byte)8);
				output.WriteInt32(SpellDefId);
			}
			if (SpellInstanceId != 0)
			{
				output.WriteRawTag((byte)16);
				output.WriteInt32(SpellInstanceId);
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
			if (SpellDefId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(SpellDefId);
			}
			if (SpellInstanceId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(SpellInstanceId);
			}
			if (_unknownFields != null)
			{
				num += _unknownFields.CalculateSize();
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(FightSnapshotSpell other)
		{
			if (other != null)
			{
				if (other.SpellDefId != 0)
				{
					SpellDefId = other.SpellDefId;
				}
				if (other.SpellInstanceId != 0)
				{
					SpellInstanceId = other.SpellInstanceId;
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
					SpellDefId = input.ReadInt32();
					break;
				case 16u:
					SpellInstanceId = input.ReadInt32();
					break;
				}
			}
		}

		public string ToDiagnosticString()
		{
			return "FightSnapshotSpell";
		}
	}
}
