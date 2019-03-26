using Ankama.Cube.Protocols.CommonProtocol;
using Google.Protobuf.Reflection;
using System;

namespace Ankama.Cube.Protocols.FightAdminProtocol
{
	public static class FightAdminProtocolReflection
	{
		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor => descriptor;

		static FightAdminProtocolReflection()
		{
			//IL_0299: Unknown result type (might be due to invalid IL or missing references)
			//IL_029f: Expected O, but got Unknown
			//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cf: Expected O, but got Unknown
			//IL_0301: Unknown result type (might be due to invalid IL or missing references)
			//IL_0307: Expected O, but got Unknown
			//IL_0329: Unknown result type (might be due to invalid IL or missing references)
			//IL_032f: Expected O, but got Unknown
			//IL_0359: Unknown result type (might be due to invalid IL or missing references)
			//IL_035f: Expected O, but got Unknown
			//IL_0389: Unknown result type (might be due to invalid IL or missing references)
			//IL_038f: Expected O, but got Unknown
			//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bf: Expected O, but got Unknown
			//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ef: Expected O, but got Unknown
			//IL_0419: Unknown result type (might be due to invalid IL or missing references)
			//IL_041f: Expected O, but got Unknown
			//IL_044a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0450: Expected O, but got Unknown
			//IL_048b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0491: Expected O, but got Unknown
			//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ca: Expected O, but got Unknown
			//IL_0505: Unknown result type (might be due to invalid IL or missing references)
			//IL_050b: Expected O, but got Unknown
			//IL_0546: Unknown result type (might be due to invalid IL or missing references)
			//IL_054c: Expected O, but got Unknown
			//IL_054c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0552: Expected O, but got Unknown
			//IL_0552: Unknown result type (might be due to invalid IL or missing references)
			//IL_055c: Expected O, but got Unknown
			descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String("ChhmaWdodEFkbWluUHJvdG9jb2wucHJvdG8aFGNvbW1vblByb3RvY29sLnBy" + "b3RvIr4PCg9BZG1pblJlcXVlc3RDbWQSOQoKZGVhbERhbWFnZRgBIAEoCzIj" + "LkFkbWluUmVxdWVzdENtZC5EZWFsRGFtYWdlQWRtaW5DbWRIABItCgRraWxs" + "GAIgASgLMh0uQWRtaW5SZXF1ZXN0Q21kLktpbGxBZG1pbkNtZEgAEjUKCHRl" + "bGVwb3J0GAMgASgLMiEuQWRtaW5SZXF1ZXN0Q21kLlRlbGVwb3J0QWRtaW5D" + "bWRIABI0CgpkcmF3U3BlbGxzGAQgASgLMh4uQWRtaW5SZXF1ZXN0Q21kLkRy" + "YXdTcGVsbHNDbWRIABI6Cg1kaXNjYXJkU3BlbGxzGAUgASgLMiEuQWRtaW5S" + "ZXF1ZXN0Q21kLkRpc2NhcmRTcGVsbHNDbWRIABJCChFnYWluRWxlbWVudFBv" + "aW50cxgGIAEoCzIlLkFkbWluUmVxdWVzdENtZC5HYWluRWxlbWVudFBvaW50" + "c0NtZEgAEkAKEGdhaW5BY3Rpb25Qb2ludHMYByABKAsyJC5BZG1pblJlcXVl" + "c3RDbWQuR2FpbkFjdGlvblBvaW50c0NtZEgAEkIKEWdhaW5SZXNlcnZlUG9p" + "bnRzGAggASgLMiUuQWRtaW5SZXF1ZXN0Q21kLkdhaW5SZXNlcnZlUG9pbnRz" + "Q21kSAASMgoJcGlja1NwZWxsGAkgASgLMh0uQWRtaW5SZXF1ZXN0Q21kLlBp" + "Y2tTcGVsbENtZEgAEjYKC3NldFByb3BlcnR5GAogASgLMh8uQWRtaW5SZXF1" + "ZXN0Q21kLlNldFByb3BlcnR5Q21kSAASLQoEaGVhbBgLIAEoCzIdLkFkbWlu" + "UmVxdWVzdENtZC5IZWFsQWRtaW5DbWRIABJDCg9pbnZva2VTdW1tb25pbmcY" + "DCABKAsyKC5BZG1pblJlcXVlc3RDbWQuSW52b2tlU3VtbW9uaW5nQWRtaW5D" + "bWRIABJDCg9pbnZva2VDb21wYW5pb24YDSABKAsyKC5BZG1pblJlcXVlc3RD" + "bWQuSW52b2tlQ29tcGFuaW9uQWRtaW5DbWRIABJJChJzZXRFbGVtZW50YXJ5" + "U3RhdGUYDiABKAsyKy5BZG1pblJlcXVlc3RDbWQuU2V0RWxlbWVudGFyeVN0" + "YXRlQWRtaW5DbWRIABpMCg5TZXRQcm9wZXJ0eUNtZBIWCg50YXJnZXRFbnRp" + "dHlJZBgBIAEoBRISCgpwcm9wZXJ0eUlkGAIgASgFEg4KBmFjdGl2ZRgDIAEo" + "CBpPChpTZXRFbGVtZW50YXJ5U3RhdGVBZG1pbkNtZBIWCg50YXJnZXRFbnRp" + "dHlJZBgBIAEoBRIZChFlbGVtZW50YXJ5U3RhdGVJZBgCIAEoBRpPChJEZWFs" + "RGFtYWdlQWRtaW5DbWQSFgoOdGFyZ2V0RW50aXR5SWQYASABKAUSEAoIcXVh" + "bnRpdHkYAiABKAUSDwoHbWFnaWNhbBgDIAEoCBomCgxLaWxsQWRtaW5DbWQS" + "FgoOdGFyZ2V0RW50aXR5SWQYASABKAUaSwoQVGVsZXBvcnRBZG1pbkNtZBIW" + "Cg50YXJnZXRFbnRpdHlJZBgBIAEoBRIfCgtkZXN0aW5hdGlvbhgCIAEoCzIK" + "LkNlbGxDb29yZBo5Cg1EcmF3U3BlbGxzQ21kEhYKDnBsYXllckVudGl0eUlk" + "GAEgASgFEhAKCHF1YW50aXR5GAIgASgFGjwKEERpc2NhcmRTcGVsbHNDbWQS" + "FgoOcGxheWVyRW50aXR5SWQYASABKAUSEAoIcXVhbnRpdHkYAiABKAUaQAoU" + "R2FpbkVsZW1lbnRQb2ludHNDbWQSFgoOcGxheWVyRW50aXR5SWQYASABKAUS" + "EAoIcXVhbnRpdHkYAiABKAUaPwoTR2FpbkFjdGlvblBvaW50c0NtZBIWCg5w" + "bGF5ZXJFbnRpdHlJZBgBIAEoBRIQCghxdWFudGl0eRgCIAEoBRpAChRHYWlu" + "UmVzZXJ2ZVBvaW50c0NtZBIWCg5wbGF5ZXJFbnRpdHlJZBgBIAEoBRIQCghx" + "dWFudGl0eRgCIAEoBRpnCgxQaWNrU3BlbGxDbWQSFgoOcGxheWVyRW50aXR5" + "SWQYASABKAUSEAoIcXVhbnRpdHkYAiABKAUSGQoRc3BlbGxEZWZpbml0aW9u" + "SWQYAyABKAUSEgoKc3BlbGxMZXZlbBgEIAEoBRpJCgxIZWFsQWRtaW5DbWQS" + "FgoOdGFyZ2V0RW50aXR5SWQYASABKAUSEAoIcXVhbnRpdHkYAiABKAUSDwoH" + "bWFnaWNhbBgDIAEoCBp/ChdJbnZva2VTdW1tb25pbmdBZG1pbkNtZBIUCgxk" + "ZWZpbml0aW9uSWQYASABKAUSFQoNb3duZXJFbnRpdHlJZBgCIAEoBRIWCg5z" + "dW1tb25pbmdMZXZlbBgDIAEoBRIfCgtkZXN0aW5hdGlvbhgEIAEoCzIKLkNl" + "bGxDb29yZBp/ChdJbnZva2VDb21wYW5pb25BZG1pbkNtZBIUCgxkZWZpbml0" + "aW9uSWQYASABKAUSFQoNb3duZXJFbnRpdHlJZBgCIAEoBRIWCg5jb21wYW5p" + "b25MZXZlbBgDIAEoBRIfCgtkZXN0aW5hdGlvbhgEIAEoCzIKLkNlbGxDb29y" + "ZEIFCgNjbWRCQgoVY29tLmFua2FtYS5jdWJlLnByb3RvqgIoQW5rYW1hLkN1" + "YmUuUHJvdG9jb2xzLkZpZ2h0QWRtaW5Qcm90b2NvbGIGcHJvdG8z"), (FileDescriptor[])new FileDescriptor[1]
			{
				CommonProtocolReflection.Descriptor
			}, new GeneratedClrTypeInfo((Type[])null, (GeneratedClrTypeInfo[])new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(AdminRequestCmd), AdminRequestCmd.Parser, new string[14]
				{
					"DealDamage",
					"Kill",
					"Teleport",
					"DrawSpells",
					"DiscardSpells",
					"GainElementPoints",
					"GainActionPoints",
					"GainReservePoints",
					"PickSpell",
					"SetProperty",
					"Heal",
					"InvokeSummoning",
					"InvokeCompanion",
					"SetElementaryState"
				}, new string[1]
				{
					"Cmd"
				}, (Type[])null, (GeneratedClrTypeInfo[])new GeneratedClrTypeInfo[14]
				{
					new GeneratedClrTypeInfo(typeof(AdminRequestCmd.Types.SetPropertyCmd), AdminRequestCmd.Types.SetPropertyCmd.Parser, new string[3]
					{
						"TargetEntityId",
						"PropertyId",
						"Active"
					}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
					new GeneratedClrTypeInfo(typeof(AdminRequestCmd.Types.SetElementaryStateAdminCmd), AdminRequestCmd.Types.SetElementaryStateAdminCmd.Parser, new string[2]
					{
						"TargetEntityId",
						"ElementaryStateId"
					}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
					new GeneratedClrTypeInfo(typeof(AdminRequestCmd.Types.DealDamageAdminCmd), AdminRequestCmd.Types.DealDamageAdminCmd.Parser, new string[3]
					{
						"TargetEntityId",
						"Quantity",
						"Magical"
					}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
					new GeneratedClrTypeInfo(typeof(AdminRequestCmd.Types.KillAdminCmd), AdminRequestCmd.Types.KillAdminCmd.Parser, new string[1]
					{
						"TargetEntityId"
					}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
					new GeneratedClrTypeInfo(typeof(AdminRequestCmd.Types.TeleportAdminCmd), AdminRequestCmd.Types.TeleportAdminCmd.Parser, new string[2]
					{
						"TargetEntityId",
						"Destination"
					}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
					new GeneratedClrTypeInfo(typeof(AdminRequestCmd.Types.DrawSpellsCmd), AdminRequestCmd.Types.DrawSpellsCmd.Parser, new string[2]
					{
						"PlayerEntityId",
						"Quantity"
					}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
					new GeneratedClrTypeInfo(typeof(AdminRequestCmd.Types.DiscardSpellsCmd), AdminRequestCmd.Types.DiscardSpellsCmd.Parser, new string[2]
					{
						"PlayerEntityId",
						"Quantity"
					}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
					new GeneratedClrTypeInfo(typeof(AdminRequestCmd.Types.GainElementPointsCmd), AdminRequestCmd.Types.GainElementPointsCmd.Parser, new string[2]
					{
						"PlayerEntityId",
						"Quantity"
					}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
					new GeneratedClrTypeInfo(typeof(AdminRequestCmd.Types.GainActionPointsCmd), AdminRequestCmd.Types.GainActionPointsCmd.Parser, new string[2]
					{
						"PlayerEntityId",
						"Quantity"
					}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
					new GeneratedClrTypeInfo(typeof(AdminRequestCmd.Types.GainReservePointsCmd), AdminRequestCmd.Types.GainReservePointsCmd.Parser, new string[2]
					{
						"PlayerEntityId",
						"Quantity"
					}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
					new GeneratedClrTypeInfo(typeof(AdminRequestCmd.Types.PickSpellCmd), AdminRequestCmd.Types.PickSpellCmd.Parser, new string[4]
					{
						"PlayerEntityId",
						"Quantity",
						"SpellDefinitionId",
						"SpellLevel"
					}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
					new GeneratedClrTypeInfo(typeof(AdminRequestCmd.Types.HealAdminCmd), AdminRequestCmd.Types.HealAdminCmd.Parser, new string[3]
					{
						"TargetEntityId",
						"Quantity",
						"Magical"
					}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
					new GeneratedClrTypeInfo(typeof(AdminRequestCmd.Types.InvokeSummoningAdminCmd), AdminRequestCmd.Types.InvokeSummoningAdminCmd.Parser, new string[4]
					{
						"DefinitionId",
						"OwnerEntityId",
						"SummoningLevel",
						"Destination"
					}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
					new GeneratedClrTypeInfo(typeof(AdminRequestCmd.Types.InvokeCompanionAdminCmd), AdminRequestCmd.Types.InvokeCompanionAdminCmd.Parser, new string[4]
					{
						"DefinitionId",
						"OwnerEntityId",
						"CompanionLevel",
						"Destination"
					}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null)
				})
			}));
		}
	}
}
