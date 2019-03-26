using System;
using System.Threading;
using UnityEngine;

namespace Ankama.Cube.Configuration
{
	public class HaapiRequestBehaviour : MonoBehaviour
	{
		private HaapiRequest m_request;

		private bool m_done;

		public void ExecuteRequest<U>(Func<U> function, Action<U> onSuccess, Action<Exception> onException)
		{
			m_request = new HaapiRequest<U>(function, onSuccess, onException);
			HaapiRequest request = m_request;
			new Thread(request.SendRequest).Start();
		}

		private void Update()
		{
			if (!m_done && m_request != null && m_request.isDone)
			{
				try
				{
					m_request.ExecuteResult();
				}
				finally
				{
					m_done = true;
					Object.Destroy(this.get_gameObject());
				}
			}
		}

		public HaapiRequestBehaviour()
			: this()
		{
		}
	}
}
