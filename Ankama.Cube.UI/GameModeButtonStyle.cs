using DG.Tweening;
using UnityEngine;

namespace Ankama.Cube.UI
{
	[CreateAssetMenu]
	public class GameModeButtonStyle : ScriptableObject
	{
		[SerializeField]
		public Ease ease;

		[SerializeField]
		public float transitionDuration;

		[SerializeField]
		public GameModeButtonState normal;

		[SerializeField]
		public GameModeButtonState highlight;

		[SerializeField]
		public GameModeButtonState pressed;

		[SerializeField]
		public GameModeButtonState disable;

		public GameModeButtonStyle()
			: this()
		{
		}
	}
}
