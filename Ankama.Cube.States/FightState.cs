using Ankama.AssetManagement;
using Ankama.AssetManagement.InputManagement;
using Ankama.Cube.Code.UI;
using Ankama.Cube.Data;
using Ankama.Cube.Data.UI.Localization.TextFormatting;
using Ankama.Cube.Fight;
using Ankama.Cube.Maps;
using Ankama.Cube.Maps.VisualEffects;
using Ankama.Cube.Protocols.CommonProtocol;
using Ankama.Cube.Protocols.FightCommonProtocol;
using Ankama.Cube.Protocols.FightProtocol;
using Ankama.Cube.UI;
using Ankama.Cube.UI.Components;
using Ankama.Cube.UI.Fight;
using Ankama.Cube.Utility;
using Ankama.Utilities;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ankama.Cube.States
{
	public class FightState : LoadSceneStateContext, IStateUITransitionPriority
	{
		public FightFrame frame;

		private FightUIRework m_uiRework;

		private readonly FightInfo m_fightInfo;

		private FightDefinition m_fightDefinition;

		private readonly int m_ownFightId;

		private readonly int m_fightDefId;

		private readonly int m_fightMapId;

		private readonly int m_concurrentFightsCount;

		private readonly bool m_hardResumed;

		private Scene m_fightMapScene;

		public static FightState instance
		{
			get;
			private set;
		}

		public UIPriority uiTransitionPriority => UIPriority.Back;

		public FightUIRework uiRework => m_uiRework;

		public FightState(FightInfo fightInfo, bool hardResumed = false)
		{
			m_fightInfo = fightInfo;
			m_ownFightId = fightInfo.OwnFightId;
			m_fightDefId = fightInfo.FightDefId;
			m_fightMapId = fightInfo.FightMapId;
			m_concurrentFightsCount = fightInfo.ConcurrentFightsCount;
			m_hardResumed = hardResumed;
		}

		protected override IEnumerator Load()
		{
			if (m_concurrentFightsCount == 1)
			{
				RuntimeData.currentKeywordContext = KeywordContext.FightSolo;
			}
			else
			{
				RuntimeData.currentKeywordContext = KeywordContext.FightMulti;
			}
			instance = this;
			int fightCount = m_concurrentFightsCount;
			if (!RuntimeData.fightDefinitions.TryGetValue(m_fightDefId, out m_fightDefinition))
			{
				Log.Error(string.Format("Could not find {0} with id {1}.", "FightDefinition", m_fightDefId), 78, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\States\\FightState.cs");
				yield break;
			}
			yield return RuntimeData.LoadTextCollectionAsync("Fight");
			yield return LoadSceneAndBundleRequest("FightMapWrapper", "core/scenes/maps/fight_maps");
			Scene sceneByName = SceneManager.GetSceneByName("FightMapWrapper");
			if (!sceneByName.get_isLoaded())
			{
				Log.Error("Could not load scene named 'FightMapWrapper' from bundle 'core/scenes/maps/fight_maps'.", 93, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\States\\FightState.cs");
				yield break;
			}
			yield return LoadFightMap();
			FightMap current = FightMap.current;
			if (null == current)
			{
				Log.Error("Failed to load fight map.", 104, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\States\\FightState.cs");
				yield break;
			}
			FightLogicExecutor.Initialize(fightCount);
			FightMapDefinition definition = current.definition;
			FightStatus[] array = new FightStatus[fightCount];
			for (int i = 0; i < fightCount; i++)
			{
				FightMapStatus mapStatus = definition.CreateFightMapStatus(i);
				FightStatus fightStatus = new FightStatus(i, mapStatus);
				if (fightStatus.fightId == m_ownFightId)
				{
					FightStatus.local = fightStatus;
				}
				FightLogicExecutor.AddFightStatus(fightStatus);
				array[i] = fightStatus;
			}
			GameStatus.Initialize((FightType)m_fightInfo.FightType, m_fightDefinition, array);
			yield return current.Initialize();
			VisualEffectFactory.Initialize();
			yield return FightObjectFactory.Load();
			if (!FightObjectFactory.isReady)
			{
				yield break;
			}
			yield return FightSpellEffectFactory.Load(fightCount);
			if (!FightSpellEffectFactory.isReady)
			{
				yield break;
			}
			yield return FightUIFactory.Load();
			if (!FightUIFactory.isReady)
			{
				yield break;
			}
			UILoader<FightUIRework> loaderRework = new UILoader<FightUIRework>(this, "FightUIRework", "core/scenes/ui/fight");
			yield return loaderRework.Load();
			m_uiRework = loaderRework.ui;
			m_uiRework.Init(GameStatus.fightType, m_fightDefinition);
			frame = new FightFrame
			{
				onOtherPlayerLeftFight = OnOtherPlayerLeftFight
			};
			if (m_hardResumed)
			{
				FightSnapshot snapshot = null;
				frame.onFightSnapshot = delegate(FightSnapshot fightSnapshot)
				{
					snapshot = fightSnapshot;
				};
				frame.SendFightSnapshotRequest();
				while (snapshot == null)
				{
					yield return null;
				}
				frame.onFightSnapshot = null;
				yield return ApplyFightSnapshot(snapshot);
			}
			frame.SendPlayerReady();
			while (!FightLogicExecutor.fightInitialized)
			{
				yield return null;
			}
			yield return uiRework.Load();
		}

		private IEnumerator LoadFightMap()
		{
			string fightMapBundleName = AssetBundlesUtility.GetFightMapAssetBundleName(m_fightMapId);
			string fightMapSceneName = ScenesUtility.GetFightMapSceneName(m_fightMapId);
			yield return LoadSceneAndBundleRequest(fightMapSceneName, fightMapBundleName);
			Scene sceneByName = SceneManager.GetSceneByName(fightMapSceneName);
			if (!sceneByName.IsValid())
			{
				Log.Error("Could not load scene named '" + fightMapSceneName + "' from bundle named '" + fightMapBundleName + "'.", 217, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\States\\FightState.cs");
				yield break;
			}
			m_fightMapScene = sceneByName;
			FightMap componentInRootGameObjects = ScenesUtility.GetComponentInRootGameObjects<FightMap>(sceneByName);
			if (null == componentInRootGameObjects)
			{
				Log.Error("Could not find a FightMap in scene named '" + fightMapSceneName + "'.", 226, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\States\\FightState.cs");
				yield break;
			}
			FightMap.current = componentInRootGameObjects;
			MapRenderSettings.ApplyToScene(FightMap.current.ambience);
		}

		private IEnumerator ApplyFightSnapshot(FightSnapshot snapshot)
		{
			yield break;
		}

		protected override void Enable()
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			m_uiRework.OnQuitRequest += OnQuitClick;
			m_uiRework.onTurnEndButtonClick = EndTurn;
			SceneManager.SetActiveScene(m_fightMapScene);
			RenderSettings.set_ambientLight(FightMap.current.ambience.lightSettings.ambientColor);
			FightLogicExecutor.Start();
			FightMap current = FightMap.current;
			if (null != current)
			{
				current.Begin();
			}
		}

		protected unsafe override bool UseInput(InputState inputState)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			if ((int)((IntPtr)(void*)inputState).state != 1)
			{
				return this.UseInput(inputState);
			}
			int id = ((IntPtr)(void*)inputState).id;
			if (id == 2)
			{
				if (m_uiRework != null)
				{
					m_uiRework.SimulateClickTurnEndButton();
				}
				return true;
			}
			return this.UseInput(inputState);
		}

		protected override void Disable()
		{
			if (frame != null)
			{
				frame.Dispose();
				frame = null;
			}
			m_uiRework.OnQuitRequest -= OnQuitClick;
			m_uiRework.onTurnEndButtonClick = null;
			FightMap current = FightMap.current;
			if (null != current)
			{
				current.End();
			}
			FightLogicExecutor.Stop();
		}

		protected override IEnumerator Unload()
		{
			while (FightLogicExecutor.isValid)
			{
				yield return null;
			}
			FightMap current = FightMap.current;
			if (null != current)
			{
				current.Release();
				FightMap.current = null;
			}
			if (FightSpellEffectFactory.isReady)
			{
				yield return FightSpellEffectFactory.Unload();
			}
			if (FightObjectFactory.isReady)
			{
				yield return FightObjectFactory.Unload();
			}
			if (FightUIFactory.isReady)
			{
				yield return FightUIFactory.Unload();
			}
			VisualEffectFactory.Dispose();
			yield return RuntimeData.UnloadTextCollectionAsync("Fight");
			yield return _003C_003En__0();
			DragNDropListener.instance.CancelSnapDrag();
			instance = null;
		}

		public IEnumerator ShowFightEndFeedback(FightStatusEndReason endReason)
		{
			FightEndFeedbackState feedbackState = new FightEndFeedbackState(endReason);
			this.SetChildState(feedbackState, 0);
			while (feedbackState.isActive)
			{
				yield return null;
			}
		}

		public void GotoFightEndState(FightResult result, GameStatistics gameStatistics, int fightTime)
		{
			FightEndedState fightEndedState = new FightEndedState(result, gameStatistics, fightTime);
			this.SetChildState(fightEndedState, 0);
		}

		public void LeaveAndGotoMainState()
		{
			RuntimeData.currentKeywordContext = KeywordContext.FightSolo;
			frame.SendLeave();
			StatesUtility.GotoMainMenu();
		}

		private void OnQuitClick()
		{
			if (FightStatus.local.isEnded)
			{
				DisplayQuitPopup();
			}
			else
			{
				PopupInfoManager.Show(StateManager.GetDefaultLayer().GetChainEnd(), new PopupInfo
				{
					message = 99373,
					buttons = new ButtonData[2]
					{
						new ButtonData(9912, OnAcceptResign, closeOnClick: true, ButtonStyle.Negative),
						new ButtonData(68421, OnRefuseQuit)
					},
					selectedButton = 2,
					style = PopupStyle.Normal,
					useBlur = true
				});
			}
		}

		public void DisplayQuitPopup()
		{
			PopupInfoManager.Show(StateManager.GetDefaultLayer().GetChainEnd(), new PopupInfo
			{
				message = 76997,
				buttons = new ButtonData[2]
				{
					new ButtonData(9912, LeaveAndGotoMainState, closeOnClick: true, ButtonStyle.Negative),
					new ButtonData(68421, OnRefuseQuit)
				},
				selectedButton = 2,
				style = PopupStyle.Normal,
				useBlur = true
			});
		}

		private void OnRefuseQuit()
		{
			FightStatus local = FightStatus.local;
			if (local != null && !local.isEnded)
			{
				FightUIRework instance = FightUIRework.instance;
				if (null != instance)
				{
					instance.SetResignButtonEnabled(value: true);
				}
			}
		}

		private void OnAcceptResign()
		{
			frame.SendResign();
		}

		private void OnOtherPlayerLeftFight(int playerId)
		{
		}

		private void EndTurn()
		{
			frame.SendTurnEnd(FightStatus.local.turnIndex);
		}
	}
}
