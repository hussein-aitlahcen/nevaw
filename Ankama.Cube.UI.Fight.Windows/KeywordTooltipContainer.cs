using Ankama.Cube.Data;
using Ankama.Cube.Data.UI.Localization.TextFormatting.ValueProviders.FightValueProviders;
using Ankama.Cube.Fight.Entities;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Fight.Windows
{
	public class KeywordTooltipContainer : MonoBehaviour
	{
		public enum VerticalAlignment
		{
			Up,
			Down
		}

		public enum HorizontalAlignment
		{
			Left,
			Right
		}

		[SerializeField]
		protected TooltipWindowParameters m_parameters;

		[SerializeField]
		private KeywordTooltip m_tooltipTemplate;

		[SerializeField]
		private CanvasGroup m_canvasGroup;

		[Header("Layout")]
		[SerializeField]
		private float m_spacing;

		[SerializeField]
		private VerticalAlignment m_verticalAlignment;

		[SerializeField]
		private HorizontalAlignment m_horizontalAlignment;

		[SerializeField]
		private float m_openingDuration;

		[SerializeField]
		private float m_openingDelay;

		[SerializeField]
		private float m_nextOpeningDelay = 0.1f;

		private bool m_opening;

		private Tween m_tweenAlpha;

		private readonly List<Tween> m_openingTweens = new List<Tween>();

		private readonly List<KeywordTooltip> m_activeTooltips = new List<KeywordTooltip>();

		private readonly Stack<KeywordTooltip> m_tooltipPool = new Stack<KeywordTooltip>();

		public float width
		{
			get
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				Rect rect = this.get_transform().get_rect();
				return rect.get_width();
			}
		}

		public float spacing => m_spacing;

		public void SetAlignement(HorizontalAlignment h, VerticalAlignment v)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			m_horizontalAlignment = h;
			m_verticalAlignment = v;
			Vector2 anchoredPosition = default(Vector2);
			anchoredPosition._002Ector(0f, 0f);
			Vector2 zero = Vector2.get_zero();
			Vector2 val = default(Vector2);
			val._002Ector(1f, 0f);
			switch (h)
			{
			case HorizontalAlignment.Left:
				anchoredPosition.x = 0f - m_spacing;
				zero.x = 1f;
				val.x = 0f;
				break;
			case HorizontalAlignment.Right:
				anchoredPosition.x = m_spacing;
				zero.x = 0f;
				val.x = 1f;
				break;
			default:
				throw new ArgumentOutOfRangeException("h", h, null);
			}
			switch (v)
			{
			case VerticalAlignment.Up:
				zero.y = 1f;
				val.y = 1f;
				break;
			case VerticalAlignment.Down:
				zero.y = 0f;
				val.y = 0f;
				break;
			default:
				throw new ArgumentOutOfRangeException("v", v, null);
			}
			_003F val2 = this.get_transform();
			val2.set_pivot(zero);
			val2.set_anchorMin(val);
			val2.set_anchorMax(val);
			val2.set_anchoredPosition(anchoredPosition);
		}

		public void Initialize(ITooltipDataProvider dataProvider)
		{
			RemoveAllTooltip();
			IFightValueProvider valueProvider = dataProvider.GetValueProvider();
			KeywordReference[] keywordReferences = dataProvider.keywordReferences;
			if (keywordReferences == null)
			{
				return;
			}
			for (int num = keywordReferences.Length - 1; num >= 0; num--)
			{
				KeywordReference keywordReference = keywordReferences[num];
				if (keywordReference.IsValidFor(RuntimeData.currentKeywordContext))
				{
					ITooltipDataProvider tooltipDataProvider = TooltipDataProviderFactory.Create(keywordReference, valueProvider);
					if (tooltipDataProvider != null)
					{
						KeywordTooltip tooltip = GetTooltip();
						tooltip.Initialize(tooltipDataProvider);
						m_activeTooltips.Add(tooltip);
					}
				}
			}
		}

		public void Show()
		{
			StopTweens();
			if (m_activeTooltips.Count == 0)
			{
				this.get_gameObject().SetActive(false);
				return;
			}
			this.get_gameObject().SetActive(true);
			TweenAlphaSetter(1f);
			m_opening = true;
		}

		public unsafe void LateUpdate()
		{
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			if (m_opening)
			{
				m_opening = false;
				int num = 0;
				int num2 = 0;
				switch (m_verticalAlignment)
				{
				case VerticalAlignment.Up:
					num = 1;
					num2 = 1;
					break;
				case VerticalAlignment.Down:
					num = 0;
					num2 = 0;
					break;
				default:
					throw new ArgumentOutOfRangeException("m_verticalAlignment", m_verticalAlignment, null);
				}
				float num3 = 0f;
				float num4 = 0f;
				float num5 = (m_verticalAlignment != 0) ? 1 : (-1);
				for (int i = 0; i < m_activeTooltips.Count; i++)
				{
					float num6 = m_nextOpeningDelay * (float)i + m_openingDelay;
					KeywordTooltip tooltip = m_activeTooltips[i];
					RectTransform component = tooltip.GetComponent<RectTransform>();
					LayoutRebuilder.ForceRebuildLayoutImmediate(component);
					Rect rect = component.get_rect();
					float height = rect.get_height();
					Vector2 pivot = component.get_pivot();
					pivot.y = num;
					component.set_pivot(pivot);
					Vector2 val = component.get_anchorMin();
					val.y = num2;
					component.set_anchorMin(val);
					val = component.get_anchorMax();
					val.y = num2;
					component.set_anchorMax(val);
					component.set_anchoredPosition(new Vector2(0f, num4));
					tooltip.alpha = 0f;
					m_openingTweens.Add(TweenSettingsExtensions.SetDelay<Tweener>(DOTweenModuleUI.DOAnchorPos(component, new Vector2(0f, num3), m_openingDuration, false), num6));
					_003C_003Ec__DisplayClass23_0 _003C_003Ec__DisplayClass23_;
					m_openingTweens.Add(TweenSettingsExtensions.SetDelay<TweenerCore<float, float, FloatOptions>>(DOTween.To(new DOGetter<float>((object)_003C_003Ec__DisplayClass23_, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<float>((object)_003C_003Ec__DisplayClass23_, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), 1f, m_openingDuration), num6));
					num4 = num3;
					num3 += num5 * (height + m_spacing);
				}
			}
		}

		public unsafe void Hide()
		{
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Expected O, but got Unknown
			StopTweens();
			TooltipWindowParameters parameters = m_parameters;
			m_tweenAlpha = TweenSettingsExtensions.OnComplete<TweenerCore<float, float, FloatOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(new DOGetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), 0f, TweenAlphaGetter() * parameters.closeDuration), parameters.closeEase), new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		}

		private float TweenAlphaGetter()
		{
			return m_canvasGroup.get_alpha();
		}

		private void TweenAlphaSetter(float value)
		{
			m_canvasGroup.set_alpha(value);
		}

		private void HideCompleteCallback()
		{
			this.get_gameObject().SetActive(false);
			RemoveAllTooltip();
		}

		private void StopTweens()
		{
			Tween tweenAlpha = m_tweenAlpha;
			if (tweenAlpha != null)
			{
				TweenExtensions.Kill(tweenAlpha, false);
			}
			m_tweenAlpha = null;
			for (int i = 0; i < m_openingTweens.Count; i++)
			{
				TweenExtensions.Kill(m_openingTweens[i], false);
			}
			m_openingTweens.Clear();
		}

		private void RemoveAllTooltip()
		{
			for (int i = 0; i < m_activeTooltips.Count; i++)
			{
				ReleaseTooltip(m_activeTooltips[i]);
			}
			m_activeTooltips.Clear();
		}

		private KeywordTooltip GetTooltip()
		{
			KeywordTooltip keywordTooltip = (m_tooltipPool.Count <= 0) ? Object.Instantiate<KeywordTooltip>(m_tooltipTemplate, this.get_transform()) : m_tooltipPool.Pop();
			keywordTooltip.get_gameObject().SetActive(true);
			return keywordTooltip;
		}

		private void ReleaseTooltip(KeywordTooltip keywordTooltip)
		{
			keywordTooltip.get_gameObject().SetActive(false);
			m_tooltipPool.Push(keywordTooltip);
		}

		public KeywordTooltipContainer()
			: this()
		{
		}
	}
}
