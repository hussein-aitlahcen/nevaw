using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Maps
{
	[RequireComponent(typeof(SpriteRenderer))]
	public sealed class CellPointer : MonoBehaviour
	{
		private const float AnimationAlpha = 0.25f;

		private const float AnimationPeriod = 0.5f;

		private static Tweener s_animationTween;

		private static float s_animatedAlpha = 1f;

		private static List<CellPointer> s_animatedPointers;

		[SerializeField]
		[HideInInspector]
		private SpriteRenderer m_spriteRenderer;

		private bool m_animated;

		public unsafe static void Initialize()
		{
			s_animatedAlpha = 1f;
			s_animationTween = TweenSettingsExtensions.SetLoops<TweenerCore<float, float, FloatOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(new DOGetter<float>((object)null, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<float>((object)null, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), 0.25f, 0.5f), 10), -1, 1);
			s_animatedPointers = new List<CellPointer>();
		}

		public static void Release()
		{
			if (s_animationTween != null)
			{
				TweenExtensions.Kill(s_animationTween, false);
				s_animationTween = null;
			}
			if (s_animatedPointers != null)
			{
				s_animatedPointers.Clear();
				s_animatedPointers = null;
			}
		}

		public void Initialize(Material material, uint renderLayerMask)
		{
			m_spriteRenderer = this.GetComponent<SpriteRenderer>();
			m_spriteRenderer.set_sharedMaterial(material);
			m_spriteRenderer.set_enabled(false);
			m_spriteRenderer.set_renderingLayerMask(renderLayerMask);
		}

		public void SetSprite(Sprite sprite)
		{
			m_spriteRenderer.set_sprite(sprite);
		}

		public void SetColor(Color color)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			m_spriteRenderer.set_color(color);
		}

		public void Show()
		{
			m_spriteRenderer.set_enabled(true);
		}

		public void Hide()
		{
			m_spriteRenderer.set_enabled(false);
			SetAnimated(value: false);
		}

		public void SetAnimated(bool value)
		{
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			if (m_animated == value)
			{
				return;
			}
			if (value)
			{
				s_animatedPointers.Add(this);
				if (!TweenExtensions.IsPlaying(s_animationTween))
				{
					TweenExtensions.Restart(s_animationTween, true, -1f);
				}
			}
			else
			{
				s_animatedPointers.Remove(this);
				if (s_animatedPointers.Count == 0)
				{
					TweenExtensions.Pause<Tweener>(s_animationTween);
				}
				m_spriteRenderer.set_color(Color.get_white());
			}
			m_animated = value;
		}

		private static float BlinkerTweenGetter()
		{
			return s_animatedAlpha;
		}

		private static void BlinkerTweenSetter(float value)
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			Color color = default(Color);
			color._002Ector(1f, 1f, 1f, value);
			List<CellPointer> list = s_animatedPointers;
			int count = list.Count;
			for (int i = 0; i < count; i++)
			{
				list[i].SetColor(color);
			}
			s_animatedAlpha = value;
		}

		private void OnDisable()
		{
			if (s_animatedPointers != null)
			{
				SetAnimated(value: false);
			}
		}

		public CellPointer()
			: this()
		{
		}
	}
}
