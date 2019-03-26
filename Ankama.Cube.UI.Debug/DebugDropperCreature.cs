using Ankama.Cube.Data;
using System;
using System.Linq;
using UnityEngine;

namespace Ankama.Cube.UI.Debug
{
	public class DebugDropperCreature : DebugDropperDataDefinition<CharacterDefinition>
	{
		private const string DisplayName = "Creature";

		private const string SearchPrefKey = "DebugCreatureDropperSearch";

		private CharacterDefinition m_selected;

		public CharacterDefinition selected => m_selected;

		protected override CharacterDefinition[] dataValues => RuntimeData.summoningDefinitions.Values.Concat<CharacterDefinition>(RuntimeData.companionDefinitions.Values).ToArray();

		public void SetSelected(CharacterDefinition current)
		{
			m_selected = current;
		}

		protected override void Awake()
		{
			base.Awake();
			Initialize("Creature", "DebugCreatureDropperSearch");
		}

		protected unsafe override void OnGUI()
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Invalid comparison between Unknown and I4
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Expected O, but got Unknown
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Expected O, but got Unknown
			if (m_selected == null)
			{
				base.OnGUI();
				return;
			}
			Event current = Event.get_current();
			if ((int)current.get_keyCode() == 27)
			{
				m_selected = null;
				Close();
			}
			else
			{
				GUI.Label(new Rect((float)m_offsetX, 10f, (float)m_width, 20f), m_title + " : " + m_selected.get_idAndName(), new GUIStyle(GUIStyle.op_Implicit("box")));
				GUI.Label(new Rect(((IntPtr)(void*)current.get_mousePosition()).x + 10f, ((IntPtr)(void*)current.get_mousePosition()).y + 20f, 150f, 20f), m_selected.get_displayName() ?? "", new GUIStyle(GUIStyle.op_Implicit("box")));
			}
		}
	}
}
