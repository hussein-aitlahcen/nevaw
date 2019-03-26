using Google.Protobuf;
using Google.Protobuf.Reflection;
using System;
using System.Diagnostics;

namespace Ankama.Cube.Protocols.AdminCommandsProtocol
{
	public sealed class AdminCmd : IMessage<AdminCmd>, IMessage, IEquatable<AdminCmd>, IDeepCloneable<AdminCmd>, ICustomDiagnosticMessage
	{
		public enum CmdOneofCase
		{
			None = 0,
			GiveAllCompanions = 2,
			GiveAllWeapons = 3,
			SetWeaponLevel = 4,
			SetAllWeaponLevels = 5,
			SetGender = 6
		}

		[DebuggerNonUserCode]
		public static class Types
		{
			public sealed class SetLevel : IMessage<SetLevel>, IMessage, IEquatable<SetLevel>, IDeepCloneable<SetLevel>, ICustomDiagnosticMessage
			{
				private static readonly MessageParser<SetLevel> _parser = new MessageParser<SetLevel>((Func<SetLevel>)(() => new SetLevel()));

				private UnknownFieldSet _unknownFields;

				public const int IdFieldNumber = 1;

				private int id_;

				public const int LevelFieldNumber = 2;

				private int level_;

				[DebuggerNonUserCode]
				public static MessageParser<SetLevel> Parser => _parser;

				[DebuggerNonUserCode]
				public static MessageDescriptor Descriptor => AdminCmd.Descriptor.get_NestedTypes()[0];

				[DebuggerNonUserCode]
				MessageDescriptor Descriptor => Descriptor;

				[DebuggerNonUserCode]
				public int Id
				{
					get
					{
						return id_;
					}
					set
					{
						id_ = value;
					}
				}

				[DebuggerNonUserCode]
				public int Level
				{
					get
					{
						return level_;
					}
					set
					{
						level_ = value;
					}
				}

				[DebuggerNonUserCode]
				public SetLevel()
				{
				}

				[DebuggerNonUserCode]
				public SetLevel(SetLevel other)
					: this()
				{
					id_ = other.id_;
					level_ = other.level_;
					_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
				}

				[DebuggerNonUserCode]
				public SetLevel Clone()
				{
					return new SetLevel(this);
				}

				[DebuggerNonUserCode]
				public override bool Equals(object other)
				{
					return Equals(other as SetLevel);
				}

				[DebuggerNonUserCode]
				public bool Equals(SetLevel other)
				{
					if (other == null)
					{
						return false;
					}
					if (other == this)
					{
						return true;
					}
					if (Id != other.Id)
					{
						return false;
					}
					if (Level != other.Level)
					{
						return false;
					}
					return object.Equals(_unknownFields, other._unknownFields);
				}

				[DebuggerNonUserCode]
				public override int GetHashCode()
				{
					int num = 1;
					if (Id != 0)
					{
						num ^= Id.GetHashCode();
					}
					if (Level != 0)
					{
						num ^= Level.GetHashCode();
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
					if (Id != 0)
					{
						output.WriteRawTag((byte)8);
						output.WriteInt32(Id);
					}
					if (Level != 0)
					{
						output.WriteRawTag((byte)16);
						output.WriteInt32(Level);
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
					if (Id != 0)
					{
						num += 1 + CodedOutputStream.ComputeInt32Size(Id);
					}
					if (Level != 0)
					{
						num += 1 + CodedOutputStream.ComputeInt32Size(Level);
					}
					if (_unknownFields != null)
					{
						num += _unknownFields.CalculateSize();
					}
					return num;
				}

				[DebuggerNonUserCode]
				public void MergeFrom(SetLevel other)
				{
					if (other != null)
					{
						if (other.Id != 0)
						{
							Id = other.Id;
						}
						if (other.Level != 0)
						{
							Level = other.Level;
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
							Id = input.ReadInt32();
							break;
						case 16u:
							Level = input.ReadInt32();
							break;
						}
					}
				}

				public string ToDiagnosticString()
				{
					return "SetLevel";
				}
			}

			public sealed class SetAllLevels : IMessage<SetAllLevels>, IMessage, IEquatable<SetAllLevels>, IDeepCloneable<SetAllLevels>, ICustomDiagnosticMessage
			{
				private static readonly MessageParser<SetAllLevels> _parser = new MessageParser<SetAllLevels>((Func<SetAllLevels>)(() => new SetAllLevels()));

				private UnknownFieldSet _unknownFields;

				public const int LevelFieldNumber = 1;

				private int level_;

				[DebuggerNonUserCode]
				public static MessageParser<SetAllLevels> Parser => _parser;

				[DebuggerNonUserCode]
				public static MessageDescriptor Descriptor => AdminCmd.Descriptor.get_NestedTypes()[1];

				[DebuggerNonUserCode]
				MessageDescriptor Descriptor => Descriptor;

				[DebuggerNonUserCode]
				public int Level
				{
					get
					{
						return level_;
					}
					set
					{
						level_ = value;
					}
				}

				[DebuggerNonUserCode]
				public SetAllLevels()
				{
				}

				[DebuggerNonUserCode]
				public SetAllLevels(SetAllLevels other)
					: this()
				{
					level_ = other.level_;
					_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
				}

				[DebuggerNonUserCode]
				public SetAllLevels Clone()
				{
					return new SetAllLevels(this);
				}

				[DebuggerNonUserCode]
				public override bool Equals(object other)
				{
					return Equals(other as SetAllLevels);
				}

				[DebuggerNonUserCode]
				public bool Equals(SetAllLevels other)
				{
					if (other == null)
					{
						return false;
					}
					if (other == this)
					{
						return true;
					}
					if (Level != other.Level)
					{
						return false;
					}
					return object.Equals(_unknownFields, other._unknownFields);
				}

				[DebuggerNonUserCode]
				public override int GetHashCode()
				{
					int num = 1;
					if (Level != 0)
					{
						num ^= Level.GetHashCode();
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
					if (Level != 0)
					{
						output.WriteRawTag((byte)8);
						output.WriteInt32(Level);
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
					if (Level != 0)
					{
						num += 1 + CodedOutputStream.ComputeInt32Size(Level);
					}
					if (_unknownFields != null)
					{
						num += _unknownFields.CalculateSize();
					}
					return num;
				}

				[DebuggerNonUserCode]
				public void MergeFrom(SetAllLevels other)
				{
					if (other != null)
					{
						if (other.Level != 0)
						{
							Level = other.Level;
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
							Level = input.ReadInt32();
						}
					}
				}

				public string ToDiagnosticString()
				{
					return "SetAllLevels";
				}
			}

			public sealed class SetGender : IMessage<SetGender>, IMessage, IEquatable<SetGender>, IDeepCloneable<SetGender>, ICustomDiagnosticMessage
			{
				private static readonly MessageParser<SetGender> _parser = new MessageParser<SetGender>((Func<SetGender>)(() => new SetGender()));

				private UnknownFieldSet _unknownFields;

				public const int GenderFieldNumber = 1;

				private int gender_;

				[DebuggerNonUserCode]
				public static MessageParser<SetGender> Parser => _parser;

				[DebuggerNonUserCode]
				public static MessageDescriptor Descriptor => AdminCmd.Descriptor.get_NestedTypes()[2];

				[DebuggerNonUserCode]
				MessageDescriptor Descriptor => Descriptor;

				[DebuggerNonUserCode]
				public int Gender
				{
					get
					{
						return gender_;
					}
					set
					{
						gender_ = value;
					}
				}

				[DebuggerNonUserCode]
				public SetGender()
				{
				}

				[DebuggerNonUserCode]
				public SetGender(SetGender other)
					: this()
				{
					gender_ = other.gender_;
					_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
				}

				[DebuggerNonUserCode]
				public SetGender Clone()
				{
					return new SetGender(this);
				}

				[DebuggerNonUserCode]
				public override bool Equals(object other)
				{
					return Equals(other as SetGender);
				}

				[DebuggerNonUserCode]
				public bool Equals(SetGender other)
				{
					if (other == null)
					{
						return false;
					}
					if (other == this)
					{
						return true;
					}
					if (Gender != other.Gender)
					{
						return false;
					}
					return object.Equals(_unknownFields, other._unknownFields);
				}

				[DebuggerNonUserCode]
				public override int GetHashCode()
				{
					int num = 1;
					if (Gender != 0)
					{
						num ^= Gender.GetHashCode();
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
					if (Gender != 0)
					{
						output.WriteRawTag((byte)8);
						output.WriteInt32(Gender);
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
					if (Gender != 0)
					{
						num += 1 + CodedOutputStream.ComputeInt32Size(Gender);
					}
					if (_unknownFields != null)
					{
						num += _unknownFields.CalculateSize();
					}
					return num;
				}

				[DebuggerNonUserCode]
				public void MergeFrom(SetGender other)
				{
					if (other != null)
					{
						if (other.Gender != 0)
						{
							Gender = other.Gender;
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
							Gender = input.ReadInt32();
						}
					}
				}

				public string ToDiagnosticString()
				{
					return "SetGender";
				}
			}
		}

		private static readonly MessageParser<AdminCmd> _parser = new MessageParser<AdminCmd>((Func<AdminCmd>)(() => new AdminCmd()));

		private UnknownFieldSet _unknownFields;

		public const int IdFieldNumber = 1;

		private int id_;

		public const int GiveAllCompanionsFieldNumber = 2;

		public const int GiveAllWeaponsFieldNumber = 3;

		public const int SetWeaponLevelFieldNumber = 4;

		public const int SetAllWeaponLevelsFieldNumber = 5;

		public const int SetGenderFieldNumber = 6;

		private object cmd_;

		private CmdOneofCase cmdCase_;

		[DebuggerNonUserCode]
		public static MessageParser<AdminCmd> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => AdminCommandsProtocolReflection.Descriptor.get_MessageTypes()[0];

		[DebuggerNonUserCode]
		MessageDescriptor Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public int Id
		{
			get
			{
				return id_;
			}
			set
			{
				id_ = value;
			}
		}

		[DebuggerNonUserCode]
		public bool GiveAllCompanions
		{
			get
			{
				if (cmdCase_ != CmdOneofCase.GiveAllCompanions)
				{
					return false;
				}
				return (bool)cmd_;
			}
			set
			{
				cmd_ = value;
				cmdCase_ = CmdOneofCase.GiveAllCompanions;
			}
		}

		[DebuggerNonUserCode]
		public bool GiveAllWeapons
		{
			get
			{
				if (cmdCase_ != CmdOneofCase.GiveAllWeapons)
				{
					return false;
				}
				return (bool)cmd_;
			}
			set
			{
				cmd_ = value;
				cmdCase_ = CmdOneofCase.GiveAllWeapons;
			}
		}

		[DebuggerNonUserCode]
		public Types.SetLevel SetWeaponLevel
		{
			get
			{
				if (cmdCase_ != CmdOneofCase.SetWeaponLevel)
				{
					return null;
				}
				return (Types.SetLevel)cmd_;
			}
			set
			{
				cmd_ = value;
				cmdCase_ = ((value != null) ? CmdOneofCase.SetWeaponLevel : CmdOneofCase.None);
			}
		}

		[DebuggerNonUserCode]
		public Types.SetAllLevels SetAllWeaponLevels
		{
			get
			{
				if (cmdCase_ != CmdOneofCase.SetAllWeaponLevels)
				{
					return null;
				}
				return (Types.SetAllLevels)cmd_;
			}
			set
			{
				cmd_ = value;
				cmdCase_ = ((value != null) ? CmdOneofCase.SetAllWeaponLevels : CmdOneofCase.None);
			}
		}

		[DebuggerNonUserCode]
		public Types.SetGender SetGender
		{
			get
			{
				if (cmdCase_ != CmdOneofCase.SetGender)
				{
					return null;
				}
				return (Types.SetGender)cmd_;
			}
			set
			{
				cmd_ = value;
				cmdCase_ = ((value != null) ? CmdOneofCase.SetGender : CmdOneofCase.None);
			}
		}

		[DebuggerNonUserCode]
		public CmdOneofCase CmdCase => cmdCase_;

		[DebuggerNonUserCode]
		public AdminCmd()
		{
		}

		[DebuggerNonUserCode]
		public AdminCmd(AdminCmd other)
			: this()
		{
			id_ = other.id_;
			switch (other.CmdCase)
			{
			case CmdOneofCase.GiveAllCompanions:
				GiveAllCompanions = other.GiveAllCompanions;
				break;
			case CmdOneofCase.GiveAllWeapons:
				GiveAllWeapons = other.GiveAllWeapons;
				break;
			case CmdOneofCase.SetWeaponLevel:
				SetWeaponLevel = other.SetWeaponLevel.Clone();
				break;
			case CmdOneofCase.SetAllWeaponLevels:
				SetAllWeaponLevels = other.SetAllWeaponLevels.Clone();
				break;
			case CmdOneofCase.SetGender:
				SetGender = other.SetGender.Clone();
				break;
			}
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public AdminCmd Clone()
		{
			return new AdminCmd(this);
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
			return Equals(other as AdminCmd);
		}

		[DebuggerNonUserCode]
		public bool Equals(AdminCmd other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (Id != other.Id)
			{
				return false;
			}
			if (GiveAllCompanions != other.GiveAllCompanions)
			{
				return false;
			}
			if (GiveAllWeapons != other.GiveAllWeapons)
			{
				return false;
			}
			if (!object.Equals(SetWeaponLevel, other.SetWeaponLevel))
			{
				return false;
			}
			if (!object.Equals(SetAllWeaponLevels, other.SetAllWeaponLevels))
			{
				return false;
			}
			if (!object.Equals(SetGender, other.SetGender))
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
			if (Id != 0)
			{
				num ^= Id.GetHashCode();
			}
			if (cmdCase_ == CmdOneofCase.GiveAllCompanions)
			{
				num ^= GiveAllCompanions.GetHashCode();
			}
			if (cmdCase_ == CmdOneofCase.GiveAllWeapons)
			{
				num ^= GiveAllWeapons.GetHashCode();
			}
			if (cmdCase_ == CmdOneofCase.SetWeaponLevel)
			{
				num ^= SetWeaponLevel.GetHashCode();
			}
			if (cmdCase_ == CmdOneofCase.SetAllWeaponLevels)
			{
				num ^= SetAllWeaponLevels.GetHashCode();
			}
			if (cmdCase_ == CmdOneofCase.SetGender)
			{
				num ^= SetGender.GetHashCode();
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
			if (Id != 0)
			{
				output.WriteRawTag((byte)8);
				output.WriteInt32(Id);
			}
			if (cmdCase_ == CmdOneofCase.GiveAllCompanions)
			{
				output.WriteRawTag((byte)16);
				output.WriteBool(GiveAllCompanions);
			}
			if (cmdCase_ == CmdOneofCase.GiveAllWeapons)
			{
				output.WriteRawTag((byte)24);
				output.WriteBool(GiveAllWeapons);
			}
			if (cmdCase_ == CmdOneofCase.SetWeaponLevel)
			{
				output.WriteRawTag((byte)34);
				output.WriteMessage(SetWeaponLevel);
			}
			if (cmdCase_ == CmdOneofCase.SetAllWeaponLevels)
			{
				output.WriteRawTag((byte)42);
				output.WriteMessage(SetAllWeaponLevels);
			}
			if (cmdCase_ == CmdOneofCase.SetGender)
			{
				output.WriteRawTag((byte)50);
				output.WriteMessage(SetGender);
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
			if (Id != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Id);
			}
			if (cmdCase_ == CmdOneofCase.GiveAllCompanions)
			{
				num += 2;
			}
			if (cmdCase_ == CmdOneofCase.GiveAllWeapons)
			{
				num += 2;
			}
			if (cmdCase_ == CmdOneofCase.SetWeaponLevel)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(SetWeaponLevel);
			}
			if (cmdCase_ == CmdOneofCase.SetAllWeaponLevels)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(SetAllWeaponLevels);
			}
			if (cmdCase_ == CmdOneofCase.SetGender)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(SetGender);
			}
			if (_unknownFields != null)
			{
				num += _unknownFields.CalculateSize();
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(AdminCmd other)
		{
			if (other == null)
			{
				return;
			}
			if (other.Id != 0)
			{
				Id = other.Id;
			}
			switch (other.CmdCase)
			{
			case CmdOneofCase.GiveAllCompanions:
				GiveAllCompanions = other.GiveAllCompanions;
				break;
			case CmdOneofCase.GiveAllWeapons:
				GiveAllWeapons = other.GiveAllWeapons;
				break;
			case CmdOneofCase.SetWeaponLevel:
				if (SetWeaponLevel == null)
				{
					SetWeaponLevel = new Types.SetLevel();
				}
				SetWeaponLevel.MergeFrom(other.SetWeaponLevel);
				break;
			case CmdOneofCase.SetAllWeaponLevels:
				if (SetAllWeaponLevels == null)
				{
					SetAllWeaponLevels = new Types.SetAllLevels();
				}
				SetAllWeaponLevels.MergeFrom(other.SetAllWeaponLevels);
				break;
			case CmdOneofCase.SetGender:
				if (SetGender == null)
				{
					SetGender = new Types.SetGender();
				}
				SetGender.MergeFrom(other.SetGender);
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
				case 8u:
					Id = input.ReadInt32();
					break;
				case 16u:
					GiveAllCompanions = input.ReadBool();
					break;
				case 24u:
					GiveAllWeapons = input.ReadBool();
					break;
				case 34u:
				{
					Types.SetLevel setLevel = new Types.SetLevel();
					if (cmdCase_ == CmdOneofCase.SetWeaponLevel)
					{
						setLevel.MergeFrom(SetWeaponLevel);
					}
					input.ReadMessage(setLevel);
					SetWeaponLevel = setLevel;
					break;
				}
				case 42u:
				{
					Types.SetAllLevels setAllLevels = new Types.SetAllLevels();
					if (cmdCase_ == CmdOneofCase.SetAllWeaponLevels)
					{
						setAllLevels.MergeFrom(SetAllWeaponLevels);
					}
					input.ReadMessage(setAllLevels);
					SetAllWeaponLevels = setAllLevels;
					break;
				}
				case 50u:
				{
					Types.SetGender setGender = new Types.SetGender();
					if (cmdCase_ == CmdOneofCase.SetGender)
					{
						setGender.MergeFrom(SetGender);
					}
					input.ReadMessage(setGender);
					SetGender = setGender;
					break;
				}
				}
			}
		}

		public string ToDiagnosticString()
		{
			return "AdminCmd";
		}
	}
}
