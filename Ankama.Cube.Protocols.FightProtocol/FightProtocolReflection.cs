using Ankama.Cube.Fight.Events;
using Ankama.Cube.Protocols.CommonProtocol;
using Ankama.Cube.Protocols.FightCommonProtocol;
using Google.Protobuf.Reflection;
using Google.Protobuf.WellKnownTypes;
using System;

namespace Ankama.Cube.Protocols.FightProtocol
{
	public static class FightProtocolReflection
	{
		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor => descriptor;

		static FightProtocolReflection()
		{
			//IL_024e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0254: Expected O, but got Unknown
			//IL_0269: Unknown result type (might be due to invalid IL or missing references)
			//IL_026f: Expected O, but got Unknown
			//IL_0284: Unknown result type (might be due to invalid IL or missing references)
			//IL_028a: Expected O, but got Unknown
			//IL_029f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a5: Expected O, but got Unknown
			//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c0: Expected O, but got Unknown
			//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e8: Expected O, but got Unknown
			//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0303: Expected O, but got Unknown
			//IL_033d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0343: Expected O, but got Unknown
			//IL_036d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0373: Expected O, but got Unknown
			//IL_046a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0470: Expected O, but got Unknown
			//IL_049a: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a0: Expected O, but got Unknown
			//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a6: Expected O, but got Unknown
			//IL_04d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d7: Expected O, but got Unknown
			//IL_04ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f3: Expected O, but got Unknown
			//IL_0526: Unknown result type (might be due to invalid IL or missing references)
			//IL_052c: Expected O, but got Unknown
			//IL_0557: Unknown result type (might be due to invalid IL or missing references)
			//IL_055d: Expected O, but got Unknown
			//IL_0588: Unknown result type (might be due to invalid IL or missing references)
			//IL_058e: Expected O, but got Unknown
			//IL_05c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c7: Expected O, but got Unknown
			//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e3: Expected O, but got Unknown
			//IL_0606: Unknown result type (might be due to invalid IL or missing references)
			//IL_060c: Expected O, but got Unknown
			//IL_0622: Unknown result type (might be due to invalid IL or missing references)
			//IL_0628: Expected O, but got Unknown
			//IL_0653: Unknown result type (might be due to invalid IL or missing references)
			//IL_0659: Expected O, but got Unknown
			//IL_0659: Unknown result type (might be due to invalid IL or missing references)
			//IL_0663: Expected O, but got Unknown
			descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String("ChNmaWdodFByb3RvY29sLnByb3RvGh5nb29nbGUvcHJvdG9idWYvd3JhcHBl" + "cnMucHJvdG8aDGV2ZW50cy5wcm90bxoUY29tbW9uUHJvdG9jb2wucHJvdG8a" + "GWZpZ2h0Q29tbW9uUHJvdG9jb2wucHJvdG8iWQoRRmlnaHRTdGFydGVkRXZl" + "bnQSHQoJZmlnaHRJbmZvGAEgASgLMgouRmlnaHRJbmZvEhIKCmZpZ2h0RGVm" + "SWQYAiABKAUSEQoJZmlnaHRUeXBlGAMgASgFIhYKFEZpZ2h0Tm90U3RhcnRl" + "ZEV2ZW50IgsKCVJlc2lnbkNtZCIKCghMZWF2ZUNtZCIRCg9HZXRGaWdodElu" + "Zm9DbWQiLwoORmlnaHRJbmZvRXZlbnQSHQoJZmlnaHRJbmZvGAEgASgLMgou" + "RmlnaHRJbmZvIhUKE0dldEZpZ2h0U25hcHNob3RDbWQikQEKEkZpZ2h0U25h" + "cHNob3RFdmVudBInCg9maWdodHNTbmFwc2hvdHMYASADKAsyDi5GaWdodFNu" + "YXBzaG90EhIKCm93bkZpZ2h0SWQYAiABKAUSEwoLb3duUGxheWVySWQYAyAB" + "KAUSKQoMb3duU3BlbGxzSWRzGAQgAygLMhMuRmlnaHRTbmFwc2hvdFNwZWxs" + "IkEKEkZpZ2h0U25hcHNob3RTcGVsbBISCgpzcGVsbERlZklkGAEgASgFEhcK" + "D3NwZWxsSW5zdGFuY2VJZBgCIAEoBSKxCQoNRmlnaHRTbmFwc2hvdBIPCgdm" + "aWdodElkGAEgASgFEi8KCGVudGl0aWVzGAIgAygLMh0uRmlnaHRTbmFwc2hv" + "dC5FbnRpdHlTbmFwc2hvdBIRCgl0dXJuSW5kZXgYAyABKAUSHAoUdHVyblJl" + "bWFpbmluZ1RpbWVTZWMYBCABKAUSQAoRcGxheWVyc0NvbXBhbmlvbnMYBSAD" + "KAsyJS5GaWdodFNuYXBzaG90LlBsYXllcnNDb21wYW5pb25zRW50cnkSQAoR" + "cGxheWVyc0NhcmRzQ291bnQYBiADKAsyJS5GaWdodFNuYXBzaG90LlBsYXll" + "cnNDYXJkc0NvdW50RW50cnkaUwoWUGxheWVyc0NvbXBhbmlvbnNFbnRyeRIL" + "CgNrZXkYASABKAUSKAoFdmFsdWUYAiABKAsyGS5GaWdodFNuYXBzaG90LkNv" + "bXBhbmlvbnM6AjgBGjgKFlBsYXllcnNDYXJkc0NvdW50RW50cnkSCwoDa2V5" + "GAEgASgFEg0KBXZhbHVlGAIgASgFOgI4ARriBQoORW50aXR5U25hcHNob3QS" + "EAoIZW50aXR5SWQYASABKAUSEgoKZW50aXR5VHlwZRgCIAEoBRIqCgRuYW1l" + "GAMgASgLMhwuZ29vZ2xlLnByb3RvYnVmLlN0cmluZ1ZhbHVlEioKBWRlZklk" + "GAQgASgLMhsuZ29vZ2xlLnByb3RvYnVmLkludDMyVmFsdWUSLQoId2VhcG9u" + "SWQYBSABKAsyGy5nb29nbGUucHJvdG9idWYuSW50MzJWYWx1ZRItCghnZW5k" + "ZXJJZBgGIAEoCzIbLmdvb2dsZS5wcm90b2J1Zi5JbnQzMlZhbHVlEjcKEnBs" + "YXllckluZGV4SW5GaWdodBgHIAEoCzIbLmdvb2dsZS5wcm90b2J1Zi5JbnQz" + "MlZhbHVlEiwKB293bmVySWQYCCABKAsyGy5nb29nbGUucHJvdG9idWYuSW50" + "MzJWYWx1ZRIrCgZ0ZWFtSWQYCSABKAsyGy5nb29nbGUucHJvdG9idWYuSW50" + "MzJWYWx1ZRIqCgVsZXZlbBgKIAEoCzIbLmdvb2dsZS5wcm90b2J1Zi5JbnQz" + "MlZhbHVlEhIKCnByb3BlcnRpZXMYCyADKAUSHAoIcG9zaXRpb24YDCABKAsy" + "Ci5DZWxsQ29vcmQSLgoJZGlyZWN0aW9uGA0gASgLMhsuZ29vZ2xlLnByb3Rv" + "YnVmLkludDMyVmFsdWUSOQoGY2FyYWNzGA4gAygLMikuRmlnaHRTbmFwc2hv" + "dC5FbnRpdHlTbmFwc2hvdC5DYXJhY3NFbnRyeRIwCgpjdXN0b21Ta2luGA8g" + "ASgLMhwuZ29vZ2xlLnByb3RvYnVmLlN0cmluZ1ZhbHVlEjYKEmFjdGlvbkRv" + "bmVUaGlzVHVybhgQIAEoCzIaLmdvb2dsZS5wcm90b2J1Zi5Cb29sVmFsdWUa" + "LQoLQ2FyYWNzRW50cnkSCwoDa2V5GAEgASgFEg0KBXZhbHVlGAIgASgFOgI4" + "ARo1CgpDb21wYW5pb25zEhEKCWFsbERlZklkcxgBIAMoBRIUCgxhdmFpbGFi" + "bGVJZHMYAiADKAUiOQoUUGxheWVyTGVmdEZpZ2h0RXZlbnQSDwoHZmlnaHRJ" + "ZBgBIAEoBRIQCghwbGF5ZXJJZBgCIAEoBSIQCg5QbGF5ZXJSZWFkeUNtZCJy" + "Cg1Nb3ZlRW50aXR5Q21kEhAKCGVudGl0eUlkGAEgASgFEhgKBHBhdGgYAiAD" + "KAsyCi5DZWxsQ29vcmQSNQoQZW50aXR5VG9BdHRhY2tJZBgDIAEoCzIbLmdv" + "b2dsZS5wcm90b2J1Zi5JbnQzMlZhbHVlIkEKDFBsYXlTcGVsbENtZBIPCgdz" + "cGVsbElkGAEgASgFEiAKC2Nhc3RUYXJnZXRzGAIgAygLMgsuQ2FzdFRhcmdl" + "dCJIChJJbnZva2VDb21wYW5pb25DbWQSFgoOY29tcGFuaW9uRGVmSWQYASAB" + "KAUSGgoGY29vcmRzGAIgASgLMgouQ2VsbENvb3JkIlkKEEdpdmVDb21wYW5p" + "b25DbWQSFgoOY29tcGFuaW9uRGVmSWQYASABKAUSFQoNdGFyZ2V0RmlnaHRJ" + "ZBgCIAEoBRIWCg50YXJnZXRQbGF5ZXJJZBgDIAEoBSIPCg1Vc2VSZXNlcnZl" + "Q21kIiEKDEVuZE9mVHVybkNtZBIRCgl0dXJuSW5kZXgYASABKAUiFQoTQ29t" + "bWFuZEhhbmRsZWRFdmVudCJLChBGaWdodEV2ZW50c0V2ZW50Eg8KB2ZpZ2h0" + "SWQYASABKAUSJgoGZXZlbnRzGAIgAygLMhYuZXZlbnRzLkZpZ2h0RXZlbnRE" + "YXRhQj0KFWNvbS5hbmthbWEuY3ViZS5wcm90b6oCI0Fua2FtYS5DdWJlLlBy" + "b3RvY29scy5GaWdodFByb3RvY29sYgZwcm90bzM="), (FileDescriptor[])new FileDescriptor[4]
			{
				WrappersReflection.get_Descriptor(),
				EventsReflection.Descriptor,
				CommonProtocolReflection.Descriptor,
				FightCommonProtocolReflection.Descriptor
			}, new GeneratedClrTypeInfo((Type[])null, (GeneratedClrTypeInfo[])new GeneratedClrTypeInfo[20]
			{
				new GeneratedClrTypeInfo(typeof(FightStartedEvent), FightStartedEvent.Parser, new string[3]
				{
					"FightInfo",
					"FightDefId",
					"FightType"
				}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
				new GeneratedClrTypeInfo(typeof(FightNotStartedEvent), FightNotStartedEvent.Parser, (string[])null, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
				new GeneratedClrTypeInfo(typeof(ResignCmd), ResignCmd.Parser, (string[])null, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
				new GeneratedClrTypeInfo(typeof(LeaveCmd), LeaveCmd.Parser, (string[])null, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
				new GeneratedClrTypeInfo(typeof(GetFightInfoCmd), GetFightInfoCmd.Parser, (string[])null, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
				new GeneratedClrTypeInfo(typeof(FightInfoEvent), FightInfoEvent.Parser, new string[1]
				{
					"FightInfo"
				}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
				new GeneratedClrTypeInfo(typeof(GetFightSnapshotCmd), GetFightSnapshotCmd.Parser, (string[])null, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
				new GeneratedClrTypeInfo(typeof(FightSnapshotEvent), FightSnapshotEvent.Parser, new string[4]
				{
					"FightsSnapshots",
					"OwnFightId",
					"OwnPlayerId",
					"OwnSpellsIds"
				}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
				new GeneratedClrTypeInfo(typeof(FightSnapshotSpell), FightSnapshotSpell.Parser, new string[2]
				{
					"SpellDefId",
					"SpellInstanceId"
				}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
				new GeneratedClrTypeInfo(typeof(FightSnapshot), FightSnapshot.Parser, new string[6]
				{
					"FightId",
					"Entities",
					"TurnIndex",
					"TurnRemainingTimeSec",
					"PlayersCompanions",
					"PlayersCardsCount"
				}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])new GeneratedClrTypeInfo[4]
				{
					default(GeneratedClrTypeInfo),
					default(GeneratedClrTypeInfo),
					new GeneratedClrTypeInfo(typeof(FightSnapshot.Types.EntitySnapshot), FightSnapshot.Types.EntitySnapshot.Parser, new string[16]
					{
						"EntityId",
						"EntityType",
						"Name",
						"DefId",
						"WeaponId",
						"GenderId",
						"PlayerIndexInFight",
						"OwnerId",
						"TeamId",
						"Level",
						"Properties",
						"Position",
						"Direction",
						"Caracs",
						"CustomSkin",
						"ActionDoneThisTurn"
					}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])new GeneratedClrTypeInfo[1]),
					new GeneratedClrTypeInfo(typeof(FightSnapshot.Types.Companions), FightSnapshot.Types.Companions.Parser, new string[2]
					{
						"AllDefIds",
						"AvailableIds"
					}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null)
				}),
				new GeneratedClrTypeInfo(typeof(PlayerLeftFightEvent), PlayerLeftFightEvent.Parser, new string[2]
				{
					"FightId",
					"PlayerId"
				}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
				new GeneratedClrTypeInfo(typeof(PlayerReadyCmd), PlayerReadyCmd.Parser, (string[])null, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
				new GeneratedClrTypeInfo(typeof(MoveEntityCmd), MoveEntityCmd.Parser, new string[3]
				{
					"EntityId",
					"Path",
					"EntityToAttackId"
				}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
				new GeneratedClrTypeInfo(typeof(PlaySpellCmd), PlaySpellCmd.Parser, new string[2]
				{
					"SpellId",
					"CastTargets"
				}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
				new GeneratedClrTypeInfo(typeof(InvokeCompanionCmd), InvokeCompanionCmd.Parser, new string[2]
				{
					"CompanionDefId",
					"Coords"
				}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
				new GeneratedClrTypeInfo(typeof(GiveCompanionCmd), GiveCompanionCmd.Parser, new string[3]
				{
					"CompanionDefId",
					"TargetFightId",
					"TargetPlayerId"
				}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
				new GeneratedClrTypeInfo(typeof(UseReserveCmd), UseReserveCmd.Parser, (string[])null, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
				new GeneratedClrTypeInfo(typeof(EndOfTurnCmd), EndOfTurnCmd.Parser, new string[1]
				{
					"TurnIndex"
				}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
				new GeneratedClrTypeInfo(typeof(CommandHandledEvent), CommandHandledEvent.Parser, (string[])null, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
				new GeneratedClrTypeInfo(typeof(FightEventsEvent), FightEventsEvent.Parser, new string[2]
				{
					"FightId",
					"Events"
				}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null)
			}));
		}
	}
}
