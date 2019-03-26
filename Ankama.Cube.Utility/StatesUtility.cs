using Ankama.AssetManagement;
using Ankama.AssetManagement.StateManagement;
using Ankama.Cube.Configuration;
using Ankama.Cube.Demo.States;
using Ankama.Cube.Network;
using Ankama.Cube.States;
using Ankama.Cube.UI;
using Ankama.Utilities;
using JetBrains.Annotations;

namespace Ankama.Cube.Utility
{
	public static class StatesUtility
	{
		public const string PlayerUILayerName = "PlayerUI";

		public const string OptionUILayerName = "OptionUI";

		private static StateLayer s_optionLayer;

		private static StateLayer s_playerLayer;

		private static StateLayer optionLayer
		{
			get
			{
				if (s_optionLayer == null)
				{
					s_optionLayer = StateManager.AddLayer("OptionUI");
				}
				return s_optionLayer;
			}
		}

		private static StateContext playerLayer
		{
			get
			{
				if (s_playerLayer == null)
				{
					s_playerLayer = StateManager.AddLayer("PlayerUI");
				}
				return s_playerLayer;
			}
		}

		public static void GotoLoginState()
		{
			ConnectionHandler.instance.Disconnect();
			StateContext childState = StateManager.GetDefaultLayer().GetChildState();
			if (!(childState is LoginState) && !(childState is LoginStateDemo))
			{
				StateContext nextState = (!ApplicationConfig.simulateDemo) ? ((object)new LoginState()) : ((object)new LoginStateDemo());
				DoTransition(nextState, childState);
			}
		}

		public static void GotoMainMenu()
		{
			GotoDimensionState();
		}

		public static void GotoDimensionState()
		{
			DoTransition(new HavreDimensionMainState(), StateManager.GetDefaultLayer().GetChildState());
			PlayerUIMainState playerUIMainState = new PlayerUIMainState();
			playerLayer.SetChildState(playerUIMainState, 0);
			ParametersState parametersState = new ParametersState();
			optionLayer.SetChildState(parametersState, 0);
			StateManager.SetActiveInputLayer(optionLayer);
			UIManager.instance.NotifyLayerIndexChange();
		}

		public static void GotoMatchMakingState()
		{
			if (ApplicationConfig.debugMode)
			{
				GotoClearState(new MatchMakingState());
				return;
			}
			Log.Warning("GotoMatchMakingState while not in debugMode. Goto HavreDimension instead", 87, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Utility\\StatesUtility.cs");
			GotoDimensionState();
		}

		public static void GotoClearState(StateContext state)
		{
			ClearSecondaryLayers();
			DoTransition(state, StateManager.GetDefaultLayer().GetChildState());
		}

		public static void ClearSecondaryLayers()
		{
			ClearOptionLayer();
			playerLayer.ClearChildState(0);
		}

		public static void ClearOptionLayer()
		{
			optionLayer.ClearChildState(0);
		}

		public static TransitionState DoTransition([NotNull] StateContext nextState, [CanBeNull] StateContext previousState, [CanBeNull] StateContext parentState = null)
		{
			TransitionState transitionState = new TransitionState(nextState, previousState);
			(parentState ?? ((object)((previousState != null) ? previousState.get_parent() : null)) ?? ((object)StateManager.GetDefaultLayer())).SetChildState(transitionState, 0);
			return transitionState;
		}
	}
}
