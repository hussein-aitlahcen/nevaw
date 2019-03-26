using Ankama.AssetManagement;
using Ankama.AssetManagement.AssetBundles;
using Ankama.AssetManagement.AssetReferences;
using Ankama.Utilities;
using JetBrains.Annotations;
using System;
using System.Collections;
using UnityEngine;

namespace Ankama.Cube.UI.Components
{
	public abstract class UIResourceLoader<T> : MonoBehaviour, IUIResourceProvider where T : Object
	{
		private IUIResourceConsumer m_resourceConsumer;

		private Coroutine m_coroutine;

		private string m_pendingBundleName = string.Empty;

		private string m_loadedBundleName = string.Empty;

		[PublicAPI]
		public UIResourceLoadState loadState
		{
			get;
			protected set;
		}

		private void OnDestroy()
		{
			if (m_coroutine != null)
			{
				MonoBehaviour monoBehaviour = Main.monoBehaviour;
				if (null != monoBehaviour)
				{
					monoBehaviour.StopCoroutine(m_coroutine);
					m_coroutine = null;
				}
			}
			switch (loadState)
			{
			case UIResourceLoadState.Loading:
			case UIResourceLoadState.Loaded:
				if (m_pendingBundleName.Length != 0)
				{
					AssetManager.UnloadAssetBundle(m_pendingBundleName);
					m_pendingBundleName = string.Empty;
				}
				if (m_loadedBundleName.Length != 0)
				{
					AssetManager.UnloadAssetBundle(m_loadedBundleName);
					m_loadedBundleName = string.Empty;
				}
				if (m_resourceConsumer != null)
				{
					m_resourceConsumer.UnRegister(this);
					m_resourceConsumer = null;
				}
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case UIResourceLoadState.None:
			case UIResourceLoadState.Error:
				break;
			}
			loadState = UIResourceLoadState.None;
		}

		[PublicAPI]
		public void Setup(AssetReference assetReference, [NotNull] string bundleName, [CanBeNull] IUIResourceConsumer resourceConsumer = null)
		{
			MonoBehaviour monoBehaviour = Main.monoBehaviour;
			if (null == monoBehaviour)
			{
				throw new Exception();
			}
			if (m_coroutine != null)
			{
				monoBehaviour.StopCoroutine(m_coroutine);
				m_coroutine = null;
			}
			if (loadState == UIResourceLoadState.Loading && m_pendingBundleName.Length != 0)
			{
				AssetManager.UnloadAssetBundle(m_pendingBundleName);
				m_pendingBundleName = string.Empty;
			}
			if (m_resourceConsumer != null)
			{
				m_resourceConsumer.UnRegister(this);
			}
			UIResourceDisplayMode displayMode = resourceConsumer?.Register(this) ?? UIResourceDisplayMode.None;
			m_resourceConsumer = resourceConsumer;
			if (!assetReference.get_hasValue())
			{
				m_coroutine = MonoBehaviourExtensions.StartCoroutineImmediate(monoBehaviour, Clear(displayMode));
				return;
			}
			if (string.IsNullOrEmpty(bundleName))
			{
				throw new ArgumentException("The bundle name must be a non-null non-empty string.", "bundleName");
			}
			m_pendingBundleName = bundleName;
			m_coroutine = MonoBehaviourExtensions.StartCoroutineImmediate(monoBehaviour, Load(assetReference.get_value(), bundleName, displayMode));
		}

		[PublicAPI]
		public void Clear([CanBeNull] IUIResourceConsumer resourceConsumer = null)
		{
			MonoBehaviour monoBehaviour = Main.monoBehaviour;
			if (null == monoBehaviour)
			{
				throw new Exception();
			}
			if (m_coroutine != null)
			{
				monoBehaviour.StopCoroutine(m_coroutine);
				m_coroutine = null;
			}
			if (loadState == UIResourceLoadState.Loading && m_pendingBundleName.Length != 0)
			{
				AssetManager.UnloadAssetBundle(m_pendingBundleName);
				m_pendingBundleName = string.Empty;
			}
			if (m_resourceConsumer != null)
			{
				m_resourceConsumer.UnRegister(this);
			}
			UIResourceDisplayMode displayMode = resourceConsumer?.Register(this) ?? UIResourceDisplayMode.None;
			m_resourceConsumer = resourceConsumer;
			m_coroutine = MonoBehaviourExtensions.StartCoroutineImmediate(monoBehaviour, Clear(displayMode));
		}

		private IEnumerator Load(string guid, string bundleName, UIResourceDisplayMode displayMode)
		{
			loadState = UIResourceLoadState.Loading;
			AssetBundleLoadRequest bundleLoadRequest = AssetManager.LoadAssetBundle(bundleName);
			while (!bundleLoadRequest.get_isDone())
			{
				yield return null;
			}
			if (AssetManagerError.op_Implicit(bundleLoadRequest.get_error()) != 0)
			{
				loadState = UIResourceLoadState.Error;
				m_pendingBundleName = string.Empty;
				if (m_resourceConsumer != null)
				{
					m_resourceConsumer.UnRegister(this);
					m_resourceConsumer = null;
				}
				Log.Error($"Could not load bundle named '{bundleName}': {bundleLoadRequest.get_error()}", 226, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\Components\\UIResourceLoader.cs");
				m_coroutine = null;
				yield break;
			}
			AssetLoadRequest<T> assetLoadRequest = AssetManager.LoadAssetAsync<T>(guid, bundleName);
			while (!assetLoadRequest.get_isDone())
			{
				yield return null;
			}
			if (AssetManagerError.op_Implicit(assetLoadRequest.get_error()) != 0)
			{
				AssetManager.UnloadAssetBundle(bundleName);
				loadState = UIResourceLoadState.Error;
				m_pendingBundleName = string.Empty;
				if (m_resourceConsumer != null)
				{
					m_resourceConsumer.UnRegister(this);
					m_resourceConsumer = null;
				}
				Log.Error($"Could not load asset with guid {guid} from bundle named '{bundleName}': {assetLoadRequest.get_error()}", 253, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\Components\\UIResourceLoader.cs");
				m_coroutine = null;
				yield break;
			}
			T asset = assetLoadRequest.get_asset();
			yield return Apply(asset, displayMode);
			if (m_loadedBundleName.Length != 0)
			{
				AssetManager.UnloadAssetBundle(m_loadedBundleName);
			}
			loadState = UIResourceLoadState.Loaded;
			m_pendingBundleName = string.Empty;
			m_loadedBundleName = bundleName;
			if (m_resourceConsumer != null)
			{
				m_resourceConsumer.UnRegister(this);
				m_resourceConsumer = null;
			}
			m_coroutine = null;
		}

		private IEnumerator Clear(UIResourceDisplayMode displayMode)
		{
			yield return Revert(displayMode);
			if (m_loadedBundleName.Length != 0)
			{
				AssetManager.UnloadAssetBundle(m_loadedBundleName);
				m_loadedBundleName = string.Empty;
			}
			loadState = UIResourceLoadState.None;
		}

		protected abstract IEnumerator Apply([NotNull] T asset, UIResourceDisplayMode displayMode);

		protected abstract IEnumerator Revert(UIResourceDisplayMode displayMode);

		protected UIResourceLoader()
			: this()
		{
		}
	}
}
