using Ankama.Utilities;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ankama.Cube.Demo.UI
{
	public class GameSelectionButton : Button
	{
		[SerializeField]
		private GameSelectionButtonStyle m_style;

		[SerializeField]
		private Image m_image;

		[SerializeField]
		private Image m_outline;

		private readonly List<Tweener> m_currentTweens = new List<Tweener>();

		public Action<GameSelectionButton> onPointerEnter;

		public Action<GameSelectionButton> onPointerExit;

		private bool m_delayAnim;

		private bool m_anotherButtonIsHightlighted;

		public bool anotherButtonIsHightlighted
		{
			set
			{
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_002c: Unknown result type (might be due to invalid IL or missing references)
				if (this.get_interactable())
				{
					if (m_anotherButtonIsHightlighted && !value && (int)this.get_currentSelectionState() == 0)
					{
						m_delayAnim = true;
					}
					m_anotherButtonIsHightlighted = value;
					this.DoStateTransition(this.get_currentSelectionState(), false);
					m_delayAnim = false;
				}
			}
		}

		public override void OnPointerEnter(PointerEventData eventData)
		{
			this.OnPointerEnter(eventData);
			if (this.get_interactable())
			{
				onPointerEnter?.Invoke(this);
			}
		}

		public override void OnPointerExit(PointerEventData eventData)
		{
			this.OnPointerExit(eventData);
			if (this.get_interactable())
			{
				onPointerExit?.Invoke(this);
			}
		}

		protected override void DoStateTransition(SelectionState state, bool instant)
		{
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Expected I4, but got Unknown
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_020d: Unknown result type (might be due to invalid IL or missing references)
			if (!this.get_gameObject().get_activeInHierarchy())
			{
				return;
			}
			if (m_style == null)
			{
				Log.Error("AnimatedTextButton " + this.get_name() + " doesn't have a style defined !", 69, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Demo\\Code\\UI\\GameSelection\\GameSelectionButton.cs");
				return;
			}
			GameSelectionButtonState gameSelectionButtonState = m_style.disable;
			switch ((int)state)
			{
			case 0:
				gameSelectionButtonState = ((!m_anotherButtonIsHightlighted) ? m_style.normal : m_style.normalButAnotherIsHighlighted);
				break;
			case 1:
				gameSelectionButtonState = m_style.highlight;
				break;
			case 2:
				gameSelectionButtonState = m_style.pressed;
				break;
			case 3:
				gameSelectionButtonState = m_style.disable;
				break;
			}
			if (instant)
			{
				if (Object.op_Implicit(m_image))
				{
					m_image.set_color(gameSelectionButtonState.imageColor);
					m_image.get_transform().set_localScale(Vector3.get_one() * gameSelectionButtonState.scale);
				}
				if (Object.op_Implicit(m_outline))
				{
					m_outline.set_color(gameSelectionButtonState.outlineColor);
				}
				return;
			}
			int i = 0;
			for (int count = m_currentTweens.Count; i < count; i++)
			{
				Tweener val = m_currentTweens[i];
				if (TweenExtensions.IsActive(val))
				{
					TweenExtensions.Kill(val, false);
				}
			}
			m_currentTweens.Clear();
			if (Object.op_Implicit(m_image))
			{
				m_currentTweens.Add(DOTweenModuleUI.DOColor(m_image, gameSelectionButtonState.imageColor, m_style.transitionDuration));
				m_currentTweens.Add(ShortcutExtensions.DOScale(m_image.get_transform(), gameSelectionButtonState.scale, m_style.transitionDuration));
			}
			if (Object.op_Implicit(m_outline))
			{
				m_currentTweens.Add(DOTweenModuleUI.DOBlendableColor(m_outline, gameSelectionButtonState.outlineColor, m_style.transitionDuration));
			}
			int j = 0;
			for (int count2 = m_currentTweens.Count; j < count2; j++)
			{
				Tweener val2 = m_currentTweens[j];
				TweenSettingsExtensions.SetEase<Tweener>(val2, m_style.ease);
				if (m_delayAnim)
				{
					TweenSettingsExtensions.SetDelay<Tweener>(val2, m_style.fromNormalAndUnHighlightedDelay);
				}
			}
		}

		public GameSelectionButton()
			: this()
		{
		}
	}
}
