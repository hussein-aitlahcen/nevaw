using UnityEngine;

namespace Ankama.Cube.Demo.UI
{
	[CreateAssetMenu]
	public class SlidingAnimUIConfig : ScriptableObject
	{
		[SerializeField]
		public float delay;

		[SerializeField]
		public float duration;

		[SerializeField]
		public AnimationCurve positionCurve;

		[SerializeField]
		public float elementDelayOffset;

		[SerializeField]
		public float endAlpha;

		[SerializeField]
		public AnimationCurve alphaCurve;

		[SerializeField]
		public Vector2 anchorOffset;

		public SlidingAnimUIConfig()
			: this()
		{
		}
	}
}
