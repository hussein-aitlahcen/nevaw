using UnityEngine;

namespace Ankama.Cube.UI.Components
{
	[CreateAssetMenu(menuName = "Waven/UI/Buttons/AnimatedImageButtonStyle", order = 1001)]
	public class AnimatedImageButtonStyle : ScriptableObject
	{
		[SerializeField]
		public BaseButtonStyle baseButtonStyle;

		[SerializeField]
		public AnimatedImageButtonState normal;

		[SerializeField]
		public AnimatedImageButtonState highlight;

		[SerializeField]
		public AnimatedImageButtonState pressed;

		[SerializeField]
		public AnimatedImageButtonState disable;

		public AnimatedImageButtonStyle()
			: this()
		{
		}
	}
}
