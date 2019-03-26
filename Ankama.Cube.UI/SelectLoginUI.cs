using Ankama.Cube.Player;
using Ankama.Cube.TEMPFastEnterMatch.Auth.ConnectionMethod;
using Ankama.Cube.UI.Components;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Ankama.Cube.UI
{
	public class SelectLoginUI : AbstractUI
	{
		[SerializeField]
		protected AnimatedTextButton m_createGuestButton;

		[SerializeField]
		protected AnimatedTextButton m_regularAccountButton;

		[SerializeField]
		protected TextField m_titleText;

		public event Action OnCreateGuest;

		public event Action OnConnectGuest;

		public event Action OnRegularAccount;

		private unsafe void Start()
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Expected O, but got Unknown
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected O, but got Unknown
			PlayerPreferences.useGuest = true;
			PlayerPreferences.Save();
			m_createGuestButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_regularAccountButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			SetTexts();
		}

		private void SetTexts()
		{
			if (CredentialProvider.gameCredentialProvider.HasGuestAccount())
			{
				m_titleText.SetText(45852);
				m_createGuestButton.textField.SetText(19445);
				m_regularAccountButton.textField.SetText(84332);
			}
			else
			{
				m_titleText.SetText(34597);
				m_createGuestButton.textField.SetText(51147);
				m_regularAccountButton.textField.SetText(74237);
			}
		}

		public void HideGuestSelection()
		{
			PlayerPreferences.useGuest = false;
			m_createGuestButton.get_gameObject().SetActive(false);
		}
	}
}
