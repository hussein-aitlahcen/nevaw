using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Fight
{
	public class FightEndedUI : BaseOpenCloseUI
	{
		[SerializeField]
		private Button m_buttonOK;

		public Action onContinueClick;

		private unsafe void Start()
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Expected O, but got Unknown
			m_buttonOK.Select();
			m_buttonOK.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		}

		public void DoContinueClick()
		{
			InputUtility.SimulateClickOn(m_buttonOK);
		}

		private void OnButtonOKClicked()
		{
			onContinueClick?.Invoke();
		}
	}
}
