using Google.Protobuf;
using Google.Protobuf.Reflection;
using System;
using System.Diagnostics;

namespace Ankama.Cube.Protocols.PlayerProtocol
{
	public sealed class SelectDeckInfo : IMessage<SelectDeckInfo>, IMessage, IEquatable<SelectDeckInfo>, IDeepCloneable<SelectDeckInfo>, ICustomDiagnosticMessage
	{
		private static readonly MessageParser<SelectDeckInfo> _parser = new MessageParser<SelectDeckInfo>((Func<SelectDeckInfo>)(() => new SelectDeckInfo()));

		private UnknownFieldSet _unknownFields;

		public const int WeaponIdFieldNumber = 1;

		private int weaponId_;

		public const int DeckIdFieldNumber = 2;

		private static readonly FieldCodec<int?> _single_deckId_codec = FieldCodec.ForStructWrapper<int>(18u);

		private int? deckId_;

		[DebuggerNonUserCode]
		public static MessageParser<SelectDeckInfo> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => PlayerProtocolReflection.Descriptor.get_MessageTypes()[12];

		[DebuggerNonUserCode]
		MessageDescriptor Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public int WeaponId
		{
			get
			{
				return weaponId_;
			}
			set
			{
				weaponId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int? DeckId
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
		public SelectDeckInfo()
		{
		}

		[DebuggerNonUserCode]
		public SelectDeckInfo(SelectDeckInfo other)
			: this()
		{
			weaponId_ = other.weaponId_;
			DeckId = other.DeckId;
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public SelectDeckInfo Clone()
		{
			return new SelectDeckInfo(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as SelectDeckInfo);
		}

		[DebuggerNonUserCode]
		public bool Equals(SelectDeckInfo other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (WeaponId != other.WeaponId)
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
			if (WeaponId != 0)
			{
				num ^= WeaponId.GetHashCode();
			}
			if (deckId_.HasValue)
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
			if (WeaponId != 0)
			{
				output.WriteRawTag((byte)8);
				output.WriteInt32(WeaponId);
			}
			if (deckId_.HasValue)
			{
				_single_deckId_codec.WriteTagAndValue(output, DeckId);
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
			if (WeaponId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(WeaponId);
			}
			if (deckId_.HasValue)
			{
				num += _single_deckId_codec.CalculateSizeWithTag(DeckId);
			}
			if (_unknownFields != null)
			{
				num += _unknownFields.CalculateSize();
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(SelectDeckInfo other)
		{
			if (other != null)
			{
				if (other.WeaponId != 0)
				{
					WeaponId = other.WeaponId;
				}
				if (other.deckId_.HasValue && (!deckId_.HasValue || other.DeckId != 0))
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
					WeaponId = input.ReadInt32();
					break;
				case 18u:
				{
					int? num2 = _single_deckId_codec.Read(input);
					if (!deckId_.HasValue || num2 != 0)
					{
						DeckId = num2;
					}
					break;
				}
				}
			}
		}

		public string ToDiagnosticString()
		{
			return "SelectDeckInfo";
		}
	}
}
