using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Components
{
	[RequireComponent(typeof(RectMask2D))]
	public class CircularList : List
	{
		private int m_previousFirstCellIndex = -1;

		private int m_previousLastCellIndex = -1;

		private int m_rows;

		[NonSerialized]
		private bool m_initialized;

		protected override void ComputeDimensions()
		{
			base.ComputeDimensions();
			if (m_horizontal)
			{
				m_rows = Math.Max(1, (int)Math.Ceiling((float)m_totalWidth / (float)m_cellSize.get_x())) + m_extraLineCount;
			}
			else
			{
				m_rows = Math.Max(1, (int)Math.Ceiling((float)m_totalHeight / (float)m_cellSize.get_y())) + m_extraLineCount;
			}
			m_needFullReLayout = true;
		}

		private void Update()
		{
			//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_035d: Unknown result type (might be due to invalid IL or missing references)
			int num = ToIndex(base.scrollPercentage, normalize: false) - m_rows / 2;
			int num2 = num + m_rows;
			if (m_needFullReLayout)
			{
				foreach (KeyValuePair<int, CellRenderer> item in m_elementsByItem)
				{
					ReturnToPool(item.Value);
				}
				m_elementsByItem.Clear();
				for (int i = num; i <= num2; i++)
				{
					int num3 = NormalizeItemIndex(i, m_items.Count);
					object value = (num3 >= 0) ? m_items[num3] : null;
					CellRenderer fromPool = GetFromPool();
					m_elementsByItem[i] = fromPool;
					fromPool.SetValue(value);
				}
				m_needFullReLayout = false;
			}
			else
			{
				for (int j = m_previousFirstCellIndex; j < Math.Min(num, m_previousLastCellIndex + 1); j++)
				{
					if (m_elementsByItem.TryGetValue(j, out CellRenderer value2))
					{
						ReturnToPool(value2);
						m_elementsByItem.Remove(j);
					}
				}
				for (int num4 = m_previousLastCellIndex; num4 > Math.Max(num2, m_previousFirstCellIndex - 1); num4--)
				{
					if (m_elementsByItem.TryGetValue(num4, out CellRenderer value3))
					{
						ReturnToPool(value3);
						m_elementsByItem.Remove(num4);
					}
				}
				for (int k = num; k < Math.Min(m_previousFirstCellIndex, num2 + 1); k++)
				{
					int num5 = NormalizeItemIndex(k, m_items.Count);
					object value4 = (num5 >= 0) ? m_items[num5] : null;
					CellRenderer fromPool2 = GetFromPool();
					m_elementsByItem[k] = fromPool2;
					fromPool2.SetValue(value4);
				}
				for (int l = Math.Max(num, m_previousLastCellIndex + 1); l <= num2; l++)
				{
					int num6 = NormalizeItemIndex(l, m_items.Count);
					object value5 = (num6 >= 0) ? m_items[num6] : null;
					CellRenderer fromPool3 = GetFromPool();
					m_elementsByItem[l] = fromPool3;
					fromPool3.SetValue(value5);
				}
			}
			float num7 = ToRowIndex(base.scrollPercentage, normalize: false);
			if (m_horizontal)
			{
				float num8 = num7 * (float)m_cellSize.get_x() % (float)m_cellSize.get_x();
				int num9 = num;
				for (int m = 0; m <= m_rows; m++)
				{
					float num10 = (float)(-(m_rows / 2) * m_cellSize.get_x()) - num8 + (float)(m_cellSize.get_x() * m);
					int num11 = 0;
					if (m_elementsByItem.TryGetValue(num9++, out CellRenderer value6))
					{
						value6.GetComponent<RectTransform>().set_anchoredPosition(new Vector2(num10, (float)num11));
					}
				}
			}
			else
			{
				float num13 = num7 * (float)m_cellSize.get_y() % (float)m_cellSize.get_y();
				int num14 = num;
				for (int n = 0; n <= m_rows; n++)
				{
					float num15 = (float)(m_rows / 2 * m_cellSize.get_y()) + num13 - (float)(m_cellSize.get_y() * n);
					int num16 = 0;
					if (m_elementsByItem.TryGetValue(num14++, out CellRenderer value7))
					{
						value7.GetComponent<RectTransform>().set_anchoredPosition(new Vector2((float)num16, num15));
					}
				}
			}
			m_previousFirstCellIndex = num;
			m_previousLastCellIndex = num2;
		}

		protected override void SetCellRectTransformAnchors(RectTransform rectTransform)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			Vector2 val = default(Vector2);
			val._002Ector(0.5f, 0.5f);
			rectTransform.set_anchorMin(val);
			rectTransform.set_anchorMax(val);
			rectTransform.set_pivot(val);
		}

		private static int NormalizeItemIndex(int index, int itemCount)
		{
			if (itemCount > 0)
			{
				return (index % itemCount + itemCount) % itemCount;
			}
			return -1;
		}

		private static float NormalizeScrollPercentage(float percentage)
		{
			return (percentage % 1f + 1f) % 1f;
		}

		private int ToIndex(float perc, bool normalize)
		{
			if (!normalize)
			{
				return Mathf.FloorToInt(perc * (float)m_items.Count);
			}
			return Mathf.FloorToInt(NormalizeScrollPercentage(perc) * (float)m_items.Count);
		}

		private float ToRowIndex(float perc, bool normalize)
		{
			if (!normalize)
			{
				return perc * (float)m_items.Count;
			}
			return NormalizeScrollPercentage(perc) * (float)m_items.Count;
		}
	}
}
