using Ankama.Cube.UI.Components;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ankama.Cube.UI
{
	public class ListPageScroller : MonoBehaviour
	{
		[SerializeField]
		private DynamicList m_list;

		[SerializeField]
		private Button m_previousButton;

		[SerializeField]
		private Button m_nextButton;

		private int m_pageCount;

		private bool m_scrolling;

		private unsafe void Awake()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Expected O, but got Unknown
			m_previousButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_nextButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		}

		private void OnEnable()
		{
			m_list.OnSetValues += OnContentUpdate;
			m_list.OnInsertion += UpdatePageCount2;
			m_list.OnRemoved += UpdatePageCount2;
			m_list.OnScrollPercentage += UpdatePageCount;
			OnContentUpdate();
		}

		private void OnDisable()
		{
			m_list.OnSetValues -= OnContentUpdate;
			m_list.OnInsertion -= UpdatePageCount2;
			m_list.OnRemoved -= UpdatePageCount2;
			m_list.OnScrollPercentage -= UpdatePageCount;
		}

		private void PreviousButton()
		{
			if (!m_scrolling)
			{
				this.StartCoroutine(PreviousButtonCoroutine());
			}
		}

		private IEnumerator PreviousButtonCoroutine()
		{
			m_scrolling = true;
			yield return m_list.ScrollToPage(m_list.currentPageIndex - 1, instant: false);
			OnContentUpdate();
			m_scrolling = false;
		}

		private void NextButton()
		{
			if (!m_scrolling)
			{
				this.StartCoroutine(NextButtonCoroutine());
			}
		}

		private IEnumerator NextButtonCoroutine()
		{
			m_scrolling = true;
			yield return m_list.ScrollToPage(m_list.currentPageIndex + 1, instant: false);
			OnContentUpdate();
			m_scrolling = false;
		}

		private void UpdatePageCount(float percentage)
		{
			OnContentUpdate();
		}

		private void UpdatePageCount2(object arg1, int arg2)
		{
			OnContentUpdate();
		}

		private void OnContentUpdate()
		{
			m_previousButton.set_interactable(m_list.HasPreviousPage());
			m_nextButton.set_interactable(m_list.HastNextPage());
		}

		public ListPageScroller()
			: this()
		{
		}
	}
}
