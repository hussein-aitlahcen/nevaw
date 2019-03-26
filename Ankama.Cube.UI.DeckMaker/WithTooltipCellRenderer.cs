using Ankama.Cube.Data;
using Ankama.Cube.Data.UI.Localization.TextFormatting.ValueProviders.FightValueProviders;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.UI.Components;
using Ankama.Cube.UI.Fight.Windows;
using UnityEngine.EventSystems;

namespace Ankama.Cube.UI.DeckMaker
{
	public abstract class WithTooltipCellRenderer<T, U> : CellRenderer<T, U>, ITooltipDataProvider, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler where U : IWithTooltipCellRendererConfigurator
	{
		protected IFightValueProvider m_valueProvider;

		private FightTooltip m_tooltip;

		private TooltipPosition m_tooltipPosition;

		public abstract TooltipDataType tooltipDataType
		{
			get;
		}

		public abstract KeywordReference[] keywordReferences
		{
			get;
		}

		public void SetTooltip(FightTooltip tooltip, TooltipPosition tooltipPosition)
		{
			m_tooltip = tooltip;
			m_tooltipPosition = tooltipPosition;
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (!(m_tooltip == null) && value != null && !ItemDragNDropListener.instance.dragging)
			{
				m_tooltip.Initialize(this);
				m_tooltip.ShowAt(m_tooltipPosition, base.rectTransform);
			}
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (!(m_tooltip == null))
			{
				m_tooltip.Hide();
			}
		}

		public abstract int GetTitleKey();

		public abstract int GetDescriptionKey();

		public abstract IFightValueProvider GetValueProvider();

		public override void OnConfiguratorUpdate(bool instant)
		{
			ref U reference = ref m_configurator;
			U val = default(U);
			object tooltip;
			if (val == null)
			{
				val = reference;
				reference = ref val;
				if (val == null)
				{
					tooltip = null;
					goto IL_0036;
				}
			}
			tooltip = reference.tooltip;
			goto IL_0036;
			IL_0036:
			m_tooltip = (FightTooltip)tooltip;
			if (m_configurator != null)
			{
				m_tooltipPosition = m_configurator.tooltipPosition;
			}
		}
	}
}
