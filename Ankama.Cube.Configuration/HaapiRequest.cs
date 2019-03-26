using System;

namespace Ankama.Cube.Configuration
{
	public abstract class HaapiRequest
	{
		protected bool m_done;

		public bool isDone => m_done;

		public abstract void SendRequest();

		public abstract void ExecuteResult();
	}
	public class HaapiRequest<U> : HaapiRequest
	{
		private Func<U> m_function;

		private Action<U> m_onSuccess;

		private U m_result;

		private Action<Exception> m_onException;

		private Exception m_exception;

		private bool m_sentResult;

		public HaapiRequest(Func<U> function, Action<U> onSuccess, Action<Exception> onException)
		{
			m_function = function;
			m_onSuccess = onSuccess;
			m_onException = onException;
		}

		public override void SendRequest()
		{
			try
			{
				m_result = m_function();
			}
			catch (Exception exception)
			{
				Exception ex = m_exception = exception;
			}
			m_done = true;
		}

		public override void ExecuteResult()
		{
			if (m_done)
			{
				if (m_result != null)
				{
					m_onSuccess?.Invoke(m_result);
				}
				if (m_exception != null)
				{
					m_onException?.Invoke(m_exception);
				}
			}
		}
	}
}
