using DG.Tweening;
using UnityEngine;

namespace Ankama.Cube.Demo.UI
{
	public class GameSelectionButtonStyle : ScriptableObject
	{
		[SerializeField]
		public Ease ease;

		[SerializeField]
		public float transitionDuration;

		[SerializeField]
		public float fromNormalAndUnHighlightedDelay;

		[SerializeField]
		public GameSelectionButtonState normal;

		[SerializeField]
		public GameSelectionButtonState normalButAnotherIsHighlighted;

		[SerializeField]
		public GameSelectionButtonState highlight;

		[SerializeField]
		public GameSelectionButtonState pressed;

		[SerializeField]
		public GameSelectionButtonState disable;

		public GameSelectionButtonStyle()
			: this()
		{
		}
	}
}
