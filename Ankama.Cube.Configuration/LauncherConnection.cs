using Ankama.Cube.Data.UI;
using Ankama.Launcher;
using Ankama.Launcher.Messages;
using Ankama.Launcher.Zaap;
using Ankama.Utilities;
using com.ankama.zaap;
using JetBrains.Annotations;
using System;
using System.Collections;

namespace Ankama.Cube.Configuration
{
	public static class LauncherConnection
	{
		private static ILauncherLink s_launcherLink;

		private static bool s_requestingLanguage;

		private static string s_language;

		public static ILauncherLink instance => s_launcherLink;

		[NotNull]
		public static string launcherLanguage => s_language ?? string.Empty;

		public static IEnumerator InitializeConnection()
		{
			RuntimeData.CultureCodeChanged += OnCultureCodeChanged;
			s_launcherLink = ZaapLink.Create();
			if (s_launcherLink == null)
			{
				Log.Warning("Unable to get Zaap connection, falling back to NoConnection.", 42, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Configuration\\LauncherConnection.cs");
				s_launcherLink = NoConnection.instance;
			}
			else
			{
				Log.Info("Connection to Zaap: OK", 47, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Configuration\\LauncherConnection.cs");
			}
			s_requestingLanguage = true;
			s_launcherLink.RequestLanguage((Action<string>)OnLauncherLanguage, (Action<Exception>)OnLauncherLanguageError);
			while (s_requestingLanguage)
			{
				yield return null;
			}
		}

		public static void Release()
		{
			RuntimeData.CultureCodeChanged -= OnCultureCodeChanged;
		}

		public static void RequestApiToken(Action<ApiToken> onApiToken, int serviceId)
		{
			s_launcherLink.RequestApiToken(serviceId, onApiToken, (Action<Exception>)delegate(Exception error)
			{
				Log.Error($"Error received while expecting apiToken: {error}", 70, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Configuration\\LauncherConnection.cs");
				onApiToken(null);
			});
		}

		private static void OnLauncherLanguage(string language)
		{
			s_language = "fr";
			s_requestingLanguage = false;
		}

		private static void OnLauncherLanguageError(Exception e)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			ZaapError val = e as ZaapError;
			Log.Warning((val == null) ? ("Unable to get language : " + (e.InnerException?.Message ?? e.Message)) : $"Unable to get language because of ZaapError code {val.get_Code()} : {val.get_Details()}", 87, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Configuration\\LauncherConnection.cs");
			OnLauncherLanguage(null);
		}

		private static void OnCultureCodeChanged(CultureCode cultureCode, FontLanguage fontLanguage)
		{
			string language = cultureCode.GetLanguage();
			if (!language.Equals(s_language))
			{
				s_language = language;
				s_launcherLink.UpdateLanguage(language, (Action<bool>)OnLanguageUpdate, (Action<Exception>)OnLanguageUpdateError);
			}
		}

		private static void OnLanguageUpdateError(Exception e)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			ZaapError val = e as ZaapError;
			Log.Warning((val == null) ? ("Unable to update language : " + (e.InnerException?.Message ?? e.Message)) : $"Unable to update language because of ZaapError code {val.get_Code()} : {val.get_Details()}", 115, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Configuration\\LauncherConnection.cs");
		}

		private static void OnLanguageUpdate(bool obj)
		{
		}
	}
}
