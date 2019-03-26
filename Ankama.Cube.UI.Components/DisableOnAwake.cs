using UnityEngine;

namespace Ankama.Cube.UI.Components
{
	public class DisableOnAwake : MonoBehaviour
	{
		private void Awake()
		{
			this.get_gameObject().SetActive(false);
		}

		public DisableOnAwake()
			: this()
		{
		}
	}
}
