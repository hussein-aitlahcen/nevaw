using Google.Protobuf;
using Google.Protobuf.Reflection;
using System;
using System.Diagnostics;

namespace Ankama.Cube.Protocols.FightCommonProtocol
{
	public sealed class SpellMovement : IMessage<SpellMovement>, IMessage, IEquatable<SpellMovement>, IDeepCloneable<SpellMovement>, ICustomDiagnosticMessage
	{
		private static readonly MessageParser<SpellMovement> _parser = new MessageParser<SpellMovement>((Func<SpellMovement>)(() => new SpellMovement()));

		private UnknownFieldSet _unknownFields;

		public const int SpellFieldNumber = 1;

		private SpellInfo spell_;

		public const int FromFieldNumber = 2;

		private SpellMovementZone from_;

		public const int ToFieldNumber = 3;

		private SpellMovementZone to_;

		public const int DiscardedBecauseHandWasFullFieldNumber = 4;

		private static readonly FieldCodec<bool?> _single_discardedBecauseHandWasFull_codec = FieldCodec.ForStructWrapper<bool>(34u);

		private bool? discardedBecauseHandWasFull_;

		[DebuggerNonUserCode]
		public static MessageParser<SpellMovement> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => FightCommonProtocolReflection.Descriptor.get_MessageTypes()[0];

		[DebuggerNonUserCode]
		MessageDescriptor Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public SpellInfo Spell
		{
			get
			{
				return spell_;
			}
			set
			{
				spell_ = value;
			}
		}

		[DebuggerNonUserCode]
		public SpellMovementZone From
		{
			get
			{
				return from_;
			}
			set
			{
				from_ = value;
			}
		}

		[DebuggerNonUserCode]
		public SpellMovementZone To
		{
			get
			{
				return to_;
			}
			set
			{
				to_ = value;
			}
		}

		[DebuggerNonUserCode]
		public bool? DiscardedBecauseHandWasFull
		{
			get
			{
				return discardedBecauseHandWasFull_;
			}
			set
			{
				discardedBecauseHandWasFull_ = value;
			}
		}

		[DebuggerNonUserCode]
		public SpellMovement()
		{
		}

		[DebuggerNonUserCode]
		public SpellMovement(SpellMovement other)
			: this()
		{
			spell_ = ((other.spell_ != null) ? other.spell_.Clone() : null);
			from_ = other.from_;
			to_ = other.to_;
			DiscardedBecauseHandWasFull = other.DiscardedBecauseHandWasFull;
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public SpellMovement Clone()
		{
			return new SpellMovement(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as SpellMovement);
		}

		[DebuggerNonUserCode]
		public bool Equals(SpellMovement other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (!object.Equals(Spell, other.Spell))
			{
				return false;
			}
			if (From != other.From)
			{
				return false;
			}
			if (To != other.To)
			{
				return false;
			}
			if (DiscardedBecauseHandWasFull != other.DiscardedBecauseHandWasFull)
			{
				return false;
			}
			return object.Equals(_unknownFields, other._unknownFields);
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (spell_ != null)
			{
				num ^= Spell.GetHashCode();
			}
			if (From != 0)
			{
				num ^= From.GetHashCode();
			}
			if (To != 0)
			{
				num ^= To.GetHashCode();
			}
			if (discardedBecauseHandWasFull_.HasValue)
			{
				num ^= DiscardedBecauseHandWasFull.GetHashCode();
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
			if (spell_ != null)
			{
				output.WriteRawTag((byte)10);
				output.WriteMessage(Spell);
			}
			if (From != 0)
			{
				output.WriteRawTag((byte)16);
				output.WriteEnum((int)From);
			}
			if (To != 0)
			{
				output.WriteRawTag((byte)24);
				output.WriteEnum((int)To);
			}
			if (discardedBecauseHandWasFull_.HasValue)
			{
				_single_discardedBecauseHandWasFull_codec.WriteTagAndValue(output, DiscardedBecauseHandWasFull);
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
			if (spell_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(Spell);
			}
			if (From != 0)
			{
				num += 1 + CodedOutputStream.ComputeEnumSize((int)From);
			}
			if (To != 0)
			{
				num += 1 + CodedOutputStream.ComputeEnumSize((int)To);
			}
			if (discardedBecauseHandWasFull_.HasValue)
			{
				num += _single_discardedBecauseHandWasFull_codec.CalculateSizeWithTag(DiscardedBecauseHandWasFull);
			}
			if (_unknownFields != null)
			{
				num += _unknownFields.CalculateSize();
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(SpellMovement other)
		{
			if (other == null)
			{
				return;
			}
			if (other.spell_ != null)
			{
				if (spell_ == null)
				{
					spell_ = new SpellInfo();
				}
				Spell.MergeFrom(other.Spell);
			}
			if (other.From != 0)
			{
				From = other.From;
			}
			if (other.To != 0)
			{
				To = other.To;
			}
			if (other.discardedBecauseHandWasFull_.HasValue && (!discardedBecauseHandWasFull_.HasValue || other.DiscardedBecauseHandWasFull != false))
			{
				DiscardedBecauseHandWasFull = other.DiscardedBecauseHandWasFull;
			}
			_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
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
					if (spell_ == null)
					{
						spell_ = new SpellInfo();
					}
					input.ReadMessage(spell_);
					break;
				case 16u:
					from_ = (SpellMovementZone)input.ReadEnum();
					break;
				case 24u:
					to_ = (SpellMovementZone)input.ReadEnum();
					break;
				case 34u:
				{
					bool? flag = _single_discardedBecauseHandWasFull_codec.Read(input);
					if (!discardedBecauseHandWasFull_.HasValue || flag != false)
					{
						DiscardedBecauseHandWasFull = flag;
					}
					break;
				}
				}
			}
		}

		public string ToDiagnosticString()
		{
			return "SpellMovement";
		}
	}
}
