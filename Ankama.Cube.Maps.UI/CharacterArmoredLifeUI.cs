using DG.Tweening;
using DG.Tweening.Core;
using System;
using UnityEngine;

namespace Ankama.Cube.Maps.UI
{
	[ExecuteInEditMode]
	public sealed class CharacterArmoredLifeUI : CharacterUILayoutElement
	{
		private const float IconOverlap = -0.04f;

		private const float TweenDurationFactor = 0.05f;

		[Header("Renderers")]
		[SerializeField]
		private SpriteRenderer m_lifeIconRenderer;

		[SerializeField]
		private SpriteTextRenderer m_lifeValueRenderer;

		[SerializeField]
		private SpriteRenderer m_armorIconRenderer;

		[SerializeField]
		private SpriteTextRenderer m_armorValueRenderer;

		[Header("Layout")]
		[SerializeField]
		private int m_layoutSpacing = 1;

		private int m_life;

		private int m_armor;

		private int m_currentLife;

		private int m_currentArmor;

		private int m_maximumLife;

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
				//IL_0025: Unknown result type (might be due to invalid IL or missing references)
				//IL_0031: Unknown result type (might be due to invalid IL or missing references)
				m_color = value;
				m_lifeIconRenderer.set_color(value);
				m_lifeValueRenderer.color = value;
				m_armorIconRenderer.set_color(value);
				m_armorValueRenderer.color = value;
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
				m_lifeIconRenderer.set_sortingOrder(sortingOrder);
				m_lifeValueRenderer.sortingOrder = sortingOrder;
				m_armorIconRenderer.set_sortingOrder(sortingOrder);
				m_armorValueRenderer.sortingOrder = sortingOrder;
			}
		}

		public override void SetLayoutPosition(int value)
		{
			if (m_layoutPosition != value)
			{
				float delta = 0.01f * (float)(value - m_layoutPosition);
				CharacterUILayoutElement.LayoutMoveTransform(m_lifeIconRenderer.get_transform(), delta);
				CharacterUILayoutElement.LayoutMoveTransform(m_lifeValueRenderer.get_transform(), delta);
				CharacterUILayoutElement.LayoutMoveTransform(m_armorIconRenderer.get_transform(), delta);
				CharacterUILayoutElement.LayoutMoveTransform(m_armorValueRenderer.get_transform(), delta);
				m_layoutPosition = value;
			}
		}

		public void Setup(int maximumLife)
		{
			m_maximumLife = maximumLife;
		}

		public void SetMaximumLife(int maximumLife)
		{
			if (maximumLife != m_maximumLife)
			{
				m_maximumLife = maximumLife;
				Render();
			}
		}

		public void SetValues(int life, int armor)
		{
			if (life != m_life || armor != m_armor)
			{
				m_life = life;
				m_currentLife = life;
				m_armor = armor;
				m_currentArmor = armor;
				bool enabled = armor > 0 && this.get_enabled();
				m_armorIconRenderer.set_enabled(enabled);
				m_armorValueRenderer.set_enabled(enabled);
				if (m_tweener != null)
				{
					TweenExtensions.Kill(m_tweener, false);
					m_tweener = null;
				}
				Render();
			}
		}

		public unsafe void ChangeValues(int life, int armor)
		{
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Expected O, but got Unknown
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Expected O, but got Unknown
			if (life == m_life && armor == m_armor)
			{
				return;
			}
			if (m_tweener != null)
			{
				TweenExtensions.Kill(m_tweener, false);
				m_tweener = null;
			}
			int num = Math.Abs(armor - m_currentArmor);
			if (num != 0)
			{
				if (armor > 0)
				{
					bool enabled = this.get_enabled();
					m_armorIconRenderer.set_enabled(enabled);
					m_armorValueRenderer.set_enabled(enabled);
				}
				float num2 = (float)num * 0.05f;
				m_tweener = TweenSettingsExtensions.OnComplete<Tweener>(DOTween.To(new DOGetter<int>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<int>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), armor, num2), new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			}
			else
			{
				int num3 = Math.Abs(life - m_currentLife);
				if (num3 != 0)
				{
					float num4 = (float)num3 * 0.05f;
					m_tweener = TweenSettingsExtensions.OnComplete<Tweener>(DOTween.To(new DOGetter<int>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<int>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), life, num4), new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
				}
			}
			m_life = life;
			m_armor = armor;
		}

		private void Render()
		{
			int currentLife = m_currentLife;
			m_lifeValueRenderer.text = currentLife.ToString();
			int currentArmor = m_currentArmor;
			m_armorValueRenderer.text = currentArmor.ToString();
			Layout();
		}

		protected override void Layout()
		{
			float num = 0.01f * (float)m_layoutPosition;
			num += CharacterUILayoutElement.LayoutSetTransform(m_lifeIconRenderer, num) + -0.04f;
			num += CharacterUILayoutElement.LayoutSetTransform(m_lifeValueRenderer, num);
			if (m_currentArmor > 0)
			{
				num += 0.01f * (float)m_layoutSpacing;
				num += CharacterUILayoutElement.LayoutSetTransform(m_armorIconRenderer, num) + -0.04f;
				num += CharacterUILayoutElement.LayoutSetTransform(m_armorValueRenderer, num);
			}
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

		private int ArmorTweenGetter()
		{
			return m_currentArmor;
		}

		private void ArmorTweenSetter(int value)
		{
			m_currentArmor = value;
			Render();
		}

		private unsafe void OnArmorTweenComplete()
		{
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Expected O, but got Unknown
			bool enabled = m_currentArmor > 0 && this.get_enabled();
			m_armorIconRenderer.set_enabled(enabled);
			m_armorValueRenderer.set_enabled(enabled);
			if (m_currentLife != m_life)
			{
				float num = (float)Math.Abs(m_life - m_life) * 0.05f;
				m_tweener = TweenSettingsExtensions.OnComplete<Tweener>(DOTween.To(new DOGetter<int>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<int>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), m_life, num), new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			}
			else
			{
				m_tweener = null;
			}
		}

		private void OnEnable()
		{
			bool enabled = m_armor > 0;
			m_lifeIconRenderer.set_enabled(true);
			m_lifeValueRenderer.set_enabled(true);
			m_armorValueRenderer.set_enabled(enabled);
			m_armorIconRenderer.set_enabled(enabled);
			if (m_life != m_currentLife || m_armor != m_currentArmor)
			{
				m_currentLife = m_life;
				m_currentArmor = m_armor;
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
			m_lifeIconRenderer.set_enabled(false);
			m_lifeValueRenderer.set_enabled(false);
			m_armorValueRenderer.set_enabled(false);
			m_armorIconRenderer.set_enabled(false);
			base.Layout();
		}
	}
}
