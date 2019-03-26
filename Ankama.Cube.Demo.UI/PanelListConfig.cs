using DG.Tweening;
using UnityEngine;

namespace Ankama.Cube.Demo.UI
{
	public class PanelListConfig : ScriptableObject
	{
		[Header("Bounds Offset")]
		[SerializeField]
		public int leftOffset;

		[SerializeField]
		public int rightOffset;

		[Header("Selection Anim")]
		[SerializeField]
		public float selectionTweenDuration = 0.2f;

		[SerializeField]
		public Ease selectionTweenEase = 1;

		[SerializeField]
		public AnimationCurve elementRepartition;

		[Header("Element visibility")]
		[SerializeField]
		public float imageDepthDarken = 0.35f;

		[SerializeField]
		public float imageDepthDesaturation = 0.5f;

		[SerializeField]
		public Color textDepthTint = Color.get_grey();

		[SerializeField]
		public AnimationCurve depthRepartition;

		[SerializeField]
		public float shadowDepthAlpha;

		[SerializeField]
		public AnimationCurve shadowDepthRepartition;

		[Header("Transition Anim")]
		[SerializeField]
		public SlidingAnimUIConfig openAnim;

		[SerializeField]
		public SlidingAnimUIConfig closeAnim;

		public PanelListConfig()
			: this()
		{
		}//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)

	}
}
