using Google.Protobuf.Reflection;
using Google.Protobuf.WellKnownTypes;
using System;

namespace Ankama.Cube.Protocols.CommonProtocol
{
	public static class CommonProtocolReflection
	{
		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor => descriptor;

		static CommonProtocolReflection()
		{
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Expected O, but got Unknown
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Expected O, but got Unknown
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Expected O, but got Unknown
			descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String("ChRjb21tb25Qcm90b2NvbC5wcm90bxoeZ29vZ2xlL3Byb3RvYnVmL3dyYXBw" + "ZXJzLnByb3RvIkUKCkNhc3RUYXJnZXQSGgoEY2VsbBgBIAEoCzIKLkNlbGxD" + "b29yZEgAEhIKCGVudGl0eUlkGAIgASgFSABCBwoFdmFsdWUiIQoJQ2VsbENv" + "b3JkEgkKAXgYASABKAUSCQoBeRgCIAEoBSokCglDbWRSZXN1bHQSCgoGRmFp" + "bGVkEAASCwoHU3VjY2VzcxABKo8BChNEYW1hZ2VSZWR1Y3Rpb25UeXBlEgsK" + "B1VOS05PV04QABIKCgZTSElFTEQQARILCgdDT1VOVEVSEAISDQoJUFJPVEVD" + "VE9SEAMSDgoKUkVGTEVDVElPThAEEhAKDERBTUFHRV9QUk9PRhAFEg4KClJF" + "U0lTVEFOQ0UQBhIRCg1QRVRSSUZJQ0FUSU9OEAcqYgoMTW92ZW1lbnRUeXBl" + "EhoKFk1PVkVNRU5UX1RZUEVfTk9UX1VTRUQQABIICgRXQUxLEAESBwoDUlVO" + "EAISDAoIVEVMRVBPUlQQAxIJCgVTTElERRAEEgoKBkFUVEFDSxAFKj4KDVZh" + "bHVlTW9kaWZpZXISGwoXVkFMVUVfTU9ESUZJRVJfTk9UX1VTRUQQABIHCgNB" + "REQQARIHCgNTRVQQAiowCgtGaWdodFJlc3VsdBIICgREcmF3EAASCwoHVmlj" + "dG9yeRABEgoKBkRlZmVhdBACQj4KFWNvbS5hbmthbWEuY3ViZS5wcm90b6oC" + "JEFua2FtYS5DdWJlLlByb3RvY29scy5Db21tb25Qcm90b2NvbGIGcHJvdG8z"), (FileDescriptor[])new FileDescriptor[1]
			{
				WrappersReflection.get_Descriptor()
			}, new GeneratedClrTypeInfo(new Type[5]
			{
				typeof(CmdResult),
				typeof(DamageReductionType),
				typeof(MovementType),
				typeof(ValueModifier),
				typeof(FightResult)
			}, (GeneratedClrTypeInfo[])new GeneratedClrTypeInfo[2]
			{
				new GeneratedClrTypeInfo(typeof(CastTarget), CastTarget.Parser, new string[2]
				{
					"Cell",
					"EntityId"
				}, new string[1]
				{
					"Value"
				}, (Type[])null, (GeneratedClrTypeInfo[])null),
				new GeneratedClrTypeInfo(typeof(CellCoord), CellCoord.Parser, new string[2]
				{
					"X",
					"Y"
				}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null)
			}));
		}
	}
}
