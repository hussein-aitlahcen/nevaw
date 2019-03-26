using Ankama.Utilities;
using Com.Ankama.Haapi.Swagger.Api;
using IO.Swagger.Client;
using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace Ankama.Cube.Configuration
{
	public class HaapiManager : MonoBehaviour
	{
		public static readonly AccountApi accountApi = new AccountApi(null);

		private static HaapiManager s_instance;

		private static bool s_initialized;

		private static readonly RemoteCertificateValidationCallback DefaultServerCertificateValidationCallback = ServicePointManager.ServerCertificateValidationCallback;

		private static readonly RemoteCertificateValidationCallback AlwaysValidateCertificateCallback = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true;

		public static void Initialize()
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Expected O, but got Unknown
			if (s_instance == null)
			{
				Log.Info("Adding HaapiManager to scene.", 26, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Configuration\\Haapi\\HaapiManager.cs");
				s_instance = new GameObject("HaapiManager").AddComponent<HaapiManager>();
				Object.DontDestroyOnLoad(s_instance);
			}
			string text = ApplicationConfig.haapiServerUrl;
			if (string.IsNullOrWhiteSpace(text))
			{
				text = "https://haapi.ankama.lan/json/Ankama/v2";
			}
			Configuration.DefaultApiClient = new ApiClient(text);
			ServicePointManager.ServerCertificateValidationCallback = (NeedCertificateValidation() ? DefaultServerCertificateValidationCallback : AlwaysValidateCertificateCallback);
			accountApi.set_ApiClient(Configuration.DefaultApiClient);
			s_initialized = true;
		}

		private static bool NeedCertificateValidation()
		{
			string basePath = Configuration.DefaultApiClient.get_BasePath();
			return !StringExtensions.StartsWithFast(basePath, "https://haapi.ankama.tst") && !StringExtensions.StartsWithFast(basePath, "https://haapi.ankama.lan");
		}

		public static void ExecuteRequest<T>(Func<T> func, Action<T> onResult, Action<Exception> onException)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			CheckInit();
			GameObject val = new GameObject();
			val.get_transform().SetParent(s_instance.get_transform());
			val.AddComponent<HaapiRequestBehaviour>().ExecuteRequest(func, onResult, onException);
		}

		private static void CheckInit()
		{
			if (s_initialized)
			{
				return;
			}
			Log.Error("HaapiManager.Initialize should have been called.", 71, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Configuration\\Haapi\\HaapiManager.cs");
			throw new Exception("HaapiManager.Initialize should have been called.");
		}

		public HaapiManager()
			: this()
		{
		}
	}
}
