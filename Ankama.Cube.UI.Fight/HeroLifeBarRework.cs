using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps;
using Ankama.Cube.UI.Components;
using DG.Tweening;
using DG.Tweening.Core;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Fight
{
	public sealed class HeroLifeBarRework : MonoBehaviour
	{
		[SerializeField]
		private Image m_imageFill;

		[SerializeField]
		private Image m_background;

		[SerializeField]
		private RawTextField m_text;

		[SerializeField]
		private PointCounterStyleRework m_style;

		[SerializeField]
		private FightMapFeedbackColors m_colors;

		private int m_currentBaseLife;

		private int m_targetBaseLife;

		private Tweener m_baseLifeTween;

		private int m_currentLife;

		private int m_targetLife;

		private Tweener m_lifeTween;

		public unsafe void SetStartLife(int value, PlayerType playerType)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			Color playerColor = m_colors.GetPlayerColor(playerType);
			Color color = playerColor * 0.5f;
			color.a = ((IntPtr)(void*)playerColor).a;
			m_background.set_color(color);
			m_imageFill.set_color(playerColor);
			if (m_targetBaseLife != value || m_targetLife != value)
			{
				if (m_lifeTween != null)
				{
					TweenExtensions.Kill(m_lifeTween, false);
					m_lifeTween = null;
				}
				if (m_baseLifeTween != null)
				{
					TweenExtensions.Kill(m_baseLifeTween, false);
					m_baseLifeTween = null;
				}
				m_currentLife = value;
				m_targetLife = value;
				m_currentBaseLife = value;
				m_targetBaseLife = value;
				Refresh();
			}
		}

		public unsafe void ChangeBaseLife(int value)
		{
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Expected O, but got Unknown
			if (value != m_targetBaseLife)
			{
				float tweenDuration = m_style.GetTweenDuration(m_currentBaseLife, value);
				if (m_baseLifeTween != null)
				{
					m_baseLifeTween.ChangeEndValue((object)value, tweenDuration, false);
				}
				else
				{
					m_baseLifeTween = TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(DOTween.To(new DOGetter<int>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<int>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), value, tweenDuration), m_style.tweenEasing), new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
				}
				m_targetBaseLife = value;
			}
		}

		public unsafe void ChangeLife(int value)
		{
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Expected O, but got Unknown
			if (value != m_targetLife)
			{
				float tweenDuration = m_style.GetTweenDuration(m_currentLife, value);
				if (m_lifeTween != null)
				{
					m_lifeTween.ChangeEndValue((object)value, tweenDuration, false);
				}
				else
				{
					m_lifeTween = TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(DOTween.To(new DOGetter<int>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<int>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), value, tweenDuration), m_style.tweenEasing), new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
				}
				m_targetLife = value;
			}
		}

		private int BaseLifeTweenGetter()
		{
			return m_currentBaseLife;
		}

		private void BaseLifeTweenSetter(int value)
		{
			m_currentBaseLife = value;
			Refresh();
		}

		private void OnBaseLifeTweenComplete()
		{
			m_baseLifeTween = null;
		}

		private int LifeTweenGetter()
		{
			return m_currentLife;
		}

		private void LifeTweenSetter(int value)
		{
			m_currentLife = value;
			Refresh();
		}

		private void OnLifeTweenComplete()
		{
			m_lifeTween = null;
		}

		private void Refresh()
		{
			m_text.SetText($"{m_currentLife} / {m_currentBaseLife}");
			m_imageFill.set_fillAmount((float)m_currentLife / (float)m_currentBaseLife);
		}

		public HeroLifeBarRework()
			: this()
		{
		}
	}
}
