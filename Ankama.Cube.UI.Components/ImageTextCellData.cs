using UnityEngine;

namespace Ankama.Cube.UI.Components
{
	public class ImageTextCellData
	{
		private readonly Sprite m_sprite;

		private readonly string m_text;

		public Sprite Sprite => m_sprite;

		public string Text => m_text;

		public ImageTextCellData(Sprite sprite, string text)
		{
			m_sprite = sprite;
			m_text = text;
		}

		public override string ToString()
		{
			return m_text;
		}
	}
}
