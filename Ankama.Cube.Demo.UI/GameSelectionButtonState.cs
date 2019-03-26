using System;
using UnityEngine;

namespace Ankama.Cube.Demo.UI
{
	[Serializable]
	public struct GameSelectionButtonState
	{
		[SerializeField]
		public Color imageColor;

		[SerializeField]
		public Color outlineColor;

		[SerializeField]
		public float scale;
	}
}
