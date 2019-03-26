using Ankama.Cube.UI.Components;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ankama.Cube.UI
{
	public class SpellListPageScroller : MonoBehaviour
	{
		[SerializeField]
		private DynamicList m_list;

		[SerializeField]
		private List<Button> m_buttons;

		[SerializeField]
		private Scrollbar m_scrollBar;

		private int m_pageCount;

		private bool m_scrolling;

		private unsafe void Awake()
		{
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Expected O, but got Unknown
			m_scrollBar.set_interactable(false);
			int i = 0;
			for (int count = m_buttons.Count; i < count; i++)
			{
				Button obj = m_buttons[i];
				int index = i;
				_003C_003Ec__DisplayClass5_0 _003C_003Ec__DisplayClass5_;
				obj.get_onClick().AddListener(new UnityAction((object)_003C_003Ec__DisplayClass5_, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			}
		}

		private void OnEnable()
		{
			m_list.OnSetValues += OnContentUpdate;
			m_list.OnInsertion += UpdatePageCount2;
			m_list.OnRemoved += UpdatePageCount2;
			m_list.OnScrollPercentage += OnScrollUpdate;
			OnContentUpdate();
		}

		private void OnDisable()
		{
			m_list.OnSetValues -= OnContentUpdate;
			m_list.OnInsertion -= UpdatePageCount2;
			m_list.OnRemoved -= UpdatePageCount2;
			m_list.OnScrollPercentage -= OnScrollUpdate;
		}

		private void ScrollTo(int i)
		{
			if (!m_scrolling)
			{
				this.StartCoroutine(ScrollToCoroutine(i));
			}
		}

		private unsafe IEnumerator ScrollToCoroutine(int i)
		{
			m_scrolling = true;
			yield return m_list.ScrollToPage(i, instant: false, new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			OnContentUpdate();
			m_scrolling = false;
		}

		private void UpdatePageCount2(object arg1, int arg2)
		{
			OnContentUpdate();
		}

		private void OnScrollUpdate(float percentage)
		{
			OnContentUpdate();
		}

		private void OnContentUpdate()
		{
			m_scrollBar.set_value(m_list.scrollPercentage);
		}

		public SpellListPageScroller()
			: this()
		{
		}
	}
}
