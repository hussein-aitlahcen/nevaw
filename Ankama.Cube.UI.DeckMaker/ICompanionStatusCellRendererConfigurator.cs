using Ankama.Cube.UI.Components;
using Ankama.Cube.UI.Fight;

namespace Ankama.Cube.UI.DeckMaker
{
	public interface ICompanionStatusCellRendererConfigurator : ICompanionCellRendererConfigurator, IWithTooltipCellRendererConfigurator, ICellRendererConfigurator
	{
		IDragNDropValidator GetDragNDropValidator();

		bool IsParentInteractable();

		CastEventListener GetEventListener();

		CompanionStatusData? GetCompanionStatusData(ReserveCompanionStatus companion);
	}
}
