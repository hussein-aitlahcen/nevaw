using Ankama.Cube.UI.Components;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ankama.Cube.UI.NicknameRequest
{
	public class NicknameRequestUI : AbstractUI
	{
		[SerializeField]
		private InputField m_nicknameInputField;

		[SerializeField]
		private GameObject m_errorContainer;

		[SerializeField]
		private RawTextField m_errorMessage;

		[SerializeField]
		private RawTextField m_suggestions;

		[SerializeField]
		private Button m_okButton;

		public event Action<string> OnNicknameRequest;

		protected unsafe override void Awake()
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			m_errorContainer.SetActive(false);
			m_okButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_okButton.set_interactable(false);
			m_nicknameInputField.get_onValueChanged().AddListener(new UnityAction<string>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		}

		private void OnValueChanged(string value)
		{
			bool flag = true;
			flag &= (value.Length >= 3 && value.Length <= 20);
			m_okButton.set_interactable(flag);
		}

		private void OnNickname()
		{
			base.interactable = true;
			this.OnNicknameRequest?.Invoke(m_nicknameInputField.get_text());
		}

		public void OnNicknameError(IList<string> suggestList, string errorKey, string errorTranslated)
		{
			m_errorContainer.SetActive(true);
			m_errorMessage.SetText(errorTranslated);
			m_suggestions.SetText(string.Join(", ", suggestList));
			base.interactable = true;
			m_nicknameInputField.ActivateInputField();
		}
	}
}
