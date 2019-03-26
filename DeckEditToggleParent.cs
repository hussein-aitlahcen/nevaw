using Ankama.Cube.UI.Components;
using Ankama.Cube.UI.DeckMaker;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeckEditToggleParent : MonoBehaviour
{
	[SerializeField]
	private List<DeckEditToggleFilter> m_filtersToogle;

	[SerializeField]
	private InputTextField m_searchTextField;

	private float m_outYPosition;

	private Action m_refreshFilter;

	private string m_previousText;

	private RectTransform m_rect;

	public IEnumerable<DeckEditToggleFilter> ToggleFilter => m_filtersToogle;

	public unsafe void Initialise(Action onFilterChange, float outPosition)
	{
		m_previousText = m_searchTextField.GetText();
		m_rect = this.GetComponent<RectTransform>();
		m_refreshFilter = onFilterChange;
		foreach (DeckEditToggleFilter item in m_filtersToogle)
		{
			item.Initialise(OnSubFilterChange);
		}
		m_outYPosition = outPosition;
		m_searchTextField.onValueChanged.AddListener(new UnityAction<string>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
	}

	protected void OnSubFilterChange(bool b)
	{
		m_refreshFilter?.Invoke();
	}

	private void OnTextChange(string t)
	{
		if (string.Compare(m_previousText, t) != 0)
		{
			m_previousText = t;
			m_refreshFilter();
		}
	}

	public void OnEditModeChange(EditModeSelection selection)
	{
		foreach (DeckEditToggleFilter item in m_filtersToogle)
		{
			item.OnEditModeChange(selection);
		}
		m_searchTextField.SetText("");
	}

	public Tween TweenOut(float duration, Ease ease)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		return TweenSettingsExtensions.SetEase<Tweener>(DOTweenModuleUI.DOAnchorPosY(m_rect, m_outYPosition, duration, false), ease);
	}

	public Tween TweenIn(float duration, Ease ease)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		return TweenSettingsExtensions.SetEase<Tweener>(DOTweenModuleUI.DOAnchorPosY(m_rect, 0f, duration, false), ease);
	}

	public string GetTextFilter()
	{
		return m_searchTextField.GetText();
	}

	public DeckEditToggleParent()
		: this()
	{
	}
}
