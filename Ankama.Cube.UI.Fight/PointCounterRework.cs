using DG.Tweening;
using DG.Tweening.Core;
using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Fight
{
	public sealed class PointCounterRework : MonoBehaviour
	{
		[SerializeField]
		private PointCounterStyleRework m_style;

		[SerializeField]
		[UsedImplicitly]
		private Image m_image;

		[SerializeField]
		[UsedImplicitly]
		private UISpriteTextRenderer m_text;

		private int m_currentValue;

		private int m_targetValue;

		private Tweener m_tween;

		private static readonly string[] s_intToString = new string[11]
		{
			"0",
			"1",
			"2",
			"3",
			"4",
			"5",
			"6",
			"7",
			"8",
			"9",
			"10"
		};

		public int targetValue => m_targetValue;

		private void Awake()
		{
			Refresh();
		}

		public void SetValue(int value)
		{
			if (value != m_targetValue)
			{
				if (m_tween != null)
				{
					TweenExtensions.Kill(m_tween, false);
					m_tween = null;
				}
				m_currentValue = value;
				m_targetValue = value;
				Refresh();
			}
		}

		public unsafe void ChangeValue(int value)
		{
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Expected O, but got Unknown
			if (value != m_targetValue)
			{
				float tweenDuration = m_style.GetTweenDuration(m_currentValue, value);
				if (m_tween != null)
				{
					m_tween.ChangeEndValue((object)value, tweenDuration, true);
				}
				else
				{
					m_tween = TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(DOTween.To(new DOGetter<int>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<int>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), value, tweenDuration), m_style.tweenEasing), new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
				}
				m_targetValue = value;
			}
		}

		private int TweenGetter()
		{
			return m_currentValue;
		}

		private void TweenSetter(int value)
		{
			m_currentValue = value;
			Refresh();
		}

		private void OnTweenComplete()
		{
			m_tween = null;
		}

		private void Refresh()
		{
			if (null != m_text)
			{
				if (m_currentValue >= 0 && m_currentValue <= 10)
				{
					m_text.text = s_intToString[m_currentValue];
				}
				else
				{
					m_text.text = m_currentValue.ToString();
				}
			}
		}

		public PointCounterRework()
			: this()
		{
		}
	}
}
