using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Components
{
	public class TextButton : Button
	{
		[SerializeField]
		private AbstractTextField m_text;

		[SerializeField]
		private TextButtonStyle m_style;

		public AbstractTextField textField => m_text;

		protected override void OnEnable()
		{
			this.OnEnable();
		}

		protected override void DoStateTransition(SelectionState state, bool instant)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected I4, but got Unknown
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			if (this.get_gameObject().get_activeInHierarchy())
			{
				TextButtonState textButtonState = m_style.disable;
				switch ((int)state)
				{
				case 0:
					textButtonState = m_style.normal;
					break;
				case 1:
					textButtonState = m_style.highlight;
					break;
				case 2:
					textButtonState = m_style.pressed;
					break;
				case 3:
					textButtonState = m_style.disable;
					break;
				}
				if (this.get_image() != null)
				{
					this.get_image().set_overrideSprite(textButtonState.sprite);
				}
				if (m_text != null)
				{
					m_text.color = textButtonState.textColor;
				}
			}
		}

		public TextButton()
			: this()
		{
		}
	}
}
