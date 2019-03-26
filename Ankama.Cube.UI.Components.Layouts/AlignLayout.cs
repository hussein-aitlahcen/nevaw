using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Components.Layouts
{
	[ExecuteInEditMode]
	public class AlignLayout : MonoBehaviour
	{
		[SerializeField]
		[UsedImplicitly]
		protected float m_spacing;

		[SerializeField]
		[UsedImplicitly]
		protected Edge m_direction;

		[SerializeField]
		[UsedImplicitly]
		protected bool m_reverseOrder;

		private bool m_dirty;

		private RectTransform m_rect;

		protected RectTransform rectTransform
		{
			get
			{
				if (m_rect == null)
				{
					m_rect = this.GetComponent<RectTransform>();
				}
				return m_rect;
			}
		}

		private void Refresh()
		{
			int childCount = this.get_transform().get_childCount();
			float offset = 0f;
			if (m_reverseOrder)
			{
				for (int num = childCount - 1; num >= 0; num--)
				{
					SetChildPosition(num, ref offset);
				}
			}
			else
			{
				for (int i = 0; i < childCount; i++)
				{
					SetChildPosition(i, ref offset);
				}
			}
		}

		private void SetChildPosition(int childIndex, ref float offset)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected I4, but got Unknown
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			Transform child = this.get_transform().GetChild(childIndex);
			if (child.GetComponent<ILayoutIgnorer>() == null)
			{
				RectTransform component = child.GetComponent<RectTransform>();
				Vector3 localPosition = component.get_localPosition();
				Edge direction = m_direction;
				Rect rect;
				float num;
				switch ((int)direction)
				{
				case 0:
					rect = component.get_rect();
					num = rect.get_width();
					localPosition.x = 0f - offset;
					localPosition.y = 0f;
					break;
				case 1:
					rect = component.get_rect();
					num = rect.get_width();
					localPosition.x = offset;
					localPosition.y = 0f;
					break;
				case 2:
					rect = component.get_rect();
					num = rect.get_height();
					localPosition.x = 0f;
					localPosition.y = offset;
					break;
				case 3:
					rect = component.get_rect();
					num = rect.get_height();
					localPosition.x = 0f;
					localPosition.y = 0f - offset;
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
				component.set_localPosition(localPosition);
				offset += num + m_spacing;
			}
		}

		private void Update()
		{
			Refresh();
		}

		public AlignLayout()
			: this()
		{
		}
	}
}
