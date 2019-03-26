using Ankama.Cube.Data;
using JetBrains.Annotations;
using System;
using UnityEngine;

namespace Ankama.Cube.Maps.UI
{
	public sealed class CharacterAttackableUI : MonoBehaviour, ICharacterUI
	{
		[Header("Resources")]
		[UsedImplicitly]
		[SerializeField]
		private CharacterUIResources m_resources;

		[Header("Renderers")]
		[UsedImplicitly]
		[SerializeField]
		private SpriteRenderer m_feedbackIconRenderer;

		public Color color
		{
			get
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				return m_feedbackIconRenderer.get_color();
			}
			set
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				m_feedbackIconRenderer.set_color(value);
			}
		}

		public int sortingOrder
		{
			get
			{
				return m_feedbackIconRenderer.get_sortingOrder();
			}
			set
			{
				m_feedbackIconRenderer.set_sortingOrder(value);
			}
		}

		public void Setup()
		{
			m_feedbackIconRenderer.set_enabled(false);
			m_feedbackIconRenderer.set_sprite(null);
		}

		public void SetValue(ActionType actionType, bool selected)
		{
			switch (actionType)
			{
			case ActionType.None:
				m_feedbackIconRenderer.set_enabled(false);
				m_feedbackIconRenderer.set_sprite(null);
				break;
			case ActionType.Attack:
				m_feedbackIconRenderer.set_sprite(selected ? m_resources.attackTargetSelectedFeedbackIcon : m_resources.attackTargetFeedbackIcon);
				m_feedbackIconRenderer.set_enabled(true);
				break;
			case ActionType.Heal:
				m_feedbackIconRenderer.set_sprite(selected ? m_resources.healTargetSelectedFeedbackIcon : m_resources.healTargetFeedbackIcon);
				m_feedbackIconRenderer.set_enabled(true);
				break;
			default:
				throw new ArgumentOutOfRangeException("actionType", actionType, null);
			}
		}

		public CharacterAttackableUI()
			: this()
		{
		}
	}
}
