using Ankama.Cube.Network.Spin2;

namespace Ankama.Cube.TEMPFastEnterMatch.Auth.ConnectionMethod
{
	public interface ICredentialProvider : ISpinCredentialsProvider
	{
		AutoConnectLevel AutoConnectLevel();

		bool CanDisconnect();

		bool CanDisplayDisconnectButton();

		bool HasGuestMode();

		LoginUIType LoginUIType();

		bool HasGuestAccount();

		void CleanCredentials();
	}
}
