using UnityEngine;
using UnityEngine.EventSystems;

namespace Ankama.Cube.UI.Components.Tooltip
{
	[RequireComponent(typeof(RectTransform))]
	public class GenericTooltipRequester : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		[SerializeField]
		private GenericTooltipWindow m_tooltip;

		[SerializeField]
		private TooltipPosition m_tooltipPosition;

		[SerializeField]
		[TextKey]
		private int m_textKeyId;

		[SerializeField]
		private bool m_withTitle;

		[SerializeField]
		[TextKey]
		private int m_titleTextKeyId;

		[SerializeField]
		private bool m_multiline = true;

		private RectTransform m_rectTransform;

		private bool m_showingTooltip;

		private void Awake()
		{
			m_rectTransform = this.GetComponent<RectTransform>();
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (!(m_tooltip == null))
			{
				int? titleTextKeyId = null;
				if (m_withTitle)
				{
					titleTextKeyId = m_titleTextKeyId;
				}
				m_tooltip.SetText(m_textKeyId, null, titleTextKeyId, null, m_multiline);
				m_tooltip.ShowAt(m_tooltipPosition, m_rectTransform);
				m_showingTooltip = true;
			}
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (!(m_tooltip == null))
			{
				m_tooltip.Hide();
				m_showingTooltip = false;
			}
		}

		private void OnDisable()
		{
			if (!(m_tooltip == null) && m_showingTooltip)
			{
				m_tooltip.Hide();
				m_showingTooltip = false;
			}
		}

		public GenericTooltipRequester()
			: this()
		{
		}
	}
}
