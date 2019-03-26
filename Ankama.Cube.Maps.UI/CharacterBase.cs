using Ankama.Cube.Fight;
using Ankama.Cube.Fight.Entities;
using System;
using UnityEngine;

namespace Ankama.Cube.Maps.UI
{
	public sealed class CharacterBase : MonoBehaviour, ICharacterUI
	{
		public enum State
		{
			NotPlayable,
			ActionUsed,
			ActionAvailable
		}

		public enum TargetState
		{
			None,
			Targetable,
			Targeted
		}

		[SerializeField]
		private CharacterBaseFeedbackResources m_resources;

		[SerializeField]
		private SpriteRenderer m_renderer;

		private Color m_color = Color.get_white();

		private int m_sortingOrder;

		private State m_state;

		private TargetState m_targetState;

		private Color m_stateColor = Color.get_white();

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
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				m_color = value;
				m_renderer.set_color(value * m_stateColor);
			}
		}

		public int sortingOrder
		{
			get
			{
				return m_sortingOrder;
			}
			set
			{
				m_sortingOrder = value;
				m_renderer.set_sortingOrder(sortingOrder);
			}
		}

		public void Setup(PlayerType playerType)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			m_stateColor = m_resources.feedbackColors.GetPlayerColor(playerType);
			Refresh();
		}

		public void InitializeState(FightStatus fightStatus, CharacterStatus characterStatus, PlayerStatus ownerStatus)
		{
			if (fightStatus.currentTurnPlayerId != ownerStatus.id)
			{
				SetState(State.NotPlayable);
			}
			else
			{
				SetState(characterStatus.actionUsed ? State.ActionUsed : State.ActionAvailable);
			}
		}

		public void SetState(State state)
		{
			if (state != m_state)
			{
				m_state = state;
				Refresh();
			}
		}

		public void SetTargetState(TargetState state)
		{
			if (state != m_targetState)
			{
				m_targetState = state;
				Refresh();
			}
		}

		private void Refresh()
		{
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			CharacterBaseFeedbackResources resources = m_resources;
			Sprite sprite;
			float a;
			switch (m_targetState)
			{
			case TargetState.None:
				sprite = resources.baseSprite;
				switch (m_state)
				{
				case State.NotPlayable:
					a = resources.notPlayableAlpha;
					break;
				case State.ActionUsed:
					a = resources.actionUsedAlpha;
					break;
				case State.ActionAvailable:
					a = resources.actionAvailableAlpha;
					break;
				default:
					throw new ArgumentOutOfRangeException("m_state", m_state, null);
				}
				break;
			case TargetState.Targetable:
				sprite = resources.attackedSprite;
				a = 1f;
				break;
			case TargetState.Targeted:
				sprite = resources.attackedSprite;
				a = 1f;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			Color stateColor = m_stateColor;
			stateColor.a = a;
			m_renderer.set_sprite(sprite);
			m_renderer.set_color(m_color * stateColor);
			m_stateColor = stateColor;
		}

		public CharacterBase()
			: this()
		{
		}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)

	}
}
