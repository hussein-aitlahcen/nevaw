using System;
using UnityEngine;

namespace Ankama.Cube.UI.Components
{
	[Serializable]
	public struct TextButtonStyle
	{
		[SerializeField]
		public TextButtonState normal;

		[SerializeField]
		public TextButtonState highlight;

		[SerializeField]
		public TextButtonState pressed;

		[SerializeField]
		public TextButtonState disable;
	}
}
