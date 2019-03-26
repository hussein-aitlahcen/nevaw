using Ankama.AssetManagement;
using Ankama.Cube.Code.UI;
using System.Collections;

namespace Ankama.Cube.States
{
	public class ZaapRequiredState : LoadSceneStateContext
	{
		protected override IEnumerator Load()
		{
			yield break;
		}

		protected override IEnumerator Update()
		{
			ButtonData[] buttons = new ButtonData[1]
			{
				new ButtonData(75192, Main.Quit)
			};
			PopupInfoManager.Show(StateManager.GetDefaultLayer().GetChainEnd(), new PopupInfo
			{
				title = 20267,
				message = 21217,
				buttons = buttons,
				selectedButton = 1,
				style = PopupStyle.Error
			});
			while (true)
			{
				yield return null;
			}
		}

		protected override IEnumerator Unload()
		{
			yield return _003C_003En__0();
		}
	}
}
