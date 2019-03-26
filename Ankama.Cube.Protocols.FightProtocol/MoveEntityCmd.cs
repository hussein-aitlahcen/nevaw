using Ankama.Cube.Protocols.CommonProtocol;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Ankama.Cube.Protocols.FightProtocol
{
	public sealed class MoveEntityCmd : IMessage<MoveEntityCmd>, IMessage, IEquatable<MoveEntityCmd>, IDeepCloneable<MoveEntityCmd>, ICustomDiagnosticMessage
	{
		private static readonly MessageParser<MoveEntityCmd> _parser = new MessageParser<MoveEntityCmd>((Func<MoveEntityCmd>)(() => new MoveEntityCmd()));

		private UnknownFieldSet _unknownFields;

		public const int EntityIdFieldNumber = 1;

		private int entityId_;

		public const int PathFieldNumber = 2;

		private static readonly FieldCodec<CellCoord> _repeated_path_codec = FieldCodec.ForMessage<CellCoord>(18u, CellCoord.Parser);

		private readonly RepeatedField<CellCoord> path_ = new RepeatedField<CellCoord>();

		public const int EntityToAttackIdFieldNumber = 3;

		private static readonly FieldCodec<int?> _single_entityToAttackId_codec = FieldCodec.ForStructWrapper<int>(26u);

		private int? entityToAttackId_;

		[DebuggerNonUserCode]
		public static MessageParser<MoveEntityCmd> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => FightProtocolReflection.Descriptor.get_MessageTypes()[12];

		[DebuggerNonUserCode]
		MessageDescriptor Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public int EntityId
		{
			get
			{
				return entityId_;
			}
			set
			{
				entityId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<CellCoord> Path => path_;

		[DebuggerNonUserCode]
		public int? EntityToAttackId
		{
			get
			{
				return entityToAttackId_;
			}
			set
			{
				entityToAttackId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public MoveEntityCmd()
		{
		}

		[DebuggerNonUserCode]
		public MoveEntityCmd(MoveEntityCmd other)
			: this()
		{
			entityId_ = other.entityId_;
			path_ = other.path_.Clone();
			EntityToAttackId = other.EntityToAttackId;
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public MoveEntityCmd Clone()
		{
			return new MoveEntityCmd(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as MoveEntityCmd);
		}

		[DebuggerNonUserCode]
		public bool Equals(MoveEntityCmd other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (EntityId != other.EntityId)
			{
				return false;
			}
			if (!path_.Equals(other.path_))
			{
				return false;
			}
			if (EntityToAttackId != other.EntityToAttackId)
			{
				return false;
			}
			return object.Equals(_unknownFields, other._unknownFields);
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (EntityId != 0)
			{
				num ^= EntityId.GetHashCode();
			}
			num ^= ((object)path_).GetHashCode();
			if (entityToAttackId_.HasValue)
			{
				num ^= EntityToAttackId.GetHashCode();
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
			if (EntityId != 0)
			{
				output.WriteRawTag((byte)8);
				output.WriteInt32(EntityId);
			}
			path_.WriteTo(output, _repeated_path_codec);
			if (entityToAttackId_.HasValue)
			{
				_single_entityToAttackId_codec.WriteTagAndValue(output, EntityToAttackId);
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
			if (EntityId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(EntityId);
			}
			num += path_.CalculateSize(_repeated_path_codec);
			if (entityToAttackId_.HasValue)
			{
				num += _single_entityToAttackId_codec.CalculateSizeWithTag(EntityToAttackId);
			}
			if (_unknownFields != null)
			{
				num += _unknownFields.CalculateSize();
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(MoveEntityCmd other)
		{
			if (other != null)
			{
				if (other.EntityId != 0)
				{
					EntityId = other.EntityId;
				}
				path_.Add((IEnumerable<CellCoord>)other.path_);
				if (other.entityToAttackId_.HasValue && (!entityToAttackId_.HasValue || other.EntityToAttackId != 0))
				{
					EntityToAttackId = other.EntityToAttackId;
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
					EntityId = input.ReadInt32();
					break;
				case 18u:
					path_.AddEntriesFrom(input, _repeated_path_codec);
					break;
				case 26u:
				{
					int? num2 = _single_entityToAttackId_codec.Read(input);
					if (!entityToAttackId_.HasValue || num2 != 0)
					{
						EntityToAttackId = num2;
					}
					break;
				}
				}
			}
		}

		public string ToDiagnosticString()
		{
			return "MoveEntityCmd";
		}
	}
}
