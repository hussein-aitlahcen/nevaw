using DG.Tweening;
using UnityEngine;

namespace Ankama.Cube.UI.Components
{
	[CreateAssetMenu(menuName = "Waven/UI/RotativeListConfig")]
	public class RotativeListConfig : ScriptableObject
	{
		[SerializeField]
		public int minCells;

		[SerializeField]
		[Range(0f, 1f)]
		public float extraCellsDistribution;

		[SerializeField]
		public bool emptyCellsAreSelectable;

		[SerializeField]
		public float moveTweenDuration;

		[SerializeField]
		public Ease moveTweenEase;

		[SerializeField]
		public float inTweenDuration;

		[SerializeField]
		public float inTweenDelayByElement;

		[SerializeField]
		public Ease inTweenEase;

		[SerializeField]
		public float outScale;

		[SerializeField]
		public float outTweenDuration;

		[SerializeField]
		public float outTweenDelayByElement;

		[SerializeField]
		public Ease outTweenEase;

		[SerializeField]
		public AnimationCurve cellPositionCurve;

		[SerializeField]
		public AnimationCurve cellVisibilityCurve;

		[SerializeField]
		public AnimationCurve cellHighlightCurve;

		public RotativeListConfig()
			: this()
		{
		}
	}
}
