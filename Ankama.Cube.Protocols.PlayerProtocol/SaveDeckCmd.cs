using Google.Protobuf;
using Google.Protobuf.Reflection;
using System;
using System.Diagnostics;

namespace Ankama.Cube.Protocols.PlayerProtocol
{
	public sealed class SaveDeckCmd : IMessage<SaveDeckCmd>, IMessage, IEquatable<SaveDeckCmd>, IDeepCloneable<SaveDeckCmd>, ICustomDiagnosticMessage
	{
		private static readonly MessageParser<SaveDeckCmd> _parser = new MessageParser<SaveDeckCmd>((Func<SaveDeckCmd>)(() => new SaveDeckCmd()));

		private UnknownFieldSet _unknownFields;

		public const int InfoFieldNumber = 1;

		private DeckInfo info_;

		[DebuggerNonUserCode]
		public static MessageParser<SaveDeckCmd> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => PlayerProtocolReflection.Descriptor.get_MessageTypes()[7];

		[DebuggerNonUserCode]
		MessageDescriptor Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public DeckInfo Info
		{
			get
			{
				return info_;
			}
			set
			{
				info_ = value;
			}
		}

		[DebuggerNonUserCode]
		public SaveDeckCmd()
		{
		}

		[DebuggerNonUserCode]
		public SaveDeckCmd(SaveDeckCmd other)
			: this()
		{
			info_ = ((other.info_ != null) ? other.info_.Clone() : null);
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public SaveDeckCmd Clone()
		{
			return new SaveDeckCmd(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as SaveDeckCmd);
		}

		[DebuggerNonUserCode]
		public bool Equals(SaveDeckCmd other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (!object.Equals(Info, other.Info))
			{
				return false;
			}
			return object.Equals(_unknownFields, other._unknownFields);
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (info_ != null)
			{
				num ^= Info.GetHashCode();
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
			if (info_ != null)
			{
				output.WriteRawTag((byte)10);
				output.WriteMessage(Info);
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
			if (info_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(Info);
			}
			if (_unknownFields != null)
			{
				num += _unknownFields.CalculateSize();
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(SaveDeckCmd other)
		{
			if (other == null)
			{
				return;
			}
			if (other.info_ != null)
			{
				if (info_ == null)
				{
					info_ = new DeckInfo();
				}
				Info.MergeFrom(other.Info);
			}
			_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0)
			{
				if (num != 10)
				{
					_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
					continue;
				}
				if (info_ == null)
				{
					info_ = new DeckInfo();
				}
				input.ReadMessage(info_);
			}
		}

		public string ToDiagnosticString()
		{
			return "SaveDeckCmd";
		}
	}
}
