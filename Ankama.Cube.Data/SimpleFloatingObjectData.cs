using UnityEngine;

namespace Ankama.Cube.Data
{
	[CreateAssetMenu(menuName = "Waven/Animations/Simple Floating Object Data")]
	public class SimpleFloatingObjectData : ScriptableObject
	{
		[SerializeField]
		public float verticalNoise = 0.2f;

		[SerializeField]
		public float verticalSpeed = 1f;

		[SerializeField]
		public float rotationNoise = 0.2f;

		[SerializeField]
		public float rotationSpeed = 1f;

		public SimpleFloatingObjectData()
			: this()
		{
		}
	}
}
