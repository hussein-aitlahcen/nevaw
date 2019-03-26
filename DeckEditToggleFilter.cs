using Ankama.Cube.Data;
using Ankama.Cube.UI.Components;
using Ankama.Cube.UI.DeckMaker;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DeckEditToggleFilter : MonoBehaviour
{
	[SerializeField]
	private CaracId m_element;

	[SerializeField]
	private Element m_spellElement;

	[SerializeField]
	private bool m_SpellFilter;

	[SerializeField]
	private bool m_CompanionFilter;

	[Header("Visual")]
	[SerializeField]
	private Image m_ElementPicto;

	[SerializeField]
	private Color m_defaultColor;

	[SerializeField]
	private Color m_activColor;

	private AnimatedToggleButton m_toggle;

	private Action<bool> m_onFilterChange;

	public CaracId GetElement()
	{
		return m_element;
	}

	public Element GetSpellElement()
	{
		return m_spellElement;
	}

	public bool IsEnabled()
	{
		return m_toggle.get_isOn();
	}

	public unsafe void Initialise(Action<bool> onFilterChange)
	{
		m_onFilterChange = onFilterChange;
		m_toggle = this.GetComponent<AnimatedToggleButton>();
		m_toggle.onValueChanged.AddListener(new UnityAction<bool>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
	}

	private void OnValueChange(bool on)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		m_ElementPicto.set_color(on ? m_activColor : m_defaultColor);
		m_onFilterChange?.Invoke(on);
	}

	public void OnEditModeChange(EditModeSelection selection)
	{
		switch (selection)
		{
		case EditModeSelection.Spell:
			m_toggle.set_interactable(m_SpellFilter);
			this.get_gameObject().SetActive(m_SpellFilter);
			break;
		case EditModeSelection.Companion:
			this.get_gameObject().SetActive(m_CompanionFilter);
			m_toggle.set_interactable(m_CompanionFilter);
			break;
		default:
			m_toggle.set_interactable(false);
			break;
		}
		if (!m_toggle.get_interactable())
		{
			m_toggle.set_isOn(false);
		}
	}

	public DeckEditToggleFilter()
		: this()
	{
	}
}
