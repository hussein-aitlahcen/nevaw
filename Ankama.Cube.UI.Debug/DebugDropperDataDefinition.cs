using Ankama.Cube.Data;
using Ankama.Cube.Fight;
using Ankama.Cube.Fight.Entities;
using Ankama.Utilities;
using DataEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ankama.Cube.UI.Debug
{
	public abstract class DebugDropperDataDefinition<T> : MonoBehaviour where T : EditableData
	{
		private const int Limit = 25;

		protected string m_title;

		private string m_searchPrefKey;

		[SerializeField]
		private string m_search = "";

		[SerializeField]
		protected int m_width = 300;

		[SerializeField]
		protected int m_offsetX = 300;

		private string m_lastSearch = "";

		private int m_level;

		private KeyCode m_closeKeyCode;

		private Event m_lastEvent;

		private T[] m_allData;

		private T[] m_allDataNames;

		private T[] m_allDataDescriptions;

		protected abstract T[] dataValues
		{
			get;
		}

		private T[] allData
		{
			get
			{
				if (m_allData == null)
				{
					m_allData = dataValues;
				}
				return m_allData;
			}
		}

		private T[] allDataNames
		{
			get
			{
				if (m_allDataNames == null)
				{
					string search = m_lastSearch.ToLower();
					m_allDataNames = Enumerable.Where<T>(predicate: (!int.TryParse(m_lastSearch, out int _)) ? ((Func<T, bool>)((T definition) => StringExtensions.ContainsIgnoreDiacritics(definition.get_displayName(), search, StringComparison.OrdinalIgnoreCase))) : ((Func<T, bool>)((T definition) => (!definition.get_id().ToString().Contains(search)) ? definition.get_displayName().ToLower().Contains(search) : true)), source: allData).ToArray();
				}
				return m_allDataNames;
			}
		}

		private T[] allDataDescriptions
		{
			get
			{
				if (m_allDataDescriptions == null)
				{
					string search = m_lastSearch.ToLower();
					m_allDataDescriptions = allData.Where(delegate(T c)
					{
						IDefinitionWithTooltip definitionWithTooltip = c as IDefinitionWithTooltip;
						if (definitionWithTooltip == null)
						{
							return false;
						}
						string value;
						return RuntimeData.TryGetText(definitionWithTooltip.i18nDescriptionId, out value) && StringExtensions.ContainsIgnoreDiacritics(value.ToLower(), search, StringComparison.OrdinalIgnoreCase);
					}).ToArray();
				}
				return m_allDataDescriptions;
			}
		}

		public event Action<T, int, Event> OnSelected;

		protected void Initialize(string title, string searchPrefKey)
		{
			m_title = title;
			m_searchPrefKey = searchPrefKey;
			m_search = PlayerPrefs.GetString(m_searchPrefKey);
		}

		protected void Reset()
		{
			this.OnSelected = null;
		}

		protected virtual void Awake()
		{
			SetActive(active: false);
		}

		public void SetCloseKeyCode(KeyCode closeKey)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			m_closeKeyCode = closeKey;
		}

		public void SetActive(bool active)
		{
			this.get_gameObject().SetActive(active);
		}

		public void SetLocalPlayerLevel()
		{
			FightStatus local = FightStatus.local;
			if (local != null)
			{
				foreach (HeroStatus item in local.EnumerateEntities<HeroStatus>())
				{
					if (item.ownerId == local.localPlayerId)
					{
						m_level = item.level;
						break;
					}
				}
			}
		}

		protected unsafe virtual void OnGUI()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Invalid comparison between Unknown and I4
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Invalid comparison between Unknown and I4
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			Event current = Event.get_current();
			if ((int)current.get_type() == 0)
			{
				m_lastEvent = new Event(current);
			}
			int num = 50;
			int num2 = m_width + m_offsetX + num + 10;
			if (((int)current.get_type() == 0 && (((IntPtr)(void*)current.get_mousePosition()).x > (float)num2 || ((IntPtr)(void*)current.get_mousePosition()).x < (float)m_offsetX)) || ((int)current.get_type() == 4 && ((int)current.get_keyCode() == 27 || current.get_keyCode() == m_closeKeyCode)))
			{
				Close();
				return;
			}
			GUI.Label(new Rect((float)m_offsetX, 10f, (float)num, 20f), m_title + " :");
			int num3 = 20;
			GUI.SetNextControlName("SearchField");
			Rect val = default(Rect);
			val._002Ector((float)(m_offsetX + num + 10), 10f, 200f, (float)num3);
			m_search = GUI.TextField(val, m_search, 25);
			if (GUI.GetNameOfFocusedControl() == string.Empty)
			{
				GUI.FocusControl("SearchField");
			}
			GUI.Label(new Rect(val.get_xMax() + 5f, 10f, 30f, (float)num3), "level");
			GUI.SetNextControlName("LevelField");
			if (int.TryParse(GUI.TextField(new Rect(val.get_xMax() + 35f, 10f, (float)m_width - val.get_width() - 35f, (float)num3), m_level.ToString()), out int result))
			{
				m_level = result;
			}
			if (GUI.Button(new Rect((float)num2, 10f, 20f, (float)num3), "x"))
			{
				Close();
				return;
			}
			string text = m_search.ToLowerInvariant();
			if (text != m_lastSearch)
			{
				m_lastSearch = text;
				m_allDataNames = null;
				m_allDataDescriptions = null;
				PlayerPrefs.SetString(m_searchPrefKey, m_search);
			}
			int num4 = DisplayDataResults(allDataNames, m_offsetX + num + 10, 40, "by name", num3);
			DisplayDataResults(allDataDescriptions, m_offsetX + num + 10, 50 + num4, "by description", num3);
		}

		private int DisplayDataResults(T[] dataArray, int x, int y, string filter, int lineHeight)
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			List<string> list = (from c in dataArray
				select c.get_idAndName()).ToList();
			GUI.Label(new Rect((float)x, (float)y, (float)m_width, (float)lineHeight), $"{list.Count} {m_title}(s) found " + filter, GUIStyle.op_Implicit("box"));
			string[] array = list.Take(25).ToArray();
			int num = array.Length * lineHeight;
			Rect val = new Rect((float)x, (float)(y + lineHeight), (float)m_width, (float)num);
			GUI.Box(val, "", GUIStyle.op_Implicit("box"));
			int num2 = GUI.SelectionGrid(val, -1, array, 1);
			if (num2 >= 0)
			{
				OnSelect(dataArray[num2]);
			}
			y += num;
			if (list.Count > 25)
			{
				y += lineHeight;
				GUI.Label(new Rect((float)x, (float)y, (float)m_width, (float)lineHeight), "...", GUIStyle.op_Implicit("box"));
			}
			return y;
		}

		protected void Close()
		{
			SetActive(active: false);
		}

		private void OnSelect(T data)
		{
			if (m_level < 1)
			{
				m_level = 1;
			}
			if (m_level > 10)
			{
				m_level = 10;
			}
			this.OnSelected?.Invoke(data, m_level, m_lastEvent);
		}

		protected DebugDropperDataDefinition()
			: this()
		{
		}
	}
}
