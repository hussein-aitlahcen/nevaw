using Ankama.Cube.UI.Components;

namespace Ankama.Cube.UI.DeckMaker
{
	public interface IDeckDisplayConfigurator : IWithTooltipCellRendererConfigurator, ICellRendererConfigurator
	{
		DeckBuildingEventController eventController
		{
			get;
		}
	}
}
