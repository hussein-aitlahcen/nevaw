using System;

namespace Ankama.Cube.Code.UI
{
	public struct ButtonData
	{
		public readonly bool isValid;

		public readonly TextData textOverride;

		public readonly ButtonStyle style;

		public readonly Action onClick;

		public readonly bool closeOnClick;

		public ButtonData(TextData textOverride, Action onClick = null, bool closeOnClick = true, ButtonStyle style = ButtonStyle.Normal)
		{
			isValid = true;
			this.textOverride = textOverride;
			this.onClick = onClick;
			this.closeOnClick = closeOnClick;
			this.style = style;
		}
	}
}
