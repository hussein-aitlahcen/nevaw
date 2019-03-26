using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.Demo.UI
{
	public abstract class AbstractStatLine<T> : MonoBehaviour where T : AbstractStat
	{
		[SerializeField]
		protected CanvasGroup m_canvasGroup;

		[SerializeField]
		protected LayoutGroup m_alliesGroup;

		[SerializeField]
		protected LayoutGroup m_opponentsGroup;

		[SerializeField]
		protected List<T> m_alliesStats;

		[SerializeField]
		protected List<T> m_opponentStats;

		public CanvasGroup canvasGroup => m_canvasGroup;

		protected AbstractStatLine()
			: this()
		{
		}
	}
}
