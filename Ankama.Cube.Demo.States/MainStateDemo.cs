using Ankama.AssetManagement;
using Ankama.AssetManagement.InputManagement;
using Ankama.AssetManagement.StateManagement;
using Ankama.Cube.Data;
using Ankama.Cube.Demo.UI;
using Ankama.Cube.Network;
using Ankama.Cube.States;
using Ankama.Cube.TEMPFastEnterMatch.Player;
using Ankama.Cube.UI;
using Ankama.Utilities;
using System;
using System.Collections;
using UnityEngine;

namespace Ankama.Cube.Demo.States
{
	public class MainStateDemo : LoadSceneStateContext, IStateUITransitionPriority
	{
		public enum SubState
		{
			GodSelection,
			CharacterSelection,
			GameSelection,
			Matchmaking1V1,
			Matchmaking3V3,
			Matchmaking4VBoss
		}

		private const float MinLoadingTime = 3f;

		private float m_startLoadingTime;

		private MainUIDemo m_ui;

		private God m_god;

		public UIPriority uiTransitionPriority => UIPriority.Back;

		public MainStateDemo()
		{
			GodSelectionState godSelectionState = new GodSelectionState
			{
				fromSide = SlidingSide.Right
			};
			GodSelectionState godSelectionState2 = godSelectionState;
			godSelectionState2.onGodSelected = (Action<God>)Delegate.Combine(godSelectionState2.onGodSelected, new Action<God>(OnChildStateOnGodSelected));
			this.SetChildState(godSelectionState, 0);
		}

		protected override IEnumerator Load()
		{
			UILoader<MainUIDemo> loader = new UILoader<MainUIDemo>(this, "MainUIDemo", "demo/scenes/ui/main", disableOnLoad: true);
			yield return loader.Load();
			m_ui = loader.ui;
			m_ui.SetStateIndex(0, tween: false);
			m_ui.get_gameObject().SetActive(false);
		}

		protected override void Enable()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Expected O, but got Unknown
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected O, but got Unknown
			StateManager.RegisterInputDefinition(new InputKeyCodeDefinition(8, 8, 0, 0.4f, 0.1f));
			StateManager.RegisterInputDefinition(new InputKeyCodeDefinition(275, 6, 0, 0.4f, 0.1f));
			StateManager.RegisterInputDefinition(new InputKeyCodeDefinition(276, 7, 0, 0.4f, 0.1f));
			m_ui.onReturn = OnReturnClick;
		}

		protected override IEnumerator Update()
		{
			m_ui.get_gameObject().SetActive(true);
			m_ui.Open();
			yield break;
		}

		protected override void Disable()
		{
			StateManager.UnregisterInputDefinition(8);
			StateManager.UnregisterInputDefinition(6);
			StateManager.UnregisterInputDefinition(7);
			m_ui.get_gameObject().SetActive(false);
			m_ui.onReturn = null;
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
			if (id == 8)
			{
				if (null != m_ui)
				{
					m_ui.SimulateReturnClick();
				}
				return true;
			}
			return this.UseInput(inputState);
		}

		public override bool AllowsTransition(StateContext nextState)
		{
			if (!(nextState is FightState))
			{
				return nextState is LoginStateDemo;
			}
			return true;
		}

		protected override IEnumerator Transition(StateTransitionInfo transitionInfo)
		{
			if (transitionInfo == null || transitionInfo.get_stateContext() == null)
			{
				yield break;
			}
			if (transitionInfo.get_stateContext() is LoginStateDemo)
			{
				yield return m_ui.CloseCoroutine();
			}
			else
			{
				if (!(transitionInfo.get_stateContext() is FightState))
				{
					yield break;
				}
				m_startLoadingTime = Time.get_time();
				while ((int)transitionInfo.get_stateContext().get_loadState() != 2)
				{
					yield return null;
					if (transitionInfo.get_stateContext() == null)
					{
						yield break;
					}
				}
				while (Time.get_time() - m_startLoadingTime < 3f)
				{
					yield return null;
				}
				yield return m_ui.GotoFightAnim();
			}
		}

		private void GotoSubState(SubState state, SlidingSide fromSide, bool tween = true)
		{
			InactivityHandler.UpdateActivity();
			if (state == SubState.GodSelection)
			{
				GodSelectionState godSelectionState = new GodSelectionState();
				godSelectionState.onGodSelected = (Action<God>)Delegate.Combine(godSelectionState.onGodSelected, new Action<God>(OnGodSelected));
				m_ui.SetStateIndex(0, tween);
				BaseFightSelectionState baseFightSelectionState = godSelectionState;
				m_ui.returnButton.set_interactable(false);
				baseFightSelectionState.fromSide = fromSide;
				baseFightSelectionState.onUIOpeningFinished = OnChildUIOpeningFinished;
				this.SetChildState(baseFightSelectionState, 0);
				return;
			}
			throw new ArgumentOutOfRangeException("state", state, null);
		}

		private void OnChildStateOnGodSelected(God god)
		{
			m_god = god;
			GotoSubState(SubState.CharacterSelection, SlidingSide.Right);
		}

		private void OnGodSelected(God god)
		{
			m_god = god;
			GotoSubState(SubState.CharacterSelection, SlidingSide.Right);
		}

		private void OnDeckSelected(int deckId)
		{
			GotoSubState(SubState.GameSelection, SlidingSide.Right);
			PlayerData instance = PlayerData.instance;
			SquadDefinition squadDefinition = RuntimeData.squadDefinitions[deckId];
			WeaponDefinition weaponDefinition = RuntimeData.weaponDefinitions[squadDefinition.weapon.value];
			m_ui.playerAvatar.nickname = instance.nickName;
			m_ui.playerAvatar.weaponDefinition = weaponDefinition;
			m_ui.ShowPlayerAvatarAnim(value: true);
		}

		private void OnGameSelected(int gameIdx)
		{
			SubState state = SubState.Matchmaking1V1;
			switch (gameIdx)
			{
			case 0:
				state = SubState.Matchmaking3V3;
				break;
			case 1:
				state = SubState.Matchmaking1V1;
				break;
			case 2:
				state = SubState.Matchmaking4VBoss;
				break;
			}
			GotoSubState(state, SlidingSide.Right);
			m_ui.ShowStepMenuAnim(value: false);
		}

		private void OnChildUIOpeningFinished()
		{
			m_ui.returnButton.set_interactable(true);
		}

		private void OnReturnClick()
		{
			m_ui.returnButton.set_interactable(false);
			StateContext childState = this.GetChildState();
			GotoPreviousState(childState);
		}

		public void GotoPreviousState(StateContext currentChildState)
		{
			BaseFightSelectionState baseFightSelectionState = currentChildState as BaseFightSelectionState;
			if (baseFightSelectionState != null)
			{
				baseFightSelectionState.toSide = SlidingSide.Right;
			}
			if (currentChildState is GodSelectionState)
			{
				ConnectionHandler.instance.Disconnect();
				this.get_parent().SetChildState(new LoginStateDemo(), 0);
			}
			else
			{
				Log.Error("We don't know where we are, this Should not happend", 330, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Demo\\Code\\States\\MainStateDemo.cs");
			}
		}
	}
}
