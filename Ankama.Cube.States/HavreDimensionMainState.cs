using Ankama.Animations;
using Ankama.AssetManagement;
using Ankama.AssetManagement.AssetBundles;
using Ankama.AssetManagement.AssetReferences;
using Ankama.AssetManagement.StateManagement;
using Ankama.Cube.Data;
using Ankama.Cube.Maps;
using Ankama.Cube.TEMPFastEnterMatch.Player;
using Ankama.Cube.UI.Zaap;
using Ankama.Cube.Utility;
using Ankama.Utilities;
using DataEditor;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ankama.Cube.States
{
	public class HavreDimensionMainState : LoadSceneStateContext
	{
		private HavreMap m_havreMap;

		private string m_lastLoadedCharacterBundle;

		private Coroutine m_loadCharacterCoroutine;

		private string m_lastLoadedGodBundle;

		private Coroutine m_loadGodCoroutine;

		protected override IEnumerator Load()
		{
			string sceneName = ScenesUtility.GetHavreMapSceneName(PlayerData.instance.havreMapSceneIndex);
			yield return LoadSceneAndBundleRequest(sceneName, "core/scenes/maps/havre_maps");
			if (!LoadSceneStateContext.TryGetSceneAndRoot(sceneName, out Scene _, out m_havreMap))
			{
				Log.Error("could not find Havre Map", 37, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\States\\HavreDimension\\HavreDimensionMainState.cs");
				yield break;
			}
			yield return m_havreMap.Initialize();
			yield return LoadCharacterSkin();
			yield return LoadGod();
			m_havreMap.InitEnterAnimFirstFrame();
		}

		protected override IEnumerator Update()
		{
			yield return m_havreMap.PlayEnerAnim();
		}

		protected override void Enable()
		{
			this.Enable();
			m_havreMap.onPvPTrigger = OnPvPTrigger;
			m_havreMap.onGodTrigger = OnGodTrigger;
			PlayerData.instance.OnPlayerHeroSkinChanged += UpdateSkin;
			PlayerData.instance.OnPlayerGodChanged += UpdateGod;
			m_havreMap.Begin();
		}

		protected override void Disable()
		{
			this.Disable();
			m_havreMap.onPvPTrigger = null;
			m_havreMap.onGodTrigger = null;
			PlayerData.instance.OnPlayerHeroSkinChanged -= UpdateSkin;
			PlayerData.instance.OnPlayerGodChanged -= UpdateGod;
		}

		protected override IEnumerator Unload()
		{
			if (m_loadGodCoroutine != null)
			{
				Main.monoBehaviour.StopCoroutine(m_loadGodCoroutine);
			}
			if (m_loadCharacterCoroutine != null)
			{
				Main.monoBehaviour.StopCoroutine(m_loadCharacterCoroutine);
			}
			m_havreMap.Release();
			yield return _003C_003En__0();
		}

		private void OnPvPTrigger()
		{
			UIZaapState uIZaapState = new UIZaapState();
			UIZaapState uIZaapState2 = uIZaapState;
			uIZaapState2.onDisable = (Action)Delegate.Combine(uIZaapState2.onDisable, new Action(OnOpenStateDisable));
			UIZaapState uIZaapState3 = uIZaapState;
			uIZaapState3.onTransition = (Action)Delegate.Combine(uIZaapState3.onTransition, new Action(OnOpenStateTransition));
			OpenState(uIZaapState);
		}

		private void OnGodTrigger()
		{
			GodSelectionState godSelectionState = new GodSelectionState();
			GodSelectionState godSelectionState2 = godSelectionState;
			godSelectionState2.onDisable = (Action)Delegate.Combine(godSelectionState2.onDisable, new Action(OnOpenStateDisable));
			GodSelectionState godSelectionState3 = godSelectionState;
			godSelectionState3.onTransition = (Action)Delegate.Combine(godSelectionState3.onTransition, new Action(OnOpenStateTransition));
			OpenState(godSelectionState);
		}

		private void OpenState(StateContext state)
		{
			m_havreMap.SetInteractable(value: false);
			StateLayer val = default(StateLayer);
			StateLayer val2 = default(StateLayer);
			if (StateManager.TryGetLayer("OptionUI", ref val) && val.HasChildState() && val.GetChildState().HasChildState())
			{
				m_havreMap.SetInteractable(value: true);
			}
			else if (StateManager.TryGetLayer("PlayerUI", ref val2))
			{
				if (val2.HasChildState() && val2.GetChildState().HasChildState())
				{
					m_havreMap.SetInteractable(value: true);
					return;
				}
				StateManager.SetActiveInputLayer(val2);
				val2.GetChainEnd().SetChildState(state, 0);
			}
		}

		private void OnOpenStateDisable()
		{
			m_havreMap.SetInteractable(value: true);
		}

		private void OnOpenStateTransition()
		{
			m_havreMap.MoveCharacterOutsideZaap();
		}

		private void UpdateGod()
		{
			if (m_loadGodCoroutine != null)
			{
				Main.monoBehaviour.StopCoroutine(m_loadGodCoroutine);
			}
			m_loadGodCoroutine = Main.monoBehaviour.StartCoroutine(LoadGod());
		}

		private void UpdateSkin()
		{
			if (m_loadCharacterCoroutine != null)
			{
				Main.monoBehaviour.StopCoroutine(m_loadCharacterCoroutine);
			}
			m_loadCharacterCoroutine = Main.monoBehaviour.StartCoroutine(LoadCharacterSkin());
		}

		private IEnumerator LoadGod()
		{
			if (!string.IsNullOrEmpty(m_lastLoadedGodBundle))
			{
				this.UnloadAssetBundle(m_lastLoadedGodBundle, true, true);
				m_lastLoadedGodBundle = null;
			}
			God god = PlayerData.instance.god;
			if (!RuntimeData.godDefinitions.TryGetValue(god, out GodDefinition value))
			{
				yield break;
			}
			AssetReference statuePrefabReference = value.statuePrefabReference;
			string bundleName = m_lastLoadedGodBundle = AssetBundlesUtility.GetUIGodsResourcesBundleName();
			AssetBundleLoadRequest bundleRequest = this.LoadAssetBundle(bundleName);
			while (!bundleRequest.get_isDone())
			{
				yield return null;
			}
			if (AssetManagerError.op_Implicit(bundleRequest.get_error()) != 0)
			{
				Log.Error($"Could not load bundle named '{bundleName}': {bundleRequest.get_error()}", 196, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\States\\HavreDimension\\HavreDimensionMainState.cs");
				yield break;
			}
			AssetLoadRequest<GameObject> assetReferenceRequest = statuePrefabReference.LoadFromAssetBundleAsync<GameObject>(bundleName);
			while (!assetReferenceRequest.get_isDone())
			{
				yield return null;
			}
			if (AssetManagerError.op_Implicit(assetReferenceRequest.get_error()) != 0)
			{
				Log.Error($"Could not load requested asset ({statuePrefabReference.get_value()}) from bundle named '{bundleName}': {assetReferenceRequest.get_error()}", 208, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\States\\HavreDimension\\HavreDimensionMainState.cs");
				yield break;
			}
			GameObject asset = assetReferenceRequest.get_asset();
			m_havreMap.godZaap.SetStatue(asset);
			m_loadGodCoroutine = null;
		}

		private IEnumerator LoadCharacterSkin()
		{
			if (!string.IsNullOrEmpty(m_lastLoadedCharacterBundle))
			{
				this.UnloadAssetBundle(m_lastLoadedCharacterBundle, true, true);
				m_lastLoadedCharacterBundle = null;
			}
			PlayerData instance = PlayerData.instance;
			Gender gender = instance.gender;
			Id<CharacterSkinDefinition> skin = instance.Skin;
			if (skin == null || !RuntimeData.characterSkinDefinitions.TryGetValue(skin.value, out CharacterSkinDefinition characterSkinDefinition))
			{
				yield break;
			}
			BundleCategory bundleCategory = characterSkinDefinition.bundleCategory;
			string bundleName = m_lastLoadedCharacterBundle = AssetBundlesUtility.GetAnimatedCharacterDataBundle(bundleCategory);
			AssetBundleLoadRequest bundleLoadRequest = this.LoadAssetBundle(bundleName);
			while (!bundleLoadRequest.get_isDone())
			{
				yield return null;
			}
			if (AssetManagerError.op_Implicit(bundleLoadRequest.get_error()) != 0)
			{
				Log.Error($"Failed to load asset bundle named '{bundleName}' for character skin {characterSkinDefinition.get_displayName()} ({characterSkinDefinition.get_id()}): {bundleLoadRequest.get_error()}", 253, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\States\\HavreDimension\\HavreDimensionMainState.cs");
				yield break;
			}
			AssetReference animatedCharacterDataReference = characterSkinDefinition.GetAnimatedCharacterDataReference(gender);
			AssetLoadRequest<AnimatedCharacterData> animatedCharacterDataLoadRequest = animatedCharacterDataReference.LoadFromAssetBundleAsync<AnimatedCharacterData>(bundleName);
			while (!animatedCharacterDataLoadRequest.get_isDone())
			{
				yield return null;
			}
			if (AssetManagerError.op_Implicit(animatedCharacterDataLoadRequest.get_error()) != 0)
			{
				this.UnloadAssetBundle(bundleName, true, true);
				Log.Error(string.Format("Failed to load {0} asset from bundle '{1}' for character skin {2} ({3}): {4}", "AnimatedCharacterData", bundleName, characterSkinDefinition.get_displayName(), characterSkinDefinition.get_id(), animatedCharacterDataLoadRequest.get_error()), 269, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\States\\HavreDimension\\HavreDimensionMainState.cs");
				yield break;
			}
			AnimatedCharacterData animatedCharacterData = animatedCharacterDataLoadRequest.get_asset();
			yield return animatedCharacterData.LoadTimelineResources();
			AnimatedFightCharacterData animatedFightCharacterData = animatedCharacterData as AnimatedFightCharacterData;
			AnimatedObjectDefinition animatedObjectDefinition = animatedFightCharacterData.animatedObjectDefinition;
			m_havreMap.character.SetCharacterData(animatedFightCharacterData, animatedObjectDefinition);
			m_loadCharacterCoroutine = null;
		}
	}
}
