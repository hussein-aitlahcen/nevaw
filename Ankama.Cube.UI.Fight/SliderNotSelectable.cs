using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Fight
{
	[ExecuteInEditMode]
	public class SliderNotSelectable : MonoBehaviour
	{
		[SerializeField]
		private RectTransform m_fillRect;

		[SerializeField]
		private Direction m_direction;

		[SerializeField]
		[Range(0f, 1f)]
		private float m_factor;

		private Axis axis
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Invalid comparison between Unknown and I4
				if ((int)m_direction != 0 && (int)m_direction != 1)
				{
					return 1;
				}
				return 0;
			}
		}

		private bool reverseValue
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Invalid comparison between Unknown and I4
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Invalid comparison between Unknown and I4
				if ((int)m_direction != 1)
				{
					return (int)m_direction == 3;
				}
				return true;
			}
		}

		private void OnEnable()
		{
			UpdateVisual();
		}

		protected void OnDidApplyAnimationProperties()
		{
			UpdateVisual();
		}

		private void UpdateVisual()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Expected I4, but got Unknown
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Expected I4, but got Unknown
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			if (m_fillRect != null)
			{
				Vector2 zero = Vector2.get_zero();
				Vector2 one = Vector2.get_one();
				if (reverseValue)
				{
					zero.set_Item((int)axis, 1f - m_factor);
				}
				else
				{
					one.set_Item((int)axis, m_factor);
				}
				m_fillRect.set_anchorMin(zero);
				m_fillRect.set_anchorMax(one);
			}
		}

		public SliderNotSelectable()
			: this()
		{
		}
	}
}
