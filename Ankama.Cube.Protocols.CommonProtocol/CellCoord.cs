using Ankama.Cube.Data;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using JetBrains.Annotations;
using System;
using System.Diagnostics;
using UnityEngine;

namespace Ankama.Cube.Protocols.CommonProtocol
{
	public sealed class CellCoord : IMessage<CellCoord>, IMessage, IEquatable<CellCoord>, IDeepCloneable<CellCoord>, ICustomDiagnosticMessage
	{
		private static readonly MessageParser<CellCoord> _parser = new MessageParser<CellCoord>((Func<CellCoord>)(() => new CellCoord()));

		private UnknownFieldSet _unknownFields;

		public const int XFieldNumber = 1;

		private int x_;

		public const int YFieldNumber = 2;

		private int y_;

		[DebuggerNonUserCode]
		public static MessageParser<CellCoord> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => CommonProtocolReflection.Descriptor.get_MessageTypes()[1];

		[DebuggerNonUserCode]
		MessageDescriptor Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public int X
		{
			get
			{
				return x_;
			}
			set
			{
				x_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Y
		{
			get
			{
				return y_;
			}
			set
			{
				y_ = value;
			}
		}

		[DebuggerNonUserCode]
		public CellCoord()
		{
		}

		[DebuggerNonUserCode]
		public CellCoord(CellCoord other)
			: this()
		{
			x_ = other.x_;
			y_ = other.y_;
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public CellCoord Clone()
		{
			return new CellCoord(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as CellCoord);
		}

		[DebuggerNonUserCode]
		public bool Equals(CellCoord other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (X != other.X)
			{
				return false;
			}
			if (Y != other.Y)
			{
				return false;
			}
			return object.Equals(_unknownFields, other._unknownFields);
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (X != 0)
			{
				num ^= X.GetHashCode();
			}
			if (Y != 0)
			{
				num ^= Y.GetHashCode();
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
			if (X != 0)
			{
				output.WriteRawTag((byte)8);
				output.WriteInt32(X);
			}
			if (Y != 0)
			{
				output.WriteRawTag((byte)16);
				output.WriteInt32(Y);
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
			if (X != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(X);
			}
			if (Y != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Y);
			}
			if (_unknownFields != null)
			{
				num += _unknownFields.CalculateSize();
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CellCoord other)
		{
			if (other != null)
			{
				if (other.X != 0)
				{
					X = other.X;
				}
				if (other.Y != 0)
				{
					Y = other.Y;
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
					X = input.ReadInt32();
					break;
				case 16u:
					Y = input.ReadInt32();
					break;
				}
			}
		}

		public string ToDiagnosticString()
		{
			return "CellCoord";
		}

		[Pure]
		public static explicit operator Coord(CellCoord value)
		{
			return new Coord(value.x_, value.y_);
		}

		[Pure]
		public static explicit operator Vector2Int(CellCoord value)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2Int(value.x_, value.y_);
		}
	}
}
