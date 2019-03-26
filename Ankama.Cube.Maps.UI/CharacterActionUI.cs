using Ankama.Cube.Data;
using DG.Tweening;
using DG.Tweening.Core;
using JetBrains.Annotations;
using System;
using UnityEngine;

namespace Ankama.Cube.Maps.UI
{
	[ExecuteInEditMode]
	public sealed class CharacterActionUI : CharacterUILayoutElement
	{
		private const float IconOverlap = -0.04f;

		private const float TweenDurationFactor = 0.05f;

		[Header("Resources")]
		[UsedImplicitly]
		[SerializeField]
		private CharacterUIResources m_resources;

		[Header("Renderers")]
		[UsedImplicitly]
		[SerializeField]
		private SpriteRenderer m_actionIconRenderer;

		[UsedImplicitly]
		[SerializeField]
		private SpriteTextRenderer m_actionValueRenderer;

		private int m_value;

		private int m_currentValue;

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
				m_actionIconRenderer.set_color(value);
				m_actionValueRenderer.color = value;
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
				m_actionIconRenderer.set_sortingOrder(sortingOrder);
				m_actionValueRenderer.sortingOrder = sortingOrder;
			}
		}

		public override void SetLayoutPosition(int value)
		{
			if (m_layoutPosition != value)
			{
				float delta = 0.01f * (float)(value - m_layoutPosition);
				CharacterUILayoutElement.LayoutMoveTransform(m_actionIconRenderer.get_transform(), delta);
				CharacterUILayoutElement.LayoutMoveTransform(m_actionValueRenderer.get_transform(), delta);
				m_layoutPosition = value;
			}
		}

		public void Setup(ActionType actionType, bool ranged)
		{
			switch (actionType)
			{
			case ActionType.None:
				m_actionIconRenderer.set_sprite(null);
				m_actionIconRenderer.set_enabled(false);
				m_actionValueRenderer.set_enabled(false);
				break;
			case ActionType.Attack:
			{
				bool enabled2 = this.get_enabled();
				m_actionIconRenderer.set_sprite(ranged ? m_resources.actionRangedAttackIcon : m_resources.actionAttackIcon);
				m_actionIconRenderer.set_enabled(enabled2);
				m_actionValueRenderer.set_enabled(enabled2);
				break;
			}
			case ActionType.Heal:
			{
				bool enabled = this.get_enabled();
				m_actionIconRenderer.set_sprite(ranged ? m_resources.actionRangedHealIcon : m_resources.actionHealIcon);
				m_actionIconRenderer.set_enabled(enabled);
				m_actionValueRenderer.set_enabled(enabled);
				break;
			}
			default:
				throw new ArgumentOutOfRangeException("actionType", actionType, null);
			}
		}

		public void SetValue(int value)
		{
			if (m_value != value)
			{
				m_value = value;
				m_currentValue = value;
				if (m_tweener != null)
				{
					TweenExtensions.Kill(m_tweener, false);
					m_tweener = null;
				}
				Render();
			}
		}

		public unsafe void ChangeValue(int value)
		{
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Expected O, but got Unknown
			if (value != m_value)
			{
				if (m_tweener != null)
				{
					TweenExtensions.Kill(m_tweener, false);
					m_tweener = null;
				}
				int num = Math.Abs(value - m_currentValue);
				if (num != 0)
				{
					float num2 = (float)num * 0.05f;
					m_tweener = TweenSettingsExtensions.OnComplete<Tweener>(DOTween.To(new DOGetter<int>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<int>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), value, num2), new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
				}
				m_value = value;
			}
		}

		private void Render()
		{
			m_actionValueRenderer.text = m_currentValue.ToString();
			Layout();
		}

		protected override void Layout()
		{
			float num = 0.01f * (float)m_layoutPosition;
			num += CharacterUILayoutElement.LayoutSetTransform(m_actionIconRenderer, num) + -0.04f;
			num += CharacterUILayoutElement.LayoutSetTransform(m_actionValueRenderer, num);
			base.layoutWidth = Mathf.CeilToInt(100f * num) - m_layoutPosition;
			base.Layout();
		}

		private int ValueTweenGetter()
		{
			return m_currentValue;
		}

		private void ValueTweenSetter(int value)
		{
			m_currentValue = value;
			Render();
		}

		private void OnTweenComplete()
		{
			m_tweener = null;
		}

		private void OnEnable()
		{
			bool enabled = m_actionIconRenderer.get_sprite() != null;
			m_actionIconRenderer.set_enabled(enabled);
			m_actionValueRenderer.set_enabled(enabled);
			if (m_currentValue != m_value)
			{
				m_currentValue = m_value;
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
			m_actionIconRenderer.set_enabled(false);
			m_actionValueRenderer.set_enabled(false);
			base.Layout();
		}
	}
}
