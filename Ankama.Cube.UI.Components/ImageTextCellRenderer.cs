using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Components
{
	public class ImageTextCellRenderer : CellRenderer<ImageTextCellData, ICellRendererConfigurator>
	{
		[SerializeField]
		private Image m_image;

		[SerializeField]
		private Text m_text;

		protected override void SetValue(ImageTextCellData val)
		{
			m_image.set_sprite(val.Sprite);
			m_text.set_text(val.Text);
		}

		protected override void Clear()
		{
			m_image.set_sprite(null);
			m_text.set_text("");
		}

		public override void OnConfiguratorUpdate(bool instant)
		{
		}
	}
}
