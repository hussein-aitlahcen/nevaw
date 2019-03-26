using Ankama.Cube.Protocols.CommonProtocol;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Ankama.Cube.Protocols.FightProtocol
{
	public sealed class PlaySpellCmd : IMessage<PlaySpellCmd>, IMessage, IEquatable<PlaySpellCmd>, IDeepCloneable<PlaySpellCmd>, ICustomDiagnosticMessage
	{
		private static readonly MessageParser<PlaySpellCmd> _parser = new MessageParser<PlaySpellCmd>((Func<PlaySpellCmd>)(() => new PlaySpellCmd()));

		private UnknownFieldSet _unknownFields;

		public const int SpellIdFieldNumber = 1;

		private int spellId_;

		public const int CastTargetsFieldNumber = 2;

		private static readonly FieldCodec<CastTarget> _repeated_castTargets_codec = FieldCodec.ForMessage<CastTarget>(18u, CastTarget.Parser);

		private readonly RepeatedField<CastTarget> castTargets_ = new RepeatedField<CastTarget>();

		[DebuggerNonUserCode]
		public static MessageParser<PlaySpellCmd> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => FightProtocolReflection.Descriptor.get_MessageTypes()[13];

		[DebuggerNonUserCode]
		MessageDescriptor Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public int SpellId
		{
			get
			{
				return spellId_;
			}
			set
			{
				spellId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<CastTarget> CastTargets => castTargets_;

		[DebuggerNonUserCode]
		public PlaySpellCmd()
		{
		}

		[DebuggerNonUserCode]
		public PlaySpellCmd(PlaySpellCmd other)
			: this()
		{
			spellId_ = other.spellId_;
			castTargets_ = other.castTargets_.Clone();
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public PlaySpellCmd Clone()
		{
			return new PlaySpellCmd(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as PlaySpellCmd);
		}

		[DebuggerNonUserCode]
		public bool Equals(PlaySpellCmd other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (SpellId != other.SpellId)
			{
				return false;
			}
			if (!castTargets_.Equals(other.castTargets_))
			{
				return false;
			}
			return object.Equals(_unknownFields, other._unknownFields);
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (SpellId != 0)
			{
				num ^= SpellId.GetHashCode();
			}
			num ^= ((object)castTargets_).GetHashCode();
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
			if (SpellId != 0)
			{
				output.WriteRawTag((byte)8);
				output.WriteInt32(SpellId);
			}
			castTargets_.WriteTo(output, _repeated_castTargets_codec);
			if (_unknownFields != null)
			{
				_unknownFields.WriteTo(output);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (SpellId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(SpellId);
			}
			num += castTargets_.CalculateSize(_repeated_castTargets_codec);
			if (_unknownFields != null)
			{
				num += _unknownFields.CalculateSize();
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(PlaySpellCmd other)
		{
			if (other != null)
			{
				if (other.SpellId != 0)
				{
					SpellId = other.SpellId;
				}
				castTargets_.Add((IEnumerable<CastTarget>)other.castTargets_);
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
					SpellId = input.ReadInt32();
					break;
				case 18u:
					castTargets_.AddEntriesFrom(input, _repeated_castTargets_codec);
					break;
				}
			}
		}

		public string ToDiagnosticString()
		{
			return "PlaySpellCmd";
		}
	}
}
