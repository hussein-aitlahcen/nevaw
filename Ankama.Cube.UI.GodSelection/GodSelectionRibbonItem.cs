using Ankama.AssetManagement.AssetReferences;
using Ankama.Cube.Data;
using Ankama.Cube.UI.Components;
using Ankama.Cube.Utility;
using System.Collections;
using UnityEngine;

namespace Ankama.Cube.UI.GodSelection
{
	public class GodSelectionRibbonItem : BaseRibbonItem
	{
		private GodSelectionRoot m_uiRoot;

		private GodDefinition m_definition;

		[SerializeField]
		protected ImageLoader m_visualLoader;

		public void Initialise(GodSelectionRoot uiRoot, GodDefinition definition)
		{
			m_uiRoot = uiRoot;
			m_definition = definition;
			Initialise();
		}

		protected override IEnumerator LoadAssets()
		{
			AssetReference uIIconReference = m_definition.GetUIIconReference();
			m_visualLoader.Setup(uIIconReference, AssetBundlesUtility.GetUIGodsResourcesBundleName());
			while (m_visualLoader.loadState == UIResourceLoadState.Loading)
			{
				yield return null;
			}
		}

		protected override void ApplySelect()
		{
			base.ApplySelect();
			m_uiRoot.DisplayGod(m_definition);
		}

		public void OnSelectionChange(GodDefinition definition)
		{
			bool num = definition == m_definition;
			m_selectedTicks.get_gameObject().SetActive(definition == m_definition);
			if (!num)
			{
				OnUnselect();
			}
		}

		public GodDefinition GetGod()
		{
			return m_definition;
		}
	}
}
