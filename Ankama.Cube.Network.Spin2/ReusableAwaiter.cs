using System;
using System.Runtime.CompilerServices;

namespace Ankama.Cube.Network.Spin2
{
	internal sealed class ReusableAwaiter<T> : INotifyCompletion
	{
		private Action m_continuation;

		private T m_result;

		private Exception m_exception;

		public bool IsCompleted
		{
			get;
			private set;
		}

		public T GetResult()
		{
			if (m_exception != null)
			{
				throw m_exception;
			}
			return m_result;
		}

		public void OnCompleted(Action continuation)
		{
			if (m_continuation != null)
			{
				throw new InvalidOperationException("This ReusableAwaiter instance has already been listened");
			}
			m_continuation = continuation;
		}

		public bool TrySetResult(T result)
		{
			if (!IsCompleted)
			{
				IsCompleted = true;
				m_result = result;
				if (m_continuation != null)
				{
					m_continuation();
				}
				return true;
			}
			return false;
		}

		public bool TrySetException(Exception exception)
		{
			if (!IsCompleted)
			{
				IsCompleted = true;
				m_exception = exception;
				if (m_continuation != null)
				{
					m_continuation();
				}
				return true;
			}
			return false;
		}

		public ReusableAwaiter<T> Reset()
		{
			m_result = default(T);
			m_continuation = null;
			m_exception = null;
			IsCompleted = false;
			return this;
		}

		public ReusableAwaiter<T> GetAwaiter()
		{
			return this;
		}
	}
}
