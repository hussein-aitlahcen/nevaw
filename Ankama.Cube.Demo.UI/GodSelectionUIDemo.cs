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
	public class GodSelectionUIDemo : BaseFightSelectionUI
	{
		[SerializeField]
		private Button m_selectButton;

		[SerializeField]
		private GodPanelList m_godList;

		[SerializeField]
		private GodPanel m_godPanelPrefab;

		[SerializeField]
		private SlidingAnimUI m_buttonSlidingAnim;

		[SerializeField]
		private DemoData m_fakeData;

		public Action<God> onSelect;

		private unsafe void Start()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Expected O, but got Unknown
			m_godList.elementWidth = (int)((IntPtr)(void*)m_godPanelPrefab.get_transform().get_sizeDelta()).x;
			m_godPanelPrefab.get_gameObject().SetActive(false);
			m_selectButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			GodPanelList godList = m_godList;
			godList.onElementSelected = (Action<int>)Delegate.Combine(godList.onElementSelected, new Action<int>(OnGodSelected));
		}

		public void Init()
		{
			List<Tuple<GodDefinition, GodFakeData>> displayedGods = GetDisplayedGods();
			int count = displayedGods.Count;
			for (int i = 0; i < m_fakeData.godNbElementLockedBefore; i++)
			{
				GodPanel godPanel = CreateGodPanel();
				godPanel.Set(null, null);
				m_godList.Add(godPanel);
			}
			for (int j = 0; j < count; j++)
			{
				Tuple<GodDefinition, GodFakeData> tuple = displayedGods[j];
				if (!tuple.Item2.locked)
				{
					GodPanel godPanel2 = CreateGodPanel();
					godPanel2.Set(tuple.Item1, tuple.Item2);
					m_godList.Add(godPanel2);
				}
			}
			int num = 0;
			for (int k = 0; k < count; k++)
			{
				if (displayedGods[k].Item2.locked)
				{
					GodPanel godPanel3 = CreateGodPanel();
					godPanel3.Set(null, null);
					m_godList.Add(godPanel3);
					num++;
				}
			}
			for (int l = 0; l < m_fakeData.godNbElementLockedAfter; l++)
			{
				GodPanel godPanel4 = CreateGodPanel();
				godPanel4.Set(null, null);
				m_godList.Add(godPanel4);
				num++;
			}
			m_godList.lockedLeft = m_fakeData.godNbElementLockedBefore;
			m_godList.lockedright = num;
			m_godList.SetSelectedIndex(GetSelectedIndex(displayedGods), tween: false, selectCallback: false);
			m_selectButton.set_interactable(displayedGods.Count > 0);
		}

		private List<Tuple<GodDefinition, GodFakeData>> GetDisplayedGods()
		{
			List<Tuple<GodDefinition, GodFakeData>> list = new List<Tuple<GodDefinition, GodFakeData>>();
			GodFakeData[] gods = m_fakeData.gods;
			int num = gods.Length;
			for (int i = 0; i < num; i++)
			{
				GodFakeData godFakeData = gods[i];
				if (!RuntimeData.godDefinitions.TryGetValue(godFakeData.god, out GodDefinition value))
				{
					Log.Error($"Cannot find god definition with family {godFakeData.god}", 105, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Demo\\Code\\UI\\GodSelection\\GodSelectionUIDemo.cs");
				}
				else
				{
					list.Add(new Tuple<GodDefinition, GodFakeData>(value, godFakeData));
				}
			}
			return list;
		}

		private int GetSelectedIndex(List<Tuple<GodDefinition, GodFakeData>> displayedList)
		{
			int num = -1;
			if (!m_fakeData.resetSelection)
			{
				int currentDeckId = PlayerData.instance.currentDeckId;
				if (currentDeckId < 0 && RuntimeData.squadDefinitions.TryGetValue(currentDeckId, out SquadDefinition value))
				{
					WeaponDefinition weaponDefinition = RuntimeData.weaponDefinitions[value.weapon.value];
					GodDefinition godDefinition = RuntimeData.godDefinitions[weaponDefinition.god];
					int count = displayedList.Count;
					for (int i = 0; i < count; i++)
					{
						if (displayedList[i].Item1.god == godDefinition.god)
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
			return num + m_fakeData.godNbElementLockedBefore;
		}

		private GodPanel CreateGodPanel()
		{
			GodPanel godPanel = Object.Instantiate<GodPanel>(m_godPanelPrefab);
			godPanel.get_gameObject().SetActive(true);
			return godPanel;
		}

		public void SimulateRightArrowClick()
		{
			InputUtility.SimulateClickOn(m_godList.rightButton);
		}

		public void SimulateLeftArrowClick()
		{
			InputUtility.SimulateClickOn(m_godList.leftButton);
		}

		public void SimulateSelectClick()
		{
			InputUtility.SimulateClickOn(m_selectButton);
		}

		private void OnGodSelected(int s)
		{
			UpdateInactivityTime();
		}

		private void OnSelectClick()
		{
			God god = m_fakeData.gods[m_godList.selectedIndex - m_fakeData.godNbElementLockedBefore].god;
			onSelect?.Invoke(god);
		}

		public override IEnumerator OpenFrom(SlidingSide side)
		{
			Sequence buttonSequence = m_buttonSlidingAnim.PlayAnim(open: true, side, side == SlidingSide.Left);
			Sequence elemSequence = m_godList.TransitionAnim(open: true, side);
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
			Sequence elemSequence = m_godList.TransitionAnim(open: false, side);
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
