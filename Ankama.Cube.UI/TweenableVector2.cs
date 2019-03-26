using UnityEngine;

namespace Ankama.Cube.UI
{
	public class TweenableVector2 : Tweenable<Vector2>
	{
		public override void Evaluate(float percentage)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			m_value = m_startValue + (m_endValue - m_startValue) * percentage;
		}
	}
}
