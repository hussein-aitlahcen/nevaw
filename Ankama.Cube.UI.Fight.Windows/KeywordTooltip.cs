using Ankama.Cube.Fight.Entities;
using UnityEngine;

namespace Ankama.Cube.UI.Fight.Windows
{
	public class KeywordTooltip : MonoBehaviour
	{
		[SerializeField]
		private FightTooltipContent m_content;

		[SerializeField]
		private CanvasGroup m_canvasGroup;

		public float alpha
		{
			get
			{
				return m_canvasGroup.get_alpha();
			}
			set
			{
				m_canvasGroup.set_alpha(value);
			}
		}

		public void Initialize(ITooltipDataProvider tooltipDataProvider)
		{
			m_content.Initialize(tooltipDataProvider);
		}

		public KeywordTooltip()
			: this()
		{
		}
	}
}
