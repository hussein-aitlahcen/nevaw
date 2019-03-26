using Google.Protobuf;
using Google.Protobuf.Reflection;
using System;
using System.Diagnostics;

namespace Ankama.Cube.Protocols.FightProtocol
{
	public sealed class GiveCompanionCmd : IMessage<GiveCompanionCmd>, IMessage, IEquatable<GiveCompanionCmd>, IDeepCloneable<GiveCompanionCmd>, ICustomDiagnosticMessage
	{
		private static readonly MessageParser<GiveCompanionCmd> _parser = new MessageParser<GiveCompanionCmd>((Func<GiveCompanionCmd>)(() => new GiveCompanionCmd()));

		private UnknownFieldSet _unknownFields;

		public const int CompanionDefIdFieldNumber = 1;

		private int companionDefId_;

		public const int TargetFightIdFieldNumber = 2;

		private int targetFightId_;

		public const int TargetPlayerIdFieldNumber = 3;

		private int targetPlayerId_;

		[DebuggerNonUserCode]
		public static MessageParser<GiveCompanionCmd> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => FightProtocolReflection.Descriptor.get_MessageTypes()[15];

		[DebuggerNonUserCode]
		MessageDescriptor Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public int CompanionDefId
		{
			get
			{
				return companionDefId_;
			}
			set
			{
				companionDefId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int TargetFightId
		{
			get
			{
				return targetFightId_;
			}
			set
			{
				targetFightId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int TargetPlayerId
		{
			get
			{
				return targetPlayerId_;
			}
			set
			{
				targetPlayerId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public GiveCompanionCmd()
		{
		}

		[DebuggerNonUserCode]
		public GiveCompanionCmd(GiveCompanionCmd other)
			: this()
		{
			companionDefId_ = other.companionDefId_;
			targetFightId_ = other.targetFightId_;
			targetPlayerId_ = other.targetPlayerId_;
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public GiveCompanionCmd Clone()
		{
			return new GiveCompanionCmd(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as GiveCompanionCmd);
		}

		[DebuggerNonUserCode]
		public bool Equals(GiveCompanionCmd other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (CompanionDefId != other.CompanionDefId)
			{
				return false;
			}
			if (TargetFightId != other.TargetFightId)
			{
				return false;
			}
			if (TargetPlayerId != other.TargetPlayerId)
			{
				return false;
			}
			return object.Equals(_unknownFields, other._unknownFields);
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (CompanionDefId != 0)
			{
				num ^= CompanionDefId.GetHashCode();
			}
			if (TargetFightId != 0)
			{
				num ^= TargetFightId.GetHashCode();
			}
			if (TargetPlayerId != 0)
			{
				num ^= TargetPlayerId.GetHashCode();
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
			if (CompanionDefId != 0)
			{
				output.WriteRawTag((byte)8);
				output.WriteInt32(CompanionDefId);
			}
			if (TargetFightId != 0)
			{
				output.WriteRawTag((byte)16);
				output.WriteInt32(TargetFightId);
			}
			if (TargetPlayerId != 0)
			{
				output.WriteRawTag((byte)24);
				output.WriteInt32(TargetPlayerId);
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
			if (CompanionDefId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(CompanionDefId);
			}
			if (TargetFightId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(TargetFightId);
			}
			if (TargetPlayerId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(TargetPlayerId);
			}
			if (_unknownFields != null)
			{
				num += _unknownFields.CalculateSize();
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(GiveCompanionCmd other)
		{
			if (other != null)
			{
				if (other.CompanionDefId != 0)
				{
					CompanionDefId = other.CompanionDefId;
				}
				if (other.TargetFightId != 0)
				{
					TargetFightId = other.TargetFightId;
				}
				if (other.TargetPlayerId != 0)
				{
					TargetPlayerId = other.TargetPlayerId;
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
					CompanionDefId = input.ReadInt32();
					break;
				case 16u:
					TargetFightId = input.ReadInt32();
					break;
				case 24u:
					TargetPlayerId = input.ReadInt32();
					break;
				}
			}
		}

		public string ToDiagnosticString()
		{
			return "GiveCompanionCmd";
		}
	}
}
