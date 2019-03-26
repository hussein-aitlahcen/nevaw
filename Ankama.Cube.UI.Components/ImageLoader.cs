using Ankama.Utilities;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Components
{
	public sealed class ImageLoader : UIResourceLoader<Sprite>
	{
		[Header("Target")]
		[SerializeField]
		private Image m_image;

		[Header("Tween Settings")]
		[SerializeField]
		[Range(0f, 1f)]
		private float m_fadeInTweenDuration = 0.25f;

		[SerializeField]
		[Range(0f, 1f)]
		private float m_fadeOutTweenDuration = 0.15f;

		private TweenerCore<float, float, FloatOptions> m_tween;

		private Color m_color = Color.get_white();

		private float m_alpha = 1f;

		public Color color
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return m_color;
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				m_color = value;
				if (null != m_image)
				{
					Color color = value;
					color.a *= m_alpha;
					m_image.set_color(color);
				}
			}
		}

		private void Awake()
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			if (null != m_image)
			{
				m_color = m_image.get_color();
			}
		}

		protected unsafe override IEnumerator Apply(Sprite sprite, UIResourceDisplayMode displayMode)
		{
			if (null == m_image)
			{
				Log.Warning("No image component has been linked.", 60, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\Components\\ImageLoader.cs");
				yield break;
			}
			if (m_tween != null)
			{
				TweenExtensions.Kill(m_tween, false);
				m_tween = null;
			}
			if (displayMode == UIResourceDisplayMode.Immediate || !m_image.get_gameObject().get_activeInHierarchy())
			{
				m_alpha = 1f;
				m_image.set_color(m_color);
				m_image.set_sprite(sprite);
				m_image.set_enabled(true);
				yield break;
			}
			Sprite sprite2 = m_image.get_sprite();
			if (null != sprite2)
			{
				float num = m_fadeOutTweenDuration * m_alpha;
				if (num > 0f)
				{
					m_tween = TweenSettingsExtensions.OnComplete<TweenerCore<float, float, FloatOptions>>(DOTween.To(new DOGetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), 0f, num), new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
					while (m_tween != null && TweenExtensions.IsPlaying(m_tween))
					{
						yield return null;
					}
				}
			}
			else
			{
				Color color = m_color;
				color.a = 0f;
				m_alpha = 0f;
				m_image.set_color(color);
			}
			m_image.set_enabled(true);
			m_image.set_sprite(sprite);
			m_tween = TweenSettingsExtensions.OnComplete<TweenerCore<float, float, FloatOptions>>(DOTween.To(new DOGetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), 1f, m_fadeInTweenDuration), new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			while (m_tween != null && TweenExtensions.IsPlaying(m_tween))
			{
				yield return null;
			}
		}

		protected unsafe override IEnumerator Revert(UIResourceDisplayMode displayMode)
		{
			if (null == m_image)
			{
				Log.Warning("No image component has been linked.", 119, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\Components\\ImageLoader.cs");
				yield break;
			}
			if (m_tween != null)
			{
				TweenExtensions.Kill(m_tween, false);
				m_tween = null;
			}
			if (displayMode == UIResourceDisplayMode.Immediate || !m_image.get_isActiveAndEnabled())
			{
				Color color = m_color;
				color.a = 0f;
				m_alpha = 0f;
				m_image.set_color(color);
				m_image.set_sprite(null);
				m_image.set_enabled(false);
				yield break;
			}
			Sprite sprite = m_image.get_sprite();
			if (null != sprite)
			{
				float num = m_fadeOutTweenDuration * m_alpha;
				if (num > 0f)
				{
					m_tween = TweenSettingsExtensions.OnComplete<TweenerCore<float, float, FloatOptions>>(DOTween.To(new DOGetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), 0f, num), new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
					while (m_tween != null && !TweenExtensions.IsPlaying(m_tween))
					{
						yield return null;
					}
				}
			}
			else
			{
				Color color2 = m_color;
				color2.a = 0f;
				m_alpha = 0f;
				m_image.set_color(color2);
			}
			m_image.set_sprite(null);
			m_image.set_enabled(false);
		}

		private void TweenSetter(float value)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			m_alpha = value;
			if (null != m_image)
			{
				Color color = m_color;
				color.a *= value;
				m_image.set_color(color);
			}
		}

		private float TweenGetter()
		{
			return m_alpha;
		}

		private void OnTweenComplete()
		{
			m_tween = null;
		}
	}
}
