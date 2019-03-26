using Ankama.Cube.UI.Components;

namespace Ankama.Cube.UI.DeckMaker
{
	public interface IDeckSlotCellRendererConfigurator : ICellRendererConfigurator
	{
		DeckSlot currentSlot
		{
			get;
		}

		void OnClicked(DeckSlot slot);
	}
}
