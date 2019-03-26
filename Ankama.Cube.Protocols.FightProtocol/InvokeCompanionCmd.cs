using Ankama.Cube.Protocols.CommonProtocol;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using System;
using System.Diagnostics;

namespace Ankama.Cube.Protocols.FightProtocol
{
	public sealed class InvokeCompanionCmd : IMessage<InvokeCompanionCmd>, IMessage, IEquatable<InvokeCompanionCmd>, IDeepCloneable<InvokeCompanionCmd>, ICustomDiagnosticMessage
	{
		private static readonly MessageParser<InvokeCompanionCmd> _parser = new MessageParser<InvokeCompanionCmd>((Func<InvokeCompanionCmd>)(() => new InvokeCompanionCmd()));

		private UnknownFieldSet _unknownFields;

		public const int CompanionDefIdFieldNumber = 1;

		private int companionDefId_;

		public const int CoordsFieldNumber = 2;

		private CellCoord coords_;

		[DebuggerNonUserCode]
		public static MessageParser<InvokeCompanionCmd> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => FightProtocolReflection.Descriptor.get_MessageTypes()[14];

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
		public CellCoord Coords
		{
			get
			{
				return coords_;
			}
			set
			{
				coords_ = value;
			}
		}

		[DebuggerNonUserCode]
		public InvokeCompanionCmd()
		{
		}

		[DebuggerNonUserCode]
		public InvokeCompanionCmd(InvokeCompanionCmd other)
			: this()
		{
			companionDefId_ = other.companionDefId_;
			coords_ = ((other.coords_ != null) ? other.coords_.Clone() : null);
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public InvokeCompanionCmd Clone()
		{
			return new InvokeCompanionCmd(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as InvokeCompanionCmd);
		}

		[DebuggerNonUserCode]
		public bool Equals(InvokeCompanionCmd other)
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
			if (!object.Equals(Coords, other.Coords))
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
			if (coords_ != null)
			{
				num ^= Coords.GetHashCode();
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
			if (coords_ != null)
			{
				output.WriteRawTag((byte)18);
				output.WriteMessage(Coords);
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
			if (coords_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(Coords);
			}
			if (_unknownFields != null)
			{
				num += _unknownFields.CalculateSize();
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(InvokeCompanionCmd other)
		{
			if (other == null)
			{
				return;
			}
			if (other.CompanionDefId != 0)
			{
				CompanionDefId = other.CompanionDefId;
			}
			if (other.coords_ != null)
			{
				if (coords_ == null)
				{
					coords_ = new CellCoord();
				}
				Coords.MergeFrom(other.Coords);
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
				case 8u:
					CompanionDefId = input.ReadInt32();
					break;
				case 18u:
					if (coords_ == null)
					{
						coords_ = new CellCoord();
					}
					input.ReadMessage(coords_);
					break;
				}
			}
		}

		public string ToDiagnosticString()
		{
			return "InvokeCompanionCmd";
		}
	}
}
