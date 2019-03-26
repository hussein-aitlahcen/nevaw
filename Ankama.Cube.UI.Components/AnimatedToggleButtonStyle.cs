using UnityEngine;

namespace Ankama.Cube.UI.Components
{
	[CreateAssetMenu(menuName = "Waven/UI/Buttons/AnimatedToggleButtonStyle", order = 2000)]
	public class AnimatedToggleButtonStyle : ScriptableObject
	{
		[SerializeField]
		public BaseToggleButtonStyle baseButtonStyle;

		[SerializeField]
		public Color baseGraphicColor = Color.get_white();

		[SerializeField]
		public Color selectedGraphicColor;

		[SerializeField]
		public bool useOnlyAlpha;

		[SerializeField]
		public float selectionTransitionDuration = 0.1f;

		[SerializeField]
		public AnimatedToggleButtonState normal;

		[SerializeField]
		public AnimatedToggleButtonState highlight;

		[SerializeField]
		public AnimatedToggleButtonState pressed;

		[SerializeField]
		public AnimatedToggleButtonState disable;

		public AnimatedToggleButtonStyle()
			: this()
		{
		}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)

	}
}
