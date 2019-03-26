using Ankama.Cube.Data;
using Ankama.Cube.UI.Components;
using UnityEngine;

namespace Ankama.Cube.Demo.UI
{
	public class GodPanel : Panel
	{
		[SerializeField]
		private RawTextField m_name;

		[SerializeField]
		private RawTextField m_ambience;

		public void Set(GodDefinition definition, GodFakeData fakeData)
		{
			if (definition == null || fakeData == null)
			{
				m_name.get_gameObject().SetActive(false);
				m_ambience.get_gameObject().SetActive(false);
				return;
			}
			string title = fakeData.title;
			string description = fakeData.description;
			m_illu.set_sprite(fakeData.illu);
			m_name.SetText(title);
			m_name.get_gameObject().SetActive(!string.IsNullOrEmpty(title));
			m_ambience.SetText(description);
			m_ambience.get_gameObject().SetActive(!string.IsNullOrEmpty(description));
		}
	}
}
