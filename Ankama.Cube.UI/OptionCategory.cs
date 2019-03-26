using DG.Tweening;
using UnityEngine;

namespace Ankama.Cube.UI
{
	public class OptionCategory : MonoBehaviour
	{
		[SerializeField]
		private Canvas m_canvas;

		[SerializeField]
		private CanvasGroup m_canvasGroup;

		public float alpha
		{
			set
			{
				m_canvasGroup.set_alpha(value);
			}
		}

		public void SetVisible(bool value)
		{
			if (m_canvas != null)
			{
				m_canvas.set_enabled(value);
			}
			else
			{
				this.get_gameObject().SetActive(value);
			}
			m_canvasGroup.set_enabled(value);
		}

		public Tween DoFade(float value, float duration)
		{
			return DOTweenModuleUI.DOFade(m_canvasGroup, value, duration);
		}

		public OptionCategory()
			: this()
		{
		}
	}
}
