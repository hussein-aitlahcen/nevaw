using Ankama.AssetManagement.StateManagement;
using Ankama.Cube.UI;
using JetBrains.Annotations;
using System.Collections;

namespace Ankama.Cube.States
{
	public class TransitionState : LoadSceneStateContext
	{
		private TransitionUI m_ui;

		private readonly StateContext m_previousState;

		private readonly StateContext m_nextState;

		public TransitionState([NotNull] StateContext nextState, [CanBeNull] StateContext previousState)
		{
			m_previousState = previousState;
			m_nextState = nextState;
		}

		protected override IEnumerator Load()
		{
			UILoader<TransitionUI> loader = new UILoader<TransitionUI>(this, "TransitionUI", "core/scenes/ui/transition", disableOnLoad: true);
			yield return loader.Load();
			m_ui = loader.ui;
			m_ui.get_gameObject().SetActive(true);
			yield return m_ui.OpenCoroutine();
		}

		protected override IEnumerator Update()
		{
			if (m_previousState != null)
			{
				while ((int)m_previousState.get_loadState() != 9)
				{
					yield return null;
				}
			}
			this.get_parent().SetChildState(m_nextState, 0);
		}

		protected override IEnumerator Transition(StateTransitionInfo transitionInfo)
		{
			while (transitionInfo.get_stateContext() != null && (int)transitionInfo.get_stateContext().get_loadState() != 2)
			{
				yield return null;
			}
			if (null != m_ui)
			{
				yield return m_ui.CloseCoroutine();
			}
		}

		public override bool AllowsTransition(StateContext nextState)
		{
			return true;
		}
	}
}
