using UnityEngine;

namespace Ankama.Cube.UI.Components
{
	[CreateAssetMenu(menuName = "Waven/UI/Buttons/BaseButtonStyle", order = 1)]
	public class BaseButtonStyle : ScriptableObject
	{
		[SerializeField]
		public Sprite background;

		[SerializeField]
		public Sprite border;

		[SerializeField]
		public Sprite outline;

		[SerializeField]
		public float transitionDuration;

		public BaseButtonStyle()
			: this()
		{
		}
	}
}
