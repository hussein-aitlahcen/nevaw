using System;
using UnityEngine;

namespace Ankama.Cube.UI.Components
{
	[Serializable]
	public struct AnimatedTextButtonState
	{
		[SerializeField]
		public Color textColor;

		[SerializeField]
		public Color backgroundColor;

		[SerializeField]
		public Vector2 backgroundSizeDelta;

		[SerializeField]
		public Color borderColor;

		[SerializeField]
		public Color outlineColor;

		[SerializeField]
		public Vector2 outlineSizeDelta;
	}
}
