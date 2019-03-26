using Ankama.Cube.Data;
using Ankama.Cube.UI.Components;

namespace Ankama.Cube.UI.DeckMaker
{
	public interface ICompanionDataCellRendererConfigurator : ICompanionCellRendererConfigurator, IWithTooltipCellRendererConfigurator, ICellRendererConfigurator
	{
		bool IsCompanionDataAvailable(CompanionData data);
	}
}
