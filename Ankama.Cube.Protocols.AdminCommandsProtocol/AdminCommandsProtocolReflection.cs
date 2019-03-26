using Google.Protobuf.Reflection;
using System;

namespace Ankama.Cube.Protocols.AdminCommandsProtocol
{
	public static class AdminCommandsProtocolReflection
	{
		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor => descriptor;

		static AdminCommandsProtocolReflection()
		{
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Expected O, but got Unknown
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Expected O, but got Unknown
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Expected O, but got Unknown
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Expected O, but got Unknown
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Expected O, but got Unknown
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Expected O, but got Unknown
			descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String("ChthZG1pbkNvbW1hbmRzUHJvdG9jb2wucHJvdG8ixQIKCEFkbWluQ21kEgoK" + "AmlkGAEgASgFEhsKEWdpdmVBbGxDb21wYW5pb25zGAIgASgISAASGAoOZ2l2" + "ZUFsbFdlYXBvbnMYAyABKAhIABIsCg5zZXRXZWFwb25MZXZlbBgEIAEoCzIS" + "LkFkbWluQ21kLlNldExldmVsSAASNAoSc2V0QWxsV2VhcG9uTGV2ZWxzGAUg" + "ASgLMhYuQWRtaW5DbWQuU2V0QWxsTGV2ZWxzSAASKAoJc2V0R2VuZGVyGAYg" + "ASgLMhMuQWRtaW5DbWQuU2V0R2VuZGVySAAaJQoIU2V0TGV2ZWwSCgoCaWQY" + "ASABKAUSDQoFbGV2ZWwYAiABKAUaHQoMU2V0QWxsTGV2ZWxzEg0KBWxldmVs" + "GAEgASgFGhsKCVNldEdlbmRlchIOCgZnZW5kZXIYASABKAVCBQoDY21kIkIK" + "E0FkbWluQ21kUmVzdWx0RXZlbnQSCgoCaWQYASABKAUSDwoHc3VjY2VzcxgC" + "IAEoCBIOCgZyZXN1bHQYAyABKAlCRQoVY29tLmFua2FtYS5jdWJlLnByb3Rv" + "qgIrQW5rYW1hLkN1YmUuUHJvdG9jb2xzLkFkbWluQ29tbWFuZHNQcm90b2Nv" + "bGIGcHJvdG8z"), (FileDescriptor[])new FileDescriptor[0], new GeneratedClrTypeInfo((Type[])null, (GeneratedClrTypeInfo[])new GeneratedClrTypeInfo[2]
			{
				new GeneratedClrTypeInfo(typeof(AdminCmd), AdminCmd.Parser, new string[6]
				{
					"Id",
					"GiveAllCompanions",
					"GiveAllWeapons",
					"SetWeaponLevel",
					"SetAllWeaponLevels",
					"SetGender"
				}, new string[1]
				{
					"Cmd"
				}, (Type[])null, (GeneratedClrTypeInfo[])new GeneratedClrTypeInfo[3]
				{
					new GeneratedClrTypeInfo(typeof(AdminCmd.Types.SetLevel), AdminCmd.Types.SetLevel.Parser, new string[2]
					{
						"Id",
						"Level"
					}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
					new GeneratedClrTypeInfo(typeof(AdminCmd.Types.SetAllLevels), AdminCmd.Types.SetAllLevels.Parser, new string[1]
					{
						"Level"
					}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
					new GeneratedClrTypeInfo(typeof(AdminCmd.Types.SetGender), AdminCmd.Types.SetGender.Parser, new string[1]
					{
						"Gender"
					}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null)
				}),
				new GeneratedClrTypeInfo(typeof(AdminCmdResultEvent), AdminCmdResultEvent.Parser, new string[3]
				{
					"Id",
					"Success",
					"Result"
				}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null)
			}));
		}
	}
}
