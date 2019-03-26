using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Ankama.Cube.Protocols.PlayerProtocol
{
	public sealed class SelectDeckAndWeaponCmd : IMessage<SelectDeckAndWeaponCmd>, IMessage, IEquatable<SelectDeckAndWeaponCmd>, IDeepCloneable<SelectDeckAndWeaponCmd>, ICustomDiagnosticMessage
	{
		private static readonly MessageParser<SelectDeckAndWeaponCmd> _parser = new MessageParser<SelectDeckAndWeaponCmd>((Func<SelectDeckAndWeaponCmd>)(() => new SelectDeckAndWeaponCmd()));

		private UnknownFieldSet _unknownFields;

		public const int SelectedDecksFieldNumber = 1;

		private static readonly FieldCodec<SelectDeckInfo> _repeated_selectedDecks_codec = FieldCodec.ForMessage<SelectDeckInfo>(10u, SelectDeckInfo.Parser);

		private readonly RepeatedField<SelectDeckInfo> selectedDecks_ = new RepeatedField<SelectDeckInfo>();

		public const int SelectedWeaponFieldNumber = 2;

		private static readonly FieldCodec<int?> _single_selectedWeapon_codec = FieldCodec.ForStructWrapper<int>(18u);

		private int? selectedWeapon_;

		[DebuggerNonUserCode]
		public static MessageParser<SelectDeckAndWeaponCmd> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => PlayerProtocolReflection.Descriptor.get_MessageTypes()[11];

		[DebuggerNonUserCode]
		MessageDescriptor Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public RepeatedField<SelectDeckInfo> SelectedDecks => selectedDecks_;

		[DebuggerNonUserCode]
		public int? SelectedWeapon
		{
			get
			{
				return selectedWeapon_;
			}
			set
			{
				selectedWeapon_ = value;
			}
		}

		[DebuggerNonUserCode]
		public SelectDeckAndWeaponCmd()
		{
		}

		[DebuggerNonUserCode]
		public SelectDeckAndWeaponCmd(SelectDeckAndWeaponCmd other)
			: this()
		{
			selectedDecks_ = other.selectedDecks_.Clone();
			SelectedWeapon = other.SelectedWeapon;
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public SelectDeckAndWeaponCmd Clone()
		{
			return new SelectDeckAndWeaponCmd(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as SelectDeckAndWeaponCmd);
		}

		[DebuggerNonUserCode]
		public bool Equals(SelectDeckAndWeaponCmd other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (!selectedDecks_.Equals(other.selectedDecks_))
			{
				return false;
			}
			if (SelectedWeapon != other.SelectedWeapon)
			{
				return false;
			}
			return object.Equals(_unknownFields, other._unknownFields);
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			num ^= ((object)selectedDecks_).GetHashCode();
			if (selectedWeapon_.HasValue)
			{
				num ^= SelectedWeapon.GetHashCode();
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
			selectedDecks_.WriteTo(output, _repeated_selectedDecks_codec);
			if (selectedWeapon_.HasValue)
			{
				_single_selectedWeapon_codec.WriteTagAndValue(output, SelectedWeapon);
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
			num += selectedDecks_.CalculateSize(_repeated_selectedDecks_codec);
			if (selectedWeapon_.HasValue)
			{
				num += _single_selectedWeapon_codec.CalculateSizeWithTag(SelectedWeapon);
			}
			if (_unknownFields != null)
			{
				num += _unknownFields.CalculateSize();
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(SelectDeckAndWeaponCmd other)
		{
			if (other != null)
			{
				selectedDecks_.Add((IEnumerable<SelectDeckInfo>)other.selectedDecks_);
				if (other.selectedWeapon_.HasValue && (!selectedWeapon_.HasValue || other.SelectedWeapon != 0))
				{
					SelectedWeapon = other.SelectedWeapon;
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
				case 10u:
					selectedDecks_.AddEntriesFrom(input, _repeated_selectedDecks_codec);
					break;
				case 18u:
				{
					int? num2 = _single_selectedWeapon_codec.Read(input);
					if (!selectedWeapon_.HasValue || num2 != 0)
					{
						SelectedWeapon = num2;
					}
					break;
				}
				}
			}
		}

		public string ToDiagnosticString()
		{
			return "SelectDeckAndWeaponCmd";
		}
	}
}
