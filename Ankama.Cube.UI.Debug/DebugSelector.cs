using System;
using System.Linq;
using UnityEngine;

namespace Ankama.Cube.UI.Debug
{
	internal static class DebugSelector
	{
		public static readonly GUIStyle Style = new GUIStyle(GUIStyle.op_Implicit("box"));
	}
	public abstract class DebugSelector<T> : MonoBehaviour
	{
		[SerializeField]
		protected int m_width = 300;

		[SerializeField]
		protected int m_offsetX = 300;

		private readonly string m_title;

		private bool m_showSelection;

		private T m_selected;

		public T selected => m_selected;

		protected abstract T[] dataValues
		{
			get;
		}

		protected DebugSelector(string title)
			: this()
		{
			m_title = title;
		}

		protected virtual void Awake()
		{
			m_selected = dataValues.FirstOrDefault();
			SetActive(active: false);
		}

		public void SetActive(bool active)
		{
			this.get_gameObject().SetActive(active);
		}

		private unsafe void OnGUI()
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Invalid comparison between Unknown and I4
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Invalid comparison between Unknown and I4
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Invalid comparison between Unknown and I4
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Invalid comparison between Unknown and I4
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			Event current = Event.get_current();
			if (m_showSelection)
			{
				int num = 50;
				int num2 = 20;
				int num3 = m_width + m_offsetX + num + 10;
				if (((int)current.get_type() == 0 && (((IntPtr)(void*)current.get_mousePosition()).x > (float)num3 || ((IntPtr)(void*)current.get_mousePosition()).x < (float)m_offsetX)) || ((int)current.get_type() == 4 && (int)current.get_keyCode() == 27))
				{
					m_showSelection = false;
					return;
				}
				if (GUI.Button(new Rect((float)m_offsetX, 10f, (float)m_width, 20f), $"▼ {m_title} : {selected}", DebugSelector.Style))
				{
					m_showSelection = false;
				}
				DisplayDataResults(dataValues, m_offsetX, 40f, "", num2);
			}
			else
			{
				if ((int)current.get_type() == 4 && (int)current.get_keyCode() == 27)
				{
					Close();
					return;
				}
				if (GUI.Button(new Rect((float)m_offsetX, 10f, (float)m_width, 20f), $"► {m_title} : {selected}", DebugSelector.Style))
				{
					m_showSelection = true;
				}
			}
			GUI.Label(new Rect(((IntPtr)(void*)current.get_mousePosition()).x + 10f, ((IntPtr)(void*)current.get_mousePosition()).y + 20f, 150f, 20f), $"{selected}", DebugSelector.Style);
		}

		private float DisplayDataResults(T[] dataArray, float x, float y, string filter, float lineHeight)
		{
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			string[] array = (from c in dataArray
				select c.ToString()).ToArray();
			float num = (float)array.Length * lineHeight;
			Rect val = new Rect(x, y, (float)m_width, num);
			GUI.Box(val, "");
			int num2 = GUI.SelectionGrid(val, -1, array, 1);
			if (num2 >= 0)
			{
				m_showSelection = false;
				m_selected = dataArray[num2];
			}
			y += num;
			return y;
		}

		private void Close()
		{
			SetActive(active: false);
		}
	}
}
