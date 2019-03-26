using Ankama.Cube.Fight;
using Ankama.Cube.UI;
using Ankama.Utilities;
using System.Collections;

namespace Ankama.Cube.States
{
	public class FightEndFeedbackState : LoadSceneStateContext
	{
		private FightStatusEndReason m_reason;

		private BaseOpenCloseUI m_ui;

		private bool m_isActive = true;

		public bool isActive => m_isActive;

		public FightEndFeedbackState(FightStatusEndReason reason)
		{
			m_reason = reason;
		}

		protected override IEnumerator Load()
		{
			string sceneName = (m_reason == FightStatusEndReason.Win) ? "FightEndedWinMatchUI" : "FightEndedDiedUI";
			UILoader<BaseOpenCloseUI> loader = new UILoader<BaseOpenCloseUI>(this, sceneName, "core/scenes/ui/fight", disableOnLoad: true);
			yield return loader.Load();
			m_ui = loader.ui;
		}

		protected override IEnumerator Update()
		{
			m_ui.get_gameObject().SetActive(true);
			yield return m_ui.OpenCoroutine();
			yield return (object)new WaitForTime(1.7f);
			yield return m_ui.CloseCoroutine();
			this.get_parent().ClearChildState(0);
			m_isActive = false;
		}
	}
}
