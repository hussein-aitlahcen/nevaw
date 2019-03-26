using Ankama.Cube.Fight;
using Ankama.Utilities;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using UnityEngine;

namespace Ankama.Cube.Maps.Feedbacks
{
	public sealed class ValueChangedFeedback : MonoBehaviour
	{
		public enum Type
		{
			Damage,
			Heal,
			Action,
			Movement
		}

		private const float TweenDuration = 1f;

		private const Ease TweenEasing = 9;

		private const float StartHeight = 1.25f;

		private const float MovementHeight = 0.75f;

		private const float AlphaDelay = 0.75f;

		private const float ItemDelay = 0.25f;

		[SerializeField]
		private SpriteTextRenderer m_spriteTextRenderer;

		private bool m_isNegative;

		private Vector3 m_startPosition;

		private Vector3 m_endPosition;

		private Tweener m_tween;

		private float m_tweenValue;

		public static void Launch(int value, Type type, Transform parent)
		{
			int instanceCountInTransform;
			ValueChangedFeedback valueChangedFeedback = FightObjectFactory.CreateValueChangedFeedback(parent, out instanceCountInTransform);
			valueChangedFeedback.SetValue(value, type);
			valueChangedFeedback.StartTween(instanceCountInTransform);
		}

		private void SetValue(int value, Type type)
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			switch (type)
			{
			case Type.Damage:
				m_spriteTextRenderer.color = new Color(1f, 22f / 51f, 22f / 51f);
				break;
			case Type.Heal:
				m_spriteTextRenderer.color = new Color(28f / 51f, 28f / 51f, 1f);
				break;
			case Type.Action:
				m_spriteTextRenderer.color = new Color(1f, 50f / 51f, 0f);
				break;
			case Type.Movement:
				m_spriteTextRenderer.color = new Color(28f / 51f, 227f / 255f, 18f / 85f);
				break;
			default:
				throw new ArgumentOutOfRangeException("type", type, null);
			}
			m_isNegative = (value < 0);
			m_spriteTextRenderer.text = ToStringExtensions.ToStringSigned(value);
		}

		private unsafe void StartTween(int indexInParentTransform)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Expected O, but got Unknown
			this.get_gameObject().SetActive(true);
			Vector3 val = this.get_transform().get_parent().get_position() + new Vector3(0f, 1.25f, 0f);
			Vector3 val2 = val + new Vector3(0f, 0.75f, 0f);
			float num = (float)indexInParentTransform * 0.25f;
			if (m_isNegative)
			{
				this.get_transform().set_position(val2);
				m_startPosition = val2;
				m_endPosition = val;
			}
			else
			{
				this.get_transform().set_position(val);
				m_startPosition = val;
				m_endPosition = val2;
			}
			m_tweenValue = 0f;
			m_tween = TweenSettingsExtensions.OnComplete<TweenerCore<float, float, FloatOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(TweenSettingsExtensions.SetDelay<TweenerCore<float, float, FloatOptions>>(DOTween.To(new DOGetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), 1f, 1f), num), 9), new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		}

		private float TweenGetter()
		{
			return m_tweenValue;
		}

		private void TweenSetter(float value)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			Vector3 position = Vector3.Lerp(m_startPosition, m_endPosition, value);
			Color color = m_spriteTextRenderer.color;
			color.a = Mathf.Lerp(1f, 0f, (value - 0.75f) / 0.25f);
			this.get_transform().set_position(position);
			m_spriteTextRenderer.color = color;
			m_tweenValue = value;
		}

		private void TweenCompleteCallback()
		{
			FightObjectFactory.ReleaseValueChangedFeedback(this.get_gameObject());
			m_tween = null;
		}

		private void OnDestroy()
		{
			if (m_tween != null)
			{
				TweenExtensions.Kill(m_tween, false);
				m_tween = null;
			}
		}

		public ValueChangedFeedback()
			: this()
		{
		}
	}
}
