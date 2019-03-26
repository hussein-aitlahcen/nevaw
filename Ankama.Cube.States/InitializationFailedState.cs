using Ankama.AssetManagement.StateManagement;
using Ankama.Cube.Code.UI;
using Ankama.Cube.Utility;

namespace Ankama.Cube.States
{
	public class InitializationFailedState : StateContext
	{
		private readonly Main.InitializationFailure m_cause;

		public InitializationFailedState(Main.InitializationFailure cause)
			: this()
		{
			m_cause = cause;
		}

		protected override void Enable()
		{
			this.Enable();
			string formattedText = TextCollectionUtility.InitializationFailureKeys.GetFormattedText(m_cause);
			PopupInfo popupInfo = default(PopupInfo);
			popupInfo.title = 77080;
			popupInfo.message = formattedText;
			popupInfo.buttons = new ButtonData[1]
			{
				new ButtonData(27169, Main.Quit)
			};
			popupInfo.selectedButton = 1;
			PopupInfoManager.ShowApplicationError(popupInfo);
		}
	}
}
