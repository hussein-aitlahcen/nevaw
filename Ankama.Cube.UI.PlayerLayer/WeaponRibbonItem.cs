using Ankama.AssetManagement;
using Ankama.AssetManagement.AssetBundles;
using Ankama.AssetManagement.AssetReferences;
using Ankama.Cube.Data;
using Ankama.Cube.Utility;
using Ankama.Utilities;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.UI.PlayerLayer
{
	public class WeaponRibbonItem : BaseRibbonItem
	{
		private DeckUIRoot m_deckMain;

		private WeaponDefinition m_definition;

		private Material m_equipedMaterial;

		[SerializeField]
		private Image m_shine;

		public void Initialise(DeckUIRoot uiRoot, WeaponDefinition definition)
		{
			m_deckMain = uiRoot;
			m_definition = definition;
			Initialise();
			this.StartCoroutine(LoadAssets());
		}

		protected override IEnumerator LoadAssets()
		{
			AssetReference weaponIllustrationReference = m_definition.GetWeaponIllustrationReference();
			AssetLoadRequest<Sprite> assetReferenceRequest = weaponIllustrationReference.LoadFromAssetBundleAsync<Sprite>(AssetBundlesUtility.GetUICharacterResourcesBundleName());
			while (!assetReferenceRequest.get_isDone())
			{
				yield return null;
			}
			if (AssetManagerError.op_Implicit(assetReferenceRequest.get_error()) != 0)
			{
				Log.Error($"Error while loading illustration for {((object)m_definition).GetType().Name} {m_definition.get_name()} error={assetReferenceRequest.get_error()}", 47, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\Player\\DeckRoot\\WeaponRibbonItem.cs");
				yield break;
			}
			m_visual.set_sprite(assetReferenceRequest.get_asset());
			AssetReference uIWeaponButtonReference = m_definition.GetUIWeaponButtonReference();
			AssetLoadRequest<Material> matAssetReferenceRequest = uIWeaponButtonReference.LoadFromAssetBundleAsync<Material>("core/ui/characters/heroes");
			while (!matAssetReferenceRequest.get_isDone())
			{
				yield return null;
			}
			if (AssetManagerError.op_Implicit(matAssetReferenceRequest.get_error()) != 0)
			{
				Log.Error(string.Format("Error while loading material {0} error={1}", "core/ui/characters/weaponbutton", assetReferenceRequest.get_error()), 63, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\Player\\DeckRoot\\WeaponRibbonItem.cs");
				yield break;
			}
			m_equipedMaterial = matAssetReferenceRequest.get_asset();
			m_shine.set_color(m_definition.deckBuildingWeaponShine);
		}

		protected override void ApplySelect()
		{
			base.ApplySelect();
			m_deckMain.DisplayWeapon(m_definition);
		}

		public override void SetEquiped(bool equipped)
		{
			base.SetEquiped(equipped);
			m_visual.set_material(equipped ? m_equipedMaterial : null);
			m_shine.get_gameObject().SetActive(equipped);
		}

		public void OnSelectionChange(WeaponDefinition definition)
		{
			bool flag = definition == m_definition;
			m_selectedTicks.get_gameObject().SetActive(flag);
			if (!flag)
			{
				OnUnselect();
			}
		}

		public WeaponDefinition GetWeapon()
		{
			return m_definition;
		}
	}
}
