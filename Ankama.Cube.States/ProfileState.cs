using Ankama.AssetManagement.StateManagement;
using Ankama.Cube.UI;
using Ankama.Cube.UI.PlayerLayer;
using System.Collections;

namespace Ankama.Cube.States
{
	public class ProfileState : LoadSceneStateContext, IStateUIChildPriority
	{
		private ProfileUIRoot m_ui;

		public UIPriority uiChildPriority => UIPriority.Front;

		protected override IEnumerator Load()
		{
			UILoader<ProfileUIRoot> loader = new UILoader<ProfileUIRoot>(this, "PlayerLayer_ProfilCanvas", "core/scenes/ui/deck", disableOnLoad: true);
			yield return loader.Load();
			m_ui = loader.ui;
			m_ui.get_gameObject().SetActive(true);
			m_ui.Initialise();
		}

		protected override IEnumerator Update()
		{
			yield return m_ui.PlayEnterAnimation();
			_003C_003En__0();
		}

		public override bool AllowsTransition(StateContext nextState)
		{
			return true;
		}

		protected override IEnumerator Transition(StateTransitionInfo transitionInfo)
		{
			yield return m_ui.Unload();
		}

		protected override void Enable()
		{
			m_ui.PlayEnterAnimation();
			this.Enable();
		}
	}
}
