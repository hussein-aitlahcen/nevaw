using Ankama.Cube.Network.Spin2;
using System.Threading.Tasks;

namespace Ankama.Cube.TEMPFastEnterMatch.Auth.ConnectionMethod
{
	internal class CredentialProviderWrapper : ICredentialProvider, ISpinCredentialsProvider
	{
		private ICredentialProvider m_provider;

		public bool IsInit()
		{
			return m_provider != null;
		}

		public void SetProvider(ICredentialProvider provider)
		{
			m_provider = provider;
		}

		public AutoConnectLevel AutoConnectLevel()
		{
			return m_provider.AutoConnectLevel();
		}

		public bool CanDisconnect()
		{
			return m_provider.CanDisconnect();
		}

		public bool CanDisplayDisconnectButton()
		{
			return m_provider.CanDisplayDisconnectButton();
		}

		public bool HasGuestMode()
		{
			return m_provider.HasGuestMode();
		}

		public LoginUIType LoginUIType()
		{
			return m_provider.LoginUIType();
		}

		public bool HasGuestAccount()
		{
			return m_provider.HasGuestAccount();
		}

		public void CleanCredentials()
		{
			m_provider.CleanCredentials();
		}

		public async Task<ISpinCredentials> GetCredentials()
		{
			return await m_provider.GetCredentials();
		}
	}
}
