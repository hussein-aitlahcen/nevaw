namespace Ankama.Cube.Fight.Entities
{
	public interface ISpellTooltipDataProvider : ITooltipDataProvider
	{
		TooltipElementValues GetGaugeModifications();
	}
}
