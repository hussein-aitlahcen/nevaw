using Google.Protobuf.Reflection;
using System;

namespace Ankama.Cube.Protocols.ServerProtocol
{
	public static class ServerProtocolReflection
	{
		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor => descriptor;

		static ServerProtocolReflection()
		{
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Expected O, but got Unknown
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Expected O, but got Unknown
			descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String("ChRzZXJ2ZXJQcm90b2NvbC5wcm90byLBAQoZRGlzY29ubmVjdGVkQnlTZXJ2" + "ZXJFdmVudBIxCgZyZWFzb24YASABKA4yIS5EaXNjb25uZWN0ZWRCeVNlcnZl" + "ckV2ZW50LlJlYXNvbiJxCgZSZWFzb24SCwoHVW5rbm93bhAAEgkKBUVycm9y" + "EAESFAoQU2VydmVySXNTdG9wcGluZxACEhcKE1VuYWJsZVRvTG9hZEFjY291" + "bnQQAxIgChxMb2dnZWRJbkFnYWluV2l0aFNhbWVBY2NvdW50EARCPgoVY29t" + "LmFua2FtYS5jdWJlLnByb3RvqgIkQW5rYW1hLkN1YmUuUHJvdG9jb2xzLlNl" + "cnZlclByb3RvY29sYgZwcm90bzM="), (FileDescriptor[])new FileDescriptor[0], new GeneratedClrTypeInfo((Type[])null, (GeneratedClrTypeInfo[])new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(DisconnectedByServerEvent), DisconnectedByServerEvent.Parser, new string[1]
				{
					"Reason"
				}, (string[])null, new Type[1]
				{
					typeof(DisconnectedByServerEvent.Types.Reason)
				}, (GeneratedClrTypeInfo[])null)
			}));
		}
	}
}
