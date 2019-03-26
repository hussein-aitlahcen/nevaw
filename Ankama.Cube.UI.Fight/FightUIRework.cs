using Ankama.AssetManagement;
using Ankama.AssetManagement.StateManagement;
using Ankama.Cube.Configuration;
using Ankama.Cube.Data;
using Ankama.Cube.Fight;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps;
using Ankama.Cube.Maps.Objects;
using Ankama.Cube.Protocols.FightCommonProtocol;
using Ankama.Cube.States;
using Ankama.Cube.UI.Components;
using Ankama.Cube.UI.Debug;
using Ankama.Cube.UI.Fight.Info;
using Ankama.Cube.UI.Fight.TeamCounter;
using Ankama.Cube.UI.Fight.Windows;
using Ankama.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Fight
{
	public sealed class FightUIRework : AbstractUI, IUIResourceConsumer
	{
		private enum LoadingState
		{
			None,
			Loading,
			Loaded
		}

		[Header("Components")]
		[SerializeField]
		private FightUIFactory m_factory;

		[SerializeField]
		private LocalPlayerUIRework m_localPlayerUIRework;

		[SerializeField]
		private Transform m_allyPlayersUIParent;

		[SerializeField]
		private Transform m_opponentPlayersUIParent;

		[SerializeField]
		private EndOfTurnButtonRework m_endOfTurnButton;

		[SerializeField]
		private TurnFeedbackUI m_turnFeedbackUI;

		[SerializeField]
		private FightTooltip m_fightTooltip;

		[SerializeField]
		private PlaySpellCompanionUI m_playSpellCompanionUI;

		[Header("Team HUD")]
		[SerializeField]
		private Transform m_teamPointCounterParent;

		[SerializeField]
		private Transform m_messageRibbonRootParent;

		[Header("PopupMenu")]
		[SerializeField]
		private PopupMenu m_popupMenu;

		[SerializeField]
		private Button m_popupMenuButton;

		[SerializeField]
		private Button m_optionsButton;

		[SerializeField]
		private Button m_quitButton;

		[SerializeField]
		private Button m_bugReportButton;

		private static FightUIRework s_instance;

		private static bool s_tooltipEnabled = true;

		private LoadingState m_loadingState;

		private readonly HashSet<IUIResourceProvider> m_uiResourceProviders = new HashSet<IUIResourceProvider>();

		private DebugFightUI m_debugUI;

		private TeamPointCounter m_teamPointCounter;

		private FightInfoMessageRoot m_messageRibbonRoot;

		public static FightUIRework instance => s_instance;

		public static bool tooltipsEnabled
		{
			get
			{
				return s_tooltipEnabled;
			}
			set
			{
				if (value != s_tooltipEnabled)
				{
					if (!value)
					{
						HideTooltip();
					}
					s_tooltipEnabled = value;
				}
			}
		}

		public Action onTurnEndButtonClick
		{
			set
			{
				if (null != m_endOfTurnButton)
				{
					m_endOfTurnButton.onClick = value;
				}
			}
		}

		public event Action OnQuitRequest;

		public static Vector3 WorldToUIWorld(Vector3 worldPos)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			return ScreenToWorldUI(CameraHandler.current.camera.WorldToScreenPoint(worldPos));
		}

		public static Vector3 ScreenToWorldUI(Vector3 screenPos)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			screenPos.z = s_instance.canvas.get_planeDistance();
			return s_instance.canvas.get_worldCamera().ScreenToWorldPoint(screenPos);
		}

		protected unsafe override void Awake()
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Expected O, but got Unknown
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Expected O, but got Unknown
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Expected O, but got Unknown
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Expected O, but got Unknown
			base.Awake();
			s_instance = this;
			m_turnFeedbackUI.get_gameObject().SetActive(false);
			m_popupMenuButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_optionsButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_quitButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_bugReportButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			if (ApplicationConfig.debugMode)
			{
				m_debugUI = m_factory.CreateDebugUI(this.get_transform());
			}
		}

		protected unsafe override void OnDestroy()
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Expected O, but got Unknown
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Expected O, but got Unknown
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Expected O, but got Unknown
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Expected O, but got Unknown
			base.OnDestroy();
			m_popupMenuButton.get_onClick().RemoveListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_optionsButton.get_onClick().RemoveListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_quitButton.get_onClick().RemoveListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_bugReportButton.get_onClick().RemoveListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			if (s_instance == this)
			{
				s_instance = null;
			}
		}

		public void Init(FightType fightType, FightDefinition fightDefinition)
		{
			CreateMessageRibbonRoot();
			if (fightType == FightType.TeamVersus)
			{
				CreateTeamPointCounter();
			}
		}

		public IEnumerator Load()
		{
			m_loadingState = LoadingState.Loading;
			do
			{
				yield return null;
			}
			while (m_uiResourceProviders.Count > 0);
			m_loadingState = LoadingState.Loaded;
		}

		public LocalPlayerUIRework GetLocalPlayerUI(PlayerStatus playerStatus)
		{
			return m_localPlayerUIRework;
		}

		public PlayerUIRework AddPlayer(PlayerStatus playerStatus)
		{
			Transform parent = (playerStatus.teamIndex != GameStatus.localPlayerTeamIndex) ? m_opponentPlayersUIParent : m_allyPlayersUIParent;
			return m_factory.CreatePlayerUI(playerStatus, parent);
		}

		public void SetResignButtonEnabled(bool value)
		{
			m_quitButton.set_interactable(value);
		}

		private void OnPopupMenu()
		{
			m_popupMenu.Open();
		}

		private void OnOptions()
		{
			StateLayer val = default(StateLayer);
			if (StateManager.TryGetLayer("OptionUI", ref val))
			{
				StateManager.SetActiveInputLayer(val);
				UIManager.instance.NotifyLayerIndexChange();
				OptionState optionState = new OptionState
				{
					onStateClosed = OnOptionsClosed
				};
				val.GetChainEnd().SetChildState(optionState, 0);
			}
			m_popupMenu.Close();
		}

		private void OnOptionsClosed()
		{
			StateLayer val = default(StateLayer);
			if (StateManager.TryGetLayer("OptionUI", ref val))
			{
				StateManager.DiscardInputLayer(val);
				UIManager.instance.NotifyLayerIndexChange();
			}
		}

		private void OnQuit()
		{
			m_popupMenu.Close();
			this.OnQuitRequest?.Invoke();
		}

		private void OnBugReport()
		{
			if (BugReportState.isReady)
			{
				StateLayer defaultLayer = default(StateLayer);
				if (!StateManager.TryGetLayer("OptionUI", ref defaultLayer))
				{
					defaultLayer = StateManager.GetDefaultLayer();
				}
				StateManager.SetActiveInputLayer(defaultLayer);
				UIManager.instance.NotifyLayerIndexChange();
				BugReportState bugReportState = new BugReportState();
				bugReportState.Initialize();
				defaultLayer.GetChainEnd().SetChildState(bugReportState, 0);
				m_popupMenu.Close();
			}
		}

		public static IEnumerator ShowPlayingSpell(SpellStatus spellStatus, CellObject cell)
		{
			PlaySpellCompanionUI playSpellCompanionUI = s_instance.m_playSpellCompanionUI;
			if (null != playSpellCompanionUI)
			{
				yield return playSpellCompanionUI.ShowPlaying(spellStatus, cell);
			}
		}

		public static IEnumerator ShowPlayingCompanion(ReserveCompanionStatus reserveCompanion, CellObject cell)
		{
			PlaySpellCompanionUI playSpellCompanionUI = s_instance.m_playSpellCompanionUI;
			if (null != playSpellCompanionUI)
			{
				yield return playSpellCompanionUI.ShowPlaying(reserveCompanion, cell);
			}
		}

		public static void ShowTooltip(ITooltipDataProvider tooltipDataProvider, TooltipPosition position, RectTransform rectTransform)
		{
			FightTooltip fightTooltip = GetFightTooltip();
			if (fightTooltip != null)
			{
				fightTooltip.Initialize(tooltipDataProvider);
				fightTooltip.ShowAt(position, rectTransform);
			}
		}

		public static void ShowTooltip(ITooltipDataProvider tooltipDataProvider, TooltipPosition position, Vector3 worldPosition)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			FightTooltip fightTooltip = GetFightTooltip();
			if (fightTooltip != null)
			{
				fightTooltip.Initialize(tooltipDataProvider);
				fightTooltip.ShowAt(position, worldPosition);
			}
		}

		public static void HideTooltip()
		{
			if (null == s_instance)
			{
				Log.Error("HideTooltip called while no instance exists.", 316, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\FightRework\\FightUIRework.cs");
				return;
			}
			FightTooltip fightTooltip = s_instance.m_fightTooltip;
			if (!(null == fightTooltip))
			{
				fightTooltip.Hide();
			}
		}

		private static FightTooltip GetFightTooltip()
		{
			if (null == s_instance)
			{
				Log.Error("ShowTooltip called while no instance exists.", 333, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\FightRework\\FightUIRework.cs");
				return null;
			}
			if (!s_tooltipEnabled)
			{
				return null;
			}
			return s_instance.m_fightTooltip;
		}

		public void SimulateClickTurnEndButton()
		{
			if (null != m_endOfTurnButton)
			{
				m_endOfTurnButton.SimulateClick();
			}
		}

		public void StartTurn(int turnIndex, int turnDuration, bool isLocalPlayerTeam)
		{
			if (null != m_endOfTurnButton)
			{
				if (isLocalPlayerTeam)
				{
					m_endOfTurnButton.SetState(EndOfTurnButtonRework.State.LocalPlayerTeam);
				}
				else
				{
					m_endOfTurnButton.SetState(EndOfTurnButtonRework.State.OpponentTeam);
				}
				m_endOfTurnButton.StartTurn(turnIndex, turnDuration);
			}
			if (null != m_debugUI)
			{
				m_debugUI.SetTurnIdex(turnIndex);
			}
		}

		public void StartLocalPlayerTurn()
		{
			if (null != m_endOfTurnButton)
			{
				m_endOfTurnButton.SetState(EndOfTurnButtonRework.State.LocalPlayer);
			}
		}

		public void EndLocalPlayerTurn()
		{
			if (null != m_endOfTurnButton)
			{
				m_endOfTurnButton.SetState(EndOfTurnButtonRework.State.LocalPlayerTeam);
			}
		}

		public void EndTurn()
		{
			if (null != m_endOfTurnButton)
			{
				m_endOfTurnButton.EndTurn();
			}
		}

		public IEnumerator ShowTurnFeedback(TurnFeedbackUI.Type type, int entityNameKey)
		{
			if (!RuntimeData.TryGetText(entityNameKey, out string value))
			{
				value = string.Empty;
			}
			if (null != m_turnFeedbackUI)
			{
				m_turnFeedbackUI.Show(type, value);
				do
				{
					yield return null;
				}
				while (m_turnFeedbackUI.isAnimating);
			}
		}

		public void ShowEndOfTurn()
		{
			if (null != m_endOfTurnButton)
			{
				m_endOfTurnButton.ShowEndOfTurn();
			}
		}

		public void SetScore(FightScore score, string playerOrigin, TeamsScoreModificationReason reason)
		{
			if (m_teamPointCounter != null)
			{
				m_teamPointCounter.OnScoreChange(score);
			}
			if (reason != TeamsScoreModificationReason.HeroLifeModified)
			{
				FightInfoMessage message = FightInfoMessage.Score(score, reason);
				DrawScore(message, playerOrigin);
			}
		}

		public void DrawScore(FightInfoMessage message, string playerOrigin)
		{
			if (m_messageRibbonRoot != null)
			{
				m_messageRibbonRoot.BuildAndDrawScoreMessage(message, playerOrigin);
			}
		}

		public void DrawInfoMessage(FightInfoMessage message, params string[] parameters)
		{
			if (m_messageRibbonRoot != null)
			{
				m_messageRibbonRoot.BuildAndDrawInfoMessage(message, parameters);
			}
		}

		private void CreateMessageRibbonRoot()
		{
			m_messageRibbonRoot = m_factory.CreateMessageRibbonRoot(m_messageRibbonRootParent);
		}

		private void CreateTeamPointCounter()
		{
			m_teamPointCounter = m_factory.CreateTeamPointCounter(m_teamPointCounterParent);
			m_teamPointCounter.InitialiseScore(0, 0);
		}

		public UIResourceDisplayMode Register(IUIResourceProvider provider)
		{
			if (!m_uiResourceProviders.Add(provider))
			{
				Log.Error("A UI resource provider tried to register itself multiple times.", 498, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\FightRework\\FightUIRework.cs");
			}
			if (m_loadingState != LoadingState.Loaded)
			{
				return UIResourceDisplayMode.Immediate;
			}
			return UIResourceDisplayMode.None;
		}

		public void UnRegister(IUIResourceProvider provider)
		{
			if (!m_uiResourceProviders.Remove(provider))
			{
				Log.Error("A UI resource provider tried to un-register itself but it was not registered.", 508, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\FightRework\\FightUIRework.cs");
			}
		}
	}
}
