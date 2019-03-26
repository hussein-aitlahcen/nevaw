using UnityEngine;

namespace Ankama.Cube.UI
{
	[CreateAssetMenu(menuName = "Waven/UI/SpinLoadingAnimData")]
	public class SpinLoadingAnimData : ScriptableObject
	{
		public enum OffsetType
		{
			Value,
			WoldPos
		}

		[SerializeField]
		public float speed = 1f;

		[SerializeField]
		public float step;

		[SerializeField]
		public OffsetType offsetType;

		[SerializeField]
		public float offset;

		public SpinLoadingAnimData()
			: this()
		{
		}
	}
}
