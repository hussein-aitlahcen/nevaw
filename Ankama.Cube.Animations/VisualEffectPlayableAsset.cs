using Ankama.AssetManagement;
using Ankama.AssetManagement.AssetBundles;
using Ankama.AssetManagement.AssetReferences;
using Ankama.Cube.Maps.VisualEffects;
using Ankama.Utilities;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Ankama.Cube.Animations
{
	[Serializable]
	public sealed class VisualEffectPlayableAsset : PlayableAsset, ITimelineClipAsset, ITimelineResourcesProvider
	{
		public enum StopMode
		{
			None,
			Stop,
			Destroy
		}

		public enum ParentingMode
		{
			Owner,
			Parent,
			ContextOwner,
			ContextParent,
			World
		}

		public enum OrientationMethod
		{
			None,
			Context,
			Director,
			Transform
		}

		[SerializeField]
		private AssetReference m_prefabReference;

		[SerializeField]
		private string m_assetBundleName;

		[SerializeField]
		private StopMode m_stopMode = StopMode.Stop;

		[SerializeField]
		private ParentingMode m_parentingMode = ParentingMode.Parent;

		[SerializeField]
		private OrientationMethod m_orientationMethod;

		[SerializeField]
		private Vector3 m_offset = Vector3.get_zero();

		[NonSerialized]
		private bool m_loadedAssetBundle;

		[NonSerialized]
		private VisualEffect m_prefab;

		public ClipCaps clipCaps
		{
			get;
		}

		public IEnumerator LoadResources()
		{
			if (!m_prefabReference.get_hasValue())
			{
				yield break;
			}
			GameObject asset;
			if (string.IsNullOrEmpty(m_assetBundleName))
			{
				AssetReferenceRequest<GameObject> assetReferenceRequest2 = m_prefabReference.LoadFromResourcesAsync<GameObject>();
				while (!assetReferenceRequest2.get_isDone())
				{
					yield return null;
				}
				asset = assetReferenceRequest2.get_asset();
			}
			else
			{
				AssetBundleLoadRequest bundleRequest = AssetManager.LoadAssetBundle(m_assetBundleName);
				while (!bundleRequest.get_isDone())
				{
					yield return null;
				}
				if (AssetManagerError.op_Implicit(bundleRequest.get_error()) != 0)
				{
					Log.Error($"Could not load bundle named '{m_assetBundleName}': {bundleRequest.get_error()}", 99, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Animations\\VisualEffectPlayableAsset.cs");
					yield break;
				}
				m_loadedAssetBundle = true;
				AssetLoadRequest<GameObject> assetReferenceRequest = m_prefabReference.LoadFromAssetBundleAsync<GameObject>(m_assetBundleName);
				while (!assetReferenceRequest.get_isDone())
				{
					yield return null;
				}
				if (AssetManagerError.op_Implicit(assetReferenceRequest.get_error()) != 0)
				{
					Log.Error($"Could not load requested asset ({m_prefabReference.get_value()}) from bundle named '{m_assetBundleName}': {assetReferenceRequest.get_error()}", 113, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Animations\\VisualEffectPlayableAsset.cs");
					yield break;
				}
				asset = assetReferenceRequest.get_asset();
			}
			VisualEffect component = asset.GetComponent<VisualEffect>();
			if (null == component)
			{
				Log.Error("Could not use prefab because it doesn't have a VisualEffect component.", 123, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Animations\\VisualEffectPlayableAsset.cs");
				yield break;
			}
			VisualEffectFactory.PreparePool(asset);
			m_prefab = component;
		}

		public void UnloadResources()
		{
			if (m_loadedAssetBundle)
			{
				AssetManager.UnloadAssetBundle(m_assetBundleName);
				m_loadedAssetBundle = false;
			}
			m_prefab = null;
		}

		public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			if (!m_prefabReference.get_hasValue())
			{
				return Playable.get_Null();
			}
			VisualEffectContext context = TimelineContextUtility.GetContext<VisualEffectContext>(graph);
			VisualEffectPlayableBehaviour visualEffectPlayableBehaviour = new VisualEffectPlayableBehaviour(owner, context, m_prefab, m_stopMode, m_parentingMode, m_orientationMethod, m_offset);
			return ScriptPlayable<VisualEffectPlayableBehaviour>.op_Implicit(ScriptPlayable<VisualEffectPlayableBehaviour>.Create(graph, visualEffectPlayableBehaviour, 0));
		}

		public VisualEffectPlayableAsset()
			: this()
		{
		}//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)

	}
}
