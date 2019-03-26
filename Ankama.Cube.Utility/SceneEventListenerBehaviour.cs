using System;
using UnityEngine;

namespace Ankama.Cube.Utility
{
	[ExecuteInEditMode]
	public class SceneEventListenerBehaviour : MonoBehaviour
	{
		public Action onUpdate;

		public Action onDrawGizmos;

		private void Update()
		{
			onUpdate?.Invoke();
		}

		public SceneEventListenerBehaviour()
			: this()
		{
		}
	}
}
