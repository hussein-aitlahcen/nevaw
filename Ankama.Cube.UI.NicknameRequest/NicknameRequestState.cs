using Ankama.AssetManagement.StateManagement;
using Ankama.Cube.States;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Ankama.Cube.UI.NicknameRequest
{
	public class NicknameRequestState : LoadSceneStateContext
	{
		private NicknameRequestUI m_ui;

		private string m_nickname;

		public event Action<string> OnSuccess;

		protected override IEnumerator Load()
		{
			UILoader<NicknameRequestUI> loader = new UILoader<NicknameRequestUI>(this, "NicknameRequestUI", "core/scenes/ui/login", disableOnLoad: true);
			yield return loader.Load();
			m_ui = loader.ui;
			m_ui.get_gameObject().SetActive(true);
			m_ui.OnNicknameRequest += OnNicknameRequest;
		}

		private void OnNicknameRequest(string nickname)
		{
			m_ui.interactable = false;
			m_nickname = nickname;
		}

		private void OnNicknameResult(bool success, IList<string> suggestList, string errorKey, string errorTranslated)
		{
			if (!success)
			{
				m_ui.interactable = true;
				m_ui.OnNicknameError(suggestList, errorKey, errorTranslated);
				return;
			}
			StateContext parent = this.get_parent();
			if (parent != null)
			{
				parent.ClearChildState(0);
			}
		}

		protected override IEnumerator Unload()
		{
			yield return _003C_003En__0();
			this.OnSuccess?.Invoke(m_nickname);
		}
	}
}
