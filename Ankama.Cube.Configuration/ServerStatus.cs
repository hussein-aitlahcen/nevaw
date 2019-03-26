using Ankama.Utilities;
using System;

namespace Ankama.Cube.Configuration
{
	public struct ServerStatus
	{
		public enum StatusCode
		{
			None,
			OK,
			Error,
			Maintenance,
			MaintenanceExpected
		}

		public readonly StatusCode code;

		public readonly DateTime maintenanceStartTimeUtc;

		public readonly TimeSpan maintenanceDuration;

		public static readonly ServerStatus ok = new ServerStatus(StatusCode.OK);

		public static readonly ServerStatus none = new ServerStatus(StatusCode.None);

		public static readonly ServerStatus error = new ServerStatus(StatusCode.Error);

		public DateTime maintenanceEstimatedEndTimeUtc => maintenanceStartTimeUtc + maintenanceDuration;

		private ServerStatus(StatusCode code)
		{
			this.code = code;
			maintenanceStartTimeUtc = DateTime.UtcNow;
			maintenanceDuration = TimeSpan.Zero;
		}

		public ServerStatus(DateTime maintenanceStartTimeUtc, TimeSpan maintenanceDuration)
		{
			code = ((maintenanceStartTimeUtc.CompareTo(DateTime.UtcNow) <= 0) ? StatusCode.Maintenance : StatusCode.MaintenanceExpected);
			this.maintenanceStartTimeUtc = maintenanceStartTimeUtc;
			this.maintenanceDuration = maintenanceDuration;
		}

		public static ServerStatus Parse(string text)
		{
			ConfigReader configReader = new ConfigReader(text);
			if (configReader.HasProperty("maintenanceStartTimeUtc"))
			{
				try
				{
					string @string = configReader.GetString("maintenanceStartTimeUtc");
					string string2 = configReader.GetString("maintenanceDuration");
					DateTime dateTime = DateTime.Parse(@string);
					TimeSpan timeSpan = TimeSpan.Parse(string2);
					return new ServerStatus(dateTime, timeSpan);
				}
				catch (Exception ex)
				{
					Log.Error("Error parsing serverStatus: {text}", (object)ex, 56, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Configuration\\ServerStatus.cs");
					return new ServerStatus(StatusCode.Error);
				}
			}
			return new ServerStatus(StatusCode.OK);
		}
	}
}
