using DG.Tweening;
using JetBrains.Annotations;
using System;
using UnityEngine;

namespace Ankama.Cube.Maps.Feedbacks
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class CellHighlight : MonoBehaviour
	{
		private SpriteRenderer m_spriteRenderer;

		private Quaternion m_originalRotation;

		private Tweener m_tweener;

		private bool m_shown;

		public void Initialize(Material material, uint renderLayerMask)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			SpriteRenderer component = this.GetComponent<SpriteRenderer>();
			component.set_sharedMaterial(material);
			component.set_color(new Color(1f, 1f, 1f, 0f));
			component.set_renderingLayerMask(renderLayerMask);
			m_spriteRenderer = component;
			m_originalRotation = this.get_transform().get_localRotation();
			m_shown = false;
		}

		public void SetSprite([NotNull] Sprite sprite, Color color)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			if (!m_shown)
			{
				Show();
				m_shown = true;
			}
			this.get_transform().set_localRotation(m_originalRotation);
			m_spriteRenderer.set_sprite(sprite);
			m_spriteRenderer.set_color(color);
		}

		public void SetSprite([NotNull] Sprite sprite, Color color, float angle)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			if (!m_shown)
			{
				Show();
				m_shown = true;
			}
			this.get_transform().set_localRotation(m_originalRotation * Quaternion.AngleAxis(angle, Vector3.get_forward()));
			m_spriteRenderer.set_sprite(sprite);
			m_spriteRenderer.set_color(color);
		}

		public void ClearSprite()
		{
			if (m_shown)
			{
				Hide();
				m_shown = false;
			}
		}

		private unsafe void Show()
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected O, but got Unknown
			Tweener tweener = m_tweener;
			if (tweener != null)
			{
				TweenExtensions.Kill(tweener, false);
			}
			m_tweener = TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(DOTweenModuleSprite.DOFade(m_spriteRenderer, 1f, 7f / 60f), 5), new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_spriteRenderer.set_enabled(true);
		}

		private unsafe void Hide()
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected O, but got Unknown
			Tweener tweener = m_tweener;
			if (tweener != null)
			{
				TweenExtensions.Kill(tweener, false);
			}
			m_tweener = TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(DOTweenModuleSprite.DOFade(m_spriteRenderer, 0f, 355f / (678f * MathF.PI)), 6), new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		}

		private void OnShowTweenerComplete()
		{
			m_tweener = null;
		}

		private void OnHideTweenerComplete()
		{
			m_spriteRenderer.set_enabled(false);
			m_spriteRenderer.set_sprite(null);
			m_tweener = null;
		}

		public CellHighlight()
			: this()
		{
		}
	}
}
