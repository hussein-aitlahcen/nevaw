using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

namespace Ankama.Cube.Network.Spin2
{
	public abstract class AnkamaSpinCredentials : ISpinCredentials
	{
		public string CreateMessage()
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			int num = 2;
			int num2 = OsType();
			int num3 = DeviceType();
			StringWriter stringWriter = new StringWriter();
			JsonTextWriter val = new JsonTextWriter((TextWriter)stringWriter);
			try
			{
				val.WriteStartObject();
				WriteCredentials(val);
				val.WritePropertyName("clientType");
				val.WriteValue(num);
				val.WritePropertyName("osType");
				val.WriteValue(num2);
				val.WritePropertyName("deviceType");
				val.WriteValue(num3);
				val.WritePropertyName("deviceId");
				val.WriteValue(SystemInfo.get_deviceUniqueIdentifier());
				val.WriteEndObject();
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			return stringWriter.ToString();
		}

		protected abstract void WriteCredentials(JsonTextWriter jsonWriter);

		private static int DeviceType()
		{
			switch (Device.currentType)
			{
			case Device.Type.Mobile:
				return 1;
			case Device.Type.Tablet:
				return 2;
			case Device.Type.PC:
				return 3;
			default:
				return 3;
			}
		}

		private static int OsType()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected I4, but got Unknown
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Invalid comparison between Unknown and I4
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Invalid comparison between Unknown and I4
			RuntimePlatform platform = Application.get_platform();
			switch ((int)platform)
			{
			default:
				if ((int)platform != 11)
				{
					if ((int)platform != 13)
					{
						break;
					}
					return 5;
				}
				return 1;
			case 8:
				return 2;
			case 0:
			case 1:
				return 4;
			case 2:
			case 7:
				return 3;
			case 3:
			case 4:
			case 5:
			case 6:
				break;
			}
			return 0;
		}
	}
}
