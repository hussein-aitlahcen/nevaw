using Ankama.Cube.Protocols.CommonProtocol;
using Google.Protobuf.Reflection;
using Google.Protobuf.WellKnownTypes;
using System;

namespace Ankama.Cube.Protocols.FightCommonProtocol
{
	public static class FightCommonProtocolReflection
	{
		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor => descriptor;

		static FightCommonProtocolReflection()
		{
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Expected O, but got Unknown
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Expected O, but got Unknown
			//IL_023e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Expected O, but got Unknown
			//IL_0276: Unknown result type (might be due to invalid IL or missing references)
			//IL_027c: Expected O, but got Unknown
			//IL_027c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0282: Expected O, but got Unknown
			//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ee: Expected O, but got Unknown
			//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f4: Expected O, but got Unknown
			//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fe: Expected O, but got Unknown
			descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String("ChlmaWdodENvbW1vblByb3RvY29sLnByb3RvGh5nb29nbGUvcHJvdG9idWYv" + "d3JhcHBlcnMucHJvdG8aFGNvbW1vblByb3RvY29sLnByb3RvIq0BCg1TcGVs" + "bE1vdmVtZW50EhkKBXNwZWxsGAEgASgLMgouU3BlbGxJbmZvEiAKBGZyb20Y" + "AiABKA4yEi5TcGVsbE1vdmVtZW50Wm9uZRIeCgJ0bxgDIAEoDjISLlNwZWxs" + "TW92ZW1lbnRab25lEj8KG2Rpc2NhcmRlZEJlY2F1c2VIYW5kV2FzRnVsbBgE" + "IAEoCzIaLmdvb2dsZS5wcm90b2J1Zi5Cb29sVmFsdWUijQEKCVNwZWxsSW5m" + "bxIXCg9zcGVsbEluc3RhbmNlSWQYASABKAUSNgoRc3BlbGxEZWZpbml0aW9u" + "SWQYAiABKAsyGy5nb29nbGUucHJvdG9idWYuSW50MzJWYWx1ZRIvCgpzcGVs" + "bExldmVsGAMgASgLMhsuZ29vZ2xlLnByb3RvYnVmLkludDMyVmFsdWUisQIK" + "CUZpZ2h0SW5mbxISCgpmaWdodERlZklkGAEgASgFEhIKCmZpZ2h0TWFwSWQY" + "AiABKAUSEQoJZmlnaHRUeXBlGAMgASgFEh0KFWNvbmN1cnJlbnRGaWdodHND" + "b3VudBgEIAEoBRISCgpvd25GaWdodElkGAUgASgFEhQKDG93blRlYW1JbmRl" + "eBgGIAEoBRIeCgV0ZWFtcxgHIAMoCzIPLkZpZ2h0SW5mby5UZWFtGioKBFRl" + "YW0SIgoHcGxheWVycxgBIAMoCzIRLkZpZ2h0SW5mby5QbGF5ZXIaVAoGUGxh" + "eWVyEgwKBG5hbWUYASABKAkSDQoFbGV2ZWwYAiABKAUSLQoId2VhcG9uSWQY" + "AyABKAsyGy5nb29nbGUucHJvdG9idWYuSW50MzJWYWx1ZSLqAQoOR2FtZVN0" + "YXRpc3RpY3MSMAoLcGxheWVyU3RhdHMYASADKAsyGy5HYW1lU3RhdGlzdGlj" + "cy5QbGF5ZXJTdGF0cxqlAQoLUGxheWVyU3RhdHMSEAoIcGxheWVySWQYASAB" + "KAUSDwoHZmlnaHRJZBgCIAEoBRI1CgVzdGF0cxgDIAMoCzImLkdhbWVTdGF0" + "aXN0aWNzLlBsYXllclN0YXRzLlN0YXRzRW50cnkSDgoGdGl0bGVzGAQgAygF" + "GiwKClN0YXRzRW50cnkSCwoDa2V5GAEgASgFEg0KBXZhbHVlGAIgASgFOgI4" + "ASo0ChFTcGVsbE1vdmVtZW50Wm9uZRILCgdOT1dIRVJFEAASCAoESEFORBAB" + "EggKBERFQ0sQAipEChVDb21wYW5pb25SZXNlcnZlU3RhdGUSCAoESURMRRAA" + "EgwKCElOX0ZJR0hUEAESCQoFR0lWRU4QAhIICgRERUFEEAMqbgocVGVhbXNT" + "Y29yZU1vZGlmaWNhdGlvblJlYXNvbhIRCg1GSVJTVF9WSUNUT1JZEAASDgoK" + "SEVST19ERUFUSBABEhMKD0NPTVBBTklPTl9ERUFUSBACEhYKEkhFUk9fTElG" + "RV9NT0RJRklFRBADQkMKFWNvbS5hbmthbWEuY3ViZS5wcm90b6oCKUFua2Ft" + "YS5DdWJlLlByb3RvY29scy5GaWdodENvbW1vblByb3RvY29sYgZwcm90bzM="), (FileDescriptor[])new FileDescriptor[2]
			{
				WrappersReflection.get_Descriptor(),
				CommonProtocolReflection.Descriptor
			}, new GeneratedClrTypeInfo(new Type[3]
			{
				typeof(SpellMovementZone),
				typeof(CompanionReserveState),
				typeof(TeamsScoreModificationReason)
			}, (GeneratedClrTypeInfo[])new GeneratedClrTypeInfo[4]
			{
				new GeneratedClrTypeInfo(typeof(SpellMovement), SpellMovement.Parser, new string[4]
				{
					"Spell",
					"From",
					"To",
					"DiscardedBecauseHandWasFull"
				}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
				new GeneratedClrTypeInfo(typeof(SpellInfo), SpellInfo.Parser, new string[3]
				{
					"SpellInstanceId",
					"SpellDefinitionId",
					"SpellLevel"
				}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
				new GeneratedClrTypeInfo(typeof(FightInfo), FightInfo.Parser, new string[7]
				{
					"FightDefId",
					"FightMapId",
					"FightType",
					"ConcurrentFightsCount",
					"OwnFightId",
					"OwnTeamIndex",
					"Teams"
				}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])new GeneratedClrTypeInfo[2]
				{
					new GeneratedClrTypeInfo(typeof(FightInfo.Types.Team), FightInfo.Types.Team.Parser, new string[1]
					{
						"Players"
					}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
					new GeneratedClrTypeInfo(typeof(FightInfo.Types.Player), FightInfo.Types.Player.Parser, new string[3]
					{
						"Name",
						"Level",
						"WeaponId"
					}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null)
				}),
				new GeneratedClrTypeInfo(typeof(GameStatistics), GameStatistics.Parser, new string[1]
				{
					"PlayerStats"
				}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])new GeneratedClrTypeInfo[1]
				{
					new GeneratedClrTypeInfo(typeof(GameStatistics.Types.PlayerStats), GameStatistics.Types.PlayerStats.Parser, new string[4]
					{
						"PlayerId",
						"FightId",
						"Stats",
						"Titles"
					}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])new GeneratedClrTypeInfo[1])
				})
			}));
		}
	}
}
