using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Components
{
	public class DynamicListPageScroller : MonoBehaviour
	{
		[SerializeField]
		private DynamicList m_list;

		[SerializeField]
		private Button m_previousButton;

		[SerializeField]
		private Button m_nextButton;

		[SerializeField]
		private RawTextField m_pageText;

		private bool m_scrolling;

		private unsafe void Awake()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Expected O, but got Unknown
			m_previousButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_nextButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_list.OnSetValues += OnContentUpdate;
			m_list.OnInsertion += UpdatePageCount2;
			m_list.OnRemoved += UpdatePageCount2;
			OnContentUpdate();
		}

		private void PreviousButton()
		{
			this.StartCoroutine(PreviousButtonCoroutine());
		}

		private IEnumerator PreviousButtonCoroutine()
		{
			if (!m_scrolling)
			{
				m_scrolling = true;
				yield return m_list.ScrollToPage(m_list.currentPageIndex - 1, instant: false);
				OnContentUpdate();
				m_scrolling = false;
			}
		}

		private void NextButton()
		{
			this.StartCoroutine(NextButtonCoroutine());
		}

		private IEnumerator NextButtonCoroutine()
		{
			if (!m_scrolling)
			{
				m_scrolling = true;
				yield return m_list.ScrollToPage(m_list.currentPageIndex + 1, instant: false);
				OnContentUpdate();
				m_scrolling = false;
			}
		}

		private void UpdatePageCount2(object arg1, int arg2)
		{
			OnContentUpdate();
		}

		private void OnContentUpdate()
		{
			int num = m_list.currentPageIndex + 1;
			int pagesCount = m_list.pagesCount;
			m_previousButton.set_interactable(m_list.HasPreviousPage());
			m_nextButton.set_interactable(m_list.HastNextPage());
			m_pageText.SetText($"{num} / {pagesCount}");
		}

		public DynamicListPageScroller()
			: this()
		{
		}
	}
}
