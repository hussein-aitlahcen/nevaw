using Ankama.AssetManagement.StateManagement;
using Ankama.Cube.Utility;
using System.Collections;

namespace Ankama.Cube.UI.Zaap
{
	public class VersusState : StateContext
	{
		private UIZaapPVPLoading m_ui;

		private readonly StateContext m_nextState;

		public VersusState(UIZaapPVPLoading ui, StateContext nextState)
			: this()
		{
			m_ui = ui;
			m_nextState = nextState;
		}

		protected override IEnumerator Update()
		{
			while ((int)m_nextState.get_loadState() == 1)
			{
				yield return null;
			}
			yield return m_ui.CloseUI();
			StatesUtility.ClearSecondaryLayers();
		}
	}
}
