using Ankama.AssetManagement;
using Ankama.AssetManagement.AssetReferences;
using Ankama.AssetManagement.StateManagement;
using Ankama.Cube.Data;
using Ankama.Cube.Protocols.PlayerProtocol;
using Ankama.Cube.States;
using Ankama.Cube.TEMPFastEnterMatch.Player;
using Ankama.Cube.UI.Components;
using Ankama.Cube.Utility;
using Ankama.Utilities;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Ankama.Cube.UI.PlayerLayer
{
	public class PlayerIconRoot : AbstractUI
	{
		[Header("Visual")]
		[SerializeField]
		private ImageLoader m_VisualLoader;

		[SerializeField]
		private AnimatedGraphicButton m_button;

		private bool m_isOpen;

		private PlayerUIMainState m_state;

		public static PlayerIconRoot instance
		{
			get;
			private set;
		}

		public unsafe void Initialise(PlayerUIMainState mainState)
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Expected O, but got Unknown
			m_state = mainState;
			instance = this;
			base.canvasGroup.set_alpha(0f);
			m_button.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		}

		public void OnClicVisual()
		{
			if (!m_isOpen)
			{
				ExpendPanel();
			}
			else
			{
				ReducePanel();
			}
		}

		private void ExpendPanel()
		{
			if (m_state.TryExpandPanel())
			{
				m_isOpen = true;
			}
		}

		public void ReducePanel()
		{
			m_isOpen = false;
			StateLayer val = default(StateLayer);
			if (StateManager.TryGetLayer("PlayerUI", ref val))
			{
				val.GetChainRoot().ClearChildState(0);
			}
		}

		public IEnumerator PlayEnterAnimation()
		{
			yield return (object)new WaitForTime(0.5f);
			DOTweenModuleUI.DOFade(base.canvasGroup, 1f, 1.5f);
			yield return (object)new WaitForTime(0.5f);
		}

		public void LoadVisual(int weaponID)
		{
			WeaponDefinition weaponDefinition = RuntimeData.weaponDefinitions[weaponID];
			if (weaponDefinition != null)
			{
				this.StartCoroutine(LoadPlayerVisual(weaponDefinition));
			}
		}

		public void LoadVisual()
		{
			if (PlayerData.instance.TryGetDeckById(PlayerData.instance.currentDeckId, out DeckInfo deckInfo))
			{
				foreach (KeyValuePair<int, WeaponDefinition> weaponDefinition in RuntimeData.weaponDefinitions)
				{
					if (weaponDefinition.Key == deckInfo.Weapon)
					{
						this.StartCoroutine(LoadPlayerVisual(weaponDefinition.Value));
					}
				}
			}
		}

		private IEnumerator LoadPlayerVisual(WeaponDefinition definition)
		{
			AssetReference illustrationReference = definition.GetIllustrationReference();
			m_VisualLoader.Setup(illustrationReference, AssetBundlesUtility.GetUICharacterResourcesBundleName());
			while (m_VisualLoader.loadState == UIResourceLoadState.Loading)
			{
				yield return null;
			}
		}
	}
}
