using Ankama.Cube;
using Ankama.Cube.Data;
using Ankama.Cube.Data.UI.Localization.TextFormatting;
using Ankama.Cube.Protocols.PlayerProtocol;
using Ankama.Cube.TEMPFastEnterMatch.Player;
using Ankama.Cube.UI;
using Ankama.Cube.UI.Components;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WorldUI : AbstractUI
{
	private static WorldUI s_instance;

	[SerializeField]
	private Button m_pvpButton;

	[SerializeField]
	private Button m_gotoTofuButton;

	[SerializeField]
	private Button m_gotoGardenButton;

	[SerializeField]
	private Button m_gotoHomeButton;

	[SerializeField]
	private Button m_gotoCityButton;

	[SerializeField]
	private Button m_squadMakerButton;

	[SerializeField]
	private CustomDropdown m_godChoiceDropdown;

	[SerializeField]
	private Button m_disconnectButton;

	[SerializeField]
	private TextField m_currentDeckName;

	public Action onPvp;

	public Action onGotoDungeon;

	public Action onGotoGarden;

	public Action onGotoHome;

	public Action onGotoCity;

	public Action onDisconnect;

	public Action onDeckMaker;

	private readonly List<God> m_playableGods = new List<God>();

	public event Action<God> onGodSelectedChanged;

	[UsedImplicitly]
	protected unsafe override void Awake()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		s_instance = this;
		m_pvpButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		m_gotoTofuButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		m_gotoGardenButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		m_gotoHomeButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		m_gotoCityButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		m_disconnectButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		m_squadMakerButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		m_gotoHomeButton.set_interactable(false);
		m_gotoTofuButton.set_interactable(true);
		m_gotoGardenButton.set_interactable(true);
		m_gotoCityButton.set_interactable(true);
		InitGodsChoice();
	}

	private void OnDisconnect()
	{
		onDisconnect?.Invoke();
	}

	private void RoomSelected(Button button)
	{
		m_gotoTofuButton.set_interactable(m_gotoTofuButton != button);
		m_gotoGardenButton.set_interactable(m_gotoGardenButton != button);
		m_gotoHomeButton.set_interactable(m_gotoHomeButton != button);
		m_gotoCityButton.set_interactable(m_gotoCityButton != button);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		s_instance = null;
	}

	private unsafe void InitGodsChoice()
	{
		m_playableGods.Clear();
		List<string> list = new List<string>();
		God god = PlayerData.instance.god;
		int num = -1;
		int num2 = 0;
		foreach (GodDefinition value in RuntimeData.godDefinitions.Values)
		{
			if (value.playable)
			{
				m_playableGods.Add(value.god);
				list.Add(RuntimeData.FormattedText(value.i18nNameId));
				if (value.god == god)
				{
					num = num2;
				}
				num2++;
			}
		}
		m_godChoiceDropdown.ClearOptions();
		m_godChoiceDropdown.AddOptions(list);
		m_godChoiceDropdown.value = ((num >= 0) ? num : 0);
		m_godChoiceDropdown.onValueChanged.AddListener(new UnityAction<int>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
	}

	public void SetCurrentDeck(DeckInfo deckInfo)
	{
		m_pvpButton.set_interactable(deckInfo != null);
		if (deckInfo == null)
		{
			m_currentDeckName.SetText(64793);
		}
		else
		{
			m_currentDeckName.SetText(66030, new IndexedValueProvider(deckInfo.Name));
		}
	}

	private void OnGodSelected(int god)
	{
		this.onGodSelectedChanged?.Invoke(m_playableGods[god]);
	}

	public void OnPlayerHeroInfoUpdated()
	{
		InitGodsChoice();
	}
}
