using Ankama.Cube.Data;
using Ankama.Cube.UI.Components;
using UnityEngine;

namespace Ankama.Cube.Demo.UI
{
	public class CharacterPanel : Panel
	{
		[SerializeField]
		private RawTextField m_name;

		[SerializeField]
		private RawTextField m_ambience;

		[SerializeField]
		private RawTextField m_difficulty;

		public void Set(SquadDefinition definition, SquadFakeData fakeData)
		{
			if (definition == null || fakeData == null)
			{
				m_name.get_gameObject().SetActive(false);
				m_ambience.get_gameObject().SetActive(false);
				m_difficulty.get_gameObject().SetActive(false);
				return;
			}
			string title = fakeData.title;
			string description = fakeData.description;
			string difficulty = fakeData.difficulty;
			m_illu.set_sprite(fakeData.illu);
			m_name.SetText(title);
			m_name.get_gameObject().SetActive(!string.IsNullOrEmpty(title));
			m_ambience.SetText(description);
			m_ambience.get_gameObject().SetActive(!string.IsNullOrEmpty(description));
			m_difficulty.richText = true;
			m_difficulty.SetText(difficulty);
			m_difficulty.get_gameObject().SetActive(!string.IsNullOrEmpty(difficulty));
		}
	}
}
