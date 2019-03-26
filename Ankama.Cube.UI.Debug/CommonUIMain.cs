using Ankama.AssetManagement;
using Ankama.AssetManagement.AssetReferences;
using Ankama.Cube.Configuration;
using Ankama.Cube.Data.UI;
using Ankama.Utilities;
using System.Collections;
using UnityEngine;

namespace Ankama.Cube.UI.Debug
{
	public class CommonUIMain : MonoBehaviour
	{
		private void Awake()
		{
			if (!InitializeAssetManager())
			{
				Application.Quit();
			}
			RuntimeData.InitializeFonts();
			RuntimeData.InitializeLanguage(CultureCode.FR_FR);
			this.StartCoroutine(LoadScene());
		}

		private static bool InitializeAssetManager()
		{
			AssetManager.Initialize(true);
			AssetReferenceMap val = Resources.Load<AssetReferenceMap>("AssetReferenceMap");
			if (null == val)
			{
				Log.Error("[CRITICAL] Could not load AssetReferenceMap.", 31, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\Debug\\CommonUIMain.cs");
				return false;
			}
			AssetManager.SetAssetReferenceMap(val);
			Resources.UnloadAsset(val);
			return true;
		}

		private IEnumerator LoadScene()
		{
			yield return ApplicationStarter.ConfigureLocalAssetManager();
			StateManager.GetDefaultLayer().SetChildState(new CommonUIState(), 0);
		}

		public CommonUIMain()
			: this()
		{
		}
	}
}
