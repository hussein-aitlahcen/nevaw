using Ankama.Cube.Configuration;
using Ankama.Cube.Network.Spin2;
using Ankama.Cube.Player;
using Ankama.Utilities;
using Com.Ankama.Haapi.Swagger.Api;
using Com.Ankama.Haapi.Swagger.Model;
using IO.Swagger.Client;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Ankama.Cube.TEMPFastEnterMatch.Auth.ConnectionMethod
{
	public class LoginPasswordCredentialProvider : ICredentialProvider, ISpinCredentialsProvider
	{
		public async Task<ISpinCredentials> GetCredentials()
		{
			TaskCompletionSource<ISpinCredentials> taskCompletionSource = new TaskCompletionSource<ISpinCredentials>();
			DoRequest(taskCompletionSource);
			return await taskCompletionSource.Task;
		}

		private void DoRequest(TaskCompletionSource<ISpinCredentials> task)
		{
			string login = PlayerPreferences.useGuest ? PlayerPreferences.guestLogin : PlayerPreferences.lastLogin;
			string password = PlayerPreferences.useGuest ? PlayerPreferences.guestPassword : PlayerPreferences.lastPassword;
			HaapiManager.ExecuteRequest(() => HaapiManager.accountApi.CreateTokenWithPassword(login, password, (long?)ApplicationConfig.GameAppId), delegate(RAccountApi<Token> res)
			{
				Log.Info("CreateTokenWithPassword success ! Login=" + res.Data.get__Token(), 277, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\TEMPFastEnterMatch\\Auth\\ConnectionMethod\\ICredentialProvider.cs");
				task.SetResult(new AnkamaTokenCredentials(res.Data.get__Token()));
			}, delegate(Exception exception)
			{
				ApiException val = exception as ApiException;
				if (val != null && val.get_ErrorCode() == 601)
				{
					Log.Error($"CreateTokenWithPassword error: {exception}", 284, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\TEMPFastEnterMatch\\Auth\\ConnectionMethod\\ICredentialProvider.cs");
					ErrorAccountLogin val2 = JsonConvert.DeserializeObject<ErrorAccountLogin>((string)val.get_ErrorContent());
					SpinConnectionError spinConnectionError = (val2 == null) ? null : HaapiHelper.From(val2);
					task.SetException(spinConnectionError ?? exception);
				}
				else
				{
					Log.Error($"CreateTokenWithPassword error: {exception}", 291, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\TEMPFastEnterMatch\\Auth\\ConnectionMethod\\ICredentialProvider.cs");
					task.SetException(exception);
				}
			});
		}

		public AutoConnectLevel AutoConnectLevel()
		{
			if (PlayerPreferences.autoLogin)
			{
				if (PlayerPreferences.useGuest && CredentialProvider.HasGuestAccount() && CredentialProvider.HasGuestMode())
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
			return CredentialProvider.HasGuestMode();
		}

		public LoginUIType LoginUIType()
		{
			return CredentialProvider.LoginUIType();
		}

		public bool HasGuestAccount()
		{
			return CredentialProvider.HasGuestAccount();
		}

		public void CleanCredentials()
		{
			PlayerPreferences.autoLogin = false;
			PlayerPreferences.lastPassword = "";
			PlayerPreferences.Save();
		}
	}
}
