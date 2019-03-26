using Ankama.Utilities;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ankama.Cube.UI
{
	public class TabButton : Toggle
	{
		[SerializeField]
		protected Image m_background;

		[SerializeField]
		protected Image m_border;

		[SerializeField]
		protected TabStyle m_style;

		private Sequence m_tweenSequence;

		protected unsafe override void Awake()
		{
			this.Awake();
			base.onValueChanged.AddListener(new UnityAction<bool>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			OnValueChanged(this.get_isOn());
		}

		private void OnValueChanged(bool on)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			this.DoStateTransition(this.get_currentSelectionState(), false);
		}

		protected override void OnEnable()
		{
			if (m_style == null)
			{
				Log.Error("TabButton " + this.get_name() + " doesn't have a style defined !", 38, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\Components\\TabButton.cs");
				this.OnEnable();
			}
			else
			{
				this.OnEnable();
			}
		}

		protected override void DoStateTransition(SelectionState state, bool instant)
		{
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Expected I4, but got Unknown
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			if (!this.get_gameObject().get_activeInHierarchy())
			{
				return;
			}
			if (m_style == null)
			{
				Log.Error("TabButton " + this.get_name() + " doesn't have a style defined !", 70, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\Components\\TabButton.cs");
				return;
			}
			TabStyle.TabState tabState = m_style.disable;
			switch ((int)state)
			{
			case 0:
				tabState = (this.get_isOn() ? m_style.selected : m_style.normal);
				break;
			case 1:
				tabState = (this.get_isOn() ? m_style.selected : m_style.highlight);
				break;
			case 2:
				tabState = (this.get_isOn() ? m_style.selected : m_style.pressed);
				break;
			case 3:
				tabState = m_style.disable;
				break;
			}
			Sequence tweenSequence = m_tweenSequence;
			if (tweenSequence != null)
			{
				TweenExtensions.Kill(tweenSequence, false);
			}
			if (instant)
			{
				if (Object.op_Implicit(m_background))
				{
					m_background.set_color(tabState.backgroundColor);
				}
				if (Object.op_Implicit(m_border))
				{
					m_border.set_color(tabState.borderColor);
				}
				return;
			}
			m_tweenSequence = DOTween.Sequence();
			if (Object.op_Implicit(m_background))
			{
				TweenSettingsExtensions.Insert(m_tweenSequence, 0f, DOTweenModuleUI.DOBlendableColor(m_background, tabState.backgroundColor, m_style.transitionDuration));
			}
			if (Object.op_Implicit(m_border))
			{
				TweenSettingsExtensions.Insert(m_tweenSequence, 0f, DOTweenModuleUI.DOBlendableColor(m_border, tabState.borderColor, m_style.transitionDuration));
			}
		}

		public TabButton()
			: this()
		{
		}
	}
}
