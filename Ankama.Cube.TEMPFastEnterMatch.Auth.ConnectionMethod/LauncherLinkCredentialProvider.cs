using Ankama.Cube.Configuration;
using Ankama.Cube.Network.Spin2;
using Ankama.Launcher.Messages;
using Ankama.Utilities;
using System.Threading.Tasks;

namespace Ankama.Cube.TEMPFastEnterMatch.Auth.ConnectionMethod
{
	public class LauncherLinkCredentialProvider : ICredentialProvider, ISpinCredentialsProvider
	{
		private readonly int m_serviceId;

		public LauncherLinkCredentialProvider(int serviceId)
		{
			m_serviceId = serviceId;
		}

		public async Task<ISpinCredentials> GetCredentials()
		{
			TaskCompletionSource<ISpinCredentials> taskCompletionSource = new TaskCompletionSource<ISpinCredentials>();
			DoRequest(taskCompletionSource);
			return await taskCompletionSource.Task;
		}

		private void DoRequest(TaskCompletionSource<ISpinCredentials> task)
		{
			LauncherConnection.RequestApiToken(delegate(ApiToken token)
			{
				if (token != null)
				{
					Log.Info("Launcher token: " + token.get_Value(), 210, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\TEMPFastEnterMatch\\Auth\\ConnectionMethod\\ICredentialProvider.cs");
					task.SetResult(new AnkamaTokenCredentials(token.get_Value()));
				}
				else
				{
					task.SetException(new CredentialException(string.Format("Unable to get token from launcher {0}={1}", "m_serviceId", m_serviceId)));
				}
			}, m_serviceId);
		}

		public AutoConnectLevel AutoConnectLevel()
		{
			return Ankama.Cube.TEMPFastEnterMatch.Auth.ConnectionMethod.AutoConnectLevel.Mandatory;
		}

		public bool CanDisconnect()
		{
			return false;
		}

		public bool CanDisplayDisconnectButton()
		{
			return false;
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
		}
	}
}
