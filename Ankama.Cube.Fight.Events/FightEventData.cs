using Ankama.Cube.Protocols.CommonProtocol;
using Ankama.Cube.Protocols.FightCommonProtocol;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Ankama.Cube.Fight.Events
{
	public sealed class FightEventData : IMessage<FightEventData>, IMessage, IEquatable<FightEventData>, IDeepCloneable<FightEventData>, ICustomDiagnosticMessage
	{
		[DebuggerNonUserCode]
		public static class Types
		{
			public enum EventType
			{
				[OriginalName("EFFECT_STOPPED")]
				EffectStopped = 0,
				[OriginalName("TURN_STARTED")]
				TurnStarted = 2,
				[OriginalName("ENTITY_AREA_MOVED")]
				EntityAreaMoved = 3,
				[OriginalName("PLAYER_ADDED")]
				PlayerAdded = 4,
				[OriginalName("HERO_ADDED")]
				HeroAdded = 5,
				[OriginalName("COMPANION_ADDED")]
				CompanionAdded = 6,
				[OriginalName("ENTITY_ACTIONED")]
				EntityActioned = 7,
				[OriginalName("SPELLS_MOVED")]
				SpellsMoved = 8,
				[OriginalName("TEAM_TURN_ENDED")]
				TeamTurnEnded = 9,
				[OriginalName("PLAY_SPELL")]
				PlaySpell = 10,
				[OriginalName("TURN_ENDED")]
				TurnEnded = 11,
				[OriginalName("ARMORED_LIFE_CHANGED")]
				ArmoredLifeChanged = 12,
				[OriginalName("ENTITY_REMOVED")]
				EntityRemoved = 13,
				[OriginalName("ELEMENT_POINTS_CHANGED")]
				ElementPointsChanged = 14,
				[OriginalName("COMPANION_ADDED_IN_RESERVE")]
				CompanionAddedInReserve = 0xF,
				[OriginalName("ACTION_POINTS_CHANGED")]
				ActionPointsChanged = 0x10,
				[OriginalName("COMPANION_RESERVE_STATE_CHANGED")]
				CompanionReserveStateChanged = 17,
				[OriginalName("ENTITY_PROTECTION_ADDED")]
				EntityProtectionAdded = 18,
				[OriginalName("ENTITY_PROTECTION_REMOVED")]
				EntityProtectionRemoved = 19,
				[OriginalName("MAGICAL_DAMAGE_MODIFIER_CHANGED")]
				MagicalDamageModifierChanged = 20,
				[OriginalName("MAGICAL_HEAL_MODIFIER_CHANGED")]
				MagicalHealModifierChanged = 21,
				[OriginalName("MOVEMENT_POINTS_CHANGED")]
				MovementPointsChanged = 22,
				[OriginalName("DICE_THROWN")]
				DiceThrown = 24,
				[OriginalName("SUMMONING_ADDED")]
				SummoningAdded = 25,
				[OriginalName("ENTITY_ACTION_RESET")]
				EntityActionReset = 26,
				[OriginalName("RESERVE_POINTS_CHANGED")]
				ReservePointsChanged = 27,
				[OriginalName("RESERVE_USED")]
				ReserveUsed = 28,
				[OriginalName("PROPERTY_CHANGED")]
				PropertyChanged = 29,
				[OriginalName("FLOOR_MECHANISM_ADDED")]
				FloorMechanismAdded = 30,
				[OriginalName("FLOOR_MECHANISM_ACTIVATION")]
				FloorMechanismActivation = 0x1F,
				[OriginalName("OBJECT_MECHANISM_ADDED")]
				ObjectMechanismAdded = 0x20,
				[OriginalName("SPELL_COST_MODIFIER_ADDED")]
				SpellCostModifierAdded = 33,
				[OriginalName("SPELL_COST_MODIFIER_REMOVED")]
				SpellCostModifierRemoved = 34,
				[OriginalName("TEAM_ADDED")]
				TeamAdded = 35,
				[OriginalName("FIGHT_ENDED")]
				FightEnded = 36,
				[OriginalName("TRANSFORMATION")]
				Transformation = 37,
				[OriginalName("ELEMENTARY_CHANGED")]
				ElementaryChanged = 38,
				[OriginalName("DAMAGE_REDUCED")]
				DamageReduced = 39,
				[OriginalName("ATTACK")]
				Attack = 40,
				[OriginalName("EXPLOSION")]
				Explosion = 41,
				[OriginalName("ENTITY_ANIMATION")]
				EntityAnimation = 42,
				[OriginalName("ENTITY_SKIN_CHANGED")]
				EntitySkinChanged = 43,
				[OriginalName("FLOATING_COUNTER_VALUE_CHANGED")]
				FloatingCounterValueChanged = 44,
				[OriginalName("ASSEMBLAGE_CHANGED")]
				AssemblageChanged = 45,
				[OriginalName("PHYSICAL_DAMAGE_MODIFIER_CHANGED")]
				PhysicalDamageModifierChanged = 46,
				[OriginalName("PHYSICAL_HEAL_MODIFIER_CHANGED")]
				PhysicalHealModifierChanged = 47,
				[OriginalName("BOSS_SUMMONINGS_WARNING")]
				BossSummoningsWarning = 48,
				[OriginalName("BOSS_RESERVE_MODIFICATION")]
				BossReserveModification = 49,
				[OriginalName("BOSS_EVOLUTION_STEP_MODIFICATION")]
				BossEvolutionStepModification = 50,
				[OriginalName("BOSS_TURN_START")]
				BossTurnStart = 51,
				[OriginalName("BOSS_LIFE_MODIFICATION")]
				BossLifeModification = 52,
				[OriginalName("BOSS_CAST_SPELL")]
				BossCastSpell = 53,
				[OriginalName("GAME_ENDED")]
				GameEnded = 54,
				[OriginalName("COMPANION_GIVEN")]
				CompanionGiven = 55,
				[OriginalName("COMPANION_RECEIVED")]
				CompanionReceived = 56,
				[OriginalName("TEAM_TURN_STARTED")]
				TeamTurnStarted = 57,
				[OriginalName("BOSS_TURN_END")]
				BossTurnEnd = 58,
				[OriginalName("TURN_SYNCHRONIZATION")]
				TurnSynchronization = 59,
				[OriginalName("EVENT_FOR_PARENTING")]
				EventForParenting = 60,
				[OriginalName("TEAMS_SCORE_MODIFICATION")]
				TeamsScoreModification = 61,
				[OriginalName("MAX_LIFE_CHANGED")]
				MaxLifeChanged = 62,
				[OriginalName("FIGHT_INITIALIZED")]
				FightInitialized = 0x3F
			}
		}

		private static readonly MessageParser<FightEventData> _parser = new MessageParser<FightEventData>((Func<FightEventData>)(() => new FightEventData()));

		private UnknownFieldSet _unknownFields;

		public const int EventTypeFieldNumber = 1;

		private Types.EventType eventType_;

		public const int EventIdFieldNumber = 2;

		private int eventId_;

		public const int ParentEventIdFieldNumber = 3;

		private static readonly FieldCodec<int?> _single_parentEventId_codec = FieldCodec.ForStructWrapper<int>(26u);

		private int? parentEventId_;

		public const int Int1FieldNumber = 4;

		private int int1_;

		public const int Int2FieldNumber = 5;

		private int int2_;

		public const int Int3FieldNumber = 6;

		private int int3_;

		public const int Int4FieldNumber = 7;

		private int int4_;

		public const int Int5FieldNumber = 8;

		private int int5_;

		public const int Int6FieldNumber = 9;

		private int int6_;

		public const int Int7FieldNumber = 10;

		private int int7_;

		public const int String1FieldNumber = 11;

		private string string1_ = "";

		public const int Bool1FieldNumber = 12;

		private bool bool1_;

		public const int CellCoord1FieldNumber = 13;

		private CellCoord cellCoord1_;

		public const int CellCoord2FieldNumber = 14;

		private CellCoord cellCoord2_;

		public const int CompanionReserveState1FieldNumber = 15;

		private CompanionReserveState companionReserveState1_;

		public const int CompanionReserveState2FieldNumber = 16;

		private CompanionReserveState companionReserveState2_;

		public const int DamageReductionType1FieldNumber = 17;

		private DamageReductionType damageReductionType1_;

		public const int FightResult1FieldNumber = 18;

		private FightResult fightResult1_;

		public const int GameStatistics1FieldNumber = 19;

		private GameStatistics gameStatistics1_;

		public const int TeamsScoreModificationReason1FieldNumber = 20;

		private TeamsScoreModificationReason teamsScoreModificationReason1_;

		public const int OptInt1FieldNumber = 21;

		private static readonly FieldCodec<int?> _single_optInt1_codec = FieldCodec.ForStructWrapper<int>(170u);

		private int? optInt1_;

		public const int OptInt2FieldNumber = 22;

		private static readonly FieldCodec<int?> _single_optInt2_codec = FieldCodec.ForStructWrapper<int>(178u);

		private int? optInt2_;

		public const int OptInt3FieldNumber = 23;

		private static readonly FieldCodec<int?> _single_optInt3_codec = FieldCodec.ForStructWrapper<int>(186u);

		private int? optInt3_;

		public const int OptInt4FieldNumber = 24;

		private static readonly FieldCodec<int?> _single_optInt4_codec = FieldCodec.ForStructWrapper<int>(194u);

		private int? optInt4_;

		public const int CellCoordList1FieldNumber = 25;

		private static readonly FieldCodec<CellCoord> _repeated_cellCoordList1_codec = FieldCodec.ForMessage<CellCoord>(202u, CellCoord.Parser);

		private readonly RepeatedField<CellCoord> cellCoordList1_ = new RepeatedField<CellCoord>();

		public const int SpellMovementList1FieldNumber = 26;

		private static readonly FieldCodec<SpellMovement> _repeated_spellMovementList1_codec = FieldCodec.ForMessage<SpellMovement>(210u, SpellMovement.Parser);

		private readonly RepeatedField<SpellMovement> spellMovementList1_ = new RepeatedField<SpellMovement>();

		public const int CastTargetList1FieldNumber = 27;

		private static readonly FieldCodec<CastTarget> _repeated_castTargetList1_codec = FieldCodec.ForMessage<CastTarget>(218u, CastTarget.Parser);

		private readonly RepeatedField<CastTarget> castTargetList1_ = new RepeatedField<CastTarget>();

		public const int IntList1FieldNumber = 28;

		private static readonly FieldCodec<int> _repeated_intList1_codec = FieldCodec.ForInt32(226u);

		private readonly RepeatedField<int> intList1_ = new RepeatedField<int>();

		public const int IntList2FieldNumber = 29;

		private static readonly FieldCodec<int> _repeated_intList2_codec = FieldCodec.ForInt32(234u);

		private readonly RepeatedField<int> intList2_ = new RepeatedField<int>();

		[DebuggerNonUserCode]
		public static MessageParser<FightEventData> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => EventsReflection.Descriptor.get_MessageTypes()[0];

		[DebuggerNonUserCode]
		MessageDescriptor Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public Types.EventType EventType
		{
			get
			{
				return eventType_;
			}
			set
			{
				eventType_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int EventId
		{
			get
			{
				return eventId_;
			}
			set
			{
				eventId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int? ParentEventId
		{
			get
			{
				return parentEventId_;
			}
			set
			{
				parentEventId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Int1
		{
			get
			{
				return int1_;
			}
			set
			{
				int1_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Int2
		{
			get
			{
				return int2_;
			}
			set
			{
				int2_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Int3
		{
			get
			{
				return int3_;
			}
			set
			{
				int3_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Int4
		{
			get
			{
				return int4_;
			}
			set
			{
				int4_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Int5
		{
			get
			{
				return int5_;
			}
			set
			{
				int5_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Int6
		{
			get
			{
				return int6_;
			}
			set
			{
				int6_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Int7
		{
			get
			{
				return int7_;
			}
			set
			{
				int7_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string String1
		{
			get
			{
				return string1_;
			}
			set
			{
				string1_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public bool Bool1
		{
			get
			{
				return bool1_;
			}
			set
			{
				bool1_ = value;
			}
		}

		[DebuggerNonUserCode]
		public CellCoord CellCoord1
		{
			get
			{
				return cellCoord1_;
			}
			set
			{
				cellCoord1_ = value;
			}
		}

		[DebuggerNonUserCode]
		public CellCoord CellCoord2
		{
			get
			{
				return cellCoord2_;
			}
			set
			{
				cellCoord2_ = value;
			}
		}

		[DebuggerNonUserCode]
		public CompanionReserveState CompanionReserveState1
		{
			get
			{
				return companionReserveState1_;
			}
			set
			{
				companionReserveState1_ = value;
			}
		}

		[DebuggerNonUserCode]
		public CompanionReserveState CompanionReserveState2
		{
			get
			{
				return companionReserveState2_;
			}
			set
			{
				companionReserveState2_ = value;
			}
		}

		[DebuggerNonUserCode]
		public DamageReductionType DamageReductionType1
		{
			get
			{
				return damageReductionType1_;
			}
			set
			{
				damageReductionType1_ = value;
			}
		}

		[DebuggerNonUserCode]
		public FightResult FightResult1
		{
			get
			{
				return fightResult1_;
			}
			set
			{
				fightResult1_ = value;
			}
		}

		[DebuggerNonUserCode]
		public GameStatistics GameStatistics1
		{
			get
			{
				return gameStatistics1_;
			}
			set
			{
				gameStatistics1_ = value;
			}
		}

		[DebuggerNonUserCode]
		public TeamsScoreModificationReason TeamsScoreModificationReason1
		{
			get
			{
				return teamsScoreModificationReason1_;
			}
			set
			{
				teamsScoreModificationReason1_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int? OptInt1
		{
			get
			{
				return optInt1_;
			}
			set
			{
				optInt1_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int? OptInt2
		{
			get
			{
				return optInt2_;
			}
			set
			{
				optInt2_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int? OptInt3
		{
			get
			{
				return optInt3_;
			}
			set
			{
				optInt3_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int? OptInt4
		{
			get
			{
				return optInt4_;
			}
			set
			{
				optInt4_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<CellCoord> CellCoordList1 => cellCoordList1_;

		[DebuggerNonUserCode]
		public RepeatedField<SpellMovement> SpellMovementList1 => spellMovementList1_;

		[DebuggerNonUserCode]
		public RepeatedField<CastTarget> CastTargetList1 => castTargetList1_;

		[DebuggerNonUserCode]
		public RepeatedField<int> IntList1 => intList1_;

		[DebuggerNonUserCode]
		public RepeatedField<int> IntList2 => intList2_;

		[DebuggerNonUserCode]
		public FightEventData()
		{
		}

		[DebuggerNonUserCode]
		public FightEventData(FightEventData other)
			: this()
		{
			eventType_ = other.eventType_;
			eventId_ = other.eventId_;
			ParentEventId = other.ParentEventId;
			int1_ = other.int1_;
			int2_ = other.int2_;
			int3_ = other.int3_;
			int4_ = other.int4_;
			int5_ = other.int5_;
			int6_ = other.int6_;
			int7_ = other.int7_;
			string1_ = other.string1_;
			bool1_ = other.bool1_;
			cellCoord1_ = ((other.cellCoord1_ != null) ? other.cellCoord1_.Clone() : null);
			cellCoord2_ = ((other.cellCoord2_ != null) ? other.cellCoord2_.Clone() : null);
			companionReserveState1_ = other.companionReserveState1_;
			companionReserveState2_ = other.companionReserveState2_;
			damageReductionType1_ = other.damageReductionType1_;
			fightResult1_ = other.fightResult1_;
			gameStatistics1_ = ((other.gameStatistics1_ != null) ? other.gameStatistics1_.Clone() : null);
			teamsScoreModificationReason1_ = other.teamsScoreModificationReason1_;
			OptInt1 = other.OptInt1;
			OptInt2 = other.OptInt2;
			OptInt3 = other.OptInt3;
			OptInt4 = other.OptInt4;
			cellCoordList1_ = other.cellCoordList1_.Clone();
			spellMovementList1_ = other.spellMovementList1_.Clone();
			castTargetList1_ = other.castTargetList1_.Clone();
			intList1_ = other.intList1_.Clone();
			intList2_ = other.intList2_.Clone();
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public FightEventData Clone()
		{
			return new FightEventData(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as FightEventData);
		}

		[DebuggerNonUserCode]
		public bool Equals(FightEventData other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (EventType != other.EventType)
			{
				return false;
			}
			if (EventId != other.EventId)
			{
				return false;
			}
			if (ParentEventId != other.ParentEventId)
			{
				return false;
			}
			if (Int1 != other.Int1)
			{
				return false;
			}
			if (Int2 != other.Int2)
			{
				return false;
			}
			if (Int3 != other.Int3)
			{
				return false;
			}
			if (Int4 != other.Int4)
			{
				return false;
			}
			if (Int5 != other.Int5)
			{
				return false;
			}
			if (Int6 != other.Int6)
			{
				return false;
			}
			if (Int7 != other.Int7)
			{
				return false;
			}
			if (String1 != other.String1)
			{
				return false;
			}
			if (Bool1 != other.Bool1)
			{
				return false;
			}
			if (!object.Equals(CellCoord1, other.CellCoord1))
			{
				return false;
			}
			if (!object.Equals(CellCoord2, other.CellCoord2))
			{
				return false;
			}
			if (CompanionReserveState1 != other.CompanionReserveState1)
			{
				return false;
			}
			if (CompanionReserveState2 != other.CompanionReserveState2)
			{
				return false;
			}
			if (DamageReductionType1 != other.DamageReductionType1)
			{
				return false;
			}
			if (FightResult1 != other.FightResult1)
			{
				return false;
			}
			if (!object.Equals(GameStatistics1, other.GameStatistics1))
			{
				return false;
			}
			if (TeamsScoreModificationReason1 != other.TeamsScoreModificationReason1)
			{
				return false;
			}
			if (OptInt1 != other.OptInt1)
			{
				return false;
			}
			if (OptInt2 != other.OptInt2)
			{
				return false;
			}
			if (OptInt3 != other.OptInt3)
			{
				return false;
			}
			if (OptInt4 != other.OptInt4)
			{
				return false;
			}
			if (!cellCoordList1_.Equals(other.cellCoordList1_))
			{
				return false;
			}
			if (!spellMovementList1_.Equals(other.spellMovementList1_))
			{
				return false;
			}
			if (!castTargetList1_.Equals(other.castTargetList1_))
			{
				return false;
			}
			if (!intList1_.Equals(other.intList1_))
			{
				return false;
			}
			if (!intList2_.Equals(other.intList2_))
			{
				return false;
			}
			return object.Equals(_unknownFields, other._unknownFields);
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (EventType != 0)
			{
				num ^= EventType.GetHashCode();
			}
			if (EventId != 0)
			{
				num ^= EventId.GetHashCode();
			}
			if (parentEventId_.HasValue)
			{
				num ^= ParentEventId.GetHashCode();
			}
			if (Int1 != 0)
			{
				num ^= Int1.GetHashCode();
			}
			if (Int2 != 0)
			{
				num ^= Int2.GetHashCode();
			}
			if (Int3 != 0)
			{
				num ^= Int3.GetHashCode();
			}
			if (Int4 != 0)
			{
				num ^= Int4.GetHashCode();
			}
			if (Int5 != 0)
			{
				num ^= Int5.GetHashCode();
			}
			if (Int6 != 0)
			{
				num ^= Int6.GetHashCode();
			}
			if (Int7 != 0)
			{
				num ^= Int7.GetHashCode();
			}
			if (String1.Length != 0)
			{
				num ^= String1.GetHashCode();
			}
			if (Bool1)
			{
				num ^= Bool1.GetHashCode();
			}
			if (cellCoord1_ != null)
			{
				num ^= CellCoord1.GetHashCode();
			}
			if (cellCoord2_ != null)
			{
				num ^= CellCoord2.GetHashCode();
			}
			if (CompanionReserveState1 != 0)
			{
				num ^= CompanionReserveState1.GetHashCode();
			}
			if (CompanionReserveState2 != 0)
			{
				num ^= CompanionReserveState2.GetHashCode();
			}
			if (DamageReductionType1 != 0)
			{
				num ^= DamageReductionType1.GetHashCode();
			}
			if (FightResult1 != 0)
			{
				num ^= FightResult1.GetHashCode();
			}
			if (gameStatistics1_ != null)
			{
				num ^= GameStatistics1.GetHashCode();
			}
			if (TeamsScoreModificationReason1 != 0)
			{
				num ^= TeamsScoreModificationReason1.GetHashCode();
			}
			if (optInt1_.HasValue)
			{
				num ^= OptInt1.GetHashCode();
			}
			if (optInt2_.HasValue)
			{
				num ^= OptInt2.GetHashCode();
			}
			if (optInt3_.HasValue)
			{
				num ^= OptInt3.GetHashCode();
			}
			if (optInt4_.HasValue)
			{
				num ^= OptInt4.GetHashCode();
			}
			num ^= ((object)cellCoordList1_).GetHashCode();
			num ^= ((object)spellMovementList1_).GetHashCode();
			num ^= ((object)castTargetList1_).GetHashCode();
			num ^= ((object)intList1_).GetHashCode();
			num ^= ((object)intList2_).GetHashCode();
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
			if (EventType != 0)
			{
				output.WriteRawTag((byte)8);
				output.WriteEnum((int)EventType);
			}
			if (EventId != 0)
			{
				output.WriteRawTag((byte)16);
				output.WriteSInt32(EventId);
			}
			if (parentEventId_.HasValue)
			{
				_single_parentEventId_codec.WriteTagAndValue(output, ParentEventId);
			}
			if (Int1 != 0)
			{
				output.WriteRawTag((byte)32);
				output.WriteInt32(Int1);
			}
			if (Int2 != 0)
			{
				output.WriteRawTag((byte)40);
				output.WriteInt32(Int2);
			}
			if (Int3 != 0)
			{
				output.WriteRawTag((byte)48);
				output.WriteInt32(Int3);
			}
			if (Int4 != 0)
			{
				output.WriteRawTag((byte)56);
				output.WriteInt32(Int4);
			}
			if (Int5 != 0)
			{
				output.WriteRawTag((byte)64);
				output.WriteInt32(Int5);
			}
			if (Int6 != 0)
			{
				output.WriteRawTag((byte)72);
				output.WriteInt32(Int6);
			}
			if (Int7 != 0)
			{
				output.WriteRawTag((byte)80);
				output.WriteInt32(Int7);
			}
			if (String1.Length != 0)
			{
				output.WriteRawTag((byte)90);
				output.WriteString(String1);
			}
			if (Bool1)
			{
				output.WriteRawTag((byte)96);
				output.WriteBool(Bool1);
			}
			if (cellCoord1_ != null)
			{
				output.WriteRawTag((byte)106);
				output.WriteMessage(CellCoord1);
			}
			if (cellCoord2_ != null)
			{
				output.WriteRawTag((byte)114);
				output.WriteMessage(CellCoord2);
			}
			if (CompanionReserveState1 != 0)
			{
				output.WriteRawTag((byte)120);
				output.WriteEnum((int)CompanionReserveState1);
			}
			if (CompanionReserveState2 != 0)
			{
				output.WriteRawTag((byte)128, (byte)1);
				output.WriteEnum((int)CompanionReserveState2);
			}
			if (DamageReductionType1 != 0)
			{
				output.WriteRawTag((byte)136, (byte)1);
				output.WriteEnum((int)DamageReductionType1);
			}
			if (FightResult1 != 0)
			{
				output.WriteRawTag((byte)144, (byte)1);
				output.WriteEnum((int)FightResult1);
			}
			if (gameStatistics1_ != null)
			{
				output.WriteRawTag((byte)154, (byte)1);
				output.WriteMessage(GameStatistics1);
			}
			if (TeamsScoreModificationReason1 != 0)
			{
				output.WriteRawTag((byte)160, (byte)1);
				output.WriteEnum((int)TeamsScoreModificationReason1);
			}
			if (optInt1_.HasValue)
			{
				_single_optInt1_codec.WriteTagAndValue(output, OptInt1);
			}
			if (optInt2_.HasValue)
			{
				_single_optInt2_codec.WriteTagAndValue(output, OptInt2);
			}
			if (optInt3_.HasValue)
			{
				_single_optInt3_codec.WriteTagAndValue(output, OptInt3);
			}
			if (optInt4_.HasValue)
			{
				_single_optInt4_codec.WriteTagAndValue(output, OptInt4);
			}
			cellCoordList1_.WriteTo(output, _repeated_cellCoordList1_codec);
			spellMovementList1_.WriteTo(output, _repeated_spellMovementList1_codec);
			castTargetList1_.WriteTo(output, _repeated_castTargetList1_codec);
			intList1_.WriteTo(output, _repeated_intList1_codec);
			intList2_.WriteTo(output, _repeated_intList2_codec);
			if (_unknownFields != null)
			{
				_unknownFields.WriteTo(output);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (EventType != 0)
			{
				num += 1 + CodedOutputStream.ComputeEnumSize((int)EventType);
			}
			if (EventId != 0)
			{
				num += 1 + CodedOutputStream.ComputeSInt32Size(EventId);
			}
			if (parentEventId_.HasValue)
			{
				num += _single_parentEventId_codec.CalculateSizeWithTag(ParentEventId);
			}
			if (Int1 != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Int1);
			}
			if (Int2 != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Int2);
			}
			if (Int3 != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Int3);
			}
			if (Int4 != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Int4);
			}
			if (Int5 != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Int5);
			}
			if (Int6 != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Int6);
			}
			if (Int7 != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Int7);
			}
			if (String1.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(String1);
			}
			if (Bool1)
			{
				num += 2;
			}
			if (cellCoord1_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(CellCoord1);
			}
			if (cellCoord2_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(CellCoord2);
			}
			if (CompanionReserveState1 != 0)
			{
				num += 1 + CodedOutputStream.ComputeEnumSize((int)CompanionReserveState1);
			}
			if (CompanionReserveState2 != 0)
			{
				num += 2 + CodedOutputStream.ComputeEnumSize((int)CompanionReserveState2);
			}
			if (DamageReductionType1 != 0)
			{
				num += 2 + CodedOutputStream.ComputeEnumSize((int)DamageReductionType1);
			}
			if (FightResult1 != 0)
			{
				num += 2 + CodedOutputStream.ComputeEnumSize((int)FightResult1);
			}
			if (gameStatistics1_ != null)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(GameStatistics1);
			}
			if (TeamsScoreModificationReason1 != 0)
			{
				num += 2 + CodedOutputStream.ComputeEnumSize((int)TeamsScoreModificationReason1);
			}
			if (optInt1_.HasValue)
			{
				num += _single_optInt1_codec.CalculateSizeWithTag(OptInt1);
			}
			if (optInt2_.HasValue)
			{
				num += _single_optInt2_codec.CalculateSizeWithTag(OptInt2);
			}
			if (optInt3_.HasValue)
			{
				num += _single_optInt3_codec.CalculateSizeWithTag(OptInt3);
			}
			if (optInt4_.HasValue)
			{
				num += _single_optInt4_codec.CalculateSizeWithTag(OptInt4);
			}
			num += cellCoordList1_.CalculateSize(_repeated_cellCoordList1_codec);
			num += spellMovementList1_.CalculateSize(_repeated_spellMovementList1_codec);
			num += castTargetList1_.CalculateSize(_repeated_castTargetList1_codec);
			num += intList1_.CalculateSize(_repeated_intList1_codec);
			num += intList2_.CalculateSize(_repeated_intList2_codec);
			if (_unknownFields != null)
			{
				num += _unknownFields.CalculateSize();
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(FightEventData other)
		{
			if (other == null)
			{
				return;
			}
			if (other.EventType != 0)
			{
				EventType = other.EventType;
			}
			if (other.EventId != 0)
			{
				EventId = other.EventId;
			}
			if (other.parentEventId_.HasValue && (!parentEventId_.HasValue || other.ParentEventId != 0))
			{
				ParentEventId = other.ParentEventId;
			}
			if (other.Int1 != 0)
			{
				Int1 = other.Int1;
			}
			if (other.Int2 != 0)
			{
				Int2 = other.Int2;
			}
			if (other.Int3 != 0)
			{
				Int3 = other.Int3;
			}
			if (other.Int4 != 0)
			{
				Int4 = other.Int4;
			}
			if (other.Int5 != 0)
			{
				Int5 = other.Int5;
			}
			if (other.Int6 != 0)
			{
				Int6 = other.Int6;
			}
			if (other.Int7 != 0)
			{
				Int7 = other.Int7;
			}
			if (other.String1.Length != 0)
			{
				String1 = other.String1;
			}
			if (other.Bool1)
			{
				Bool1 = other.Bool1;
			}
			if (other.cellCoord1_ != null)
			{
				if (cellCoord1_ == null)
				{
					cellCoord1_ = new CellCoord();
				}
				CellCoord1.MergeFrom(other.CellCoord1);
			}
			if (other.cellCoord2_ != null)
			{
				if (cellCoord2_ == null)
				{
					cellCoord2_ = new CellCoord();
				}
				CellCoord2.MergeFrom(other.CellCoord2);
			}
			if (other.CompanionReserveState1 != 0)
			{
				CompanionReserveState1 = other.CompanionReserveState1;
			}
			if (other.CompanionReserveState2 != 0)
			{
				CompanionReserveState2 = other.CompanionReserveState2;
			}
			if (other.DamageReductionType1 != 0)
			{
				DamageReductionType1 = other.DamageReductionType1;
			}
			if (other.FightResult1 != 0)
			{
				FightResult1 = other.FightResult1;
			}
			if (other.gameStatistics1_ != null)
			{
				if (gameStatistics1_ == null)
				{
					gameStatistics1_ = new GameStatistics();
				}
				GameStatistics1.MergeFrom(other.GameStatistics1);
			}
			if (other.TeamsScoreModificationReason1 != 0)
			{
				TeamsScoreModificationReason1 = other.TeamsScoreModificationReason1;
			}
			if (other.optInt1_.HasValue && (!optInt1_.HasValue || other.OptInt1 != 0))
			{
				OptInt1 = other.OptInt1;
			}
			if (other.optInt2_.HasValue && (!optInt2_.HasValue || other.OptInt2 != 0))
			{
				OptInt2 = other.OptInt2;
			}
			if (other.optInt3_.HasValue && (!optInt3_.HasValue || other.OptInt3 != 0))
			{
				OptInt3 = other.OptInt3;
			}
			if (other.optInt4_.HasValue && (!optInt4_.HasValue || other.OptInt4 != 0))
			{
				OptInt4 = other.OptInt4;
			}
			cellCoordList1_.Add((IEnumerable<CellCoord>)other.cellCoordList1_);
			spellMovementList1_.Add((IEnumerable<SpellMovement>)other.spellMovementList1_);
			castTargetList1_.Add((IEnumerable<CastTarget>)other.castTargetList1_);
			intList1_.Add((IEnumerable<int>)other.intList1_);
			intList2_.Add((IEnumerable<int>)other.intList2_);
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
					eventType_ = (Types.EventType)input.ReadEnum();
					break;
				case 16u:
					EventId = input.ReadSInt32();
					break;
				case 26u:
				{
					int? num6 = _single_parentEventId_codec.Read(input);
					if (!parentEventId_.HasValue || num6 != 0)
					{
						ParentEventId = num6;
					}
					break;
				}
				case 32u:
					Int1 = input.ReadInt32();
					break;
				case 40u:
					Int2 = input.ReadInt32();
					break;
				case 48u:
					Int3 = input.ReadInt32();
					break;
				case 56u:
					Int4 = input.ReadInt32();
					break;
				case 64u:
					Int5 = input.ReadInt32();
					break;
				case 72u:
					Int6 = input.ReadInt32();
					break;
				case 80u:
					Int7 = input.ReadInt32();
					break;
				case 90u:
					String1 = input.ReadString();
					break;
				case 96u:
					Bool1 = input.ReadBool();
					break;
				case 106u:
					if (cellCoord1_ == null)
					{
						cellCoord1_ = new CellCoord();
					}
					input.ReadMessage(cellCoord1_);
					break;
				case 114u:
					if (cellCoord2_ == null)
					{
						cellCoord2_ = new CellCoord();
					}
					input.ReadMessage(cellCoord2_);
					break;
				case 120u:
					companionReserveState1_ = (CompanionReserveState)input.ReadEnum();
					break;
				case 128u:
					companionReserveState2_ = (CompanionReserveState)input.ReadEnum();
					break;
				case 136u:
					damageReductionType1_ = (DamageReductionType)input.ReadEnum();
					break;
				case 144u:
					fightResult1_ = (FightResult)input.ReadEnum();
					break;
				case 154u:
					if (gameStatistics1_ == null)
					{
						gameStatistics1_ = new GameStatistics();
					}
					input.ReadMessage(gameStatistics1_);
					break;
				case 160u:
					teamsScoreModificationReason1_ = (TeamsScoreModificationReason)input.ReadEnum();
					break;
				case 170u:
				{
					int? num5 = _single_optInt1_codec.Read(input);
					if (!optInt1_.HasValue || num5 != 0)
					{
						OptInt1 = num5;
					}
					break;
				}
				case 178u:
				{
					int? num4 = _single_optInt2_codec.Read(input);
					if (!optInt2_.HasValue || num4 != 0)
					{
						OptInt2 = num4;
					}
					break;
				}
				case 186u:
				{
					int? num3 = _single_optInt3_codec.Read(input);
					if (!optInt3_.HasValue || num3 != 0)
					{
						OptInt3 = num3;
					}
					break;
				}
				case 194u:
				{
					int? num2 = _single_optInt4_codec.Read(input);
					if (!optInt4_.HasValue || num2 != 0)
					{
						OptInt4 = num2;
					}
					break;
				}
				case 202u:
					cellCoordList1_.AddEntriesFrom(input, _repeated_cellCoordList1_codec);
					break;
				case 210u:
					spellMovementList1_.AddEntriesFrom(input, _repeated_spellMovementList1_codec);
					break;
				case 218u:
					castTargetList1_.AddEntriesFrom(input, _repeated_castTargetList1_codec);
					break;
				case 224u:
				case 226u:
					intList1_.AddEntriesFrom(input, _repeated_intList1_codec);
					break;
				case 232u:
				case 234u:
					intList2_.AddEntriesFrom(input, _repeated_intList2_codec);
					break;
				}
			}
		}

		public string ToDiagnosticString()
		{
			return "FightEventData";
		}
	}
}
