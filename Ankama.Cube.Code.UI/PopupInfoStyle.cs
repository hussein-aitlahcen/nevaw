using System;
using UnityEngine;

namespace Ankama.Cube.Code.UI
{
	[Serializable]
	public struct PopupInfoStyle
	{
		[SerializeField]
		public PopupStyle style;

		[SerializeField]
		public Color titleColor;

		[SerializeField]
		public Color textColor;
	}
}
