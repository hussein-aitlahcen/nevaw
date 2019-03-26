using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps.Feedbacks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Maps.UI
{
	public class FloorMechanismBase : MonoBehaviour, ICharacterUI
	{
		public enum TargetState
		{
			None,
			Targetable,
			Targeted
		}

		[SerializeField]
		private FloorMechanismBaseFeedbackResources m_resources;

		[SerializeField]
		private SpriteRenderer m_renderer;

		private bool m_alliedWithLocalPlayer;

		private TargetState m_targetState;

		private Color m_color = Color.get_white();

		private int m_sortingOrder;

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
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			m_alliedWithLocalPlayer = playerType.HasFlag(PlayerType.Ally);
			m_color = Color.get_white();
			m_targetState = TargetState.None;
			m_renderer.set_sprite(m_resources.sprites[0]);
			m_renderer.set_color(m_stateColor);
			m_stateColor = m_resources.feedbackColors.GetPlayerColor(playerType);
			Refresh();
		}

		public void SetTargetState(TargetState state)
		{
			if (state != m_targetState)
			{
				m_targetState = state;
				Refresh();
			}
		}

		public void RefreshAssemblage(Vector2Int selfCoords, IEnumerable<Vector2Int> positions)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			int num = 0;
			foreach (Vector2Int position in positions)
			{
				Vector2Int current = position;
				if (current.get_y() == selfCoords.get_y())
				{
					if (current.get_x() == selfCoords.get_x() - 1)
					{
						num |= 1;
					}
					else if (current.get_x() == selfCoords.get_x() + 1)
					{
						num |= 2;
					}
				}
				else if (current.get_x() == selfCoords.get_x())
				{
					if (current.get_y() == selfCoords.get_y() - 1)
					{
						num |= 8;
					}
					else if (current.get_y() == selfCoords.get_y() + 1)
					{
						num |= 4;
					}
				}
			}
			int num2;
			float num3;
			switch (num)
			{
			case 0:
				num2 = 0;
				num3 = 0f;
				break;
			case 1:
				num2 = 1;
				num3 = 90f;
				break;
			case 2:
				num2 = 1;
				num3 = -90f;
				break;
			case 3:
				num2 = 3;
				num3 = 0f;
				break;
			case 4:
				num2 = 1;
				num3 = 180f;
				break;
			case 5:
				num2 = 2;
				num3 = -90f;
				break;
			case 6:
				num2 = 2;
				num3 = 0f;
				break;
			case 7:
				num2 = 4;
				num3 = 180f;
				break;
			case 8:
				num2 = 1;
				num3 = 0f;
				break;
			case 9:
				num2 = 2;
				num3 = 180f;
				break;
			case 10:
				num2 = 2;
				num3 = 90f;
				break;
			case 11:
				num2 = 4;
				num3 = 0f;
				break;
			case 12:
				num2 = 3;
				num3 = -90f;
				break;
			case 13:
				num2 = 4;
				num3 = 90f;
				break;
			case 14:
				num2 = 4;
				num3 = -90f;
				break;
			case 15:
				num2 = 5;
				num3 = 0f;
				break;
			default:
				throw new ArgumentOutOfRangeException("bitSet", num, "Invalid bitSet.");
			}
			m_renderer.set_sprite(m_resources.sprites[num2]);
			m_renderer.get_transform().set_rotation(Quaternion.Euler(90f, num3, 0f));
		}

		private void Refresh()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			m_renderer.set_color(m_color * m_stateColor);
		}

		public FloorMechanismBase()
			: this()
		{
		}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)

	}
}
