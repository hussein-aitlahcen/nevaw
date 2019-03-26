using Ankama.Cube.SRP;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ankama.Cube.UI
{
	public class OptionUI : BaseOpenCloseUI
	{
		[SerializeField]
		private Button m_closeButton;

		[SerializeField]
		private TabButton[] m_categoryButtons;

		[SerializeField]
		private OptionCategory[] m_category;

		[SerializeField]
		private float m_transitionDuration = 0.15f;

		private OptionCategory m_selectedCategory;

		private OptionCategory m_previousCategory;

		private Sequence m_transitionTweenSequence;

		public Action onCloseClick;

		public unsafe void Initialise()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			m_closeButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		}

		private unsafe void Start()
		{
			for (int i = 0; i < m_categoryButtons.Length; i++)
			{
				m_categoryButtons[i].set_isOn(i == 0);
				m_categoryButtons[i].onValueChanged.AddListener(new UnityAction<bool>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			}
			for (int j = 0; j < m_category.Length; j++)
			{
				OptionCategory optionCategory = m_category[j];
				optionCategory.get_gameObject().SetActive(true);
				if (j == 0)
				{
					m_selectedCategory = optionCategory;
					optionCategory.SetVisible(value: true);
					optionCategory.alpha = 1f;
				}
				else
				{
					optionCategory.SetVisible(value: false);
					optionCategory.alpha = 0f;
				}
			}
		}

		private void OnCategorySelected(bool value)
		{
			if (!value)
			{
				return;
			}
			int num = 0;
			while (true)
			{
				if (num < m_categoryButtons.Length)
				{
					if (m_categoryButtons[num].get_isOn())
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			ShowCategory(m_category[num]);
		}

		private unsafe void ShowCategory(OptionCategory selectedCategory)
		{
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Expected O, but got Unknown
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Expected O, but got Unknown
			if (selectedCategory == m_selectedCategory)
			{
				return;
			}
			if (m_transitionTweenSequence != null && TweenExtensions.IsActive(m_transitionTweenSequence))
			{
				TweenExtensions.Kill(m_transitionTweenSequence, false);
				if (m_previousCategory != null)
				{
					m_previousCategory.SetVisible(value: false);
				}
			}
			m_previousCategory = m_selectedCategory;
			m_selectedCategory = selectedCategory;
			m_transitionTweenSequence = DOTween.Sequence();
			if (m_previousCategory != null)
			{
				TweenSettingsExtensions.Append(m_transitionTweenSequence, m_previousCategory.DoFade(0f, m_transitionDuration));
				TweenSettingsExtensions.AppendCallback(m_transitionTweenSequence, new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			}
			if (m_selectedCategory != null)
			{
				TweenSettingsExtensions.AppendCallback(m_transitionTweenSequence, new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
				TweenSettingsExtensions.Append(m_transitionTweenSequence, m_selectedCategory.DoFade(1f, m_transitionDuration));
			}
		}

		private void OnFadeInStart()
		{
			m_selectedCategory.SetVisible(value: true);
			m_selectedCategory.alpha = 0f;
		}

		private void OnFadeOutComplete()
		{
			m_previousCategory.SetVisible(value: false);
		}

		private void OnBlurChanged(int index)
		{
			QualityManager.get_current().set_uiBlurQuality(index);
		}

		public void OnCloseClick()
		{
			onCloseClick?.Invoke();
		}

		public void SimulateCloseClick()
		{
			InputUtility.SimulateClickOn(m_closeButton);
		}
	}
}
