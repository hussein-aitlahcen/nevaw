using Ankama.Cube.Fight.Entities;
using Ankama.Cube.UI.Components;
using Ankama.Cube.UI.Fight;

namespace Ankama.Cube.UI.DeckMaker
{
	public interface ISpellStatusCellRendererConfigurator : ISpellCellRendererConfigurator, IWithTooltipCellRendererConfigurator, ICellRendererConfigurator
	{
		IDragNDropValidator GetDragNDropValidator();

		bool IsParentInteractable();

		CastEventListener GetEventListener();

		SpellStatusData? GetSpellStatusData(SpellStatus data);
	}
}
