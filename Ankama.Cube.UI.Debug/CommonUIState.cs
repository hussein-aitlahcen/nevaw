using Ankama.Cube.States;
using System.Collections;

namespace Ankama.Cube.UI.Debug
{
	public class CommonUIState : LoadSceneStateContext
	{
		private CommonUI m_ui;

		protected override IEnumerator Load()
		{
			UILoader<CommonUI> loader = new UILoader<CommonUI>(this, "CommonUI", "core/scenes/ui/examples", disableOnLoad: true);
			yield return loader.Load();
			m_ui = loader.ui;
			m_ui.get_gameObject().SetActive(true);
		}
	}
}
