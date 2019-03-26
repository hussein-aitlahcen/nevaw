using Ankama.Cube.UI.Components;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.UI
{
	public class ListPageDisplayer : MonoBehaviour
	{
		[SerializeField]
		private DynamicList m_list;

		[SerializeField]
		private GameObject m_pageDotContainer;

		[SerializeField]
		private Image m_pageDotPrefab;

		[SerializeField]
		private ListPageScrollerConfig m_config;

		private readonly List<Image> m_pageDots = new List<Image>();

		private int m_pageCount;

		private void Awake()
		{
			m_list.OnSetValues += OnContentUpdate;
			m_list.OnInsertion += UpdatePageCount2;
			m_list.OnRemoved += UpdatePageCount2;
			m_list.OnScrollPercentage += OnScrollUpdate;
			OnContentUpdate();
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
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			int pageCount = m_pageCount;
			int currentPageIndex = m_list.currentPageIndex;
			int num = m_pageCount = m_list.pagesCount;
			float num2 = m_list.scrollPercentage * (float)(num - 1);
			for (int i = pageCount; i < num; i++)
			{
				Image val = Object.Instantiate<Image>(m_pageDotPrefab);
				val.get_transform().SetParent(m_pageDotContainer.get_transform(), false);
				val.WithAlpha<Image>((currentPageIndex == i) ? m_config.selectedPageAlpha : m_config.unselectedPageAlpha);
				m_pageDots.Add(val);
			}
			for (int num3 = pageCount - 1; num3 >= num; num3--)
			{
				Object.Destroy(m_pageDots[num3].get_gameObject());
				m_pageDots.RemoveAt(num3);
			}
			int j = 0;
			for (int count = m_pageDots.Count; j < count; j++)
			{
				Image obj = m_pageDots[j];
				float num4 = Mathf.Clamp01(1f - Mathf.Abs((float)j - num2));
				float num5 = Mathf.Lerp(m_config.unselectedPageAlpha, m_config.selectedPageAlpha, num4);
				obj.set_color(new Color(1f, 1f, 1f, num5));
			}
		}

		public ListPageDisplayer()
			: this()
		{
		}
	}
}
