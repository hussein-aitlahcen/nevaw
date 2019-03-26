using Ankama.AssetManagement;
using Ankama.AssetManagement.StateManagement;
using Ankama.Cube.States;
using Ankama.Cube.UI.Components;
using Ankama.Cube.UI.Components.Tooltip;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Ankama.Cube.UI.PlayerLayer
{
	public class PlayerLayerNavRoot : AbstractUI
	{
		public Action OnCloseAction;

		[SerializeField]
		private CanvasGroup m_BG;

		[SerializeField]
		private List<PlayerLayerNavButton> m_navButton;

		[Header("Button")]
		[SerializeField]
		private PlayerLayerNavButton m_profilButton;

		[SerializeField]
		private PlayerLayerNavButton m_collectionButton;

		[SerializeField]
		private PlayerLayerNavButton m_deckButton;

		[SerializeField]
		private AnimatedGraphicButton m_cancelButton;

		[SerializeField]
		private GenericTooltipWindow m_genericTooltipWindow;

		private PlayerLayerNavButton m_curentButton;

		public unsafe void Initialise()
		{
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Expected O, but got Unknown
			m_genericTooltipWindow.alpha = 0f;
			m_BG.set_alpha(0f);
			m_BG.set_blocksRaycasts(true);
			Initialise(OpenProfileUI, OpenDeckUI, null);
			m_cancelButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		}

		private void OnPreviousPress()
		{
			OnCloseAction?.Invoke();
		}

		public void Initialise(Action ProfileAction, Action DeckAction, Action CollectionAction)
		{
			m_navButton = new List<PlayerLayerNavButton>();
			m_profilButton.SetMethode(ProfileAction);
			m_deckButton.SetMethode(DeckAction);
			m_collectionButton.SetMethode(CollectionAction);
			m_navButton.Add(m_profilButton);
			m_navButton.Add(m_deckButton);
			m_navButton.Add(m_collectionButton);
			foreach (PlayerLayerNavButton item in m_navButton)
			{
				item.Initialise(this);
				item.OnDeselect();
			}
		}

		private void OpenDeckUI()
		{
			StateLayer val = default(StateLayer);
			if (StateManager.TryGetLayer("PlayerUI", ref val))
			{
				DeckMainState deckMainState = new DeckMainState();
				val.GetChainRoot().GetChildState().SetChildState(deckMainState, 0);
			}
		}

		private void OpenProfileUI()
		{
			StateLayer val = default(StateLayer);
			if (StateManager.TryGetLayer("PlayerUI", ref val))
			{
				ProfileState profileState = new ProfileState();
				val.GetChainRoot().GetChildState().SetChildState(profileState, 0);
			}
		}

		private void ClicOnButton(PlayerLayerNavButton button, Action action)
		{
			if (!(m_curentButton == button))
			{
				m_curentButton = button;
				foreach (PlayerLayerNavButton item in m_navButton)
				{
					item.OnDeselect();
				}
				button.OnValidate();
				action();
			}
		}

		public IEnumerator OnClose()
		{
			m_BG.set_blocksRaycasts(true);
			m_curentButton = null;
			yield return PlayAnimation(m_animationDirector.GetAnimation("Close"));
		}

		public IEnumerator PlayEnterAnimation()
		{
			m_BG.set_blocksRaycasts(true);
			yield return PlayAnimation(m_animationDirector.GetAnimation("Open"));
			OpenOnDeck();
		}

		private void OpenOnProfile()
		{
			OnClic(m_profilButton);
		}

		private void OpenOnDeck()
		{
			OnClic(m_deckButton);
		}

		public void OnClic(PlayerLayerNavButton button)
		{
			ClicOnButton(button, button.GetMethod());
		}
	}
}
