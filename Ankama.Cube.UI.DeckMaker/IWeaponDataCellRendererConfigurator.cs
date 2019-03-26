using Ankama.Cube.Data;
using Ankama.Cube.UI.Components;

namespace Ankama.Cube.UI.DeckMaker
{
	public interface IWeaponDataCellRendererConfigurator : IWithTooltipCellRendererConfigurator, ICellRendererConfigurator
	{
		bool IsWeaponDataAvailable(WeaponData data);
	}
}
