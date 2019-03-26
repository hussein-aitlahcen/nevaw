using Ankama.AssetManagement;
using Ankama.AssetManagement.InputManagement;
using Ankama.AssetManagement.StateManagement;
using Ankama.Cube.Code.UI;
using Ankama.Cube.Protocols.FightCommonProtocol;
using Ankama.Cube.States;
using Ankama.Cube.TEMPFastEnterMatch.MatchMaking;
using Ankama.Cube.TEMPFastEnterMatch.Player;
using Ankama.Cube.Utility;
using Ankama.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Ankama.Cube.UI.Zaap
{
	public class UIPVPLoadingState : LoadSceneStateContext
	{
		private UIZaapPVPLoading m_ui;

		private MatchMakingFrame m_frame;

		private int m_fightDefinitionId;

		public void SetGameMode(int fightDefininitionId)
		{
			m_fightDefinitionId = fightDefininitionId;
		}

		protected override IEnumerator Load()
		{
			AssetManager.LoadAssetBundle(AssetBundlesUtility.GetUIAnimatedCharacterResourcesBundleName());
			UILoader<UIZaapPVPLoading> loader = new UILoader<UIZaapPVPLoading>(this, "MatchmakingUI_1v1", "core/scenes/maps/havre_maps", disableOnLoad: true);
			yield return loader.Load();
			m_ui = loader.ui;
			m_ui.onForceAiRequested = OnForceAiRequested;
			m_ui.onCancelRequested = OnCancelRequested;
			m_ui.onEnterAnimationFinished = OnPlayRequested;
			yield return m_ui.LoadAssets();
			m_frame = new MatchMakingFrame
			{
				onGameCreated = OnGameCreated,
				onGameCanceled = OnGameCancel,
				onGameError = OnGameError
			};
		}

		protected override void Disable()
		{
			m_ui.onCancelRequested = null;
			m_ui.onForceAiRequested = null;
			m_ui.get_gameObject().SetActive(false);
			m_frame.Dispose();
		}

		protected override IEnumerator Update()
		{
			m_ui.get_gameObject().SetActive(true);
			m_ui.Init(m_fightDefinitionId);
			yield return m_ui.LoadUI();
			yield return null;
		}

		protected override IEnumerator Unload()
		{
			m_ui.UnloadAsset();
			yield return _003C_003En__0();
		}

		protected override IEnumerator Transition(StateTransitionInfo transitionInfo)
		{
			yield return m_ui.CloseUI();
			yield return _003C_003En__1(transitionInfo);
		}

		public override bool AllowsTransition(StateContext nextState)
		{
			return true;
		}

		private void OnCancelRequested()
		{
			if (m_frame != null)
			{
				m_frame.SendCancelGame();
			}
		}

		private void OnPlayRequested(int fightDefinitionId, int? forcedLevel)
		{
			m_frame.SendCreateGame(fightDefinitionId, PlayerData.instance.currentDeckId, forcedLevel);
		}

		private void OnGameError()
		{
			PopupInfoManager.Show(this, new PopupInfo
			{
				message = 54306,
				buttons = new ButtonData[1]
				{
					new ButtonData(new TextData(27169))
				},
				selectedButton = 1,
				style = PopupStyle.Error
			});
		}

		private void OnForceAiRequested()
		{
			m_frame.SendToggleMatchmaking();
		}

		private void OnGameCancel()
		{
			this.GetLayer().GetChainRoot().ClearChildState(0);
		}

		private void OnGameCreated(FightInfo fightInfo)
		{
			if (m_ui != null)
			{
				m_ui.StartCoroutine(StartGame(fightInfo));
			}
		}

		private IEnumerator StartGame(FightInfo fightInfo)
		{
			StatesUtility.ClearOptionLayer();
			yield return ApplyFightInfos(fightInfo);
			yield return m_ui.GotoVersusAnim();
			yield return (object)new WaitForTime(2f);
			StateLayer defaultLayer = StateManager.GetDefaultLayer();
			StateContext currentState = defaultLayer.GetChildState();
			if (currentState != null)
			{
				defaultLayer.ClearChildState(0);
				while ((int)currentState.get_loadState() == 8)
				{
					yield return null;
				}
			}
			FightState fightState = new FightState(fightInfo);
			defaultLayer.SetChildState(fightState, 0);
			VersusState versusState = new VersusState(m_ui, fightState);
			this.SetChildState(versusState, 0);
		}

		private IEnumerator ApplyFightInfos(FightInfo fightInfo)
		{
			FightInfo.Types.Player opponent = GetOpponent((IList<FightInfo.Types.Team>)fightInfo.Teams);
			yield return m_ui.SetOpponent(opponent);
		}

		private FightInfo.Types.Player GetOpponent(IList<FightInfo.Types.Team> teams)
		{
			for (int i = 0; i < teams.Count; i++)
			{
				FightInfo.Types.Team team = teams[i];
				int count = team.Players.get_Count();
				for (int j = 0; j < count; j++)
				{
					FightInfo.Types.Player player = team.Players.get_Item(j);
					if (player.Name != PlayerData.instance.nickName)
					{
						return player;
					}
				}
			}
			return null;
		}

		protected unsafe override bool UseInput(InputState inputState)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Invalid comparison between Unknown and I4
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			int id = ((IntPtr)(void*)inputState).id;
			if (id == 1)
			{
				if ((int)((IntPtr)(void*)inputState).state == 1)
				{
					OnCancelRequested();
				}
				return true;
			}
			return this.UseInput(inputState);
		}
	}
}
