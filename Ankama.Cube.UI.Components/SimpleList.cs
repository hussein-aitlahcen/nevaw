using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Components
{
	[RequireComponent(typeof(RectMask2D))]
	public class SimpleList : List
	{
		[Header("ScrollBars")]
		[SerializeField]
		private Scrollbar m_horizontalScrollbar;

		[SerializeField]
		private Scrollbar m_verticalScrollbar;

		[SerializeField]
		[Range(0f, 1f)]
		private float m_minScrollBarButtonSize;

		private int m_previousFirstCellIndex = -1;

		private int m_previousLastCellIndex = -1;

		private int m_rows;

		private int m_columns;

		private float m_horizontalLeeway;

		private float m_verticalLeeway;

		[NonSerialized]
		private bool m_initialized;

		protected unsafe override void Awake()
		{
			base.Awake();
			if (m_horizontal)
			{
				if (Object.op_Implicit(m_horizontalScrollbar))
				{
					m_horizontalScrollbar.get_onValueChanged().AddListener(new UnityAction<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
				}
				if (Object.op_Implicit(m_verticalScrollbar))
				{
					m_verticalScrollbar.get_onValueChanged().RemoveListener(new UnityAction<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
				}
			}
			else
			{
				if (Object.op_Implicit(m_horizontalScrollbar))
				{
					m_horizontalScrollbar.get_onValueChanged().RemoveListener(new UnityAction<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
				}
				if (Object.op_Implicit(m_verticalScrollbar))
				{
					m_verticalScrollbar.get_onValueChanged().AddListener(new UnityAction<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
				}
			}
		}

		private void OnScrollValueChanged(float value)
		{
			base.scrollPercentage = value;
		}

		protected override void CheckInit()
		{
			base.CheckInit();
			if (Object.op_Implicit(m_horizontalScrollbar))
			{
				m_horizontalScrollbar.get_gameObject().SetActive(m_horizontal);
			}
			else if (m_horizontal)
			{
				Debug.LogWarningFormat("List {0} doesn't have a horizontal scrollBar", new object[1]
				{
					this.get_name()
				});
			}
			if (Object.op_Implicit(m_verticalScrollbar))
			{
				m_verticalScrollbar.get_gameObject().SetActive(!m_horizontal);
			}
			else if (!m_horizontal)
			{
				Debug.LogWarningFormat("List {0} doesn't have a vertical scrollBar", new object[1]
				{
					this.get_name()
				});
			}
		}

		protected override void ComputeDimensions()
		{
			base.ComputeDimensions();
			if (m_horizontal)
			{
				m_rows = Math.Max(1, m_totalHeight / m_cellSize.get_y());
				m_columns = Math.Max(1, (int)Math.Ceiling((float)m_totalWidth / (float)m_cellSize.get_x())) + m_extraLineCount;
				m_horizontalLeeway = Math.Max(0f, Mathf.Ceil((float)m_items.Count / (float)m_rows) * (float)m_cellSize.get_x() - (float)m_totalWidth);
				m_verticalLeeway = Math.Max(0, m_columns * m_cellSize.get_y() - m_totalHeight);
			}
			else
			{
				m_rows = Math.Max(1, (int)Math.Ceiling((float)m_totalHeight / (float)m_cellSize.get_y())) + m_extraLineCount;
				m_columns = Math.Max(1, m_totalWidth / m_cellSize.get_x());
				m_verticalLeeway = Math.Max(0f, Mathf.Ceil((float)m_items.Count / (float)m_columns) * (float)m_cellSize.get_y() - (float)m_totalHeight);
				m_horizontalLeeway = Math.Max(0, m_rows * m_cellSize.get_x() - m_totalWidth);
			}
			SetupScrollBars();
			m_needFullReLayout = true;
		}

		private void SetupScrollBars()
		{
			if (m_horizontal && Object.op_Implicit(m_horizontalScrollbar))
			{
				float num = (float)(m_items.Count * m_cellSize.get_x()) / (float)m_rows;
				m_horizontalScrollbar.set_size(Mathf.Min(1f, Mathf.Max(m_minScrollBarButtonSize, (float)m_totalWidth / num)));
				m_horizontalScrollbar.set_numberOfSteps(0);
				m_horizontalScrollbar.set_value(base.scrollPercentage);
			}
			else if (!m_horizontal && Object.op_Implicit(m_verticalScrollbar))
			{
				float num2 = (float)(m_items.Count * m_cellSize.get_y()) / (float)m_columns;
				m_verticalScrollbar.set_size(Mathf.Min(1f, Mathf.Max(m_minScrollBarButtonSize, (float)m_totalHeight / num2)));
				m_verticalScrollbar.set_numberOfSteps(0);
				m_verticalScrollbar.set_value(base.scrollPercentage);
			}
		}

		private void Update()
		{
			//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0368: Unknown result type (might be due to invalid IL or missing references)
			int num = ToIndex(base.scrollPercentage);
			int num2 = Mathf.Clamp(num, 0, m_items.Count);
			int num3 = num + m_rows * m_columns - 1;
			if (m_needFullReLayout)
			{
				foreach (KeyValuePair<int, CellRenderer> item in m_elementsByItem)
				{
					ReturnToPool(item.Value);
				}
				m_elementsByItem.Clear();
				for (int i = num2; i <= num3; i++)
				{
					object value = (i < m_items.Count) ? m_items[i] : null;
					CellRenderer fromPool = GetFromPool();
					m_elementsByItem[i] = fromPool;
					fromPool.SetValue(value);
				}
				m_needFullReLayout = false;
			}
			else
			{
				for (int j = m_previousFirstCellIndex; j < Math.Min(num2, m_previousLastCellIndex + 1); j++)
				{
					if (m_elementsByItem.TryGetValue(j, out CellRenderer value2))
					{
						ReturnToPool(value2);
						m_elementsByItem.Remove(j);
					}
				}
				for (int num4 = m_previousLastCellIndex; num4 > Math.Max(num3, m_previousFirstCellIndex - 1); num4--)
				{
					if (m_elementsByItem.TryGetValue(num4, out CellRenderer value3))
					{
						ReturnToPool(value3);
						m_elementsByItem.Remove(num4);
					}
				}
				for (int k = num2; k < Math.Min(m_previousFirstCellIndex, num3 + 1); k++)
				{
					object value4 = (k < m_items.Count) ? m_items[k] : null;
					CellRenderer fromPool2 = GetFromPool();
					m_elementsByItem[k] = fromPool2;
					fromPool2.SetValue(value4);
				}
				for (int l = Math.Max(num2, m_previousLastCellIndex + 1); l <= num3; l++)
				{
					object value5 = (l < m_items.Count) ? m_items[l] : null;
					CellRenderer fromPool3 = GetFromPool();
					m_elementsByItem[l] = fromPool3;
					fromPool3.SetValue(value5);
				}
			}
			float num5 = ToRowIndex(base.scrollPercentage);
			if (m_horizontal)
			{
				float num6 = num5 * (float)m_cellSize.get_x() % (float)m_cellSize.get_x();
				int num7 = num2;
				for (int m = 0; m < m_columns; m++)
				{
					float num8 = (float)(m_cellSize.get_x() * m) - num6;
					for (int n = 0; n < m_rows; n++)
					{
						int num9 = m_totalHeight - m_cellSize.get_y() * (n + 1);
						if (m_elementsByItem.TryGetValue(num7++, out CellRenderer value6))
						{
							value6.GetComponent<RectTransform>().set_anchoredPosition(new Vector2(num8, (float)num9));
						}
					}
				}
			}
			else
			{
				float num11 = num5 * (float)m_cellSize.get_y() % (float)m_cellSize.get_y();
				int num12 = num2;
				for (int num13 = 0; num13 < m_rows; num13++)
				{
					float num14 = (float)m_totalHeight - ((float)(m_cellSize.get_y() * (num13 + 1)) - num11);
					for (int num15 = 0; num15 < m_columns; num15++)
					{
						int num16 = m_cellSize.get_x() * num15;
						if (m_elementsByItem.TryGetValue(num12++, out CellRenderer value7))
						{
							value7.GetComponent<RectTransform>().set_anchoredPosition(new Vector2((float)num16, num14));
						}
					}
				}
			}
			m_previousFirstCellIndex = num2;
			m_previousLastCellIndex = num3;
		}

		private int ToIndex(float perc)
		{
			float num = Mathf.Clamp01(perc);
			if (m_horizontal)
			{
				return (int)Math.Floor(m_horizontalLeeway * num / (float)m_cellSize.get_x()) * m_rows;
			}
			return (int)Math.Floor(m_verticalLeeway * num / (float)m_cellSize.get_y()) * m_columns;
		}

		private float ToRowIndex(float perc)
		{
			float num = Mathf.Clamp01(perc);
			if (m_horizontal)
			{
				return m_horizontalLeeway * num / (float)m_cellSize.get_x();
			}
			return m_verticalLeeway * num / (float)m_cellSize.get_y();
		}

		protected override void SetCellRectTransformAnchors(RectTransform rectTransform)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			rectTransform.set_anchorMin(Vector2.get_zero());
			rectTransform.set_anchorMax(Vector2.get_zero());
			rectTransform.set_pivot(Vector2.get_zero());
		}
	}
}
