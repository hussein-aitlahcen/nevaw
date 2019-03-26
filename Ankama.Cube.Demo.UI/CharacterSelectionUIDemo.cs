using Ankama.Cube.Data;
using Ankama.Cube.TEMPFastEnterMatch.Player;
using Ankama.Cube.UI;
using Ankama.Utilities;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace Ankama.Cube.Demo.UI
{
	public class CharacterSelectionUIDemo : BaseFightSelectionUI
	{
		[SerializeField]
		private Button m_selectButton;

		[SerializeField]
		private CharacterPanelList m_characterList;

		[SerializeField]
		private CharacterPanel m_characterPrefab;

		[SerializeField]
		private SlidingAnimUI m_buttonSlidingAnim;

		[SerializeField]
		private DemoData m_fakeData;

		public Action<int> onSelect;

		private List<Tuple<SquadDefinition, SquadFakeData>> m_displayedSquad;

		private unsafe void Start()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Expected O, but got Unknown
			m_characterList.elementWidth = (int)((IntPtr)(void*)m_characterPrefab.get_transform().get_sizeDelta()).x;
			m_characterPrefab.get_gameObject().SetActive(false);
			m_selectButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			CharacterPanelList characterList = m_characterList;
			characterList.onElementSelected = (Action<int>)Delegate.Combine(characterList.onElementSelected, (Action<int>)delegate
			{
				UpdateInactivityTime();
			});
		}

		public void Init(God god)
		{
			List<Tuple<SquadDefinition, SquadFakeData>> displayedSquads = GetDisplayedSquads(god);
			for (int i = 0; i < m_fakeData.squadNbElementLockedBefore; i++)
			{
				CharacterPanel characterPanel = CreateCharacterPanel();
				characterPanel.Set(null, null);
				m_characterList.Add(characterPanel);
			}
			int count = displayedSquads.Count;
			for (int j = 0; j < count; j++)
			{
				Tuple<SquadDefinition, SquadFakeData> tuple = displayedSquads[j];
				CharacterPanel characterPanel2 = CreateCharacterPanel();
				characterPanel2.Set(tuple.Item1, tuple.Item2);
				m_characterList.Add(characterPanel2);
			}
			for (int k = 0; k < m_fakeData.squadNbElementLockedAfter; k++)
			{
				CharacterPanel characterPanel3 = CreateCharacterPanel();
				characterPanel3.Set(null, null);
				m_characterList.Add(characterPanel3);
			}
			m_characterList.lockedLeft = m_fakeData.squadNbElementLockedBefore;
			m_characterList.lockedright = m_fakeData.squadNbElementLockedAfter;
			m_characterList.SetSelectedIndex(GetSelectedIndex(displayedSquads), tween: false, selectCallback: false);
			m_selectButton.set_interactable(displayedSquads.Count > 0);
			m_displayedSquad = displayedSquads;
		}

		private List<Tuple<SquadDefinition, SquadFakeData>> GetDisplayedSquads(God god)
		{
			List<Tuple<SquadDefinition, SquadFakeData>> list = new List<Tuple<SquadDefinition, SquadFakeData>>();
			SquadFakeData[] squads = m_fakeData.squads;
			int num = squads.Length;
			for (int i = 0; i < num; i++)
			{
				SquadFakeData squadFakeData = squads[i];
				WeaponDefinition value2;
				if (!RuntimeData.squadDefinitions.TryGetValue(squadFakeData.id, out SquadDefinition value))
				{
					Log.Error($"Cannot find squad definition with id {squadFakeData.id}", 91, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Demo\\Code\\UI\\CharacterSelection\\CharacterSelectionUIDemo.cs");
				}
				else if (!RuntimeData.weaponDefinitions.TryGetValue(value.weapon.value, out value2))
				{
					Log.Error($"Cannot find weapon definition with id {value.weapon.value}", 98, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Demo\\Code\\UI\\CharacterSelection\\CharacterSelectionUIDemo.cs");
				}
				else if (value2.god == god)
				{
					list.Add(new Tuple<SquadDefinition, SquadFakeData>(value, squadFakeData));
				}
			}
			return list;
		}

		private int GetSelectedIndex(List<Tuple<SquadDefinition, SquadFakeData>> displayedList)
		{
			int num = -1;
			if (!m_fakeData.resetSelection)
			{
				int currentDeckId = PlayerData.instance.currentDeckId;
				if (currentDeckId < 0 && RuntimeData.squadDefinitions.TryGetValue(-currentDeckId, out SquadDefinition value))
				{
					int count = displayedList.Count;
					for (int i = 0; i < count; i++)
					{
						if (displayedList[i].Item1.get_id() == value.get_id())
						{
							num = i;
							break;
						}
					}
				}
			}
			if (num < 0)
			{
				num = 0;
			}
			return num + m_fakeData.squadNbElementLockedBefore;
		}

		private CharacterPanel CreateCharacterPanel()
		{
			CharacterPanel characterPanel = Object.Instantiate<CharacterPanel>(m_characterPrefab);
			characterPanel.get_gameObject().SetActive(true);
			return characterPanel;
		}

		public void SimulateRightArrowClick()
		{
			InputUtility.SimulateClickOn(m_characterList.rightButton);
		}

		public void SimulateLeftArrowClick()
		{
			InputUtility.SimulateClickOn(m_characterList.leftButton);
		}

		public void SimulateSelectClick()
		{
			InputUtility.SimulateClickOn(m_selectButton);
		}

		private void OnSelectClick()
		{
			int id = m_displayedSquad[m_characterList.selectedIndex - m_fakeData.squadNbElementLockedBefore].Item2.id;
			onSelect?.Invoke(id);
		}

		public override IEnumerator OpenFrom(SlidingSide side)
		{
			Sequence buttonSequence = m_buttonSlidingAnim.PlayAnim(open: true, side, side == SlidingSide.Left);
			Sequence elemSequence = m_characterList.TransitionAnim(open: true, side);
			m_openDirector.set_time(0.0);
			m_openDirector.Play();
			while (true)
			{
				if (!TweenExtensions.IsActive(elemSequence) && !TweenExtensions.IsActive(buttonSequence))
				{
					PlayableGraph playableGraph = m_openDirector.get_playableGraph();
					if (!playableGraph.IsValid())
					{
						break;
					}
					playableGraph = m_openDirector.get_playableGraph();
					if (playableGraph.IsDone())
					{
						break;
					}
				}
				yield return null;
			}
		}

		public override IEnumerator CloseTo(SlidingSide side)
		{
			Sequence buttonSequence = m_buttonSlidingAnim.PlayAnim(open: false, side, side == SlidingSide.Right);
			Sequence elemSequence = m_characterList.TransitionAnim(open: false, side);
			m_closeDirector.set_time(0.0);
			m_closeDirector.Play();
			while (true)
			{
				if (!TweenExtensions.IsActive(elemSequence) && !TweenExtensions.IsActive(buttonSequence))
				{
					PlayableGraph playableGraph = m_closeDirector.get_playableGraph();
					if (!playableGraph.IsValid())
					{
						break;
					}
					playableGraph = m_closeDirector.get_playableGraph();
					if (playableGraph.IsDone())
					{
						break;
					}
				}
				yield return null;
			}
		}
	}
}
