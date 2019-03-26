using Ankama.AssetManagement;
using Ankama.AssetManagement.InputManagement;
using Ankama.AssetManagement.StateManagement;
using Ankama.Cube.Code.UI;
using Ankama.Cube.Data;
using Ankama.Cube.Data.UI.Localization.TextFormatting;
using Ankama.Cube.Network;
using Ankama.Cube.Protocols.CommonProtocol;
using Ankama.Cube.Protocols.PlayerProtocol;
using Ankama.Cube.UI.DeckMaker;
using Ankama.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.States
{
	public class DeckEditState : LoadSceneStateContext
	{
		private List<int> m_companions;

		private DeckMakerFrame m_frame;

		private Gender m_gender;

		private Family m_god;

		private bool m_isBeingSave;

		private string m_name;

		private DeckSlot m_selectedSlot;

		private DeckSlot m_previousSlot;

		private List<int> m_spells;

		private DeckUI m_ui;

		private int m_weapon;

		private WeaponAndDeckModifications m_modifications;

		private bool m_wasValid;

		private bool m_inAnimation;

		private bool m_safeExit;

		private bool ExitAfterSave;

		public event Action OnCloseComplete;

		public void SetDeckSlot(DeckSlot slot, WeaponAndDeckModifications modifications)
		{
			m_wasValid = slot.DeckInfo.IsValid();
			m_previousSlot = slot;
			m_selectedSlot = slot.Clone();
			m_weapon = (slot.Weapon ?? 0);
			m_modifications = modifications;
		}

		public void OpenUIAnimation()
		{
			Main.monoBehaviour.StartCoroutine(GotoEdit());
		}

		private IEnumerator GotoEdit()
		{
			m_ui.SetValue(m_selectedSlot);
			m_inAnimation = true;
			EditModeSelection selection = EditModeSelection.Spell;
			yield return m_ui.GotoEdit(selection);
			m_inAnimation = false;
		}

		protected override IEnumerator Load()
		{
			RuntimeData.currentKeywordContext = KeywordContext.DeckBuilding;
			float start = Time.get_realtimeSinceStartup();
			UILoader<DeckUI> loader = new UILoader<DeckUI>(this, "DeckUI", "core/scenes/ui/deck", disableOnLoad: true);
			yield return loader.Load();
			m_ui = loader.ui;
			m_ui.get_gameObject().SetActive(true);
			float num = Time.get_realtimeSinceStartup() - start;
			Log.Info($"Scene load duration : {num}", 84, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\States\\PlayerUI\\DeckEditState.cs");
			DeckBuildingEventController deckBuildingEventController = new DeckBuildingEventController();
			deckBuildingEventController.OnCloseRequest += OnExit;
			deckBuildingEventController.OnSaveRequest += OnSave;
			deckBuildingEventController.OnCancelRequest += OnCancel;
			deckBuildingEventController.OnDeleteRequest += OnRemoveRequest;
			deckBuildingEventController.OnDeckSlotSelectionChanged += OnSelectionChanged;
			deckBuildingEventController.OnCloneRequest += OnCloneRequest;
			m_ui.eventController = deckBuildingEventController;
		}

		protected override void Enable()
		{
			m_frame = new DeckMakerFrame
			{
				onRemoveConfigResult = OnRemoveDeckResult,
				onSaveConfigResult = OnSaveResult
			};
		}

		protected override void Disable()
		{
			m_frame.Dispose();
		}

		protected override IEnumerator Unload()
		{
			yield return HideAllEnumerator();
			yield return _003C_003En__0();
		}

		private IEnumerator HideAllEnumerator()
		{
			bool wasOpen = false;
			if (m_ui != null)
			{
				wasOpen = m_ui.IsOpen();
				yield return m_ui.GotoSelectMode();
			}
			if (wasOpen && !m_safeExit && !DeckUtility.DecksAreEqual(m_previousSlot?.DeckInfo, m_selectedSlot?.DeckInfo))
			{
				OnSaveConfirm();
			}
			m_safeExit = false;
			if (wasOpen)
			{
				this.OnCloseComplete?.Invoke();
			}
			RuntimeData.currentKeywordContext = KeywordContext.FightSolo;
		}

		protected unsafe override bool UseInput(InputState inputState)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Invalid comparison between Unknown and I4
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			int id = ((IntPtr)(void*)inputState).id;
			if (id == 1 && (int)((IntPtr)(void*)inputState).state == 1 && null != m_ui && m_ui.IsOpen())
			{
				OnSave();
				return true;
			}
			return this.UseInput(inputState);
		}

		private void OnSelectionChanged(DeckSlot obj)
		{
			m_selectedSlot = obj;
		}

		private void OnSave()
		{
			if (m_inAnimation)
			{
				return;
			}
			if (!m_selectedSlot.DeckInfo.IsValid() && m_wasValid)
			{
				ButtonData[] buttons = new ButtonData[2]
				{
					new ButtonData(75192, OnSaveConfirm),
					new ButtonData(38763)
				};
				StateLayer val = default(StateLayer);
				if (StateManager.TryGetLayer("PlayerUI", ref val))
				{
					PopupInfoManager.ClearAllMessages();
					PopupInfoManager.Show(val.GetChainEnd(), new PopupInfo
					{
						title = 56031,
						message = 57158,
						buttons = buttons,
						selectedButton = 1,
						style = PopupStyle.Error
					});
				}
			}
			else if (!DeckUtility.DecksAreEqual(m_previousSlot?.DeckInfo, m_selectedSlot?.DeckInfo))
			{
				OnSaveConfirm();
			}
			else
			{
				OnExit();
			}
		}

		private void OnSaveConfirm()
		{
			m_safeExit = true;
			ExitAfterSave = true;
			DeckInfo deckInfo = m_selectedSlot.DeckInfo.TrimCopy();
			string name = string.IsNullOrWhiteSpace(deckInfo.Name) ? RuntimeData.FormattedText(92537) : deckInfo.Name;
			m_frame.SendSaveSquadRequest(deckInfo.Id, name, (Family)deckInfo.God, deckInfo.Weapon, (IReadOnlyList<int>)deckInfo.Companions, (IReadOnlyList<int>)deckInfo.Spells);
			m_ui.interactable = false;
		}

		private void OnSaveResult(SaveDeckResultEvent result)
		{
			m_isBeingSave = false;
			m_ui.interactable = true;
			if (result.Result == CmdResult.Success)
			{
				m_selectedSlot.SetId(result.DeckId);
			}
			if (m_selectedSlot.Id.HasValue && m_selectedSlot.HasDeckInfo && m_selectedSlot.DeckInfo.IsValid())
			{
				m_modifications.SetSelectedDeckForWeapon(m_weapon, m_selectedSlot.Id.Value);
				m_modifications.SetSelectedWeapon(m_weapon);
			}
			if (ExitAfterSave)
			{
				OnExit();
			}
		}

		private void OnCancel()
		{
			if (!m_inAnimation)
			{
				if (!m_selectedSlot.Preconstructed && !DeckUtility.DecksAreEqual(m_previousSlot?.DeckInfo, m_selectedSlot?.DeckInfo))
				{
					OnSave();
				}
				else
				{
					OnCancelConfirm();
				}
			}
		}

		private void OnCancelConfirm()
		{
			OnExit();
		}

		private void OnRemoveRequest()
		{
			if (!m_selectedSlot.Preconstructed)
			{
				ButtonData[] buttons = new ButtonData[2]
				{
					new ButtonData(9912, OnRemoveConfirm),
					new ButtonData(68421)
				};
				StateLayer val = default(StateLayer);
				if (StateManager.TryGetLayer("PlayerUI", ref val))
				{
					PopupInfoManager.ClearAllMessages();
					PopupInfoManager.Show(val.GetChainEnd(), new PopupInfo
					{
						title = 52822,
						message = 76361,
						buttons = buttons,
						selectedButton = 1,
						style = PopupStyle.Error
					});
				}
			}
		}

		private void OnRemoveConfirm()
		{
			DeckSlot selectedSlot = m_selectedSlot;
			if (selectedSlot.Id.HasValue)
			{
				m_ui.interactable = false;
				m_frame.SendRemoveSquadRequest(selectedSlot.Id.Value);
			}
		}

		private void OnExit()
		{
			ExitAfterSave = false;
			m_safeExit = true;
			if (!m_isBeingSave)
			{
				Main.monoBehaviour.StartCoroutine(HideAllEnumerator());
			}
		}

		private void OnRemoveDeckResult(RemoveDeckResultEvent result)
		{
			m_ui.interactable = true;
			if (result.Result == CmdResult.Success)
			{
				OnExit();
			}
		}

		private void OnCloneConfirme()
		{
			if (DeckUtility.GetRemainingSlotsForWeapon(m_weapon) != 0)
			{
				m_selectedSlot = m_selectedSlot.Clone(keepPreconstructed: false);
				RuntimeData.TryGetText(92537, out string value);
				m_selectedSlot.SetName(value);
				m_previousSlot = null;
				m_selectedSlot.DeckInfo.Id = null;
				m_ui.interactable = true;
				DeckInfo obj = new DeckInfo(m_selectedSlot.DeckInfo)
				{
					Name = RuntimeData.FormattedText(92537),
					Id = null
				};
				DeckInfo deckInfo = m_selectedSlot.DeckInfo.TrimCopy();
				m_ui.OnCloneValidate(m_selectedSlot);
				m_frame.SendSaveSquadRequest(deckInfo.Id, deckInfo.Name, (Family)deckInfo.God, deckInfo.Weapon, (IReadOnlyList<int>)deckInfo.Companions, (IReadOnlyList<int>)deckInfo.Spells);
			}
		}

		private void OnCloneCanceld()
		{
			m_selectedSlot = m_ui.OnCloneCanceled();
		}

		private void OnCloneRequest(int titleid, int desc)
		{
			PopupInfo info;
			StateLayer val2 = default(StateLayer);
			if (DeckUtility.GetRemainingSlotsForWeapon(new DeckInfo(m_selectedSlot.DeckInfo).Weapon) > 0)
			{
				StateLayer val = default(StateLayer);
				if (StateManager.TryGetLayer("PlayerUI", ref val))
				{
					ButtonData[] buttons = new ButtonData[2]
					{
						new ButtonData(48064, OnCloneConfirme),
						new ButtonData(26918, OnCloneCanceld)
					};
					PopupInfoManager.ClearAllMessages();
					StateContext chainEnd = val.GetChainEnd();
					info = new PopupInfo
					{
						title = titleid,
						message = desc,
						buttons = buttons,
						selectedButton = 1,
						style = PopupStyle.Normal
					};
					PopupInfoManager.Show(chainEnd, info);
				}
			}
			else if (StateManager.TryGetLayer("PlayerUI", ref val2))
			{
				ButtonData[] buttons2 = new ButtonData[1]
				{
					new ButtonData(27169, OnCloneCanceld)
				};
				PopupInfoManager.ClearAllMessages();
				StateContext chainEnd2 = val2.GetChainEnd();
				info = new PopupInfo
				{
					title = 4176,
					message = 52887,
					buttons = buttons2,
					selectedButton = 1,
					style = PopupStyle.Normal
				};
				PopupInfoManager.Show(chainEnd2, info);
			}
		}
	}
}
