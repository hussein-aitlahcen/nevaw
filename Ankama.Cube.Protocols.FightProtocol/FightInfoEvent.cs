using Ankama.Cube.Protocols.FightCommonProtocol;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using System;
using System.Diagnostics;

namespace Ankama.Cube.Protocols.FightProtocol
{
	public sealed class FightInfoEvent : IMessage<FightInfoEvent>, IMessage, IEquatable<FightInfoEvent>, IDeepCloneable<FightInfoEvent>, ICustomDiagnosticMessage
	{
		private static readonly MessageParser<FightInfoEvent> _parser = new MessageParser<FightInfoEvent>((Func<FightInfoEvent>)(() => new FightInfoEvent()));

		private UnknownFieldSet _unknownFields;

		public const int FightInfoFieldNumber = 1;

		private FightInfo fightInfo_;

		[DebuggerNonUserCode]
		public static MessageParser<FightInfoEvent> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => FightProtocolReflection.Descriptor.get_MessageTypes()[5];

		[DebuggerNonUserCode]
		MessageDescriptor Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public FightInfo FightInfo
		{
			get
			{
				return fightInfo_;
			}
			set
			{
				fightInfo_ = value;
			}
		}

		[DebuggerNonUserCode]
		public FightInfoEvent()
		{
		}

		[DebuggerNonUserCode]
		public FightInfoEvent(FightInfoEvent other)
			: this()
		{
			fightInfo_ = ((other.fightInfo_ != null) ? other.fightInfo_.Clone() : null);
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public FightInfoEvent Clone()
		{
			return new FightInfoEvent(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as FightInfoEvent);
		}

		[DebuggerNonUserCode]
		public bool Equals(FightInfoEvent other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (!object.Equals(FightInfo, other.FightInfo))
			{
				return false;
			}
			return object.Equals(_unknownFields, other._unknownFields);
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (fightInfo_ != null)
			{
				num ^= FightInfo.GetHashCode();
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
			if (fightInfo_ != null)
			{
				output.WriteRawTag((byte)10);
				output.WriteMessage(FightInfo);
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
			if (fightInfo_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(FightInfo);
			}
			if (_unknownFields != null)
			{
				num += _unknownFields.CalculateSize();
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(FightInfoEvent other)
		{
			if (other == null)
			{
				return;
			}
			if (other.fightInfo_ != null)
			{
				if (fightInfo_ == null)
				{
					fightInfo_ = new FightInfo();
				}
				FightInfo.MergeFrom(other.FightInfo);
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
				if (fightInfo_ == null)
				{
					fightInfo_ = new FightInfo();
				}
				input.ReadMessage(fightInfo_);
			}
		}

		public string ToDiagnosticString()
		{
			return "FightInfoEvent";
		}
	}
}
