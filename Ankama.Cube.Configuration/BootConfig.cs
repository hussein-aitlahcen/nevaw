using JetBrains.Annotations;

namespace Ankama.Cube.Configuration
{
	public static class BootConfig
	{
		public static bool initialized
		{
			get;
			private set;
		}

		public static string remoteConfigUrl
		{
			get;
			private set;
		} = string.Empty;


		public static void Read([NotNull] ConfigReader reader)
		{
			remoteConfigUrl = RemoteConfig.ReplaceVars(reader.GetUrl("remoteConfigUrl", string.Empty));
			initialized = true;
		}
	}
}
