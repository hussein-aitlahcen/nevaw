using Ankama.Cube.UI.Components;
using Ankama.Cube.UI.Fight.Windows;

namespace Ankama.Cube.UI.DeckMaker
{
	public interface IWithTooltipCellRendererConfigurator : ICellRendererConfigurator
	{
		FightTooltip tooltip
		{
			get;
		}

		TooltipPosition tooltipPosition
		{
			get;
		}
	}
}
