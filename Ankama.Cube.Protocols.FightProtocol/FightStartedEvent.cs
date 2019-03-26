using Ankama.Cube.Protocols.FightCommonProtocol;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using System;
using System.Diagnostics;

namespace Ankama.Cube.Protocols.FightProtocol
{
	public sealed class FightStartedEvent : IMessage<FightStartedEvent>, IMessage, IEquatable<FightStartedEvent>, IDeepCloneable<FightStartedEvent>, ICustomDiagnosticMessage
	{
		private static readonly MessageParser<FightStartedEvent> _parser = new MessageParser<FightStartedEvent>((Func<FightStartedEvent>)(() => new FightStartedEvent()));

		private UnknownFieldSet _unknownFields;

		public const int FightInfoFieldNumber = 1;

		private FightInfo fightInfo_;

		public const int FightDefIdFieldNumber = 2;

		private int fightDefId_;

		public const int FightTypeFieldNumber = 3;

		private int fightType_;

		[DebuggerNonUserCode]
		public static MessageParser<FightStartedEvent> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => FightProtocolReflection.Descriptor.get_MessageTypes()[0];

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
		public int FightDefId
		{
			get
			{
				return fightDefId_;
			}
			set
			{
				fightDefId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int FightType
		{
			get
			{
				return fightType_;
			}
			set
			{
				fightType_ = value;
			}
		}

		[DebuggerNonUserCode]
		public FightStartedEvent()
		{
		}

		[DebuggerNonUserCode]
		public FightStartedEvent(FightStartedEvent other)
			: this()
		{
			fightInfo_ = ((other.fightInfo_ != null) ? other.fightInfo_.Clone() : null);
			fightDefId_ = other.fightDefId_;
			fightType_ = other.fightType_;
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public FightStartedEvent Clone()
		{
			return new FightStartedEvent(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as FightStartedEvent);
		}

		[DebuggerNonUserCode]
		public bool Equals(FightStartedEvent other)
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
			if (FightDefId != other.FightDefId)
			{
				return false;
			}
			if (FightType != other.FightType)
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
			if (FightDefId != 0)
			{
				num ^= FightDefId.GetHashCode();
			}
			if (FightType != 0)
			{
				num ^= FightType.GetHashCode();
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
			if (FightDefId != 0)
			{
				output.WriteRawTag((byte)16);
				output.WriteInt32(FightDefId);
			}
			if (FightType != 0)
			{
				output.WriteRawTag((byte)24);
				output.WriteInt32(FightType);
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
			if (FightDefId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(FightDefId);
			}
			if (FightType != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(FightType);
			}
			if (_unknownFields != null)
			{
				num += _unknownFields.CalculateSize();
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(FightStartedEvent other)
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
			if (other.FightDefId != 0)
			{
				FightDefId = other.FightDefId;
			}
			if (other.FightType != 0)
			{
				FightType = other.FightType;
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
					if (fightInfo_ == null)
					{
						fightInfo_ = new FightInfo();
					}
					input.ReadMessage(fightInfo_);
					break;
				case 16u:
					FightDefId = input.ReadInt32();
					break;
				case 24u:
					FightType = input.ReadInt32();
					break;
				}
			}
		}

		public string ToDiagnosticString()
		{
			return "FightStartedEvent";
		}
	}
}
