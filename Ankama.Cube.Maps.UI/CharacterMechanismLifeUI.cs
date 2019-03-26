using DG.Tweening;
using DG.Tweening.Core;
using System;
using UnityEngine;

namespace Ankama.Cube.Maps.UI
{
	[ExecuteInEditMode]
	public sealed class CharacterMechanismLifeUI : CharacterUILayoutElement
	{
		private const float IconOverlap = -0.04f;

		private const float TweenDurationFactor = 0.05f;

		[Header("Renderers")]
		[SerializeField]
		private SpriteRenderer m_iconRenderer;

		[SerializeField]
		private SpriteTextRenderer m_valueRenderer;

		private int m_life;

		private int m_currentLife;

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
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				m_color = value;
				m_iconRenderer.set_color(value);
				m_valueRenderer.color = value;
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
				m_iconRenderer.set_sortingOrder(sortingOrder);
				m_valueRenderer.sortingOrder = sortingOrder;
			}
		}

		public override void SetLayoutPosition(int value)
		{
			if (m_layoutPosition != value)
			{
				float delta = 0.01f * (float)(value - m_layoutPosition);
				CharacterUILayoutElement.LayoutMoveTransform(m_iconRenderer.get_transform(), delta);
				CharacterUILayoutElement.LayoutMoveTransform(m_valueRenderer.get_transform(), delta);
				m_layoutPosition = value;
			}
		}

		public void SetValue(int life)
		{
			if (life != m_life)
			{
				m_life = life;
				m_currentLife = life;
				if (m_tweener != null)
				{
					TweenExtensions.Kill(m_tweener, false);
					m_tweener = null;
				}
				Render();
			}
		}

		public unsafe void ChangeValue(int life)
		{
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Expected O, but got Unknown
			if (life != m_life)
			{
				if (m_tweener != null)
				{
					TweenExtensions.Kill(m_tweener, false);
					m_tweener = null;
				}
				int num = Math.Abs(life - m_currentLife);
				if (num != 0)
				{
					float num2 = (float)num * 0.05f;
					m_tweener = TweenSettingsExtensions.OnComplete<Tweener>(DOTween.To(new DOGetter<int>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<int>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), life, num2), new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
				}
				m_life = life;
			}
		}

		private void Render()
		{
			m_valueRenderer.text = m_currentLife.ToString();
			Layout();
		}

		protected override void Layout()
		{
			float num = 0.01f * (float)m_layoutPosition;
			num += CharacterUILayoutElement.LayoutSetTransform(m_iconRenderer, num) + -0.04f;
			num += CharacterUILayoutElement.LayoutSetTransform(m_valueRenderer, num);
			base.layoutWidth = Mathf.CeilToInt(100f * num) - m_layoutPosition;
			base.Layout();
		}

		private int LifeTweenGetter()
		{
			return m_currentLife;
		}

		private void LifeTweenSetter(int value)
		{
			m_currentLife = value;
			Render();
		}

		private void OnLifeTweenComplete()
		{
			m_tweener = null;
		}

		private void OnEnable()
		{
			m_iconRenderer.set_enabled(true);
			m_valueRenderer.set_enabled(true);
			if (m_currentLife != m_life)
			{
				m_currentLife = m_life;
				Render();
			}
			else
			{
				Layout();
			}
		}

		private void OnDisable()
		{
			if (m_tweener != null)
			{
				TweenExtensions.Kill(m_tweener, false);
				m_tweener = null;
			}
			m_iconRenderer.set_enabled(false);
			m_valueRenderer.set_enabled(false);
			base.Layout();
		}
	}
}
