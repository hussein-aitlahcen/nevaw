using Ankama.Cube.Protocols.CommonProtocol;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Ankama.Cube.Protocols.FightProtocol
{
	public sealed class FightSnapshot : IMessage<FightSnapshot>, IMessage, IEquatable<FightSnapshot>, IDeepCloneable<FightSnapshot>, ICustomDiagnosticMessage
	{
		[DebuggerNonUserCode]
		public static class Types
		{
			public sealed class EntitySnapshot : IMessage<EntitySnapshot>, IMessage, IEquatable<EntitySnapshot>, IDeepCloneable<EntitySnapshot>, ICustomDiagnosticMessage
			{
				private static readonly MessageParser<EntitySnapshot> _parser = new MessageParser<EntitySnapshot>((Func<EntitySnapshot>)(() => new EntitySnapshot()));

				private UnknownFieldSet _unknownFields;

				public const int EntityIdFieldNumber = 1;

				private int entityId_;

				public const int EntityTypeFieldNumber = 2;

				private int entityType_;

				public const int NameFieldNumber = 3;

				private static readonly FieldCodec<string> _single_name_codec = FieldCodec.ForClassWrapper<string>(26u);

				private string name_;

				public const int DefIdFieldNumber = 4;

				private static readonly FieldCodec<int?> _single_defId_codec = FieldCodec.ForStructWrapper<int>(34u);

				private int? defId_;

				public const int WeaponIdFieldNumber = 5;

				private static readonly FieldCodec<int?> _single_weaponId_codec = FieldCodec.ForStructWrapper<int>(42u);

				private int? weaponId_;

				public const int GenderIdFieldNumber = 6;

				private static readonly FieldCodec<int?> _single_genderId_codec = FieldCodec.ForStructWrapper<int>(50u);

				private int? genderId_;

				public const int PlayerIndexInFightFieldNumber = 7;

				private static readonly FieldCodec<int?> _single_playerIndexInFight_codec = FieldCodec.ForStructWrapper<int>(58u);

				private int? playerIndexInFight_;

				public const int OwnerIdFieldNumber = 8;

				private static readonly FieldCodec<int?> _single_ownerId_codec = FieldCodec.ForStructWrapper<int>(66u);

				private int? ownerId_;

				public const int TeamIdFieldNumber = 9;

				private static readonly FieldCodec<int?> _single_teamId_codec = FieldCodec.ForStructWrapper<int>(74u);

				private int? teamId_;

				public const int LevelFieldNumber = 10;

				private static readonly FieldCodec<int?> _single_level_codec = FieldCodec.ForStructWrapper<int>(82u);

				private int? level_;

				public const int PropertiesFieldNumber = 11;

				private static readonly FieldCodec<int> _repeated_properties_codec = FieldCodec.ForInt32(90u);

				private readonly RepeatedField<int> properties_ = new RepeatedField<int>();

				public const int PositionFieldNumber = 12;

				private CellCoord position_;

				public const int DirectionFieldNumber = 13;

				private static readonly FieldCodec<int?> _single_direction_codec = FieldCodec.ForStructWrapper<int>(106u);

				private int? direction_;

				public const int CaracsFieldNumber = 14;

				private static readonly Codec<int, int> _map_caracs_codec = new Codec<int, int>(FieldCodec.ForInt32(8u), FieldCodec.ForInt32(16u), 114u);

				private readonly MapField<int, int> caracs_ = new MapField<int, int>();

				public const int CustomSkinFieldNumber = 15;

				private static readonly FieldCodec<string> _single_customSkin_codec = FieldCodec.ForClassWrapper<string>(122u);

				private string customSkin_;

				public const int ActionDoneThisTurnFieldNumber = 16;

				private static readonly FieldCodec<bool?> _single_actionDoneThisTurn_codec = FieldCodec.ForStructWrapper<bool>(130u);

				private bool? actionDoneThisTurn_;

				[DebuggerNonUserCode]
				public static MessageParser<EntitySnapshot> Parser => _parser;

				[DebuggerNonUserCode]
				public static MessageDescriptor Descriptor => FightSnapshot.Descriptor.get_NestedTypes()[2];

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
				public int EntityType
				{
					get
					{
						return entityType_;
					}
					set
					{
						entityType_ = value;
					}
				}

				[DebuggerNonUserCode]
				public string Name
				{
					get
					{
						return name_;
					}
					set
					{
						name_ = value;
					}
				}

				[DebuggerNonUserCode]
				public int? DefId
				{
					get
					{
						return defId_;
					}
					set
					{
						defId_ = value;
					}
				}

				[DebuggerNonUserCode]
				public int? WeaponId
				{
					get
					{
						return weaponId_;
					}
					set
					{
						weaponId_ = value;
					}
				}

				[DebuggerNonUserCode]
				public int? GenderId
				{
					get
					{
						return genderId_;
					}
					set
					{
						genderId_ = value;
					}
				}

				[DebuggerNonUserCode]
				public int? PlayerIndexInFight
				{
					get
					{
						return playerIndexInFight_;
					}
					set
					{
						playerIndexInFight_ = value;
					}
				}

				[DebuggerNonUserCode]
				public int? OwnerId
				{
					get
					{
						return ownerId_;
					}
					set
					{
						ownerId_ = value;
					}
				}

				[DebuggerNonUserCode]
				public int? TeamId
				{
					get
					{
						return teamId_;
					}
					set
					{
						teamId_ = value;
					}
				}

				[DebuggerNonUserCode]
				public int? Level
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
				public RepeatedField<int> Properties => properties_;

				[DebuggerNonUserCode]
				public CellCoord Position
				{
					get
					{
						return position_;
					}
					set
					{
						position_ = value;
					}
				}

				[DebuggerNonUserCode]
				public int? Direction
				{
					get
					{
						return direction_;
					}
					set
					{
						direction_ = value;
					}
				}

				[DebuggerNonUserCode]
				public MapField<int, int> Caracs => caracs_;

				[DebuggerNonUserCode]
				public string CustomSkin
				{
					get
					{
						return customSkin_;
					}
					set
					{
						customSkin_ = value;
					}
				}

				[DebuggerNonUserCode]
				public bool? ActionDoneThisTurn
				{
					get
					{
						return actionDoneThisTurn_;
					}
					set
					{
						actionDoneThisTurn_ = value;
					}
				}

				[DebuggerNonUserCode]
				public EntitySnapshot()
				{
				}

				[DebuggerNonUserCode]
				public EntitySnapshot(EntitySnapshot other)
					: this()
				{
					entityId_ = other.entityId_;
					entityType_ = other.entityType_;
					Name = other.Name;
					DefId = other.DefId;
					WeaponId = other.WeaponId;
					GenderId = other.GenderId;
					PlayerIndexInFight = other.PlayerIndexInFight;
					OwnerId = other.OwnerId;
					TeamId = other.TeamId;
					Level = other.Level;
					properties_ = other.properties_.Clone();
					position_ = ((other.position_ != null) ? other.position_.Clone() : null);
					Direction = other.Direction;
					caracs_ = other.caracs_.Clone();
					CustomSkin = other.CustomSkin;
					ActionDoneThisTurn = other.ActionDoneThisTurn;
					_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
				}

				[DebuggerNonUserCode]
				public EntitySnapshot Clone()
				{
					return new EntitySnapshot(this);
				}

				[DebuggerNonUserCode]
				public override bool Equals(object other)
				{
					return Equals(other as EntitySnapshot);
				}

				[DebuggerNonUserCode]
				public bool Equals(EntitySnapshot other)
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
					if (EntityType != other.EntityType)
					{
						return false;
					}
					if (Name != other.Name)
					{
						return false;
					}
					if (DefId != other.DefId)
					{
						return false;
					}
					if (WeaponId != other.WeaponId)
					{
						return false;
					}
					if (GenderId != other.GenderId)
					{
						return false;
					}
					if (PlayerIndexInFight != other.PlayerIndexInFight)
					{
						return false;
					}
					if (OwnerId != other.OwnerId)
					{
						return false;
					}
					if (TeamId != other.TeamId)
					{
						return false;
					}
					if (Level != other.Level)
					{
						return false;
					}
					if (!properties_.Equals(other.properties_))
					{
						return false;
					}
					if (!object.Equals(Position, other.Position))
					{
						return false;
					}
					if (Direction != other.Direction)
					{
						return false;
					}
					if (!Caracs.Equals(other.Caracs))
					{
						return false;
					}
					if (CustomSkin != other.CustomSkin)
					{
						return false;
					}
					if (ActionDoneThisTurn != other.ActionDoneThisTurn)
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
					if (EntityType != 0)
					{
						num ^= EntityType.GetHashCode();
					}
					if (name_ != null)
					{
						num ^= Name.GetHashCode();
					}
					if (defId_.HasValue)
					{
						num ^= DefId.GetHashCode();
					}
					if (weaponId_.HasValue)
					{
						num ^= WeaponId.GetHashCode();
					}
					if (genderId_.HasValue)
					{
						num ^= GenderId.GetHashCode();
					}
					if (playerIndexInFight_.HasValue)
					{
						num ^= PlayerIndexInFight.GetHashCode();
					}
					if (ownerId_.HasValue)
					{
						num ^= OwnerId.GetHashCode();
					}
					if (teamId_.HasValue)
					{
						num ^= TeamId.GetHashCode();
					}
					if (level_.HasValue)
					{
						num ^= Level.GetHashCode();
					}
					num ^= ((object)properties_).GetHashCode();
					if (position_ != null)
					{
						num ^= Position.GetHashCode();
					}
					if (direction_.HasValue)
					{
						num ^= Direction.GetHashCode();
					}
					num ^= ((object)Caracs).GetHashCode();
					if (customSkin_ != null)
					{
						num ^= CustomSkin.GetHashCode();
					}
					if (actionDoneThisTurn_.HasValue)
					{
						num ^= ActionDoneThisTurn.GetHashCode();
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
					if (EntityType != 0)
					{
						output.WriteRawTag((byte)16);
						output.WriteInt32(EntityType);
					}
					if (name_ != null)
					{
						_single_name_codec.WriteTagAndValue(output, Name);
					}
					if (defId_.HasValue)
					{
						_single_defId_codec.WriteTagAndValue(output, DefId);
					}
					if (weaponId_.HasValue)
					{
						_single_weaponId_codec.WriteTagAndValue(output, WeaponId);
					}
					if (genderId_.HasValue)
					{
						_single_genderId_codec.WriteTagAndValue(output, GenderId);
					}
					if (playerIndexInFight_.HasValue)
					{
						_single_playerIndexInFight_codec.WriteTagAndValue(output, PlayerIndexInFight);
					}
					if (ownerId_.HasValue)
					{
						_single_ownerId_codec.WriteTagAndValue(output, OwnerId);
					}
					if (teamId_.HasValue)
					{
						_single_teamId_codec.WriteTagAndValue(output, TeamId);
					}
					if (level_.HasValue)
					{
						_single_level_codec.WriteTagAndValue(output, Level);
					}
					properties_.WriteTo(output, _repeated_properties_codec);
					if (position_ != null)
					{
						output.WriteRawTag((byte)98);
						output.WriteMessage(Position);
					}
					if (direction_.HasValue)
					{
						_single_direction_codec.WriteTagAndValue(output, Direction);
					}
					caracs_.WriteTo(output, _map_caracs_codec);
					if (customSkin_ != null)
					{
						_single_customSkin_codec.WriteTagAndValue(output, CustomSkin);
					}
					if (actionDoneThisTurn_.HasValue)
					{
						_single_actionDoneThisTurn_codec.WriteTagAndValue(output, ActionDoneThisTurn);
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
					if (EntityType != 0)
					{
						num += 1 + CodedOutputStream.ComputeInt32Size(EntityType);
					}
					if (name_ != null)
					{
						num += _single_name_codec.CalculateSizeWithTag(Name);
					}
					if (defId_.HasValue)
					{
						num += _single_defId_codec.CalculateSizeWithTag(DefId);
					}
					if (weaponId_.HasValue)
					{
						num += _single_weaponId_codec.CalculateSizeWithTag(WeaponId);
					}
					if (genderId_.HasValue)
					{
						num += _single_genderId_codec.CalculateSizeWithTag(GenderId);
					}
					if (playerIndexInFight_.HasValue)
					{
						num += _single_playerIndexInFight_codec.CalculateSizeWithTag(PlayerIndexInFight);
					}
					if (ownerId_.HasValue)
					{
						num += _single_ownerId_codec.CalculateSizeWithTag(OwnerId);
					}
					if (teamId_.HasValue)
					{
						num += _single_teamId_codec.CalculateSizeWithTag(TeamId);
					}
					if (level_.HasValue)
					{
						num += _single_level_codec.CalculateSizeWithTag(Level);
					}
					num += properties_.CalculateSize(_repeated_properties_codec);
					if (position_ != null)
					{
						num += 1 + CodedOutputStream.ComputeMessageSize(Position);
					}
					if (direction_.HasValue)
					{
						num += _single_direction_codec.CalculateSizeWithTag(Direction);
					}
					num += caracs_.CalculateSize(_map_caracs_codec);
					if (customSkin_ != null)
					{
						num += _single_customSkin_codec.CalculateSizeWithTag(CustomSkin);
					}
					if (actionDoneThisTurn_.HasValue)
					{
						num += _single_actionDoneThisTurn_codec.CalculateSizeWithTag(ActionDoneThisTurn);
					}
					if (_unknownFields != null)
					{
						num += _unknownFields.CalculateSize();
					}
					return num;
				}

				[DebuggerNonUserCode]
				public void MergeFrom(EntitySnapshot other)
				{
					if (other == null)
					{
						return;
					}
					if (other.EntityId != 0)
					{
						EntityId = other.EntityId;
					}
					if (other.EntityType != 0)
					{
						EntityType = other.EntityType;
					}
					if (other.name_ != null && (name_ == null || other.Name != ""))
					{
						Name = other.Name;
					}
					if (other.defId_.HasValue && (!defId_.HasValue || other.DefId != 0))
					{
						DefId = other.DefId;
					}
					if (other.weaponId_.HasValue && (!weaponId_.HasValue || other.WeaponId != 0))
					{
						WeaponId = other.WeaponId;
					}
					if (other.genderId_.HasValue && (!genderId_.HasValue || other.GenderId != 0))
					{
						GenderId = other.GenderId;
					}
					if (other.playerIndexInFight_.HasValue && (!playerIndexInFight_.HasValue || other.PlayerIndexInFight != 0))
					{
						PlayerIndexInFight = other.PlayerIndexInFight;
					}
					if (other.ownerId_.HasValue && (!ownerId_.HasValue || other.OwnerId != 0))
					{
						OwnerId = other.OwnerId;
					}
					if (other.teamId_.HasValue && (!teamId_.HasValue || other.TeamId != 0))
					{
						TeamId = other.TeamId;
					}
					if (other.level_.HasValue && (!level_.HasValue || other.Level != 0))
					{
						Level = other.Level;
					}
					properties_.Add((IEnumerable<int>)other.properties_);
					if (other.position_ != null)
					{
						if (position_ == null)
						{
							position_ = new CellCoord();
						}
						Position.MergeFrom(other.Position);
					}
					if (other.direction_.HasValue && (!direction_.HasValue || other.Direction != 0))
					{
						Direction = other.Direction;
					}
					caracs_.Add((IDictionary<int, int>)other.caracs_);
					if (other.customSkin_ != null && (customSkin_ == null || other.CustomSkin != ""))
					{
						CustomSkin = other.CustomSkin;
					}
					if (other.actionDoneThisTurn_.HasValue && (!actionDoneThisTurn_.HasValue || other.ActionDoneThisTurn != false))
					{
						ActionDoneThisTurn = other.ActionDoneThisTurn;
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
							EntityId = input.ReadInt32();
							break;
						case 16u:
							EntityType = input.ReadInt32();
							break;
						case 26u:
						{
							string text2 = _single_name_codec.Read(input);
							if (name_ == null || text2 != "")
							{
								Name = text2;
							}
							break;
						}
						case 34u:
						{
							int? num6 = _single_defId_codec.Read(input);
							if (!defId_.HasValue || num6 != 0)
							{
								DefId = num6;
							}
							break;
						}
						case 42u:
						{
							int? num5 = _single_weaponId_codec.Read(input);
							if (!weaponId_.HasValue || num5 != 0)
							{
								WeaponId = num5;
							}
							break;
						}
						case 50u:
						{
							int? num9 = _single_genderId_codec.Read(input);
							if (!genderId_.HasValue || num9 != 0)
							{
								GenderId = num9;
							}
							break;
						}
						case 58u:
						{
							int? num7 = _single_playerIndexInFight_codec.Read(input);
							if (!playerIndexInFight_.HasValue || num7 != 0)
							{
								PlayerIndexInFight = num7;
							}
							break;
						}
						case 66u:
						{
							int? num3 = _single_ownerId_codec.Read(input);
							if (!ownerId_.HasValue || num3 != 0)
							{
								OwnerId = num3;
							}
							break;
						}
						case 74u:
						{
							int? num2 = _single_teamId_codec.Read(input);
							if (!teamId_.HasValue || num2 != 0)
							{
								TeamId = num2;
							}
							break;
						}
						case 82u:
						{
							int? num8 = _single_level_codec.Read(input);
							if (!level_.HasValue || num8 != 0)
							{
								Level = num8;
							}
							break;
						}
						case 88u:
						case 90u:
							properties_.AddEntriesFrom(input, _repeated_properties_codec);
							break;
						case 98u:
							if (position_ == null)
							{
								position_ = new CellCoord();
							}
							input.ReadMessage(position_);
							break;
						case 106u:
						{
							int? num4 = _single_direction_codec.Read(input);
							if (!direction_.HasValue || num4 != 0)
							{
								Direction = num4;
							}
							break;
						}
						case 114u:
							caracs_.AddEntriesFrom(input, _map_caracs_codec);
							break;
						case 122u:
						{
							string text = _single_customSkin_codec.Read(input);
							if (customSkin_ == null || text != "")
							{
								CustomSkin = text;
							}
							break;
						}
						case 130u:
						{
							bool? flag = _single_actionDoneThisTurn_codec.Read(input);
							if (!actionDoneThisTurn_.HasValue || flag != false)
							{
								ActionDoneThisTurn = flag;
							}
							break;
						}
						}
					}
				}

				public string ToDiagnosticString()
				{
					return "EntitySnapshot";
				}
			}

			public sealed class Companions : IMessage<Companions>, IMessage, IEquatable<Companions>, IDeepCloneable<Companions>, ICustomDiagnosticMessage
			{
				private static readonly MessageParser<Companions> _parser = new MessageParser<Companions>((Func<Companions>)(() => new Companions()));

				private UnknownFieldSet _unknownFields;

				public const int AllDefIdsFieldNumber = 1;

				private static readonly FieldCodec<int> _repeated_allDefIds_codec = FieldCodec.ForInt32(10u);

				private readonly RepeatedField<int> allDefIds_ = new RepeatedField<int>();

				public const int AvailableIdsFieldNumber = 2;

				private static readonly FieldCodec<int> _repeated_availableIds_codec = FieldCodec.ForInt32(18u);

				private readonly RepeatedField<int> availableIds_ = new RepeatedField<int>();

				[DebuggerNonUserCode]
				public static MessageParser<Companions> Parser => _parser;

				[DebuggerNonUserCode]
				public static MessageDescriptor Descriptor => FightSnapshot.Descriptor.get_NestedTypes()[3];

				[DebuggerNonUserCode]
				MessageDescriptor Descriptor => Descriptor;

				[DebuggerNonUserCode]
				public RepeatedField<int> AllDefIds => allDefIds_;

				[DebuggerNonUserCode]
				public RepeatedField<int> AvailableIds => availableIds_;

				[DebuggerNonUserCode]
				public Companions()
				{
				}

				[DebuggerNonUserCode]
				public Companions(Companions other)
					: this()
				{
					allDefIds_ = other.allDefIds_.Clone();
					availableIds_ = other.availableIds_.Clone();
					_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
				}

				[DebuggerNonUserCode]
				public Companions Clone()
				{
					return new Companions(this);
				}

				[DebuggerNonUserCode]
				public override bool Equals(object other)
				{
					return Equals(other as Companions);
				}

				[DebuggerNonUserCode]
				public bool Equals(Companions other)
				{
					if (other == null)
					{
						return false;
					}
					if (other == this)
					{
						return true;
					}
					if (!allDefIds_.Equals(other.allDefIds_))
					{
						return false;
					}
					if (!availableIds_.Equals(other.availableIds_))
					{
						return false;
					}
					return object.Equals(_unknownFields, other._unknownFields);
				}

				[DebuggerNonUserCode]
				public override int GetHashCode()
				{
					int num = 1;
					num ^= ((object)allDefIds_).GetHashCode();
					num ^= ((object)availableIds_).GetHashCode();
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
					allDefIds_.WriteTo(output, _repeated_allDefIds_codec);
					availableIds_.WriteTo(output, _repeated_availableIds_codec);
					if (_unknownFields != null)
					{
						_unknownFields.WriteTo(output);
					}
				}

				[DebuggerNonUserCode]
				public int CalculateSize()
				{
					int num = 0;
					num += allDefIds_.CalculateSize(_repeated_allDefIds_codec);
					num += availableIds_.CalculateSize(_repeated_availableIds_codec);
					if (_unknownFields != null)
					{
						num += _unknownFields.CalculateSize();
					}
					return num;
				}

				[DebuggerNonUserCode]
				public void MergeFrom(Companions other)
				{
					if (other != null)
					{
						allDefIds_.Add((IEnumerable<int>)other.allDefIds_);
						availableIds_.Add((IEnumerable<int>)other.availableIds_);
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
						case 10u:
							allDefIds_.AddEntriesFrom(input, _repeated_allDefIds_codec);
							break;
						case 16u:
						case 18u:
							availableIds_.AddEntriesFrom(input, _repeated_availableIds_codec);
							break;
						}
					}
				}

				public string ToDiagnosticString()
				{
					return "Companions";
				}
			}
		}

		private static readonly MessageParser<FightSnapshot> _parser = new MessageParser<FightSnapshot>((Func<FightSnapshot>)(() => new FightSnapshot()));

		private UnknownFieldSet _unknownFields;

		public const int FightIdFieldNumber = 1;

		private int fightId_;

		public const int EntitiesFieldNumber = 2;

		private static readonly FieldCodec<Types.EntitySnapshot> _repeated_entities_codec = FieldCodec.ForMessage<Types.EntitySnapshot>(18u, Types.EntitySnapshot.Parser);

		private readonly RepeatedField<Types.EntitySnapshot> entities_ = new RepeatedField<Types.EntitySnapshot>();

		public const int TurnIndexFieldNumber = 3;

		private int turnIndex_;

		public const int TurnRemainingTimeSecFieldNumber = 4;

		private int turnRemainingTimeSec_;

		public const int PlayersCompanionsFieldNumber = 5;

		private static readonly Codec<int, Types.Companions> _map_playersCompanions_codec = new Codec<int, Types.Companions>(FieldCodec.ForInt32(8u), FieldCodec.ForMessage<Types.Companions>(18u, Types.Companions.Parser), 42u);

		private readonly MapField<int, Types.Companions> playersCompanions_ = new MapField<int, Types.Companions>();

		public const int PlayersCardsCountFieldNumber = 6;

		private static readonly Codec<int, int> _map_playersCardsCount_codec = new Codec<int, int>(FieldCodec.ForInt32(8u), FieldCodec.ForInt32(16u), 50u);

		private readonly MapField<int, int> playersCardsCount_ = new MapField<int, int>();

		[DebuggerNonUserCode]
		public static MessageParser<FightSnapshot> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => FightProtocolReflection.Descriptor.get_MessageTypes()[9];

		[DebuggerNonUserCode]
		MessageDescriptor Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public int FightId
		{
			get
			{
				return fightId_;
			}
			set
			{
				fightId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<Types.EntitySnapshot> Entities => entities_;

		[DebuggerNonUserCode]
		public int TurnIndex
		{
			get
			{
				return turnIndex_;
			}
			set
			{
				turnIndex_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int TurnRemainingTimeSec
		{
			get
			{
				return turnRemainingTimeSec_;
			}
			set
			{
				turnRemainingTimeSec_ = value;
			}
		}

		[DebuggerNonUserCode]
		public MapField<int, Types.Companions> PlayersCompanions => playersCompanions_;

		[DebuggerNonUserCode]
		public MapField<int, int> PlayersCardsCount => playersCardsCount_;

		[DebuggerNonUserCode]
		public FightSnapshot()
		{
		}

		[DebuggerNonUserCode]
		public FightSnapshot(FightSnapshot other)
			: this()
		{
			fightId_ = other.fightId_;
			entities_ = other.entities_.Clone();
			turnIndex_ = other.turnIndex_;
			turnRemainingTimeSec_ = other.turnRemainingTimeSec_;
			playersCompanions_ = other.playersCompanions_.Clone();
			playersCardsCount_ = other.playersCardsCount_.Clone();
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public FightSnapshot Clone()
		{
			return new FightSnapshot(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as FightSnapshot);
		}

		[DebuggerNonUserCode]
		public bool Equals(FightSnapshot other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (FightId != other.FightId)
			{
				return false;
			}
			if (!entities_.Equals(other.entities_))
			{
				return false;
			}
			if (TurnIndex != other.TurnIndex)
			{
				return false;
			}
			if (TurnRemainingTimeSec != other.TurnRemainingTimeSec)
			{
				return false;
			}
			if (!PlayersCompanions.Equals(other.PlayersCompanions))
			{
				return false;
			}
			if (!PlayersCardsCount.Equals(other.PlayersCardsCount))
			{
				return false;
			}
			return object.Equals(_unknownFields, other._unknownFields);
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (FightId != 0)
			{
				num ^= FightId.GetHashCode();
			}
			num ^= ((object)entities_).GetHashCode();
			if (TurnIndex != 0)
			{
				num ^= TurnIndex.GetHashCode();
			}
			if (TurnRemainingTimeSec != 0)
			{
				num ^= TurnRemainingTimeSec.GetHashCode();
			}
			num ^= ((object)PlayersCompanions).GetHashCode();
			num ^= ((object)PlayersCardsCount).GetHashCode();
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
			if (FightId != 0)
			{
				output.WriteRawTag((byte)8);
				output.WriteInt32(FightId);
			}
			entities_.WriteTo(output, _repeated_entities_codec);
			if (TurnIndex != 0)
			{
				output.WriteRawTag((byte)24);
				output.WriteInt32(TurnIndex);
			}
			if (TurnRemainingTimeSec != 0)
			{
				output.WriteRawTag((byte)32);
				output.WriteInt32(TurnRemainingTimeSec);
			}
			playersCompanions_.WriteTo(output, _map_playersCompanions_codec);
			playersCardsCount_.WriteTo(output, _map_playersCardsCount_codec);
			if (_unknownFields != null)
			{
				_unknownFields.WriteTo(output);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (FightId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(FightId);
			}
			num += entities_.CalculateSize(_repeated_entities_codec);
			if (TurnIndex != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(TurnIndex);
			}
			if (TurnRemainingTimeSec != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(TurnRemainingTimeSec);
			}
			num += playersCompanions_.CalculateSize(_map_playersCompanions_codec);
			num += playersCardsCount_.CalculateSize(_map_playersCardsCount_codec);
			if (_unknownFields != null)
			{
				num += _unknownFields.CalculateSize();
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(FightSnapshot other)
		{
			if (other != null)
			{
				if (other.FightId != 0)
				{
					FightId = other.FightId;
				}
				entities_.Add((IEnumerable<Types.EntitySnapshot>)other.entities_);
				if (other.TurnIndex != 0)
				{
					TurnIndex = other.TurnIndex;
				}
				if (other.TurnRemainingTimeSec != 0)
				{
					TurnRemainingTimeSec = other.TurnRemainingTimeSec;
				}
				playersCompanions_.Add((IDictionary<int, Types.Companions>)other.playersCompanions_);
				playersCardsCount_.Add((IDictionary<int, int>)other.playersCardsCount_);
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
					FightId = input.ReadInt32();
					break;
				case 18u:
					entities_.AddEntriesFrom(input, _repeated_entities_codec);
					break;
				case 24u:
					TurnIndex = input.ReadInt32();
					break;
				case 32u:
					TurnRemainingTimeSec = input.ReadInt32();
					break;
				case 42u:
					playersCompanions_.AddEntriesFrom(input, _map_playersCompanions_codec);
					break;
				case 50u:
					playersCardsCount_.AddEntriesFrom(input, _map_playersCardsCount_codec);
					break;
				}
			}
		}

		public string ToDiagnosticString()
		{
			return "FightSnapshot";
		}
	}
}
