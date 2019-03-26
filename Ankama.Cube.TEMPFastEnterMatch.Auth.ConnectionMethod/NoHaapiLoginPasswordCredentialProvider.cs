using Ankama.Cube.Network.Spin2;
using Ankama.Cube.Player;
using System.Threading.Tasks;

namespace Ankama.Cube.TEMPFastEnterMatch.Auth.ConnectionMethod
{
	public class NoHaapiLoginPasswordCredentialProvider : ICredentialProvider, ISpinCredentialsProvider
	{
		public async Task<ISpinCredentials> GetCredentials()
		{
			TaskCompletionSource<ISpinCredentials> taskCompletionSource = new TaskCompletionSource<ISpinCredentials>();
			DoRequest(taskCompletionSource);
			return await taskCompletionSource.Task;
		}

		private void DoRequest(TaskCompletionSource<ISpinCredentials> task)
		{
			string lastLogin = PlayerPreferences.lastLogin;
			task.SetResult(new AnkamaNoHaapiCredentials(lastLogin));
		}

		public AutoConnectLevel AutoConnectLevel()
		{
			if (PlayerPreferences.autoLogin)
			{
				if (CredentialProvider.HasGuestAccount() && HasGuestMode() && PlayerPreferences.useGuest)
				{
					return Ankama.Cube.TEMPFastEnterMatch.Auth.ConnectionMethod.AutoConnectLevel.IfAvailable;
				}
				if (PlayerPreferences.rememberPassword && !string.IsNullOrEmpty(PlayerPreferences.lastLogin) && !string.IsNullOrEmpty(PlayerPreferences.lastPassword))
				{
					return Ankama.Cube.TEMPFastEnterMatch.Auth.ConnectionMethod.AutoConnectLevel.IfAvailable;
				}
			}
			return Ankama.Cube.TEMPFastEnterMatch.Auth.ConnectionMethod.AutoConnectLevel.NoAutoConnect;
		}

		public bool CanDisconnect()
		{
			return true;
		}

		public bool CanDisplayDisconnectButton()
		{
			return true;
		}

		public bool HasGuestMode()
		{
			return false;
		}

		public LoginUIType LoginUIType()
		{
			return Ankama.Cube.TEMPFastEnterMatch.Auth.ConnectionMethod.LoginUIType.LoginPassword;
		}

		public bool HasGuestAccount()
		{
			return false;
		}

		public void CleanCredentials()
		{
			PlayerPreferences.autoLogin = false;
			PlayerPreferences.lastPassword = "";
			PlayerPreferences.Save();
		}
	}
}
