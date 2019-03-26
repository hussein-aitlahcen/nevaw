using Ankama.AssetManagement;
using Ankama.AssetManagement.AssetBundles;
using Ankama.AssetManagement.AssetReferences;
using Ankama.AssetManagement.StreamingAssets;
using Ankama.Cube.Data.UI;
using Ankama.Cube.Network;
using Ankama.Utilities;
using System.Collections;
using UnityEngine;

namespace Ankama.Cube.Configuration
{
	public static class ApplicationStarter
	{
		public static bool InitializeRuntimeData()
		{
			if (!RuntimeData.InitializeLanguage(GetCurrentCulture()))
			{
				Log.Error("[CRITICAL] Could not initialize language.", 23, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Configuration\\ApplicationStarter.cs");
				return false;
			}
			if (!RuntimeData.InitializeFonts())
			{
				Log.Error("[CRITICAL] Could not initialize fonts.", 29, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Configuration\\ApplicationStarter.cs");
				return false;
			}
			return true;
		}

		public static bool InitializeAssetManager()
		{
			AssetManager.Initialize(true);
			AssetReferenceMap val = Resources.Load<AssetReferenceMap>("AssetReferenceMap");
			if (null == val)
			{
				Log.Error("[CRITICAL] Could not load AssetReferenceMap.", 43, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Configuration\\ApplicationStarter.cs");
				return false;
			}
			AssetManager.SetAssetReferenceMap(val);
			Resources.UnloadAsset(val);
			return true;
		}

		public static IEnumerator ReadBootConfig()
		{
			StreamingAssetLoadRequest loadOperation = AssetManager.LoadStreamingAssetAsync("application.json", 4096);
			while (!loadOperation.get_isDone())
			{
				yield return null;
			}
			if (AssetManagerError.op_Implicit(loadOperation.get_error()) != 0)
			{
				Log.Error($"Error while reading application.json: {loadOperation.get_error()}", 64, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Configuration\\ApplicationStarter.cs");
			}
			else
			{
				BootConfig.Read(new ConfigReader(loadOperation.get_text(), loadOperation.assetPath));
			}
		}

		public static IEnumerator ReadRemoteConfig(string remoteSettingsFileUrl)
		{
			TextWebRequest.AsyncResult remoteSettingsFileLoadResult = new TextWebRequest.AsyncResult();
			yield return TextWebRequest.ReadFile(remoteSettingsFileUrl, remoteSettingsFileLoadResult);
			if (remoteSettingsFileLoadResult.hasException)
			{
				long responseCode = remoteSettingsFileLoadResult.exception.responseCode;
				if (responseCode == 404)
				{
					Log.Error("Could not find remote settings file at URL '" + remoteSettingsFileUrl + "'.", 83, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Configuration\\ApplicationStarter.cs");
				}
				else
				{
					Log.Error($"Error {responseCode} when trying to download remote settings file at URL '{remoteSettingsFileUrl}'.", 90, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Configuration\\ApplicationStarter.cs");
				}
			}
			else
			{
				ApplicationConfig.SetFromRemoteConfig(RemoteConfig.From(new ConfigReader(remoteSettingsFileLoadResult.value, remoteSettingsFileUrl)));
			}
		}

		public static IEnumerator ReadVersion(string versionFileUrl)
		{
			if (string.IsNullOrEmpty(versionFileUrl))
			{
				Log.Warning("No version file defined: force version valid", 106, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Configuration\\ApplicationStarter.cs");
				ApplicationConfig.versionCheckResult = VersionChecker.Result.Success;
				yield break;
			}
			TextWebRequest.AsyncResult versionFileLoadResult = new TextWebRequest.AsyncResult();
			yield return TextWebRequest.ReadFile(versionFileUrl, versionFileLoadResult);
			if (versionFileLoadResult.hasException)
			{
				long responseCode = versionFileLoadResult.exception.responseCode;
				if (responseCode == 404)
				{
					Log.Error("Could not find version file at URL '" + versionFileUrl + "'.", 121, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Configuration\\ApplicationStarter.cs");
				}
				else
				{
					Log.Error($"Error {responseCode} when trying to download version file at URL '{versionFileUrl}'.", 125, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Configuration\\ApplicationStarter.cs");
				}
				ApplicationConfig.versionCheckResult = VersionChecker.Result.VersionFileError;
			}
			else
			{
				ApplicationConfig.versionCheckResult = VersionChecker.ParseVersionFile(versionFileLoadResult.value);
			}
		}

		public static IEnumerator ConfigureAssetManager(string serverUrl, bool patchAvailable)
		{
			AssetManager.UnloadAssetBundleManifest();
			AssetManager.SetAssetBundlesConfiguration(serverUrl, true);
			AssetBundleSource val = (!patchAvailable) ? 1 : 0;
			AssetBundleManifestLoadRequest loadOperation = AssetManager.LoadAssetBundleManifest(val, true);
			while (!loadOperation.get_isDone())
			{
				yield return null;
			}
			if (AssetManagerError.op_Implicit(loadOperation.get_error()) != 0)
			{
				Log.Error($"Could not load manifests data: {loadOperation.get_error()}.", 159, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Configuration\\ApplicationStarter.cs");
			}
		}

		public static IEnumerator ConfigureLocalAssetManager()
		{
			AssetManager.UnloadAssetBundleManifest();
			AssetManager.SetAssetBundlesConfiguration(string.Empty, true);
			AssetBundleManifestLoadRequest loadOperation = AssetManager.LoadAssetBundleManifest(1, true);
			while (!loadOperation.get_isDone())
			{
				yield return null;
			}
			if (AssetManagerError.op_Implicit(loadOperation.get_error()) != 0)
			{
				Log.Error($"Could not load manifests data: {loadOperation.get_error()}.", 185, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Configuration\\ApplicationStarter.cs");
			}
		}

		public static IEnumerator CheckPatch()
		{
			foreach (CachedAssetBundle item in AssetManager.EnumerateCachedAssetBundles())
			{
				CachedAssetBundle current = item;
				if (!Caching.IsVersionCached(current))
				{
					string assetBundleName = current.get_name();
					AssetBundleLoadRequest loadRequest = AssetManager.LoadAssetBundle(assetBundleName);
					while (!loadRequest.get_isDone())
					{
						yield return null;
					}
					AssetBundleUnloadRequest unloadRequest = AssetManager.UnloadAssetBundle(assetBundleName);
					while (!unloadRequest.get_isDone())
					{
						yield return null;
					}
				}
			}
		}

		private static CultureCode GetCurrentCulture()
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Invalid comparison between Unknown and I4
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Invalid comparison between Unknown and I4
			if (TryGetCultureFromCode(LauncherConnection.launcherLanguage, out CultureCode cultureCode))
			{
				return cultureCode;
			}
			if (TryGetCultureFromCode(ApplicationConfig.langCode, out cultureCode))
			{
				return cultureCode;
			}
			SystemLanguage systemLanguage = Application.get_systemLanguage();
			if ((int)systemLanguage != 10)
			{
				if ((int)systemLanguage == 14)
				{
					return CultureCode.FR_FR;
				}
				return CultureCode.Fallback;
			}
			return CultureCode.EN_US;
		}

		private static bool TryGetCultureFromCode(string code, out CultureCode cultureCode)
		{
			if (!(code == "FR"))
			{
				if (!(code == "EN"))
				{
					if (code == "ES")
					{
						cultureCode = CultureCode.ES_ES;
						return true;
					}
					cultureCode = default(CultureCode);
					return false;
				}
				cultureCode = CultureCode.EN_US;
				return true;
			}
			cultureCode = CultureCode.FR_FR;
			return true;
		}
	}
}
