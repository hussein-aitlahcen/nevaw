using Ankama.Cube.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

namespace Ankama.Cube.Demo.UI
{
	public abstract class BaseFightSelectionUI : AbstractUI
	{
		[SerializeField]
		protected PlayableDirector m_openDirector;

		[SerializeField]
		protected PlayableDirector m_closeDirector;

		protected void UpdateInactivityTime()
		{
			InactivityHandler.UpdateActivity();
		}

		public abstract IEnumerator OpenFrom(SlidingSide side);

		public abstract IEnumerator CloseTo(SlidingSide side);
	}
}
