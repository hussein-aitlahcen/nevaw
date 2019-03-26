using UnityEngine;
using UnityEngine.Events;

namespace Ankama.Cube.Animations
{
	public sealed class InvokeEventOnEnable : MonoBehaviour
	{
		[SerializeField]
		private UnityEvent m_event;

		private void OnEnable()
		{
			if (m_event != null)
			{
				m_event.Invoke();
			}
			this.get_gameObject().SetActive(false);
		}

		public InvokeEventOnEnable()
			: this()
		{
		}
	}
}
