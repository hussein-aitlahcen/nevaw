using Ankama.Utilities;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Components
{
	public class AnimatedGraphicButton : Button
	{
		[SerializeField]
		private Graphic m_image;

		[SerializeField]
		private AnimatedImageButtonStyle m_style;

		[SerializeField]
		private Image m_background;

		[SerializeField]
		private Image m_border;

		[SerializeField]
		private Image m_outline;

		private RectTransform m_imageRectTransform;

		private RectTransform m_outlineRectTransform;

		private RectTransform m_backgroundRectTransform;

		private Sequence m_tweenSequence;

		public Graphic buttonImage => m_image;

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
			if (Object.op_Implicit(m_image))
			{
				m_imageRectTransform = m_image.GetComponent<RectTransform>();
			}
		}

		protected override void OnEnable()
		{
			if (m_style == null)
			{
				Log.Error("AnimatedImageButton " + this.get_name() + " doesn't have a style defined !", 46, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\Components\\AnimatedGraphicButton.cs");
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
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_026e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_032c: Unknown result type (might be due to invalid IL or missing references)
			if (!this.get_gameObject().get_activeInHierarchy())
			{
				return;
			}
			if (m_style == null)
			{
				Log.Error("AnimatedImageButton " + this.get_name() + " doesn't have a style defined !", 84, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\Components\\AnimatedGraphicButton.cs");
				return;
			}
			AnimatedImageButtonState animatedImageButtonState = m_style.disable;
			switch ((int)state)
			{
			case 0:
				animatedImageButtonState = m_style.normal;
				break;
			case 1:
				animatedImageButtonState = m_style.highlight;
				break;
			case 2:
				animatedImageButtonState = m_style.pressed;
				break;
			case 3:
				animatedImageButtonState = m_style.disable;
				break;
			}
			Sequence tweenSequence = m_tweenSequence;
			if (tweenSequence != null)
			{
				TweenExtensions.Kill(tweenSequence, false);
			}
			if (instant)
			{
				if (Object.op_Implicit(m_image))
				{
					m_image.set_color(animatedImageButtonState.imageColor);
				}
				if (Object.op_Implicit(m_imageRectTransform))
				{
					m_imageRectTransform.set_sizeDelta(animatedImageButtonState.imageSizeDelta);
				}
				if (Object.op_Implicit(m_background))
				{
					m_background.set_color(animatedImageButtonState.backgroundColor);
				}
				if (Object.op_Implicit(m_backgroundRectTransform))
				{
					m_backgroundRectTransform.set_sizeDelta(animatedImageButtonState.backgroundSizeDelta);
				}
				if (Object.op_Implicit(m_border))
				{
					m_border.set_color(animatedImageButtonState.borderColor);
				}
				if (Object.op_Implicit(m_outline))
				{
					m_outline.set_color(animatedImageButtonState.outlineColor);
				}
				if (Object.op_Implicit(m_outlineRectTransform))
				{
					m_outlineRectTransform.set_sizeDelta(animatedImageButtonState.outlineSizeDelta);
				}
				return;
			}
			m_tweenSequence = DOTween.Sequence();
			if (Object.op_Implicit(m_image))
			{
				TweenSettingsExtensions.Insert(m_tweenSequence, 0f, DOTweenModuleUI.DOColor(m_image, animatedImageButtonState.imageColor, m_style.baseButtonStyle.transitionDuration));
			}
			if (Object.op_Implicit(m_imageRectTransform))
			{
				TweenSettingsExtensions.Insert(m_tweenSequence, 0f, DOTweenModuleUI.DOSizeDelta(m_imageRectTransform, animatedImageButtonState.imageSizeDelta, m_style.baseButtonStyle.transitionDuration, false));
			}
			if (Object.op_Implicit(m_background))
			{
				TweenSettingsExtensions.Insert(m_tweenSequence, 0f, DOTweenModuleUI.DOBlendableColor(m_background, animatedImageButtonState.backgroundColor, m_style.baseButtonStyle.transitionDuration));
			}
			if (Object.op_Implicit(m_backgroundRectTransform))
			{
				TweenSettingsExtensions.Insert(m_tweenSequence, 0f, DOTweenModuleUI.DOSizeDelta(m_backgroundRectTransform, animatedImageButtonState.backgroundSizeDelta, m_style.baseButtonStyle.transitionDuration, false));
			}
			if (Object.op_Implicit(m_border))
			{
				TweenSettingsExtensions.Insert(m_tweenSequence, 0f, DOTweenModuleUI.DOBlendableColor(m_border, animatedImageButtonState.borderColor, m_style.baseButtonStyle.transitionDuration));
			}
			if (Object.op_Implicit(m_outline))
			{
				TweenSettingsExtensions.Insert(m_tweenSequence, 0f, DOTweenModuleUI.DOBlendableColor(m_outline, animatedImageButtonState.outlineColor, m_style.baseButtonStyle.transitionDuration));
			}
			if (Object.op_Implicit(m_outlineRectTransform))
			{
				TweenSettingsExtensions.Insert(m_tweenSequence, 0f, DOTweenModuleUI.DOSizeDelta(m_outlineRectTransform, animatedImageButtonState.outlineSizeDelta, m_style.baseButtonStyle.transitionDuration, false));
			}
		}

		public AnimatedGraphicButton()
			: this()
		{
		}
	}
}
