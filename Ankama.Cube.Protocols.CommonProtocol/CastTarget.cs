using Ankama.Cube.Data;
using Ankama.Cube.Fight;
using Ankama.Cube.Fight.Entities;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using JetBrains.Annotations;
using System;
using System.Diagnostics;

namespace Ankama.Cube.Protocols.CommonProtocol
{
	public sealed class CastTarget : IMessage<CastTarget>, IMessage, IEquatable<CastTarget>, IDeepCloneable<CastTarget>, ICustomDiagnosticMessage
	{
		public enum ValueOneofCase
		{
			None,
			Cell,
			EntityId
		}

		private static readonly MessageParser<CastTarget> _parser = new MessageParser<CastTarget>((Func<CastTarget>)(() => new CastTarget()));

		private UnknownFieldSet _unknownFields;

		public const int CellFieldNumber = 1;

		public const int EntityIdFieldNumber = 2;

		private object value_;

		private ValueOneofCase valueCase_;

		[DebuggerNonUserCode]
		public static MessageParser<CastTarget> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => CommonProtocolReflection.Descriptor.get_MessageTypes()[0];

		[DebuggerNonUserCode]
		MessageDescriptor Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public CellCoord Cell
		{
			get
			{
				if (valueCase_ != ValueOneofCase.Cell)
				{
					return null;
				}
				return (CellCoord)value_;
			}
			set
			{
				value_ = value;
				valueCase_ = ((value != null) ? ValueOneofCase.Cell : ValueOneofCase.None);
			}
		}

		[DebuggerNonUserCode]
		public int EntityId
		{
			get
			{
				if (valueCase_ != ValueOneofCase.EntityId)
				{
					return 0;
				}
				return (int)value_;
			}
			set
			{
				value_ = value;
				valueCase_ = ValueOneofCase.EntityId;
			}
		}

		[DebuggerNonUserCode]
		public ValueOneofCase ValueCase => valueCase_;

		[DebuggerNonUserCode]
		public CastTarget()
		{
		}

		[DebuggerNonUserCode]
		public CastTarget(CastTarget other)
			: this()
		{
			switch (other.ValueCase)
			{
			case ValueOneofCase.Cell:
				Cell = other.Cell.Clone();
				break;
			case ValueOneofCase.EntityId:
				EntityId = other.EntityId;
				break;
			}
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public CastTarget Clone()
		{
			return new CastTarget(this);
		}

		[DebuggerNonUserCode]
		public void ClearValue()
		{
			valueCase_ = ValueOneofCase.None;
			value_ = null;
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as CastTarget);
		}

		[DebuggerNonUserCode]
		public bool Equals(CastTarget other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (!object.Equals(Cell, other.Cell))
			{
				return false;
			}
			if (EntityId != other.EntityId)
			{
				return false;
			}
			if (ValueCase != other.ValueCase)
			{
				return false;
			}
			return object.Equals(_unknownFields, other._unknownFields);
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (valueCase_ == ValueOneofCase.Cell)
			{
				num ^= Cell.GetHashCode();
			}
			if (valueCase_ == ValueOneofCase.EntityId)
			{
				num ^= EntityId.GetHashCode();
			}
			num ^= (int)valueCase_;
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
			if (valueCase_ == ValueOneofCase.Cell)
			{
				output.WriteRawTag((byte)10);
				output.WriteMessage(Cell);
			}
			if (valueCase_ == ValueOneofCase.EntityId)
			{
				output.WriteRawTag((byte)16);
				output.WriteInt32(EntityId);
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
			if (valueCase_ == ValueOneofCase.Cell)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(Cell);
			}
			if (valueCase_ == ValueOneofCase.EntityId)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(EntityId);
			}
			if (_unknownFields != null)
			{
				num += _unknownFields.CalculateSize();
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CastTarget other)
		{
			if (other == null)
			{
				return;
			}
			switch (other.ValueCase)
			{
			case ValueOneofCase.Cell:
				if (Cell == null)
				{
					Cell = new CellCoord();
				}
				Cell.MergeFrom(other.Cell);
				break;
			case ValueOneofCase.EntityId:
				EntityId = other.EntityId;
				break;
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
				{
					CellCoord cellCoord = new CellCoord();
					if (valueCase_ == ValueOneofCase.Cell)
					{
						cellCoord.MergeFrom(Cell);
					}
					input.ReadMessage(cellCoord);
					Cell = cellCoord;
					break;
				}
				case 16u:
					EntityId = input.ReadInt32();
					break;
				}
			}
		}

		public string ToDiagnosticString()
		{
			return "CastTarget";
		}

		[Pure]
		public Target ToTarget(FightStatus fightStatus)
		{
			switch (ValueCase)
			{
			case ValueOneofCase.Cell:
				return new Target((Coord)Cell);
			case ValueOneofCase.EntityId:
				return new Target(fightStatus.GetEntity(EntityId));
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
	}
}
