using Ankama.Cube.Data.UI.Localization.TextFormatting;
using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Components.Tooltip
{
	public class GenericTooltipWindow : AbstractTooltipWindow
	{
		[SerializeField]
		private TextField m_titleText;

		[SerializeField]
		private TextField m_text;

		[SerializeField]
		private int m_multilineMaxWidth;

		private LayoutElement m_layoutElement;

		public override void Awake()
		{
			base.Awake();
			m_layoutElement = this.get_gameObject().AddComponent<LayoutElement>();
		}

		public void SetText(int textKeyId, IValueProvider valueProvider = null, int? titleTextKeyId = default(int?), IValueProvider titleValueProvider = null, bool multiline = true)
		{
			m_layoutElement.set_preferredWidth((float)(multiline ? m_multilineMaxWidth : (-1)));
			m_text.SetText(textKeyId, valueProvider);
			m_titleText.get_gameObject().SetActive(titleTextKeyId.HasValue);
			if (titleTextKeyId.HasValue)
			{
				m_titleText.SetText(titleTextKeyId.Value, titleValueProvider);
			}
		}

		public void SetText(string textKeyName, IValueProvider valueProvider = null, string titleTextKeyName = null, IValueProvider titleValueProvider = null, bool multiline = true)
		{
			m_layoutElement.set_preferredWidth((float)(multiline ? m_multilineMaxWidth : (-1)));
			m_text.SetText(textKeyName, valueProvider);
			m_titleText.get_gameObject().SetActive(titleTextKeyName != null);
			if (titleTextKeyName != null)
			{
				m_titleText.SetText(titleTextKeyName, titleValueProvider);
			}
		}
	}
}
