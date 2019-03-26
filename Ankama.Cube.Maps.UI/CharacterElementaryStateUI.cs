using Ankama.Cube.Data;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using JetBrains.Annotations;
using System;
using UnityEngine;

namespace Ankama.Cube.Maps.UI
{
	[ExecuteInEditMode]
	public sealed class CharacterElementaryStateUI : CharacterUILayoutElement
	{
		private const float TweenFullDuration = 0.25f;

		[Header("Resources")]
		[UsedImplicitly]
		[SerializeField]
		private CharacterUIResources m_resources;

		[Header("Renderers")]
		[UsedImplicitly]
		[SerializeField]
		private SpriteRenderer m_elementaryIconRenderer;

		private ElementaryStates m_elementaryState = ElementaryStates.None;

		private ElementaryStates m_currentElementaryState = ElementaryStates.None;

		private float m_alpha;

		private Tweener m_tweener;

		public override Color color
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
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				m_color = value;
				Color color = value;
				color.a *= m_alpha;
				m_elementaryIconRenderer.set_color(color);
			}
		}

		public override int sortingOrder
		{
			get
			{
				return m_sortingOrder;
			}
			set
			{
				m_sortingOrder = value;
				m_elementaryIconRenderer.set_sortingOrder(sortingOrder);
			}
		}

		public override void SetLayoutPosition(int value)
		{
			if (m_layoutPosition != value)
			{
				m_layoutPosition = value;
				Layout();
			}
		}

		public void Setup()
		{
			m_elementaryState = ElementaryStates.None;
			m_currentElementaryState = ElementaryStates.None;
			SetIcon();
			SetAlpha(0f);
		}

		public void SetValue(ElementaryStates value)
		{
			if (value != m_elementaryState)
			{
				m_elementaryState = value;
				m_currentElementaryState = value;
				if (m_tweener != null)
				{
					TweenExtensions.Kill(m_tweener, false);
					m_tweener = null;
				}
				SetIcon();
				SetAlpha((value == ElementaryStates.None) ? 0f : 1f);
			}
		}

		public unsafe void ChangeValue(ElementaryStates value)
		{
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Expected O, but got Unknown
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Expected O, but got Unknown
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Expected O, but got Unknown
			if (value == m_elementaryState)
			{
				return;
			}
			if (m_tweener != null)
			{
				TweenExtensions.Kill(m_tweener, false);
				m_tweener = null;
			}
			m_elementaryState = value;
			if (m_currentElementaryState == ElementaryStates.None)
			{
				m_currentElementaryState = value;
				SetIcon();
				float num = Mathf.Lerp(0.25f, 0f, m_alpha);
				if (num > 0f)
				{
					m_tweener = TweenSettingsExtensions.OnComplete<TweenerCore<float, float, FloatOptions>>(DOTween.To(new DOGetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), 1f, num), new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
				}
			}
			else if (value == ElementaryStates.None)
			{
				float num2 = Mathf.Lerp(0f, 0.25f, m_alpha);
				if (num2 > 0f)
				{
					m_tweener = TweenSettingsExtensions.OnComplete<TweenerCore<float, float, FloatOptions>>(DOTween.To(new DOGetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), 0f, num2), new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
				}
				else
				{
					OnIconChangeTweenComplete();
				}
			}
			else
			{
				float num3 = Mathf.Lerp(0f, 0.25f, m_alpha);
				if (num3 > 0f)
				{
					m_tweener = TweenSettingsExtensions.OnComplete<TweenerCore<float, float, FloatOptions>>(DOTween.To(new DOGetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), 0f, num3), new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
				}
				else
				{
					OnIconSwitchTweenComplete();
				}
			}
		}

		private void SetIcon()
		{
			Sprite val;
			switch (m_currentElementaryState)
			{
			case ElementaryStates.None:
				val = null;
				break;
			case ElementaryStates.Muddy:
				val = m_resources.elementaryStateMuddyIcon;
				break;
			case ElementaryStates.Oiled:
				val = m_resources.elementaryStateOiledIcon;
				break;
			case ElementaryStates.Ventilated:
				val = m_resources.elementaryStateVentilatedIcon;
				break;
			case ElementaryStates.Wet:
				val = m_resources.elementaryStateWetIcon;
				break;
			default:
				throw new ArgumentOutOfRangeException("m_currentElementaryState", m_currentElementaryState, null);
			}
			if (null == val)
			{
				m_elementaryIconRenderer.set_enabled(false);
				m_elementaryIconRenderer.set_sprite(null);
			}
			else
			{
				m_elementaryIconRenderer.set_sprite(val);
				m_elementaryIconRenderer.set_enabled(true);
			}
			Layout();
		}

		protected override void Layout()
		{
			float position = 0.01f * (float)m_layoutPosition;
			position = CharacterUILayoutElement.LayoutSetTransform(m_elementaryIconRenderer, position);
			base.layoutWidth = Mathf.CeilToInt(100f * position);
			base.Layout();
		}

		private void SetAlpha(float value)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			m_alpha = value;
			Color color = this.color;
			color.a *= value;
			m_elementaryIconRenderer.set_color(color);
		}

		private float TweenGetter()
		{
			return m_alpha;
		}

		private unsafe void OnIconSwitchTweenComplete()
		{
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Expected O, but got Unknown
			m_currentElementaryState = m_elementaryState;
			SetIcon();
			m_tweener = TweenSettingsExtensions.OnComplete<TweenerCore<float, float, FloatOptions>>(DOTween.To(new DOGetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), 1f, 0.25f), new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		}

		private void OnIconChangeTweenComplete()
		{
			m_currentElementaryState = m_elementaryState;
			SetIcon();
			m_tweener = null;
		}

		private void OnTweenComplete()
		{
			m_tweener = null;
		}

		private void OnEnable()
		{
			m_elementaryIconRenderer.set_enabled(true);
			Layout();
		}

		private void OnDisable()
		{
			m_elementaryIconRenderer.set_enabled(false);
			base.Layout();
		}

		private void OnDestroy()
		{
			if (m_tweener != null)
			{
				TweenExtensions.Kill(m_tweener, false);
				m_tweener = null;
			}
		}
	}
}
