namespace Ankama.Cube.UI
{
	public class TweenableFloat : Tweenable<float>
	{
		public override void Evaluate(float percentage)
		{
			m_value = m_startValue + (m_endValue - m_startValue) * percentage;
		}
	}
}
