using UnityEngine;

namespace Ankama.Cube.UI.Components
{
	[CreateAssetMenu(menuName = "Waven/UI/Buttons/BaseToggleButtonStyle", order = 100)]
	public class BaseToggleButtonStyle : ScriptableObject
	{
		[SerializeField]
		public Sprite toggle;

		[SerializeField]
		public Sprite background;

		[SerializeField]
		public Sprite border;

		[SerializeField]
		public Sprite outline;

		[SerializeField]
		public float transitionDuration;

		public BaseToggleButtonStyle()
			: this()
		{
		}
	}
}
