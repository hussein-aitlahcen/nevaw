using Ankama.Cube.UI.Components;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Debug
{
	public class TextFieldDropdownTest : MonoBehaviour
	{
		[SerializeField]
		private TextFieldDropdown m_dropdown;

		private void Awake()
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Expected O, but got Unknown
			List<OptionData> list = new List<OptionData>();
			int i = 0;
			for (int num = 5; i < num; i++)
			{
				list.Add(new OptionData($"Choix {i}"));
			}
			m_dropdown.AddOptions(list);
		}

		public TextFieldDropdownTest()
			: this()
		{
		}
	}
}
