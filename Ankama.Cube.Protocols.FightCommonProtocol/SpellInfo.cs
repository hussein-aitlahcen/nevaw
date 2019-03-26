using Google.Protobuf;
using Google.Protobuf.Reflection;
using System;
using System.Diagnostics;

namespace Ankama.Cube.Protocols.FightCommonProtocol
{
	public sealed class SpellInfo : IMessage<SpellInfo>, IMessage, IEquatable<SpellInfo>, IDeepCloneable<SpellInfo>, ICustomDiagnosticMessage
	{
		private static readonly MessageParser<SpellInfo> _parser = new MessageParser<SpellInfo>((Func<SpellInfo>)(() => new SpellInfo()));

		private UnknownFieldSet _unknownFields;

		public const int SpellInstanceIdFieldNumber = 1;

		private int spellInstanceId_;

		public const int SpellDefinitionIdFieldNumber = 2;

		private static readonly FieldCodec<int?> _single_spellDefinitionId_codec = FieldCodec.ForStructWrapper<int>(18u);

		private int? spellDefinitionId_;

		public const int SpellLevelFieldNumber = 3;

		private static readonly FieldCodec<int?> _single_spellLevel_codec = FieldCodec.ForStructWrapper<int>(26u);

		private int? spellLevel_;

		[DebuggerNonUserCode]
		public static MessageParser<SpellInfo> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => FightCommonProtocolReflection.Descriptor.get_MessageTypes()[1];

		[DebuggerNonUserCode]
		MessageDescriptor Descriptor => Descriptor;

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
		public int? SpellDefinitionId
		{
			get
			{
				return spellDefinitionId_;
			}
			set
			{
				spellDefinitionId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int? SpellLevel
		{
			get
			{
				return spellLevel_;
			}
			set
			{
				spellLevel_ = value;
			}
		}

		[DebuggerNonUserCode]
		public SpellInfo()
		{
		}

		[DebuggerNonUserCode]
		public SpellInfo(SpellInfo other)
			: this()
		{
			spellInstanceId_ = other.spellInstanceId_;
			SpellDefinitionId = other.SpellDefinitionId;
			SpellLevel = other.SpellLevel;
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public SpellInfo Clone()
		{
			return new SpellInfo(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as SpellInfo);
		}

		[DebuggerNonUserCode]
		public bool Equals(SpellInfo other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (SpellInstanceId != other.SpellInstanceId)
			{
				return false;
			}
			if (SpellDefinitionId != other.SpellDefinitionId)
			{
				return false;
			}
			if (SpellLevel != other.SpellLevel)
			{
				return false;
			}
			return object.Equals(_unknownFields, other._unknownFields);
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (SpellInstanceId != 0)
			{
				num ^= SpellInstanceId.GetHashCode();
			}
			if (spellDefinitionId_.HasValue)
			{
				num ^= SpellDefinitionId.GetHashCode();
			}
			if (spellLevel_.HasValue)
			{
				num ^= SpellLevel.GetHashCode();
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
			if (SpellInstanceId != 0)
			{
				output.WriteRawTag((byte)8);
				output.WriteInt32(SpellInstanceId);
			}
			if (spellDefinitionId_.HasValue)
			{
				_single_spellDefinitionId_codec.WriteTagAndValue(output, SpellDefinitionId);
			}
			if (spellLevel_.HasValue)
			{
				_single_spellLevel_codec.WriteTagAndValue(output, SpellLevel);
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
			if (SpellInstanceId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(SpellInstanceId);
			}
			if (spellDefinitionId_.HasValue)
			{
				num += _single_spellDefinitionId_codec.CalculateSizeWithTag(SpellDefinitionId);
			}
			if (spellLevel_.HasValue)
			{
				num += _single_spellLevel_codec.CalculateSizeWithTag(SpellLevel);
			}
			if (_unknownFields != null)
			{
				num += _unknownFields.CalculateSize();
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(SpellInfo other)
		{
			if (other != null)
			{
				if (other.SpellInstanceId != 0)
				{
					SpellInstanceId = other.SpellInstanceId;
				}
				if (other.spellDefinitionId_.HasValue && (!spellDefinitionId_.HasValue || other.SpellDefinitionId != 0))
				{
					SpellDefinitionId = other.SpellDefinitionId;
				}
				if (other.spellLevel_.HasValue && (!spellLevel_.HasValue || other.SpellLevel != 0))
				{
					SpellLevel = other.SpellLevel;
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
					SpellInstanceId = input.ReadInt32();
					break;
				case 18u:
				{
					int? num3 = _single_spellDefinitionId_codec.Read(input);
					if (!spellDefinitionId_.HasValue || num3 != 0)
					{
						SpellDefinitionId = num3;
					}
					break;
				}
				case 26u:
				{
					int? num2 = _single_spellLevel_codec.Read(input);
					if (!spellLevel_.HasValue || num2 != 0)
					{
						SpellLevel = num2;
					}
					break;
				}
				}
			}
		}

		public string ToDiagnosticString()
		{
			return "SpellInfo";
		}
	}
}
