using Ankama.Cube.Configuration;

namespace Ankama.Cube.TEMPFastEnterMatch.Auth.ConnectionMethod
{
	public static class CredentialProvider
	{
		private static readonly CredentialProviderWrapper s_gameProvider = new CredentialProviderWrapper();

		private static readonly CredentialProviderWrapper s_chatProvider = new CredentialProviderWrapper();

		public static ICredentialProvider chatCredentialProvider
		{
			get
			{
				if (!s_chatProvider.IsInit())
				{
					s_chatProvider.SetProvider(ChatProvider());
				}
				return s_chatProvider;
			}
		}

		public static ICredentialProvider gameCredentialProvider
		{
			get
			{
				if (!s_gameProvider.IsInit())
				{
					s_gameProvider.SetProvider(GameProvider());
				}
				return s_gameProvider;
			}
		}

		public static bool HasGuestAccount()
		{
			return false;
		}

		public static bool HasGuestMode()
		{
			return false;
		}

		public static LoginUIType LoginUIType()
		{
			return Ankama.Cube.TEMPFastEnterMatch.Auth.ConnectionMethod.LoginUIType.LoginPassword;
		}

		private static ICredentialProvider ChatProvider()
		{
			if (LauncherConnection.instance.get_opened())
			{
				return new LauncherLinkCredentialProvider(ApplicationConfig.ChatAppId);
			}
			return new LoginPasswordCredentialProvider();
		}

		private static ICredentialProvider GameProvider()
		{
			if (!ApplicationConfig.haapiAllowed)
			{
				return new NoHaapiLoginPasswordCredentialProvider();
			}
			if (LauncherConnection.instance.get_opened())
			{
				return new LauncherLinkCredentialProvider(ApplicationConfig.GameAppId);
			}
			return new LoginPasswordCredentialProvider();
		}

		public static void DeteteCredentialProviders()
		{
			s_chatProvider.SetProvider(null);
			s_gameProvider.SetProvider(null);
		}
	}
}
