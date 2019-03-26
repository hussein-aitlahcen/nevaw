using UnityEngine;
using UnityEngine.Serialization;

namespace Ankama.Cube.UI.Components
{
	[CreateAssetMenu(menuName = "Waven/UI/Buttons/AnimatedTextButtonStyle", order = 1000)]
	public class AnimatedTextButtonStyle : ScriptableObject
	{
		[FormerlySerializedAs("m_baseButtonStyle")]
		[SerializeField]
		public BaseButtonStyle baseButtonStyle;

		[SerializeField]
		public AnimatedTextButtonState normal;

		[SerializeField]
		public AnimatedTextButtonState highlight;

		[SerializeField]
		public AnimatedTextButtonState pressed;

		[SerializeField]
		public AnimatedTextButtonState disable;

		public AnimatedTextButtonStyle()
			: this()
		{
		}
	}
}
