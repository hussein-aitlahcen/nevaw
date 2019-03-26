using Ankama.Cube.UI;
using Ankama.Cube.UI.Components;
using Ankama.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ankama.Cube.Code.UI
{
	public class PopupInfoUI : BaseOpenCloseUI
	{
		[Header("Components")]
		[SerializeField]
		protected RawTextField m_titleText;

		[SerializeField]
		protected RawTextField m_descriptionText;

		[SerializeField]
		protected Image m_blackBackground;

		[SerializeField]
		protected Image m_windowBackground;

		[Header("Buttons")]
		[SerializeField]
		protected Button m_buttonBackground;

		[SerializeField]
		protected AnimatedTextButton m_buttonNormal;

		[SerializeField]
		protected AnimatedTextButton m_buttonNegative;

		[SerializeField]
		protected AnimatedTextButton m_buttonCancel;

		[Header("Styles")]
		[SerializeField]
		protected PopupInfoStyle[] m_styles;

		public Action closeAction;

		private readonly List<Button> m_buttons = new List<Button>();

		private int m_selectedIndex;

		public unsafe void Initialize(PopupInfo data)
		{
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Expected O, but got Unknown
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			m_useBlur = data.useBlur;
			m_buttonNormal.get_gameObject().SetActive(false);
			m_buttonNegative.get_gameObject().SetActive(false);
			m_buttonCancel.get_gameObject().SetActive(false);
			ApplyRawText(m_titleText, data.title);
			ApplyRawText(m_descriptionText, data.message);
			ButtonData[] buttons = data.buttons;
			if (buttons != null)
			{
				int num = buttons.Length;
				for (int i = 0; i < num; i++)
				{
					AddButton(buttons[i]);
				}
			}
			if (data.closeOnBackgroundClick)
			{
				m_buttonBackground.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			}
			PopupInfoStyle style = GetStyle(data.style);
			m_titleText.color = style.titleColor;
			m_descriptionText.color = style.textColor;
			m_selectedIndex = data.selectedButton - 1;
			if (m_selectedIndex < 0 || m_selectedIndex >= m_buttons.Count)
			{
				m_selectedIndex = 0;
			}
			if (m_buttons.Count > 0)
			{
				m_buttons[m_selectedIndex].Select();
			}
		}

		private PopupInfoStyle GetStyle(PopupStyle style)
		{
			PopupInfoStyle[] styles = m_styles;
			int num = styles.Length;
			for (int i = 0; i < num; i++)
			{
				PopupInfoStyle popupInfoStyle = styles[i];
				if (popupInfoStyle.style == style)
				{
					return popupInfoStyle;
				}
			}
			Log.Error($"Cannot find style {style}", 96, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\PopupInfo\\PopupInfoUI.cs");
			return m_styles[0];
		}

		public void RemoveListeners()
		{
			List<Button> buttons = m_buttons;
			int count = buttons.Count;
			for (int i = 0; i < count; i++)
			{
				buttons[i].get_onClick().RemoveAllListeners();
			}
			m_buttonBackground.get_onClick().RemoveAllListeners();
		}

		private unsafe void AddButton(ButtonData data)
		{
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Expected O, but got Unknown
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Expected O, but got Unknown
			if (data.isValid)
			{
				AnimatedTextButton freeButton = GetFreeButton(data.style);
				freeButton.get_gameObject().SetActive(true);
				freeButton.get_transform().SetAsLastSibling();
				if (data.textOverride.isValid)
				{
					ApplyText(freeButton.textField, data.textOverride);
				}
				if (data.onClick != null)
				{
					freeButton.get_onClick().AddListener(new UnityAction((object)data.onClick, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
				}
				if (data.closeOnClick)
				{
					freeButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
				}
				m_buttons.Add(freeButton);
			}
		}

		private AnimatedTextButton GetFreeButton(ButtonStyle style)
		{
			AnimatedTextButton animatedTextButton;
			switch (style)
			{
			case ButtonStyle.Normal:
				animatedTextButton = m_buttonNormal;
				break;
			case ButtonStyle.Negative:
				animatedTextButton = m_buttonNegative;
				break;
			case ButtonStyle.Cancel:
				animatedTextButton = m_buttonCancel;
				break;
			default:
				throw new ArgumentOutOfRangeException("style", style, null);
			}
			if (!animatedTextButton.get_gameObject().get_activeSelf())
			{
				return animatedTextButton;
			}
			AnimatedTextButton animatedTextButton2 = Object.Instantiate<AnimatedTextButton>(animatedTextButton);
			animatedTextButton2.get_transform().SetParent(animatedTextButton.get_transform().get_parent(), false);
			return animatedTextButton2;
		}

		private void DoClose()
		{
			closeAction?.Invoke();
		}

		private static void ApplyRawText(RawTextField text, RawTextData data)
		{
			text.get_gameObject().SetActive(data.isValid);
			if (data.isValid)
			{
				text.SetText(data.formattedText);
			}
		}

		private static void ApplyText(TextField text, TextData data)
		{
			text.get_gameObject().SetActive(data.isValid);
			if (data.isValid)
			{
				text.SetText(data.textId.Value, data.valueProvider);
			}
		}

		public void DoClickSelected()
		{
			List<Button> buttons = m_buttons;
			if (buttons.Count != 0)
			{
				Button val = buttons[m_selectedIndex];
				if (val.get_gameObject() == EventSystem.get_current().get_currentSelectedGameObject())
				{
					InputUtility.SimulateClickOn(val);
				}
			}
		}

		public void SelectNext()
		{
			List<Button> buttons = m_buttons;
			int count = buttons.Count;
			if (count != 0)
			{
				m_selectedIndex = (m_selectedIndex + 1) % count;
				buttons[m_selectedIndex].Select();
			}
		}
	}
}
