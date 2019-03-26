using Ankama.AssetManagement;
using Ankama.AssetManagement.AssetBundles;
using Ankama.AssetManagement.StateManagement;
using Ankama.Cube.UI;
using Ankama.Cube.Utility;
using Ankama.Utilities;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ankama.Cube.States
{
	public class LoadSceneStateContext : StateContext
	{
		public class UILoader<S> where S : AbstractUI
		{
			private LoadSceneStateContext m_stateLoader;

			private StateContext m_stateHoster;

			private string m_bundleName;

			private string m_sceneName;

			private Scene m_uiScene;

			private S m_ui;

			private bool m_disableOnLoad;

			public S ui => m_ui;

			public Scene scene => m_uiScene;

			public UILoader(LoadSceneStateContext uiloader, string sceneName, string uiBundleName, bool disableOnLoad = false)
				: this(uiloader, uiloader, sceneName, uiBundleName, disableOnLoad)
			{
			}

			public UILoader(LoadSceneStateContext uiloader, StateContext uiHoster, string sceneName, string uiBundleName, bool disableOnLoad = false)
			{
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				m_stateLoader = uiloader;
				m_stateHoster = uiHoster;
				m_bundleName = uiBundleName;
				m_sceneName = sceneName;
				m_uiScene = default(Scene);
				m_ui = null;
				m_disableOnLoad = disableOnLoad;
			}

			public IEnumerator Load()
			{
				yield return m_stateLoader.LoadSceneAndBundleRequest(m_sceneName, m_bundleName, 1, OnSceneLoadRequestComplete);
			}

			private void OnSceneLoadRequestComplete(SceneLoadRequest sceneLoadRequest)
			{
				if (!TryGetSceneAndRoot(sceneLoadRequest.sceneName, out m_uiScene, out m_ui))
				{
					Log.Error("Something went wrong while loading scene named '" + sceneLoadRequest.sceneName + "'.", 78, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\States\\LoadSceneStateContext.cs");
				}
				if (m_disableOnLoad)
				{
					m_ui.get_gameObject().SetActive(false);
				}
				m_ui.SetDepth(m_stateHoster);
			}
		}

		protected IEnumerator LoadBundleRequest(string bundleName)
		{
			AssetBundleLoadRequest bundleLoadRequest = this.LoadAssetBundle(bundleName);
			while (!bundleLoadRequest.get_isDone())
			{
				yield return null;
			}
			if (AssetManagerError.op_Implicit(bundleLoadRequest.get_error()) != 0)
			{
				AssetManagerError error = bundleLoadRequest.get_error();
				Log.Error(((object)error).ToString(), 97, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\States\\LoadSceneStateContext.cs");
			}
		}

		public IEnumerator LoadSceneAndBundleRequest(string sceneName, string bundleName, LoadSceneMode mode = 1, Action<SceneLoadRequest> completed = null)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			AssetBundleLoadRequest bundleLoadRequest = this.LoadAssetBundle(bundleName);
			while (!bundleLoadRequest.get_isDone())
			{
				yield return null;
			}
			AssetManagerError error;
			if (AssetManagerError.op_Implicit(bundleLoadRequest.get_error()) != 0)
			{
				error = bundleLoadRequest.get_error();
				Log.Error(((object)error).ToString(), 112, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\States\\LoadSceneStateContext.cs");
				yield break;
			}
			LoadSceneParameters val = default(LoadSceneParameters);
			val._002Ector(mode);
			SceneLoadRequest sceneLoadRequest = this.LoadScene(sceneName, bundleName, val, true, completed);
			while (!sceneLoadRequest.get_isDone())
			{
				yield return null;
			}
			if (AssetManagerError.op_Implicit(sceneLoadRequest.get_error()) != 0)
			{
				error = sceneLoadRequest.get_error();
				Log.Error(((object)error).ToString(), 126, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\States\\LoadSceneStateContext.cs");
			}
		}

		protected IEnumerator UnloadSceneRequest(Scene scene)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			if (scene.IsValid())
			{
				yield return SceneManager.UnloadSceneAsync(scene);
			}
		}

		protected static bool TryGetSceneAndRoot<T>(string sceneName, out Scene scene, out T root) where T : MonoBehaviour
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			scene = SceneManager.GetSceneByName(sceneName);
			if (!scene.IsValid())
			{
				Log.Error("Invalid scene '" + sceneName + "'.", 144, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\States\\LoadSceneStateContext.cs");
				root = default(T);
				return false;
			}
			root = ScenesUtility.GetSceneRoot<T>(scene);
			return null != (object)root;
		}

		public LoadSceneStateContext()
			: this()
		{
		}
	}
}
