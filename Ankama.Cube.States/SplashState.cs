using Ankama.AssetManagement.StateManagement;
using Ankama.Cube.UI;
using Ankama.Cube.Utility;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Ankama.Cube.States
{
	public class SplashState : StateContext
	{
		private const string SplashUISceneName = "SplashUI";

		private Scene m_splashUIScene;

		private AbstractUI m_ui;

		protected override IEnumerator Load()
		{
			yield return SceneManager.LoadSceneAsync("SplashUI", 1);
			m_splashUIScene = SceneManager.GetSceneByName("SplashUI");
			while (!m_splashUIScene.get_isLoaded())
			{
				yield return null;
			}
			m_ui = ScenesUtility.GetSceneRoot<AbstractUI>(m_splashUIScene);
			m_ui.SetDepth(this);
		}

		protected override IEnumerator Unload()
		{
			if (m_splashUIScene.IsValid())
			{
				yield return SceneManager.UnloadSceneAsync(m_splashUIScene);
			}
			yield return _003C_003En__0();
		}

		public SplashState()
			: this()
		{
		}
	}
}
