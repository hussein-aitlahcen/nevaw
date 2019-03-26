using Ankama.Cube.Protocols.CommonProtocol;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using System;
using System.Diagnostics;

namespace Ankama.Cube.Protocols.FightAdminProtocol
{
	public sealed class AdminRequestCmd : IMessage<AdminRequestCmd>, IMessage, IEquatable<AdminRequestCmd>, IDeepCloneable<AdminRequestCmd>, ICustomDiagnosticMessage
	{
		public enum CmdOneofCase
		{
			None,
			DealDamage,
			Kill,
			Teleport,
			DrawSpells,
			DiscardSpells,
			GainElementPoints,
			GainActionPoints,
			GainReservePoints,
			PickSpell,
			SetProperty,
			Heal,
			InvokeSummoning,
			InvokeCompanion,
			SetElementaryState
		}

		[DebuggerNonUserCode]
		public static class Types
		{
			public sealed class SetPropertyCmd : IMessage<SetPropertyCmd>, IMessage, IEquatable<SetPropertyCmd>, IDeepCloneable<SetPropertyCmd>, ICustomDiagnosticMessage
			{
				private static readonly MessageParser<SetPropertyCmd> _parser = new MessageParser<SetPropertyCmd>((Func<SetPropertyCmd>)(() => new SetPropertyCmd()));

				private UnknownFieldSet _unknownFields;

				public const int TargetEntityIdFieldNumber = 1;

				private int targetEntityId_;

				public const int PropertyIdFieldNumber = 2;

				private int propertyId_;

				public const int ActiveFieldNumber = 3;

				private bool active_;

				[DebuggerNonUserCode]
				public static MessageParser<SetPropertyCmd> Parser => _parser;

				[DebuggerNonUserCode]
				public static MessageDescriptor Descriptor => AdminRequestCmd.Descriptor.get_NestedTypes()[0];

				[DebuggerNonUserCode]
				MessageDescriptor Descriptor => Descriptor;

				[DebuggerNonUserCode]
				public int TargetEntityId
				{
					get
					{
						return targetEntityId_;
					}
					set
					{
						targetEntityId_ = value;
					}
				}

				[DebuggerNonUserCode]
				public int PropertyId
				{
					get
					{
						return propertyId_;
					}
					set
					{
						propertyId_ = value;
					}
				}

				[DebuggerNonUserCode]
				public bool Active
				{
					get
					{
						return active_;
					}
					set
					{
						active_ = value;
					}
				}

				[DebuggerNonUserCode]
				public SetPropertyCmd()
				{
				}

				[DebuggerNonUserCode]
				public SetPropertyCmd(SetPropertyCmd other)
					: this()
				{
					targetEntityId_ = other.targetEntityId_;
					propertyId_ = other.propertyId_;
					active_ = other.active_;
					_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
				}

				[DebuggerNonUserCode]
				public SetPropertyCmd Clone()
				{
					return new SetPropertyCmd(this);
				}

				[DebuggerNonUserCode]
				public override bool Equals(object other)
				{
					return Equals(other as SetPropertyCmd);
				}

				[DebuggerNonUserCode]
				public bool Equals(SetPropertyCmd other)
				{
					if (other == null)
					{
						return false;
					}
					if (other == this)
					{
						return true;
					}
					if (TargetEntityId != other.TargetEntityId)
					{
						return false;
					}
					if (PropertyId != other.PropertyId)
					{
						return false;
					}
					if (Active != other.Active)
					{
						return false;
					}
					return object.Equals(_unknownFields, other._unknownFields);
				}

				[DebuggerNonUserCode]
				public override int GetHashCode()
				{
					int num = 1;
					if (TargetEntityId != 0)
					{
						num ^= TargetEntityId.GetHashCode();
					}
					if (PropertyId != 0)
					{
						num ^= PropertyId.GetHashCode();
					}
					if (Active)
					{
						num ^= Active.GetHashCode();
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
					if (TargetEntityId != 0)
					{
						output.WriteRawTag((byte)8);
						output.WriteInt32(TargetEntityId);
					}
					if (PropertyId != 0)
					{
						output.WriteRawTag((byte)16);
						output.WriteInt32(PropertyId);
					}
					if (Active)
					{
						output.WriteRawTag((byte)24);
						output.WriteBool(Active);
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
					if (TargetEntityId != 0)
					{
						num += 1 + CodedOutputStream.ComputeInt32Size(TargetEntityId);
					}
					if (PropertyId != 0)
					{
						num += 1 + CodedOutputStream.ComputeInt32Size(PropertyId);
					}
					if (Active)
					{
						num += 2;
					}
					if (_unknownFields != null)
					{
						num += _unknownFields.CalculateSize();
					}
					return num;
				}

				[DebuggerNonUserCode]
				public void MergeFrom(SetPropertyCmd other)
				{
					if (other != null)
					{
						if (other.TargetEntityId != 0)
						{
							TargetEntityId = other.TargetEntityId;
						}
						if (other.PropertyId != 0)
						{
							PropertyId = other.PropertyId;
						}
						if (other.Active)
						{
							Active = other.Active;
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
							TargetEntityId = input.ReadInt32();
							break;
						case 16u:
							PropertyId = input.ReadInt32();
							break;
						case 24u:
							Active = input.ReadBool();
							break;
						}
					}
				}

				public string ToDiagnosticString()
				{
					return "SetPropertyCmd";
				}
			}

			public sealed class SetElementaryStateAdminCmd : IMessage<SetElementaryStateAdminCmd>, IMessage, IEquatable<SetElementaryStateAdminCmd>, IDeepCloneable<SetElementaryStateAdminCmd>, ICustomDiagnosticMessage
			{
				private static readonly MessageParser<SetElementaryStateAdminCmd> _parser = new MessageParser<SetElementaryStateAdminCmd>((Func<SetElementaryStateAdminCmd>)(() => new SetElementaryStateAdminCmd()));

				private UnknownFieldSet _unknownFields;

				public const int TargetEntityIdFieldNumber = 1;

				private int targetEntityId_;

				public const int ElementaryStateIdFieldNumber = 2;

				private int elementaryStateId_;

				[DebuggerNonUserCode]
				public static MessageParser<SetElementaryStateAdminCmd> Parser => _parser;

				[DebuggerNonUserCode]
				public static MessageDescriptor Descriptor => AdminRequestCmd.Descriptor.get_NestedTypes()[1];

				[DebuggerNonUserCode]
				MessageDescriptor Descriptor => Descriptor;

				[DebuggerNonUserCode]
				public int TargetEntityId
				{
					get
					{
						return targetEntityId_;
					}
					set
					{
						targetEntityId_ = value;
					}
				}

				[DebuggerNonUserCode]
				public int ElementaryStateId
				{
					get
					{
						return elementaryStateId_;
					}
					set
					{
						elementaryStateId_ = value;
					}
				}

				[DebuggerNonUserCode]
				public SetElementaryStateAdminCmd()
				{
				}

				[DebuggerNonUserCode]
				public SetElementaryStateAdminCmd(SetElementaryStateAdminCmd other)
					: this()
				{
					targetEntityId_ = other.targetEntityId_;
					elementaryStateId_ = other.elementaryStateId_;
					_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
				}

				[DebuggerNonUserCode]
				public SetElementaryStateAdminCmd Clone()
				{
					return new SetElementaryStateAdminCmd(this);
				}

				[DebuggerNonUserCode]
				public override bool Equals(object other)
				{
					return Equals(other as SetElementaryStateAdminCmd);
				}

				[DebuggerNonUserCode]
				public bool Equals(SetElementaryStateAdminCmd other)
				{
					if (other == null)
					{
						return false;
					}
					if (other == this)
					{
						return true;
					}
					if (TargetEntityId != other.TargetEntityId)
					{
						return false;
					}
					if (ElementaryStateId != other.ElementaryStateId)
					{
						return false;
					}
					return object.Equals(_unknownFields, other._unknownFields);
				}

				[DebuggerNonUserCode]
				public override int GetHashCode()
				{
					int num = 1;
					if (TargetEntityId != 0)
					{
						num ^= TargetEntityId.GetHashCode();
					}
					if (ElementaryStateId != 0)
					{
						num ^= ElementaryStateId.GetHashCode();
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
					if (TargetEntityId != 0)
					{
						output.WriteRawTag((byte)8);
						output.WriteInt32(TargetEntityId);
					}
					if (ElementaryStateId != 0)
					{
						output.WriteRawTag((byte)16);
						output.WriteInt32(ElementaryStateId);
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
					if (TargetEntityId != 0)
					{
						num += 1 + CodedOutputStream.ComputeInt32Size(TargetEntityId);
					}
					if (ElementaryStateId != 0)
					{
						num += 1 + CodedOutputStream.ComputeInt32Size(ElementaryStateId);
					}
					if (_unknownFields != null)
					{
						num += _unknownFields.CalculateSize();
					}
					return num;
				}

				[DebuggerNonUserCode]
				public void MergeFrom(SetElementaryStateAdminCmd other)
				{
					if (other != null)
					{
						if (other.TargetEntityId != 0)
						{
							TargetEntityId = other.TargetEntityId;
						}
						if (other.ElementaryStateId != 0)
						{
							ElementaryStateId = other.ElementaryStateId;
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
							TargetEntityId = input.ReadInt32();
							break;
						case 16u:
							ElementaryStateId = input.ReadInt32();
							break;
						}
					}
				}

				public string ToDiagnosticString()
				{
					return "SetElementaryStateAdminCmd";
				}
			}

			public sealed class DealDamageAdminCmd : IMessage<DealDamageAdminCmd>, IMessage, IEquatable<DealDamageAdminCmd>, IDeepCloneable<DealDamageAdminCmd>, ICustomDiagnosticMessage
			{
				private static readonly MessageParser<DealDamageAdminCmd> _parser = new MessageParser<DealDamageAdminCmd>((Func<DealDamageAdminCmd>)(() => new DealDamageAdminCmd()));

				private UnknownFieldSet _unknownFields;

				public const int TargetEntityIdFieldNumber = 1;

				private int targetEntityId_;

				public const int QuantityFieldNumber = 2;

				private int quantity_;

				public const int MagicalFieldNumber = 3;

				private bool magical_;

				[DebuggerNonUserCode]
				public static MessageParser<DealDamageAdminCmd> Parser => _parser;

				[DebuggerNonUserCode]
				public static MessageDescriptor Descriptor => AdminRequestCmd.Descriptor.get_NestedTypes()[2];

				[DebuggerNonUserCode]
				MessageDescriptor Descriptor => Descriptor;

				[DebuggerNonUserCode]
				public int TargetEntityId
				{
					get
					{
						return targetEntityId_;
					}
					set
					{
						targetEntityId_ = value;
					}
				}

				[DebuggerNonUserCode]
				public int Quantity
				{
					get
					{
						return quantity_;
					}
					set
					{
						quantity_ = value;
					}
				}

				[DebuggerNonUserCode]
				public bool Magical
				{
					get
					{
						return magical_;
					}
					set
					{
						magical_ = value;
					}
				}

				[DebuggerNonUserCode]
				public DealDamageAdminCmd()
				{
				}

				[DebuggerNonUserCode]
				public DealDamageAdminCmd(DealDamageAdminCmd other)
					: this()
				{
					targetEntityId_ = other.targetEntityId_;
					quantity_ = other.quantity_;
					magical_ = other.magical_;
					_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
				}

				[DebuggerNonUserCode]
				public DealDamageAdminCmd Clone()
				{
					return new DealDamageAdminCmd(this);
				}

				[DebuggerNonUserCode]
				public override bool Equals(object other)
				{
					return Equals(other as DealDamageAdminCmd);
				}

				[DebuggerNonUserCode]
				public bool Equals(DealDamageAdminCmd other)
				{
					if (other == null)
					{
						return false;
					}
					if (other == this)
					{
						return true;
					}
					if (TargetEntityId != other.TargetEntityId)
					{
						return false;
					}
					if (Quantity != other.Quantity)
					{
						return false;
					}
					if (Magical != other.Magical)
					{
						return false;
					}
					return object.Equals(_unknownFields, other._unknownFields);
				}

				[DebuggerNonUserCode]
				public override int GetHashCode()
				{
					int num = 1;
					if (TargetEntityId != 0)
					{
						num ^= TargetEntityId.GetHashCode();
					}
					if (Quantity != 0)
					{
						num ^= Quantity.GetHashCode();
					}
					if (Magical)
					{
						num ^= Magical.GetHashCode();
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
					if (TargetEntityId != 0)
					{
						output.WriteRawTag((byte)8);
						output.WriteInt32(TargetEntityId);
					}
					if (Quantity != 0)
					{
						output.WriteRawTag((byte)16);
						output.WriteInt32(Quantity);
					}
					if (Magical)
					{
						output.WriteRawTag((byte)24);
						output.WriteBool(Magical);
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
					if (TargetEntityId != 0)
					{
						num += 1 + CodedOutputStream.ComputeInt32Size(TargetEntityId);
					}
					if (Quantity != 0)
					{
						num += 1 + CodedOutputStream.ComputeInt32Size(Quantity);
					}
					if (Magical)
					{
						num += 2;
					}
					if (_unknownFields != null)
					{
						num += _unknownFields.CalculateSize();
					}
					return num;
				}

				[DebuggerNonUserCode]
				public void MergeFrom(DealDamageAdminCmd other)
				{
					if (other != null)
					{
						if (other.TargetEntityId != 0)
						{
							TargetEntityId = other.TargetEntityId;
						}
						if (other.Quantity != 0)
						{
							Quantity = other.Quantity;
						}
						if (other.Magical)
						{
							Magical = other.Magical;
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
							TargetEntityId = input.ReadInt32();
							break;
						case 16u:
							Quantity = input.ReadInt32();
							break;
						case 24u:
							Magical = input.ReadBool();
							break;
						}
					}
				}

				public string ToDiagnosticString()
				{
					return "DealDamageAdminCmd";
				}
			}

			public sealed class KillAdminCmd : IMessage<KillAdminCmd>, IMessage, IEquatable<KillAdminCmd>, IDeepCloneable<KillAdminCmd>, ICustomDiagnosticMessage
			{
				private static readonly MessageParser<KillAdminCmd> _parser = new MessageParser<KillAdminCmd>((Func<KillAdminCmd>)(() => new KillAdminCmd()));

				private UnknownFieldSet _unknownFields;

				public const int TargetEntityIdFieldNumber = 1;

				private int targetEntityId_;

				[DebuggerNonUserCode]
				public static MessageParser<KillAdminCmd> Parser => _parser;

				[DebuggerNonUserCode]
				public static MessageDescriptor Descriptor => AdminRequestCmd.Descriptor.get_NestedTypes()[3];

				[DebuggerNonUserCode]
				MessageDescriptor Descriptor => Descriptor;

				[DebuggerNonUserCode]
				public int TargetEntityId
				{
					get
					{
						return targetEntityId_;
					}
					set
					{
						targetEntityId_ = value;
					}
				}

				[DebuggerNonUserCode]
				public KillAdminCmd()
				{
				}

				[DebuggerNonUserCode]
				public KillAdminCmd(KillAdminCmd other)
					: this()
				{
					targetEntityId_ = other.targetEntityId_;
					_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
				}

				[DebuggerNonUserCode]
				public KillAdminCmd Clone()
				{
					return new KillAdminCmd(this);
				}

				[DebuggerNonUserCode]
				public override bool Equals(object other)
				{
					return Equals(other as KillAdminCmd);
				}

				[DebuggerNonUserCode]
				public bool Equals(KillAdminCmd other)
				{
					if (other == null)
					{
						return false;
					}
					if (other == this)
					{
						return true;
					}
					if (TargetEntityId != other.TargetEntityId)
					{
						return false;
					}
					return object.Equals(_unknownFields, other._unknownFields);
				}

				[DebuggerNonUserCode]
				public override int GetHashCode()
				{
					int num = 1;
					if (TargetEntityId != 0)
					{
						num ^= TargetEntityId.GetHashCode();
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
					if (TargetEntityId != 0)
					{
						output.WriteRawTag((byte)8);
						output.WriteInt32(TargetEntityId);
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
					if (TargetEntityId != 0)
					{
						num += 1 + CodedOutputStream.ComputeInt32Size(TargetEntityId);
					}
					if (_unknownFields != null)
					{
						num += _unknownFields.CalculateSize();
					}
					return num;
				}

				[DebuggerNonUserCode]
				public void MergeFrom(KillAdminCmd other)
				{
					if (other != null)
					{
						if (other.TargetEntityId != 0)
						{
							TargetEntityId = other.TargetEntityId;
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
						if (num != 8)
						{
							_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
						}
						else
						{
							TargetEntityId = input.ReadInt32();
						}
					}
				}

				public string ToDiagnosticString()
				{
					return "KillAdminCmd";
				}
			}

			public sealed class TeleportAdminCmd : IMessage<TeleportAdminCmd>, IMessage, IEquatable<TeleportAdminCmd>, IDeepCloneable<TeleportAdminCmd>, ICustomDiagnosticMessage
			{
				private static readonly MessageParser<TeleportAdminCmd> _parser = new MessageParser<TeleportAdminCmd>((Func<TeleportAdminCmd>)(() => new TeleportAdminCmd()));

				private UnknownFieldSet _unknownFields;

				public const int TargetEntityIdFieldNumber = 1;

				private int targetEntityId_;

				public const int DestinationFieldNumber = 2;

				private CellCoord destination_;

				[DebuggerNonUserCode]
				public static MessageParser<TeleportAdminCmd> Parser => _parser;

				[DebuggerNonUserCode]
				public static MessageDescriptor Descriptor => AdminRequestCmd.Descriptor.get_NestedTypes()[4];

				[DebuggerNonUserCode]
				MessageDescriptor Descriptor => Descriptor;

				[DebuggerNonUserCode]
				public int TargetEntityId
				{
					get
					{
						return targetEntityId_;
					}
					set
					{
						targetEntityId_ = value;
					}
				}

				[DebuggerNonUserCode]
				public CellCoord Destination
				{
					get
					{
						return destination_;
					}
					set
					{
						destination_ = value;
					}
				}

				[DebuggerNonUserCode]
				public TeleportAdminCmd()
				{
				}

				[DebuggerNonUserCode]
				public TeleportAdminCmd(TeleportAdminCmd other)
					: this()
				{
					targetEntityId_ = other.targetEntityId_;
					destination_ = ((other.destination_ != null) ? other.destination_.Clone() : null);
					_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
				}

				[DebuggerNonUserCode]
				public TeleportAdminCmd Clone()
				{
					return new TeleportAdminCmd(this);
				}

				[DebuggerNonUserCode]
				public override bool Equals(object other)
				{
					return Equals(other as TeleportAdminCmd);
				}

				[DebuggerNonUserCode]
				public bool Equals(TeleportAdminCmd other)
				{
					if (other == null)
					{
						return false;
					}
					if (other == this)
					{
						return true;
					}
					if (TargetEntityId != other.TargetEntityId)
					{
						return false;
					}
					if (!object.Equals(Destination, other.Destination))
					{
						return false;
					}
					return object.Equals(_unknownFields, other._unknownFields);
				}

				[DebuggerNonUserCode]
				public override int GetHashCode()
				{
					int num = 1;
					if (TargetEntityId != 0)
					{
						num ^= TargetEntityId.GetHashCode();
					}
					if (destination_ != null)
					{
						num ^= Destination.GetHashCode();
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
					if (TargetEntityId != 0)
					{
						output.WriteRawTag((byte)8);
						output.WriteInt32(TargetEntityId);
					}
					if (destination_ != null)
					{
						output.WriteRawTag((byte)18);
						output.WriteMessage(Destination);
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
					if (TargetEntityId != 0)
					{
						num += 1 + CodedOutputStream.ComputeInt32Size(TargetEntityId);
					}
					if (destination_ != null)
					{
						num += 1 + CodedOutputStream.ComputeMessageSize(Destination);
					}
					if (_unknownFields != null)
					{
						num += _unknownFields.CalculateSize();
					}
					return num;
				}

				[DebuggerNonUserCode]
				public void MergeFrom(TeleportAdminCmd other)
				{
					if (other == null)
					{
						return;
					}
					if (other.TargetEntityId != 0)
					{
						TargetEntityId = other.TargetEntityId;
					}
					if (other.destination_ != null)
					{
						if (destination_ == null)
						{
							destination_ = new CellCoord();
						}
						Destination.MergeFrom(other.Destination);
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
							TargetEntityId = input.ReadInt32();
							break;
						case 18u:
							if (destination_ == null)
							{
								destination_ = new CellCoord();
							}
							input.ReadMessage(destination_);
							break;
						}
					}
				}

				public string ToDiagnosticString()
				{
					return "TeleportAdminCmd";
				}
			}

			public sealed class DrawSpellsCmd : IMessage<DrawSpellsCmd>, IMessage, IEquatable<DrawSpellsCmd>, IDeepCloneable<DrawSpellsCmd>, ICustomDiagnosticMessage
			{
				private static readonly MessageParser<DrawSpellsCmd> _parser = new MessageParser<DrawSpellsCmd>((Func<DrawSpellsCmd>)(() => new DrawSpellsCmd()));

				private UnknownFieldSet _unknownFields;

				public const int PlayerEntityIdFieldNumber = 1;

				private int playerEntityId_;

				public const int QuantityFieldNumber = 2;

				private int quantity_;

				[DebuggerNonUserCode]
				public static MessageParser<DrawSpellsCmd> Parser => _parser;

				[DebuggerNonUserCode]
				public static MessageDescriptor Descriptor => AdminRequestCmd.Descriptor.get_NestedTypes()[5];

				[DebuggerNonUserCode]
				MessageDescriptor Descriptor => Descriptor;

				[DebuggerNonUserCode]
				public int PlayerEntityId
				{
					get
					{
						return playerEntityId_;
					}
					set
					{
						playerEntityId_ = value;
					}
				}

				[DebuggerNonUserCode]
				public int Quantity
				{
					get
					{
						return quantity_;
					}
					set
					{
						quantity_ = value;
					}
				}

				[DebuggerNonUserCode]
				public DrawSpellsCmd()
				{
				}

				[DebuggerNonUserCode]
				public DrawSpellsCmd(DrawSpellsCmd other)
					: this()
				{
					playerEntityId_ = other.playerEntityId_;
					quantity_ = other.quantity_;
					_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
				}

				[DebuggerNonUserCode]
				public DrawSpellsCmd Clone()
				{
					return new DrawSpellsCmd(this);
				}

				[DebuggerNonUserCode]
				public override bool Equals(object other)
				{
					return Equals(other as DrawSpellsCmd);
				}

				[DebuggerNonUserCode]
				public bool Equals(DrawSpellsCmd other)
				{
					if (other == null)
					{
						return false;
					}
					if (other == this)
					{
						return true;
					}
					if (PlayerEntityId != other.PlayerEntityId)
					{
						return false;
					}
					if (Quantity != other.Quantity)
					{
						return false;
					}
					return object.Equals(_unknownFields, other._unknownFields);
				}

				[DebuggerNonUserCode]
				public override int GetHashCode()
				{
					int num = 1;
					if (PlayerEntityId != 0)
					{
						num ^= PlayerEntityId.GetHashCode();
					}
					if (Quantity != 0)
					{
						num ^= Quantity.GetHashCode();
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
					if (PlayerEntityId != 0)
					{
						output.WriteRawTag((byte)8);
						output.WriteInt32(PlayerEntityId);
					}
					if (Quantity != 0)
					{
						output.WriteRawTag((byte)16);
						output.WriteInt32(Quantity);
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
					if (PlayerEntityId != 0)
					{
						num += 1 + CodedOutputStream.ComputeInt32Size(PlayerEntityId);
					}
					if (Quantity != 0)
					{
						num += 1 + CodedOutputStream.ComputeInt32Size(Quantity);
					}
					if (_unknownFields != null)
					{
						num += _unknownFields.CalculateSize();
					}
					return num;
				}

				[DebuggerNonUserCode]
				public void MergeFrom(DrawSpellsCmd other)
				{
					if (other != null)
					{
						if (other.PlayerEntityId != 0)
						{
							PlayerEntityId = other.PlayerEntityId;
						}
						if (other.Quantity != 0)
						{
							Quantity = other.Quantity;
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
							PlayerEntityId = input.ReadInt32();
							break;
						case 16u:
							Quantity = input.ReadInt32();
							break;
						}
					}
				}

				public string ToDiagnosticString()
				{
					return "DrawSpellsCmd";
				}
			}

			public sealed class DiscardSpellsCmd : IMessage<DiscardSpellsCmd>, IMessage, IEquatable<DiscardSpellsCmd>, IDeepCloneable<DiscardSpellsCmd>, ICustomDiagnosticMessage
			{
				private static readonly MessageParser<DiscardSpellsCmd> _parser = new MessageParser<DiscardSpellsCmd>((Func<DiscardSpellsCmd>)(() => new DiscardSpellsCmd()));

				private UnknownFieldSet _unknownFields;

				public const int PlayerEntityIdFieldNumber = 1;

				private int playerEntityId_;

				public const int QuantityFieldNumber = 2;

				private int quantity_;

				[DebuggerNonUserCode]
				public static MessageParser<DiscardSpellsCmd> Parser => _parser;

				[DebuggerNonUserCode]
				public static MessageDescriptor Descriptor => AdminRequestCmd.Descriptor.get_NestedTypes()[6];

				[DebuggerNonUserCode]
				MessageDescriptor Descriptor => Descriptor;

				[DebuggerNonUserCode]
				public int PlayerEntityId
				{
					get
					{
						return playerEntityId_;
					}
					set
					{
						playerEntityId_ = value;
					}
				}

				[DebuggerNonUserCode]
				public int Quantity
				{
					get
					{
						return quantity_;
					}
					set
					{
						quantity_ = value;
					}
				}

				[DebuggerNonUserCode]
				public DiscardSpellsCmd()
				{
				}

				[DebuggerNonUserCode]
				public DiscardSpellsCmd(DiscardSpellsCmd other)
					: this()
				{
					playerEntityId_ = other.playerEntityId_;
					quantity_ = other.quantity_;
					_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
				}

				[DebuggerNonUserCode]
				public DiscardSpellsCmd Clone()
				{
					return new DiscardSpellsCmd(this);
				}

				[DebuggerNonUserCode]
				public override bool Equals(object other)
				{
					return Equals(other as DiscardSpellsCmd);
				}

				[DebuggerNonUserCode]
				public bool Equals(DiscardSpellsCmd other)
				{
					if (other == null)
					{
						return false;
					}
					if (other == this)
					{
						return true;
					}
					if (PlayerEntityId != other.PlayerEntityId)
					{
						return false;
					}
					if (Quantity != other.Quantity)
					{
						return false;
					}
					return object.Equals(_unknownFields, other._unknownFields);
				}

				[DebuggerNonUserCode]
				public override int GetHashCode()
				{
					int num = 1;
					if (PlayerEntityId != 0)
					{
						num ^= PlayerEntityId.GetHashCode();
					}
					if (Quantity != 0)
					{
						num ^= Quantity.GetHashCode();
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
					if (PlayerEntityId != 0)
					{
						output.WriteRawTag((byte)8);
						output.WriteInt32(PlayerEntityId);
					}
					if (Quantity != 0)
					{
						output.WriteRawTag((byte)16);
						output.WriteInt32(Quantity);
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
					if (PlayerEntityId != 0)
					{
						num += 1 + CodedOutputStream.ComputeInt32Size(PlayerEntityId);
					}
					if (Quantity != 0)
					{
						num += 1 + CodedOutputStream.ComputeInt32Size(Quantity);
					}
					if (_unknownFields != null)
					{
						num += _unknownFields.CalculateSize();
					}
					return num;
				}

				[DebuggerNonUserCode]
				public void MergeFrom(DiscardSpellsCmd other)
				{
					if (other != null)
					{
						if (other.PlayerEntityId != 0)
						{
							PlayerEntityId = other.PlayerEntityId;
						}
						if (other.Quantity != 0)
						{
							Quantity = other.Quantity;
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
							PlayerEntityId = input.ReadInt32();
							break;
						case 16u:
							Quantity = input.ReadInt32();
							break;
						}
					}
				}

				public string ToDiagnosticString()
				{
					return "DiscardSpellsCmd";
				}
			}

			public sealed class GainElementPointsCmd : IMessage<GainElementPointsCmd>, IMessage, IEquatable<GainElementPointsCmd>, IDeepCloneable<GainElementPointsCmd>, ICustomDiagnosticMessage
			{
				private static readonly MessageParser<GainElementPointsCmd> _parser = new MessageParser<GainElementPointsCmd>((Func<GainElementPointsCmd>)(() => new GainElementPointsCmd()));

				private UnknownFieldSet _unknownFields;

				public const int PlayerEntityIdFieldNumber = 1;

				private int playerEntityId_;

				public const int QuantityFieldNumber = 2;

				private int quantity_;

				[DebuggerNonUserCode]
				public static MessageParser<GainElementPointsCmd> Parser => _parser;

				[DebuggerNonUserCode]
				public static MessageDescriptor Descriptor => AdminRequestCmd.Descriptor.get_NestedTypes()[7];

				[DebuggerNonUserCode]
				MessageDescriptor Descriptor => Descriptor;

				[DebuggerNonUserCode]
				public int PlayerEntityId
				{
					get
					{
						return playerEntityId_;
					}
					set
					{
						playerEntityId_ = value;
					}
				}

				[DebuggerNonUserCode]
				public int Quantity
				{
					get
					{
						return quantity_;
					}
					set
					{
						quantity_ = value;
					}
				}

				[DebuggerNonUserCode]
				public GainElementPointsCmd()
				{
				}

				[DebuggerNonUserCode]
				public GainElementPointsCmd(GainElementPointsCmd other)
					: this()
				{
					playerEntityId_ = other.playerEntityId_;
					quantity_ = other.quantity_;
					_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
				}

				[DebuggerNonUserCode]
				public GainElementPointsCmd Clone()
				{
					return new GainElementPointsCmd(this);
				}

				[DebuggerNonUserCode]
				public override bool Equals(object other)
				{
					return Equals(other as GainElementPointsCmd);
				}

				[DebuggerNonUserCode]
				public bool Equals(GainElementPointsCmd other)
				{
					if (other == null)
					{
						return false;
					}
					if (other == this)
					{
						return true;
					}
					if (PlayerEntityId != other.PlayerEntityId)
					{
						return false;
					}
					if (Quantity != other.Quantity)
					{
						return false;
					}
					return object.Equals(_unknownFields, other._unknownFields);
				}

				[DebuggerNonUserCode]
				public override int GetHashCode()
				{
					int num = 1;
					if (PlayerEntityId != 0)
					{
						num ^= PlayerEntityId.GetHashCode();
					}
					if (Quantity != 0)
					{
						num ^= Quantity.GetHashCode();
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
					if (PlayerEntityId != 0)
					{
						output.WriteRawTag((byte)8);
						output.WriteInt32(PlayerEntityId);
					}
					if (Quantity != 0)
					{
						output.WriteRawTag((byte)16);
						output.WriteInt32(Quantity);
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
					if (PlayerEntityId != 0)
					{
						num += 1 + CodedOutputStream.ComputeInt32Size(PlayerEntityId);
					}
					if (Quantity != 0)
					{
						num += 1 + CodedOutputStream.ComputeInt32Size(Quantity);
					}
					if (_unknownFields != null)
					{
						num += _unknownFields.CalculateSize();
					}
					return num;
				}

				[DebuggerNonUserCode]
				public void MergeFrom(GainElementPointsCmd other)
				{
					if (other != null)
					{
						if (other.PlayerEntityId != 0)
						{
							PlayerEntityId = other.PlayerEntityId;
						}
						if (other.Quantity != 0)
						{
							Quantity = other.Quantity;
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
							PlayerEntityId = input.ReadInt32();
							break;
						case 16u:
							Quantity = input.ReadInt32();
							break;
						}
					}
				}

				public string ToDiagnosticString()
				{
					return "GainElementPointsCmd";
				}
			}

			public sealed class GainActionPointsCmd : IMessage<GainActionPointsCmd>, IMessage, IEquatable<GainActionPointsCmd>, IDeepCloneable<GainActionPointsCmd>, ICustomDiagnosticMessage
			{
				private static readonly MessageParser<GainActionPointsCmd> _parser = new MessageParser<GainActionPointsCmd>((Func<GainActionPointsCmd>)(() => new GainActionPointsCmd()));

				private UnknownFieldSet _unknownFields;

				public const int PlayerEntityIdFieldNumber = 1;

				private int playerEntityId_;

				public const int QuantityFieldNumber = 2;

				private int quantity_;

				[DebuggerNonUserCode]
				public static MessageParser<GainActionPointsCmd> Parser => _parser;

				[DebuggerNonUserCode]
				public static MessageDescriptor Descriptor => AdminRequestCmd.Descriptor.get_NestedTypes()[8];

				[DebuggerNonUserCode]
				MessageDescriptor Descriptor => Descriptor;

				[DebuggerNonUserCode]
				public int PlayerEntityId
				{
					get
					{
						return playerEntityId_;
					}
					set
					{
						playerEntityId_ = value;
					}
				}

				[DebuggerNonUserCode]
				public int Quantity
				{
					get
					{
						return quantity_;
					}
					set
					{
						quantity_ = value;
					}
				}

				[DebuggerNonUserCode]
				public GainActionPointsCmd()
				{
				}

				[DebuggerNonUserCode]
				public GainActionPointsCmd(GainActionPointsCmd other)
					: this()
				{
					playerEntityId_ = other.playerEntityId_;
					quantity_ = other.quantity_;
					_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
				}

				[DebuggerNonUserCode]
				public GainActionPointsCmd Clone()
				{
					return new GainActionPointsCmd(this);
				}

				[DebuggerNonUserCode]
				public override bool Equals(object other)
				{
					return Equals(other as GainActionPointsCmd);
				}

				[DebuggerNonUserCode]
				public bool Equals(GainActionPointsCmd other)
				{
					if (other == null)
					{
						return false;
					}
					if (other == this)
					{
						return true;
					}
					if (PlayerEntityId != other.PlayerEntityId)
					{
						return false;
					}
					if (Quantity != other.Quantity)
					{
						return false;
					}
					return object.Equals(_unknownFields, other._unknownFields);
				}

				[DebuggerNonUserCode]
				public override int GetHashCode()
				{
					int num = 1;
					if (PlayerEntityId != 0)
					{
						num ^= PlayerEntityId.GetHashCode();
					}
					if (Quantity != 0)
					{
						num ^= Quantity.GetHashCode();
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
					if (PlayerEntityId != 0)
					{
						output.WriteRawTag((byte)8);
						output.WriteInt32(PlayerEntityId);
					}
					if (Quantity != 0)
					{
						output.WriteRawTag((byte)16);
						output.WriteInt32(Quantity);
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
					if (PlayerEntityId != 0)
					{
						num += 1 + CodedOutputStream.ComputeInt32Size(PlayerEntityId);
					}
					if (Quantity != 0)
					{
						num += 1 + CodedOutputStream.ComputeInt32Size(Quantity);
					}
					if (_unknownFields != null)
					{
						num += _unknownFields.CalculateSize();
					}
					return num;
				}

				[DebuggerNonUserCode]
				public void MergeFrom(GainActionPointsCmd other)
				{
					if (other != null)
					{
						if (other.PlayerEntityId != 0)
						{
							PlayerEntityId = other.PlayerEntityId;
						}
						if (other.Quantity != 0)
						{
							Quantity = other.Quantity;
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
							PlayerEntityId = input.ReadInt32();
							break;
						case 16u:
							Quantity = input.ReadInt32();
							break;
						}
					}
				}

				public string ToDiagnosticString()
				{
					return "GainActionPointsCmd";
				}
			}

			public sealed class GainReservePointsCmd : IMessage<GainReservePointsCmd>, IMessage, IEquatable<GainReservePointsCmd>, IDeepCloneable<GainReservePointsCmd>, ICustomDiagnosticMessage
			{
				private static readonly MessageParser<GainReservePointsCmd> _parser = new MessageParser<GainReservePointsCmd>((Func<GainReservePointsCmd>)(() => new GainReservePointsCmd()));

				private UnknownFieldSet _unknownFields;

				public const int PlayerEntityIdFieldNumber = 1;

				private int playerEntityId_;

				public const int QuantityFieldNumber = 2;

				private int quantity_;

				[DebuggerNonUserCode]
				public static MessageParser<GainReservePointsCmd> Parser => _parser;

				[DebuggerNonUserCode]
				public static MessageDescriptor Descriptor => AdminRequestCmd.Descriptor.get_NestedTypes()[9];

				[DebuggerNonUserCode]
				MessageDescriptor Descriptor => Descriptor;

				[DebuggerNonUserCode]
				public int PlayerEntityId
				{
					get
					{
						return playerEntityId_;
					}
					set
					{
						playerEntityId_ = value;
					}
				}

				[DebuggerNonUserCode]
				public int Quantity
				{
					get
					{
						return quantity_;
					}
					set
					{
						quantity_ = value;
					}
				}

				[DebuggerNonUserCode]
				public GainReservePointsCmd()
				{
				}

				[DebuggerNonUserCode]
				public GainReservePointsCmd(GainReservePointsCmd other)
					: this()
				{
					playerEntityId_ = other.playerEntityId_;
					quantity_ = other.quantity_;
					_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
				}

				[DebuggerNonUserCode]
				public GainReservePointsCmd Clone()
				{
					return new GainReservePointsCmd(this);
				}

				[DebuggerNonUserCode]
				public override bool Equals(object other)
				{
					return Equals(other as GainReservePointsCmd);
				}

				[DebuggerNonUserCode]
				public bool Equals(GainReservePointsCmd other)
				{
					if (other == null)
					{
						return false;
					}
					if (other == this)
					{
						return true;
					}
					if (PlayerEntityId != other.PlayerEntityId)
					{
						return false;
					}
					if (Quantity != other.Quantity)
					{
						return false;
					}
					return object.Equals(_unknownFields, other._unknownFields);
				}

				[DebuggerNonUserCode]
				public override int GetHashCode()
				{
					int num = 1;
					if (PlayerEntityId != 0)
					{
						num ^= PlayerEntityId.GetHashCode();
					}
					if (Quantity != 0)
					{
						num ^= Quantity.GetHashCode();
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
					if (PlayerEntityId != 0)
					{
						output.WriteRawTag((byte)8);
						output.WriteInt32(PlayerEntityId);
					}
					if (Quantity != 0)
					{
						output.WriteRawTag((byte)16);
						output.WriteInt32(Quantity);
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
					if (PlayerEntityId != 0)
					{
						num += 1 + CodedOutputStream.ComputeInt32Size(PlayerEntityId);
					}
					if (Quantity != 0)
					{
						num += 1 + CodedOutputStream.ComputeInt32Size(Quantity);
					}
					if (_unknownFields != null)
					{
						num += _unknownFields.CalculateSize();
					}
					return num;
				}

				[DebuggerNonUserCode]
				public void MergeFrom(GainReservePointsCmd other)
				{
					if (other != null)
					{
						if (other.PlayerEntityId != 0)
						{
							PlayerEntityId = other.PlayerEntityId;
						}
						if (other.Quantity != 0)
						{
							Quantity = other.Quantity;
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
							PlayerEntityId = input.ReadInt32();
							break;
						case 16u:
							Quantity = input.ReadInt32();
							break;
						}
					}
				}

				public string ToDiagnosticString()
				{
					return "GainReservePointsCmd";
				}
			}

			public sealed class PickSpellCmd : IMessage<PickSpellCmd>, IMessage, IEquatable<PickSpellCmd>, IDeepCloneable<PickSpellCmd>, ICustomDiagnosticMessage
			{
				private static readonly MessageParser<PickSpellCmd> _parser = new MessageParser<PickSpellCmd>((Func<PickSpellCmd>)(() => new PickSpellCmd()));

				private UnknownFieldSet _unknownFields;

				public const int PlayerEntityIdFieldNumber = 1;

				private int playerEntityId_;

				public const int QuantityFieldNumber = 2;

				private int quantity_;

				public const int SpellDefinitionIdFieldNumber = 3;

				private int spellDefinitionId_;

				public const int SpellLevelFieldNumber = 4;

				private int spellLevel_;

				[DebuggerNonUserCode]
				public static MessageParser<PickSpellCmd> Parser => _parser;

				[DebuggerNonUserCode]
				public static MessageDescriptor Descriptor => AdminRequestCmd.Descriptor.get_NestedTypes()[10];

				[DebuggerNonUserCode]
				MessageDescriptor Descriptor => Descriptor;

				[DebuggerNonUserCode]
				public int PlayerEntityId
				{
					get
					{
						return playerEntityId_;
					}
					set
					{
						playerEntityId_ = value;
					}
				}

				[DebuggerNonUserCode]
				public int Quantity
				{
					get
					{
						return quantity_;
					}
					set
					{
						quantity_ = value;
					}
				}

				[DebuggerNonUserCode]
				public int SpellDefinitionId
				{
					get
					{
						return spellDefinitionId_;
					}
					set
					{
						spellDefinitionId_ = value;
					}
				}

				[DebuggerNonUserCode]
				public int SpellLevel
				{
					get
					{
						return spellLevel_;
					}
					set
					{
						spellLevel_ = value;
					}
				}

				[DebuggerNonUserCode]
				public PickSpellCmd()
				{
				}

				[DebuggerNonUserCode]
				public PickSpellCmd(PickSpellCmd other)
					: this()
				{
					playerEntityId_ = other.playerEntityId_;
					quantity_ = other.quantity_;
					spellDefinitionId_ = other.spellDefinitionId_;
					spellLevel_ = other.spellLevel_;
					_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
				}

				[DebuggerNonUserCode]
				public PickSpellCmd Clone()
				{
					return new PickSpellCmd(this);
				}

				[DebuggerNonUserCode]
				public override bool Equals(object other)
				{
					return Equals(other as PickSpellCmd);
				}

				[DebuggerNonUserCode]
				public bool Equals(PickSpellCmd other)
				{
					if (other == null)
					{
						return false;
					}
					if (other == this)
					{
						return true;
					}
					if (PlayerEntityId != other.PlayerEntityId)
					{
						return false;
					}
					if (Quantity != other.Quantity)
					{
						return false;
					}
					if (SpellDefinitionId != other.SpellDefinitionId)
					{
						return false;
					}
					if (SpellLevel != other.SpellLevel)
					{
						return false;
					}
					return object.Equals(_unknownFields, other._unknownFields);
				}

				[DebuggerNonUserCode]
				public override int GetHashCode()
				{
					int num = 1;
					if (PlayerEntityId != 0)
					{
						num ^= PlayerEntityId.GetHashCode();
					}
					if (Quantity != 0)
					{
						num ^= Quantity.GetHashCode();
					}
					if (SpellDefinitionId != 0)
					{
						num ^= SpellDefinitionId.GetHashCode();
					}
					if (SpellLevel != 0)
					{
						num ^= SpellLevel.GetHashCode();
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
					if (PlayerEntityId != 0)
					{
						output.WriteRawTag((byte)8);
						output.WriteInt32(PlayerEntityId);
					}
					if (Quantity != 0)
					{
						output.WriteRawTag((byte)16);
						output.WriteInt32(Quantity);
					}
					if (SpellDefinitionId != 0)
					{
						output.WriteRawTag((byte)24);
						output.WriteInt32(SpellDefinitionId);
					}
					if (SpellLevel != 0)
					{
						output.WriteRawTag((byte)32);
						output.WriteInt32(SpellLevel);
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
					if (PlayerEntityId != 0)
					{
						num += 1 + CodedOutputStream.ComputeInt32Size(PlayerEntityId);
					}
					if (Quantity != 0)
					{
						num += 1 + CodedOutputStream.ComputeInt32Size(Quantity);
					}
					if (SpellDefinitionId != 0)
					{
						num += 1 + CodedOutputStream.ComputeInt32Size(SpellDefinitionId);
					}
					if (SpellLevel != 0)
					{
						num += 1 + CodedOutputStream.ComputeInt32Size(SpellLevel);
					}
					if (_unknownFields != null)
					{
						num += _unknownFields.CalculateSize();
					}
					return num;
				}

				[DebuggerNonUserCode]
				public void MergeFrom(PickSpellCmd other)
				{
					if (other != null)
					{
						if (other.PlayerEntityId != 0)
						{
							PlayerEntityId = other.PlayerEntityId;
						}
						if (other.Quantity != 0)
						{
							Quantity = other.Quantity;
						}
						if (other.SpellDefinitionId != 0)
						{
							SpellDefinitionId = other.SpellDefinitionId;
						}
						if (other.SpellLevel != 0)
						{
							SpellLevel = other.SpellLevel;
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
							PlayerEntityId = input.ReadInt32();
							break;
						case 16u:
							Quantity = input.ReadInt32();
							break;
						case 24u:
							SpellDefinitionId = input.ReadInt32();
							break;
						case 32u:
							SpellLevel = input.ReadInt32();
							break;
						}
					}
				}

				public string ToDiagnosticString()
				{
					return "PickSpellCmd";
				}
			}

			public sealed class HealAdminCmd : IMessage<HealAdminCmd>, IMessage, IEquatable<HealAdminCmd>, IDeepCloneable<HealAdminCmd>, ICustomDiagnosticMessage
			{
				private static readonly MessageParser<HealAdminCmd> _parser = new MessageParser<HealAdminCmd>((Func<HealAdminCmd>)(() => new HealAdminCmd()));

				private UnknownFieldSet _unknownFields;

				public const int TargetEntityIdFieldNumber = 1;

				private int targetEntityId_;

				public const int QuantityFieldNumber = 2;

				private int quantity_;

				public const int MagicalFieldNumber = 3;

				private bool magical_;

				[DebuggerNonUserCode]
				public static MessageParser<HealAdminCmd> Parser => _parser;

				[DebuggerNonUserCode]
				public static MessageDescriptor Descriptor => AdminRequestCmd.Descriptor.get_NestedTypes()[11];

				[DebuggerNonUserCode]
				MessageDescriptor Descriptor => Descriptor;

				[DebuggerNonUserCode]
				public int TargetEntityId
				{
					get
					{
						return targetEntityId_;
					}
					set
					{
						targetEntityId_ = value;
					}
				}

				[DebuggerNonUserCode]
				public int Quantity
				{
					get
					{
						return quantity_;
					}
					set
					{
						quantity_ = value;
					}
				}

				[DebuggerNonUserCode]
				public bool Magical
				{
					get
					{
						return magical_;
					}
					set
					{
						magical_ = value;
					}
				}

				[DebuggerNonUserCode]
				public HealAdminCmd()
				{
				}

				[DebuggerNonUserCode]
				public HealAdminCmd(HealAdminCmd other)
					: this()
				{
					targetEntityId_ = other.targetEntityId_;
					quantity_ = other.quantity_;
					magical_ = other.magical_;
					_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
				}

				[DebuggerNonUserCode]
				public HealAdminCmd Clone()
				{
					return new HealAdminCmd(this);
				}

				[DebuggerNonUserCode]
				public override bool Equals(object other)
				{
					return Equals(other as HealAdminCmd);
				}

				[DebuggerNonUserCode]
				public bool Equals(HealAdminCmd other)
				{
					if (other == null)
					{
						return false;
					}
					if (other == this)
					{
						return true;
					}
					if (TargetEntityId != other.TargetEntityId)
					{
						return false;
					}
					if (Quantity != other.Quantity)
					{
						return false;
					}
					if (Magical != other.Magical)
					{
						return false;
					}
					return object.Equals(_unknownFields, other._unknownFields);
				}

				[DebuggerNonUserCode]
				public override int GetHashCode()
				{
					int num = 1;
					if (TargetEntityId != 0)
					{
						num ^= TargetEntityId.GetHashCode();
					}
					if (Quantity != 0)
					{
						num ^= Quantity.GetHashCode();
					}
					if (Magical)
					{
						num ^= Magical.GetHashCode();
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
					if (TargetEntityId != 0)
					{
						output.WriteRawTag((byte)8);
						output.WriteInt32(TargetEntityId);
					}
					if (Quantity != 0)
					{
						output.WriteRawTag((byte)16);
						output.WriteInt32(Quantity);
					}
					if (Magical)
					{
						output.WriteRawTag((byte)24);
						output.WriteBool(Magical);
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
					if (TargetEntityId != 0)
					{
						num += 1 + CodedOutputStream.ComputeInt32Size(TargetEntityId);
					}
					if (Quantity != 0)
					{
						num += 1 + CodedOutputStream.ComputeInt32Size(Quantity);
					}
					if (Magical)
					{
						num += 2;
					}
					if (_unknownFields != null)
					{
						num += _unknownFields.CalculateSize();
					}
					return num;
				}

				[DebuggerNonUserCode]
				public void MergeFrom(HealAdminCmd other)
				{
					if (other != null)
					{
						if (other.TargetEntityId != 0)
						{
							TargetEntityId = other.TargetEntityId;
						}
						if (other.Quantity != 0)
						{
							Quantity = other.Quantity;
						}
						if (other.Magical)
						{
							Magical = other.Magical;
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
							TargetEntityId = input.ReadInt32();
							break;
						case 16u:
							Quantity = input.ReadInt32();
							break;
						case 24u:
							Magical = input.ReadBool();
							break;
						}
					}
				}

				public string ToDiagnosticString()
				{
					return "HealAdminCmd";
				}
			}

			public sealed class InvokeSummoningAdminCmd : IMessage<InvokeSummoningAdminCmd>, IMessage, IEquatable<InvokeSummoningAdminCmd>, IDeepCloneable<InvokeSummoningAdminCmd>, ICustomDiagnosticMessage
			{
				private static readonly MessageParser<InvokeSummoningAdminCmd> _parser = new MessageParser<InvokeSummoningAdminCmd>((Func<InvokeSummoningAdminCmd>)(() => new InvokeSummoningAdminCmd()));

				private UnknownFieldSet _unknownFields;

				public const int DefinitionIdFieldNumber = 1;

				private int definitionId_;

				public const int OwnerEntityIdFieldNumber = 2;

				private int ownerEntityId_;

				public const int SummoningLevelFieldNumber = 3;

				private int summoningLevel_;

				public const int DestinationFieldNumber = 4;

				private CellCoord destination_;

				[DebuggerNonUserCode]
				public static MessageParser<InvokeSummoningAdminCmd> Parser => _parser;

				[DebuggerNonUserCode]
				public static MessageDescriptor Descriptor => AdminRequestCmd.Descriptor.get_NestedTypes()[12];

				[DebuggerNonUserCode]
				MessageDescriptor Descriptor => Descriptor;

				[DebuggerNonUserCode]
				public int DefinitionId
				{
					get
					{
						return definitionId_;
					}
					set
					{
						definitionId_ = value;
					}
				}

				[DebuggerNonUserCode]
				public int OwnerEntityId
				{
					get
					{
						return ownerEntityId_;
					}
					set
					{
						ownerEntityId_ = value;
					}
				}

				[DebuggerNonUserCode]
				public int SummoningLevel
				{
					get
					{
						return summoningLevel_;
					}
					set
					{
						summoningLevel_ = value;
					}
				}

				[DebuggerNonUserCode]
				public CellCoord Destination
				{
					get
					{
						return destination_;
					}
					set
					{
						destination_ = value;
					}
				}

				[DebuggerNonUserCode]
				public InvokeSummoningAdminCmd()
				{
				}

				[DebuggerNonUserCode]
				public InvokeSummoningAdminCmd(InvokeSummoningAdminCmd other)
					: this()
				{
					definitionId_ = other.definitionId_;
					ownerEntityId_ = other.ownerEntityId_;
					summoningLevel_ = other.summoningLevel_;
					destination_ = ((other.destination_ != null) ? other.destination_.Clone() : null);
					_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
				}

				[DebuggerNonUserCode]
				public InvokeSummoningAdminCmd Clone()
				{
					return new InvokeSummoningAdminCmd(this);
				}

				[DebuggerNonUserCode]
				public override bool Equals(object other)
				{
					return Equals(other as InvokeSummoningAdminCmd);
				}

				[DebuggerNonUserCode]
				public bool Equals(InvokeSummoningAdminCmd other)
				{
					if (other == null)
					{
						return false;
					}
					if (other == this)
					{
						return true;
					}
					if (DefinitionId != other.DefinitionId)
					{
						return false;
					}
					if (OwnerEntityId != other.OwnerEntityId)
					{
						return false;
					}
					if (SummoningLevel != other.SummoningLevel)
					{
						return false;
					}
					if (!object.Equals(Destination, other.Destination))
					{
						return false;
					}
					return object.Equals(_unknownFields, other._unknownFields);
				}

				[DebuggerNonUserCode]
				public override int GetHashCode()
				{
					int num = 1;
					if (DefinitionId != 0)
					{
						num ^= DefinitionId.GetHashCode();
					}
					if (OwnerEntityId != 0)
					{
						num ^= OwnerEntityId.GetHashCode();
					}
					if (SummoningLevel != 0)
					{
						num ^= SummoningLevel.GetHashCode();
					}
					if (destination_ != null)
					{
						num ^= Destination.GetHashCode();
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
					if (DefinitionId != 0)
					{
						output.WriteRawTag((byte)8);
						output.WriteInt32(DefinitionId);
					}
					if (OwnerEntityId != 0)
					{
						output.WriteRawTag((byte)16);
						output.WriteInt32(OwnerEntityId);
					}
					if (SummoningLevel != 0)
					{
						output.WriteRawTag((byte)24);
						output.WriteInt32(SummoningLevel);
					}
					if (destination_ != null)
					{
						output.WriteRawTag((byte)34);
						output.WriteMessage(Destination);
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
					if (DefinitionId != 0)
					{
						num += 1 + CodedOutputStream.ComputeInt32Size(DefinitionId);
					}
					if (OwnerEntityId != 0)
					{
						num += 1 + CodedOutputStream.ComputeInt32Size(OwnerEntityId);
					}
					if (SummoningLevel != 0)
					{
						num += 1 + CodedOutputStream.ComputeInt32Size(SummoningLevel);
					}
					if (destination_ != null)
					{
						num += 1 + CodedOutputStream.ComputeMessageSize(Destination);
					}
					if (_unknownFields != null)
					{
						num += _unknownFields.CalculateSize();
					}
					return num;
				}

				[DebuggerNonUserCode]
				public void MergeFrom(InvokeSummoningAdminCmd other)
				{
					if (other == null)
					{
						return;
					}
					if (other.DefinitionId != 0)
					{
						DefinitionId = other.DefinitionId;
					}
					if (other.OwnerEntityId != 0)
					{
						OwnerEntityId = other.OwnerEntityId;
					}
					if (other.SummoningLevel != 0)
					{
						SummoningLevel = other.SummoningLevel;
					}
					if (other.destination_ != null)
					{
						if (destination_ == null)
						{
							destination_ = new CellCoord();
						}
						Destination.MergeFrom(other.Destination);
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
							DefinitionId = input.ReadInt32();
							break;
						case 16u:
							OwnerEntityId = input.ReadInt32();
							break;
						case 24u:
							SummoningLevel = input.ReadInt32();
							break;
						case 34u:
							if (destination_ == null)
							{
								destination_ = new CellCoord();
							}
							input.ReadMessage(destination_);
							break;
						}
					}
				}

				public string ToDiagnosticString()
				{
					return "InvokeSummoningAdminCmd";
				}
			}

			public sealed class InvokeCompanionAdminCmd : IMessage<InvokeCompanionAdminCmd>, IMessage, IEquatable<InvokeCompanionAdminCmd>, IDeepCloneable<InvokeCompanionAdminCmd>, ICustomDiagnosticMessage
			{
				private static readonly MessageParser<InvokeCompanionAdminCmd> _parser = new MessageParser<InvokeCompanionAdminCmd>((Func<InvokeCompanionAdminCmd>)(() => new InvokeCompanionAdminCmd()));

				private UnknownFieldSet _unknownFields;

				public const int DefinitionIdFieldNumber = 1;

				private int definitionId_;

				public const int OwnerEntityIdFieldNumber = 2;

				private int ownerEntityId_;

				public const int CompanionLevelFieldNumber = 3;

				private int companionLevel_;

				public const int DestinationFieldNumber = 4;

				private CellCoord destination_;

				[DebuggerNonUserCode]
				public static MessageParser<InvokeCompanionAdminCmd> Parser => _parser;

				[DebuggerNonUserCode]
				public static MessageDescriptor Descriptor => AdminRequestCmd.Descriptor.get_NestedTypes()[13];

				[DebuggerNonUserCode]
				MessageDescriptor Descriptor => Descriptor;

				[DebuggerNonUserCode]
				public int DefinitionId
				{
					get
					{
						return definitionId_;
					}
					set
					{
						definitionId_ = value;
					}
				}

				[DebuggerNonUserCode]
				public int OwnerEntityId
				{
					get
					{
						return ownerEntityId_;
					}
					set
					{
						ownerEntityId_ = value;
					}
				}

				[DebuggerNonUserCode]
				public int CompanionLevel
				{
					get
					{
						return companionLevel_;
					}
					set
					{
						companionLevel_ = value;
					}
				}

				[DebuggerNonUserCode]
				public CellCoord Destination
				{
					get
					{
						return destination_;
					}
					set
					{
						destination_ = value;
					}
				}

				[DebuggerNonUserCode]
				public InvokeCompanionAdminCmd()
				{
				}

				[DebuggerNonUserCode]
				public InvokeCompanionAdminCmd(InvokeCompanionAdminCmd other)
					: this()
				{
					definitionId_ = other.definitionId_;
					ownerEntityId_ = other.ownerEntityId_;
					companionLevel_ = other.companionLevel_;
					destination_ = ((other.destination_ != null) ? other.destination_.Clone() : null);
					_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
				}

				[DebuggerNonUserCode]
				public InvokeCompanionAdminCmd Clone()
				{
					return new InvokeCompanionAdminCmd(this);
				}

				[DebuggerNonUserCode]
				public override bool Equals(object other)
				{
					return Equals(other as InvokeCompanionAdminCmd);
				}

				[DebuggerNonUserCode]
				public bool Equals(InvokeCompanionAdminCmd other)
				{
					if (other == null)
					{
						return false;
					}
					if (other == this)
					{
						return true;
					}
					if (DefinitionId != other.DefinitionId)
					{
						return false;
					}
					if (OwnerEntityId != other.OwnerEntityId)
					{
						return false;
					}
					if (CompanionLevel != other.CompanionLevel)
					{
						return false;
					}
					if (!object.Equals(Destination, other.Destination))
					{
						return false;
					}
					return object.Equals(_unknownFields, other._unknownFields);
				}

				[DebuggerNonUserCode]
				public override int GetHashCode()
				{
					int num = 1;
					if (DefinitionId != 0)
					{
						num ^= DefinitionId.GetHashCode();
					}
					if (OwnerEntityId != 0)
					{
						num ^= OwnerEntityId.GetHashCode();
					}
					if (CompanionLevel != 0)
					{
						num ^= CompanionLevel.GetHashCode();
					}
					if (destination_ != null)
					{
						num ^= Destination.GetHashCode();
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
					if (DefinitionId != 0)
					{
						output.WriteRawTag((byte)8);
						output.WriteInt32(DefinitionId);
					}
					if (OwnerEntityId != 0)
					{
						output.WriteRawTag((byte)16);
						output.WriteInt32(OwnerEntityId);
					}
					if (CompanionLevel != 0)
					{
						output.WriteRawTag((byte)24);
						output.WriteInt32(CompanionLevel);
					}
					if (destination_ != null)
					{
						output.WriteRawTag((byte)34);
						output.WriteMessage(Destination);
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
					if (DefinitionId != 0)
					{
						num += 1 + CodedOutputStream.ComputeInt32Size(DefinitionId);
					}
					if (OwnerEntityId != 0)
					{
						num += 1 + CodedOutputStream.ComputeInt32Size(OwnerEntityId);
					}
					if (CompanionLevel != 0)
					{
						num += 1 + CodedOutputStream.ComputeInt32Size(CompanionLevel);
					}
					if (destination_ != null)
					{
						num += 1 + CodedOutputStream.ComputeMessageSize(Destination);
					}
					if (_unknownFields != null)
					{
						num += _unknownFields.CalculateSize();
					}
					return num;
				}

				[DebuggerNonUserCode]
				public void MergeFrom(InvokeCompanionAdminCmd other)
				{
					if (other == null)
					{
						return;
					}
					if (other.DefinitionId != 0)
					{
						DefinitionId = other.DefinitionId;
					}
					if (other.OwnerEntityId != 0)
					{
						OwnerEntityId = other.OwnerEntityId;
					}
					if (other.CompanionLevel != 0)
					{
						CompanionLevel = other.CompanionLevel;
					}
					if (other.destination_ != null)
					{
						if (destination_ == null)
						{
							destination_ = new CellCoord();
						}
						Destination.MergeFrom(other.Destination);
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
							DefinitionId = input.ReadInt32();
							break;
						case 16u:
							OwnerEntityId = input.ReadInt32();
							break;
						case 24u:
							CompanionLevel = input.ReadInt32();
							break;
						case 34u:
							if (destination_ == null)
							{
								destination_ = new CellCoord();
							}
							input.ReadMessage(destination_);
							break;
						}
					}
				}

				public string ToDiagnosticString()
				{
					return "InvokeCompanionAdminCmd";
				}
			}
		}

		private static readonly MessageParser<AdminRequestCmd> _parser = new MessageParser<AdminRequestCmd>((Func<AdminRequestCmd>)(() => new AdminRequestCmd()));

		private UnknownFieldSet _unknownFields;

		public const int DealDamageFieldNumber = 1;

		public const int KillFieldNumber = 2;

		public const int TeleportFieldNumber = 3;

		public const int DrawSpellsFieldNumber = 4;

		public const int DiscardSpellsFieldNumber = 5;

		public const int GainElementPointsFieldNumber = 6;

		public const int GainActionPointsFieldNumber = 7;

		public const int GainReservePointsFieldNumber = 8;

		public const int PickSpellFieldNumber = 9;

		public const int SetPropertyFieldNumber = 10;

		public const int HealFieldNumber = 11;

		public const int InvokeSummoningFieldNumber = 12;

		public const int InvokeCompanionFieldNumber = 13;

		public const int SetElementaryStateFieldNumber = 14;

		private object cmd_;

		private CmdOneofCase cmdCase_;

		[DebuggerNonUserCode]
		public static MessageParser<AdminRequestCmd> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => FightAdminProtocolReflection.Descriptor.get_MessageTypes()[0];

		[DebuggerNonUserCode]
		MessageDescriptor Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public Types.DealDamageAdminCmd DealDamage
		{
			get
			{
				if (cmdCase_ != CmdOneofCase.DealDamage)
				{
					return null;
				}
				return (Types.DealDamageAdminCmd)cmd_;
			}
			set
			{
				cmd_ = value;
				cmdCase_ = ((value != null) ? CmdOneofCase.DealDamage : CmdOneofCase.None);
			}
		}

		[DebuggerNonUserCode]
		public Types.KillAdminCmd Kill
		{
			get
			{
				if (cmdCase_ != CmdOneofCase.Kill)
				{
					return null;
				}
				return (Types.KillAdminCmd)cmd_;
			}
			set
			{
				cmd_ = value;
				cmdCase_ = ((value != null) ? CmdOneofCase.Kill : CmdOneofCase.None);
			}
		}

		[DebuggerNonUserCode]
		public Types.TeleportAdminCmd Teleport
		{
			get
			{
				if (cmdCase_ != CmdOneofCase.Teleport)
				{
					return null;
				}
				return (Types.TeleportAdminCmd)cmd_;
			}
			set
			{
				cmd_ = value;
				cmdCase_ = ((value != null) ? CmdOneofCase.Teleport : CmdOneofCase.None);
			}
		}

		[DebuggerNonUserCode]
		public Types.DrawSpellsCmd DrawSpells
		{
			get
			{
				if (cmdCase_ != CmdOneofCase.DrawSpells)
				{
					return null;
				}
				return (Types.DrawSpellsCmd)cmd_;
			}
			set
			{
				cmd_ = value;
				cmdCase_ = ((value != null) ? CmdOneofCase.DrawSpells : CmdOneofCase.None);
			}
		}

		[DebuggerNonUserCode]
		public Types.DiscardSpellsCmd DiscardSpells
		{
			get
			{
				if (cmdCase_ != CmdOneofCase.DiscardSpells)
				{
					return null;
				}
				return (Types.DiscardSpellsCmd)cmd_;
			}
			set
			{
				cmd_ = value;
				cmdCase_ = ((value != null) ? CmdOneofCase.DiscardSpells : CmdOneofCase.None);
			}
		}

		[DebuggerNonUserCode]
		public Types.GainElementPointsCmd GainElementPoints
		{
			get
			{
				if (cmdCase_ != CmdOneofCase.GainElementPoints)
				{
					return null;
				}
				return (Types.GainElementPointsCmd)cmd_;
			}
			set
			{
				cmd_ = value;
				cmdCase_ = ((value != null) ? CmdOneofCase.GainElementPoints : CmdOneofCase.None);
			}
		}

		[DebuggerNonUserCode]
		public Types.GainActionPointsCmd GainActionPoints
		{
			get
			{
				if (cmdCase_ != CmdOneofCase.GainActionPoints)
				{
					return null;
				}
				return (Types.GainActionPointsCmd)cmd_;
			}
			set
			{
				cmd_ = value;
				cmdCase_ = ((value != null) ? CmdOneofCase.GainActionPoints : CmdOneofCase.None);
			}
		}

		[DebuggerNonUserCode]
		public Types.GainReservePointsCmd GainReservePoints
		{
			get
			{
				if (cmdCase_ != CmdOneofCase.GainReservePoints)
				{
					return null;
				}
				return (Types.GainReservePointsCmd)cmd_;
			}
			set
			{
				cmd_ = value;
				cmdCase_ = ((value != null) ? CmdOneofCase.GainReservePoints : CmdOneofCase.None);
			}
		}

		[DebuggerNonUserCode]
		public Types.PickSpellCmd PickSpell
		{
			get
			{
				if (cmdCase_ != CmdOneofCase.PickSpell)
				{
					return null;
				}
				return (Types.PickSpellCmd)cmd_;
			}
			set
			{
				cmd_ = value;
				cmdCase_ = ((value != null) ? CmdOneofCase.PickSpell : CmdOneofCase.None);
			}
		}

		[DebuggerNonUserCode]
		public Types.SetPropertyCmd SetProperty
		{
			get
			{
				if (cmdCase_ != CmdOneofCase.SetProperty)
				{
					return null;
				}
				return (Types.SetPropertyCmd)cmd_;
			}
			set
			{
				cmd_ = value;
				cmdCase_ = ((value != null) ? CmdOneofCase.SetProperty : CmdOneofCase.None);
			}
		}

		[DebuggerNonUserCode]
		public Types.HealAdminCmd Heal
		{
			get
			{
				if (cmdCase_ != CmdOneofCase.Heal)
				{
					return null;
				}
				return (Types.HealAdminCmd)cmd_;
			}
			set
			{
				cmd_ = value;
				cmdCase_ = ((value != null) ? CmdOneofCase.Heal : CmdOneofCase.None);
			}
		}

		[DebuggerNonUserCode]
		public Types.InvokeSummoningAdminCmd InvokeSummoning
		{
			get
			{
				if (cmdCase_ != CmdOneofCase.InvokeSummoning)
				{
					return null;
				}
				return (Types.InvokeSummoningAdminCmd)cmd_;
			}
			set
			{
				cmd_ = value;
				cmdCase_ = ((value != null) ? CmdOneofCase.InvokeSummoning : CmdOneofCase.None);
			}
		}

		[DebuggerNonUserCode]
		public Types.InvokeCompanionAdminCmd InvokeCompanion
		{
			get
			{
				if (cmdCase_ != CmdOneofCase.InvokeCompanion)
				{
					return null;
				}
				return (Types.InvokeCompanionAdminCmd)cmd_;
			}
			set
			{
				cmd_ = value;
				cmdCase_ = ((value != null) ? CmdOneofCase.InvokeCompanion : CmdOneofCase.None);
			}
		}

		[DebuggerNonUserCode]
		public Types.SetElementaryStateAdminCmd SetElementaryState
		{
			get
			{
				if (cmdCase_ != CmdOneofCase.SetElementaryState)
				{
					return null;
				}
				return (Types.SetElementaryStateAdminCmd)cmd_;
			}
			set
			{
				cmd_ = value;
				cmdCase_ = ((value != null) ? CmdOneofCase.SetElementaryState : CmdOneofCase.None);
			}
		}

		[DebuggerNonUserCode]
		public CmdOneofCase CmdCase => cmdCase_;

		[DebuggerNonUserCode]
		public AdminRequestCmd()
		{
		}

		[DebuggerNonUserCode]
		public AdminRequestCmd(AdminRequestCmd other)
			: this()
		{
			switch (other.CmdCase)
			{
			case CmdOneofCase.DealDamage:
				DealDamage = other.DealDamage.Clone();
				break;
			case CmdOneofCase.Kill:
				Kill = other.Kill.Clone();
				break;
			case CmdOneofCase.Teleport:
				Teleport = other.Teleport.Clone();
				break;
			case CmdOneofCase.DrawSpells:
				DrawSpells = other.DrawSpells.Clone();
				break;
			case CmdOneofCase.DiscardSpells:
				DiscardSpells = other.DiscardSpells.Clone();
				break;
			case CmdOneofCase.GainElementPoints:
				GainElementPoints = other.GainElementPoints.Clone();
				break;
			case CmdOneofCase.GainActionPoints:
				GainActionPoints = other.GainActionPoints.Clone();
				break;
			case CmdOneofCase.GainReservePoints:
				GainReservePoints = other.GainReservePoints.Clone();
				break;
			case CmdOneofCase.PickSpell:
				PickSpell = other.PickSpell.Clone();
				break;
			case CmdOneofCase.SetProperty:
				SetProperty = other.SetProperty.Clone();
				break;
			case CmdOneofCase.Heal:
				Heal = other.Heal.Clone();
				break;
			case CmdOneofCase.InvokeSummoning:
				InvokeSummoning = other.InvokeSummoning.Clone();
				break;
			case CmdOneofCase.InvokeCompanion:
				InvokeCompanion = other.InvokeCompanion.Clone();
				break;
			case CmdOneofCase.SetElementaryState:
				SetElementaryState = other.SetElementaryState.Clone();
				break;
			}
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public AdminRequestCmd Clone()
		{
			return new AdminRequestCmd(this);
		}

		[DebuggerNonUserCode]
		public void ClearCmd()
		{
			cmdCase_ = CmdOneofCase.None;
			cmd_ = null;
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as AdminRequestCmd);
		}

		[DebuggerNonUserCode]
		public bool Equals(AdminRequestCmd other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (!object.Equals(DealDamage, other.DealDamage))
			{
				return false;
			}
			if (!object.Equals(Kill, other.Kill))
			{
				return false;
			}
			if (!object.Equals(Teleport, other.Teleport))
			{
				return false;
			}
			if (!object.Equals(DrawSpells, other.DrawSpells))
			{
				return false;
			}
			if (!object.Equals(DiscardSpells, other.DiscardSpells))
			{
				return false;
			}
			if (!object.Equals(GainElementPoints, other.GainElementPoints))
			{
				return false;
			}
			if (!object.Equals(GainActionPoints, other.GainActionPoints))
			{
				return false;
			}
			if (!object.Equals(GainReservePoints, other.GainReservePoints))
			{
				return false;
			}
			if (!object.Equals(PickSpell, other.PickSpell))
			{
				return false;
			}
			if (!object.Equals(SetProperty, other.SetProperty))
			{
				return false;
			}
			if (!object.Equals(Heal, other.Heal))
			{
				return false;
			}
			if (!object.Equals(InvokeSummoning, other.InvokeSummoning))
			{
				return false;
			}
			if (!object.Equals(InvokeCompanion, other.InvokeCompanion))
			{
				return false;
			}
			if (!object.Equals(SetElementaryState, other.SetElementaryState))
			{
				return false;
			}
			if (CmdCase != other.CmdCase)
			{
				return false;
			}
			return object.Equals(_unknownFields, other._unknownFields);
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (cmdCase_ == CmdOneofCase.DealDamage)
			{
				num ^= DealDamage.GetHashCode();
			}
			if (cmdCase_ == CmdOneofCase.Kill)
			{
				num ^= Kill.GetHashCode();
			}
			if (cmdCase_ == CmdOneofCase.Teleport)
			{
				num ^= Teleport.GetHashCode();
			}
			if (cmdCase_ == CmdOneofCase.DrawSpells)
			{
				num ^= DrawSpells.GetHashCode();
			}
			if (cmdCase_ == CmdOneofCase.DiscardSpells)
			{
				num ^= DiscardSpells.GetHashCode();
			}
			if (cmdCase_ == CmdOneofCase.GainElementPoints)
			{
				num ^= GainElementPoints.GetHashCode();
			}
			if (cmdCase_ == CmdOneofCase.GainActionPoints)
			{
				num ^= GainActionPoints.GetHashCode();
			}
			if (cmdCase_ == CmdOneofCase.GainReservePoints)
			{
				num ^= GainReservePoints.GetHashCode();
			}
			if (cmdCase_ == CmdOneofCase.PickSpell)
			{
				num ^= PickSpell.GetHashCode();
			}
			if (cmdCase_ == CmdOneofCase.SetProperty)
			{
				num ^= SetProperty.GetHashCode();
			}
			if (cmdCase_ == CmdOneofCase.Heal)
			{
				num ^= Heal.GetHashCode();
			}
			if (cmdCase_ == CmdOneofCase.InvokeSummoning)
			{
				num ^= InvokeSummoning.GetHashCode();
			}
			if (cmdCase_ == CmdOneofCase.InvokeCompanion)
			{
				num ^= InvokeCompanion.GetHashCode();
			}
			if (cmdCase_ == CmdOneofCase.SetElementaryState)
			{
				num ^= SetElementaryState.GetHashCode();
			}
			num ^= (int)cmdCase_;
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
			if (cmdCase_ == CmdOneofCase.DealDamage)
			{
				output.WriteRawTag((byte)10);
				output.WriteMessage(DealDamage);
			}
			if (cmdCase_ == CmdOneofCase.Kill)
			{
				output.WriteRawTag((byte)18);
				output.WriteMessage(Kill);
			}
			if (cmdCase_ == CmdOneofCase.Teleport)
			{
				output.WriteRawTag((byte)26);
				output.WriteMessage(Teleport);
			}
			if (cmdCase_ == CmdOneofCase.DrawSpells)
			{
				output.WriteRawTag((byte)34);
				output.WriteMessage(DrawSpells);
			}
			if (cmdCase_ == CmdOneofCase.DiscardSpells)
			{
				output.WriteRawTag((byte)42);
				output.WriteMessage(DiscardSpells);
			}
			if (cmdCase_ == CmdOneofCase.GainElementPoints)
			{
				output.WriteRawTag((byte)50);
				output.WriteMessage(GainElementPoints);
			}
			if (cmdCase_ == CmdOneofCase.GainActionPoints)
			{
				output.WriteRawTag((byte)58);
				output.WriteMessage(GainActionPoints);
			}
			if (cmdCase_ == CmdOneofCase.GainReservePoints)
			{
				output.WriteRawTag((byte)66);
				output.WriteMessage(GainReservePoints);
			}
			if (cmdCase_ == CmdOneofCase.PickSpell)
			{
				output.WriteRawTag((byte)74);
				output.WriteMessage(PickSpell);
			}
			if (cmdCase_ == CmdOneofCase.SetProperty)
			{
				output.WriteRawTag((byte)82);
				output.WriteMessage(SetProperty);
			}
			if (cmdCase_ == CmdOneofCase.Heal)
			{
				output.WriteRawTag((byte)90);
				output.WriteMessage(Heal);
			}
			if (cmdCase_ == CmdOneofCase.InvokeSummoning)
			{
				output.WriteRawTag((byte)98);
				output.WriteMessage(InvokeSummoning);
			}
			if (cmdCase_ == CmdOneofCase.InvokeCompanion)
			{
				output.WriteRawTag((byte)106);
				output.WriteMessage(InvokeCompanion);
			}
			if (cmdCase_ == CmdOneofCase.SetElementaryState)
			{
				output.WriteRawTag((byte)114);
				output.WriteMessage(SetElementaryState);
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
			if (cmdCase_ == CmdOneofCase.DealDamage)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(DealDamage);
			}
			if (cmdCase_ == CmdOneofCase.Kill)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(Kill);
			}
			if (cmdCase_ == CmdOneofCase.Teleport)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(Teleport);
			}
			if (cmdCase_ == CmdOneofCase.DrawSpells)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(DrawSpells);
			}
			if (cmdCase_ == CmdOneofCase.DiscardSpells)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(DiscardSpells);
			}
			if (cmdCase_ == CmdOneofCase.GainElementPoints)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(GainElementPoints);
			}
			if (cmdCase_ == CmdOneofCase.GainActionPoints)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(GainActionPoints);
			}
			if (cmdCase_ == CmdOneofCase.GainReservePoints)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(GainReservePoints);
			}
			if (cmdCase_ == CmdOneofCase.PickSpell)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(PickSpell);
			}
			if (cmdCase_ == CmdOneofCase.SetProperty)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(SetProperty);
			}
			if (cmdCase_ == CmdOneofCase.Heal)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(Heal);
			}
			if (cmdCase_ == CmdOneofCase.InvokeSummoning)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(InvokeSummoning);
			}
			if (cmdCase_ == CmdOneofCase.InvokeCompanion)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(InvokeCompanion);
			}
			if (cmdCase_ == CmdOneofCase.SetElementaryState)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(SetElementaryState);
			}
			if (_unknownFields != null)
			{
				num += _unknownFields.CalculateSize();
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(AdminRequestCmd other)
		{
			if (other == null)
			{
				return;
			}
			switch (other.CmdCase)
			{
			case CmdOneofCase.DealDamage:
				if (DealDamage == null)
				{
					DealDamage = new Types.DealDamageAdminCmd();
				}
				DealDamage.MergeFrom(other.DealDamage);
				break;
			case CmdOneofCase.Kill:
				if (Kill == null)
				{
					Kill = new Types.KillAdminCmd();
				}
				Kill.MergeFrom(other.Kill);
				break;
			case CmdOneofCase.Teleport:
				if (Teleport == null)
				{
					Teleport = new Types.TeleportAdminCmd();
				}
				Teleport.MergeFrom(other.Teleport);
				break;
			case CmdOneofCase.DrawSpells:
				if (DrawSpells == null)
				{
					DrawSpells = new Types.DrawSpellsCmd();
				}
				DrawSpells.MergeFrom(other.DrawSpells);
				break;
			case CmdOneofCase.DiscardSpells:
				if (DiscardSpells == null)
				{
					DiscardSpells = new Types.DiscardSpellsCmd();
				}
				DiscardSpells.MergeFrom(other.DiscardSpells);
				break;
			case CmdOneofCase.GainElementPoints:
				if (GainElementPoints == null)
				{
					GainElementPoints = new Types.GainElementPointsCmd();
				}
				GainElementPoints.MergeFrom(other.GainElementPoints);
				break;
			case CmdOneofCase.GainActionPoints:
				if (GainActionPoints == null)
				{
					GainActionPoints = new Types.GainActionPointsCmd();
				}
				GainActionPoints.MergeFrom(other.GainActionPoints);
				break;
			case CmdOneofCase.GainReservePoints:
				if (GainReservePoints == null)
				{
					GainReservePoints = new Types.GainReservePointsCmd();
				}
				GainReservePoints.MergeFrom(other.GainReservePoints);
				break;
			case CmdOneofCase.PickSpell:
				if (PickSpell == null)
				{
					PickSpell = new Types.PickSpellCmd();
				}
				PickSpell.MergeFrom(other.PickSpell);
				break;
			case CmdOneofCase.SetProperty:
				if (SetProperty == null)
				{
					SetProperty = new Types.SetPropertyCmd();
				}
				SetProperty.MergeFrom(other.SetProperty);
				break;
			case CmdOneofCase.Heal:
				if (Heal == null)
				{
					Heal = new Types.HealAdminCmd();
				}
				Heal.MergeFrom(other.Heal);
				break;
			case CmdOneofCase.InvokeSummoning:
				if (InvokeSummoning == null)
				{
					InvokeSummoning = new Types.InvokeSummoningAdminCmd();
				}
				InvokeSummoning.MergeFrom(other.InvokeSummoning);
				break;
			case CmdOneofCase.InvokeCompanion:
				if (InvokeCompanion == null)
				{
					InvokeCompanion = new Types.InvokeCompanionAdminCmd();
				}
				InvokeCompanion.MergeFrom(other.InvokeCompanion);
				break;
			case CmdOneofCase.SetElementaryState:
				if (SetElementaryState == null)
				{
					SetElementaryState = new Types.SetElementaryStateAdminCmd();
				}
				SetElementaryState.MergeFrom(other.SetElementaryState);
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
					Types.DealDamageAdminCmd dealDamageAdminCmd = new Types.DealDamageAdminCmd();
					if (cmdCase_ == CmdOneofCase.DealDamage)
					{
						dealDamageAdminCmd.MergeFrom(DealDamage);
					}
					input.ReadMessage(dealDamageAdminCmd);
					DealDamage = dealDamageAdminCmd;
					break;
				}
				case 18u:
				{
					Types.KillAdminCmd killAdminCmd = new Types.KillAdminCmd();
					if (cmdCase_ == CmdOneofCase.Kill)
					{
						killAdminCmd.MergeFrom(Kill);
					}
					input.ReadMessage(killAdminCmd);
					Kill = killAdminCmd;
					break;
				}
				case 26u:
				{
					Types.TeleportAdminCmd teleportAdminCmd = new Types.TeleportAdminCmd();
					if (cmdCase_ == CmdOneofCase.Teleport)
					{
						teleportAdminCmd.MergeFrom(Teleport);
					}
					input.ReadMessage(teleportAdminCmd);
					Teleport = teleportAdminCmd;
					break;
				}
				case 34u:
				{
					Types.DrawSpellsCmd drawSpellsCmd = new Types.DrawSpellsCmd();
					if (cmdCase_ == CmdOneofCase.DrawSpells)
					{
						drawSpellsCmd.MergeFrom(DrawSpells);
					}
					input.ReadMessage(drawSpellsCmd);
					DrawSpells = drawSpellsCmd;
					break;
				}
				case 42u:
				{
					Types.DiscardSpellsCmd discardSpellsCmd = new Types.DiscardSpellsCmd();
					if (cmdCase_ == CmdOneofCase.DiscardSpells)
					{
						discardSpellsCmd.MergeFrom(DiscardSpells);
					}
					input.ReadMessage(discardSpellsCmd);
					DiscardSpells = discardSpellsCmd;
					break;
				}
				case 50u:
				{
					Types.GainElementPointsCmd gainElementPointsCmd = new Types.GainElementPointsCmd();
					if (cmdCase_ == CmdOneofCase.GainElementPoints)
					{
						gainElementPointsCmd.MergeFrom(GainElementPoints);
					}
					input.ReadMessage(gainElementPointsCmd);
					GainElementPoints = gainElementPointsCmd;
					break;
				}
				case 58u:
				{
					Types.GainActionPointsCmd gainActionPointsCmd = new Types.GainActionPointsCmd();
					if (cmdCase_ == CmdOneofCase.GainActionPoints)
					{
						gainActionPointsCmd.MergeFrom(GainActionPoints);
					}
					input.ReadMessage(gainActionPointsCmd);
					GainActionPoints = gainActionPointsCmd;
					break;
				}
				case 66u:
				{
					Types.GainReservePointsCmd gainReservePointsCmd = new Types.GainReservePointsCmd();
					if (cmdCase_ == CmdOneofCase.GainReservePoints)
					{
						gainReservePointsCmd.MergeFrom(GainReservePoints);
					}
					input.ReadMessage(gainReservePointsCmd);
					GainReservePoints = gainReservePointsCmd;
					break;
				}
				case 74u:
				{
					Types.PickSpellCmd pickSpellCmd = new Types.PickSpellCmd();
					if (cmdCase_ == CmdOneofCase.PickSpell)
					{
						pickSpellCmd.MergeFrom(PickSpell);
					}
					input.ReadMessage(pickSpellCmd);
					PickSpell = pickSpellCmd;
					break;
				}
				case 82u:
				{
					Types.SetPropertyCmd setPropertyCmd = new Types.SetPropertyCmd();
					if (cmdCase_ == CmdOneofCase.SetProperty)
					{
						setPropertyCmd.MergeFrom(SetProperty);
					}
					input.ReadMessage(setPropertyCmd);
					SetProperty = setPropertyCmd;
					break;
				}
				case 90u:
				{
					Types.HealAdminCmd healAdminCmd = new Types.HealAdminCmd();
					if (cmdCase_ == CmdOneofCase.Heal)
					{
						healAdminCmd.MergeFrom(Heal);
					}
					input.ReadMessage(healAdminCmd);
					Heal = healAdminCmd;
					break;
				}
				case 98u:
				{
					Types.InvokeSummoningAdminCmd invokeSummoningAdminCmd = new Types.InvokeSummoningAdminCmd();
					if (cmdCase_ == CmdOneofCase.InvokeSummoning)
					{
						invokeSummoningAdminCmd.MergeFrom(InvokeSummoning);
					}
					input.ReadMessage(invokeSummoningAdminCmd);
					InvokeSummoning = invokeSummoningAdminCmd;
					break;
				}
				case 106u:
				{
					Types.InvokeCompanionAdminCmd invokeCompanionAdminCmd = new Types.InvokeCompanionAdminCmd();
					if (cmdCase_ == CmdOneofCase.InvokeCompanion)
					{
						invokeCompanionAdminCmd.MergeFrom(InvokeCompanion);
					}
					input.ReadMessage(invokeCompanionAdminCmd);
					InvokeCompanion = invokeCompanionAdminCmd;
					break;
				}
				case 114u:
				{
					Types.SetElementaryStateAdminCmd setElementaryStateAdminCmd = new Types.SetElementaryStateAdminCmd();
					if (cmdCase_ == CmdOneofCase.SetElementaryState)
					{
						setElementaryStateAdminCmd.MergeFrom(SetElementaryState);
					}
					input.ReadMessage(setElementaryStateAdminCmd);
					SetElementaryState = setElementaryStateAdminCmd;
					break;
				}
				}
			}
		}

		public string ToDiagnosticString()
		{
			return "AdminRequestCmd";
		}
	}
}
