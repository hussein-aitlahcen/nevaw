using Ankama.AssetManagement;
using Ankama.AssetManagement.AssetBundles;
using Ankama.AssetManagement.StateManagement;
using Ankama.Cube.Data;
using Ankama.Cube.Network;
using Ankama.Cube.Protocols.PlayerProtocol;
using Ankama.Cube.TEMPFastEnterMatch.Player;
using Ankama.Cube.UI;
using Ankama.Cube.UI.DeckMaker;
using Ankama.Cube.UI.PlayerLayer;
using Ankama.Cube.Utility;
using Ankama.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.States
{
	public class DeckMainState : LoadSceneStateContext, IStateUIChildPriority
	{
		private DeckMakerFrame m_frame;

		private bool m_isBeingSave;

		private DeckUIRoot m_ui;

		private DeckEditState m_subState;

		private WeaponAndDeckModifications m_modifications;

		private bool m_resultReceived;

		public Action OnSelectedWeaponChanges;

		public UIPriority uiChildPriority => UIPriority.Front;

		protected override IEnumerator Load()
		{
			m_modifications = new WeaponAndDeckModifications();
			m_modifications.Setup();
			this.LoadAssetBundle(AssetBundlesUtility.GetUIAnimatedCharacterResourcesBundleName());
			this.LoadAssetBundle("core/ui/characters/companions");
			string bundleName = AssetBundlesUtility.GetUICharacterResourcesBundleName();
			AssetBundleLoadRequest bundleLoadRequest = AssetManager.LoadAssetBundle(bundleName);
			while (!bundleLoadRequest.get_isDone())
			{
				yield return null;
			}
			if (AssetManagerError.op_Implicit(bundleLoadRequest.get_error()) != 0)
			{
				Log.Error($"Error while loading bundle '{bundleName}' error={bundleLoadRequest.get_error()}", 48, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\States\\PlayerUI\\DeckMainState.cs");
				yield break;
			}
			UILoader<DeckUIRoot> loader = new UILoader<DeckUIRoot>(this, "PlayerLayer_DeckCanvas", "core/scenes/ui/deck", disableOnLoad: true);
			yield return loader.Load();
			m_ui = loader.ui;
			yield return m_ui.LoadAssets();
			m_ui.get_gameObject().SetActive(true);
			m_ui.Initialise(m_modifications);
		}

		public void RegisterToWeaponChange(Action selectionChange)
		{
			OnSelectedWeaponChanges = selectionChange;
		}

		protected override IEnumerator Update()
		{
			yield return m_ui.PlayEnterAnimation(GetWeaponsList());
			StateLayer val = default(StateLayer);
			if (StateManager.TryGetLayer("PlayerUI", ref val))
			{
				m_subState = new DeckEditState();
				m_subState.OnCloseComplete += BackFromEditionState;
				val.GetChainEnd().SetChildState(m_subState, 0);
			}
		}

		public override bool AllowsTransition(StateContext nextState)
		{
			return true;
		}

		protected override IEnumerator Transition(StateTransitionInfo transitionInfo)
		{
			m_modifications.Unregister();
			float timeOutStart = Time.get_realtimeSinceStartup();
			if (m_modifications.hasModifications)
			{
				m_resultReceived = false;
				m_frame.SendSelectDecksAndWeapon(m_modifications.selectedWeapon, m_modifications.selectedDecksPerWeapon);
				while (!m_resultReceived && Time.get_realtimeSinceStartup() - timeOutStart < 1f)
				{
					yield return null;
				}
			}
			yield return m_ui.CloseUI();
			m_ui.UnloadAsset();
		}

		private void OnSelectDeckAndWeaponResult(SelectDeckAndWeaponResultEvent result)
		{
			m_resultReceived = true;
		}

		protected override void Enable()
		{
			m_frame = new DeckMakerFrame
			{
				onSelectDeckAndWeaponResult = OnSelectDeckAndWeaponResult
			};
			m_ui.OnEditRequest += OnEditDeckRequest;
			m_ui.OnGotoEditAnimComplete += OnGotoEditAnimComplete;
			m_ui.OnEquipWeaponRequest += EquipWeapon;
			m_ui.OnSelectDeckForWeaponRequest += SelectDeckForWeaponDeckForWeapon;
			m_modifications.OnSelectedDecksUpdated += OnEquippedDecksUpdated;
			m_modifications.OnSelectedWeaponUpdated += OnSelectedWeaponUpdated;
		}

		private void OnEditDeckRequest(DeckSlot obj)
		{
			m_subState.SetDeckSlot(obj, m_modifications);
			m_ui.GotoEditAnim();
		}

		private void BackFromEditionState()
		{
			m_ui.BackFromEditAnim();
		}

		private void OnGotoEditAnimComplete()
		{
			m_subState.OpenUIAnimation();
		}

		private List<WeaponDefinition> GetWeaponsList()
		{
			List<WeaponDefinition> list = new List<WeaponDefinition>();
			foreach (KeyValuePair<int, WeaponDefinition> weaponDefinition in RuntimeData.weaponDefinitions)
			{
				WeaponDefinition value = weaponDefinition.Value;
				if (value.god == PlayerData.instance.god)
				{
					list.Add(value);
				}
			}
			return list;
		}

		protected override void Disable()
		{
			m_frame.Dispose();
		}

		private void EquipWeapon(int weaponId)
		{
			m_modifications.SetSelectedWeapon(weaponId);
			PlayerData.instance.RequestWeaponChange(weaponId);
		}

		private void SelectDeckForWeaponDeckForWeapon(DeckSlot slot)
		{
			DeckInfo deckInfo = slot.DeckInfo;
			if (deckInfo != null)
			{
				deckInfo = deckInfo.TrimCopy();
				int? id = deckInfo.Id;
				if (id.HasValue && deckInfo.IsValid())
				{
					m_modifications.SetSelectedDeckForWeapon(deckInfo.Weapon, id.Value);
				}
			}
		}

		private void OnEquippedDecksUpdated()
		{
			m_ui.OnEquippedDeckUpdate();
		}

		private void OnSelectedWeaponUpdated()
		{
			m_ui.OnSelectedWeaponUpdate();
			OnSelectedWeaponChanges?.Invoke();
			PlayerData.instance.RequestWeaponChange(m_ui.GetCurrentWeaponID());
		}
	}
}
