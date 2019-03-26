using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.TEMPFastEnterMatch.MatchMaking
{
	public class MatchMakingButton : MonoBehaviour
	{
		[SerializeField]
		private Button m_btn;

		[SerializeField]
		private Button m_forceAiBtn;

		[SerializeField]
		private Text m_text;

		public int fightDefId;

		private string m_buttonText;

		private bool m_isSearching;

		public bool isSearching => m_isSearching;

		public Button button => m_btn;

		public Button forceAiBUtton => m_forceAiBtn;

		private string GetButtonText()
		{
			if (m_buttonText == null)
			{
				m_buttonText = RuntimeData.fightDefinitions[fightDefId].get_displayName();
			}
			return m_buttonText;
		}

		public void StartWait()
		{
			m_text.set_text("Cancel " + GetButtonText());
			m_forceAiBtn.get_gameObject().SetActive(true);
			m_isSearching = true;
		}

		public void StopWait()
		{
			m_text.set_text(GetButtonText());
			m_forceAiBtn.get_gameObject().SetActive(false);
			m_isSearching = false;
		}

		public MatchMakingButton()
			: this()
		{
		}
	}
}
