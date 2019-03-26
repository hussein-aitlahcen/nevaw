using Ankama.Cube.Data;

namespace Ankama.Cube.UI.DeckMaker
{
	public class SpellDataCellRenderer : SpellCellRenderer<SpellData, ISpellDataCellRendererConfigurator>
	{
		protected override bool IsAvailable()
		{
			return m_configurator?.IsSpellDataAvailable(m_value) ?? true;
		}

		protected override int? GetAPCost()
		{
			return m_value?.definition.GetCost(null);
		}

		protected override int? GetBaseAPCost()
		{
			return m_value?.definition.GetCost(null);
		}

		protected override void SetValue(SpellData val)
		{
			SetValue(val?.definition, (val != null) ? val.level : 0);
		}
	}
}
