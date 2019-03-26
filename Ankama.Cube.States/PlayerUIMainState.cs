using Ankama.AssetManagement;
using Ankama.AssetManagement.AssetBundles;
using Ankama.AssetManagement.StateManagement;
using Ankama.Cube.Protocols.PlayerProtocol;
using Ankama.Cube.TEMPFastEnterMatch.Player;
using Ankama.Cube.UI.PlayerLayer;
using Ankama.Cube.Utility;
using Ankama.Utilities;
using System.Collections;

namespace Ankama.Cube.States
{
	public class PlayerUIMainState : LoadSceneStateContext
	{
		private PlayerIconRoot m_ui;

		private PlayerLayerFrame m_frame;

		protected override IEnumerator Load()
		{
			string bundleName = AssetBundlesUtility.GetUICharacterResourcesBundleName();
			AssetBundleLoadRequest bundleLoadRequest = AssetManager.LoadAssetBundle(bundleName);
			while (!bundleLoadRequest.get_isDone())
			{
				yield return null;
			}
			if (AssetManagerError.op_Implicit(bundleLoadRequest.get_error()) != 0)
			{
				Log.Error($"Error while loading bundle '{bundleName}' error={bundleLoadRequest.get_error()}", 26, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\States\\PlayerUI\\PlayerUIMainState.cs");
				yield break;
			}
			UILoader<PlayerIconRoot> loader = new UILoader<PlayerIconRoot>(this, "PlayerLayerUI", "core/scenes/ui/player", disableOnLoad: true);
			yield return loader.Load();
			m_ui = loader.ui;
			yield return m_ui.LoadAssets();
			m_ui.get_gameObject().SetActive(true);
			m_ui.Initialise(this);
			m_ui.LoadVisual();
		}

		protected override void Enable()
		{
			m_frame = new PlayerLayerFrame
			{
				onGodChangeResult = OnSelectedGodResult
			};
			PlayerData.instance.OnRequestVisualWeaponUpdated += OnCurrentDeckUpdate;
		}

		protected override void Disable()
		{
			m_ui.UnloadAsset();
			this.Disable();
			m_frame.Dispose();
			PlayerData.instance.OnRequestVisualWeaponUpdated -= OnCurrentDeckUpdate;
		}

		private void OnCurrentDeckUpdate(int weaponID)
		{
			m_ui.LoadVisual(weaponID);
		}

		private void OnSelectedGodResult(ChangeGodResultEvent evt)
		{
			m_ui.LoadVisual();
		}

		private void OnWeaponChangeResult()
		{
			m_ui.LoadVisual();
		}

		protected override IEnumerator Update()
		{
			yield return m_ui.PlayEnterAnimation();
		}

		public bool TryExpandPanel()
		{
			StateLayer val = default(StateLayer);
			if (StateManager.TryGetLayer("PlayerUI", ref val) && val.GetChainEnd() == this)
			{
				StateManager.SetActiveInputLayer(val);
				PlayerNavRibbonState playerNavRibbonState = new PlayerNavRibbonState();
				val.GetChainRoot().SetChildState(playerNavRibbonState, 0);
				return true;
			}
			return false;
		}
	}
}
