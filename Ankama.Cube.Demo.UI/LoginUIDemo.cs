using Ankama.Cube.UI;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ankama.Cube.Demo.UI
{
	public class LoginUIDemo : BaseOpenCloseUI
	{
		[SerializeField]
		private InputField m_pseudo;

		[SerializeField]
		private Button m_loginButton;

		[SerializeField]
		private Image m_blackBackground;

		[SerializeField]
		private Selectable[] m_selectables;

		private int m_selectedIndex;

		public Action<bool, string> onConnect;

		private unsafe void Start()
		{
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Expected O, but got Unknown
			m_blackBackground.get_gameObject().SetActive(true);
			m_blackBackground.WithAlpha<Image>(1f);
			m_pseudo.set_text(string.Empty);
			m_pseudo.Select();
			m_pseudo.get_onValueChanged().AddListener(new UnityAction<string>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_loginButton.set_interactable(false);
			m_loginButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		}

		private void OnPseudoChanged(string pseudo)
		{
			m_loginButton.set_interactable(!string.IsNullOrWhiteSpace(pseudo));
		}

		public void DoClickSelected()
		{
			Selectable[] selectables = m_selectables;
			int num = selectables.Length;
			if (num == 0)
			{
				return;
			}
			Selectable val;
			while (true)
			{
				if (m_selectedIndex < num)
				{
					val = selectables[m_selectedIndex];
					if (val != m_pseudo)
					{
						break;
					}
					SelectNext();
					continue;
				}
				return;
			}
			InputUtility.SimulateClickOn(val);
		}

		public void SelectNext()
		{
			if (m_selectables.Length != 0)
			{
				m_selectedIndex = (m_selectedIndex + 1) % m_selectables.Length;
				m_selectables[m_selectedIndex].Select();
			}
		}

		private void OnLoginButtonClicked()
		{
			onConnect?.Invoke(arg1: false, m_pseudo.get_text().Trim());
		}
	}
}
