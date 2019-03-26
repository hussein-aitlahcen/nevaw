namespace Ankama.Cube.Code.UI
{
	public struct PopupInfo
	{
		public RawTextData title;

		public RawTextData message;

		public PopupStyle style;

		public float displayDuration;

		public ButtonData[] buttons;

		public bool closeOnBackgroundClick;

		public int selectedButton;

		public bool useBlur;

		public bool hasDisplayDuration => displayDuration > 0f;
	}
}
