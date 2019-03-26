using Ankama.Cube.Data;
using Ankama.Cube.UI.Components;

namespace Ankama.Cube.UI.DeckMaker
{
	public interface ISpellDataCellRendererConfigurator : ISpellCellRendererConfigurator, IWithTooltipCellRendererConfigurator, ICellRendererConfigurator
	{
		bool IsSpellDataAvailable(SpellData data);
	}
}
