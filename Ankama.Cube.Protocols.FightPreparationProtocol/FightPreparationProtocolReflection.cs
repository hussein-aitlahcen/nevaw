using Ankama.Cube.Protocols.PlayerProtocol;
using Google.Protobuf.Reflection;
using System;

namespace Ankama.Cube.Protocols.FightPreparationProtocol
{
	public static class FightPreparationProtocolReflection
	{
		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor => descriptor;

		static FightPreparationProtocolReflection()
		{
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Expected O, but got Unknown
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Expected O, but got Unknown
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Expected O, but got Unknown
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Expected O, but got Unknown
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Expected O, but got Unknown
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Expected O, but got Unknown
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Expected O, but got Unknown
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_020d: Expected O, but got Unknown
			//IL_0241: Unknown result type (might be due to invalid IL or missing references)
			//IL_0247: Expected O, but got Unknown
			//IL_0247: Unknown result type (might be due to invalid IL or missing references)
			//IL_0251: Expected O, but got Unknown
			descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String("Ch5maWdodFByZXBhcmF0aW9uUHJvdG9jb2wucHJvdG8aFHBsYXllclByb3Rv" + "Y29sLnByb3RvIhUKE0NyZWF0ZUZpZ2h0R3JvdXBDbWQiFAoSTGVhdmVGaWdo" + "dEdyb3VwQ21kIlIKFkZpZ2h0R3JvdXBVcGRhdGVkRXZlbnQSFAoMZ3JvdXBS" + "ZW1vdmVkGAEgASgIEiIKB21lbWJlcnMYAiADKAsyES5QbGF5ZXJQdWJsaWNJ" + "bmZvIioKFExhdW5jaE1hdGNobWFraW5nQ21kEhIKCmZpZ2h0RGVmSWQYASAB" + "KAUiHgocRm9yY2VNYXRjaG1ha2luZ0FnYWluc3RBSUNtZCKyAgocTGF1bmNo" + "TWF0Y2htYWtpbmdSZXN1bHRFdmVudBISCgpmaWdodERlZklkGAEgASgFEjQK" + "BnJlc3VsdBgCIAEoDjIkLkxhdW5jaE1hdGNobWFraW5nUmVzdWx0RXZlbnQu" + "UmVzdWx0IscBCgZSZXN1bHQSBgoCT0sQABISCg5JTlRFUk5BTF9FUlJPUhAB" + "EhgKFFZBTElEX0RFQ0tfTk9UX0ZPVU5EEAISGQoVT05MWV9PV05FUl9DQU5f" + "TEFVTkNIEAMSFQoRR1JPVVBfTk9UX0NSRUFURUQQBBIZChVTT01FX1BMQVlF" + "Ul9OT1RfUkVBRFkQBRIpCiVUT09fTUFOWV9QTEFZRVJTX0ZPUl9GSUdIVF9E" + "RUZJTklUSU9OEAYSDwoLUExBWUVSX0xFRlQQByItChdNYXRjaG1ha2luZ1N0" + "YXJ0ZWRFdmVudBISCgpmaWdodERlZklkGAEgASgFIi0KF01hdGNobWFraW5n" + "U3VjY2Vzc0V2ZW50EhIKCmZpZ2h0RGVmSWQYASABKAUikAEKF01hdGNobWFr" + "aW5nU3RvcHBlZEV2ZW50Ei8KBnJlYXNvbhgBIAEoDjIfLk1hdGNobWFraW5n" + "U3RvcHBlZEV2ZW50LlJlYXNvbiJECgZSZWFzb24SFgoSQ0FOX1RfQ1JFQVRF" + "X0ZJR0hUEAASDAoIQ0FOQ0VMRUQQARIUChBTT01FX1BMQVlFUl9MRUZUEAJC" + "SAoVY29tLmFua2FtYS5jdWJlLnByb3RvqgIuQW5rYW1hLkN1YmUuUHJvdG9j" + "b2xzLkZpZ2h0UHJlcGFyYXRpb25Qcm90b2NvbGIGcHJvdG8z"), (FileDescriptor[])new FileDescriptor[1]
			{
				PlayerProtocolReflection.Descriptor
			}, new GeneratedClrTypeInfo((Type[])null, (GeneratedClrTypeInfo[])new GeneratedClrTypeInfo[9]
			{
				new GeneratedClrTypeInfo(typeof(CreateFightGroupCmd), CreateFightGroupCmd.Parser, (string[])null, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
				new GeneratedClrTypeInfo(typeof(LeaveFightGroupCmd), LeaveFightGroupCmd.Parser, (string[])null, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
				new GeneratedClrTypeInfo(typeof(FightGroupUpdatedEvent), FightGroupUpdatedEvent.Parser, new string[2]
				{
					"GroupRemoved",
					"Members"
				}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
				new GeneratedClrTypeInfo(typeof(LaunchMatchmakingCmd), LaunchMatchmakingCmd.Parser, new string[1]
				{
					"FightDefId"
				}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
				new GeneratedClrTypeInfo(typeof(ForceMatchmakingAgainstAICmd), ForceMatchmakingAgainstAICmd.Parser, (string[])null, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
				new GeneratedClrTypeInfo(typeof(LaunchMatchmakingResultEvent), LaunchMatchmakingResultEvent.Parser, new string[2]
				{
					"FightDefId",
					"Result"
				}, (string[])null, new Type[1]
				{
					typeof(LaunchMatchmakingResultEvent.Types.Result)
				}, (GeneratedClrTypeInfo[])null),
				new GeneratedClrTypeInfo(typeof(MatchmakingStartedEvent), MatchmakingStartedEvent.Parser, new string[1]
				{
					"FightDefId"
				}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
				new GeneratedClrTypeInfo(typeof(MatchmakingSuccessEvent), MatchmakingSuccessEvent.Parser, new string[1]
				{
					"FightDefId"
				}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
				new GeneratedClrTypeInfo(typeof(MatchmakingStoppedEvent), MatchmakingStoppedEvent.Parser, new string[1]
				{
					"Reason"
				}, (string[])null, new Type[1]
				{
					typeof(MatchmakingStoppedEvent.Types.Reason)
				}, (GeneratedClrTypeInfo[])null)
			}));
		}
	}
}
