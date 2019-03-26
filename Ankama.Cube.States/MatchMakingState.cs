using Ankama.AssetManagement;
using Ankama.Cube.Code.UI;
using Ankama.Cube.Data;
using Ankama.Cube.Protocols.FightCommonProtocol;
using Ankama.Cube.Protocols.PlayerProtocol;
using Ankama.Cube.TEMPFastEnterMatch.MatchMaking;
using Ankama.Cube.TEMPFastEnterMatch.Player;
using Ankama.Cube.Utility;
using System.Collections;

namespace Ankama.Cube.States
{
	public class MatchMakingState : LoadSceneStateContext
	{
		private MatchMakingUI m_ui;

		private MatchMakingFrame m_frame;

		protected override IEnumerator Load()
		{
			UILoader<MatchMakingUI> loader = new UILoader<MatchMakingUI>(this, "MatchmakingUI", "core/scenes/ui/matchmaking", disableOnLoad: true);
			yield return loader.Load();
			m_ui = loader.ui;
			m_ui.get_gameObject().SetActive(true);
		}

		protected override void Enable()
		{
			m_frame = new MatchMakingFrame
			{
				onGameCreated = OnGameCreated,
				onGameError = OnGameError
			};
			m_frame.OnChangedGod += OnChangedGod;
			m_frame.OnSelectedWeaponAndDeck += OnSelectedDeckAndWeapon;
			PlayerData.instance.OnSelectedDeckUpdated += OnSelectedDeckUpdated;
			m_ui.onPlayRequested = OnPlayRequested;
			m_ui.onCancelRequested = OnCancelRequested;
			m_ui.onForceAiRequested = OnForceAiRequested;
			m_ui.onReturnClicked = StatesUtility.GotoDimensionState;
			m_ui.onGodSelectedChanged += OnSelectedGodChanged;
			m_ui.onSelectedWeaponChanged += OnSelectedWeaponChanged;
			m_ui.onSelectedDeckChanged += OnSelectedDeckChanged;
		}

		protected override void Disable()
		{
			m_ui.onPlayRequested = null;
			m_ui.onCancelRequested = null;
			m_ui.onReturnClicked = null;
			m_ui.onForceAiRequested = null;
			m_ui.get_gameObject().SetActive(false);
			m_frame.Dispose();
		}

		private void OnSelectedGodChanged(God god)
		{
			m_frame.UpdatePlayerGod(god);
			m_ui.interactable = false;
		}

		private void OnSelectedWeaponChanged(int weaponId)
		{
			m_ui.OnWeaponChanged(weaponId);
			m_ui.interactable = false;
		}

		private void OnSelectedDeckChanged(int deckId)
		{
			if (PlayerData.instance.TryGetDeckById(deckId, out DeckInfo deckInfo))
			{
				int? deckId2 = (deckInfo.Id > 0) ? deckInfo.Id : null;
				m_frame.SelectDeckAndWeapon(deckInfo.Weapon, deckId2);
				m_ui.interactable = false;
			}
		}

		private void OnChangedGod(ChangeGodResultEvent obj)
		{
			m_ui.OnGodChanged();
			m_ui.interactable = true;
		}

		private void OnSelectedDeckAndWeapon(SelectDeckAndWeaponResultEvent obj)
		{
			m_ui.interactable = true;
		}

		private void OnSelectedDeckUpdated()
		{
			PlayerData.instance.TryGetDeckById(PlayerData.instance.currentDeckId, out DeckInfo deckInfo);
			m_ui.SetCurrentDeck(deckInfo);
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

		private void OnGameCreated(FightInfo fightInfo)
		{
			StatesUtility.DoTransition(new FightState(fightInfo), StateManager.GetDefaultLayer().GetChildState());
		}

		private void OnCancelRequested()
		{
			m_frame.SendCancelGame();
		}

		private void OnForceAiRequested()
		{
			m_frame.SendForceFightVersusAI();
		}

		private void OnPlayRequested(int fightDefId, int? forcedLevel)
		{
			m_frame.SendCreateGame(fightDefId, PlayerData.instance.currentDeckId, forcedLevel);
		}
	}
}
