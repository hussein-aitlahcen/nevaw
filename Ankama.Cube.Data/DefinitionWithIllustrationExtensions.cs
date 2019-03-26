using Ankama.AssetManagement;
using Ankama.AssetManagement.AssetBundles;
using Ankama.AssetManagement.AssetReferences;
using Ankama.Utilities;
using DataEditor;
using System;
using System.Collections;
using UnityEngine;

namespace Ankama.Cube.Data
{
	public static class DefinitionWithIllustrationExtensions
	{
		public static IEnumerator LoadIllustrationAsync<T>(this EditableData definition, string bundleName, AssetReference assetReference, Action<T, string> onLoaded) where T : Object
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			AssetBundleLoadRequest bundleLoadRequest = AssetManager.LoadAssetBundle(bundleName);
			while (!bundleLoadRequest.get_isDone())
			{
				yield return null;
			}
			if (AssetManagerError.op_Implicit(bundleLoadRequest.get_error()) != 0)
			{
				Log.Error($"Error while loading bundle '{bundleName}' for {((object)definition).GetType().Name} {definition.get_name()} error={bundleLoadRequest.get_error()}", 36, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\Definitions\\ICastableDefinition.cs");
				onLoaded?.Invoke(default(T), null);
				yield break;
			}
			AssetLoadRequest<T> assetLoadRequest = assetReference.LoadFromAssetBundleAsync<T>(bundleName);
			while (!assetLoadRequest.get_isDone())
			{
				yield return null;
			}
			if (AssetManagerError.op_Implicit(assetLoadRequest.get_error()) != 0)
			{
				Log.Error($"Error while loading illustration for {((object)definition).GetType().Name} {definition.get_name()} error={assetLoadRequest.get_error()}", 50, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\Definitions\\ICastableDefinition.cs");
				onLoaded?.Invoke(default(T), bundleName);
			}
			else
			{
				onLoaded?.Invoke(assetLoadRequest.get_asset(), bundleName);
			}
		}

		public static void UnloadBundle(this EditableData definition, string bundleName)
		{
			AssetManager.UnloadAssetBundle(bundleName);
		}
	}
}
