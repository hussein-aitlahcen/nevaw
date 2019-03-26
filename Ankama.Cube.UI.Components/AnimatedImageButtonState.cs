using System;
using UnityEngine;

namespace Ankama.Cube.UI.Components
{
	[Serializable]
	public struct AnimatedImageButtonState
	{
		[SerializeField]
		public Color imageColor;

		[SerializeField]
		public Vector2 imageSizeDelta;

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
