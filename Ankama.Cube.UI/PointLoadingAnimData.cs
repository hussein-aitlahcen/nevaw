using UnityEngine;

namespace Ankama.Cube.UI
{
	[CreateAssetMenu(menuName = "Waven/UI/PointLoadingAnimData")]
	public class PointLoadingAnimData : ScriptableObject
	{
		[SerializeField]
		public float scale = 1.5f;

		[SerializeField]
		public float duration = 0.1f;

		[SerializeField]
		public int vibrato = 10;

		[SerializeField]
		public float elasticity = 1f;

		[SerializeField]
		public float delayBetweenPoints = 0.1f;

		[SerializeField]
		public float delayBetweenLoops = 0.2f;

		public PointLoadingAnimData()
			: this()
		{
		}
	}
}
