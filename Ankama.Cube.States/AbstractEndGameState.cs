using Ankama.AssetManagement.StateManagement;
using JetBrains.Annotations;
using System;

namespace Ankama.Cube.States
{
	public class AbstractEndGameState : LoadSceneStateContext
	{
		public Action onContinue;

		public bool allowTransition;

		public override bool AllowsTransition([CanBeNull] StateContext nextState)
		{
			return allowTransition;
		}
	}
}
