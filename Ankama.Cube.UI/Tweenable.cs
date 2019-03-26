namespace Ankama.Cube.UI
{
	public abstract class Tweenable<T>
	{
		private bool m_init;

		protected T m_value;

		protected T m_startValue;

		protected T m_endValue;

		public bool init => m_init;

		public T currentValue => m_value;

		public T value => m_endValue;

		public T startValue
		{
			set
			{
				m_startValue = value;
			}
		}

		public T endValue
		{
			set
			{
				m_endValue = value;
			}
		}

		public void SetValue(T v)
		{
			m_value = (m_startValue = (m_endValue = v));
			m_init = true;
		}

		public void SetTweenValues(T startValue, T endValue)
		{
			m_startValue = startValue;
			m_endValue = endValue;
			m_init = true;
		}

		public void Reset()
		{
			m_init = false;
			m_value = (m_startValue = (m_endValue = default(T)));
		}

		public abstract void Evaluate(float percentage);
	}
}
