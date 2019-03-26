using Ankama.Utilities;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Components
{
	public class AnimatedToggleButton : Toggle
	{
		[SerializeField]
		private Image m_background;

		[SerializeField]
		private Image m_border;

		[SerializeField]
		private Image m_outline;

		[SerializeField]
		private AnimatedToggleButtonStyle m_style;

		[SerializeField]
		private Image m_tickImage;

		private RectTransform m_backgroundRectTransform;

		private RectTransform m_graphicRectTransform;

		private RectTransform m_outlineRectTransform;

		private Sequence m_tweenSequence;

		protected unsafe override void Awake()
		{
			if (Object.op_Implicit(m_outline))
			{
				m_outlineRectTransform = m_outline.GetComponent<RectTransform>();
			}
			if (Object.op_Implicit(m_background))
			{
				m_backgroundRectTransform = m_background.GetComponent<RectTransform>();
			}
			if (Object.op_Implicit(m_tickImage))
			{
				m_graphicRectTransform = m_tickImage.GetComponent<RectTransform>();
			}
			base.onValueChanged.AddListener(new UnityAction<bool>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			OnValueChanged(this.get_isOn());
		}

		private void OnValueChanged(bool on)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			if (!(m_tickImage == null))
			{
				Color val = m_tickImage.get_color();
				if (m_style.useOnlyAlpha)
				{
					val.a = (this.get_isOn() ? m_style.selectedGraphicColor.a : m_style.baseGraphicColor.a);
				}
				else
				{
					val = (on ? m_style.selectedGraphicColor : m_style.baseGraphicColor);
				}
				if (!Application.get_isPlaying())
				{
					m_tickImage.set_color(val);
				}
				else
				{
					DOTweenModuleUI.DOBlendableColor(m_tickImage, val, m_style.selectionTransitionDuration);
				}
			}
		}

		protected override void OnEnable()
		{
			if (m_style == null)
			{
				Log.Error("AnimatedToggleButton " + this.get_name() + " doesn't have a style defined !", 64, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\Components\\AnimatedToggleButton.cs");
				this.OnEnable();
				return;
			}
			if (Object.op_Implicit(m_background))
			{
				m_background.set_sprite(m_style.baseButtonStyle.background);
			}
			if (Object.op_Implicit(m_border))
			{
				m_border.set_sprite(m_style.baseButtonStyle.border);
			}
			if (Object.op_Implicit(m_outline))
			{
				m_outline.set_sprite(m_style.baseButtonStyle.outline);
			}
			if (Object.op_Implicit(m_tickImage))
			{
				m_tickImage.set_sprite(m_style.baseButtonStyle.toggle);
			}
			this.OnEnable();
		}

		protected override void DoStateTransition(SelectionState state, bool instant)
		{
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Expected I4, but got Unknown
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Unknown result type (might be due to invalid IL or missing references)
			//IL_0254: Unknown result type (might be due to invalid IL or missing references)
			//IL_0294: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0312: Unknown result type (might be due to invalid IL or missing references)
			if (!this.get_gameObject().get_activeInHierarchy())
			{
				return;
			}
			if (m_style == null)
			{
				Log.Error("AnimatedToggleButton " + this.get_name() + " doesn't have a style defined !", 102, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\Components\\AnimatedToggleButton.cs");
				return;
			}
			AnimatedToggleButtonState animatedToggleButtonState = m_style.disable;
			switch ((int)state)
			{
			case 0:
				animatedToggleButtonState = m_style.normal;
				break;
			case 1:
				animatedToggleButtonState = m_style.highlight;
				break;
			case 2:
				animatedToggleButtonState = m_style.pressed;
				break;
			case 3:
				animatedToggleButtonState = m_style.disable;
				break;
			}
			if (animatedToggleButtonState.Equals(m_style.highlight) && this.get_group() != null && !this.get_group().get_allowSwitchOff() && this.get_isOn())
			{
				return;
			}
			Sequence tweenSequence = m_tweenSequence;
			if (tweenSequence != null)
			{
				TweenExtensions.Kill(tweenSequence, false);
			}
			if (instant)
			{
				if (Object.op_Implicit(m_graphicRectTransform))
				{
					m_graphicRectTransform.set_sizeDelta(animatedToggleButtonState.graphicSizeDelta);
				}
				if (Object.op_Implicit(m_background))
				{
					m_background.set_color(animatedToggleButtonState.backgroundColor);
				}
				if (Object.op_Implicit(m_backgroundRectTransform))
				{
					m_backgroundRectTransform.set_sizeDelta(animatedToggleButtonState.backgroundSizeDelta);
				}
				if (Object.op_Implicit(m_border))
				{
					m_border.set_color(animatedToggleButtonState.borderColor);
				}
				if (Object.op_Implicit(m_outline))
				{
					m_outline.set_color(animatedToggleButtonState.outlineColor);
				}
				if (Object.op_Implicit(m_outlineRectTransform))
				{
					m_outlineRectTransform.set_sizeDelta(animatedToggleButtonState.outlineSizeDelta);
				}
				return;
			}
			m_tweenSequence = DOTween.Sequence();
			if (Object.op_Implicit(m_graphicRectTransform))
			{
				TweenSettingsExtensions.Insert(m_tweenSequence, 0f, DOTweenModuleUI.DOSizeDelta(m_graphicRectTransform, animatedToggleButtonState.graphicSizeDelta, m_style.baseButtonStyle.transitionDuration, false));
			}
			if (Object.op_Implicit(m_background))
			{
				TweenSettingsExtensions.Insert(m_tweenSequence, 0f, DOTweenModuleUI.DOBlendableColor(m_background, animatedToggleButtonState.backgroundColor, m_style.baseButtonStyle.transitionDuration));
			}
			if (Object.op_Implicit(m_backgroundRectTransform))
			{
				TweenSettingsExtensions.Insert(m_tweenSequence, 0f, DOTweenModuleUI.DOSizeDelta(m_backgroundRectTransform, animatedToggleButtonState.backgroundSizeDelta, m_style.baseButtonStyle.transitionDuration, false));
			}
			if (Object.op_Implicit(m_border))
			{
				TweenSettingsExtensions.Insert(m_tweenSequence, 0f, DOTweenModuleUI.DOBlendableColor(m_border, animatedToggleButtonState.borderColor, m_style.baseButtonStyle.transitionDuration));
			}
			if (Object.op_Implicit(m_outline))
			{
				TweenSettingsExtensions.Insert(m_tweenSequence, 0f, DOTweenModuleUI.DOBlendableColor(m_outline, animatedToggleButtonState.outlineColor, m_style.baseButtonStyle.transitionDuration));
			}
			if (Object.op_Implicit(m_outlineRectTransform))
			{
				TweenSettingsExtensions.Insert(m_tweenSequence, 0f, DOTweenModuleUI.DOSizeDelta(m_outlineRectTransform, animatedToggleButtonState.outlineSizeDelta, m_style.baseButtonStyle.transitionDuration, false));
			}
		}

		public AnimatedToggleButton()
			: this()
		{
		}
	}
}
