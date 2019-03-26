using Ankama.Utilities;
using DG.Tweening;
using DG.Tweening.Core;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Fight
{
	public class GaugeItemUI : MonoBehaviour
	{
		[Header("Components")]
		[SerializeField]
		protected UISpriteTextRenderer m_text;

		[SerializeField]
		protected UISpriteTextRenderer m_modificationText;

		[SerializeField]
		protected Image m_image;

		[SerializeField]
		protected ParticleSystem m_upFX;

		[SerializeField]
		protected UISpriteTextRenderer m_upMFXModificationText;

		[Header("Modification")]
		[SerializeField]
		protected GaugePreviewResource m_previewResource;

		protected int m_value;

		private int m_tweenedValue;

		private int m_tweenedEndValue = int.MinValue;

		private int? m_modificationPreviewValue;

		private Tweener m_valueTweener;

		private bool m_highlighted;

		private Tween m_highlightTweener;

		public void SetSprite(Sprite sprite)
		{
			if (m_image != null)
			{
				m_image.set_sprite(sprite);
			}
		}

		public void SetColor(Color color)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			if (m_image != null)
			{
				m_image.set_color(color);
			}
			m_text.set_color(color);
		}

		public void SetAlpha(float alpha)
		{
			if (m_image != null)
			{
				m_image.WithAlpha<Image>(alpha);
			}
			m_text.WithAlpha<UISpriteTextRenderer>(alpha);
		}

		public void DoAlpha(float alpha, float duration)
		{
			if (m_image != null)
			{
				DOTweenModuleUI.DOFade(m_image, alpha, duration);
			}
			DOTweenModuleUI.DOFade(m_text, alpha, duration);
		}

		public void Desaturate(float desaturationFactor)
		{
			if (m_image != null)
			{
				m_image.Desaturate<Image>(desaturationFactor);
			}
		}

		public void Highlight(bool highlight)
		{
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			if (m_highlighted == highlight || m_previewResource == null || !m_previewResource.highlightEnabled)
			{
				return;
			}
			Tween highlightTweener = m_highlightTweener;
			if (highlightTweener != null && TweenExtensions.IsPlaying(highlightTweener))
			{
				TweenExtensions.Kill(m_highlightTweener, true);
				m_highlightTweener = null;
			}
			if (highlight && m_image != null)
			{
				float highlightPunch = m_previewResource.highlightPunch;
				m_highlightTweener = ShortcutExtensions.DOPunchScale(m_image.get_transform(), new Vector3(highlightPunch, highlightPunch, highlightPunch), m_previewResource.highlightDuration, m_previewResource.highlightVibrato, m_previewResource.highlightElasticity);
				int highlightLoopCount = m_previewResource.highlightLoopCount;
				if (highlightLoopCount != 1)
				{
					TweenSettingsExtensions.SetLoops<Tween>(m_highlightTweener, highlightLoopCount);
				}
			}
			m_highlighted = highlight;
		}

		protected int GetRealValue()
		{
			return m_value;
		}

		protected int GetTweenedValue()
		{
			return m_tweenedValue;
		}

		private void SetTweenedValue(int value)
		{
			m_tweenedValue = value;
			UpdateText(value);
		}

		protected virtual void UpdateText(int value)
		{
			m_text.text = m_tweenedValue.ToString();
		}

		public virtual void UpdateMaxValue(int maxValueModification)
		{
		}

		public virtual void SetValue(int v)
		{
			if (v > m_value)
			{
				PlayUpFX(v - m_value);
			}
			m_value = v;
			UpdateTweenedValue();
		}

		private void PlayUpFX(int modificationValue)
		{
			if (m_upFX != null)
			{
				m_upMFXModificationText.text = "+" + modificationValue;
				m_upFX.get_gameObject().SetActive(false);
				m_upFX.get_gameObject().SetActive(true);
				m_upFX.Play();
			}
		}

		private unsafe void UpdateTweenedValue(bool tweening = true)
		{
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			int num = GetRealValue() + (m_modificationPreviewValue ?? 0);
			if (num != m_tweenedEndValue)
			{
				m_tweenedEndValue = num;
				if (m_valueTweener != null && TweenExtensions.IsPlaying(m_valueTweener))
				{
					TweenExtensions.Kill(m_valueTweener, false);
					m_valueTweener = null;
				}
				if (!tweening || null == m_previewResource)
				{
					SetTweenedValue(num);
				}
				else
				{
					m_valueTweener = TweenSettingsExtensions.SetEase<Tweener>(DOTween.To(new DOGetter<int>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<int>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), num, m_previewResource.duration), m_previewResource.ease);
				}
			}
		}

		public void AddModificationPreview(int modification)
		{
			m_modificationPreviewValue = modification;
			UpdateTweenedValue();
			Highlight(highlight: true);
			if (m_modificationText != null && m_previewResource != null && m_previewResource.displayText)
			{
				m_modificationText.text = ToStringExtensions.ToStringSigned(modification);
				m_modificationText.get_gameObject().SetActive(true);
			}
		}

		public void RemoveModificationPreview()
		{
			if (m_modificationPreviewValue.HasValue)
			{
				m_modificationPreviewValue = null;
				UpdateTweenedValue(tweening: false);
				Highlight(highlight: false);
				if (m_modificationText != null)
				{
					m_modificationText.get_gameObject().SetActive(false);
				}
			}
		}

		public GaugeItemUI()
			: this()
		{
		}
	}
}
