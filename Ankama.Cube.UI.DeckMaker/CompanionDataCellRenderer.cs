using Ankama.Cube.Data;
using System.Collections.Generic;

namespace Ankama.Cube.UI.DeckMaker
{
	public class CompanionDataCellRenderer : CompanionCellRenderer<CompanionData, ICompanionDataCellRendererConfigurator>
	{
		protected override IReadOnlyList<Cost> GetCosts()
		{
			return m_value?.definition.cost;
		}

		protected override bool IsAvailable()
		{
			return m_configurator?.IsCompanionDataAvailable(m_value) ?? true;
		}

		protected override void SetValue(CompanionData val)
		{
			SetValue(val?.definition, (val != null) ? val.level : 0);
		}
	}
}
