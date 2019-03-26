using Ankama.Cube.Data;
using Ankama.Cube.TEMPFastEnterMatch.Player;
using Ankama.Cube.UI.Components;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ankama.Cube.UI
{
	public class GodSelectionUI : AbstractUI
	{
		[SerializeField]
		private Button m_closeButton;

		[SerializeField]
		private TextFieldDropdown m_godSelection;

		public Action<God> onGodSelected;

		public Action onCloseClick;

		private readonly List<God> m_playableGods = new List<God>();

		private unsafe void Start()
		{
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Expected O, but got Unknown
			m_godSelection.options.Clear();
			List<string> list = new List<string>();
			m_playableGods.Clear();
			God god = (PlayerData.instance == null) ? God.Iop : PlayerData.instance.god;
			int num = -1;
			int num2 = 0;
			foreach (GodDefinition value in RuntimeData.godDefinitions.Values)
			{
				if (value.playable)
				{
					m_playableGods.Add(value.god);
					list.Add(RuntimeData.FormattedText(value.i18nNameId));
					if (value.god == god)
					{
						num = num2;
					}
					num2++;
				}
			}
			m_godSelection.AddOptions(list);
			m_godSelection.value = ((num >= 0) ? num : 0);
			m_godSelection.onValueChanged.AddListener(new UnityAction<int>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_closeButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		}

		private void OnCloseClick()
		{
			onCloseClick?.Invoke();
		}

		private void OnValueChange(int value)
		{
			God obj = m_playableGods[value];
			onGodSelected?.Invoke(obj);
		}

		public void SimulateCloseClick()
		{
			InputUtility.SimulateClickOn(m_closeButton);
		}
	}
}
