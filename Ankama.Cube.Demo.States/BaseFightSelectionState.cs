using Ankama.Cube.Demo.UI;
using Ankama.Cube.States;
using System;

namespace Ankama.Cube.Demo.States
{
	public abstract class BaseFightSelectionState : LoadSceneStateContext
	{
		public Action onUIOpeningFinished;

		public SlidingSide fromSide
		{
			get;
			set;
		}

		public SlidingSide toSide
		{
			get;
			set;
		}

		protected void GotoPreviousState()
		{
			(this.get_parent() as MainStateDemo).GotoPreviousState(this);
		}
	}
}
