using Ankama.Utilities;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Components
{
	public class AnimatedTextButton : Button
	{
		[SerializeField]
		private TextField m_text;

		[SerializeField]
		private AnimatedTextButtonStyle m_style;

		[SerializeField]
		private Image m_background;

		[SerializeField]
		private Image m_border;

		[SerializeField]
		private Image m_outline;

		private RectTransform m_outlineRectTransform;

		private RectTransform m_backgroundRectTransform;

		private Sequence m_tweenSequence;

		public TextField textField => m_text;

		protected override void Awake()
		{
			if (Object.op_Implicit(m_outline))
			{
				m_outlineRectTransform = m_outline.GetComponent<RectTransform>();
			}
			if (Object.op_Implicit(m_background))
			{
				m_backgroundRectTransform = m_background.GetComponent<RectTransform>();
			}
		}

		protected override void OnEnable()
		{
			if (m_style == null)
			{
				Log.Error("AnimatedTextButton " + this.get_name() + " doesn't have a style defined !", 44, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\Components\\AnimatedTextButton.cs");
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
			if (EventSystem.get_current() != null && EventSystem.get_current().get_currentSelectedGameObject() == this.get_gameObject())
			{
				EventSystem.get_current().SetSelectedGameObject(null);
				this.Select();
			}
			this.OnEnable();
		}

		protected override void DoStateTransition(SelectionState state, bool instant)
		{
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Expected I4, but got Unknown
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0210: Unknown result type (might be due to invalid IL or missing references)
			//IL_0250: Unknown result type (might be due to invalid IL or missing references)
			//IL_028f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
			if (!this.get_gameObject().get_activeInHierarchy())
			{
				return;
			}
			if (m_style == null)
			{
				Log.Error("AnimatedTextButton " + this.get_name() + " doesn't have a style defined !", 92, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\Components\\AnimatedTextButton.cs");
				return;
			}
			AnimatedTextButtonState animatedTextButtonState = m_style.disable;
			switch ((int)state)
			{
			case 0:
				animatedTextButtonState = m_style.normal;
				break;
			case 1:
				animatedTextButtonState = m_style.highlight;
				break;
			case 2:
				animatedTextButtonState = m_style.pressed;
				break;
			case 3:
				animatedTextButtonState = m_style.disable;
				break;
			}
			Sequence tweenSequence = m_tweenSequence;
			if (tweenSequence != null)
			{
				TweenExtensions.Kill(tweenSequence, false);
			}
			if (instant)
			{
				if (Object.op_Implicit(m_text))
				{
					m_text.color = animatedTextButtonState.textColor;
				}
				if (Object.op_Implicit(m_background))
				{
					m_background.set_color(animatedTextButtonState.backgroundColor);
				}
				if (Object.op_Implicit(m_backgroundRectTransform))
				{
					m_backgroundRectTransform.set_sizeDelta(animatedTextButtonState.backgroundSizeDelta);
				}
				if (Object.op_Implicit(m_border))
				{
					m_border.set_color(animatedTextButtonState.borderColor);
				}
				if (Object.op_Implicit(m_outline))
				{
					m_outline.set_color(animatedTextButtonState.outlineColor);
				}
				if (Object.op_Implicit(m_outlineRectTransform))
				{
					m_outlineRectTransform.set_sizeDelta(animatedTextButtonState.outlineSizeDelta);
				}
				return;
			}
			m_tweenSequence = DOTween.Sequence();
			if (Object.op_Implicit(m_text))
			{
				TweenSettingsExtensions.Insert(m_tweenSequence, 0f, m_text.DOColor(animatedTextButtonState.textColor, m_style.baseButtonStyle.transitionDuration));
			}
			if (Object.op_Implicit(m_background))
			{
				TweenSettingsExtensions.Insert(m_tweenSequence, 0f, DOTweenModuleUI.DOBlendableColor(m_background, animatedTextButtonState.backgroundColor, m_style.baseButtonStyle.transitionDuration));
			}
			if (Object.op_Implicit(m_backgroundRectTransform))
			{
				TweenSettingsExtensions.Insert(m_tweenSequence, 0f, DOTweenModuleUI.DOSizeDelta(m_backgroundRectTransform, animatedTextButtonState.backgroundSizeDelta, m_style.baseButtonStyle.transitionDuration, false));
			}
			if (Object.op_Implicit(m_border))
			{
				TweenSettingsExtensions.Insert(m_tweenSequence, 0f, DOTweenModuleUI.DOBlendableColor(m_border, animatedTextButtonState.borderColor, m_style.baseButtonStyle.transitionDuration));
			}
			if (Object.op_Implicit(m_outline))
			{
				TweenSettingsExtensions.Insert(m_tweenSequence, 0f, DOTweenModuleUI.DOBlendableColor(m_outline, animatedTextButtonState.outlineColor, m_style.baseButtonStyle.transitionDuration));
			}
			if (Object.op_Implicit(m_outlineRectTransform))
			{
				TweenSettingsExtensions.Insert(m_tweenSequence, 0f, DOTweenModuleUI.DOSizeDelta(m_outlineRectTransform, animatedTextButtonState.outlineSizeDelta, m_style.baseButtonStyle.transitionDuration, false));
			}
		}

		public AnimatedTextButton()
			: this()
		{
		}
	}
}
